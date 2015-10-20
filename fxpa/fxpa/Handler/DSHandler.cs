using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
    public class DSHandler : MsgHandler
    {
        public override void Send(object msg)
        {
            Console.WriteLine(" DS Send ~~~~~~~~~~~ " + msg);
            Client.Send((string)msg);
        }

        public override void Receive(object msg)
        {
            //Console.WriteLine(" DS Receive ~~~~~~~~~~~ ");
            String msg1 = (string)msg;
            if (msg1 != null)
            {
                lock (msg1)
                {
                    DataHelper.ReceiveMsg(msg1);
                }
            }
        }
    }
}
