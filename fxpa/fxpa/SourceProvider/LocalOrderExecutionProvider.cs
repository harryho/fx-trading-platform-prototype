// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;




namespace fxpa
{
    [CustomName("Local Simulation Order Executioner")]
    public class LocalOrderExecutionProvider 
    {
        AnalyzerSession _expertSession;

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        List<Order> _orders = new List<Order>();

        #region IOrderExecutioner Members

        //public event GeneralHelper.GenericDelegate<IOperational> OperationalStatusChangedEvent;

        OperationalStateEnum _operationalState = OperationalStateEnum.UnInitialized;
        public OperationalStateEnum OperationalState
        {
            get { lock (this) { return _operationalState; } }
        }

        /// <summary>
        /// LiveOrderExecutioner does not support time control.
        /// </summary>
        //public ITimeControl TimeControl
        //{
        //    get { return this; }
        //}

        public string Symbol { get { lock (this) { return _expertSession.SessionInfo.Symbol; } } }

        public ReadOnlyCollection<Order> Orders { get { lock (this) { return _orders.AsReadOnly(); } } }

        public event GeneralHelper.GenericDelegate<Order> OrderAddedEvent;

        public event GeneralHelper.GenericDelegate<Order> OrderUpdatedEvent;

        public event GeneralHelper.GenericDelegate<Order> OrderRemovedEvent;

        #endregion

        public int PeriodsCount
        {
            get { return 0; }
        }

        public int CurrentPeriod
        {
            get { return 0; }
        }

        public TimeSpan Interval
        {
            get { return _expertSession.SessionInfo.TimeInterval; }
        }

        //LocalSimulationExecutionAccount _executionAccount = new LocalSimulationExecutionAccount();
        //public OrderExecutionAccount Account
        //{
        //    get { return _executionAccount; }
        //}

        /// <summary>
        /// 
        /// </summary>
        public LocalOrderExecutionProvider() 
        {
        }
        
        public bool Initialize(AnalyzerSession session)
        {
            lock (this)
            {
                //SystemMonitor.CheckThrow(_expertSession == null);
                _expertSession = session;
                //_executionAccount.Initialize(session);

                double balance = 10000;
                //_executionAccount.InitializeInfo(balance, 0, "NA", session.SessionInfo.BaseCurrency, balance,
                //    0, 100, 0, session.SessionInfo.Symbol, 1, 0, "Local");

                this.Name = "Simulation Order Executioner of " + _expertSession.SessionInfo.Id;

                if (_expertSession != null && _expertSession.DataProvider != null)
                {
                    _expertSession.DataProvider.ValuesUpdateEvent += new ValuesUpdatedDelegate(DataProvider_ValuesUpdateEvent);
                }
                else
                {// Wait for session to be ready before trying to subsribe again.
                    //_expertSession.OperationalStatusChangedEvent += new GeneralHelper.GenericDelegate<IOperational>(_expertSession_OperationalStatusChangedEvent);
                }
            }
            
            _operationalState = OperationalStateEnum.Operational;
            //GeneralHelper.SafeEventRaise(OperationalStatusChangedEvent, this);
            return OperationalState == OperationalStateEnum.Operational;
        }

        //void _expertSession_OperationalStatusChangedEvent(IOperational session)
        //{
        //    if (session.OperationalState != OperationalStateEnum.Operational)
        //    {
        //        return;
        //    }

        //    _expertSession.OperationalStatusChangedEvent -= new GeneralHelper.GenericDelegate<IOperational>(_expertSession_OperationalStatusChangedEvent);
        //    if (_expertSession != null && _expertSession.DataProvider != null)
        //    {
        //        _expertSession.DataProvider.ValuesUpdateEvent += new ValuesUpdatedDelegate(DataProvider_ValuesUpdateEvent);
        //    }
        //}

        public void UnInitialize()
        {
            lock (this)
            {
                _operationalState = OperationalStateEnum.UnInitialized;
            }
            //_executionAccount.UnInitialize();
            //_executionAccount = null;
            if (_expertSession != null && _expertSession.DataProvider != null)
            {
                _expertSession.DataProvider.ValuesUpdateEvent -= new ValuesUpdatedDelegate(DataProvider_ValuesUpdateEvent);
            }
            _expertSession = null;

            //GeneralHelper.SafeEventRaise(OperationalStatusChangedEvent, this);
        }

