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
using System.Globalization;



namespace fxpa
{
  
    public partial class FxAnalyzerControl : FxpaCommonControl
    {

        //存储Session button
        ToolStripButton _controlButton = new ToolStripButton();
        public ToolStripButton ControlButton
        {
            get { return _controlButton; }
            set { _controlButton = value; }
        }

        //Operate ChartControl
        ChartControl _opchartControl = new ChartControl();
        public ChartControl OpChartControl
        {
            get { return _opchartControl; }
            set { _opchartControl = value; }
        }

        ToolBarButtonStatus _optoolBarStatus = new ToolBarButtonStatus();
        public ToolBarButtonStatus OpToolBarStatus
        {
            get { return _optoolBarStatus; }
            set { _optoolBarStatus = value; }
        }
        //---------------------------------

        AnalyzerSession asession;
        public AnalyzerSession ASession
        {
            get { return asession; }
            set { asession = value; }
        }

        IAnalyzerSessionManager _host;

        public IAnalyzerSessionManager Host
        {
            get { return _host; }
            set { _host = value; }
        }

        
        SessionInfo[] _sessions;

  

        SessionInfo? SelectedSession
        {
            get
            {
                int ii = priceListView.SelectedIndices[0];
                return _sessions[ii];
            }
        }

        int SelectedSignal
        {
            get
            {
                int ii = priceListView.SelectedIndices[0];
                return ii;
            }
        }


        public delegate void SessionCreatedDelegate();
        public event SessionCreatedDelegate SessionCreatedEvent;
        //---------------------------------------------------------------------------

        ProfessionalAnalyzer tradeAnalyzer;

