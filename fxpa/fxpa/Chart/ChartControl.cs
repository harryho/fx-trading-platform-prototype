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
using System.Collections.ObjectModel;
using System.Threading;

namespace fxpa
{
    public partial class ChartControl : UserControl
    {
        List<ChartPane> _panes = new List<ChartPane>();

        /// <summary>
        /// All panes, including the Main Pane.
        /// </summary>
        public ReadOnlyCollection<ChartPane> Panes
        {
            get { return _panes.AsReadOnly(); }
        }

        /// <summary>
        /// This helps all panes have the same margin on the left. 
        /// The current implementation only allows the margin to grow, and not shrink back.
        /// </summary>
        int _requiredActualDrawingAreaLeftMargin = 0;

        /// <summary>
        /// The main (master) pane.
        /// </summary>
        public MainChartPane MainPane
        {
            get { lock (this) { return (MainChartPane)_panes[0]; } }
        }

        protected Color DrawingColor
        {
            //get { return toolStripButtonColor.BackColor; }
            get { return Color.Red; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ChartControl()
        {
            InitializeComponent();

            _panes.Add(mainChartPane);

            //masterPane.chartControl = this;
            mainChartPane.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            mainChartPane.Name = "MainChart Pane";
            mainChartPane.DrawingSpaceViewTransformationChangedEvent += new ChartPane.DrawingSpaceViewTransformationChangedDelegate(graphicPane_DrawingSpaceViewTransformationChangedEvent);
            mainChartPane.DrawingSpaceUpdatedEvent += new ChartPane.DrawingSpaceUpdatedDelegate(graphicPane_DrawingSpaceUpdatedEvent);
            mainChartPane.ActualDrawingSpaceMarginLeftUpdateEvent += new MainChartPane.ActualDrawingSpaceMarginLeftUpdateDelegate(masterPane_ActualDrawingSpaceMarginLeftUpdateEvent);
            //masterPane.CustomObjectsManager.DynamicObjectBuiltEvent += new CustomObjectsManager.DynamicObjectBuiltDelegate(CustomObjectsManager_DynamicObjectBuiltEvent);
            mainChartPane.ParametersUpdatedEvent += new ChartPane.ParametersUpdatedDelegate(masterPane_ParametersUpdatedEvent);

        }

        public void UpdateTimeLabel(object obj, int x, int y)
        {

            //timeLabel.Location = new Point(x, y);
            //timeLabel.Visible = true;
            //timeLabel.Text = (string)obj;
            ////timeLabel.Visible = true;

            ////this.Controls.Add(timeLabel);

            //timeLabel.Refresh();

        }

        public void Clear()
        {
            _requiredActualDrawingAreaLeftMargin = 0;
            foreach (ChartPane pane in _panes)
            {
                pane.Clear(true, true);
            }
        }

        public ChartPane CreateSlavePane(string chartName, SubChartPane.MainChartPaneSyncModeEnum masterSynchronizationMode, int height)
        {
            SubChartPane pane = new SubChartPane();
            _panes.Add(pane);
            pane.Name = "Slave Pane[" + chartName + "]" ;
            pane.ChartName = chartName;

            pane.Dock = DockStyle.Bottom;
            pane.MainChartPaneSynchronizationMode = masterSynchronizationMode;
            pane.ActualDrawingSpaceMarginLeftUpdateEvent += new SubChartPane.ActualDrawingSpaceMarginLeftUpdateDelegate(masterPane_ActualDrawingSpaceMarginLeftUpdateEvent);

            pane.XAxisLabelsFontBrush = null;

            Splitter splitter = new Splitter();
            splitter.Height = 4;
            splitter.Dock = DockStyle.Bottom;
            this.Controls.Add(splitter);
            splitter.SendToBack();

            pane.Tag = splitter;

            this.Controls.Add(pane);
            pane.SendToBack();
            pane.Height = height;
            pane.MainChartPane = MainPane;

            hScrollBar.SendToBack();
            vScrollBar.SendToBack();

            //this.toolStripDynamicObjects.SendToBack();
            //this.toolStripMain.SendToBack();

            return pane;
        }

        int masterPane_ActualDrawingSpaceMarginLeftUpdateEvent(ChartPane pane, int currentValue)
        {
            _requiredActualDrawingAreaLeftMargin = Math.Max(_requiredActualDrawingAreaLeftMargin, currentValue);
            return _requiredActualDrawingAreaLeftMargin;
        }

        void masterPane_ParametersUpdatedEvent(ChartPane pane)
        {
            if (this.IsHandleCreated == false)
            {
                return;
            }
            UpdateMainChartPaneToolbar();
        }

        void CheckToolStripDropDownButtonItem(ToolStripDropDownButton button, int index)
        {
            for (int i = 0; i < button.DropDownItems.Count; i++)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)button.DropDownItems[i];
                item.CheckState = CheckState.Unchecked;
                if (i == index)
                {
                    item.CheckState = CheckState.Checked;
                }
            }
        }

