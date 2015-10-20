// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----



using System;
using System.Reflection;
using System.Windows.Forms;

using fxpa.Properties;
using System.Threading;
using System.Collections.Generic;

namespace fxpa
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [MTAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.ThreadExit += new EventHandler(Application_ThreadExit);
            //AppContext.AppConnect();

            Console.WriteLine(Application.CommonAppDataPath);
            Console.WriteLine(Application.LocalUserAppDataPath);
            Console.WriteLine(Application.ExecutablePath);
            Console.WriteLine(Application.StartupPath);
            Console.WriteLine(Application.UserAppDataPath);
            Console.WriteLine(Application.UserAppDataRegistry);

            LogUtil.init(true, true, true, true);
            LogUtil.Info("\n\n ####################################################### \n\n ");

            Console.WriteLine(" LoadingForm loadingForm = new LoadingForm(); ");
            AppConst.InitAppSettings();
            LoadingForm loadingForm = new LoadingForm();

            if (loadingForm.ShowDialog() == DialogResult.OK)
            {
                if (!AppContext.NoSubscription)
                {
                    ProviderService.Init();
                    Form mainForm = new FxpaForm();
                    Application.Run(mainForm);
                    AppContext.FxpaMain = mainForm;
                }
            }

        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //SystemMonitor.Error(e.Exception.Message);
            Console.WriteLine(" e " + e.Exception);
            LogUtil.Error(e.Exception.ToString());
            LogUtil.Close();
        }

        static void Application_ThreadExit(object sender, EventArgs e)
        {
            LogUtil.Info("Application Exit");
            LogUtil.Close();
        }
    }
}
