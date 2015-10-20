// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace fxpa
{
    /// <summary>
    /// This class brings together the (Data/OrderExecution)Provider(s) and the ChartSeries, allowing the data to be rendered.
    /// </summary>
    public class ProviderTradeChartSeries : TradeChartSeries
    {
        IDataProvider _dataProvider;
        IOrderExecutionProvider _orderExecutionProvider;

        Image _imageUp;
        Image _imageDown;
        Image _imageCross;

        volatile bool _showOrderArrow = true;
        
        /// <summary>
        /// Show the orders arrow below the bar.
        /// </summary>
        public bool ShowOrderArrow
        {
            get { return _showOrderArrow; }
            set { _showOrderArrow = value; }
        }


        volatile bool _showOrderSpot = true;
        
        /// <summary>
        /// Show the circle and line on the spot of the very order price.
        /// </summary>
        public bool ShowOrderSpot
        {
            get { return _showOrderSpot; }
            set { _showOrderSpot = value; }
        }

        volatile bool _showClosedOrdersTracing = true;

        /// <summary>
        /// Show order tracing line - from open to close.
        /// </summary>
        public bool ShowClosedOrdersTracing
        {
            get { return _showClosedOrdersTracing; }
            set { _showClosedOrdersTracing = value; }
        }

        volatile bool _showPendingOrdersTracing = true;
        
        /// <summary>
        /// Show order tracing line for open orders - from open price to current price.
        /// </summary>
        public bool ShowPendingOrdersTracing
        {
            get { return _showPendingOrdersTracing; }
            set { _showPendingOrdersTracing = value; }
        }

        Pen _buyDashedPen = new Pen(Color.Green);
        Pen _sellDashedPen = new Pen(Color.Red);

        volatile bool _drawCurrentPriceLevel = true;
        public bool DrawCurrentPriceLevel
        {
            get { return _drawCurrentPriceLevel; }
            set { _drawCurrentPriceLevel = value; }
        }

        Pen _priceLevelPen = new Pen(Color.Gray);
        public Pen PriceLevelPen
        {
            set { _priceLevelPen = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ProviderTradeChartSeries()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public ProviderTradeChartSeries(string name)
        {
            base.Name = name;
        }

        public void Initialize(IDataProvider dataProvider, IOrderExecutionProvider orderExecutionProvider)
        {
            lock (this)
            {
                _dataProvider = dataProvider;
                _dataProvider.ValuesUpdateEvent += new ValuesUpdatedDelegate(_provider_ValuesUpdateEvent);

                _orderExecutionProvider = orderExecutionProvider;
                if (_orderExecutionProvider != null)
                {
                    _orderExecutionProvider.OrderAddedEvent += new GeneralHelper.GenericDelegate<Order>(_orderExecutionProvider_OrderUpdate);
                    _orderExecutionProvider.OrderRemovedEvent += new GeneralHelper.GenericDelegate<Order>(_orderExecutionProvider_OrderUpdate);
                    _orderExecutionProvider.OrderUpdatedEvent += new GeneralHelper.GenericDelegate<Order>(_orderExecutionProvider_OrderUpdate);
                }

                ComponentResourceManager resources = new ComponentResourceManager(typeof(ProviderTradeChartSeries));
                _imageDown = ((Image)(resources.GetObject("imageDown")));
                _imageUp = ((Image)(resources.GetObject("imageUp")));
                _imageCross = ((Image)(resources.GetObject("imageCross")));

                _buyDashedPen.DashPattern = new float[] { 5, 5 };
                _buyDashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;

                _priceLevelPen.DashPattern = new float[] { 3, 3 };
                _priceLevelPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;

                _sellDashedPen.DashPattern = new float[] { 5, 5 };
                _sellDashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            }
        }

        void _orderExecutionProvider_OrderUpdate(Order parameter1)
        {
            RaiseValuesUpdated(true);
        }

        void UpdateValues()
        {
            lock (this)
            {
                BarData[] bars = new BarData[_dataProvider.DataUnits.Count];
                _dataProvider.DataUnits.CopyTo(bars, 0);

                double scaling = 1;
                for (int i = 0; i < bars.Length; i++)
                {
                    BarData bar = bars[i];
                    bars[i] = new BarData(bar.DateTime, bar.Open * scaling, bar.Close * scaling, bar.Low * scaling, bar.High * scaling, bar.Volume * scaling);
                }
                if (bars.Length > 0)
                {
                    this.SetBarData(bars, _dataProvider.TimeInterval);
                }

                RaiseValuesUpdated(true);
            }
        }

        void _provider_ValuesUpdateEvent(IDataProvider dataProvider, UpdateType updateType, int updatedItemsCount, int stepsRemaining)
        {
            if (stepsRemaining == 0)
            {
                UpdateValues();
            }
        }

        public void UnInitialize()
        {
            lock (this)
            {
                if (_dataProvider != null)
                {
                    _dataProvider.ValuesUpdateEvent -= new ValuesUpdatedDelegate(_provider_ValuesUpdateEvent);
                    _dataProvider = null;
                }

                if (_orderExecutionProvider != null)
                {
                    _orderExecutionProvider.OrderAddedEvent -= new GeneralHelper.GenericDelegate<Order>(_orderExecutionProvider_OrderUpdate);
                    _orderExecutionProvider.OrderRemovedEvent -= new GeneralHelper.GenericDelegate<Order>(_orderExecutionProvider_OrderUpdate);
                    _orderExecutionProvider.OrderUpdatedEvent -= new GeneralHelper.GenericDelegate<Order>(_orderExecutionProvider_OrderUpdate);

                    _orderExecutionProvider = null;
                }

                _buyDashedPen.Dispose();
                _sellDashedPen.Dispose();
            }
        }

        protected override void OnAddedToChart()
        {
            UpdateValues();
        }

        protected override void OnRemovedFromChart()
        {
            UnInitialize();
        }

        public override void Draw(GraphicsWrapper g, int unitsUnification, 
            RectangleF clippingRectangle, float itemWidth, float itemMargin)
        {
            base.Draw(g, unitsUnification, clippingRectangle, itemWidth, itemMargin);

            if (Visible == false)
            {
                return;
            }

            // Draw ask line.
            if (_dataProvider != null && _drawCurrentPriceLevel ) //&& _dataProvider.OperationalState == OperationalStateEnum.Operational)
            {
                float price = (float)_dataProvider.Bid;
                g.DrawLine(_priceLevelPen, clippingRectangle.X, price, clippingRectangle.X + clippingRectangle.Width, price);
            }

            List<Order> ordersOpening;

            // Draw orders locations on chart.
            lock(this)
            {
                if (_orderExecutionProvider == null)
                {
                    return;
                }

                // Render orders.
                ordersOpening = new List<Order>(_orderExecutionProvider.Orders);
            }

            // Use for orders closes.
            List<Order> ordersClosing = new List<Order>();
            foreach (Order order in ordersOpening)
            {
                if (order.IsOpen == false)
                {// Only add orders already closed.
                    ordersClosing.Add(order);
                }
            }

            // This is used later on, since ordersClosing is modified.
            List<Order> ordersClosed = new List<Order>(ordersClosing);

            // Orders opening at current bar.
            List<Order> pendingOpeningOrders = new List<Order>();
            // Order closing at current bar.
            List<Order> pendingClosingOrders = new List<Order>();

            PointF drawingPoint = new PointF();
            int startIndex, endIndex;
            GetDrawingRangeIndecesFromClippingRectange(clippingRectangle, drawingPoint, unitsUnification, out startIndex, out endIndex, itemWidth, itemMargin);

            lock (this)
            {
                float lastBarX = (itemMargin + itemWidth) * _dataBars.Count;
                for (int i = startIndex; i < endIndex && i < _dataBars.Count
                    && (ordersOpening.Count > 0 || ordersClosing.Count > 0); i++)
                {// Foreach bar, draw orders (and volume).

                    while (ordersOpening.Count > 0)
                    {// All orders before now.
                        if (ordersOpening[0].SourceOpenTime < (_dataBars[i].DateTime - _dataProvider.TimeInterval))
                        {// Order before time period.
                            if (ordersOpening[0].IsOpen && _showPendingOrdersTracing)
                            {// Since it is an open pending order, we shall also need to draw it as well.
                                pendingOpeningOrders.Add(ordersOpening[0]);
                            }
                            ordersOpening.RemoveAt(0);
                            continue;
                        }

                        if (ordersOpening[0].SourceOpenTime > _dataBars[i].DateTime)
                        {// Order after time period - look no further.
                            break;
                        }

                        // Order open is within the current period.
                        // Only if order is part of the current period - add to pending.
                        pendingOpeningOrders.Add(ordersOpening[0]);
                        ordersOpening.RemoveAt(0);
                    }

                    for (int j = ordersClosing.Count - 1; j >= 0; j--)
                    {
                        if (ordersClosing[j].SourceCloseTime >= (_dataBars[i].DateTime - _dataProvider.TimeInterval) &&
                            ordersClosing[j].SourceCloseTime <= _dataBars[i].DateTime)
                        {// Order close is within the current period.
                            pendingClosingOrders.Add(ordersClosing[j]);
                            ordersClosing.RemoveAt(j);
                        }
                    }

                    drawingPoint.X = i * (itemMargin + itemWidth);
                    DrawOrders(g, i, drawingPoint, itemWidth, itemMargin, pendingOpeningOrders, pendingClosingOrders, _dataProvider.DataUnits[i], lastBarX);
                    pendingOpeningOrders.Clear();
                    pendingClosingOrders.Clear();
                }

                if (_showClosedOrdersTracing && _dataBars.Count > 0 && startIndex < _dataBars.Count)
                {// Since a closed order may be before or after (or during) the curren set of periods - make a special search and render for them.
                    endIndex = Math.Max(0, endIndex);
                    endIndex = Math.Min(_dataBars.Count - 1, endIndex);

                    foreach (Order order in ordersClosed)
                    {
                        if (order.SourceOpenTime <= _dataBars[endIndex].DateTime
                            && order.SourceCloseTime >= _dataBars[startIndex].DateTime - _dataProvider.TimeInterval)
                        {
                            int openIndex = _dataProvider.GetBarIndexAtTime(order.SourceOpenTime);
                            int closeIndex = _dataProvider.GetBarIndexAtTime(order.SourceCloseTime);

                            Pen pen = _buyDashedPen;
                            if (order.IsBuy == false)
                            {
                                pen = _sellDashedPen;
                            }

                            g.DrawLine(pen, new PointF(openIndex * (itemWidth + itemMargin), (float)order.OpenPrice),
                                new PointF(closeIndex * (itemWidth + itemMargin), (float)order.ClosePrice));
                        }
                    }
                }

            } // Lock
        }

        void DrawOrders(GraphicsWrapper g, int index, PointF drawingPoint, float itemWidth, float itemMargin, 
            List<Order> openingOrders, List<Order> closingOrders, BarData orderBarData, float lastBarX)
        {
            // Width is same as items in real coordinates.
            float actualImageHeight = _imageDown.Height / Math.Abs(g.DrawingSpaceTransform.Elements[3]);

            float yToXScaling = Math.Abs(g.DrawingSpaceTransform.Elements[0] / g.DrawingSpaceTransform.Elements[3]);
            PointF updatedImageDrawingPoint = drawingPoint;
            foreach (Order order in openingOrders)
            {
                DrawOrder(g, ref updatedImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, orderBarData, lastBarX, true);
            }

            foreach (Order order in closingOrders)
            {
                DrawOrder(g, ref updatedImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, orderBarData, lastBarX, false);
            }

        }


        void DrawOrder(GraphicsWrapper g, ref PointF updatedImageDrawingPoint, Order order, float itemWidth, float itemMargin,
            float yToXScaling, BarData orderBarData, float lastBarX, bool drawOpening)
        {
            Image image = _imageUp;
            Brush brush = Brushes.Green;
            Pen dashedPen = _buyDashedPen;
            Pen pen = Pens.GreenYellow;
            if (order.IsBuy == false)
            {
                image = _imageDown;
                brush = Brushes.Red;
                pen = Pens.Red;
                dashedPen = _sellDashedPen;
            }

            if (drawOpening == false)
            {
                image = _imageCross;
            }

            float price = (float)order.OpenPrice;
            if (drawOpening == false)
            {
                price = (float)order.ClosePrice;
            }

            if (drawOpening && _showPendingOrdersTracing && order.IsOpen)
            {// Open orders tracking line.
                PointF point1 = new PointF(updatedImageDrawingPoint.X + itemWidth / 2f, updatedImageDrawingPoint.Y + price);
                float sellPrice = (float)_dataProvider.Bid;
                if (order.IsBuy == false)
                {
                    sellPrice = (float)_dataProvider.Ask;
                }
                PointF point2 = new PointF(lastBarX - itemWidth / 2f, updatedImageDrawingPoint.Y + sellPrice);
                g.DrawLine(dashedPen, point1, point2);
            }

            //if (drawOpening && _showClosedOrdersTracing && order.IsOpen == false)
            //{// Closed order tracking.
            // Close order tracing is implemented in main draw function.
            //}

            if (_showOrderSpot)
            {
                PointF basePoint = new PointF(updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y + price);
                float height = (yToXScaling * itemWidth);
                if (order.IsBuy == false)
                {
                    height = -height;
                }

                if (drawOpening)
                {
                    g.FillPolygon(brush, new PointF[] { basePoint, new PointF(basePoint.X + itemWidth, basePoint.Y), 
                        new PointF(basePoint.X + (itemWidth / 2f), basePoint.Y + height) });
                    g.DrawPolygon(Pens.White, new PointF[] { basePoint, new PointF(basePoint.X + itemWidth, basePoint.Y), 
                        new PointF(basePoint.X + (itemWidth / 2f), basePoint.Y + height) });

                    // Take profit level.
                    if (double.IsInfinity(order.SourceTakeProfit) == false
                        && double.IsNaN(order.SourceTakeProfit) == false
                        && order.SourceTakeProfit != 0)
                    {
                        g.DrawLine(pen, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y + (float)order.SourceTakeProfit,
                            updatedImageDrawingPoint.X + itemWidth, updatedImageDrawingPoint.Y + (float)order.SourceTakeProfit);

                        g.DrawLine(pen, updatedImageDrawingPoint.X + itemWidth / 2f, updatedImageDrawingPoint.Y + (float)order.SourceTakeProfit,
                            updatedImageDrawingPoint.X + itemWidth / 2f, updatedImageDrawingPoint.Y + (float)order.SourceTakeProfit - height);
                    }

                    // Stop loss level.
                    if (double.IsInfinity(order.SourceStopLoss) == false
                        && double.IsNaN(order.SourceStopLoss) == false
                        && order.SourceStopLoss != 0)
                    {
                        g.DrawLine(pen, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y + (float)order.SourceStopLoss,
                            updatedImageDrawingPoint.X + itemWidth, updatedImageDrawingPoint.Y + (float)order.SourceStopLoss);

                        g.DrawLine(pen, updatedImageDrawingPoint.X + itemWidth / 2f, updatedImageDrawingPoint.Y + (float)order.SourceStopLoss,
                            updatedImageDrawingPoint.X + itemWidth / 2f, updatedImageDrawingPoint.Y + (float)order.SourceStopLoss + height);
                    }
                }
                else
                {
                    g.DrawRectangle(Pens.White, basePoint.X, basePoint.Y, 
                        itemWidth, yToXScaling * itemWidth);
                }

            }

            float imageHeight = 2 * (yToXScaling * itemWidth);
            if (_showOrderArrow)
            {
                // Draw up image.

                g.DrawImage(image, updatedImageDrawingPoint.X - (itemWidth / 2f), updatedImageDrawingPoint.Y +
                    (float)orderBarData.Low - (yToXScaling * itemWidth), 2 * itemWidth, -imageHeight);

                updatedImageDrawingPoint.Y -= 1.2f * imageHeight;
            }


        }

    }
}
