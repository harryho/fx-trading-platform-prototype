// -----
// GNU General Public License
// The Open Forex Platform is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Open Forex Platform is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;



namespace ZSFXFrontEnd
{
    /// <summary>
    /// This class brings together the DataProvider and the ChartSeries, allowing the data to be rendered.
    /// </summary>
    public class IndicatorChartSeries : SimpleChartSeries
    {
        PlatformIndicator _indicator;

        public PlatformIndicator Indicator
        {
            get { return _indicator; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IndicatorChartSeries(string name, PlatformIndicator indicator)
        {
            Name = name;

            Initialize(indicator);
        }

        protected void Initialize(PlatformIndicator indicator)
        {
            _indicator = indicator;
            _indicator.IndicatorCalculatedEvent += new Indicator.IndicatorCalculatedDelegate(_indicator_IndicatorCalculatedEvent);
            _indicator.UI.IndicatorUIUpdatedEvent += new PlatformIndicatorUI.IndicatorUIUpdatedDelegate(UI_IndicatorUIUpdatedEvent);
        }

        void UI_IndicatorUIUpdatedEvent(PlatformIndicatorUI ui)
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

        void _indicator_IndicatorCalculatedEvent(Indicator indicator)
        {
            UpdateValues();
        }

        void UpdateValues()
        {
            base.ClearValues();
            foreach(IndicatorResultSet resultSet in _indicator.Results.ResultSets)
            {
                AddValueSet(GeneralHelper.DoublesToFloats(resultSet.Values), _indicator.UI.OutputResultSetsPens[resultSet.Name]);
            }

            base.RaiseValuesUpdated(true);
        }

        public override void SaveToFile(string fileName)
        {
        }

        public override void Draw(GraphicsWrapper g, int unitsUnification, System.Drawing.RectangleF clippingRectangle, float itemWidth, float itemMargin)
        {
            base.Draw(g, unitsUnification, clippingRectangle, itemWidth, itemMargin);
        }

    }
}
