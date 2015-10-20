// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;




namespace fxpa
{
    public class RemoteDataProvider :  IDataProvider
    {
        volatile protected OperationalStateEnum _operationalState = OperationalStateEnum.UnInitialized;
        public OperationalStateEnum OperationalState
        {
            get { return _operationalState; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// LiveDataProvider does not support time control.
        /// </summary>
        public virtual ITimeControl TimeControl
        {
            get { return null; }
        }

        public virtual DateTime FirstBarDateTime
        {
            get
            {
                lock (this)
                {
                    if (_dataUnits.Count > 0)
                    {
                        return _dataUnits[0].DateTime;
                    }
                    return DateTime.MaxValue;
                }
            }
        }

        public virtual DateTime LastBarDateTime
        {
            get
            {
                lock (this)
                {
                    if (_dataUnits.Count > 0)
                    {
                        return _dataUnits[_dataUnits.Count - 1].DateTime;
                    }
                    return DateTime.MaxValue;
                }
            }
        }

        public virtual int DataUnitCount
        {
            get { lock (this) { return _dataUnits.Count; } }
        }

        /// <summary>
        /// Used when a matching of a given time to a bar data is needed.
        /// Matches a time to an index of bar data on this time.
        /// </summary>
        Dictionary<DateTime, int> _cachedIndexSearches = new Dictionary<DateTime, int>();

        protected List<BarData> _dataUnits = new List<BarData>();
        public virtual IList<BarData> DataUnits
        {
            get
            {
                lock (this) { return _dataUnits; }
            }
        }

        protected double _ask = double.NaN;
        public virtual double Ask
        {
            get 
            { 
                lock (this) 
                {
                    //SystemMonitor.CheckWarning(double.IsNaN(_ask) == false);
                    return _ask;
                } 
            }
        }

        protected double _bid = double.NaN;
        public virtual double Bid
        {
            get
            {
                lock (this) 
                {
                    //SystemMonitor.CheckWarning(double.IsNaN(_bid) == false);
                    return _bid; 
                }
            }
        }

        public virtual double Spread
        {
            get { lock (this) { return Math.Abs(Ask - Bid); } }
        }

        public event ValuesUpdatedDelegate ValuesUpdateEvent;

        /// <summary>
        /// The date time of the platform serving this data provider.
        /// </summary>
        public virtual DateTime CurrentSourceDateTime
        {
            get
            {
                lock (this)
                {
                    if (CurrentDataUnit.HasValue)
                    {
                        return CurrentDataUnit.Value.DateTime;
                    }
                    return DateTime.MinValue;
                }
            }
        }

        public virtual BarData ?CurrentDataUnit
        {
            get
            {
                lock (this)
                {
                    if (DataUnitCount == 0)
                    {
                        return null;
                    }
                    return this.DataUnits[DataUnitCount - 1];
                }
            }
        }

        public virtual TimeSpan TimeInterval
        {
            get { return _sessionInfo.TimeInterval; }
        }

        protected DateTime _lastUpdateDateTime = DateTime.MinValue;

        //List<ArbiterClientId?> _forwardTransportation;

        protected SessionInfo _sessionInfo;

        //public event GeneralHelper.GenericDelegate<IOperational> OperationalStatusChangedEvent;
        //private bool _singleThreadOnly;

        TimeSpan _defaultTimeOut = TimeSpan.Zero;
        protected TimeSpan DefaultTimeOut
        {
            get { return _defaultTimeOut; }
            set { _defaultTimeOut = value; }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public RemoteDataProvider()
            //: base("RemoteDataProvider", false)
        {
            name = "RemoteDataProvider";
            //_singleThreadOnly = false;
            DefaultTimeOut = TimeSpan.FromSeconds(15);
        }

        //public bool Initialize(SessionInfo sessionInfo, List<ArbiterClientId?> forwardTransportation)
        //{
        //    lock (this)
        //    {
        //        this.Name = "DataProvider of" + sessionInfo.Id;
        //        //_forwardTransportation = forwardTransportation;
        //        //_sessionInfo = sessionInfo;

        //        //// Subscribe to source here.
        //        //ResultTransportMessage result = SendAndReceiveForwarding<ResultTransportMessage>(_forwardTransportation,
        //        //    new SubscribeToSessionMessage(_sessionInfo));

        //        //if (result != null && result.OperationResult)
        //        //{
        //        //    _operationalState = OperationalStateEnum.Initialized;
        //        //}
        //    }

        //    //GeneralHelper.SafeEventRaise(OperationalStatusChangedEvent, this);
        //    return OperationalState == OperationalStateEnum.Initialized;
        //}

        bool _requestConfirmtion = true;
        public bool RequestConfirmation
        {
            get { return _requestConfirmtion; }
            set { _requestConfirmtion = value; }
        }

        public virtual void UnInitialize()
        {
            if (_operationalState == OperationalStateEnum.Initialized ||
                _operationalState == OperationalStateEnum.Operational)
            {
                //UnSubscribeToSessionMessage message = new UnSubscribeToSessionMessage(_sessionInfo);
                RequestConfirmation = false;

                // UnSubscribe to source here.
                //SendForwarding(_forwardTransportation, message);
            }

            _operationalState = OperationalStateEnum.UnInitialized;

            //GeneralHelper.SafeEventRaise(OperationalStatusChangedEvent, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="updatedItemsCount"></param>
        /// <param name="isMultiStepMode">Multi step mode occurs when many steps are done one after the other, fast. Usefull for UI not to update itself.</param>
        protected void RaiseValuesUpdateEvent(UpdateType updateType, int updatedItemsCount, int stepsRemaining)
        {
            if (ValuesUpdateEvent != null)
            {
                //HERRYConsole.WriteLine("   RaiseValuesUpdateEvent  updatedItemsCount !!!!!!!!!!!!!!!!!!!!!!  " + updatedItemsCount);
                ValuesUpdateEvent(this, updateType, updatedItemsCount, stepsRemaining);
                //GeneralHelper.FireAndForget(ValuesUpdateEvent, this, updateType, updatedItemsCount, stepsRemaining);
            }
        }

        /// <summary>
        /// Helper.
        /// </summary>
        //public void RequestValuesUpdate()
        //{
        //    TracerHelper.Trace(this.Name);

        //    RequestValuesMessage message = new RequestValuesMessage(_sessionInfo);

        //    TradingValuesUpdateMessage resultMessage =
        //        SendAndReceiveForwarding<TradingValuesUpdateMessage>(_forwardTransportation, message);

        //    if (resultMessage != null && resultMessage.OperationResult)
        //    {
        //        Receive(resultMessage);
        //    }
        //}

        /// <summary>
        /// Helper.
        /// </summary>
        protected void RaiseOperationalStatusChangedEvent()
        {
            // TracerHelper.Trace(this.Name + " " + this._operationalState.ToString());
            //OperationalStatusChangedEvent(this);
        }

        /// <summary>
        /// Deltas will return a line indicating the difference of the current bar to the previous one.
        /// This is the same as the MOMENTUM indicator.
        /// </summary>
        public double[] GetDataValuesDeltas(BarData.DataValueSourceEnum valueEnum)
        {
            return GetDataValuesDeltas(0, _dataUnits.Count, valueEnum);
        }

        /// <summary>
        /// A delta - the value from the previous to the current tick.
        /// This is the same as the MOMENTUM indicator.
        /// </summary>
        public double[] GetDataValuesDeltas(int startingIndex, int indecesLimit, BarData.DataValueSourceEnum valueEnum)
        {
            double[] values = GetDataValues(valueEnum, startingIndex, indecesLimit);

            for (int i = values.Length - 1; i > 0 ; i--)
            {
                values[i] = values[i] - values[i - 1];
            }

            if (values.Length > 0)
            {
                values[0] = 0;
            }

            return values;
        }


        /// <summary>
        /// 
        /// </summary>
        public double[] GetDataValues(BarData.DataValueSourceEnum valueEnum)
        {
            return GetDataValues(valueEnum, 0, _dataUnits.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        public double[] GetDataValues(BarData.DataValueSourceEnum valueEnum, int startingIndex, int indexCount)
        {
            int count = indexCount;
            if (count == 0)
            {
                count = _dataUnits.Count - startingIndex;
                GeneralHelper.Verify(count >= 0, "Invalid starting index.");
            }

            double[] result = new double[count];
            lock (this)
            {
                for (int i = startingIndex; i < startingIndex + count; i++)
                {
                    result[i - startingIndex] = _dataUnits[i].GetValue(valueEnum);
                }
            }

            return result;
        }

        /// <summary>
        /// Also time gaps must be considered.
        /// Result is -1 to indicate not found.
        /// Since this method is called very extensively on drawing, it employes a caching mechanism.
        /// </summary>
        public int GetBarIndexAtTime(DateTime time)
        {
            lock (this)
            {
                if (_dataUnits.Count == 0)
                {
                    return 0;
                }

                if (_cachedIndexSearches.ContainsKey(time))
                {
                    return _cachedIndexSearches[time];
                }

                TimeSpan difference = (time - _dataUnits[0].DateTime);
                if (difference.TotalSeconds < 0)
                {// Before time.
                    return -1;
                } else if (difference.TotalSeconds <= _sessionInfo.TimeInterval.TotalSeconds)
                {// 0 index.
                    _cachedIndexSearches.Add(time, 0);
                    return 0;
                }
                
                // Estimated index has skipped time gaps. so now start looking back from it to find the proper period.
                int estimatedIndex = 1 + (int)Math.Floor(difference.TotalSeconds / _sessionInfo.TimeInterval.TotalSeconds);
                estimatedIndex = Math.Min(_dataUnits.Count - 1, estimatedIndex);
                for (int i = estimatedIndex; i >= 1; i--)
                {
                    if (_dataUnits[i - 1].DateTime <= time
                        && _dataUnits[i].DateTime >= time)
                    {
                        _cachedIndexSearches.Add(time, i);
                        return i;
                    }
                }
            }
            return -1;
        }

      

    }
}
