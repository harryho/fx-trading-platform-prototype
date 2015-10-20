// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;


namespace fxpa
{
    public class FxpaIndicatorUI : BasicIndicatorUI
    {
        public new FxpaIndicator Indicator
        {
            get { return (FxpaIndicator)base.Indicator; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FxpaIndicatorUI(FxpaIndicator indicator)
            : base(indicator)
        {
            Console.WriteLine(indicator.Name + "," + (indicator.DataProvider != null).ToString());
        }

        public override string[] GetPropertiesNames()
        {
            List<string> results = new List<string>();
            for (int i = 0; i < Indicator.IntputParameters.Count; i++)
            {
                results.Add(Indicator.IntputParameters[i].Name);
            }

            results.AddRange(base.GetPropertiesNames());

            return results.ToArray();
        }

        public override object GetPropertyValue(string name)
        {
            for (int i = 0; i < Indicator.IntputParameters.Count; i++)
            {
                if (Indicator.IntputParameters[i].Name == name)
                {
                    return Indicator.InputParametersValues[i];
                }
            }

            return base.GetPropertyValue(name);
        }

        public override Type GetPropertyType(string name)
        {
            for (int i = 0; i < Indicator.IntputParameters.Count; i++)
            {
                if (Indicator.IntputParameters[i].Name == name)
                {
                    return Indicator.IntputParameters[i].ParameterType;
                }
            }

            return base.GetPropertyType(name);
        }

        public override bool SetPropertyValue(string name, object value)
        {
            for (int i = 0; i < Indicator.IntputParameters.Count; i++)
            {
                if (Indicator.IntputParameters[i].Name == name)
                {
                    Indicator.InputParametersValues[i] = value;
                    return true;
                }
            }

            return base.SetPropertyValue(name, value);
        }
    }
}
