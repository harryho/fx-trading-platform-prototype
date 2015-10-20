using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace fxpa
{
    public class ProviderService
    {
        static Queue<IHandler> hdlQueue = new Queue<IHandler>();
        static System.Timers.Timer timer = new System.Timers.Timer(500);

        static object locker = new object();

        static public void RegisterHandler(IHandler handler)
        {
            lock (locker)
            {
                hdlQueue.Enqueue(handler);
            }
        }


        static IHandler currHandler;

        public static void Init()
        {
            timer.Elapsed += new System.Timers.ElapsedEventHandler(CheckProvider);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Stop();
        }

        public static void KickOff()
        {
            timer.Start();
        }

        public static void UnRegisterHandler(DataProvider dp)
        {
            List<IHandler> hdlList = new List<IHandler>();
            lock (locker)
            {
                if (hdlQueue.Count > 0)
                {
                    while (hdlQueue.Count > 0)
                    {
                        hdlList.Add(hdlQueue.Dequeue());
                    }
                    int index = -1;
                    for (int i = 0; i < hdlList.Count; i++)
                    {
                        IHandler h = hdlList[i];
                        if (((ProviderHandler)h).Provider == dp)
                        {
                            h.IsProcessing = false;
                            h = null;
                            index = i;
                            break;
                        }
                    }

                    if (index > 0)
                        hdlList.RemoveAt(index);

                    foreach (IHandler hdl in hdlList)
                        hdlQueue.Enqueue(hdl);
                }
            }
        }




        static void CheckProvider(object obj, System.Timers.ElapsedEventArgs e)
        {
            if (hdlQueue.Count > 0)
            {
                if (currHandler == null)
                {
                    AppContext.IsProviderInitializing = true;
                    currHandler = hdlQueue.Dequeue();
                    currHandler.Execute();
                }
                else if (!currHandler.IsProcessing)
                {
                    lock (currHandler)
                    {
                        AppContext.IsProviderInitializing = true;
                        currHandler = hdlQueue.Dequeue();
                        currHandler.Execute();
                    }
                }
                else
                {
                    bool rm = false;
                    if (((ProviderHandler)currHandler).Provider == null)
                    {
                        rm = true;
                    }
                    else
                    {
                        DataProvider dp = DataService.GetProviderBySymbol(((ProviderHandler)currHandler).Provider.Symbol);
                        if (dp == null)
                        {
                            currHandler.IsProcessing = false;
                            rm = true;
                        }
                    }
                    if (rm)
                    {
                        List<IHandler> hdlList = new List<IHandler>();
                        lock (locker)
                        {
                            if (hdlQueue.Count > 0)
                            {
                                while (hdlQueue.Count > 0)
                                {
                                    hdlList.Add(hdlQueue.Dequeue());
                                }
                                int index = -1;
                                for (int i = 0; i < hdlList.Count; i++)
                                {
                                    IHandler h = hdlList[i];
                                    if (((ProviderHandler)h).Provider == null || h.IsProcessing == false)
                                    {
                                        h.IsProcessing = false;
                                        h = null;
                                        index = i;
                                        break;
                                    }
                                }

                                if (index > 0)
                                    hdlList.RemoveAt(index);

                                foreach (IHandler hdl in hdlList)
                                    hdlQueue.Enqueue(hdl);
                            }
                        }
                    }
                }
            }
            else
            {
                timer.Stop();
            }
        }
    }
}
