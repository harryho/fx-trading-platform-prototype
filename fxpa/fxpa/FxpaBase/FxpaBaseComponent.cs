// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;


using System.Configuration;

namespace fxpa
{
    public abstract class FxpaBaseComponent :  IFxpaBaseCompoent
    {
        FxpaBase fxpaBase;
        protected FxpaBase FxpaBase
        {
            get { return fxpaBase; }
        }
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public virtual bool IsInitialized
        {
            get { return fxpaBase != null; }
        }
        private bool IsSingleThread = false;
        /// <summary>
        /// Component will try to use UserFriendlyNameAttribute to establish a name for the current component, or if not available use class type name.
        /// </summary>
        public FxpaBaseComponent(bool singleThreadMode)
        {
            name = "FxpaBaseComponent";
            IsSingleThread = singleThreadMode;
        }

        public bool SetInitialState(SettingsBase data)
        {
            return OnSetInitialState(data);
        }

        public bool Initialize(FxpaBase platform)
        {
            //SystemMonitor.CheckThrow(_platform == null);

            if (OnInitialize(platform) == false)
            {
                return false;
            }
            fxpaBase = platform;
            
            return true;
        }

        public bool UnInitialize()
        {
            if (IsInitialized == false)
            {
                return true;
            }

            bool result = OnUnInitialize();
            fxpaBase = null;

            return result;
        }

        protected virtual bool OnSetInitialState(SettingsBase data)
        {
            return true;
        }

        protected virtual bool OnInitialize(FxpaBase platform)
        {
            return true;
        }

        protected virtual bool OnUnInitialize()
        {
            return true;
        }

    }
}
