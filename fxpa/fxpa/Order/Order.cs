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
    [CustomName("Trading Order")]
    public class Order
    {
        public enum ResultModeEnum
        {
            Currency,
            Raw,
            // Pips do not consider volume.
            Pips,
        }

        AnalyzerSession _session;

        protected IDataProvider DataProvider
        {
            get
            {
                lock (this)
                {
                    if (_session != null)
                    {
                        return _session.DataProvider;
                    }
                }
                return null;
            }
        }

        public IOrderExecutionProvider OrderExecutionProvider
        {
            get
            {
                lock (this)
                {
                    if (_session != null)
                    {
                        return _session.OrderExecutionProvider;
                    }
                }
                return null;
            }
        }

        public bool IsInitialized
        {
            get { return _session != null; }
        }

        OrderId _id;
        public OrderId Id
        {
            get { lock (this) { return _id; } }
        }

        volatile OrderTypeEnum _type;
        public OrderTypeEnum Type
        {
            get { return _type; }
        }

        double _currentVolume = 0;
        public double CurrentVolume
        {
            get { lock (this) { return _currentVolume; } }
        }

        double _initialVolume = 0;
        public double InitialVolume
        {
            get { lock (this) { return _initialVolume; } }
        }

        /// <summary>
        /// Result stored in absolute units, it does not consider lot size.
        /// So when exported or imported from class this is considered.
        /// </summary>
        double _currentResult = 0;
        double _totalResult = 0;

        public double InitialDirectionalVolume
        {
            get
            {
                if (IsBuy)
                {
                    return InitialVolume;
                }
                else
                {
                    return -InitialVolume;
                }
            }
        }

        public bool IsBuy
        {
            get { return _type == OrderTypeEnum.OP_BUY || _type == OrderTypeEnum.OP_BUYLIMIT || _type == OrderTypeEnum.OP_BUYSTOP; }
        }

        volatile bool _isOpen = false;
        public bool IsOpen
        {
            get { return _isOpen; }
        }

        double _openPrice = double.NaN;
        public double OpenPrice
        {
            get { lock(this) { return _openPrice; } }
        }

        double _closePrice = double.NaN;
        public double ClosePrice
        {
            get { lock (this) { return _closePrice; } }
        }

        double _orderMaximumResultAchieved = 0;
        public double OrderMaximumResultAchieved
        {
            get { lock (this) { return _orderMaximumResultAchieved; } }
        }

        double _defaultExecutionSlippage = 9999;
        public double DefaultExecutionSlippage
        {
            get { lock (this) { return _defaultExecutionSlippage; } }
            set { lock (this) { _defaultExecutionSlippage = value; } }
        }

        string _defaultComment = "";
        public string DefaultComment
        {
            get { return _defaultComment; }
            set { _defaultComment = value; }
        }

        DateTime _sourceOpenTime = DateTime.MinValue;
        public DateTime SourceOpenTime
        {
            get { return _sourceOpenTime; }
        }

        DateTime _sourceCloseTime = DateTime.MinValue;
        public DateTime SourceCloseTime
        {
            get { return _sourceCloseTime; }
        }

        double _sourceStopLoss = double.NaN;
        public double SourceStopLoss
        {
            get { return _sourceStopLoss; }
        }

        double _sourceTakeProfit = double.NaN;
        public double SourceTakeProfit
        {
            get { return _sourceTakeProfit; }
        }


        DateTime _localOpenTime = DateTime.MinValue;
        public DateTime LocalOpenTime
        {
            get { return _localOpenTime; }
        }

        public delegate void OrderUpdatedDelegate(Order order);
        public event OrderUpdatedDelegate OrderUpdatedEvent;

        /// <summary>
        /// 
        /// </summary>
        public Order()
        {
            _id = new OrderId(Guid.NewGuid());
        }

        public bool Initialize(AnalyzerSession session)
        {
            if (this.IsInitialized)
            {
                return (_session == session);
            }

            lock (this)
            {
                _session = session;
                _session.DataProvider.ValuesUpdateEvent += new ValuesUpdatedDelegate(DataProviderSession_OnValuesUpdateEvent);
            }

            return true;
        }

        public void UnInitialize()
        {
            lock (this)
            {
                if (_session != null && _session.DataProvider != null)
                {
                    _session.DataProvider.ValuesUpdateEvent -= new ValuesUpdatedDelegate(DataProviderSession_OnValuesUpdateEvent);
                }

                _session = null;
            }
        }

        protected virtual void DataProviderSession_OnValuesUpdateEvent(IDataProvider dataProvider, UpdateType updateType, int updateItemsCount, int stepsRemaining)
        {
            lock (this)
            {
                if (_session == null || _session.DataProvider == null || 
                    _session.OrderExecutionProvider == null || IsOpen == false)
                {
                    return;
                }

                //UpdateResult();
            } // lock
        }

        //void UpdateResult()
        //{
        //}

        /// <summary>
        /// Will create the corresponding order, based to the passed in order information.
        /// Used to create corresponding orders to ones already existing in the platform.
        /// </summary>
        public virtual bool AdoptExistingOrderInformation(OrderInformation information)
        {
            _id.ExecutionPlatformId = information.OrderPlatformId;
            _type = information.OrderType;

            _currentResult = information.CurrentProfit / _session.SessionInfo.LotSize;

            if (information.IsOpen == false)
            {
                _totalResult = _currentResult;
            }

            _currentVolume = information.Volume;
            _initialVolume = 1;

            _isOpen = information.IsOpen;
            _openPrice = information.OpenPrice;

            _orderMaximumResultAchieved = _currentResult;

            _sourceOpenTime = information.SourceOpenTime;
            _sourceCloseTime = information.SourceCloseTime;

            _sourceStopLoss = information.OrderStopLoss;
            _sourceTakeProfit = information.OrderTakeProfit;

            _localOpenTime = DateTime.Now;

            return true;
        }

        /// <summary>
        /// Calculate order result.
        /// </summary>
        public double GetResult(ResultModeEnum mode)
        {
            lock (this)
            {
                if (this.IsOpen)
                {
                    // Update result.
                    //_currentResult =
                    //    GetRawResultAtPrice(_session.DataProvider.Ask,
                    //    _session.DataProvider.Bid, mode != ResultModeEnum.Pips);
                    //_orderMaximumResultAchieved = Math.Max(_orderMaximumResultAchieved, _currentResult);
                }

                if (mode == ResultModeEnum.Pips)
                {
                    return _currentResult * Math.Pow(10, _session.SessionInfo.PointDigits);
                }

                if (mode == ResultModeEnum.Raw)
                {
                    return _currentResult;
                }
                else if (mode == ResultModeEnum.Currency)
                {
                    //return _session.OrderExecutionProvider.Account.CompensateLotSize(_currentResult * _session.SessionInfo.LotSize);
                }

                //SystemMonitor.Throw("Unhandled mode.");
                return 0;
            }
        }

        #region User Operations

        /// <summary>
        /// Will use current platform prices for the operation.
        /// </summary>
        public virtual bool Open(OrderTypeEnum orderType, double volume)
        {
            double price = DataProvider.Ask; // Buy
            if (orderType == OrderTypeEnum.OP_SELL || orderType == OrderTypeEnum.OP_SELLLIMIT || orderType == OrderTypeEnum.OP_SELLSTOP)
            {// Sell.
                price = DataProvider.Bid;
            }

            return Open(orderType, volume, this.DefaultExecutionSlippage, price, DefaultComment);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Open(OrderTypeEnum orderType, double volume, double allowedSlippage, 
            double desiredPrice, string comment)
        {
            string message;
            return Open(orderType, volume, allowedSlippage, desiredPrice, 0, 0, comment, out message);
        }
        
        /// <summary>
        /// This allows more specific control over the operation.
        /// </summary>
        public virtual bool Open(OrderTypeEnum orderType, double volume, double allowedSlippage, double desiredPrice, double sourceTakeProfit, double sourceStopLoss, string comment, out string operationResultMessage)
        {
            lock (this)
            {
                //SystemMonitor.CheckThrow(volume > 0, "Misuse of the Order class.");
                //SystemMonitor.CheckThrow(IsOpen == false, "Misuse of the Order class [Must not open trade, that has already been open].");

                double openingPrice;

                DateTime sourceOpenTime;
                if (_session.OrderExecutionProvider.OpenOrder(this, orderType, volume, allowedSlippage, desiredPrice, sourceTakeProfit, sourceStopLoss, comment, out openingPrice, out sourceOpenTime, out operationResultMessage) == false)
                {
                    //SystemMonitor.Report("Order was not executed [" + operationResultMessage + "].");
                    return false;
                }

                _type = orderType;
                _initialVolume = volume;
                _currentVolume = volume;

                _sourceStopLoss = sourceStopLoss;
                _sourceTakeProfit = sourceTakeProfit;

                _isOpen = true;
                _openPrice = openingPrice;

                _sourceOpenTime = sourceOpenTime;
                _localOpenTime = DateTime.Now;
            }

            if (OrderUpdatedEvent != null)
            {
                OrderUpdatedEvent(this);
            }

            return true;
        }

        /// <summary>
        /// Uses default slippage and current price to perform the operation.
        /// </summary>
        public bool DecreaseVolume(double volumeDecreasal, int slippage, out string operationResultMessage)
        {
            if (IsBuy)
            {// Buy.
                return DecreaseVolume(volumeDecreasal, slippage, DataProvider.Bid, out operationResultMessage);
            }
            else
            {// Sell.
                return DecreaseVolume(volumeDecreasal, slippage, DataProvider.Ask, out operationResultMessage);
            }
        }

        /// <summary>
        /// Uses default slippage and current price to perform the operation.
        /// </summary>
        public bool DecreaseVolume(double volumeDecreasal, out string operationResultMessage)
        {
            if (IsBuy)
            {// Buy.
                return DecreaseVolume(volumeDecreasal, this.DefaultExecutionSlippage, DataProvider.Bid, out operationResultMessage);
            }
            else
            {// Sell.
                return DecreaseVolume(volumeDecreasal, this.DefaultExecutionSlippage, DataProvider.Ask, out operationResultMessage);
            }
        }

        /// <summary>
        /// This allows a part of the order to be closed, or all.
        /// </summary>
        public bool DecreaseVolume(double volumeDecreasal, double allowedSlippage, double desiredPrice)
        {
            string message;
            return DecreaseVolume(volumeDecreasal, allowedSlippage, desiredPrice, out message);
        }

        /// <summary>
        /// This allows a part of the order to be closed, or all.
        /// </summary>
        public bool DecreaseVolume(double volumeDecreasal, double allowedSlippage, double desiredPrice, 
            out string operationResultMessage)
        {
            //SystemMonitor.CheckThrow(volumeDecreasal >= 0 && volumeDecreasal <= CurrentVolume && IsOpen != false, "Misuse of the Order class [Can not close more volume than already open].");

            double decreasalPrice;
            bool operationResult = false;
            if (volumeDecreasal == _currentVolume)
            {
                operationResult = _session.OrderExecutionProvider.CloseOrder(this, allowedSlippage, desiredPrice, out decreasalPrice, out _sourceCloseTime, out operationResultMessage);
            }
            else
            {
                operationResult = _session.OrderExecutionProvider.DecreaseOrderVolume(this, volumeDecreasal, allowedSlippage, desiredPrice, out decreasalPrice, out operationResultMessage);
            }

            if (operationResult == false)
            {
                //SystemMonitor.Report("Order decrease volume has failed.");
                return false;
            }

            // How much of the volume we shall close here and now.
            double priceDifference = decreasalPrice - this.OpenPrice;
            if (IsBuy == false)
            {
                priceDifference = -priceDifference;
            }

            lock (this)
            {
                // Finally apply the operation to the internal data, also round result to 6 fractional digits.
                _totalResult += Math.Round(volumeDecreasal * priceDifference, 6);

                _currentVolume -= volumeDecreasal;

                if (_currentVolume == 0)
                {
                    _isOpen = false;
                    _closePrice = decreasalPrice;
                    _currentResult = _totalResult;
                }
            }

            if (OrderUpdatedEvent != null)
            {
                OrderUpdatedEvent(this);
            }
            return true;
        }

        /// <summary>
        /// Will close using the current price as reference and default slippage as maximum allowed slippage.
        /// </summary>
        public bool Close()
        {
            if (IsBuy)
            {
                return Close(this.DefaultExecutionSlippage, DataProvider.Bid);
            }
            else
            {
                return Close(this.DefaultExecutionSlippage, DataProvider.Ask);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Close(double allowedSlippage, double desiredPrice)
        {
            return DecreaseVolume(CurrentVolume, allowedSlippage, desiredPrice);
        }
        /// <summary>

        /// </summary>
        public bool Close(out string operationResultMessage)
        {
            return DecreaseVolume(CurrentVolume, out operationResultMessage);
        }

        /// <summary>
        /// Use to see how order is performing at current stage, considerVolume == false to see absolute values.
        /// This will consider accumulated results so far only if considerVolume is true.
        /// ConsiderVolume is to signify should the calculation observe order volume at all.
        /// autoLotSizeCompensation - since lot sizes seem to change, this compensates with a starting point of 100,000
        /// (for ex. the GBPJPY with value 141.000 the lot is size reduced 100 times)
        /// </summary>
        //protected double GetRawResultAtPrice(double ask, double bid, bool considerVolume)
        //{
        //    if (IsOpen == false || _session == null || _session.OrderExecutionProvider == null
        //        || _session.OrderExecutionProvider.Account == null || _session.OrderExecutionProvider.Account.OperationalState != OperationalStateEnum.Operational)
        //    {
        //        return 0;
        //    }

        //    double difference = 0;
        //    if (IsBuy)
        //    {
        //        difference = bid - OpenPrice;
        //    }
        //    else
        //    {
        //        difference = OpenPrice - ask;
        //    }

        //    if (considerVolume)
        //    {
        //        return Math.Round(_totalResult + CurrentVolume * difference, 10);
        //    }
        //    else
        //    {
        //        return Math.Round(difference, 10);
        //    }
        //}

        #endregion
    }
}
