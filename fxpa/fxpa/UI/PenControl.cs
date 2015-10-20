// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    public partial class PenControl : UserControl
    {
        Pen pen;
        public Pen Pen
        {
            get 
            { 
                return pen; 
            }

            set 
            { 
                pen = (Pen)value.Clone();
                UpdateUI();
            }
        }

        string penName;
        public string PenName
        {
            get 
            { 
                return penName;
            }

            set 
            { 
                penName = value;
                UpdateUI();
            }
        }

        bool _readOnly = false;

        public bool ReadOnly
        {
            get 
            { 
                return _readOnly; 
            }

            set 
            { 
                _readOnly = value;
                UpdateUI();
            }
        }

        public delegate void PenChangedDelegate(PenControl control);
        public event PenChangedDelegate PenChangedEvent;

        /// <summary>
        /// 
        /// </summary>
        public PenControl()
        {
            InitializeComponent();
            comboBoxDash.Items.AddRange(Enum.GetNames(typeof(System.Drawing.Drawing2D.DashStyle)));
        }

        private void PenControl_Load(object sender, EventArgs e)
        {
        }

        void UpdateUI()
        {
            if (pen == null)
            {
                labelName.Text = "";
                comboBoxDash.SelectedIndex = -1;
                labelColor.BackColor = this.BackColor;
                return;
            }

            labelColor.BackColor = pen.Color;
            labelName.Text = penName;
            comboBoxDash.Enabled = !ReadOnly;
            comboBoxDash.SelectedIndex = (int)pen.DashStyle;
        }

        private void labelColor_Click(object sender, EventArgs e)
        {
            if (ReadOnly)
            {
                return;
            }

            ColorDialog dialog = new ColorDialog();
            dialog.Color = pen.Color;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                pen.Color = dialog.Color;
            }

            UpdateUI();

            if (PenChangedEvent != null)
            {
                PenChangedEvent(this);
            }
        }

        private void comboBoxDash_SelectedIndexChanged(object sender, EventArgs e)
        {
            pen.DashStyle = (System.Drawing.Drawing2D.DashStyle)Enum.Parse(typeof(System.Drawing.Drawing2D.DashStyle), comboBoxDash.SelectedItem.ToString());

            if (PenChangedEvent != null)
            {
                PenChangedEvent(this);
            }
        }
    }
}
