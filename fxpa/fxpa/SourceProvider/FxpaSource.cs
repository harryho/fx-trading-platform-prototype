// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

using System.Configuration;


namespace fxpa
{
    [CustomName("FxpaSource")]
    public class FxpaSource:IFxpaBaseCompoent
    {
       public static string folderPath=AppContext.APP_PATH+"\\fxd";
        //public string FolderPath
        //{
        //    get { lock (this) { return folderPath; } }
        //    set { lock (this) { folderPath = value; } }
        //}

        public static SettingsBase settings;
        FxpaBase _platform;
        protected FxpaBase Platform
        {
            get { return _platform; }
        }

        public virtual bool IsInitialized
        {
            get { return _platform != null; }
        }

        //protected Dictionary<SessionInfo, List<TransportInfo>> _sessions = new Dictionary<SessionInfo, List<TransportInfo>>();

        private static  List <SessionInfo> sessionList= new List<SessionInfo>();
        private static List<SessionInfo> availableSessionList = new List<SessionInfo>();

        public static List<SessionInfo> AvailableSessionList
        {
            get { return FxpaSource.availableSessionList; }
            //set { FxpaSource.availableSessionList = value; }
        }

        public static List<SessionInfo> SessionList
        {
            get { lock (sessionList) { return sessionList; } }
        }

        private void AddSessionInfo(SessionInfo info)
        {
            availableSessionList.Add(info);
        }

        
        public static  void AddAvailableSession(string symbol){
            SessionInfo info = new SessionInfo(symbol, symbol, Guid.NewGuid(), TimeSpan.FromMinutes(30), 100000, 2);
            if(!availableSessionList.Contains(info))
                 availableSessionList.Add( info);
        }


        protected void AddSession(SessionInfo sessionInfo)
        {
            sessionList.Add(sessionInfo);
        }

        protected void RemoveSession(SessionInfo sessionInfo)
        {
         sessionList.Remove(sessionInfo);
            
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
            _platform = platform;

            return true;
        }

        public bool UnInitialize()
        {
            if (IsInitialized == false)
            {
                return true;
            }

            bool result = OnUnInitialize();
            _platform = null;

            return result;
        }
        protected virtual bool OnUnInitialize()
        {
            return true;
        }


        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public FxpaSource()
            //: base(false)
        {
            this.Name = CustomNameAttribute.GetClassAttributeName(typeof(FxpaSource));
            //this._id.Name = this.Name;
        }

        protected bool OnSetInitialState(SettingsBase data)
        {
            settings = data;
            //folderPath = (string)data["QuoteDataFolder"];
            return true;
        }

        protected bool OnInitialize(FxpaBase platform)
        {
            UpdateAvailableSourceSessions();
            return true;
        }

        void UpdateAvailableSourceSessions()
        {
            string current = Directory.GetCurrentDirectory();
            Console.WriteLine(" XXXXXXXXXXXXXX  file  >>>>>>>>> " + current );

            foreach (string symbol in Enum.GetNames(Symbol.AUDJPY.GetType()))
            {
                SessionInfo info = new SessionInfo(symbol , symbol, Guid.NewGuid(), TimeSpan.FromMinutes(30) , 100000, 2);
                this.AddSession(info);
            }
        }
    }
}

