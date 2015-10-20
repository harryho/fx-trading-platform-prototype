// -----
// GNU General Public License
// The Open Forex Platform is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Open Forex Platform is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Text;
//using System.Windows.Forms;
//using System.Globalization;


//namespace ForexPlatformFrontEnd
//{
//    public partial class NumericComboBox : ComboBox
//    {
//        public double Value
//        {
//            get
//            {
//                double result = 0;
//                if (double.TryParse(this.SelectedText, out result))
//                {
//                    return result;
//                }
//                else
//                {
//                    return 0;
//                }
//            }

//            set
//            {
//                this.SelectedText = value.ToString();
//            }
//        }

//        double _minimumValue = 0;
//        public double MinimumValue
//        {
//            get { return _minimumValue; }
//            set { _minimumValue = value; }
//        }

//        double _maximumValue = 1000;
//        public double MaximumValue
//        {
//            get { return _maximumValue; }
//            set { _maximumValue = value; }
//        }

//        public delegate void ValueChangedDelegate(NumericComboBox comboBox);
//        public event ValueChangedDelegate ValueChangedEvent;

//        /// <summary>
//        /// 
//        /// </summary>
//        public NumericComboBox()
//        {
//            this.LostFocus += new EventHandler(NumericComboBox_LostFocus);
//        }

//        void NumericComboBox_LostFocus(object sender, EventArgs e)
//        {
//            if (this.Text.Length == 0)
//            {
//                this.Text = "0";
//            }
//        }

//        protected override void OnSelectedValueChanged(EventArgs e)
//        {
//            base.OnSelectedValueChanged(e);
//            UpdateUI();
//        }

//        protected override void OnSelectedIndexChanged(EventArgs e)
//        {
//            base.OnSelectedIndexChanged(e);
//            UpdateUI();
//        }

//        protected override void OnSelectedItemChanged(EventArgs e)
//        {
//            base.OnSelectedItemChanged(e);
//            UpdateUI();
//        }

//        void UpdateUI()
//        {
//            if (this.Text != this.SelectedText)
//            {
//                this.SelectedText = this.Text;
//            }

//            if (Value < MinimumValue)
//            {
//                Value = MinimumValue;
//            }
//            else if (Value > MaximumValue)
//            {
//                Value = MaximumValue;
//            }

//            if (ValueChangedEvent != null)
//            {
//                ValueChangedEvent(this);
//            }
//        }

//        protected override void OnKeyDown(KeyEventArgs e)
//        {
//            if (e.Control == false && e.Alt == false && e.KeyCode != Keys.Back &&
//                char.IsDigit((char)e.KeyCode) == false && e.KeyCode != Keys.OemPeriod)
//            {
//                e.SuppressKeyPress = true;
//                return;
//            }

//            if (e.KeyCode == Keys.OemPeriod && this.Text.Contains("."))
//            {
//                e.SuppressKeyPress = true;
//                return;
//            }

//            base.OnKeyDown(e);

//            UpdateUI();
//        }

//        protected override void OnTextUpdate(EventArgs e)
//        {
//            foreach(char c in this.Text)
//            {
//                if (char.IsDigit(c) == false && c != '.')
//                {
//                    this.Text = "";
//                    return;
//                }
//            }

//            base.OnTextUpdate(e);

//            UpdateUI();
//        }
//    }
//}
