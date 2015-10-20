// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Drawing;
using System.Windows.Forms;

namespace fxpa
{
    public partial class AnalyzerSessionControl : FxpaCommonControl
    {
        ChartControl chartControl1;
        public ChartControl ChartControl1
        {
            get { return chartControl1; }
            set { chartControl1 = value; }
        }

        ToolBarButtonStatus toolBarStatus = new ToolBarButtonStatus();
        public ToolBarButtonStatus ToolBarStatus
        {
            get { return toolBarStatus; }
            set { toolBarStatus = value; }
        }

        AnalyzerSession expertSession;
        public AnalyzerSession AnalyzerSession
        {
            get 
            { 
                return expertSession;
            }

            set 
            {
                if (value == expertSession)
                {
                    return;
                }

                if (expertSession != null)
                {
                    if (expertSession.DataProvider != null)
                    {
                        expertSession.DataProvider.ValuesUpdateEvent -= new ValuesUpdatedDelegate(DataProvider_ValuesUpdateEvent);
                    }
                    
                    expertSession.IndicatorAddedEvent -= new AnalyzerSession.IndicatorUpdateDelegate(_session_IndicatorAddedEvent);
                    expertSession.IndicatorRemovedEvent -= new AnalyzerSession.IndicatorUpdateDelegate(_session_IndicatorRemovedEvent);
                }

                expertSession = value;
                this.chartControl.MainPane.Clear(true, true);

                if (expertSession != null)
                {
                    TradeChartSeries series = new TradeChartSeries(expertSession.SessionInfo.Id);
                    series.ChartType = TradeChartSeries.ChartTypeEnum.CandleStick;

                    //this.Invoke(new GeneralHelper.DefaultDelegate(UpdateMasterChartName));

                    series.Initialize(expertSession.DataProvider);

                    this.chartControl.MainPane.Add(series, false, false);

                    expertSession.DataProvider.ValuesUpdateEvent += new ValuesUpdatedDelegate(DataProvider_ValuesUpdateEvent);

                    expertSession.IndicatorAddedEvent += new AnalyzerSession.IndicatorUpdateDelegate(_session_IndicatorAddedEvent);
                    expertSession.IndicatorRemovedEvent += new AnalyzerSession.IndicatorUpdateDelegate(_session_IndicatorRemovedEvent);

                    if (expertSession.DataProvider.DataUnitCount > 0)
                    {
                        this.BeginInvoke(new GeneralHelper.GenericDelegate<bool, bool>(chartControl.MainPane.FitDrawingSpaceToScreen), true, true);
                        chartControl.Invalidate();
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public AnalyzerSessionControl()
        {
            InitializeComponent();
            ChartControl1 = this.chartControl;
            this.dataUpdateLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.dataUpdateLabel.Invalidate();
        }

        private void process(object source, System.Timers.ElapsedEventArgs e)
        {
            ((DataProvider)expertSession.DataProvider).StepForward();
        }

        private void AnalyzerSessionControl_Load(object sender, EventArgs e)
        {
            this.dataUpdateLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataUpdateLabel.Invalidate();
        }

        public override void UnInitializeControl()
        {
            base.UnInitializeControl();
            AnalyzerSession = null;
        }

        void _session_IndicatorRemovedEvent(AnalyzerSession session, Indicator indicator)
        {
            foreach (ChartPane pane in chartControl.Panes)
            {
                foreach (ChartSeries series in pane.Series)
                {
                    if (series is IndicatorChartSeries && ((IndicatorChartSeries)series).Indicator == indicator)
                    {
                        pane.Remove(series);
                        if (pane.Series.Length == 0)
                        {
                            chartControl.RemoveSlavePane((SubChartPane)pane);
                        }
                        return;
                    }
                }
            }
        }

        void _session_IndicatorAddedEvent(AnalyzerSession session, Indicator indicator)
        {
        }

        /// <summary>
        /// Non UI.
        /// </summary>
        //void UpdateMasterChartName()
        //{
        //    if (_session == null || _session.DataProvider == null)
        //    {
        //        return;
        //    }

        //    string name = _session.SessionInfo.Symbol + "[M" + _session.SessionInfo.TimeInterval.TotalMinutes.ToString() + "]";
        //    if (_session.DataProvider != null  )//&& _session.DataProvider.OperationalState == OperationalStateEnum.Operational)
        //    {
        //        name += ", " + _session.DataProvider.Bid + " / " + _session.DataProvider.Ask;
        //    }

        //    if (_session.OperationalState != OperationalStateEnum.Operational)
        //    {
        //        Name = name + ", " + _session.OperationalState.ToString();
        //    }

        //    this.BeginInvoke(new GeneralHelper.GenericDelegate<string>(chartControl.MainPane.SetChartName), name);
        //}

        void DataProvider_ValuesUpdateEvent(IDataProvider dataProvider, UpdateType updateType, int updatedItemsCount, int stepsRemaining)
        {
            //UpdateMasterChartName();

            if (((DataProvider)dataProvider).Initialized == true)
            {
                this.BeginInvoke(new GeneralHelper.GenericDelegate<bool>(this.UpdateLableDataUpdating), false);
            }

            if (updateType == UpdateType.Initial)
            {// Only executes on initial adding of many items.
                this.BeginInvoke(new GeneralHelper.GenericDelegate<bool, bool>(chartControl.MainPane.FitDrawingSpaceToScreen), true, true);
            }

            chartControl.MainPane.Invalidate();
        }

        public void UpdateLableDataUpdating(bool v)
        {
            this.dataUpdateLabel.Visible = v;
        }

        public void control_AddIndicatorEvent(BasicIndicator indicator, ChartPane pane)
        {
            IndicatorChartSeries series = new IndicatorChartSeries(indicator.Name, indicator);

            if (pane == null)
            {
                pane = chartControl.CreateSlavePane(indicator.Name, SubChartPane.MainChartPaneSyncModeEnum.XAxis, this.Height / 4);
                pane.RightMouseButtonSelectionMode = ChartPane.SelectionModeEnum.HorizontalZoom;
                pane.Add(series);
            }
            else
            {
                pane.Add(series);
            }
        }
    }

    public class ToolBarButtonStatus
    {
        //Status bar
        bool _scrollbarsStatus;
        public bool ScrollbarsStatus
        {
            get { return _scrollbarsStatus; }
            set { _scrollbarsStatus = value; }
        }

        // Show cross 
        bool showMouseStatus;
        public bool ShowMouseStatus
        {
            get { return showMouseStatus; }
            set { showMouseStatus = value; }
        }

        //Show labels
        bool chartLabelsStatus;
        public bool ChartLabelsStatus
        {
            get { return chartLabelsStatus; }
            set { chartLabelsStatus = value; }
        }

        // Limit View
        bool _limitViewStatus;
        public bool LimitViewStatus
        {
            get { return _limitViewStatus; }
            set { _limitViewStatus = value; }
        }

        //¹öµ½×îºó×´Ì¬
        bool _autoScrollStatus;
        public bool AutoScrollStatus
        {
            get { return _autoScrollStatus; }
            set { _autoScrollStatus = value; }
        }

        //Interval 
        Interval _timePeriod;
        public Interval TimePeriod
        {
            get { return _timePeriod; }
            set { _timePeriod = value; }
        }

        public ToolBarButtonStatus()
        {
            _scrollbarsStatus = false;
            showMouseStatus = false;
            chartLabelsStatus = false;
            _limitViewStatus = false;
            _autoScrollStatus = false;
            _timePeriod = Interval.MIN1;
        }
    }
}
