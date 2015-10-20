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
    /// <summary>
    /// Holds information regarding the indicator UI.
    /// </summary>
    public class BasicIndicatorUI : IPropertyContainer
    {
        bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        Dictionary<string, Pen> _outputResultSetsPens = new Dictionary<string, Pen>();
        public Dictionary<string, Pen> OutputResultSetsPens
        {
            get { return _outputResultSetsPens; }
            set { _outputResultSetsPens = value; }
        }

        BasicIndicator _indicator;
        public BasicIndicator Indicator
        {
            get { return _indicator; }
        }

        public delegate void IndicatorUIUpdatedDelegate(BasicIndicatorUI ui);
        public event IndicatorUIUpdatedDelegate IndicatorUIUpdatedEvent;

        /// <summary>
        /// 
        /// </summary>
        public BasicIndicatorUI(BasicIndicator indicator)
        {
            Console.WriteLine(indicator.Name + "," + (indicator.DataProvider != null).ToString());

            _indicator = indicator;
            foreach (string name in indicator.ResultSetNames)
            {
                _outputResultSetsPens.Add(name, null);
            }
        }

        #region IDynamicPropertyContainer Members

        public virtual string[] GetPropertiesNames()
        {
            return GeneralHelper.EnumerableToArray<string>(_outputResultSetsPens.Keys);
        }

        public virtual object GetPropertyValue(string name)
        {
            return _outputResultSetsPens[name];
        }

        public virtual Type GetPropertyType(string name)
        {
            return _outputResultSetsPens[name].GetType();
        }

        public virtual bool SetPropertyValue(string name, object value)
        {
            _outputResultSetsPens[name] = (Pen)value;
            return true;
        }

        public void PropertyChanged()
        {
            if (IndicatorUIUpdatedEvent != null)
            {
                IndicatorUIUpdatedEvent(this);
            }
        }



        #endregion

    }
}
