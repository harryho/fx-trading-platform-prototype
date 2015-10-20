using System.Drawing;
using System.Windows.Forms;
namespace fxpa
{
    partial class ChartControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.mainChartPane = new fxpa.MainChartPane();
            //timeLabel = new System.Windows.Forms.Label();
            //priceLabel = new System.Windows.Forms.Label();
            //candleLabel = new DOATransparentLabel();
            this.SuspendLayout();
            // 
            // hScrollBar
            // 
            this.hScrollBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.Location = new System.Drawing.Point(0, 430);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(850, 20);
            this.hScrollBar.TabIndex = 2;
            this.hScrollBar.Visible = false;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(830, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(20, 430);
            this.vScrollBar.TabIndex = 3;
            this.vScrollBar.Visible = false;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            // 
            // masterPane
            // 
            //this.masterPane.AppearanceScheme = fxpa.ChartPane.AppearanceSchemeEnum.Default;
            this.mainChartPane.AppearanceScheme = fxpa.ChartPane.AppearanceSchemeEnum.DarkNatural;
            this.mainChartPane.AutoScrollToEnd = true;
            this.mainChartPane.AxisLabelsFont = new System.Drawing.Font("Tahoma", 8F);
            this.mainChartPane.BackColor = System.Drawing.Color.Black;
            this.mainChartPane.ChartName = "";
            this.mainChartPane.ConsiderAxisLabelsSpacingScale = true;
            this.mainChartPane.CrosshairVisible = false;
            this.mainChartPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainChartPane.LabelsFont = new System.Drawing.Font("Tahoma", 8F);
            this.mainChartPane.LabelsMargin = 10F;
            this.mainChartPane.LimitedView = true;
            this.mainChartPane.Location = new System.Drawing.Point(0, 0);
            this.mainChartPane.Margin = new System.Windows.Forms.Padding(2);
            this.mainChartPane.MaximumZoomEnabled = true;
            this.mainChartPane.Name = "masterPane";
            this.mainChartPane.RightMouseButtonSelectionMode = fxpa.ChartPane.SelectionModeEnum.HorizontalZoom;
            this.mainChartPane.ScrollMode = fxpa.ChartPane.ScrollModeEnum.HorizontalScroll;
            this.mainChartPane.SeriesItemMargin = 2F;
            this.mainChartPane.SeriesItemWidth = 12F;
            this.mainChartPane.ShowClippingRectangle = true;
            this.mainChartPane.ShowSeriesLabels = false;
            this.mainChartPane.Size = new System.Drawing.Size(850, 450);
            this.mainChartPane.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            this.mainChartPane.TabIndex = 0;
            this.mainChartPane.Text = "graphicPane1";
            this.mainChartPane.TitleFont = new System.Drawing.Font("Tahoma", 10F);
            this.mainChartPane.UnitUnificationOptimizationEnabled = true;
            this.mainChartPane.XAxisLabelsFormat = null;

            // 
            // ChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.Controls.Add(timeLabel);
            //this.Controls.Add(priceLabel);
            this.Controls.Add(this.mainChartPane);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ChartControl";
            this.Size = new System.Drawing.Size(850, 450);
            this.Load += new System.EventHandler(this.GraphicControl_Load);
            //this.ResumeLayout(false);
            this.ResumeLayout(true);
            this.PerformLayout();

        }

        #endregion

        private MainChartPane mainChartPane;
        public System.Windows.Forms.HScrollBar hScrollBar;
        public System.Windows.Forms.VScrollBar vScrollBar;
    }
}
