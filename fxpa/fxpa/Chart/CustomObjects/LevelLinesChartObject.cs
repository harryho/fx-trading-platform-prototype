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
    public class LevelLinesObject : CustomObject
    {
        List<double> _levels = new List<double>();
        public List<double> Levels
        {
            get { return _levels; }
        }

        Pen _pen = Pens.DarkGray;
        public Pen Pen
        {
            set { _pen = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public LevelLinesObject()
            : base()
        {
            this.DrawingOrder = DrawingOrderEnum.PreSeries;
        }

        public override void Draw(GraphicsWrapper g, PointF? mousePosition, RectangleF clippingRectangle, RectangleF drawingSpace)
        {
            if (Visible == false)
            {
                return;
            }

            foreach (double level in _levels)
            {
                if (level >= drawingSpace.Y && level <= drawingSpace.Y + drawingSpace.Height)
                {
                    g.DrawLine(_pen, drawingSpace.X, (float)level, drawingSpace.X + drawingSpace.Width, (float)level);
                }
            }
        }
    }
}
