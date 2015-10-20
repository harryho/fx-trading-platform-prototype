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
    public class IndicatorResultSet
    {
        string name;
        public string Name
        {
            get { return name; }
        }

        double[] _values;
        public double[] Values
        {
            get 
            {
                lock (this)
                {
                    return _values;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IndicatorResultSet(string name, double[] values)
        {
            this.name = name;
            _values = values;
        }

        public void Clear()
        {
            lock (this)
            {
                _values = new double[0];
            }
        }

        public void SetValues(double[] values)
        {
            lock (this)
            {
                _values = values;
            }
        }
    }
}
