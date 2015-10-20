using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
    //public delegate void MsgUpdatedDelegate(object msg);

    public enum Protocol
    {
        C0001_1,
        C0001_2,
        C0001_3,
        C0001_4,
        C0001_5,
        C0002_1,
        C0002_2,
        C0002_3,
        C0002_4,
        M0003_1,
        C0003_2,
        C0003_3,
        C0004_1,
        C0004_2,
        D0002_1,
        S0002_1,
        S0002_3,
        K0004_1,
        K0004_2,
        K0004_3,
        K0004_4,
        K0004_5,
        S0005_1,
        S0001,
        C0002,
        CS0004_1,
        M0001,
        M0002,
        UNKNOWN
    }

    public class MsgHandler : IHandler
    {
        public event MsgUpdatedDelegate MsgUpdateEvent;

        public static string NULL = "_NULL_";

        public static string FORMAT = "yyyy'-'MM'-'dd HH':'mm";

        //private Client client ;

        //public Client Client
        //{
        //    get { return client; }
        //    set { client = value; }
        //}

        public MsgHandler()
        {
        }

       //public MsgHandler(Client client)
       // {
       //     this.client = client;
       // }

        public virtual void Process(object msg)
        {
            if(MsgUpdateEvent!=null)
                   MsgUpdateEvent(msg);
        }

        public virtual void BeforeSend(object msg)
        {
        }

        public virtual void Send(object msg)
        {
             //client.Send(msg);
        }

        public virtual void BeforeReceive(object msg)
        {

        }

        public virtual void AfterSend(object msg)
        {

        }

        public virtual void Receive(object msg)
        {
            Console.WriteLine(" MSG Receive ~~~~~~~~~~~ "); 
        }

        public virtual void AfterReceive(object msg)
        {

        }


       protected bool isProcessing;

        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; }
        }

        //public bool IsProcessing()
        //{
        //    return isProcessing;
        //}

        public void Launch()
        {
            isProcessing = true;
        }

        public virtual void Execute()
        {

        }


      
    }
}
