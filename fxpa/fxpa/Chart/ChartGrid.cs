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
    public class ChartGrid
    {
        //float? _horizontalLineSpacing = 100;
        float? _horizontalLineSpacing = 100;

        public float? HorizontalLineSpacing
        {
            get { return _horizontalLineSpacing; }
            set 
            {
                if (value <= 0)
                {
                    // SystemMonitor.Warning("Invalid value fed to the Grid horizontal spacing.");
                    _horizontalLineSpacing = null;
                }
                else
                {
                    _horizontalLineSpacing = value;
                }
            }
        }

        //float? _verticalLineSpacing = 20;
        float? _verticalLineSpacing = 10;
        public float? VerticalLineSpacing
        {
            get { return _verticalLineSpacing; }
            set {
                if (value <= 0)
                {
                    // SystemMonitor.Warning("Invalid value fed to the Grid vertical spacing.");
                    _verticalLineSpacing = null;
                }
                else
                {
                    _verticalLineSpacing = value;
                }
            }
        }

        bool _considerScale = true;
        
        /// <summary>
        /// Is the grid considering scale, and not drawing too much of itself.
        /// </summary>
        public bool ConsiderScale
        {
            get { return _considerScale; }
            set { _considerScale = value; }
        }

        Pen _pen = Pens.DimGray;
        public Pen Pen
        {
            set { _pen = (Pen)value.Clone(); }
        }

        bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ChartGrid()
        {
        }

        public void Draw(GraphicsWrapper g, RectangleF clipping, RectangleF space, float totalItemWidth)
        {
            if (Visible == false)
            {
                return;
            }
            _pen.DashStyle = DashStyle.Dash;

            Console.WriteLine(" space ************************************************** " + space);
            Console.WriteLine(" clipping ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ " + clipping);
            if (VerticalLineSpacing.HasValue)
            {
                float actualSpacing = _verticalLineSpacing.Value * totalItemWidth;
                if (ConsiderScale)
                {
                    int xScaling = Math.Abs((int) (1 / g.DrawingSpaceTransform.Elements[0]));
                    if (xScaling > 1)
                    {
                        actualSpacing = actualSpacing * xScaling;
                    }
                }

                // Set starting to the closes compatible position.
                float starting = (int)(space.Left / actualSpacing);
                starting = starting * actualSpacing;

                for (float x = starting; x <= space.Right; x += actualSpacing)
                {// Vertical lines.
                    if (x >= clipping.X && x <= clipping.X + clipping.Width + actualSpacing)
                    {
                        g.DrawLine(_pen, x, Math.Max(space.Top, clipping.Top), x, Math.Min(space.Bottom, clipping.Bottom));
                    }
                }
            }

            if (HorizontalLineSpacing.HasValue)
            {
                float actualSpacing = _horizontalLineSpacing.Value;
                //Console.WriteLine(" actualSpacing ************************************************** " + actualSpacing);
                if (ConsiderScale)
                {
                    int yScaling = Math.Abs((int) (1 / g.DrawingSpaceTransform.Elements[3]));
                    Console.WriteLine(" yScaling ************************************************** " + yScaling);
                    if (yScaling > 1)
                    {
                        actualSpacing = actualSpacing * yScaling;
                        Console.WriteLine(" actualSpacing ************************************************** " + actualSpacing);
                    }
                }


                // Set starting to the closes compatible position.
                float starting = (int)(space.Top / actualSpacing);
                starting = starting * actualSpacing;
                //actualSpacing = actualSpacing * 0.5f;
                for (float y = starting; y <= space.Bottom; y += actualSpacing)
                {// Horizontal lines.
                    if (y >= clipping.Y && y <= clipping.Y + clipping.Height)
                    {
                        g.DrawLine(_pen, Math.Min(space.Left, clipping.Left), y, Math.Max(space.Right, clipping.Right), y);
                        //g.DrawLine(_pen, Math.Max(space.Left, clipping.Left), y, Math.Min(space.Right, clipping.Right), y);
                    }
                }
            }
        }
    }
}
