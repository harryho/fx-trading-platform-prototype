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
	public partial class LoginForm : Form
	{
        LoginHandler loginHandler;

        public LoginHandler LoginHandler
        {
            get { return loginHandler; }
            set { loginHandler = value; }
        }
		static int MAX_LOGIN_TIME = 100;
		public LoginForm()
		{

			InitializeComponent();
            this.skinEngine1.SkinFile = "skin/default.ssk";
			//this.SetAutoSizeMode = AutoCompleteMode.None;
			loginHandler = new LoginHandler(AppClient.GetInstance);
			loginHandler.LoginForm = this;
			AppClient.RegisterHandler(Protocol.C0001_1, loginHandler);
            AppClient.RegisterHandler(Protocol.C0002_4, loginHandler);
            //loginHandler.Execute();
		}
		Thread loginThread;
		private void btnlogin_Click(object sender, EventArgs e)
		{
			this.btnlogin.Focus();
			this.btnlogin.Enabled = false;
            if (AppClient.NetworkIsAvailable())
            {
				if (!string.IsNullOrEmpty(txtusername.Text) && !string.IsNullOrEmpty(txtuserpwd.Text))
				{
					Console.WriteLine(" start login thread ~~~~~ ");
					if (!AppClient.IsReady)
					{
						AppClient.Close();
						DSClient.Close();
					}
					loginThread = new Thread(new ThreadStart(this.Login));
					loginThread.Start();
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

		private void Login()
		{
			bool isBusy = false;
			bool finished = false;
			while (!finished)
			{
				Console.WriteLine(" request    ");
				if (AppClient.IsReady)
				{
					AppSetting.RECONN_USER = txtusername.Text.Trim();
					string pwd = txtuserpwd.Text.Trim();
					ICrypta crypta = SecurityService.GetCrypta();
					pwd = crypta.specialEncrypt(pwd);
					AppSetting.RECONN_PWD = pwd;
					string cpuSN = SysUtil.GetSpecialStuffValueInfo(Win32Stuff.Win32_Processor.ToString(), "ProcessorId");
					string hdID = SysUtil.GetSpecialStuffValueInfo(Win32Stuff.Win32_DiskDrive.ToString(), "Model");
					string machineInfo = cpuSN +"-"+ hdID;
					machineInfo = machineInfo ?? (LoginHandler.NULL);
					machineInfo = string.IsNullOrEmpty(machineInfo.Trim()) ? LoginHandler.NULL : machineInfo.Trim();

					AppSetting.MACHINE_INFO = machineInfo;
					string request = NetHelper.BuildMsg(Protocol.C0001_1, new string[] { txtusername.Text.Trim(), pwd, machineInfo, AppSetting.VERSION, AppSetting.A_OR_C });
					crypta = null;
					Console.WriteLine(" request    " + count);
					try
					{
						loginHandler.Send(request);
						finished = true;
					}
					catch (Exception ex)
					{
						this.Invoke(new MethodInvoker(Noconnection));
					}
				}

				count++;
				if (count >= MAX_LOGIN_TIME)
				{
					AppClient.Close();
					AppClient.IsStopAttempt = true;
					isBusy = true;
					break;
				}
				Thread.Sleep(500);
				if (isCancel)
					break;
			}

			if (isBusy == true && !isCancel)
			{
				this.Invoke(new MethodInvoker(Noconnection));
			}
		}

		private void Noconnection()
		{
			if (!this.IsDisposed)
			{
				count = 0;
				MessageBox.Show(this, "Server is busy，Please try again later!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.btnlogin.Enabled = true;
			}
		}

		static int count = 0;

		static bool isCancel = false;


		private void btncancel_Click(object sender, EventArgs e)
		{
			isCancel = true;
			AppClient.Close();
			DSClient.Close();
			AppClient.IsStopAttempt = true;
			DSClient.IsStopAttempt = true;
			try
			{
				if (loginThread.IsAlive)
					loginThread.Abort();
			}
			catch (Exception ex) { }
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void txtuserpwd_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				btnlogin_Click(sender, e);
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
                        btncancel_Click(null, null);//cscCloseWindow form
						break;
				}

			}
			return false;
		}

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (loginThread.IsAlive)
                    loginThread.Abort();
            }
            catch (Exception ex) { }
        }

	}

}
