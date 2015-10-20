using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    public class LogOutHandler : MsgHandler
    {
        FXPA fxClient;

        public FXPA FXClient
        {
            get { return fxClient; }
            set { fxClient = value; }
        }


        public LogOutHandler(Client client)
            : base(client)
        {
        }
       
       public override void Send(object msg)
        {
            Console.WriteLine(" LogOutHandler  Send ~~~~~~~~~~~ ");
            LogUtil.Info(" LogOutHandler  Send ~~~~~~~~~~~ ");
            Client.Send(msg);
        }


       public override void Receive(object msg)
       {
           //Console.WriteLine(" LogOutHandler  Receive ~~~~~~~~~~~ " + msg);
         }

       //int errCode = 0;

       public override void Execute()
       {
           string request = NetHelper.BuildMsg(Protocol.C0001_4, new string[] {AppSetting.USER  , "LOGOUT" });
           Send(request);
       }
    }
}
