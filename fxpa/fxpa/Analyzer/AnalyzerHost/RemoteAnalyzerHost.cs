// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


using System.Reflection;

namespace fxpa
{
    /// <summary>
    /// AnalyzerHost (link mode) - It can provide a link to a remote separate process space host.
    /// </summary>
    /// 
    [CustomName("Remote Analyzer")]
    public class RemoteAnalyzerHostProxy : FxpaBaseComponent, /*IAnalyzerHost,*/ IDisposable
    {
        Process _remoteHostProcess;

        //string _expertName;
        //public string AnalyzerName
        //{
        //    get { return _expertName; }
        //}

        public IntPtr ProcessMainWindowHandle
        {
            get { lock (this) { return _remoteHostProcess.MainWindowHandle; } }
        }


        /// <summary>
        /// 
        /// </summary>
        public RemoteAnalyzerHostProxy(Type expertType, Uri uri)
            : base(false)
        {// Run expert host as new process

            //_expertName = expertName;

            _remoteHostProcess = new Process();
            _remoteHostProcess.StartInfo.Arguments = "\"" + uri.ToString() + "\"" + " " + expertType.ToString() + " " + "expertName";
            _remoteHostProcess.StartInfo.FileName = Assembly.GetEntryAssembly().Location;
            _remoteHostProcess.Exited += new EventHandler(process_Exited);

            if (_remoteHostProcess.Start() == false)
            {
                _remoteHostProcess = null;
                //SystemMonitor.Error("Failed to start host process.");
            }
        }

        public void Dispose()
        {
            lock (this)
            {
                if (_remoteHostProcess != null && _remoteHostProcess.HasExited == false)
                {
                    _remoteHostProcess.Kill();
                }
            }
        }

        void process_Exited(object sender, EventArgs e)
        {// A process has exited.
            //if (HostUnInitializedEvent != null)
            //{
            //    HostUnInitializedEvent(this);
            //}

            lock (this)
            {
                _remoteHostProcess = null;
            }
        }

        protected override bool OnUnInitialize()
        {
            bool result = base.OnUnInitialize();

            lock (this)
            {
                _remoteHostProcess.CloseMainWindow();
                _remoteHostProcess.Close();
            }

            return result;
        }

    }
}
