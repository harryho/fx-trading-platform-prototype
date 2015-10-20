// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;


using System.Configuration;

namespace fxpa
{
    public class FxpaBase 
    {
        //TransportIntegrationServer _server;

        /// <summary>
        /// This includes all component types grouped.
        /// </summary>
        List<IFxpaBaseCompoent> _components = new List<IFxpaBaseCompoent>();
        public ReadOnlyCollection<IFxpaBaseCompoent> Components
        {
            get { lock (this) { return _components.AsReadOnly(); } }
        }

        //List<TransportInfo> _sourcesUpdatesSubscribers = new List<TransportInfo>();

        SettingsBase _settings;
        public SettingsBase Settings
        {
            get { lock (this) { return _settings; } }
        }

        public delegate void ActiveComponentUpdateDelegate(IFxpaBaseCompoent component, bool added);
        public event ActiveComponentUpdateDelegate ActiveComponentUpdateEvent;

        Uri _platformUri;
        public Uri PlatformUri
        {
            get { lock (this) { return _platformUri; } }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        //private bool _singleThreadOnly ;


        /// <summary>
        /// 
        /// </summary>
        public FxpaBase()
            //: base("Platform", false)
        {
            // TracerHelper.Tracer = new Tracer();

            // TracerHelper.TraceEntry();

            //Arbiter arbiter = new Arbiter("Platform");
            ////ArbiterTraceHelper.TracingEnabled = false;
            //arbiter.AddClient(this);
            name = "FxpaBase";
            //_messageFilter = new MessageFilter(true);
            //_singleThreadOnly = false;
        }

        public bool Initialize(SettingsBase settings)
        {
            //SystemMonitor.CheckThrow(_settings == null, "Platform already initialized.");
            // TracerHelper.TraceEntry();

            lock (this)
            {
                _settings = settings;
                ActiveComponentUpdateEvent += new ActiveComponentUpdateDelegate(Platform_ActiveComponentUpdateEvent);

                _platformUri = new Uri((string)settings["FxpaUriAddress"]);
                //_server = new TransportIntegrationServer(_platformUri);
                //Arbiter.AddClient(_server);
            }

            return true;
        }

        public bool UnInitialize()
        {
            // TracerHelper.TraceEntry();

            lock (this)
            {
                //this.SendRespondingToMany(_sourcesUpdatesSubscribers, new SourcesSubscriptionTerminatedMessage());
                //_sourcesUpdatesSubscribers.Clear();

                while (_components.Count > 0)
                {// Cycling this way, since a component might be removing other components.
                    IFxpaBaseCompoent component = this._components[0];
                    if (component.IsInitialized)
                    {
                        UnInitializeComponent(component);
                    }
                    _components.Remove(component);
                }

                //this.Arbiter.Dispose();
            }

            return true;
        }

        public TComponentType[] GetComponents<TComponentType>()
            // where ComponentType : IPlatformComponent, 
            // all in here are IPlatformComponent but searching is not always by that criteria
        {
            List<TComponentType> result = new List<TComponentType>();
            foreach (IFxpaBaseCompoent component in _components)
            {
                Type componentType = component.GetType();

                if (typeof(TComponentType).IsInterface)
                {// Interface checking is different.
                    List<Type> interfaces = new List<Type>(componentType.GetInterfaces());
                    if (interfaces.Contains(typeof(TComponentType)))
                    {
                        result.Add((TComponentType)component);
                    }
                }
                else
                {// Normal class check.
                    if (componentType == typeof(TComponentType)
                        || componentType.IsSubclassOf(typeof(TComponentType)))
                    {
                        result.Add((TComponentType)component);
                    }
                }
            }
            return result.ToArray();
        }

        

        /// <summary>
        /// Prepare the object for operation. Access to this allows externals to use the 2 step component registration process.
        /// </summary>
        public bool InitializeComponent(IFxpaBaseCompoent component)
        {
            // TracerHelper.Trace(component.Name);

            //Arbiter.AddClient(component);
            //if (component.IsInitialized == false && component.Initialize(this) == false)
            //{
            //    Arbiter.RemoveClient(component);
            //    return false;
            //}
            component.Initialize(this);
            return true;
        }

        /// <summary>
        /// Bring down the object from operation. Access to this allows externals to use the 2 step component registration process.
        /// </summary>
        public bool UnInitializeComponent(IFxpaBaseCompoent component)
        {
            // TracerHelper.Trace(component.Name);
            
            component.UnInitialize();
            //Arbiter.RemoveClient(component);
            return true;
        }

        void Platform_ActiveComponentUpdateEvent(IFxpaBaseCompoent component, bool isAdded)
        {
            // TracerHelper.Trace(component.Name);

            //if (component is DataSource == false && component is ExecutionSource == false)
            //{
            //    return;
            //}

            if (component is FxpaSource == false )
            {
                return;
            }

            //SourceUpdatedMessage message = new SourceUpdatedMessage(component.SubscriptionClientID, component is DataSource, isAdded);
            lock (this)
            {
                //foreach (TransportInfo info in _sourcesUpdatesSubscribers)
                //{
                //    this.SendResponding(info, message);
                //}
            }

        }

        /// <summary>
        /// Add the object to the list of active objects and call event to notify all listeners, a new object has been added.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool RegisterComponent(IFxpaBaseCompoent component)
        {
            // TracerHelper.Trace(component.Name);

            if (InitializeComponent(component) == false)
            {
                return false;
            }

            lock (this)
            {
                if (_components.Contains(component))
                {
                    return true;
                }
                _components.Add(component);
            }

            GeneralHelper.SafeEventRaise(new GeneralHelper.GenericDelegate<IFxpaBaseCompoent, bool>(
                ActiveComponentUpdateEvent), component, true);

            return true;
        }

        public bool UnRegisterComponent(IFxpaBaseCompoent component)
        {
            // TracerHelper.Trace(component.Name);

            UnInitializeComponent(component);

            lock (this)
            {
                if (_components.Remove(component) == false)
                {
                    return true;
                }
            }

            if (ActiveComponentUpdateEvent != null)
            {
                ActiveComponentUpdateEvent(component, false);
            }
            return true;
        }

        //[MessageReceiver]
        //ResultTransportMessage Receive(SubscribeToSourcesMessage message)
        //{
        //    lock (this)
            //{
                //if (_sourcesUpdatesSubscribers.Contains(message.TransportInfo) == false)
                //{
                //    _sourcesUpdatesSubscribers.Add(message.TransportInfo);
                //}

                //foreach (SessionedSource source in GetComponents<DataSource>())
                //{
                //    this.SendResponding(message.TransportInfo, new SourceUpdatedMessage(source.SubscriptionClientID, true, true));
                //}

                //foreach (SessionedSource source in GetComponents<ExecutionSource>())
                //{
                //    this.SendResponding(message.TransportInfo, new SourceUpdatedMessage(source.SubscriptionClientID, false, true));
                //}
        //    }

        //    return new ResultTransportMessage(true);
        //}

        //[MessageReceiver]
        //ResultTransportMessage Receive(UnSubscribeToSourcesMessage message)
        //{
        //    lock (this)
        //    {
        //        _sourcesUpdatesSubscribers.Remove(message.TransportInfo);
        //    }

        //    if (message.RequestConfirmation == false)
        //    {
        //        return null;
        //    }

        //    return new ResultTransportMessage(true);
        //}

    }
}
