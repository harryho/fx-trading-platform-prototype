using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    //public delegate void MsgUpdatedDelegate(object msg);

    public class ResetPwdHandler : MsgHandler
    {
        //ResetPwdForm resetPwdForm;

        //public ResetPwdForm ResetPwdForm
        //{
        //    get { return resetPwdForm; }
        //    set { resetPwdForm = value; }
        //}

        public ResetPwdHandler(Client client)
            : base(client)
        {
            
        }

       
       public override void Send(object msg)
        {
            Console.WriteLine(" ResetPwdHandler  Send ~~~~~~~~~~~ " + msg);
            Client.Send((string)msg);
        }


       public override void Receive(object msg)
       {
           //Console.WriteLine(" ResetPwdHandler  Receive ~~~~~~~~~~~ " + msg);
           //MethodInvoker mi = new MethodInvoker(CloseForm);
           //if (resetPwdForm != null && !resetPwdForm.IsDisposed)
           //{
           //    string[] msgs = (string[])msg;
           //    if (!msgs.Contains(NULL))
           //    {
           //        int paramAmount = int.Parse(msgs[2]);
           //        int result = int.Parse(msgs[3]);
           //        switch (result)
           //        {
           //            case 0:
           //                msgCode = result;
           //                AppSetting.PWD = resetPwdForm.textConfirmPwd.Text.Trim();
           //                mi = new MethodInvoker(ShowMsg);
           //                resetPwdForm.BeginInvoke(mi);
           //                break;
           //            default:
           //                msgCode = result;
           //                mi = new MethodInvoker(ShowMsg);
           //                resetPwdForm.BeginInvoke(mi);
           //                break;
           //        }
           //    }
           //    else
           //    {
           //        msgCode = -3;
           //        mi = new MethodInvoker(ShowMsg);
           //        resetPwdForm.BeginInvoke(mi);
           //    }
           //}
       }

        int msgCode=0; 

        private void ShowMsg()
        {
            //switch (msgCode)
            //{
            //    case 0:
            //        MessageBox.Show(resetPwdForm, "修改成功!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        resetPwdForm.Close();
            //        break;
            //    case -1:
            //        MessageBox.Show(resetPwdForm, "Old 密码不匹配!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        break;
            //    case -2:
            //        MessageBox.Show(resetPwdForm, "修改失败!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        break;
            //    case -3:
            //        MessageBox.Show(resetPwdForm, "Server is busy，please try 再试!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        resetPwdForm.Close();
            //        break;
            //}
        }

        private void CloseForm()
        {
            //if (resetPwdForm != null)
            //{
            //    resetPwdForm.DialogResult = DialogResult.OK;
            //    resetPwdForm.Close();
            //    resetPwdForm.Dispose();
            //    resetPwdForm = null;
            //}
        }

    }
}
