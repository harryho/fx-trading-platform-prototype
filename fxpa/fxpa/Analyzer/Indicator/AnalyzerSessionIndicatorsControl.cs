// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Reflection;

using System.Drawing;

namespace fxpa
{
    public partial class AnalyzerSessionIndicatorsControl : UserControl
    {
        // Indicator whaiting to be initialized and used.
        BasicIndicator _pendingIndicator;

        AnalyzerSession _session;

        IEnumerable<ChartPane> _chartPanes;

        public delegate void AddIndicatorDelegate(BasicIndicator indicator, ChartPane pane);
        /// <summary>
        /// This will be raised to notify owner the user selected to create the indicator in the given pane.
        /// pane will be null to specify a new pane needs to be created.
        /// </summary>
        public event AddIndicatorDelegate AddIndicatorEvent;
        
        /// <summary>
        /// 
        /// </summary>
        public AnalyzerSessionIndicatorsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public AnalyzerSessionIndicatorsControl(AnalyzerSession session, IEnumerable<ChartPane> chartPanes)
        {
            InitializeComponent();

            _session = session;
            _chartPanes = chartPanes;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            listViewIndicators.Clear();
            listViewIndicatorTypes.Clear();
            this.indicatorControl1.Indicator = null;
            _session = null;
        }

        private void AnalyzerSessionIndicatorsControl_Load(object sender, EventArgs e)
        {
            string[] groupsNames = Enum.GetNames(typeof(FxpaIndicatorManager.IndicatorGroups));

            foreach (string groupName in groupsNames)
            {
                listViewIndicatorTypes.Groups.Add(groupName, groupName + " Group");
            }

            string[] names = FxpaIndicatorManager.Instance.GetIndicatorsNames(FxpaIndicatorManager.IndicatorGroups.Fxpa);
            string[] descriptions = FxpaIndicatorManager.Instance.GetIndicatorsDescriptions(FxpaIndicatorManager.IndicatorGroups.Fxpa);

            for (int i = 0; i < names.Length; i++)
            {
                ListViewItem item = new ListViewItem(names[i] + ", " + descriptions[i]);
                item.Tag = names[i];
                item.Group = listViewIndicatorTypes.Groups[FxpaIndicatorManager.IndicatorGroups.Fxpa.ToString()];
                listViewIndicatorTypes.Items.Add(item);
            }

            UpdateUI();
        }

        void UpdateUI()
        {
            listViewIndicators.Items.Clear();
            foreach (Indicator indicator in _session.Indicators)
            {
                listViewIndicators.Items.Add(indicator.Name).Tag = indicator;
            }

            buttonNew.Enabled = _pendingIndicator != null;

            comboBoxChartAreas.Items.Clear();
            comboBoxChartAreas.Items.Add("New Chart Area");
            foreach (ChartPane pane in _chartPanes)
            {
                comboBoxChartAreas.Items.Add(pane.Name);
            }
         
            comboBoxChartAreas.SelectedIndex = 0;
            if (listViewIndicatorTypes.SelectedIndices.Count > 0)
            {
                bool? isScaledToQuotes = FxpaIndicatorManager.Instance.GetIndicatorCloneByName(FxpaIndicatorManager.IndicatorGroups.Fxpa, listViewIndicatorTypes.SelectedItems[0].Tag as string).ScaledToQuotes;
                if (isScaledToQuotes.HasValue && isScaledToQuotes.Value)
                {// Since indicator is scaled to quotes, default its pane selection to main pane.
                    comboBoxChartAreas.SelectedIndex = 0;
                }
            }

            if (_pendingIndicator != null && _pendingIndicator.ScaledToQuotes.HasValue && _pendingIndicator.ScaledToQuotes.Value)
            {// By default, scaled to quotes indicators are to be shown in main pane.
                comboBoxChartAreas.SelectedIndex = 1;
            }
        }

        private void listViewIndicators_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewIndicators.SelectedItems.Count > 0)
            {
                indicatorControl1.IsReadOnly = false;
                BasicIndicator indicator = (BasicIndicator)_session.Indicators[listViewIndicators.SelectedItems[0].Index];
                this.indicatorControl1.Indicator = indicator;
            }
            else
            {
                indicatorControl1.Indicator = null;
            }
        }

        private void listViewIndicatorTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewIndicatorTypes.SelectedItems.Count > 0)
            {
                string name = listViewIndicatorTypes.SelectedItems[0].Tag as string;

                _pendingIndicator = (BasicIndicator)FxpaIndicatorManager.Instance.GetIndicatorCloneByName(FxpaIndicatorManager.IndicatorGroups.Fxpa, name);

                foreach (string setName in GeneralHelper.EnumerableToArray<string>(_pendingIndicator.UI.OutputResultSetsPens.Keys))
                {
                    _pendingIndicator.UI.OutputResultSetsPens[setName] = Pens.WhiteSmoke;
                }

                indicatorControl1.IsReadOnly = true;
                indicatorControl1.Indicator = _pendingIndicator;
            }
            else
            {
                indicatorControl1.Indicator = null;
                _pendingIndicator = null;
            }

            UpdateUI();
        }

        ChartPane GetSelectedPane()
        {
            if (comboBoxChartAreas.SelectedIndex == 0)
            {// 0 position reserved for new pane.
                return null;
            }

            int requiredIndex = comboBoxChartAreas.SelectedIndex - 1;

            int currentPaneIndex = 0;
            foreach(ChartPane pane in _chartPanes)
            {
            //    if ((_pendingIndicator.IsScaledToQuotes.HasValue 
            //        && _pendingIndicator.IsScaledToQuotes.Value) || pane is SlaveChartPane)
            //    {
                    if (requiredIndex == currentPaneIndex)
                    {
                        return pane;
                    }
                    currentPaneIndex++;
            //    }
            }

            //SystemMonitor.Throw("Unexpected, corresponding chart pane not found.");
            return null;
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            if (listViewIndicatorTypes.SelectedItems.Count == 0 || _pendingIndicator == null)
            {
                return;
            }
            
            _pendingIndicator.Initialize(_session.DataProvider);
            _session.Add(_pendingIndicator);

            if (AddIndicatorEvent != null)
            {
                AddIndicatorEvent(_pendingIndicator, GetSelectedPane());
            }

            UpdateUI();

            // Select the newly created indicator.
            this.listViewIndicators.Items[listViewIndicators.Items.Count - 1].Selected = true;
            this.listViewIndicators.Focus();

            _pendingIndicator = null;
        
            // Invoke the creation of a new _pendingIndicator
            //listViewIndicatorTypes_SelectedIndexChanged(sender, e);
            
            // Select the newly created indicator.
            if (listViewIndicators.Items.Count > 0)
            {
                listViewIndicators.Items[listViewIndicators.Items.Count - 1].Selected = true;
            }

            listViewIndicators.Focus();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listViewIndicators.SelectedIndices.Count > 0)
            {
                _session.Remove((Indicator)listViewIndicators.SelectedItems[0].Tag);
            }

            UpdateUI();
        }

        private void listViewIndicatorTypes_DoubleClick(object sender, EventArgs e)
        {
            buttonNew_Click(sender, e);
        }

        private void AnalyzerSessionIndicatorsControl_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        
    }
}
