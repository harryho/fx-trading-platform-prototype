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
    public partial class FxpaCommonControl : UserControl
    {
        string description = "";
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        string _title = "";
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        protected string _imageName = "";
        public virtual string ImageName
        {
            get { return _imageName; }
            set { _imageName = value; }
        }

        public object Component
        {
            get { return Tag; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FxpaCommonControl()
        {
            InitializeComponent();
            this.Name = this.GetType().Name;

        }

        private void CommonBaseControl_Load(object sender, EventArgs e)
        {
            this.HandleDestroyed += new EventHandler(CommonBaseControl_HandleDestroyed);
        }

        void CommonBaseControl_HandleDestroyed(object sender, EventArgs e)
        {
            UnInitializeControl();
        }

        /// <summary>
        /// Perform unloading here.
        /// </summary>
        public virtual void UnInitializeControl()
        {
            Tag = null;
        }


        

        static public FxpaCommonControl CreateCorrespondingControl(object component)
        {
            List<Type> types = ReflectionHelper.GatherTypeChildrenTypesFromAssembliesWithMatchingConstructor(typeof(FxpaCommonControl), true, ReflectionHelper.GetReferencedAndInitialAssembly(Assembly.GetEntryAssembly()), new Type[] { component.GetType() });
            if (types.Count == 0)
            {
                //// Try also with a more relaxed type match.
                //types = ReflectionHelper.GatherTypeChildrenTypesFromAssembliesWithMatchingConstructor(typeof(CommonBaseControl), false, ReflectionHelper.GetReferencedAndInitialAssembly(Assembly.GetEntryAssembly()), new Type[] { component.GetType() });
                //if (types.Count == 0)
                //{
                    return null;
                //}
            }

           // SystemMonitor.CheckWarning(types.Count == 1, "More than 1 control found for this type of component, creating the first one.");

            // Return the first proper object.
            FxpaCommonControl control = (FxpaCommonControl)types[0].GetConstructor(new Type[] { component.GetType() }).Invoke(new object[] { component });
            control.Tag = component;
            return control;
        }



    }
}
