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

namespace fxpa
{
    public class ChartSeriesColorSelector
    {
        int _seriesIndex = 0;
        public int SeriesIndex
        {
            get { return _seriesIndex; }
            set { _seriesIndex = value; }
        }

        public ChartSeriesColorSelector()
        {
        }

        public void SetupSeries(ChartSeries inputSeries)
        {
            if (inputSeries is TradeChartSeries)
            {
                TradeChartSeries series = (TradeChartSeries)inputSeries;
                switch (_seriesIndex)
                {
                    case 0:
                        {
                            series.DefaultPen = Pens.DarkSeaGreen;
                            series.RisingBarPen = Pens.DarkSeaGreen;
                            series.RisingBarFill = null;
                            series.FallingBarPen = Pens.IndianRed;
                            series.FallingBarFill = Brushes.IndianRed;
                        }
                        break;
                    case 1:
                        {
                            series.DefaultPen = Pens.LightBlue;
                            series.RisingBarPen = Pens.LightBlue;
                            series.RisingBarFill = null;
                            series.FallingBarPen = Pens.LightBlue;
                            series.FallingBarFill = Brushes.LightBlue;
                        }
                        break;
                    case 2:
                        {
                            series.DefaultPen = Pens.LightSlateGray;
                            series.RisingBarPen = Pens.LightSlateGray;
                            series.RisingBarFill = null;
                            series.FallingBarPen = Pens.LightSlateGray;
                            series.FallingBarFill = Brushes.LightSlateGray;
                        }
                        break;
                    case 3:
                        {
                            series.DefaultPen = Pens.Lavender;
                            series.RisingBarPen = Pens.Lavender;
                            series.RisingBarFill = null;
                            series.FallingBarPen = Pens.Lavender;
                            series.FallingBarFill = Brushes.Lavender;
                        }
                        break;
                    case 4:
                        {
                            series.DefaultPen = Pens.OrangeRed;
                            series.RisingBarPen = Pens.OrangeRed;
                            series.RisingBarFill = null;
                            series.FallingBarPen = Pens.OrangeRed;
                            series.FallingBarFill = Brushes.OrangeRed;
                        }
                        break;
                    case 5:
                        {
                            series.DefaultPen = Pens.Gold;
                            series.RisingBarPen = Pens.Gold;
                            series.RisingBarFill = null;
                            series.FallingBarPen = Pens.Gold;
                            series.FallingBarFill = Brushes.Gold;
                        }
                        break;
                    case 6:
                        {
                            series.DefaultPen = Pens.GreenYellow;
                            series.RisingBarPen = Pens.GreenYellow;
                            series.RisingBarFill = null;
                            series.FallingBarPen = Pens.GreenYellow;
                            series.FallingBarFill = Brushes.GreenYellow;
                        }
                        break;
                    case 7:
                        {
                            series.DefaultPen = Pens.BlueViolet;
                            series.RisingBarPen = Pens.BlueViolet;
                            series.RisingBarFill = null;
                            series.FallingBarPen = Pens.BlueViolet;
                            series.FallingBarFill = Brushes.BlueViolet;
                        }
                        break;

                }
            }

            _seriesIndex++;
        }
    }
}
