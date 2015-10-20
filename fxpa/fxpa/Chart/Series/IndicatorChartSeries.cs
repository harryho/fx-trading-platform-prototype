// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;



namespace fxpa
{
    /// <summary>
    /// This class brings together the DataProvider and the ChartSeries, allowing the data to be rendered.
    /// </summary>
    public class IndicatorChartSeries : SimpleChartSeries
    {
        BasicIndicator _indicator;

        public BasicIndicator Indicator
        {
            get { return _indicator; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IndicatorChartSeries(string name, BasicIndicator indicator)
        {
            Name = name;

            Initialize(indicator);
        }

        protected void Initialize(BasicIndicator indicator)
        {
            _indicator = indicator;
            _indicator.IndicatorCalculatedEvent += new Indicator.IndicatorCalculatedDelegate(_indicator_IndicatorCalculatedEvent);
            _indicator.UI.IndicatorUIUpdatedEvent += new BasicIndicatorUI.IndicatorUIUpdatedDelegate(UI_IndicatorUIUpdatedEvent);
            _indicator.ics = this;
        }

        void UI_IndicatorUIUpdatedEvent(BasicIndicatorUI ui)
        {
            UpdateValues();
        }

        protected override void OnAddedToChart()
        {
            UpdateValues();
        }

        protected override void OnRemovedFromChart()
        {
            if (_indicator != null)
            {
                _indicator.IndicatorCalculatedEvent -= new Indicator.IndicatorCalculatedDelegate(_indicator_IndicatorCalculatedEvent);
                _indicator = null;
            }
        }

      public   void _indicator_IndicatorCalculatedEvent(Indicator indicator)
        {
            UpdateValues();
        }

        void UpdateValues()
        {
            base.ClearValues();
            foreach (IndicatorResultSet resultSet in _indicator.Results.ResultSets)
            {
                Console.WriteLine(" n=> " + resultSet.Name + " v => " + resultSet.Values);
                AddValueSet(GeneralHelper.DoublesToFloats(resultSet.Values), _indicator.UI.OutputResultSetsPens[resultSet.Name]);
            }

            Console.WriteLine("  chart pane "+this.GetType());
            //if (charPane != null)
            //{
            //    Console.WriteLine("  chart pane ");
            //    ChartPaneUpdateDelegate d = new ChartPaneUpdateDelegate(charPane.series_SeriesUpdatedEvent);
            //    d.Invoke(this, true);
            //}
            //else
            //{
                base.RaiseValuesUpdated(true);
            //}
        }

        delegate void ChartPaneUpdateDelegate(ChartSeries series, bool isUpdate);

        public ChartPane charPane = null;


        public override void SaveToFile(string fileName)
        {
        }

        public override void Draw(GraphicsWrapper g, int unitsUnification, System.Drawing.RectangleF clippingRectangle, float itemWidth, float itemMargin)
        {
            base.Draw(g, unitsUnification, clippingRectangle, itemWidth, itemMargin);
        }

    }
}
