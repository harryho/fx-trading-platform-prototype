namespace fxpa
{
    partial class IndicatorPropertiesControl
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
            this.labelDesription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelDesription
            // 
            this.labelDesription.AutoSize = true;
            this.labelDesription.Location = new System.Drawing.Point(2, 0);
            this.labelDesription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDesription.Name = "labelDesription";
            this.labelDesription.Size = new System.Drawing.Size(60, 13);
            this.labelDesription.TabIndex = 2;
            this.labelDesription.Text = "Description";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.Location = new System.Drawing.Point(2, 15);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.Size = new System.Drawing.Size(295, 62);
            this.textBoxDescription.TabIndex = 3;
            // 
            // IndicatorPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.labelDesription);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "IndicatorPropertiesControl";
            this.Size = new System.Drawing.Size(300, 488);
            this.Load += new System.EventHandler(this.IndicatorControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDescription;
        //private System.Windows.Forms.CheckBox checkBoxEnabled;
        private System.Windows.Forms.Label labelDesription;
    }
}
