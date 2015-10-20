// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;

using System.Collections.Generic;

namespace fxpa
{
    public abstract class Analyzer
    {
        string name;
        public string Name
        {
            get { lock (this) { return name; } }
        }

        IAnalyzerSessionManager _sessionManager;
        public IAnalyzerSessionManager SessionManager
        {
            get { lock (this) { return _sessionManager; } }
        }


        /// <summary>
        /// 
        /// </summary>
        public Analyzer(IAnalyzerSessionManager sessionManager, string name)
        {
            _sessionManager = sessionManager;
            this.name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Initialize()
        {
            return OnInitialize();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool UnInitialize()
        {
            return OnUnInitialize();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool OnInitialize()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool OnUnInitialize()
        {
            return true;
        }

    }
}
