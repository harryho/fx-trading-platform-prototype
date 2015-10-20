using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
    public delegate void MsgUpdatedDelegate(object msg);

    

    public interface IHandler
    {
        

         event MsgUpdatedDelegate MsgUpdateEvent;

         void Process( object msg );

         void Send( object msg );

         void BeforeSend(object msg);

         void AfterSend(object msg);

         void BeforeReceive(object msg);

         void Receive(object msg);

         void AfterReceive(object msg);

         bool IsProcessing{ get; set; }

         void Launch();

         void Execute();
    }
}
