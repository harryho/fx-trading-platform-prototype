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
    public partial class FxpaBaseComponentControl : FxpaCommonControl
    {
        FxpaBaseComponent _component;
        protected new FxpaBaseComponent Component
        {
            get { return _component; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FxpaBaseComponentControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public FxpaBaseComponentControl(FxpaBaseComponent component)
        {
            InitializeComponent();
            _component = component;
            //SystemMonitor.CheckWarning(_component.IsInitialized);
        }

        private void PlatformComponentControl_Load(object sender, EventArgs e)
        {
            // SystemMonitor.CheckThrow(Tag == _component, "Tag is expected to be component.");
        }

        //void _component_ComponentInitializedEvent(ActivePlatformComponent component, bool initialized)
        //{
        //    if (this.IsHandleCreated)
        //    {
        //        this.Invoke(new GeneralHelper.GenericDelegate<bool>(ComponentInitialized), initialized);
        //    }
        //    else
        //    {
        //        SystemMonitor.Warning("Component initialized before UI.");
        //    }
        //}

        //protected virtual void ComponentInitialized(bool initialized)
        //{
        //}
    }
}