        /// <summary>
        /// Obtains all the orders from the order executioner.
        /// </summary>
        public bool ObtainAllOrders(out string operationResultMessage)
        {
            operationResultMessage = string.Empty;
            return true;
        }

        protected void RegisterOrder(Order order)
        {
            lock (this)
            {
                //SystemMonitor.CheckThrow(_orders.Contains(order) == false, "Order already added.");
                _orders.Add(order);
                order.OrderUpdatedEvent += new Order.OrderUpdatedDelegate(order_OrderUpdatedEvent);
            }

            GeneralHelper.SafeEventRaise(OrderAddedEvent, order);
        }

        protected void UnRegisterOrder(Order order)
        {
            lock (this)
            {
                //SystemMonitor.CheckThrow(_orders.Contains(order), "Order not present.");
                _orders.Remove(order);
                order.OrderUpdatedEvent -= new Order.OrderUpdatedDelegate(order_OrderUpdatedEvent);
            }

            GeneralHelper.SafeEventRaise(OrderRemovedEvent, order);
        }

        void order_OrderUpdatedEvent(Order order)
        {
            if (OrderUpdatedEvent != null)
            {
                OrderUpdatedEvent(order);
            }
        }

        void DataProvider_ValuesUpdateEvent(IDataProvider dataProvider, UpdateType updateType, int count, int stepsRemaining)
        {
            if (dataProvider.DataUnitCount == 0)
            {
                return;
            }

            foreach (Order order in _orders)
            {
                if (order.IsOpen == false)
                {
                    continue;
                }

                if (double.IsNaN(order.SourceStopLoss) == false
                    && double.IsInfinity(order.SourceStopLoss) == false
                    && order.SourceStopLoss != 0)
                {// Check Stop Loss level.
                    if ((order.IsBuy && dataProvider.Bid <= order.SourceStopLoss) ||
                        (order.IsBuy == false && dataProvider.Ask >= order.SourceStopLoss))
                    {
                        order.Close();
                    }
                }

                if (double.IsNaN(order.SourceTakeProfit) == false
                    && double.IsInfinity(order.SourceTakeProfit) == false
                    && order.SourceTakeProfit != 0)
                {// Check Take Profit level.
                    if ((order.IsBuy && dataProvider.Bid >= order.SourceTakeProfit) ||
                        (order.IsBuy == false && dataProvider.Ask <= order.SourceTakeProfit))
                    {
                        order.Close();
                    }
                }
            }
        }


        public bool OpenOrder(Order order, OrderTypeEnum orderType, double volume, double allowedSlippage, double desiredPrice, double takeProfit, double stopLoss,
            string comment, out double openingPrice, out DateTime sourceOpenTime, out string operationResultMessage)
        {
            RegisterOrder(order);
            operationResultMessage = string.Empty;
            openingPrice = desiredPrice;
            sourceOpenTime = _expertSession.DataProvider.CurrentSourceDateTime;

            if (takeProfit != 0 || stopLoss != 0)
            {
                //SystemMonitor.NotImplementedWarning();
            }
            return true;
        }

        public bool DecreaseOrderVolume(Order order, double volumeDecreasal, double allowedSlippage, double desiredPrice,
            out double decreasalPrice, out string operationResultMessage)
        {
            if (volumeDecreasal == order.CurrentVolume)
            {// Closing order.
                DateTime closingDateTime;
                CloseOrder(order, allowedSlippage, desiredPrice, out decreasalPrice, out closingDateTime, out operationResultMessage);
                return true;
            }

            operationResultMessage = string.Empty;
            decreasalPrice = desiredPrice;
            GeneralHelper.SafeEventRaise(OrderUpdatedEvent, order);
            return true;
        }

        public bool CloseOrder(Order order, double allowedSlippage, double desiredPrice,
            out double closingPrice, out DateTime closingTime, out string operationResultMessage)
        {
            closingPrice = desiredPrice;
            closingTime = _expertSession.DataProvider.CurrentSourceDateTime;
            operationResultMessage = string.Empty;
            GeneralHelper.SafeEventRaise(OrderUpdatedEvent, order);
            return true;
        }


        #region ITimeControl Members

        public bool StepForward()
        {
            return true;
        }

        public bool StepBack()
        {
            return true;
        }

        public bool StepTo(int index)
        {
            return true;
        }

        #endregion


        #region IOrderExecutionProvider Members

        //public AccountInfo GetAccountInfo()
        //{
        //    return _executionAccount;
        //}

        #endregion
    }
}
