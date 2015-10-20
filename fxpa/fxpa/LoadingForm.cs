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

    public partial class LoadingForm : Form
    {
        LoadingHandler  loadingHandler;
        static string CONNECTING_STATEMENT = "Network is connecting......";
        //event ProgressUpdate ProgressUpdateEvent;

        public LoadingForm()
        {
            InitializeComponent();
            loadingHandler = new LoadingHandler( );
            loadingHandler.LoadingForm = this;
            //AppClient.RegisterHandler(Protocol.C0001_3, loadingHandler);
            //AppClient.RegisterHandler(Protocol.C0002_1, loadingHandler);
            //AppClient.RegisterHandler(Protocol.M0003_1, loadingHandler);
            //AppClient.RegisterHandler(Protocol.C0003_2, loadingHandler);
            //AppClient.RegisterHandler(Protocol.C0004_1, loadingHandler);
            //AppClient.RegisterHandler(Protocol.C0004_2, loadingHandler);
            //AppClient.RegisterHandler(Protocol.C0003_3, loadingHandler);
            //loadingProgressBar.Maximum = 100;
            //loadingProgressBar.Invalidated += new InvalidateEventHandler(loadingProgressBar_StatusChanged);
       }

        private void LoadingForm_Shown(object sender, EventArgs e)
        {
            Loading();
        }

        public void Loading()
        {
            loadingHandler.Execute();
        }

        public void UpdateLoadingProgress( int value  )
        {
           // loadingProgressBar.Value = value;
           //// connectiongLabel.Text = CONNECTING_CHINESE + value.ToString() + "%";
           // loadingProgressBar.Invalidate();
        }

        private void loadingProgressBar_StatusChanged(object sender, EventArgs e)
        {
            //if (loadingProgressBar.Value == loadingProgressBar.Maximum)
            //{
            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //}
        }

        public int GetLoadingStatus()
        {
            return 0;// loadingProgressBar.Value;
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
