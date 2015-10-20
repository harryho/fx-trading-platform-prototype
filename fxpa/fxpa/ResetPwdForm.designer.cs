namespace fxpa
{
    partial class ResetPwdForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btncancel = new System.Windows.Forms.Button();
            this.textOldPwd = new System.Windows.Forms.TextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textConfirmPwd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textNewPwd = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panel1.Controls.Add(this.btncancel);
            this.panel1.Controls.Add(this.textOldPwd);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.textConfirmPwd);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textNewPwd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(313, 230);
            this.panel1.TabIndex = 12;
            // 
            // btncancel
            // 
            this.btncancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btncancel.Location = new System.Drawing.Point(191, 159);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(59, 24);
            this.btncancel.TabIndex = 5;
            this.btncancel.Text = "Cancel";
            this.btncancel.UseVisualStyleBackColor = true;
            // 
            // textOldPwd
            // 
            this.textOldPwd.Location = new System.Drawing.Point(102, 53);
            this.textOldPwd.Name = "textOldPwd";
            this.textOldPwd.PasswordChar = '*';
            this.textOldPwd.Size = new System.Drawing.Size(159, 20);
            this.textOldPwd.TabIndex = 0;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(107, 159);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(59, 24);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Confirm";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Old Passwod: ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::fxpa.Properties.Resources.sound;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(313, 28);
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            // 
            // textConfirmPwd
            // 
            this.textConfirmPwd.Location = new System.Drawing.Point(102, 126);
            this.textConfirmPwd.Name = "textConfirmPwd";
            this.textConfirmPwd.PasswordChar = '*';
            this.textConfirmPwd.Size = new System.Drawing.Size(159, 20);
            this.textConfirmPwd.TabIndex = 3;
            this.textConfirmPwd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textConfirmPwd_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "ConfirmPasswod: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "New Passwod: ";
            // 
            // textNewPwd
            // 
            this.textNewPwd.Location = new System.Drawing.Point(102, 87);
            this.textNewPwd.Name = "textNewPwd";
            this.textNewPwd.PasswordChar = '*';
            this.textNewPwd.Size = new System.Drawing.Size(159, 20);
            this.textNewPwd.TabIndex = 2;
            // 
            // ResetPwdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 230);
            this.Controls.Add(this.panel1);
            this.Icon = global::fxpa.Properties.Resources.fxpa_app;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(329, 268);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(329, 268);
            this.Name = "ResetPwdForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reset Password";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textOldPwd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textNewPwd;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox textConfirmPwd;

    }
}