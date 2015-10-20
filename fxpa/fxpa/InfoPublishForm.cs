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

    public partial class InfoPublishForm : Form
    {
        //public ZSFXClient Main;  //Window form object 
        InfoPublishHandler infoPublishHandler;
         static string EN_DOT = ".";
         static string CN_DOT= "。";
         static string SPACE = "  ";
         //static int MAX = 10;
         //static int OFFSET = 5;
         static int MAX = 1000;
         static int OFFSET = 50;
        //event ProgressUpdate ProgressUpdateEvent;

        public InfoPublishForm()
        {
            InitializeComponent();
            infoPublishHandler = new InfoPublishHandler(AppClient.GetInstance);
            infoPublishHandler.InfoPublishForm = this;
            AppContext.InfoPublishHandler = infoPublishHandler;
            this.Show();
            this.Hide();
            AppClient.RegisterHandler(Protocol.S0005_1, infoPublishHandler);
       }

        public void AppendMsg(object obj)
        {
            PublishInfo info = (PublishInfo)obj;
            StringBuilder sb = new StringBuilder();
            string msg = info.Content;
            sb.Append(info.DateTime.ToShortDateString());
            sb.Append(SPACE);
            sb.Append(info.DateTime.ToLongTimeString());
            sb.Append(SPACE);
            sb.Append(info.Type);
            sb.Append(Environment.NewLine);
            sb.Append(AppUtil.GetSymbolChinese(info.Symbol.ToString()));
            sb.Append(SPACE);
            sb.Append(msg);
            if (!msg.EndsWith(EN_DOT) && !msg.EndsWith(CN_DOT))
            {
                sb.Append(CN_DOT);
            }
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            Font fBold = new Font("Arial", 10, FontStyle.Regular);            
          
            lock (richTextBoxInfo)
            {
                if (richTextBoxInfo.Lines.Length > MAX)
                {
                    richTextBoxInfo.Text = richTextBoxInfo.Text.Substring(richTextBoxInfo.Text.IndexOf(richTextBoxInfo.Lines[OFFSET]));
                    lastColor = Color.DarkBlue;
                }
                richTextBoxInfo.Enabled = false;

                richTextBoxInfo.SelectionStart =  richTextBoxInfo.Text.Length;
                richTextBoxInfo.SelectionFont = fBold;
                if (lastColor == Color.Red)
                {
                    richTextBoxInfo.SelectionColor = Color.DarkBlue;
                    lastColor = Color.DarkBlue;
                }
                else
                {
                    richTextBoxInfo.SelectionColor = Color.Red;
                    lastColor = Color.Red;
                }
                richTextBoxInfo.SelectedText = sb.ToString();
                richTextBoxInfo.Enabled = true;
            }
            Opacity = 100;
            Visible = true;
            Show();
        }

        Color lastColor = Color.Red;


        private void InfoPublishForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.richTextBoxInfo.Text = "";
            e.Cancel = true;
        }

        private void richTextBoxInfo_TextChanged(object sender, EventArgs e)
        {
            //vScrollBar1.Maximum= richTextBoxInfo.Lines.Length + 5;
            //vScrollBar1.Value = richTextBoxInfo.Lines.Length;
        }

        private void vScrollBar1_LocationChanged(object sender, EventArgs e)
        {
            int pos = ((ScrollBar)sender).Value;
        }


    


 
        
       
    }
}
