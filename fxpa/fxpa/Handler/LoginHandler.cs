using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    public class LoginHandler : MsgHandler
    {
        //LoginForm loginForm=new Object();

        //public LoginForm LoginForm
        //{
        //    get { return loginForm; }
        //    set { loginForm = value; }
        //}

        public LoginHandler(Client client)
            : base(client)
        {
        }
       
       public override void Send(object msg)
        {
            Console.WriteLine(" LoginHandler  Send ~~~~~~~~~~~ ");
            LogUtil.Info(" LoginHandler  Send ~~~~~~~~~~~ ");
            Client.Send((string)msg);
        }

       public override void Receive(object msg)
       {
       //    Console.WriteLine(" LoginHandler  Receive ~~~~~~~~~~~ " );
       //    LogUtil.Info(" LoginHandler  Receive ~~~~~~~~~~~ ");
       //    MethodInvoker mi = new MethodInvoker(CloseForm);
       //    string[] msgs = (string[])msg;
       //    int paramAmount = AppUtil.StringToInt(msgs[2]);
       //    int result = AppUtil.StringToInt(msgs[3]);


       //    //if (paramAmount != AppUtil.PARSE_ERROR)
       //    //{
       //        Protocol protocol = AppUtil.ParseProtocol(msgs[1]);
       //        if (protocol== Protocol.C0001_1 && paramAmount ==6 )
       //        {
       //            switch (result)
       //            {
       //                case 0:
       //                    AppSetting.STATUS = msgs[4];
       //                    AppSetting.SOFTWARE_TYPE = msgs[5];
       //                    string strEndTime = msgs[6];
       //                    string strServerTime = msgs[7];
       //                    string strLatestVersion = msgs[8];
       //                    AppSetting.LATEST_VERSION = strLatestVersion;
       //                    DateTime endTime;
       //                    DateTime serverTime;
       //                    if (strEndTime != NULL)
       //                    {
       //                        endTime = DateTime.Parse(strEndTime);
       //                        serverTime = DateTime.Parse(strServerTime);

       //                        AppSetting.END_TIME = endTime;
       //                        AppSetting.START_TIME = serverTime;
       //                        AppContext.DAY = Convert.ToInt16(serverTime.Day);

       //                        if (serverTime.Subtract(endTime) > TimeSpan.FromMinutes(30))
       //                        {
       //                            code = -5;
       //                            AppContext.IsLogin = false;
       //                            AppContext.hasLoginError = true;
       //                        }
       //                        else
       //                        {
       //                            AppContext.IsLogin = true;
       //                        }
       //                    }
       //                    else
       //                    {
       //                        AppSetting.IS_PERMANENT = true;
       //                        AppContext.IsLogin = true;
       //                    }

       //                    if (AppContext.IsLogin)
       //                    {
       //                        AppSetting.USER = AppSetting.RECONN_USER;
       //                        AppSetting.PWD = AppSetting.RECONN_PWD;
       //                        AppUtil.CheckMarketIsOpen();
       //                    }

       //                    if (!AppContext.IsReconnecting)
       //                    {
       //                        if (AppSetting.VERSION != strLatestVersion)
       //                        {
       //                            ShowMsg(100);
       //                        }
       //                        if (loginForm != null && !loginForm.IsDisposed)
       //                        {
       //                            mi = new MethodInvoker(ShowMsg);
       //                            loginForm.BeginInvoke(mi);
       //                            mi = new MethodInvoker(CloseForm);
       //                            loginForm.BeginInvoke(mi);
       //                        }
       //                    }
       //                    //System.Diagnostics.Process.Start("IExplore.exe", "www.google.com");
       //                    break;
       //                default:
       //                    code = result;
       //                    if (AppContext.IsReconnecting)
       //                        AppContext.hasLoginError = true;
       //                    if (loginForm != null && !loginForm.IsDisposed)
       //                    {
       //                        mi = new MethodInvoker(ShowMsg);
       //                        loginForm.BeginInvoke(mi);
       //                        mi = new MethodInvoker(RefreshLoginBtn);
       //                        loginForm.BeginInvoke(mi);
       //                    }
       //                    else
       //                    {
       //                        ShowMsg(code);
       //                    }
       //                    break;
       //            }
       //        }
       //        else if(protocol==Protocol.C0002_4)
       //        {
       //            AppContext.IsGetLatestVersion = true;
       //            string latestVersion =  msgs[3];
       //            int coerce;
       //            int.TryParse(msgs[4], out coerce);
       //            string desc = msgs[5];
       //            updateLink = msgs[6];
       //            if (latestVersion != AppSetting.VERSION)
       //            {
       //                switch (coerce)
       //                {
       //                    case 0:
       //                        code = -7;
       //                        break;
       //                    case 1:
       //                        code = -8;
       //                        break;
       //                }
       //                ShowMsg();

       //            }
       //            else
       //            {
       //                AppContext.IsUpdated = true;
       //                AppContext.IsFinishVerChk = true;
       //            }
       //        }
       //    //}
       //    else
       //    {
       //        code = -99;
       //        if (loginForm != null && !loginForm.IsDisposed)
       //        {
       //            mi = new MethodInvoker(ShowMsg);
       //            loginForm.BeginInvoke(mi);

       //            mi = new MethodInvoker(RefreshLoginBtn);
       //            loginForm.BeginInvoke(mi);
       //        }
       //        else
       //        {
       //            ShowMsg(code);
       //        }
       //    }
       //    AppContext.IsLoginning = false;
       //}

       // int code=0;
       // string updateLink = "";
       // private void ShowMsg()
       // {
       //     switch (code)
       //     {
       //         //case 100:
       //         //    MessageBox.Show(loginForm, "新的版本已经可以下载!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //         //    break;
       //         case -1:
       //             MessageBox.Show(loginForm, "您的User ID或者密码不正确!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //             break;
       //         case -2:
       //             MessageBox.Show(loginForm, "Your account 已经被停用!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //             break;
       //         case -3:
       //             MessageBox.Show(loginForm, "Your account 正在被人使用!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //             break;
       //         case -4:
       //             MessageBox.Show(loginForm, "系统无法识别你的身份，请勿非法Login!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //             loginForm.Close();
       //             break;
       //         case -5:
       //             MessageBox.Show( loginForm, "Your account 暂时被停止使用，请与我们客服联系!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //             loginForm.Close();
       //             break;
       //         case -6:
       //             MessageBox.Show(loginForm, "您尝试Login过多，请10分钟后再Login!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //             break;
       //         //case -7:
       //         //    DialogResult dr= MessageBox.Show(loginForm, "检测到有更新的版本，是否立刻下载？", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
       //         //    if (dr == DialogResult.Yes && !string.IsNullOrEmpty(updateLink))
       //         //    {
       //         //        System.Diagnostics.Process.Start("IExplore.exe", updateLink );
       //         //    }
       //         //    break;
       //         //case -8:
       //         //    MessageBox.Show(loginForm, "Since 系统升级，必须下载最新的客户端方可Login！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //         //    loginForm.Close();
       //         //    if (!string.IsNullOrEmpty(updateLink))
       //         //    System.Diagnostics.Process.Start("IExplore.exe", updateLink);
       //         //    break;
       //         case -7:
       //             DialogResult dr = MessageBox.Show( "检测到有更新的版本，是否立刻下载？", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
       //             if (dr == DialogResult.Yes && !string.IsNullOrEmpty(updateLink))
       //             {
       //                 Application.ExitThread();
       //                 System.Diagnostics.Process.Start("IExplore.exe", updateLink);
       //             }
       //             else
       //             {
       //                 AppContext.IsUpdated = true;
       //             }
       //             AppContext.IsFinishVerChk = true;
       //             break;
       //         case -8:
       //             MessageBox.Show( "Since 系统升级，必须下载最新的客户端方可Login！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //             //loginForm.Close();
       //             if (!string.IsNullOrEmpty(updateLink))
       //             {
       //                 AppContext.IsFinishVerChk = true;
       //                 Application.ExitThread();
       //                 System.Diagnostics.Process.Start("IExplore.exe", updateLink);
       //             }
       //             break;
       //         case -99:
       //             MessageBox.Show(loginForm, "Server is busy，please try 再登陆!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
       //             break;
       //     }
       //     //loginForm.btnlogin.Enabled = true;
        }

        private void ShowMsg(int code)
        {
            switch (code)
            {
                //case 100:
                //    MessageBox.Show(loginForm, "新的版本已经可以下载!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    break;
                //case 0:
                //    MessageBox.Show(loginForm, "您已Re-Login成功!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //break;
                case -1:
                    MessageBox.Show("您的User ID或者密码不正确!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case -2:
                    MessageBox.Show("Your account 被停用!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case -3:
                    MessageBox.Show("Your account 正在被人使用!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case -4:
                    MessageBox.Show("您非法Login!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case -5:
                    MessageBox.Show("Your account has been expired!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case -99:
                    MessageBox.Show("Server is busy，请再次尝试!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }


        public override void Execute()
        {
            string request = NetHelper.BuildMsg(Protocol.C0002_4, new string[] { AppSetting.A_OR_C,  AppSetting.VERSION});
            Send(request);
        }


        private void RefreshLoginBtn()
        {
            //if (loginForm != null)
            //{
            //    loginForm.btnlogin.Enabled = true;
            //}
        }

        private void CloseForm()
        {
            //if (loginForm != null)
            //{
            //    loginForm.DialogResult = DialogResult.OK;
            //    loginForm.Close();
            //    loginForm.Dispose();
            //    loginForm = null;
            //}
        }

    }
}
