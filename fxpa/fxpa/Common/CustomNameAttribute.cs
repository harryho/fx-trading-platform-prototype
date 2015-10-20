// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Text;

namespace fxpa
{
    public class CustomNameAttribute : Attribute
    {
        string name;
        public string Name
        {
            get { return name; }
        }

        public CustomNameAttribute(string name)
        {
            this.name = name;
        }

        static public string GetClassAttributeName(Type classType)
        {
            string name = classType.Name;
            GetClassAttributeValue(classType, ref name);
            return name;
        }

        static public bool GetClassAttributeValue(Type classType, ref string name)
        {
            object[] attributes = classType.GetCustomAttributes(typeof(CustomNameAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                name = ((CustomNameAttribute)attributes[0]).Name;
                return true;
            }
            return false;
        }
    }
}
