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
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace fxpa
{
    /// <summary>
    /// Thread safe.
    /// </summary>
    public class TradeChartSeries : SimpleChartSeries
    {
        public new enum ChartTypeEnum
        {
            CandleStick,
            BarChart,
            ColoredArea,
            Histogram,
            Line,
        }

        public override string[] ChartTypes
        {
            get { return Enum.GetNames(typeof(ChartTypeEnum)); }
        }

        public override string SelectedChartType
        {
            get
            {
                return Enum.GetName(typeof(ChartTypeEnum), _chartType);
            }
        }

        volatile ChartTypeEnum _chartType = ChartTypeEnum.BarChart;
        public new ChartTypeEnum ChartType
        {
            get { return _chartType; }
            set { _chartType = value; }
        }

        protected TimeSpan? _period;
        public TimeSpan? Period
        {
            get { return _period; }
        }

        /// <summary>
        /// Needed for proper rendering of volume (since it is uniform scaled).
        /// </summary>
        float _minVolume = 0;
        float _maxVolume = 0;

        protected List<BarData> _dataBars = new List<BarData>();
        public ReadOnlyCollection<BarData> DataBars
        {
            get
            {
                lock (this)
                {
                    return _dataBars.AsReadOnly();
                }
            }
        }

        public int DataBarsCount
        {
            get { return _dataBars.Count; }
        }

        public override SeriesTypeEnum SeriesType
        {
            get
            {
                return SeriesTypeEnum.TimeBased;
            }
        }

        BarData.DataValueSourceEnum _simpleValuesType = BarData.DataValueSourceEnum.Close;
        /// <summary>
        /// Simple values are created when a bar data source is put in, or when you put them in manually.
        /// </summary>
        public BarData.DataValueSourceEnum SimpleValuesSourceType
        {
            get { return _simpleValuesType; }
            set
            {
                _simpleValuesType = value;
                if (_dataBars.Count > 0)
                {// Refresh the simple values array in this case.
                    UpdateSimpleValues();
                }
            }
        }


        // Harry -- MOD
        Brush _risingBarFill = null;
        public Brush RisingBarFill
        {
            set { _risingBarFill = value; }
        }

        Brush _fallingBarFill = Brushes.Green;
        public Brush FallingBarFill
        {
            set { _fallingBarFill = value; }
        }

        Pen _risingBarPen = Pens.Red;
        public Pen RisingBarPen
        {
            set { _risingBarPen = value; }
        }

        Pen _fallingBarPen = Pens.LightGreen;
        public Pen FallingBarPen
        {
            set { _fallingBarPen = value; }
        }

        Pen _timeGapsLinePen = new Pen(Color.DarkRed);
        /// <summary>
        /// Time gaps occur when there is space between one period and the next.
        /// </summary>
        public Pen TimeGapsLinePen
        {
            set { _timeGapsLinePen = value; }
        }

        volatile bool _showTimeGaps = false;
        /// <summary>
        /// If enabled, time gaps are shown but only on zoom 1 (units unification = 1).
        /// </summary>
        public bool ShowTimeGaps
        {
            get { return _showTimeGaps; }
            set { _showTimeGaps = value; }
        }

        bool _showVolume = true;
        public bool ShowVolume
        {
            get { return _showVolume; }
            set { _showVolume = value; }
        }

        Pen _volumePen = Pens.Gainsboro;
        public Pen VolumePen
        {
            get { return _volumePen; }
            set { _volumePen = value; }
        }

        Brush _volumeBrush = Brushes.Gainsboro;
        public Brush VolumeBrush
        {
            set { _volumeBrush = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TradeChartSeries()
        {
            _timeGapsLinePen.DashPattern = new float[] { 10, 10 };
            _timeGapsLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TradeChartSeries));
            _imageDown = ((Image)(resources.GetObject("imageDown")));
            _imageUp = ((Image)(resources.GetObject("imageUp")));
            _imageCross = ((Image)(resources.GetObject("imageCross")));
            _imageCross = ((Image)(resources.GetObject("imageStopLost")));
            _imageCross = ((Image)(resources.GetObject("imageInfo")));
            //_imageDown = ((Image)(resources.GetObject("imageStopLost")));
            //_imageUp = ((Image)(resources.GetObject("imageInfo")));
        }

        public TradeChartSeries(string name)
        {
            base.Name = name;
        }
        IDataProvider _dataProvider;
        public void Initialize(IDataProvider dataProvider)
        {
            lock (this)
            {
                _dataProvider = dataProvider;
                _dataProvider.ValuesUpdateEvent += new ValuesUpdatedDelegate(_provider_ValuesUpdateEvent);

                ComponentResourceManager resources = new ComponentResourceManager(typeof(TradeChartSeries));
                //_imageDown = ((Image)(resources.GetObject("imageDown")));
                //_imageUp = ((Image)(resources.GetObject("imageUp")));
                //_imageCross = ((Image)(resources.GetObject("imageCross")));

                //_imageStopGain = ((Image)(resources.GetObject("imageStopGain")));
                //_imageGainTip = ((Image)(resources.GetObject("star4")));

                _imageDown = ((Image)(resources.GetObject("arrow_down")));
                _imageUp = ((Image)(resources.GetObject("arrow_up")));
                _imageStopLoss = ((Image)(resources.GetObject("imageStopLost")));
                //_imageStopLoss = ((Image)(resources.GetObject("tt")));
                _imageStopGain = ((Image)(resources.GetObject("vv")));
                _imageGainTip = ((Image)(resources.GetObject("dd")));

                ////_imageTip = ((Image)(resources.GetObject("imageTip")));

                //_imageUp = ((Image)(resources.GetObject("imageInfo")));

            }
        }

        void _provider_ValuesUpdateEvent(IDataProvider dataProvider, UpdateType updateType, int updatedItemsCount, int stepsRemaining)
        {
            if (stepsRemaining == 0)
            {
                UpdateValues();
            }
        }

        void UpdateValues()
        {
            lock (this)
            {
                BarData[] bars;
                lock (_dataProvider.DataUnits)
                {
                    bars = new BarData[_dataProvider.DataUnits.Count];
                    _dataProvider.DataUnits.CopyTo(bars, 0);
                }

                if (null != bars && bars.Length > 0)
                {
                    this.SetBarData(bars, _dataProvider.TimeInterval);
                    RaiseValuesUpdated(true);
                }


            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TradeChartSeries(string name, ChartTypeEnum chartType)
            : base(name)
        {
            _chartType = chartType;
        }

        /// <summary>
        /// 
        /// </summary>
        public TradeChartSeries(string name, ChartTypeEnum chartType, BarData[] bars, TimeSpan period)
        {
            _chartType = chartType;

            SetBarData(bars, period);
        }

        /// <summary>
        /// Establish the total minimum value of any item in this interval.
        /// </summary>
        /// <param name="startIndex">Inclusive starting index.</param>
        /// <param name="endIndex">Exclusive ending index.</param>
        public override float GetTotalMinimum(int startIndex, int endIndex)
        {
            float min = float.MaxValue;
            lock (this)
            {
                if (_dataBars.Count > 0)
                {
                    for (int i = startIndex; i < endIndex && i < _dataBars.Count; i++)
                    {
                        // Modified due to mouse move
                        if (i > 0 && _dataBars[i].HasDataValues)
                        {
                            //min = (float)Math.Min(_dataBars[i].Low, min);
                            min = (float)Math.Min(_dataBars[i].ExLow, min);
                        }
                    }
                }
                else
                {
                    min = base.GetTotalMinimum(startIndex, endIndex);
                }
            }
            return min;
        }

        /// <summary>
        /// Establish the total maximum value of any item in this interval.
        /// </summary>
        /// <param name="startIndex">Inclusive starting index.</param>
        /// <param name="endIndex">Exclusive ending index.</param>
        public override float GetTotalMaximum(int startIndex, int endIndex)
        {
            float max = float.MinValue;
            lock (this)
            {
                if (_dataBars.Count > 0)
                {
                    for (int i = startIndex; i < endIndex && i < _dataBars.Count; i++)
                    {
                        if (i > 0 && _dataBars[i].HasDataValues)
                        {
                            //max = (float)Math.Max(_dataBars[i].High, max);
                            max = (float)Math.Max(_dataBars[i].ExHigh, max);
                        }
                    }
                }
                else
                {
                    base.GetTotalMaximum(startIndex, endIndex);
                }
            }

            return max;
        }

        public override DateTime GetTimeAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].DateTime;
            }
        }

        public override double GetOpenAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Open;
            }
        }

        public override double GetCloseAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Close;
            }
        }

        public override double GetLowAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Low;
            }
        }
        public override double GetExLowAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].ExLow;
            }
        }

        public override double GetHightAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].High;
            }
        }

        public override double GetExHightAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].ExHigh;
            }
        }

        public override double GetSarAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Sar;
            }
        }
        public override double GetCciAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Cci;
            }
        }

        public override double GetCci2AtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Cci2;
            }
        }

        public override double GetCci3AtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Cci3;
            }
        }

        public override double GetWrAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Wr;
            }

        }


        public override double GetWr2AtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Wr2;
            }

        }

        public override Dictionary<string, double> GetLwrAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Lwr;
            }
        }
        public override Dictionary<string, double> GetBollAtIndex(int index)
        {

            lock (this)
            {
                return _dataBars[index].Boll;
            }
        }

        public override Dictionary<string, double> GetIndicatorsAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Indicators;
            }
        }

        public override double GetRsiAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Rsi;
            }
        }
        public override double GetRsi2AtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Rsi2;
            }

        }

        public override double GetRsi3AtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Rsi3;
            }

        }

        public override double GetRsi4AtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Rsi4;
            }

        }
        public override double GetArAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Ar;
            }
        }
        public override double GetAr2AtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Ar2;
            }
        }
        public override double GetBrAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Br;
            }
        }
        public override double GetCrAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].Cr;
            }
        }

        public override List<CandleSignal> GetSignalsAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index].SignalList;
            }
        }

        public override BarData GetCandleAtIndex(int index)
        {
            lock (this)
            {
                return _dataBars[index];
            }
        }
        //public override double GetRSIAtIndex(int index)
        //{
        //    lock (this)
        //    {
        //        return _dataBars[index].Rsi;
        //    }
        //}

        public override void SaveToFile(string fileName)
        {
            BarData[] dataBars;
            lock (this)
            {
                dataBars = _dataBars.ToArray();
            }
            BarDataHelper.SaveToFile(BarDataHelper.FileFormat.CVSDefault, fileName, dataBars);
        }

        /// <summary>
        /// To handle bar data properly it must have a regularly spread items over the period.
        /// </summary>
        public bool SetBarData(IEnumerable<BarData> datas, TimeSpan period)
        {
            lock (this)
            {
                _period = period;

                _dataBars.Clear();
                _dataBars.AddRange(datas);

                _minVolume = float.MaxValue;
                _maxVolume = 0;

                UpdateSimpleValues();
            }

            return true;
        }

        void UpdateSimpleValues()
        {
            // Harry -- Comment -- 2009.09.22
            //Create corresponding simple values and calculate min and max volume.
            float[] simpleValues = new float[_dataBars.Count];
            for (int i = 0; i < _dataBars.Count; i++)
            {// Empty values will have the value of double.NaN assigned.
                simpleValues[i] = (float)_dataBars[i].GetValue(_simpleValuesType);

                _minVolume = (float)Math.Min(_minVolume, _dataBars[i].Volume);
                _maxVolume = (float)Math.Max(_maxVolume, _dataBars[i].Volume);
            }

            if (_minVolume == float.MaxValue || float.IsPositiveInfinity(_minVolume))
            {
                _minVolume = 0;
            }

            base.SetValues(new float[][] { simpleValues });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="unitsUnification">Draw a few units as one. Used when zooming is too big to show each unit - so unify them. 1 means no unification, 10 means unify 10 units together</param>
        /// <param name="clippingRectangle"></param>
        /// <param name="itemWidth"></param>
        /// <param name="itemMargin"></param>
        public override void Draw(GraphicsWrapper g, int unitsUnification, RectangleF clippingRectangle, float itemWidth, float itemMargin)
        {
            if (Visible == false)
            {
                return;
            }

            PointF drawingPoint = new PointF();
            lock (this)
            {
                if (_chartType == ChartTypeEnum.Line)
                {
                    SetChartType(SimpleChartSeries.ChartTypeEnum.Line);
                    base.Draw(g, unitsUnification, clippingRectangle, itemWidth, itemMargin);
                }
                else if (_chartType == ChartTypeEnum.Histogram)
                {
                    SetChartType(SimpleChartSeries.ChartTypeEnum.Histogram);
                    base.Draw(g, unitsUnification, clippingRectangle, itemWidth, itemMargin);
                }
                else if (_chartType == ChartTypeEnum.ColoredArea)
                {
                    SetChartType(SimpleChartSeries.ChartTypeEnum.ColoredArea);
                    base.Draw(g, unitsUnification, clippingRectangle, itemWidth, itemMargin);
                }
                else if (_chartType == ChartTypeEnum.CandleStick || _chartType == ChartTypeEnum.BarChart)
                {// Unit unification is done trough combining many bars together.
                    List<BarData> combinationDatas = new List<BarData>();
                    bool timeGapFound = false;

                    int startIndex, endIndex;
                    GetDrawingRangeIndecesFromClippingRectange(clippingRectangle, drawingPoint, unitsUnification, out startIndex, out endIndex, itemWidth, itemMargin);

                    float volumeDisplayRange = clippingRectangle.Height / 4f;
                    double volumeDisplayMultiplicator = volumeDisplayRange / _maxVolume;

                    for (int i = startIndex; i < endIndex && i < _dataBars.Count; i++)
                    {
                        combinationDatas.Add(_dataBars[i]);

                        if (unitsUnification == 1 && ShowTimeGaps && i > 0 && _dataBars[i].DateTime - _dataBars[i - 1].DateTime != _period)
                        {
                            timeGapFound = true;
                        }

                        if (i % unitsUnification == 0)
                        {

                            BarData combinedData = BarData.CombinedBar(combinationDatas.ToArray());

                            if (i == _dataBars.Count - 1)
                                Console.WriteLine("  LAST  combinedData ######  " + combinedData);
                            combinationDatas.Clear();
                            if (combinedData.HasDataValues)
                            //&& drawingPoint.X >= clippingRectangle.X 
                            //&& drawingPoint.X <= clippingRectangle.X + clippingRectangle.Width)
                            {
                                if (timeGapFound && ShowTimeGaps && _timeGapsLinePen != null)
                                {// Draw time gap.
                                    timeGapFound = false;
                                    g.DrawLine(_timeGapsLinePen, new PointF(drawingPoint.X, clippingRectangle.Y), new PointF(drawingPoint.X, clippingRectangle.Y + clippingRectangle.Height));

                                    //g.DrawLine(_timeGapsLinePen, new PointF(drawingPoint.X, clippingRectangle.Y), new PointF(drawingPoint.X, (float)(data.High)));
                                    //g.DrawLine(_timeGapsLinePen, new PointF(drawingPoint.X, (float)(data.High + data.BarTotalLength / 2f)), new PointF(drawingPoint.X, clippingRectangle.Y + clippingRectangle.Height));
                                }

                                if (_chartType == ChartTypeEnum.CandleStick)
                                {
                                    if (i > 0 && combinedData.Open > _dataBars[i - 1].Close)
                                        combinedData.IsHigherPrev = true;

                                    DrawCandleStick(g, ref drawingPoint, combinedData, itemWidth, itemMargin, unitsUnification);
                                }
                                else
                                {
                                    DrawBar(g, ref drawingPoint, combinedData, itemWidth, itemMargin, unitsUnification);
                                }

                                // Draw volume for this bar.
                                if (_showVolume)
                                {
                                    float actualHeight = (float)(combinedData.Volume * volumeDisplayMultiplicator);
                                    g.DrawLine(_volumePen, drawingPoint.X, clippingRectangle.Y, drawingPoint.X, clippingRectangle.Y + actualHeight);
                                }

                                if (combinedData.Sar != 0)
                                {
                                    float dy = drawingPoint.Y;
                                    float y = (drawingPoint.Y + (float)combinedData.Sar);

                                    float yts = Math.Abs(g.DrawingSpaceTransform.Elements[0] / g.DrawingSpaceTransform.Elements[3]);
                                    float x = drawingPoint.X + (1 + yts) * itemWidth * 3f / 16f;
                                    //float x = drawingPoint.X + (1 + yts) * itemWidth * 3f / 8f;
                                    if (combinedData.Sar >= combinedData.Low && combinedData.High >= combinedData.Sar)
                                        g.DrawEllipse(new Pen(Color.White), x, y, yts * itemWidth / 2f, yts * itemWidth / 2f);
                                    else
                                        g.DrawEllipse(new Pen(Color.White), x, y, yts * itemWidth / 2f, yts * itemWidth / 2f);

                                    if (combinedData.Boll != null)
                                    {
                                        float uy = dy + (float)combinedData.Boll[BOLL.UPPER];
                                        float my = dy + (float)combinedData.Boll[BOLL.MID];
                                        float ly = dy + (float)combinedData.Boll[BOLL.LOWER];
                                        g.DrawEllipse(new Pen(Color.Cyan), x, uy, yts * itemWidth / 2f, yts * itemWidth / 2f);
                                        g.DrawEllipse(new Pen(Color.Cyan), x, my, yts * itemWidth / 2f, yts * itemWidth / 2f);
                                        g.DrawEllipse(new Pen(Color.Cyan), x, ly, yts * itemWidth / 2f, yts * itemWidth / 2f);
                                    }
                                }

                                // Harry  --- Draw Signal
                                //_dataBars[i].RefreshExValues();


                                float actualImageHeight = _imageDown.Height / Math.Abs(g.DrawingSpaceTransform.Elements[3]);
                                float yToXScaling = Math.Abs(g.DrawingSpaceTransform.Elements[0] / g.DrawingSpaceTransform.Elements[3]);
                                PointF upImageDrawingPoint = drawingPoint;
                                PointF downImageDrawingPoint = drawingPoint;
                                Order order = new Order();
                                BarData orderBarData = _dataBars[i];
                                float lastBarX = (itemMargin + itemWidth) * _dataBars.Count;

                                if (_dataBars[i].SignalList != null && _dataBars[i].SignalList.Count > 0)
                                {
                                    foreach (CandleSignal cs in _dataBars[i].SignalList)
                                    {
                                        cs.BarData = orderBarData;
                                        if (cs.Code == 1)
                                            DrawSignal(g, ref downImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, cs, lastBarX, false);

                                        if (cs.Code == -1)
                                            DrawSignal(g, ref upImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, cs, lastBarX, false);

                                        if (cs.Code == 2)
                                            DrawGainTip(g, ref upImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, cs, lastBarX, false);

                                        if (cs.Code == -2)
                                            DrawGainTip(g, ref downImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, cs, lastBarX, false);

                                        if (cs.Code == 3)
                                            DrawStopLoss(g, ref downImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, cs, lastBarX, false);

                                        if (cs.Code == -3)
                                            DrawStopLoss(g, ref upImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, cs, lastBarX, false);

                                        if (cs.Code == 4)
                                            DrawStopGain(g, ref upImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, cs, lastBarX, false);

                                        if (cs.Code == -4)
                                            DrawStopGain(g, ref downImageDrawingPoint, order, itemWidth, itemMargin, yToXScaling, cs, lastBarX, false);

                                    }
                                }
                            }
                            drawingPoint.X = (i + 1) * (itemMargin + itemWidth);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Enter locked.
        /// </summary>
        void DrawBar(GraphicsWrapper g, ref PointF startingPoint, BarData barData, float itemWidth, float itemMargin, int itemUnitification)
        {
            float xMiddle = startingPoint.X + itemWidth / 2;
            float xHalfWidth = itemWidth / 2;

            Pen pen = _risingBarPen;
            if (barData.BarIsRising == false)
            {
                pen = _fallingBarPen;
            }

            if (pen == null)
            {
                return;
            }

            float yDisplacement = startingPoint.Y;

            g.DrawLine(pen, xMiddle, yDisplacement + (float)barData.Low, xMiddle, yDisplacement + (float)barData.High);
            g.DrawLine(pen, xMiddle, yDisplacement + (float)barData.Open, xMiddle - xHalfWidth, yDisplacement + (float)barData.Open);
            g.DrawLine(pen, xMiddle, yDisplacement + (float)barData.Close, xMiddle + xHalfWidth, yDisplacement + (float)barData.Close);
        }

        /// <summary>
        /// Enter locked.
        /// </summary>
        void DrawCandleStick(GraphicsWrapper g, ref PointF startingPoint, BarData barData, float itemWidth, float itemMargin, int itemUnitification)
        {
            if (barData.BarIsRising || (barData.IsHigherPrev && barData.BarBodyLength == 0))
            {
                if (_risingBarFill != null)
                {
                    g.FillRectangle(_risingBarFill, startingPoint.X, startingPoint.Y + (float)barData.Open, itemWidth, (float)barData.BarBodyLength);
                }

                if (_risingBarPen != null)
                {
                    if (itemWidth > 4)
                    {
                        g.DrawRectangle(_risingBarPen, startingPoint.X, startingPoint.Y + (float)barData.Open, itemWidth, (float)barData.BarBodyLength);
                        // Harry -- Draw a line for open = = close
                        if ((float)barData.BarBodyLength <= 0)
                        {
                            g.DrawLine(_risingBarPen,
                             startingPoint.X,
                             startingPoint.Y + (float)barData.Close,
                             startingPoint.X + itemWidth,
                             startingPoint.Y + (float)barData.Close);
                        }
                    }
                    else
                    {
                        g.FillRectangle(Brushes.Green, startingPoint.X, startingPoint.Y + (float)barData.Open, itemWidth, (float)barData.BarBodyLength);
                    }

                    // Lower shadow
                    g.DrawLine(_risingBarPen,
                        startingPoint.X + itemWidth / 2,
                        startingPoint.Y + (float)barData.Low,
                        startingPoint.X + itemWidth / 2,
                        startingPoint.Y + (float)barData.Open);

                    // Upper shadow
                    g.DrawLine(_risingBarPen,
                        startingPoint.X + itemWidth / 2,
                        (float)(startingPoint.Y + barData.High),
                        startingPoint.X + itemWidth / 2,
                        (float)(startingPoint.Y + barData.Close));
                    //Console.WriteLine("  startingPoint.X    " + startingPoint.X + "   startingPoint.Y " + startingPoint.Y + "  (float)barData.Low " + (float)barData.Low + "  (float)barData.Close  " + (float)barData.Close + "  (float)barData.High " + (float)barData.High + "  (float)barData.Open  " + (float)barData.Open);
                }
            }
            else
            {
                if (_fallingBarFill != null)
                {
                    g.FillRectangle(_fallingBarFill, startingPoint.X, startingPoint.Y + (float)barData.Close, itemWidth, (float)barData.BarBodyLength);
                    // Harry -- Draw a line for open = = close
                    if ((float)barData.BarBodyLength <= 0)
                    {
                        g.DrawLine(_fallingBarPen,
                         startingPoint.X,
                         startingPoint.Y + (float)barData.Close,
                         startingPoint.X + itemWidth,
                         startingPoint.Y + (float)barData.Close);
                    }

                }

                if (_fallingBarPen != null)
                {
                    if (itemWidth >= 4)
                    {// Only if an item is clearly visible, show the border, otherwise, hide to improver overal visibility.
                        // Showing this border adds nice detail on close zooming, but not useful otherwise.
                        g.DrawRectangle(_fallingBarPen, startingPoint.X, startingPoint.Y + (float)barData.Close, itemWidth, (float)barData.BarBodyLength);
                    }

                    // Lower shadow
                    g.DrawLine(_fallingBarPen,
                        startingPoint.X + itemWidth / 2,
                        startingPoint.Y + (float)barData.Low,
                        startingPoint.X + itemWidth / 2,
                        startingPoint.Y + (float)barData.Close);

                    // Upper shadow
                    g.DrawLine(_fallingBarPen,
                        startingPoint.X + itemWidth / 2,
                        (float)(startingPoint.Y + barData.High),
                        startingPoint.X + itemWidth / 2,
                        (float)(startingPoint.Y + barData.Open));
                }
            }
        }

        public override void SetSelectedChartType(string chartType)
        {
            _chartType = (ChartTypeEnum)Enum.Parse(typeof(ChartTypeEnum), chartType);
        }


        Image _imageUp;
        Image _imageDown;
        Image _imageCross;
        Image _imageStopLoss;
        Image _imageStopGain;
        Image _imageGainTip;
        Pen _buyDashedPen = new Pen(Color.Green);
        Pen _sellDashedPen = new Pen(Color.Red);

        //void DrawSignal(GraphicsWrapper g, ref PointF updatedImageDrawingPoint, Order order, float itemWidth, float itemMargin,
        //    float yToXScaling, BarData orderBarData, float lastBarX, bool drawOpening)
        void DrawSignal(GraphicsWrapper g, ref PointF updatedImageDrawingPoint, Order order, float itemWidth, float itemMargin,
float yToXScaling, CandleSignal cs, float lastBarX, bool drawOpening)
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

            //if (drawOpening == false)
            //{
            if (cs.Arrow == -1)
                image = _imageDown;
            else
            {
                image = _imageUp;
            }

            float imageHeight = 4 * (yToXScaling * itemWidth);

            if (cs.Arrow == -1)
            {
                g.DrawImage(image, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y +
                  (float)cs.BarData.High + (yToXScaling * itemWidth) + imageHeight, itemWidth, -imageHeight);
                updatedImageDrawingPoint.Y += 1.1f * imageHeight;
            }
            else
            {
                g.DrawImage(image, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y +
                      (float)cs.BarData.Low - (yToXScaling * itemWidth), itemWidth, -imageHeight);
                updatedImageDrawingPoint.Y -= 1.1f * imageHeight;
            }
        }

        void DrawGainTip(GraphicsWrapper g, ref PointF updatedImageDrawingPoint, Order order, float itemWidth, float itemMargin,
           float yToXScaling, CandleSignal cs, float lastBarX, bool drawOpening)
        {
            Image image = _imageUp;
            Brush brush = Brushes.Green;
            Pen dashedPen = _buyDashedPen;
            Pen pen = Pens.GreenYellow;

            image = _imageGainTip;

            float imageHeight = 2 * (yToXScaling * itemWidth);

            // Gain tip should occur on the opposite side of arrow
            if (cs.Arrow == 1)
            {
                g.DrawImage(image, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y +
                  (float)cs.BarData.High + (yToXScaling * itemWidth) + imageHeight, itemWidth, -imageHeight);
                updatedImageDrawingPoint.Y += 1.1f * imageHeight;
            }
            else
            {
                g.DrawImage(image, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y +
                      (float)cs.BarData.Low - (yToXScaling * itemWidth), itemWidth, -imageHeight);
                updatedImageDrawingPoint.Y -= 1.1f * imageHeight;
            }
        }

        void DrawStopLoss(GraphicsWrapper g, ref PointF updatedImageDrawingPoint, Order order, float itemWidth, float itemMargin,
            float yToXScaling, CandleSignal cs, float lastBarX, bool drawOpening)
        {
            Image image = _imageUp;
            Brush brush = Brushes.Green;
            Pen dashedPen = _buyDashedPen;
            Pen pen = Pens.GreenYellow;

            image = _imageStopLoss;

            float imageHeight = 2 * (yToXScaling * itemWidth);

            if (cs.Arrow == -1)
            {
                g.DrawImage(image, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y +
                  (float)cs.BarData.High + (yToXScaling * itemWidth) + imageHeight, itemWidth, -imageHeight);
                updatedImageDrawingPoint.Y += 1.1f * imageHeight;
            }
            else
            {
                g.DrawImage(image, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y +
                      (float)cs.BarData.Low - (yToXScaling * itemWidth), itemWidth, -imageHeight);
                updatedImageDrawingPoint.Y -= 1.1f * imageHeight;
            }
        }


        void DrawStopGain(GraphicsWrapper g, ref PointF updatedImageDrawingPoint, Order order, float itemWidth, float itemMargin,
            float yToXScaling, CandleSignal cs, float lastBarX, bool drawOpening)
        {
            Image image = _imageUp;
            Brush brush = Brushes.Green;
            Pen dashedPen = _buyDashedPen;
            Pen pen = Pens.GreenYellow;

            image = _imageStopGain;

            //itemWidth += itemMargin;
            float imageHeight = 2 * (yToXScaling * itemWidth);
            //float imageHeight = (yToXScaling * itemWidth);
            //imageHeight =itemWidth;
            Brush brushx = Brushes.Yellow;
            // Gain tip should occur on the opposite side of arrow
            if (cs.Arrow == 1)
            {
                g.DrawImage(image, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y +
                  (float)cs.BarData.High + (yToXScaling * itemWidth) + imageHeight, itemWidth, -imageHeight);
                updatedImageDrawingPoint.Y += 1.1f * imageHeight;
            }
            else
            {
                g.DrawImage(image, updatedImageDrawingPoint.X, updatedImageDrawingPoint.Y +
                       (float)cs.BarData.Low - (yToXScaling * itemWidth), itemWidth, -imageHeight);
                updatedImageDrawingPoint.Y -= 1.1f * imageHeight;
            }
        }
    }
}
