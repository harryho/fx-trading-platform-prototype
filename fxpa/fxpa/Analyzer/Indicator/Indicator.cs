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
    /// <summary>
    /// </summary>
    public abstract class Indicator
    {

        public Indicator() { }
        
        volatile protected IndicatorResults _results;
        public IndicatorResults Results
        {
            get 
            {
                if (this.Enabled == false)
                {
                    return null;
                }
                return _results; 
            }
        }

        volatile private IDataProvider _dataProvider;
        public IDataProvider DataProvider
        {
            get { return _dataProvider; }
        }

        public bool Initialized
        {
            get { return _dataProvider != null; }
        }

        volatile string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        volatile string description = string.Empty;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        volatile bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        bool? _tradeable;
        /// <summary>
        /// Null value here means - do not know.
        /// </summary>
        public bool? Tradeable
        {
            get { return _tradeable; }
        }

        bool? _scaledToQuotes;
        /// <summary>
        /// Null value here means = do not know.
        /// </summary>
        public bool? ScaledToQuotes
        {
            get { return _scaledToQuotes; }
        }

        public delegate void IndicatorCalculatedDelegate(Indicator indicator);
        public event IndicatorCalculatedDelegate IndicatorCalculatedEvent;
        public IndicatorChartSeries ics;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isTradeable"></param>
        /// <param name="isScaledToQuotes">Is the indicator the same number scale as quote data (for ex. a same scaled indicator is a MA indicator, and a RSI is not)</param>
        public Indicator(bool? isTradeable, bool? isScaledToQuotes, string[] resultSetNames)
        {
            _tradeable = isTradeable;
            _scaledToQuotes = isScaledToQuotes;

            name = GetUserFriendlyName();
            _results = new IndicatorResults(this, resultSetNames);
        }

        public bool Initialize(IDataProvider dataProviderSession)
        {
            //TracerHelper.Trace(this.Name);
            lock (this)
            {
                _dataProvider = dataProviderSession;
                _dataProvider.ValuesUpdateEvent += new ValuesUpdatedDelegate(_dataProvider_ValuesUpdateEvent);
            }
            return true;
        }

        void _dataProvider_ValuesUpdateEvent(IDataProvider dataProvider, UpdateType updateType, int count, int stepsRemaining)
        {
            if (updateType != UpdateType.Quote)
            {
                Calculate();
            }
        }

        public void UnInitialize()
        {
            lock (this)
            {
                if (_dataProvider != null)
                {
                    _dataProvider.ValuesUpdateEvent -= new ValuesUpdatedDelegate(_dataProvider_ValuesUpdateEvent);
                    _dataProvider = null;
                }

                _results = null;
            }
        }

        /// <summary>
        /// Child classes must be marked with UserFriendlyAttribute, to provide user friendly name.
        /// </summary>
        /// <returns></returns>
        public string GetUserFriendlyName()
        {
            string name = this.GetType().Name;
            CustomNameAttribute.GetClassAttributeValue(this.GetType(), ref name);
            return name;
        }

        /// <summary>
        /// Calculation helper function.
        /// </summary>
        public void Calculate()
        {
            Results.Clear();

            if (this.Initialized == false || this.Enabled == false)
            {
                return;
            }

            if (_dataProvider.DataUnitCount == 0)
            {
                return;
            }

            // Calculate the indicator level lines.
            OnCalculate(0, _dataProvider.DataUnitCount);

            // Start the mostly child performed signal analisys.
            Results.PerformCrossingResultAnalysis(ProvideSignalAnalysisLines());

            Results.PerformExtremumResultAnalysis();

            GeneralHelper.SafeEventRaise(IndicatorCalculatedEvent, this);
        }

        /// <summary>
        /// Perform calculation on the given piece of data.
        /// </summary>
        protected abstract void OnCalculate(int startingIndex, int indexCount);

        /// <summary>
        /// Line indeces always come from lower to biger.
        /// </summary>
        /// <param name="currentSignalPositionValue">This is what currently the value for this position is. Allows to handle multiple signals at a given place.</param>
        virtual public float OnResultAnalysisCrossingFound(int line1index, double line1value, int line2index, double line2value, bool direction, double currentPositionSignalValue)
        {
            return 0;
        }

        /// <summary>
        /// Line indeces always come from lower to biger.
        /// </summary>
        /// <param name="currentSignalPositionValue">This is what currently the value for this position is. Allows to handle multiple signals at a given place.</param>
        virtual public float OnResultAnalysisExtremumFound(int lineIndex, double lineValue, bool extremumDirection, double currentPositionSignalValue)
        {
            return 0;
        }

        /// <summary>
        /// The child should return null in case it want no calculation to be done, or not inherit at all.
        /// </summary>
        /// <returns></returns>
        virtual protected double[][] ProvideSignalAnalysisLines()
        {
            return null;
        }


    }
}
