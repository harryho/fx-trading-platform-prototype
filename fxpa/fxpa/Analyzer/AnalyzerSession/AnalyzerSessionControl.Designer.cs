
using System.Drawing;
namespace fxpa
{
    partial class AnalyzerSessionControl
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
            this.chartControl = new fxpa.ChartControl();
            this.dataUpdateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chartControl
            // 
            this.chartControl.BackColor = System.Drawing.Color.Transparent;
            this.chartControl.BackgroundImage = global::fxpa.Properties.Resources.dataupdate;
            this.chartControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chartControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl.Location = new System.Drawing.Point(0, 0);
            this.chartControl.Margin = new System.Windows.Forms.Padding(2);
            this.chartControl.Name = "chartControl";
            this.chartControl.Size = new System.Drawing.Size(800, 480);
            this.chartControl.TabIndex = 9;
            // 
            // dataUpdateLabel
            // 
            if (AppContext.TradeAnalyzerControl == null)
                this.dataUpdateLabel.Visible = false;
            this.dataUpdateLabel.AutoSize = true;
            this.dataUpdateLabel.BackColor = Color.FromArgb(5, 41, 46);
            this.dataUpdateLabel.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataUpdateLabel.ForeColor = System.Drawing.Color.White;
            this.dataUpdateLabel.Location = new System.Drawing.Point(58, 11);
            this.dataUpdateLabel.Name = "labelDataUpdating";
            this.dataUpdateLabel.Size = new System.Drawing.Size(232, 40);
            this.dataUpdateLabel.TabIndex = 10;
            this.dataUpdateLabel.Text = "Data is loading…";
            this.dataUpdateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // 
            // AnalyzerSessionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataUpdateLabel);
            this.Controls.Add(this.chartControl);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AnalyzerSessionControl";
            this.Load += new System.EventHandler(this.AnalyzerSessionControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
           }

        #endregion

        private ChartControl chartControl;
        public System.Windows.Forms.Label dataUpdateLabel;

    }
}
