#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Text;

#endregion

namespace System.Windows.Forms.Samples
{
    public class HeaderRenderer : ToolStripProfessionalRenderer
    {
        Color       _startColor = Color.White;
        Color       _endColor = Color.FromArgb(168, 186, 212);
        int         _lines = 6;
        bool        _drawEndLine = true;

        public HeaderRenderer()
        {
        }

        public Color EndColor
        {
            get { return _endColor; }
            set { _endColor = value; }
        }

        public Color StartColor
        {
            get { return _startColor; }
            set { _startColor = value; }
        }

        public int Lines
        {
            get { return _lines; }
            set { _lines = value; }
        }

        public bool DrawEndLine
        {
            get { return _drawEndLine; }
            set { _drawEndLine = value; }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            Color       start =  _startColor;
            Color       end = _endColor;

            ToolStrip   toolStrip = e.ToolStrip;
            Graphics    g = e.Graphics;

            int         boundsHeight = e.AffectedBounds.Height;
            int         height = (boundsHeight + _lines - 1) / _lines;
            int         width = e.AffectedBounds.Width;
            int         stripeHeight = height - 1;
            Rectangle   stripeRect;

            using (Brush b = new LinearGradientBrush(new Rectangle(0, 0, width, stripeHeight), start, end, LinearGradientMode.Horizontal))
            {
                for (int idx = 0; idx < _lines; idx++)
                {
                    stripeRect = new Rectangle(0, height * idx + 1, width, stripeHeight);
                    g.FillRectangle(b, stripeRect);
                }
            }

            if (this.DrawEndLine)
            {
                using (Brush solidBrush = new SolidBrush(Color.FromArgb(177, 177, 177)))
                {
                    g.FillRectangle(solidBrush, new Rectangle(0, boundsHeight - 1, width, 1));
                }
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
        }
    }
}
