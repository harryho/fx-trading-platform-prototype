// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.ObjectModel;

namespace fxpa
{
    public interface IOrderExecutionProvider 
    {
        string Symbol { get; }

        /// <summary>
        /// Orders are expected to be provided in chronological order.
        /// </summary>
        ReadOnlyCollection<Order> Orders { get; }

        //OrderExecutionAccount Account { get; }

        //AccountInfo GetAccountInfo();

        /// <summary>
        /// This is where time control how be exercised on a provider. 
        /// If provider does not support time control, member is null.
        /// </summary>
        ITimeControl TimeControl { get; }

        event GeneralHelper.GenericDelegate<Order> OrderAddedEvent;
        event GeneralHelper.GenericDelegate<Order> OrderUpdatedEvent;
        event GeneralHelper.GenericDelegate<Order> OrderRemovedEvent;

        bool OpenOrder(Order order, OrderTypeEnum orderType, double volume, double allowedSlippage, double desiredPrice,
            double takeProfit, double stopLoss, string comment, out double openingPrice, out DateTime openingTime, out string operationResultMessage);

        /// <summary>
        /// Data Provider needed, to initialize the newly obtained orders with it.
        /// </summary>
        bool ObtainAllOrders(out string operationResultMessage);

        bool DecreaseOrderVolume(Order order, double volumeDecreasal, double allowedSlippage, double desiredPrice, out double decreasalPrice, out string operationResultMessage);

        bool CloseOrder(Order order, double allowedSlippage, double desiredPrice, out double closingPrice, out DateTime closingTime, out string operationResultMessage);

        bool Initialize(AnalyzerSession session);

        void UnInitialize();
    }
}