        void UpdateMainChartPaneToolbar()
        {
            //toolStripComboBoxRightMouseButtonSelectionMode.SelectedIndex = (int)masterPane.RightMouseButtonSelectionMode;
            //CheckToolStripDropDownButtonItem(toolStripDropDownButtonSelectionMode, (int)masterPane.RightMouseButtonSelectionMode);
            //toolStripComboBoxAppearance.SelectedIndex = (int)masterPane.AppearanceScheme;
            //CheckToolStripDropDownButtonItem(toolStripDropDownButtonAppearance, (int)masterPane.AppearanceScheme);
            //toolStripComboBoxScrollMode.SelectedIndex = (int)masterPane.ScrollMode;
            //CheckToolStripDropDownButtonItem(toolStripDropDownButtonScrollMode, (int)masterPane.ScrollMode);

            //toolStripButtonCrosshair.Checked = masterPane.CrosshairVisible;
            //toolStripButtonShowLabels.Checked = masterPane.ShowSeriesLabels;
            //toolStripButtonLimitView.Checked = masterPane.LimitedView;
        }

        /// <summary>
        /// This will not remove the main pane, since it is mandatory.
        /// </summary>
        public bool RemoveSlavePane(SubChartPane pane)
        {
            if (_panes.Remove(pane))
            {
                pane.ActualDrawingSpaceMarginLeftUpdateEvent -= new SubChartPane.ActualDrawingSpaceMarginLeftUpdateDelegate(masterPane_ActualDrawingSpaceMarginLeftUpdateEvent);
                pane.MainChartPane = null;
                pane.Clear(true, true);
                
                this.Controls.Remove((Splitter)pane.Tag);
                pane.Tag = null;

                pane.Parent = null;

                _requiredActualDrawingAreaLeftMargin = 0;
                return true;
            }
            return false;
        }

        public ChartPane GetPaneByName(string paneName)
        {
            foreach (ChartPane pane in _panes)
            {
                if (pane.Name == paneName)
                {
                    return pane;
                }
            }

            return null;
        }

        private void GraphicControl_Load(object sender, EventArgs e)
        {
            MainPane.FitDrawingSpaceToScreen(false, true);
            UpdateMainChartPaneToolbar();
        }

        void graphicPane_DrawingSpaceUpdatedEvent(ChartPane pane)
        {// Drawing space or Drawing space view was changed - update.

            RectangleF actualDrawingSpaceView = mainChartPane.GraphicsWrapper.ActualSpaceToDrawingSpace(mainChartPane.ActualDrawingSpaceArea);

            int width = (int)(mainChartPane.DrawingSpaceDisplayLimit.Width - actualDrawingSpaceView.Width);
            int height = (int)(mainChartPane.DrawingSpaceDisplayLimit.Height - actualDrawingSpaceView.Height);

            if (width > 0)
            {
                this.hScrollBar.Maximum = width;
                hScrollBar.LargeChange = hScrollBar.Maximum / 100 + 1;
                hScrollBar.SmallChange = hScrollBar.Maximum / 1000 + 1;
            }
            this.hScrollBar.Enabled = (width > 0);

            if (height > 0)
            {
                this.vScrollBar.Maximum = height;
                vScrollBar.LargeChange = vScrollBar.Maximum / 100 + 1;
                vScrollBar.SmallChange = vScrollBar.Maximum / 1000 + 1;
            }
            this.vScrollBar.Enabled = (height > 0);

            int xLocation = (int)(actualDrawingSpaceView.X - mainChartPane.DrawingSpaceDisplayLimit.X);
            int yLocation = (int)(actualDrawingSpaceView.Y - mainChartPane.DrawingSpaceDisplayLimit.Y);

            if (xLocation > 0)
            {
                hScrollBar.Value = Math.Min(hScrollBar.Maximum, xLocation);
            }
            else
            {
                hScrollBar.Value = 0;
            }

            if (yLocation > 0 && vScrollBar.Maximum - yLocation > 0)
            {
                // Y bars operate in the other faship - top is top
                vScrollBar.Value = Math.Min(vScrollBar.Maximum, vScrollBar.Maximum - yLocation);
            }
            else
            {
                vScrollBar.Value = 0;
            }

            hScrollBar.Refresh();
            vScrollBar.Refresh();

            // Also Update the series in the Save To File button.
            //UpdateSaveToFileUI();
        }

        void graphicPane_DrawingSpaceViewTransformationChangedEvent(ChartPane pane, System.Drawing.Drawing2D.Matrix previousTransformation, System.Drawing.Drawing2D.Matrix currentTransformation)
        {// Pane has changed its view.
            graphicPane_DrawingSpaceUpdatedEvent(pane);
            //toolStripLabelUnitUnification.Text = "Optimization: " + masterPane.CurrentUnitUnification;
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            RectangleF actualDrawingSpaceView = mainChartPane.GraphicsWrapper.ActualSpaceToDrawingSpace(mainChartPane.ActualDrawingSpaceArea);

            int yLocation = (int)(actualDrawingSpaceView.Y - mainChartPane.DrawingSpaceDisplayLimit.Y);
            int yValue = (vScrollBar.Value + yLocation) - vScrollBar.Maximum;

            MainPane.HandlePan(true, new PointF(0, yValue));
            MainPane.Invalidate();
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            RectangleF actualDrawingSpaceView = mainChartPane.GraphicsWrapper.ActualSpaceToDrawingSpace(mainChartPane.ActualDrawingSpaceArea);

            int xLocation = (int)(actualDrawingSpaceView.X - mainChartPane.DrawingSpaceDisplayLimit.X);
            int xValue = hScrollBar.Value - xLocation;
            MainPane.HandlePan(true, new PointF(-xValue, 0));
            MainPane.Invalidate();
        }
    }
}
