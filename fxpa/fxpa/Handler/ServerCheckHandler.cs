using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    public class ServerCheckHandler : MsgHandler
    {
        FXPA fxClient;

        public FXPA FXClient
        {
            get { return fxClient; }
            set { fxClient = value; }
        }


        public ServerCheckHandler(Client client)
            : base(client)
        {
        }
       
       public override void Send(object msg)
        {
            Console.WriteLine(" ServerCheckHandler  Send ~~~~~~~~~~~ ");
            LogUtil.Info(" ServerCheckHandler  Send ~~~~~~~~~~~ ");
            Client.Send(msg);
        }


       public override void Receive(object msg)
       {
            LogUtil.Info(" ServerCheckHandler  Receive ~~~~~~~~~~~ ");
            string[] msgs = (string[])msg;
            Protocol protocol = AppUtil.ParseProtocol(msgs[1]);
            int paramAmount = AppUtil.StringToInt(msgs[2]);
            if (protocol != Protocol.UNKNOWN && paramAmount >0)
            {
                try
                {
                    string token = msgs[3];
                    string strServerTime = msgs[4];
                    LogUtil.Info(" ServerCheckHandler  Receive  ~~~~~~~~~~~ strServerTime " + strServerTime);
                    DateTime serverTime;
                    serverTime = DateTime.Parse(strServerTime);
                    AppContext.CURRENT_TIME = serverTime;
                    lock (AppContext.appTokenLock)
                    {
                        AppContext.ServToken[token] = serverTime.ToLongTimeString();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" ServerCheckHandler  Receive  Exception~~~~~~~~~~~ " + e.StackTrace);
                    LogUtil.Info(" ServerCheckHandler  Receive  Exception~~~~~~~~~~~ " + e.StackTrace);
                }
            }
       }
       static int count = 0;
       public override void Execute()
       {
           DateTime now = DateTime.Now;
           count = count >= 9999 ? 0 : count;
           string token = AppSetting.USER + "_" + AppSetting.VERSION + "_" + now.Year + "." + now.Month + "." + now.Day + "." + now.Hour + "." + now.Minute + "." + now.Second + "." + count;
           count++;
           string request = NetHelper.BuildMsg(Protocol.C0002_3, new string[] { token });
           lock (AppContext.appTokenLock)
           {
               AppContext.ServToken.Add(token, MsgHandler.NULL);
           }
           Send(request);        
       }

    }
}
