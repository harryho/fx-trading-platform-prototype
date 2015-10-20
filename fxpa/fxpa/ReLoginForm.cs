using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace fxpa
{
    public partial class ReLoginForm : Form
    {

        public ReLoginForm()
        {
            InitializeComponent();
            txtusername.Text= AppSetting.USER;
            txtuserpwd.Text = "";
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            this.btnlogin.Enabled = false;

            if (AppClient.NetworkIsAvailable())
            {
                if (!string.IsNullOrEmpty(txtusername.Text) && !string.IsNullOrEmpty(txtuserpwd.Text))
                {
                 //   ICrypta crypta = SecurityService.GetCrypta();
                    string pwd = crypta.specialEncrypt(txtuserpwd.Text.Trim());
                    AppSetting.RECONN_USER = txtusername.Text.Trim();
                    AppSetting.RECONN_PWD = pwd;
                    AppContext.ProcessReLogin();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("User ID and 密码均不能为空!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnlogin.Enabled = true;
                    return;
                }
            }
            else
            {
                MessageBox.Show("Network is disconnected!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnlogin.Enabled = true;
                return;
            }

        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtuserpwd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btnlogin_Click(sender, e);
            }
        }
    }
}
