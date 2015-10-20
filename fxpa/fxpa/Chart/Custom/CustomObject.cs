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
    public abstract class CustomObject
    {
        public enum DrawingOrderEnum
        {
            PreSeries,
            PostSeries
        }

        DrawingOrderEnum _drawingOrder = DrawingOrderEnum.PreSeries;
        public DrawingOrderEnum DrawingOrder
        {
            get { return _drawingOrder; }
            set { _drawingOrder = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool IsInitialized
        {
            get { return _manager != null; }
        }

        CustomObjectsManager _manager;
        protected CustomObjectsManager Manager
        {
            get { return _manager; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CustomObject()
        {
        }

        public abstract void Draw(GraphicsWrapper g, PointF? mousePosition, RectangleF clippingRectangle, RectangleF drawingSpace);

        public virtual bool Initialize(CustomObjectsManager manager)
        {
            // SystemMonitor.CheckThrow(_manager == null);
            _manager = manager;
            return true;
        }

        public void UnInitialize()
        {
            _manager = null;
        }

    }
}
