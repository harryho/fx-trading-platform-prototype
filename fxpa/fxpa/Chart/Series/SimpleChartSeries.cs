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
using System.Collections.ObjectModel;

namespace fxpa
{
    /// <summary>
    /// Thread safe.
    /// </summary>
    public class SimpleChartSeries : ChartSeries
    {
        public enum ChartTypeEnum
        {
            Line,
            Histogram,
            ColoredArea
        }

        protected List<Pen> _valueSetsPens = new List<Pen>();

        List<float[]> _valueSets = new List<float[]>();
        public ReadOnlyCollection<float[]> ValueSets
        {
            get
            {
                lock (this)
                {
                    return _valueSets.AsReadOnly();
                }
            }
        }

        protected Pen _defaultPen = Pens.White;
        public Pen DefaultPen
        {
            set { _defaultPen = value; }
        }

        protected Brush _fill = Brushes.WhiteSmoke;
        public Brush Fill
        {
            set { _fill = value; }
        }

        volatile int _maximumIndex = 0;
        public override int MaximumIndex
        {
            get { return _maximumIndex; }
        }

        public override ChartSeries.SeriesTypeEnum SeriesType
        {
            get { return SeriesTypeEnum.IndexBased; }
        }

        volatile ChartTypeEnum _chartType = ChartTypeEnum.Line;
        public ChartTypeEnum ChartType
        {
            get { return _chartType; }
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

        /// <summary>
        /// 
        /// </summary>
        public SimpleChartSeries()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public SimpleChartSeries(string name)
            : base(name)
        {
        }

        public SimpleChartSeries(string name, ChartTypeEnum chartType, float[] values)
            : base(name)
        {
            _chartType = chartType;
            AddValueSet(values);
        }

        public void SetChartType(ChartTypeEnum type)
        {
            _chartType = type;
        }

        public void ClearValues()
        {
            lock (this)
            {
                _valueSets.Clear();
                _valueSetsPens.Clear();
            }

            _maximumIndex = 0;
        }

        public void AddValueSet(float[] valueSet)
        {
            AddValueSet(valueSet, null);
        }

        public void AddValueSet(float[] valueSet, Pen pen)
        {
            lock (this)
            {
                _valueSets.Add(valueSet);
                _valueSetsPens.Add(pen);
            }

            _maximumIndex = Math.Max(valueSet.Length, _maximumIndex);

            RaiseValuesUpdated(true);
        }

        public void SetValues(IEnumerable<float[]> valueSets)
        {
            _maximumIndex = 0;

            foreach (float[] valueSet in valueSets)
            {
                _maximumIndex = Math.Max(_maximumIndex, valueSet.Length);
            }

            lock (this)
            {
                _valueSets = new List<float[]>(valueSets);
                _valueSetsPens = new List<Pen>();
                for (int i = 0; i < _valueSets.Count; i++)
                {
                    _valueSetsPens.Add(null);
                }
            }
            this.RaiseValuesUpdated(true);
        }

        public override void SaveToFile(string fileName)
        {// TODO, save simple values to file.
        }

        public override DateTime GetTimeAtIndex(int index)
        {
            // SystemMonitor.NotImplementedCritical();
            return DateTime.MinValue;
        }

        public override float GetTotalMinimum(int startIndex, int endIndex)
        {
            float min = float.MaxValue;
            lock (this)
            {
                foreach (float[] valueSet in _valueSets)
                {
                    for (int i = startIndex; i < endIndex && i < valueSet.Length; i++)
                    {
                        if (double.IsNaN(valueSet[i]) == false)
                        {
                            min = (float)Math.Min(valueSet[i], min);
                        }
                    }
                }
            }
            return min;
        }

        public override float GetTotalMaximum(int startIndex, int endIndex)
        {
            float max = float.MinValue;
            lock (this)
            {
                foreach (float[] valueSet in _valueSets)
                {
                    for (int i = startIndex; i < endIndex && i < valueSet.Length; i++)
                    {
                        if (double.IsNaN(valueSet[i]) == false)
                        {
                            max = (float)Math.Max(valueSet[i], max);
                        }
                    }
                }
            }
            return max;
        }

        public override float CalculateTotalWidth(float itemWidth, float itemMargin)
        {
            return (itemWidth + itemMargin) * _maximumIndex;
        }

        public override void DrawSeriesIcon(GraphicsWrapper g, Rectangle rectangle)
        {
            base.DrawIcon(g, _defaultPen, _fill, rectangle); 
        }

        public override void Draw(GraphicsWrapper g, int unitsUnification, RectangleF clippingRectangle, float itemWidth, float itemMargin)
        {
            if (this.Visible == false)
            {
                return;
            }
            lock (this)
            {
                if (_chartType == ChartTypeEnum.Line)
                {
                    for (int i = 0; i < _valueSets.Count; i++)
                    {
                        if (_valueSetsPens[i] == null)
                        {
                            base.DrawLine(g, _defaultPen, _valueSets[i], unitsUnification, clippingRectangle, itemWidth, itemMargin);
                        }
                        else
                        {
                            base.DrawLine(g, _valueSetsPens[i], _valueSets[i], unitsUnification, clippingRectangle, itemWidth, itemMargin);
                        }
                    }
                }
                else if (_chartType == ChartTypeEnum.Histogram)
                {
                    for (int i = 0; i < _valueSets.Count; i++)
                    {
                        if (_valueSetsPens[i] == null)
                        {
                            base.DrawHistogramBars(g, _defaultPen, _fill, _valueSets[i], unitsUnification, clippingRectangle, itemWidth, itemMargin);
                        }
                        else
                        {
                            base.DrawHistogramBars(g, _valueSetsPens[i], _fill, _valueSets[i], unitsUnification, clippingRectangle, itemWidth, itemMargin);
                        }
                    }
                }
                else if (_chartType == ChartTypeEnum.ColoredArea)
                {
                    for (int i = 0; i < _valueSets.Count; i++)
                    {
                        if (_valueSetsPens[i] == null)
                        {
                            base.DrawColoredArea(g, _defaultPen, _fill, _valueSets[i], unitsUnification, clippingRectangle, itemWidth, itemMargin);
                        }
                        else
                        {
                            base.DrawColoredArea(g, _valueSetsPens[i], _fill, _valueSets[i], unitsUnification, clippingRectangle, itemWidth, itemMargin);
                        }
                    }
                }
            } // Lock
        }

        public override void SetSelectedChartType(string chartType)
        {
            _chartType = (ChartTypeEnum)Enum.Parse(typeof(ChartTypeEnum), chartType);
        }

        public override double GetOpenAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetCloseAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetLowAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetHightAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetSarAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetCrAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override List<CandleSignal> GetSignalsAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override BarData GetCandleAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetExLowAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetExHightAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetRsiAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetArAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetBrAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetRsi2AtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetRsi3AtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetRsi4AtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetCciAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetCci2AtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetWrAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, double> GetLwrAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, double> GetIndicatorsAtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetCci3AtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetWr2AtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override double GetAr2AtIndex(int index)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, double> GetBollAtIndex(int index)
        {
            throw new NotImplementedException();
        }
    }
}