        public override string ImageName
        {
            get { return _imageName; }
            set { _imageName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FxAnalyzerControl()
        {
            // 
            // label1
            // 

            InitializeComponent();
        }

        List<Symbol> signalSymbols = new List<Symbol>();

        /// <summary>
        /// 
        /// </summary>
        public FxAnalyzerControl(ProfessionalAnalyzer expert)
        {
            InitializeComponent();

            tradeAnalyzer = expert;
            tradeAnalyzer.SessionManager.SessionsUpdateEvent += new GeneralHelper.GenericDelegate<IAnalyzerSessionManager>(SessionManager_SessionUpdateEvent);

            //Adjust Listview column width
            priceListView_ColumnWidthChanged();

            DataService.priceList = priceListView;
            DataService.ImageList = imageList1;            

           //设置数据源
            _host = tradeAnalyzer.SessionManager;
            
            _sessions = null; 
            _sessions = FxpaSource.SessionList.ToArray();
            
            int i;

            for (i = 0; i < _sessions.Length; i++)
            {
                string symbol = AppUtil.GetSymbolChinese(_sessions[i].Symbol);
                if (symbol != "")
                {
                  ListViewItem   p = new ListViewItem(new string[] { AppUtil.GetSymbolChinese(_sessions[i].Symbol), "", "", "" });
                    priceListView.Items.Add(p);
                }
            }

            //priceListView.Items.AddRange(p);

            // Comment by Harry
            //根据内容自动Adjust 列头大小
            //AutoResizeColumnWidth(priceListView);

            //
            DateTime now = DateTime.Now;
            //this.titleStrip1.HeaderText.Text = " " + now.ToString("MM dd, yyyy  " + AppConst.Days[Convert.ToInt16(now.DayOfWeek)]);
            //this.titleStrip1.HeaderText.Font = new Font(this.titleStrip1.HeaderText.Font, FontStyle.Bold);
            //this.titleStrip1.HeaderText.Margin = new Padding(10, 0, 0, 0);
            //this.titleStrip1.HeaderText.ForeColor = Color.FromArgb(91, 89, 91);
            timeLabel.Text =now.ToString("MM'月'dd' day ' " + AppConst.Days[Convert.ToInt16(now.DayOfWeek)]);
            //this.label1.Margin = new Padding(10, 0, 0, 0);
            //this.label1.ForeColor = Color.FromArgb(91, 89, 91);
            timeLabel.Refresh();

             AnalyzerSession session;
            foreach (SessionInfo sin in FxpaSource.AvailableSessionList)
            {
                    aliveSessions.Add(sin.Symbol);
                    session = _host.CreateSimulationSession(sin);
                    break;
            }

            //AppContext.SignalHandler.PriceListView = priceListView;
            //AppContext.SignalHandler.AnalyzerControl = this;
            //AppContext.SignalListHandler.PriceListView = priceListView;
          //  AppContext.TimeCheckHandler.PriceListView = priceListView;
            AppContext.PriceListView = priceListView;
            AppContext.TradeAnalyzerControl = this;
        }


        public void AddSignalSymbol(Symbol symbol)
        {
            lock (signalSymbols)
            {
                if(!signalSymbols.Contains(symbol))
                     signalSymbols.Add(symbol);
            }
        }

        public void ActiveTimer()
        {
            timer.Start();
        }

        Color btnBOriColor;
        Color btnFOriColor;
        void TwinklingButton(object obj, System.Timers.ElapsedEventArgs e)
        {
            if (signalSymbols.Count > 0)
            {
                MethodInvoker mi = new MethodInvoker(Twinkling);
                this.Invoke(mi);
            }
            else
            {
                timer.Stop();
            }
        }


       void Twinkling()
        {
            foreach (ToolStripButton button in GetSessionsButtons())
            {
                //Console.WriteLine("signalSymbols c  ~~~~  " + signalSymbols.Count);
                if (!signalSymbols.Contains(((AnalyzerSessionControl)button.Tag).AnalyzerSession.Symbol))
                {
                    //Console.WriteLine(" NOT contain ~~~~  " +((AnalyzerSessionControl)button.Tag).Session.Symbol);
                    continue;
                }
                if (button.BackColor != Color.DarkGreen)
                {
                    button.BackColor = Color.DarkGreen;
                    button.ForeColor = Color.OrangeRed;
                    button.Font = new Font(button.Font, FontStyle.Bold);
                    button.Invalidate();
                }
                else
                {
                    button.BackColor = btnBOriColor;
                    button.ForeColor = btnFOriColor;
                    button.Font = new Font(button.Font, FontStyle.Bold);
                    button.Invalidate();
                }
            }
        }

       public DataProvider GetProviderBySymbol( string symbol )
       {
           foreach (ToolStripButton button in GetSessionsButtons())
           {
               if (((AnalyzerSessionControl)button.Tag).AnalyzerSession.Symbol.ToString() == symbol)
               {
                   return   (DataProvider)((AnalyzerSessionControl)(button.Tag)).AnalyzerSession.DataProvider;
               }
           }
           return null;
       }

       Font btnOriFont;
        System.Timers.Timer timer = new System.Timers.Timer(500);
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Clear temp items.
            for (int i = toolStrip.Items.Count - 1; i >= 0; i--)
            {
                ToolStripItem item = toolStrip.Items[i];
                if (item.Tag as string == "del")
                {
                    toolStrip.Items.Remove(item);
                }
            }

            UpdateUI();

            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.TwinklingButton);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Stop();
        }

        Type[] GetCompatibleSimulationOrderExecutionSources(SessionInfo dataProviderSession)
        {
            List<Type> types = ReflectionHelper.GatherTypeChildrenTypesFromAssemblies(typeof(LocalOrderExecutionProvider), ReflectionHelper.GetApplicationEntryAssemblyReferencedAssemblies());
            types.Add(typeof(LocalOrderExecutionProvider));
            return types.ToArray();
        }

        void SessionManager_SessionUpdateEvent(IAnalyzerSessionManager sessionManager)
        {
            this.BeginInvoke(new GeneralHelper.DefaultDelegate(UpdateUI));
        }

        public List<ToolStripButton> GetSessionsButtons()
        {
            List<ToolStripButton> sessionsButtons = new List<ToolStripButton>();
            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (item is ToolStripButton && item.Tag is AnalyzerSessionControl)
                {
                    sessionsButtons.Add((ToolStripButton)item);
                }
            }
            return sessionsButtons;
        }

