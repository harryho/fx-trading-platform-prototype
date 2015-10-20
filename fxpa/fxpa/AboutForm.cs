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

    public partial class AboutForm : Form
    {

        public AboutForm()
        {
            InitializeComponent();
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
						this.Close();//cscCloseWindow form
						break;
				}

			}
			return false;
		}

        private void label3_Click(object sender, EventArgs e)
        {

        }




       
    }
}
