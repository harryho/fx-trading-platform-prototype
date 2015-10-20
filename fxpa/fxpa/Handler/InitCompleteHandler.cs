using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    public class InitCompleteHandler : MsgHandler
    {
       

        public InitCompleteHandler(Client client)
            : base(client)
        {
        }
       
       public override void Send(object msg)
        {
            Console.WriteLine(" InitCompleteHandler  Send ~~~~~~~~~~~ ");
            LogUtil.Info(" InitCompleteHandler  Send ~~~~~~~~~~~ ");
            Client.Send(msg);
        }


       public override void Receive(object msg)
       {
           //Console.WriteLine(" LogOutHandler  Receive ~~~~~~~~~~~ " + msg);
         }

       //int errCode = 0;

       static string SUCCESS = "1";
       static string FAILURE = "0";

       public override void Execute()
       {
           string request = NetHelper.BuildMsg(Protocol.C0002_2, new string[] { SUCCESS });
           //string request = NetHelper.BuildMsg(Protocol.C0011, new string[] { });
           Send(request);
       }
    }
}