        void UpdateUI()
        {
            if (tradeAnalyzer == null)
            {
                return;
            }

            List<ToolStripButton> sessionsButtons = GetSessionsButtons();

            foreach(AnalyzerSession session in tradeAnalyzer.SessionManager.Sessions)
            {
                bool sessionFound = false;
                foreach(ToolStripButton button in sessionsButtons)
                {
                    if (((AnalyzerSessionControl)button.Tag).AnalyzerSession == session)
                    {// Session found and presented, continue.
                        sessionsButtons.Remove(button);
                        sessionFound = true;
                        break;
                    }
                }

                if (sessionFound)
                {
                    continue;
                }

                // Session not found, create new button for it.
                AnalyzerSessionControl control = new AnalyzerSessionControl();
                control.Visible = false;
                control.Dock = DockStyle.Fill;
                control.Parent = this;
                control.BringToFront();
                control.AnalyzerSession = session;
                control.CreateControl();

                ((DataProvider)session.DataProvider).IsAddCandle = true;
                ((DataProvider)session.DataProvider).Process(null, null);

                ToolStripButton newButton = new ToolStripButton(session.SessionInfo.Id, Properties.Resources.coins3);
                
                //ToolStripButton newButton = new TimerButton(session.SessionInfo.Id, Properties.Resources.PIN_GREY);
                newButton.Tag = control;                
                newButton.Click += new EventHandler(toolStripButton_Click);
                newButton.Enabled = true;
                newButton.BackColor = Color.Yellow;
                this.toolStrip.Items.Add(newButton);

                btnOriFont = newButton.Font;
                //newButton.BackColor = Color.Pink;
                btnBOriColor = newButton.BackColor;
                btnFOriColor = newButton.ForeColor;
                //this.toolStrip.RenderMode = ToolStripRenderMode.Custom;

                
                // Show newly created session.
                toolStripButton_Click(newButton, EventArgs.Empty);


            }

            // Those buttons no longer have corresponding sessions and must be removed.
            foreach(ToolStripButton button in sessionsButtons)
            {
                ((AnalyzerSessionControl)(button.Tag)).AnalyzerSession = null;
                ((AnalyzerSessionControl)(button.Tag)).Visible = false;
                ((AnalyzerSessionControl)(button.Tag)).Dispose();

                button.Click -= new EventHandler(toolStripButton_Click);
                toolStrip.Items.Remove(button);
            }

            // Make sure there is at least one visibile session if there are any.
            sessionsButtons = GetSessionsButtons();
            bool visibleSessionFound = false;
            foreach (ToolStripButton button in sessionsButtons)
            {
                if (((AnalyzerSessionControl)(button.Tag)).Visible)
                {
                    visibleSessionFound = true;
                    break;
                }
            }

            if (visibleSessionFound == false && sessionsButtons.Count > 0)
            {// Show the first available session.
                toolStripButton_Click(sessionsButtons[0], EventArgs.Empty);
            }
        }

        public override void UnInitializeControl()
        {
            if (tradeAnalyzer != null)
            {
                tradeAnalyzer.SessionManager.SessionsUpdateEvent -= new GeneralHelper.GenericDelegate<IAnalyzerSessionManager>(SessionManager_SessionUpdateEvent);
                tradeAnalyzer = null;
            }
        }

        List<string> aliveSessions = new List<string>();

        //bool isDeletingSession = false;
        private void toolStripButtonDeleteSession_Click(object sender, EventArgs e)
        {
            lock (this)
            {
                if (GetSessionsButtons().Count > 1 && aliveSessions.Count >1)
                {
                    foreach (ToolStripButton button in GetSessionsButtons())
                    {
                        if (button.Checked)
                        {
                            //aliveSessions.Remove(((AnalyzerSession)((AnalyzerSessionControl)(button.Tag)).Session).Symbol.ToString());

                            //DataProvider provider = (DataProvider)((AnalyzerSessionControl)(button.Tag)).AnalyzerSession.DataProvider;
                            //ProviderService.UnRegisterHandler(provider);
                            //aliveSessions.Remove(provider.Symbol.ToString());
                            //ProviderHandler ph = (ProviderHandler)AppClient.GetHandler(Protocol.M0003_1);
                            //if (ph.Provider.Symbol == provider.Symbol)
                            //{
                            //    ph.IsProcessing = false;                            
                            //    ph = null;
                            //}
                            //DataService.UnRegisterDataProvider(provider);
                            //tradeAnalyzer.SessionManager.DestroySession(((AnalyzerSessionControl)(button.Tag)).AnalyzerSession);
                    
                        }
                    }
                    Console.WriteLine("  GetSessionsButtons().Count  " + GetSessionsButtons().Count);
                    if (aliveSessions.Count <= 1)
                    {
                        Console.WriteLine("  GetSessionsButtons().Count  " + GetSessionsButtons().Count);
                        closeToolStripButton.Enabled = false;
                    }
                }
                else
                {
                    closeToolStripButton.Enabled = false;
                }
            }            
        }

