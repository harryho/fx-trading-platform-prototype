// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace fxpa
{
    [CustomName("Professional Analyzer")]
    public class ProfessionalAnalyzer : Analyzer
    {
        IAnalyzerSessionManager _sessionManager;

        /// <summary>
        /// 
        /// </summary>
        public ProfessionalAnalyzer(IAnalyzerSessionManager sessionManager, string name)
            : base(sessionManager, name)
        {
            _sessionManager = sessionManager;
            //_sessionManager.SourcesUpdateEvent += new GeneralHelper.GenericDelegate<IAnalyzerSessionManager>(_sessionManager_SourcesUpdateEvent);
        }

        protected override bool OnInitialize()
        {
            return true;
        }

        void _sessionManager_SessionsUpdateEvent(IAnalyzerSessionManager parameter1)
        {
        }

        void _sessionManager_SourcesUpdateEvent(IAnalyzerSessionManager parameter1)
        {
            //lock (this)
            //{
            //    if (_sessionManager.OrderExecutionSources.Length > 0
            //        && _sessionManager.DataProviderSources.Length > 0)
            //    {
            //        SessionInfo[] sessionInfos = new SessionInfo[] { };
            //        while (sessionInfos.Length == 0)
            //        {
            //            Thread.Sleep(1000);
            //            sessionInfos = _sessionManager.GetSourceSessions(_sessionManager.DataProviderSources[0], false);
            //        }

            //        _sessionManager.CreateSession(AnalyzerSessionTypeEnum.LiveTrading,
            //            _sessionManager.DataProviderSources[0], _sessionManager.OrderExecutionSources[0], sessionInfos[0]);
            //    }
            //}
        }

    }
}
