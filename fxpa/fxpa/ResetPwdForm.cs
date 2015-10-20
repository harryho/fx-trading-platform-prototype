using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    public partial class ResetPwdForm : Form
    {
        ResetPwdHandler resetPwdHandler;
        public ResetPwdForm()
        {
            InitializeComponent();
            resetPwdHandler = new ResetPwdHandler(AppClient.GetInstance);
            resetPwdHandler.ResetPwdForm = this;
            AppClient.RegisterHandler(Protocol.C0001_2, resetPwdHandler);
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (AppClient.IsReady)
            {
                ICrypta crypta = SecurityService.GetCrypta();
                if (string.IsNullOrEmpty(textOldPwd.Text) && string.IsNullOrEmpty(textNewPwd.Text) && string.IsNullOrEmpty(textConfirmPwd.Text ) )
                {
                    MessageBox.Show(this, "Passwords can not be empty!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }else if (textNewPwd.Text.Trim() != textConfirmPwd.Text ){
                       MessageBox.Show(this, "Re-type password is not the same as new password.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (crypta.specialEncrypt( textOldPwd.Text.Trim()) != AppSetting.PWD){
                MessageBox.Show(this, "Old password is incorrect, please try again! ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (textNewPwd.Text.Length<6 )
                {
                    MessageBox.Show(this, "Password must be at least 8 characters, please try again. ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (!AppUtil.PwdIsValid(textNewPwd.Text.Trim()))
                {
                    MessageBox.Show(this, "Password cannot be any triple repeat characters, please try again. ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //string oldPwd = textOldPwd.Text.Trim();
                    string newPwd= textNewPwd.Text.Trim();
                    //oldPwd = crypta.specialEncrypt(oldPwd);
                    newPwd = crypta.specialEncrypt(newPwd);
                    string request = NetHelper.BuildMsg(Protocol.C0001_2, new string[] { AppSetting.PWD, newPwd });
                    Console.WriteLine(" request    " + request);
                    resetPwdHandler.Send(request);                   
                }
                crypta = null;
            }
            else
            {
                MessageBox.Show(this, "Network is disconnected!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textConfirmPwd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btnReset_Click(sender, e);
            }
        }

		protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
		{
			int WM_KEYDOWN = 256;
			int WM_SYSKEYDOWN = 260;

			if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
			{
				switch (keyData)
				{
					case Keys.Escape:
						try
						{
							this.Close();//cscCloseWindow form
						}
						catch
						{

						}
						break;
				}

			}
			return false;
		}
    }
}
