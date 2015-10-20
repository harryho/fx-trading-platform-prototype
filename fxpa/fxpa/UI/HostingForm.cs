// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    public partial class HostingForm : Form
    {
        Control control;
        public Control Control
        {
            get { return control; }
        }

        public bool ShowCloseButton
        {
            get { return panelButtonClose.Visible; }
            set { panelButtonClose.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public HostingForm(string formText)
        {
            InitializeComponent();
            this.Text = formText;
        }

        /// <summary>
        /// 
        /// </summary>
        public HostingForm(string formText, Control control)
        {
            InitializeComponent();
            this.Text = formText;

            this.control = control;
            this.control.Visible = true;

            this.SuspendLayout();

            Controls.Add(control);
            control.Dock = System.Windows.Forms.DockStyle.Fill;

            Size = control.Size;
            Width += 6;
            Height += 55;

            this.ResumeLayout();
        }

        void controlVisibleChanged(object sender, EventArgs e)
        {
            if (control.Visible == false && this.Visible == true)
            {
                this.Close();
            }
        }

        public static HostingForm ShowHostingForm(Control control, string controlName)
        {
            HostingForm form = CreateHostingFormControl(control, controlName);
            form.Show();
            return form;
        }

        /// <summary>
        /// 
        /// </summary>
        public static HostingForm CreateHostingFormControl(Control control, string controlName)
        {
            Size requiredSize = control.Size;
            HostingForm hostingForm = new HostingForm(controlName);
            hostingForm.Controls.Add(control);
            control.Dock = System.Windows.Forms.DockStyle.Fill;
            hostingForm.Size = requiredSize;
            return hostingForm;
        }

        private void HostingForm_Load(object sender, EventArgs e)
        {
            // Not before that.
            control.VisibleChanged += new EventHandler(controlVisibleChanged);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            // If shown with Show() dialog result is not considered.
            this.Close();
        }

    }
}
