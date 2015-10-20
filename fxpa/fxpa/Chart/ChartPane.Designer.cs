using System.Windows.Forms;
using System.Drawing;
namespace fxpa
{
    partial class ChartPane
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
            this.timeLabel = new System.Windows.Forms.Label();
            this.priceLabel = new System.Windows.Forms.Label();
            this.candleLabel = new fxpa.DOATransparentLabel();
            this.SuspendLayout();
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.BackColor = System.Drawing.Color.Maroon;
            this.timeLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.timeLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.timeLabel.ForeColor = System.Drawing.Color.LightBlue;
            this.timeLabel.Location = new System.Drawing.Point(3, 438);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(0, 16);
            this.timeLabel.TabIndex = 1;
            this.timeLabel.UseCompatibleTextRendering = true;
            // 
            // priceLabel
            // 
            this.priceLabel.AutoSize = true;
            this.priceLabel.BackColor = System.Drawing.Color.DarkOrange;
            this.priceLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.priceLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.priceLabel.ForeColor = System.Drawing.Color.White;
            this.priceLabel.Location = new System.Drawing.Point(3, 438);
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size(0, 16);
            this.priceLabel.TabIndex = 1;
            this.priceLabel.UseCompatibleTextRendering = true;
            this.priceLabel.Visible = false;
            // 
            // candleLabel
            // 
            this.candleLabel.AutoSize = true;
            this.candleLabel.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.candleLabel.BackColor = System.Drawing.Color.LightGray;
            this.candleLabel.Caption = "";
            this.candleLabel.DimmedColor = System.Drawing.Color.Yellow;
            //this.candleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.candleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, FontStyle.Regular);
            this.candleLabel.ForeColor = System.Drawing.Color.LightYellow;
            this.candleLabel.Location = new System.Drawing.Point(3, 438);
            this.candleLabel.Margin = new System.Windows.Forms.Padding(4);
            this.candleLabel.Name = "candleLabel";
            this.candleLabel.Opacity = 60;
            this.candleLabel.Radius = 10;
            this.candleLabel.ShapeBorderStyle = fxpa.DOATransparentLabel.ShapeBorderStyles.ShapeBSFixedSingle;
            this.candleLabel.Size = new System.Drawing.Size(130, 100);
            this.candleLabel.TabIndex = 0;
            this.candleLabel.Visible = false;
            // 
            // ChartPane
            // 
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.candleLabel);
            //this.Controls.Add(this.dataUpdateLabel);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private  Label timeLabel;
        private  Label priceLabel;
        private  DOATransparentLabel candleLabel;
        //public   Label dataUpdateLabel;
    }
}
