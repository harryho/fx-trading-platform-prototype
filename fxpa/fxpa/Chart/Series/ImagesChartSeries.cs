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
    class ImagesChartSeries : SimpleChartSeries
    {
        PointF _imagesDisplacement = new PointF();
        /// <summary>
        /// What is the default displacement for images - allowing to render them besides/above etc. the graphics.
        /// </summary>
        public PointF ImagesDisplacement
        {
            get
            {
                lock (this)
                {
                    return _imagesDisplacement;
                }
            }
            set
            {
                lock (this)
                {
                    _imagesDisplacement = value;
                }
            }
        }

        Dictionary<float, Image> _images = new Dictionary<float, Image>();
        public Dictionary<float, Image> Images
        {
            get
            {
                lock (this)
                {
                    return _images;
                }
            }
        }

        public override void Draw(GraphicsWrapper g, int unitsUnification, RectangleF clippingRectangle, float itemWidth, float itemMargin)
        {
            PointF drawingPoint = new PointF();

            lock (this)
            {
                Image image;
                foreach (float[] valueSet in ValueSets)
                {
                    foreach (float value in valueSet)
                    {// Images mode does not apply unit unification
                        if (double.IsNaN(value) == false && drawingPoint.X >= clippingRectangle.X && drawingPoint.X <= clippingRectangle.X + clippingRectangle.Width
                            && _images.ContainsKey((int)value))
                        {
                            image = _images[(int)value];
                            g.DrawImage(image, drawingPoint.X + _imagesDisplacement.X - image.Width / 2, drawingPoint.Y + _imagesDisplacement.Y + (float)value);
                        }

                        drawingPoint.X += itemMargin + itemWidth;
                    }
                }
            }
        }

    }
}
