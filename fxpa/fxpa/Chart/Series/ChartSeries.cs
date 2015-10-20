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

using System.Drawing.Drawing2D;

namespace fxpa
{
    /// <summary>
    /// Note - all the Brush and Pens do not have Get accessors on purpose, since defaults can not be modified. 
    /// So to change them directly use the "set".
    /// Thread safe.
    /// </summary>
    public abstract class ChartSeries : IPropertyContainer
    {
        public enum SeriesTypeEnum
        {
            IndexBased,
            TimeBased // Time based series must consider time gaps.
        }

        public abstract SeriesTypeEnum SeriesType { get; }

        volatile string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        volatile bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public abstract string[] ChartTypes { get; }

        public abstract string SelectedChartType { get; }

        public abstract int MaximumIndex { get; }

        public delegate void SeriesUpdatedDelegate(ChartSeries series, bool updateUI);
        public event SeriesUpdatedDelegate SeriesUpdatedEvent;

        /// <summary>
        /// 
        /// </summary>
        public ChartSeries()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public ChartSeries(string name)
        {
            Name = name;
        }

        public void AddedToChart()
        {
            OnAddedToChart();
        }

        protected virtual void OnAddedToChart()
        {
        }

        public void RemovedFromChart()
        {
            OnRemovedFromChart();
        }

        protected virtual void OnRemovedFromChart()
        {
        }

        public abstract void SetSelectedChartType(string chartType);
        public abstract void SaveToFile(string fileName);

        /// <summary>
        /// This is only valid for time based series.
        /// </summary>
        public abstract DateTime GetTimeAtIndex(int index);


        public abstract double GetOpenAtIndex(int index);
        public abstract double GetCloseAtIndex(int index);
        public abstract double GetLowAtIndex(int index);
        public abstract double GetHightAtIndex(int index);
        public abstract double GetExLowAtIndex(int index);
        public abstract double GetExHightAtIndex(int index);
        public abstract double GetSarAtIndex(int index);
        public abstract double GetCrAtIndex(int index);
        public abstract double GetRsiAtIndex(int index);
        public abstract double GetArAtIndex(int index);
        public abstract double GetAr2AtIndex(int index);
        public abstract double GetBrAtIndex(int index);
        public abstract double GetRsi2AtIndex(int index);
        public abstract double GetRsi3AtIndex(int index);
        public abstract double GetRsi4AtIndex(int index);
        public abstract double GetCciAtIndex(int index);
        public abstract double GetCci2AtIndex(int index);
        public abstract double GetCci3AtIndex(int index);
        public abstract double GetWrAtIndex(int index);
        public abstract double GetWr2AtIndex(int index);
        public abstract Dictionary<string,double> GetLwrAtIndex(int index);
        public abstract Dictionary<string, double> GetBollAtIndex(int index);
        public abstract Dictionary<string, double> GetIndicatorsAtIndex(int index);

        public abstract List<CandleSignal> GetSignalsAtIndex(int index);
        public abstract BarData GetCandleAtIndex(int index);
        /// <summary>
        /// Establish the total minimum value of any item in this interval.
        /// </summary>
        /// <param name="startIndex">Inclusive starting index.</param>
        /// <param name="endIndex">Exclusive ending index.</param>
        public abstract float GetTotalMinimum(int startIndex, int endIndex);

        /// <summary>
        /// Establish the total maximum value of any item in this interval.
        /// </summary>
        /// <param name="startIndex">Inclusive starting index.</param>
        /// <param name="endIndex">Exclusive ending index.</param>
        public abstract float GetTotalMaximum(int startIndex, int endIndex);

