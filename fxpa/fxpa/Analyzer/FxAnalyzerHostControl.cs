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





namespace fxpa
{
    public partial class FxAnalyzerHostControl : FxpaCommonControl
    {
        AnalyzerHost _host;
        FxpaCommonControl _expertControl;

        /// <summary>
        /// 
        /// </summary>
        public FxAnalyzerHostControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public FxAnalyzerHostControl(LocalAnalyzerHost expertHost)
        {
            InitializeComponent();

            Initialize(expertHost);
        }

        /// <summary>
        /// 
        /// </summary>
        //public ActualAnalyzerHostControl(RemoteAnalyzerHost expertHost)
        //{
        //    InitializeComponent();

        //    Initialize(expertHost);
        //}

        public FxAnalyzerHostControl(AnalyzerHost expertHost)
        {
            InitializeComponent();

            Initialize(expertHost);
        }

        void Initialize(AnalyzerHost expertHost)
        {
            _host = expertHost;
            this.Title = expertHost.Name;
            //_host.SessionsUpdateEvent += new GeneralHelper.GenericDelegate<IAnalyzerSessionManager>(_expertHost_SessionsUpdateEvent);
            //_host.SourcesUpdateEvent += new GeneralHelper.GenericDelegate<IAnalyzerSessionManager>(_expertHost_SourcesUpdateEvent);
            //UpdateUI();
        }

        private void AnalyzerHostControl_Load(object sender, EventArgs e)
        {
            _expertControl = FxpaCommonControl.CreateCorrespondingControl(_host.Analyzer);
            if (_expertControl == null)
            {
                labelMain.Text = _host.AnalyzerName + ", " + _host.Analyzer.GetType().Name + " has no user interface component.";
                return;
            }

            _expertControl.Dock = DockStyle.Fill;
            _expertControl.Parent = this;
            _expertControl.BringToFront();
        }

        public override void UnInitializeControl()
        {
            base.UnInitializeControl();

            if (_host != null)
            {
                //_host.SessionsUpdateEvent -= new GeneralHelper.GenericDelegate<IAnalyzerSessionManager>(_expertHost_SessionsUpdateEvent);
                //_host.SourcesUpdateEvent -= new GeneralHelper.GenericDelegate<IAnalyzerSessionManager>(_expertHost_SourcesUpdateEvent);
                _host = null;
            }
        }

   

    }
}
