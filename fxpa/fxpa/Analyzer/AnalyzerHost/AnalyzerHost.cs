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
using System.Collections.ObjectModel;


namespace fxpa
{
    /// <summary>
    /// Analyzer host has a tripple role:
    /// (local mode) - It can host an expert on the local ForexPlatformFrontEnd in the platform execution process
    /// </summary>
    public abstract class AnalyzerHost :  IAnalyzerSessionManager
    {
        Type _expertType;

        protected Analyzer _expert;
        public Analyzer Analyzer
        {
            get { lock (this) { return _expert; } }
        }

        bool _isConnectedToPlatform = false;
        public bool IsConnectedToPlatform
        {
            get { lock (this) { return _isConnectedToPlatform; } }
        }

        
        List<AnalyzerSession> _existingSessions = new List<AnalyzerSession>();
        public ReadOnlyCollection<AnalyzerSession> Sessions
        {
            get { lock (this) { return _existingSessions.AsReadOnly(); } }
        }

        public int ExistingSessionsCount
        {
            get { lock (this) { return _existingSessions.Count; } }
        }

        public string AnalyzerName
        {
            get
            {
                lock (this)
                {
                    if (_expert != null)
                    {
                        return _expert.Name;
                    }
                    else { return ""; }
                }
            }
        }
        private bool _singleThreadOnly;
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        //public delegate void SourceSessionUpdateDelegate(IAnalyzerSessionManager manager, ArbiterClientId source, SessionInfo info, bool isAdded);

        public event GeneralHelper.GenericDelegate<IAnalyzerSessionManager> SourcesUpdateEvent;
        public event GeneralHelper.GenericDelegate<IAnalyzerSessionManager> SessionsUpdateEvent;
        TimeSpan _defaultTimeOut = TimeSpan.Zero;
        protected TimeSpan DefaultTimeOut
        {
            get { return _defaultTimeOut; }
            set { _defaultTimeOut = value; }
        }
        /// <summary>
        /// Local execution. Analyzer to be executed locally, within the platform process space and on the platforms ForexPlatformFrontEnd.
        /// </summary>
        protected AnalyzerHost(string name, Type expertType)
            //: base(name, false)
        {
            // TracerHelper.Trace(this.Name);
            _expertType = expertType;
            Name = name;
            _singleThreadOnly = false;
            DefaultTimeOut = TimeSpan.FromSeconds(10);
        }

        #region Host Related

        protected bool HostInitialize()
        {
            string expertName = this.Name + ".expert";// this.Name.Replace(".host", ".expert");

            // Clean expertname since we might be sending it trough command line.
            expertName = expertName.Replace(" ", "_");
            expertName = expertName.Replace("\"", "");

            lock (this)
            {
                // Create the expert.
                ConstructorInfo constructor = _expertType.GetConstructor(new Type[] { typeof(AnalyzerHost), typeof(string) });

                //SystemMonitor.CheckThrow(constructor != null, "Failed to find corresponding constructor for expert type [" + _expertType.ToString() + "].");
                _expert = (Analyzer)constructor.Invoke(new object[] { this, expertName });

                if (_expert.Initialize() == false)
                {
                    //SystemMonitor.Error("Analyzer host failed to connect to platform.");
                    return false;
                }
               _isConnectedToPlatform = true;
                return true;

            }
        }

        protected bool HostUnInitialize()
        {
            lock (this)
            {
                if (_isConnectedToPlatform)
                {
                    _isConnectedToPlatform = false;
                }

                while (_existingSessions.Count > 0)
                {
                    DestroySession(_existingSessions[0]);
                }
                _expert.UnInitialize();
                _expert = null;
            }
            return true;
        }      

        #endregion

   
        #region IAnalyzerSessionManager Members

        public AnalyzerSession CreateSimulationSession(
          SessionInfo sessionInfo)
        {

            AnalyzerSession session = new AnalyzerSession(sessionInfo);
            DataProvider dataProvider = null;
            lock (this)
            {
                if (AppContext.FirstDataProvider != null)
                {
                    dataProvider = AppContext.FirstDataProvider; // new DataProvider(sessionInfo.Symbol);
                    dataProvider.Run();
                }
                else
                {
                     dataProvider = new DataProvider(sessionInfo.Symbol);
                    dataProvider.Init();
                }
                session.Initialize(dataProvider);
                _existingSessions.Add(session);
            }
            // comment by Harry
            //GeneralHelper.SafeEventRaise(SessionsUpdateEvent, this);
            return session;
        }
        
        public void DestroySession(AnalyzerSession session)
        {
            lock (this)
            {
                ((DataProvider)session.DataProvider).UnInitialize();
                session.UnInitialize();
                _existingSessions.Remove(session);
            }

            GeneralHelper.SafeEventRaise(SessionsUpdateEvent, this);
        }

        #endregion
    }
}