        protected void RaiseValuesUpdated(bool updateUI)
        {
            if (SeriesUpdatedEvent != null)
            {
                SeriesUpdatedEvent(this, updateUI);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract float CalculateTotalWidth(float itemWidth, float itemMargin);

        /// <summary>
        /// 
        /// </summary>
        public abstract void DrawSeriesIcon(GraphicsWrapper g, Rectangle rectangle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="unitsUnification">Draw a few units as one. Used when zooming is too big to show each unit - so unify them. 1 means no unification, 10 means unify 10 units together</param>
        /// <param name="clippingRectangle"></param>
        /// <param name="itemWidth"></param>
        /// <param name="itemMargin"></param>
        public abstract void Draw(GraphicsWrapper g, int unitsUnification, RectangleF clippingRectangle, float itemWidth, float itemMargin);


        /// <summary>
        /// Indeces returned are not checked for maximum value, so they may be beyond the available data.
        /// </summary>
        protected void GetDrawingRangeIndecesFromClippingRectange(RectangleF clippingRectangle, PointF baseDrawingPoint, 
            int unitsUnification, out int startingIndex, out int endingIndex, float itemWidth, float itemMargin)
        {
            float xStartLocation = Math.Max(0, baseDrawingPoint.X + clippingRectangle.X);
            float xEndLocation = baseDrawingPoint.X + clippingRectangle.X + clippingRectangle.Width;

            startingIndex = (int)(xStartLocation / (itemWidth + itemMargin));
            // Make it round to unitsUnification, round downwards.
            startingIndex = startingIndex / unitsUnification;
            startingIndex = startingIndex * unitsUnification;

            endingIndex = (int)(xEndLocation / (itemWidth + itemMargin));
            // Make it round to unitsUnification, but round upwards.
            int leftOver = endingIndex % unitsUnification;
            endingIndex = endingIndex / unitsUnification;
            endingIndex = endingIndex * unitsUnification;
            if (leftOver > 0)
            {
                endingIndex += unitsUnification;
            }
        }

        protected void DrawLine(GraphicsWrapper g, Pen linePen, float[] values, int unitsUnification, RectangleF clippingRectangle, float itemWidth, float itemMargin)
        {
            PointF drawingPoint = new PointF();

            int startIndex, endIndex;
            GetDrawingRangeIndecesFromClippingRectange(clippingRectangle, drawingPoint, unitsUnification, out startIndex, out endIndex, itemWidth, itemMargin);

            for (int i = startIndex + unitsUnification; i < endIndex && i < values.Length + unitsUnification - 1; i += unitsUnification)
            {
                int actualIndex = i;
                int actualPreviousIndex = i - unitsUnification;

                if (actualIndex >= values.Length)
                {
                    actualIndex = values.Length - 1;
                }

                float previousValue = (float)values[actualPreviousIndex];
                float value = (float)values[actualIndex];

                if (float.IsNaN(previousValue) == false && float.IsNaN(value) == false)
                {
                    g.DrawLine(linePen, drawingPoint.X, drawingPoint.Y + previousValue, drawingPoint.X + itemMargin + itemWidth, drawingPoint.Y + value);
                }

                drawingPoint.X = i * (itemMargin + itemWidth);
            }
        }

        protected void DrawColoredArea(GraphicsWrapper g, Pen pen, Brush fill, float[] values, int unitsUnification, RectangleF clippingRectangle, float itemWidth, float itemMargin)
        {
            PointF drawingPoint = new PointF();

            int startIndex, endIndex;
            GetDrawingRangeIndecesFromClippingRectange(clippingRectangle, drawingPoint, unitsUnification, out startIndex, out endIndex, itemWidth, itemMargin);

            for (int i = startIndex + unitsUnification; i < endIndex && i < values.Length + unitsUnification - 1; i += unitsUnification)
            {
                int actualIndex = i;
                int actualPreviousIndex = i - unitsUnification;

                if (actualIndex >= values.Length)
                {
                    actualIndex = values.Length - 1;
                }

                DrawColorAreaItem(g, ref drawingPoint, values, pen, fill, actualIndex, 
                    actualPreviousIndex, itemWidth, itemMargin);

                drawingPoint.X = i * (itemMargin + itemWidth);
            }
        }

        protected void DrawColorAreaItem(GraphicsWrapper g, ref PointF drawingPoint, float[] values, Pen pen, Brush fill, 
            int index, int previousItemIndex, float itemWidth, float itemMargin)
        {
            float indexValue = values[index];
            float previousItemIndexValue = values[previousItemIndex];

            int unificationCount = index - previousItemIndex;

            for (int i = previousItemIndex; i <= index; i++)
            {
                if (float.IsNaN(previousItemIndexValue))
                {
                    previousItemIndexValue = values[i];
                } else if (float.IsNaN(indexValue))
                {
                    indexValue = values[i];
                }
            }

            if (float.IsNaN(indexValue) || float.IsNaN(previousItemIndexValue))
            {// Failed to find reasonable values to draw.
                return;
            }

            if (fill != null)
            {
                g.FillPolygon(fill, new PointF[] { 
                    drawingPoint, 
                    new PointF(drawingPoint.X + (itemMargin + itemWidth) * unificationCount, drawingPoint.Y), 
                    new PointF(drawingPoint.X + (itemMargin + itemWidth) * unificationCount, drawingPoint.Y + indexValue), 
                    new PointF(drawingPoint.X, drawingPoint.Y + previousItemIndexValue) });
            }

            if (pen != null)
            {
                g.DrawLine(pen, drawingPoint.X, drawingPoint.Y + previousItemIndexValue,
                    drawingPoint.X + (itemMargin + itemWidth) * unificationCount, drawingPoint.Y + indexValue);
            }

        }

        protected void DrawHistogramBars(GraphicsWrapper g, Pen pen, Brush fill, float[] values, int unitsUnification, RectangleF clippingRectangle, float itemWidth, float itemMargin)
        {
            PointF drawingPoint = new PointF();

            int startIndex, endIndex;
            GetDrawingRangeIndecesFromClippingRectange(clippingRectangle, drawingPoint, unitsUnification, out startIndex, out endIndex, itemWidth, itemMargin);

            for (int i = startIndex + unitsUnification; i < endIndex && i < values.Length + unitsUnification - 1; i += unitsUnification)
            {
                int actualIndex = i;
                int actualPreviousIndex = i - unitsUnification;

                if (actualIndex >= values.Length)
                {
                    actualIndex = values.Length - 1;
                }

                DrawHistogramBar(g, ref drawingPoint, values, pen, fill, actualIndex, actualPreviousIndex, itemWidth, itemMargin);

                drawingPoint.X = i * (itemMargin + itemWidth);
            }
        }

        void DrawHistogramBar(GraphicsWrapper g, ref PointF drawingPoint, float[] values, Pen pen, Brush fill, 
            int index, int previousItemIndex, float itemWidth, float itemMargin)
        {
            float y = drawingPoint.Y;
            
            double heightSum = 0;
            int actualSumCount = 0;

            int unificationCount = index - previousItemIndex;

            for (int i = previousItemIndex; i <= index; i++)
            {
                if (float.IsNaN(values[i]) 
                    || float.IsInfinity(values[i]))
                {
                    continue;
                }

                heightSum += values[i];
                actualSumCount++;
            }

            if (actualSumCount == 0)
            {
                return;
            }

            float height = (float)(heightSum / actualSumCount);

            if (height < 0)
            {
                y += height;
                height = -height;
            }

            if (fill != null)
            {
                g.FillRectangle(fill, drawingPoint.X, y, (itemWidth) * unificationCount, height);
            }

            if (pen != null)
            {
                g.DrawRectangle(pen, drawingPoint.X, y, itemWidth, height);
            }
        }

        protected void DrawIcon(GraphicsWrapper g, Pen pen, Brush fill, Rectangle rectangle)
        {
            if (Visible == false)
            {
                return;
            }

            if (fill != null)
            {
                g.FillRectangle(fill, rectangle);
            }

            if (pen != null)
            {
                g.DrawRectangle(pen, rectangle);
            }
        }


        #region IDynamicPropertyContainer Members

        public string[] GetPropertiesNames()
        {
            return new string[] { };
        }

        public object GetPropertyValue(string name)
        {
            return null;
        }

        public Type GetPropertyType(string name)
        {
            return null;
        }

        public bool SetPropertyValue(string name, object value)
        {
            return false;
        }

        public void PropertyChanged()
        {
            RaiseValuesUpdated(true);
        }

        #endregion
    }
}
