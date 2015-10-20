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

using System.Reflection;


namespace fxpa
{
    public partial class IndicatorPropertiesControl : CustomPropertiesControl
    {
        public new BasicIndicator SelectedObject
        {
            get
            {
                return Indicator;
            }

            set
            {
                Indicator = value;
            }
        }

        public BasicIndicator Indicator
        {
            get
            {
                if (base.SelectedObject == null)
                {
                    return null;
                }
                else
                {
                    return ((BasicIndicatorUI)base.SelectedObject).Indicator;
                }
            }

            set 
            {
                if (value == null)
                {
                    //TracerHelper.Trace("Null");
                    base.SelectedObject = null;
                }
                else
                {
                    //TracerHelper.Trace(value.Name + ", " + (value.DataProvider != null).ToString());
                    base.SelectedObject = value.UI;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IndicatorPropertiesControl()
        {
            InitializeComponent();
        }

        private void IndicatorControl_Load(object sender, EventArgs e)
        {
            UpdateUI();

            base.FilteringPropertiesNames.AddRange( new string[] { "Description"} );
            base._startingYLocation = textBoxDescription.Bottom + InterControlMargin;
        }
        
        /// <summary>
        /// Main UI logic function.
        /// </summary>
        protected override void OnUpdateUI(int startingYValue)
        {
            textBoxDescription.Visible = Indicator != null;
            labelDesription.Visible = Indicator != null;

            if (Indicator == null)
            {
                return;
            }
            
            textBoxDescription.Text = Indicator.Description;

            int lastYValue = startingYValue;

        }

    }
}