        private void toolStripButton_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton button in GetSessionsButtons())
            {
                if (button == sender)
                {
                    //设置Operate 按钮
                    ControlButton.Tag = button.Tag;
                    OpChartControl = ((AnalyzerSessionControl)(ControlButton.Tag)).ChartControl1;
                    asession = ((AnalyzerSessionControl)(ControlButton.Tag)).AnalyzerSession;
                    Symbol symbol = asession.Symbol;
                    OpToolBarStatus = ((AnalyzerSessionControl)(ControlButton.Tag)).ToolBarStatus;

                    button.Checked = true;
                    ((AnalyzerSessionControl)(button.Tag)).Visible = true;

                    if (signalSymbols.Contains(symbol))
                    {
                        signalSymbols.Remove(symbol);
                    }
                    button.BackColor = btnBOriColor;
                    button.ForeColor = btnFOriColor;
                    button.Font = btnOriFont;
                    //更改工具条的状态
                    ChangeToolBarStatus();
                 //   if(!AppContext.IsAppInitializing)
                        InvokeSignalListUpdate(symbol.ToString());

                }
            }

             //Iterate 2nd time for better UI transition.
            foreach (ToolStripButton button in GetSessionsButtons())
            {
                if (button != sender)
                {
                    button.Checked = false;
                    //((DataProvider)((AnalyzerSessionControl)(button.Tag)).Session.DataProvider).timerAuto.Stop(); 
                    ((AnalyzerSessionControl)(button.Tag)).Visible = false;
                }
            }
        }

        private void priceListView_ColumnWidthChanged()
        {
            priceListView.ColumnWidthChanged -= new ColumnWidthChangedEventHandler(priceListView_ColumnWidthChanged);
            columnHeader5.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            priceListView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(priceListView_ColumnWidthChanged);
        }

        private void priceListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            //Adjust Listview column width
            priceListView_ColumnWidthChanged();            
        }

        private void priceListView_SizeChanged(object sender, EventArgs e)
        {
            //Adjust Listview column width
            priceListView_ColumnWidthChanged();
        }

        private void priceListView_DoubleClick(object sender, EventArgs e)
        {
            AnalyzerSession session = null;
            SessionInfo seletedSession = SelectedSession.Value;         

            bool existed = false;

             foreach (ToolStripButton button in GetSessionsButtons())
             {
                 if (((AnalyzerSessionControl)(button.Tag)).AnalyzerSession.SessionInfo.Symbol == seletedSession.Symbol)
                 {
                     button.Checked = true;
                     existed = true;
                     ((AnalyzerSessionControl)(button.Tag)).Visible = true;
                     session =   ((AnalyzerSessionControl)(button.Tag)).AnalyzerSession;
                 }
                 else
                 {
                     ((AnalyzerSessionControl)(button.Tag)).Visible = false;
                 }
             }

             if (!existed)
             {
                 foreach (SessionInfo sin in FxpaSource.AvailableSessionList)
                 {
                     if (sin.Symbol == seletedSession.Symbol){
                         session = _host.CreateSimulationSession(seletedSession);
                         break;
                     }
                 }
             }            

            if (session == null)
            {
                MessageBox.Show("该商品没有被订阅。", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                foreach (ToolStripButton button in GetSessionsButtons())
                {
                    button.Checked = true;
                    ((AnalyzerSessionControl)(button.Tag)).Visible = true;
                }
            }
            else
            {
                closeToolStripButton.Enabled = true;
                if(!aliveSessions.Contains(session.Symbol.ToString()))
                     aliveSessions.Add(session.Symbol.ToString());
                UpdateUI();
                //GeneralHelper.SafeEventRaise(_host.SessionsUpdateEvent, _host);
                //SessionCreatedEvent();
            }
          }

        private void AutoResizeColumnWidth(ListView lv)
        {
            int count = lv.Columns.Count;
            int MaxWidth = 0;
            Graphics graphics = lv.CreateGraphics();
            Font font = lv.Font;
            ListView.ListViewItemCollection items = lv.Items;

            string str;
            int width;

            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            for (int i = 0; i < count; i++)
            {
                str = lv.Columns[i].Text;
                MaxWidth = lv.Columns[i].Width;

                foreach (ListViewItem item in items)
                {
                    if (item.SubItems.Count > i)
                    {
                        str = item.SubItems[i].Text;
                        width = (int)graphics.MeasureString(str, font).Width;
                        if (width > MaxWidth)
                        {
                            MaxWidth = width;
                        }
                    }
                }
                if (i == 0)
                {
                    lv.Columns[i].Width = lv.SmallImageList.ImageSize.Width + MaxWidth;
                }
                else if (i == 1)
                {
                    lv.Columns[i].Width = 60;
                }
                else
                {
                    lv.Columns[i].Width = MaxWidth;
                }
            }
        }

        private void toolStripButtonShowScrollbars_Click(object sender, EventArgs e)
        {
            OpChartControl.hScrollBar.Visible = toolStripButtonShowScrollbars.Checked;
            OpChartControl.vScrollBar.Visible = toolStripButtonShowScrollbars.Checked;
            ((AnalyzerSessionControl)(ControlButton.Tag)).ToolBarStatus.ScrollbarsStatus = toolStripButtonShowScrollbars.Checked;
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            OpChartControl.MainPane.ZoomIn(2);
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            OpChartControl.MainPane.ZoomOut(0.5f);
        }

        private void toolStripButtonFit_Click(object sender, EventArgs e)
        {
            OpChartControl.MainPane.FitDrawingSpaceToScreen(true, true);
            OpChartControl.MainPane.Invalidate();
        }

        private void toolStripButtonFitHorizontal_Click(object sender, EventArgs e)
        {
            OpChartControl.MainPane.FitDrawingSpaceToScreen(true, false);
            OpChartControl.MainPane.Invalidate();
        }

        private void toolStripButtonFitVertical_Click(object sender, EventArgs e)
        {
            OpChartControl.MainPane.FitDrawingSpaceToScreen(false, true, OpChartControl.MainPane.DrawingSpaceDisplayLimit);
            OpChartControl.MainPane.Invalidate();
        }

        private void toolStripButtonCrosshair_Click(object sender, EventArgs e)
        {
            OpChartControl.MainPane.CrosshairVisible = toolStripButtonCrosshair.Checked;
            OpChartControl.Refresh();
            _optoolBarStatus.ShowMouseStatus = toolStripButtonCrosshair.Checked;
            //this.OpToolBarStatus.ShowMouseStatus = toolStripButtonCrosshair.Checked;
        }

        private void toolStripButtonShowLabels_Click(object sender, EventArgs e)
        {
            OpChartControl.MainPane.ShowSeriesLabels = toolStripButtonShowLabels.Checked;
            OpChartControl.MainPane.Invalidate();
            _optoolBarStatus.ChartLabelsStatus = toolStripButtonShowLabels.Checked;
        }

        private void toolStripButtonShowTimeGaps_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ChartSeries series in OpChartControl.MainPane.Series)
            {
                if (series is TradeChartSeries)
                {
                    ((TradeChartSeries)series).ShowTimeGaps = toolStripButtonShowTimeGaps.Checked;
                }
            }
            OpChartControl.MainPane.Invalidate();
        }

        private void toolStripButtonLimitView_Click(object sender, EventArgs e)
        {
            OpChartControl.MainPane.LimitedView = toolStripButtonLimitView.Checked;
            _optoolBarStatus.LimitViewStatus = toolStripButtonLimitView.Checked;
        }

        private void toolStripButtonAutoScrollToEnd_Click(object sender, EventArgs e)
        {
            OpChartControl.MainPane.AutoScrollToEnd = toolStripButtonAutoScrollToEnd.Checked;
            _optoolBarStatus.AutoScrollStatus = toolStripButtonAutoScrollToEnd.Checked;
        }

        private void toolStripMenuItemMIN1_Click(object sender, EventArgs e)
        {
            if (asession != null)
            {
                ((DataProvider)asession.DataProvider).CurrentTInterval = Interval.MIN1;
                ((ToolStripMenuItem)sender).Checked = true;
                _optoolBarStatus.TimePeriod = Interval.MIN1;
                UpdateTimeIntervalButton(sender);
            }
        }

        private void toolStripMenuItemMIN3_Click(object sender, EventArgs e)
        {
            if (asession != null)
            {
                ((DataProvider)asession.DataProvider).CurrentTInterval = Interval.MIN3;
                ((ToolStripMenuItem)sender).Checked = true;
                _optoolBarStatus.TimePeriod = Interval.MIN3;
                UpdateTimeIntervalButton(sender);
            }
        }

        private void toolStripMenuItemMIN5_Click(object sender, EventArgs e)
        {
            if (asession != null)
            {
                ((DataProvider)asession.DataProvider).CurrentTInterval = Interval.MIN5;
                ((ToolStripMenuItem)sender).Checked = true;
                _optoolBarStatus.TimePeriod = Interval.MIN5;
                UpdateTimeIntervalButton(sender);
            }
        }

        private void toolStripMenuItemMIN15_Click(object sender, EventArgs e)
        {
            if (asession != null)
            {
                ((DataProvider)asession.DataProvider).CurrentTInterval = Interval.MIN15;
                ((ToolStripMenuItem)sender).Checked = true;
                _optoolBarStatus.TimePeriod = Interval.MIN15;
                UpdateTimeIntervalButton(sender);
            }
        }

        private void toolStripMenuItemMIN30_Click(object sender, EventArgs e)
        {
            if (asession != null)
            {
                ((DataProvider)asession.DataProvider).CurrentTInterval = Interval.MIN30;
                ((ToolStripMenuItem)sender).Checked = true;
                _optoolBarStatus.TimePeriod = Interval.MIN30;
                UpdateTimeIntervalButton(sender);
            }
        }

        private void toolStripMenuItemMIN60_Click(object sender, EventArgs e)
        {
            if (asession != null)
            {
                ((DataProvider)asession.DataProvider).CurrentTInterval = Interval.MIN60;
                ((ToolStripMenuItem)sender).Checked = true;
                _optoolBarStatus.TimePeriod = Interval.MIN60;
                UpdateTimeIntervalButton(sender);
            }
        }

        private void toolStripMenuItemMIN120_Click(object sender, EventArgs e)
        {
            if (asession != null)
            {
                ((DataProvider)asession.DataProvider).CurrentTInterval = Interval.MIN120;
                ((ToolStripMenuItem)sender).Checked = true;
                _optoolBarStatus.TimePeriod = Interval.MIN120;
                UpdateTimeIntervalButton(sender);
            }
        }

        private void toolStripMenuItemDAY1_Click(object sender, EventArgs e)
        {
            if (asession != null)
            {
                ((DataProvider)asession.DataProvider).CurrentTInterval = Interval.DAY1;
                ((ToolStripMenuItem)sender).Checked = true;
                _optoolBarStatus.TimePeriod = Interval.DAY1;
                UpdateTimeIntervalButton(sender);
            }
        }

        private void UpdateTimeIntervalButton(object sender)
        {
            if (asession != null)
            {
                ((DataProvider)asession.DataProvider).timerAuto.Stop();
                ((DataProvider)asession.DataProvider).SwitchTimeInterval();
                ((DataProvider)asession.DataProvider).Execute(null, null);
                foreach (Object item in toolStripDropDownButton1.DropDownItems)
                {
                    if (item is ToolStripMenuItem && item != sender)
                        ((ToolStripMenuItem)item).Checked = false;
                }
                ((DataProvider)asession.DataProvider).timerAuto.Start();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (asession != null)
            {
                AnalyzerSessionIndicatorsControl control = new AnalyzerSessionIndicatorsControl(asession, OpChartControl.Panes);
                control.AddIndicatorEvent += new AnalyzerSessionIndicatorsControl.AddIndicatorDelegate(((AnalyzerSessionControl)(ControlButton.Tag)).control_AddIndicatorEvent);
                HostingForm form = new HostingForm("Session " + asession.SessionInfo.Id + " Indicators", control);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.ShowDialog();
            }
        }

        private void ChangeToolBarStatus()
        {
            toolStripButtonShowScrollbars.Checked = OpToolBarStatus.ScrollbarsStatus;
            toolStripButtonCrosshair.Checked = OpToolBarStatus.ShowMouseStatus;
            toolStripButtonShowLabels.Checked = OpToolBarStatus.ChartLabelsStatus;
            toolStripButtonLimitView.Checked = OpToolBarStatus.LimitViewStatus;
            toolStripButtonAutoScrollToEnd.Checked = OpToolBarStatus.AutoScrollStatus;
            //toolStripButtonPause.Checked = OpToolBarStatus.IsPause;

            foreach (Object item in toolStripDropDownButton1.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                {
                    ((ToolStripMenuItem)item).Checked = false;
                }
            }

            if (OpToolBarStatus.TimePeriod == Interval.MIN1)
            {
                toolStripMenuItemMIN1.Checked = true;
            }
            else if (OpToolBarStatus.TimePeriod == Interval.MIN3)
            {
                toolStripMenuItemMIN3.Checked = true;
            }
            else if (OpToolBarStatus.TimePeriod == Interval.MIN5)
            {
                toolStripMenuItemMIN5.Checked = true;
            }
            else if (OpToolBarStatus.TimePeriod == Interval.MIN15)
            {
                toolStripMenuItemMIN15.Checked = true;
            }
            else if (OpToolBarStatus.TimePeriod == Interval.MIN30)
            {
                toolStripMenuItemMIN30.Checked = true;
            }
            else if (OpToolBarStatus.TimePeriod == Interval.MIN60)
            {
                toolStripMenuItemMIN60.Checked = true;
            }
            else if (OpToolBarStatus.TimePeriod == Interval.MIN120)
            {
                toolStripMenuItemMIN120.Checked = true;
            }
            else if (OpToolBarStatus.TimePeriod == Interval.DAY1)
            {
                toolStripMenuItemDAY1.Checked = true;
            }
        }

    delegate  void  SignalListViewUpateDelegate(object obj ); 

        private void priceListView_Click(object sender, EventArgs e)
        {
            
          string signal=  ((DoubleBufferListView)sender).SelectedItems[0].SubItems[2].Text;
          int num;
          //if ( ( num= AppUtil.ParseSignalNum(signal)) != AppUtil.PARSE_ERROR && num >0)
          //{
              SessionInfo seletedSession = SelectedSession.Value;
              InvokeSignalListUpdate(seletedSession.Symbol);
          //}   
        }

        private void InvokeSignalListUpdate(string symbol)
        {
            List<Signal> datas = new List<Signal>();

            lock (AppConst.SignalDatasLocker)
            {
                foreach (Signal s in AppContext.SignalDatas)
                {
                    if (s.Symbol.ToString() == symbol)
                        datas.Add(s);
                }
            }

                SignalListViewUpateDelegate d = new SignalListViewUpateDelegate(UpdateSignalListView);
                AppContext.SignalListView.BeginInvoke(d, datas);
                SignalListViewUpateDelegate dd = new SignalListViewUpateDelegate(UpdateStatListView);
                AppContext.StatListView.BeginInvoke(dd, datas);

        }

        private void UpdateSignalListView(object obj)
        {
            List<Signal> datas = ((List<Signal>)obj);
            if (datas.Count > 0)
            {
                Signal[] signals = null;
                lock (AppConst.SignalDatasLocker)
                {
                    signals = new Signal[datas.Count];
                    datas.CopyTo(signals, 0);
                    Array.Sort(signals);
                }
                lock (AppConst.SignalListLocker)
                {
                    AppContext.SignalListView.Items.Clear();
                    foreach (Signal signal in signals)
                    {
                        string strSignal = AppUtil.GetSignalChinese(signal.Arrow);
                        ListViewItem item = null; int arrow = signal.Arrow == -1 ? 0 : 1;
                        if (signal.StopGainPrice > 0)
                            item = new ListViewItem(
                            new string[] {signal.Symbol.ToString(), 
                          signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), signal.StopLossPrice.ToString(), signal.StopGainPrice.ToString()  }, arrow);
                        else
                            item = new ListViewItem(new string[] {signal.Symbol.ToString(), 
                          signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), signal.StopLossPrice.ToString(),"" }, arrow);
                        if (signal.ProfitPrice == 0)
                        {
                            item.BackColor = Color.MediumPurple;
                            int count = item.SubItems.Count - 1;
                            while (count >= 1)
                            {
                                item.SubItems[count].BackColor = Color.MediumPurple;
                                count--;
                            }
                        }
                        AppContext.SignalListView.Items.Add(item);
                    }
                }
            }
            else
            {
                AppContext.SignalListView.Items.Clear();
            }
        }

        private void UpdateStatListView(object obj)
        {

            List<Signal> datas = ((List<Signal>)obj);
            if (datas.Count > 0)
            {
                Signal[] signals = null;
                lock (AppConst.SignalDatasLocker)
                {
                    signals = new Signal[datas.Count];
                    datas.CopyTo(signals, 0);
                    Array.Sort(signals);
                }

                lock (AppConst.StatListLocker)
                {
                    AppContext.StatListView.Items.Clear();
                    foreach (Signal signal in signals)
                    {
                        string strSignal = AppUtil.GetSignalChinese(signal.Arrow);
                        ListViewItem item = null; int arrow = signal.Arrow == -1 ? 0 : 1;
                        if (signal.ProfitPrice > 0)
                            item = new ListViewItem(new string[] { signal.Symbol.ToString(), signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), signal.ProfitTime.ToString(), signal.ProfitPrice.ToString(), signal.Profit.ToString() }, arrow);
                        else
                            item = new ListViewItem(new string[] { signal.Symbol.ToString(), signal.ActPrice.ToString(), strSignal, signal.ActTime.ToString(), "", "", "" }, arrow);

                        if (signal.ProfitPrice <= 0)
                        {
                            item.BackColor = Color.MediumPurple;
                            int count = item.SubItems.Count - 1;
                            while (count >= 1)
                            {
                                item.SubItems[count].BackColor = Color.MediumPurple;
                                count--;
                            }
                        }
                        AppContext.StatListView.Items.Add(item);
                    }
                }
            }
            else
            {
                AppContext.StatListView.Items.Clear();
            }
        }

        bool isPause = false;

        public bool IsPause
        {
            get { return isPause; }
            set { isPause = value; }
        }

        private void toolStripButtonPause_Click(object sender, EventArgs e)
        {
            isPause = !isPause;
            Console.WriteLine(" mmm isPause   " + isPause);
            foreach (ToolStripButton button in GetSessionsButtons())
            {
                ((DataProvider)((AnalyzerSessionControl)(button.Tag)).AnalyzerSession.DataProvider).IsPause = isPause;
            }
        }

        private void priceListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void speakerToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.speakerToolStripButton.Text == "无声")
            {
                this.speakerToolStripButton.Image = global::fxpa.Properties.Resources.sound_none;
                this.speakerToolStripButton.Text = "有声";
                this.speakerToolStripButton.Invalidate();
                AppContext.IsOpenSpeaker = false;
            }
            else
            {
                this.speakerToolStripButton.Image = global::fxpa.Properties.Resources.sound1;
                this.speakerToolStripButton.Text = "无声";
                this.speakerToolStripButton.Invalidate();
                AppContext.IsOpenSpeaker = true;
            }
        }

        private void dayStatBtn_Click(object sender, EventArgs e)
        {
            DayStatBtn.Enabled = false;
            TriDayStatBtn.Enabled = false;
            WeekStatBtn.Enabled = false;
            //AppContext.SignalListHandler.GetSignalsByTime("0");
        }

        private void triDayStatBtn_Click(object sender, EventArgs e)
        {
            DayStatBtn.Enabled = false;
            TriDayStatBtn.Enabled = false;
            WeekStatBtn.Enabled = false;
            //AppContext.SignalListHandler.GetSignalsByTime("1");
        }

        private void weekStatBtn_Click(object sender, EventArgs e)
        {
            DayStatBtn.Enabled = false;
            TriDayStatBtn.Enabled = false;
            WeekStatBtn.Enabled = false;
            //AppContext.SignalListHandler.GetSignalsByTime("2");
        }


        public System.Windows.Forms.Button DayStatBtn
        {
            get { return dayStatBtn; }
            set { dayStatBtn = value; }
        }


        public System.Windows.Forms.Button WeekStatBtn
        {
            get { return weekStatBtn; }
            set { weekStatBtn = value; }
        }


        public System.Windows.Forms.Button TriDayStatBtn
        {
            get { return triDayStatBtn; }
            set { triDayStatBtn = value; }
        }

        public System.Windows.Forms.Label TimeLabel
        {
            get { return timeLabel; }
            set { timeLabel = value; }
        }
    }
}
