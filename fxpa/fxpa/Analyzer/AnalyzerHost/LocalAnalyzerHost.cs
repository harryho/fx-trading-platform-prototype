// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

using System.Diagnostics;
using System.Reflection;

namespace fxpa
{
    /// <summary>
    /// Analyzer host has a tripple role:
    /// (local mode) - It can host an expert on the local ForexPlatformFrontEnd in the platform execution process
    /// </summary>
    public class LocalAnalyzerHost : AnalyzerHost, IFxpaBaseCompoent
    {
        FxpaBase _platform;

        public bool IsInitialized
        {
            get { lock (this) { return _platform != null; } }
        }

        /// <summary>
        /// Local execution. Analyzer to be executed locally, within the platform process space and on the platforms ForexPlatformFrontEnd.
        /// </summary>
        /// <param name="platformAddress"></param>
        public LocalAnalyzerHost(string name, Type expertType)
            : base(name, expertType)
        {
            // TracerHelper.Trace(this.Name);
        }

        public bool Initialize(FxpaBase platform)
        {
            lock (this)
            {
                _platform = platform;
                //HostInitialize(new List<ArbiterClientId?>(new ArbiterClientId? [] { _platform.SubscriptionClientID } ));
                HostInitialize();
            }

            return true;
        }

        public bool UnInitialize()
        {
            lock (this)
            {
                HostUnInitialize();
                _platform = null;
            }
            return true;
        }

        public bool SetInitialState(System.Configuration.SettingsBase data)
        {
            return true;
        }

        //public override AnalyzerSession CreateLiveTradingSession(SessionInfo sessionInfo)
        //{

        //    //DataProviderSource[] sources = _platform.GetComponents<DataProviderSource>();
        //    //List<ArbiterClientId?> stack = new List<ArbiterClientId?>();
        //    //stack.Add(sources[0].SubscriptionClientID);
        //    //DataProvider dataProvider = new DataProvider("DP1", stack);
        //    //ForexPlatformFrontEnd.AddClient(dataProvider);
        //    //dataProvider.Initialize();

        //    //OrderExecutionProvider executionProvider = new OrderExecutionProvider("EP1");
        //    //ForexPlatformFrontEnd.AddClient(executionProvider);

        //    //AnalyzerSession session = new AnalyzerSession(dataProvider, executionProvider);
        //    //return session;

        //}


    }
}
