
namespace ForexPlatformFrontEnd
{
    partial class GraphicsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.graphControlZed1 = new ChartControl();
            this.SuspendLayout();
            // 
            // graphControlZed1
            // 
            this.graphControlZed1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControlZed1.Location = new System.Drawing.Point(0, 0);
            this.graphControlZed1.Name = "graphControlZed1";
            this.graphControlZed1.Size = new System.Drawing.Size(1014, 710);
            this.graphControlZed1.TabIndex = 0;
            // 
            // GraphicsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 710);
            this.Controls.Add(this.graphControlZed1);
            this.Name = "GraphicsForm";
            this.Text = "GraphicsForm";
            this.ResumeLayout(false);

        }

        #endregion

        protected ChartControl graphControlZed1;


    }
}