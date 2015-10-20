#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

#endregion

namespace System.Windows.Forms.Samples
{
    //[Designer(typeof(System.Windows.Forms.Design))]
    public class TitleStrip : ToolStrip
    {
        private HeaderRenderer      _renderer;
        private ToolStripLabel      _headerText;

        public TitleStrip()
        {
            // Check Dock
            this.Dock = DockStyle.Top;
            this.GripStyle = ToolStripGripStyle.Hidden;

            // Set renderer
            _renderer = new HeaderRenderer();

            // Look for headerText
            // Add children
            _headerText = new ToolStripLabel();

            // Add Label and Image
            _headerText.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _headerText.Text = "[Enter Text]";

            this.Items.AddRange(new ToolStripItem[] { _headerText });

            // Hook Draw Events
            this.Renderer = _renderer;
        }

        #region Public Properties
        public ToolStripLabel HeaderText
        {
            get
            {
                return _headerText;
            }
        }

        public Color GradientStartColor
        {
            get
            {
                return _renderer.StartColor;
            }
            set
            {
                _renderer.StartColor = value;
            }
        }

        public int Lines
        {
            get
            {
                return _renderer.Lines;
            }
            set
            {
                _renderer.Lines = value;
            }
        }

        public Color GradientEndColor
        {
            get
            {
                return _renderer.EndColor;
            }
            set
            {
                _renderer.EndColor = value;
            }
        }

        public bool DrawEndLine
        {
            get { return _renderer.DrawEndLine; }
            set { _renderer.DrawEndLine = value; }
        }
        #endregion
    }
}
