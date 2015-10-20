// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;

using System.Collections.ObjectModel;

namespace fxpa
{
    public sealed class AnalyzerSession
    {
        Symbol symbol;

        public Symbol Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        SessionInfo _sessionInfo;
        public SessionInfo SessionInfo
        {
            get { return _sessionInfo; }
        }

        IDataProvider _dataProvider;
        public IDataProvider DataProvider
        {
            get { return _dataProvider; }
        }

        IOrderExecutionProvider _orderExecutionProvider;
        public IOrderExecutionProvider OrderExecutionProvider
        {
            get { return _orderExecutionProvider; }
        }

        ListEx<Indicator> _indicators = new ListEx<Indicator>();
        public ReadOnlyCollection<Indicator> Indicators
        {
            get { return _indicators.AsReadOnly(); }
        }

        public bool IsInitialized
        {
            get { return _dataProvider != null; }
        }

        #region IOperational Members

        OperationalStateEnum _operationalState = OperationalStateEnum.UnInitialized;
        
        public OperationalStateEnum OperationalState
        {
            get { lock (this) { return _operationalState; } }
        }

        //public event GeneralHelper.GenericDelegate<IOperational> OperationalStatusChangedEvent;

        #endregion

        public delegate void IndicatorUpdateDelegate(AnalyzerSession session, Indicator indicator);
        public event IndicatorUpdateDelegate IndicatorAddedEvent;
        public event IndicatorUpdateDelegate IndicatorRemovedEvent;

        /// <summary>
        /// 
        /// </summary>
        public AnalyzerSession(SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
            symbol = AppUtil.ParseSymbol(sessionInfo.Symbol);
        }

        public bool Initialize(IDataProvider dataProvider, IOrderExecutionProvider orderExectioner)
        {
            lock (this)
            {
                _dataProvider = dataProvider;

                _operationalState = OperationalStateEnum.Initialized;
            }

            //GeneralHelper.SafeEventRaise(OperationalStatusChangedEvent, this);
            return true;
        }

        public bool Initialize(IDataProvider dataProvider)
        {
            lock (this)
            {
                _dataProvider = dataProvider;
                ////_dataProvider.OperationalStatusChangedEvent += new GeneralHelper.GenericDelegate<IOperational>(_subItem_OperationalStatusChangedEvent);
                _operationalState = OperationalStateEnum.Initialized;
            }
            return true;
        }

        public void UnInitialize()
        {
            lock (this)
            {
                _operationalState = OperationalStateEnum.UnInitialized;

                if (_dataProvider != null)
                {
                    //_dataProvider.OperationalStatusChangedEvent -= new GeneralHelper.GenericDelegate<IOperational>(_subItem_OperationalStatusChangedEvent);
                    _dataProvider = null;
                }

                if (_orderExecutionProvider != null)
                {
                    //_orderExecutionProvider.OperationalStatusChangedEvent -= new GeneralHelper.GenericDelegate<IOperational>(_subItem_OperationalStatusChangedEvent);
                    _orderExecutionProvider = null;
                }
            }
            //GeneralHelper.SafeEventRaise(OperationalStatusChangedEvent, this);
        }

        public void Add(Indicator indicator)
        {
            indicator.Calculate();
            if (_indicators.Add(indicator) && IndicatorAddedEvent != null)
            {
                IndicatorAddedEvent(this, indicator);
            }
        }

        public void Remove(Indicator indicator)
        {
            if (_indicators.Remove(indicator) && IndicatorRemovedEvent != null)
            {
                IndicatorRemovedEvent(this, indicator);
            }
        }


    }
}
