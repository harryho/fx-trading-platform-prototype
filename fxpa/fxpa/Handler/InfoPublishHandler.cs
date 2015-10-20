using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace fxpa
{
    public class InfoPublishHandler : MsgHandler
    {
        //InfoPublishForm infoPublishForm;

        //DoubleBufferListView infoListView;

        //public DoubleBufferListView InfoListView
        //{
        //    get { return infoListView; }
        //    set { infoListView = value; }
        //}

        //public InfoPublishForm InfoPublishForm
        //{
        //    get { return infoPublishForm; }
        //    set { infoPublishForm = value; }
        //}

        //public InfoPublishHandler(Client client)
        //    : base(client)
        //{
        //}

        //public override void Send(object msg)
        //{
        //    Console.WriteLine(" InfoPublishHandler  Send ~~~~~~~~~~~ " + msg);
        //    Client.Send(msg);
        //}


        //public override void Receive(object msg)
        //{

        //    Console.WriteLine(" InfoPublishHandler  Receive ~~~~~~~~~~~ " + msg);

        //    string[] msgs = (string[])msg;
        //    //string[] msgs = new string[] { "", "S0005", "3", "AUDUSD", "5", "xxxxxssssssssssss sXXXXXXXXXXXXXXX JJJJJJJJJJ ssssssssss aaaaaaaaaaaaaa sssssss  sdfsd 23.58" };
        //    Protocol protocol = AppUtil.ParseProtocol(msgs[1]);
        //    int paramAmount = AppUtil.StringToInt(msgs[2]);
        //    if (protocol != Protocol.UNKNOWN)
        //    {
        //        if (paramAmount > 0)
        //        {
        //            switch (protocol)
        //            {
        //                case Protocol.S0005_1:

        //                    Symbol symbol = AppUtil.ParseSymbol(msgs[3]);
        //                    Interval interval = AppUtil.StringToInterval(msgs[4]);

        //                    if (AppSetting.SYMBOLS.Contains(symbol) && AppSetting.INTEVALS.Contains(interval))
        //                    {

        //                        DateTime dateTime = DateTime.Parse(msgs[6]);
        //                        PublishInfo info = new PublishInfo(   dateTime, msgs[5]);
        //                        info.Symbol = symbol;
        //                        info.Interval = interval;
        //                        MsgUpdatedDelegate d;
        //                        if (infoPublishForm != null && !infoPublishForm.IsDisposed)
        //                        {
        //                            d  = new MsgUpdatedDelegate(UpdateInfoPublishForm);
        //                            infoPublishForm.BeginInvoke(d, info);
        //                        }

        //                        if (infoListView != null && !infoListView.IsDisposed)
        //                        {
        //                            d = new MsgUpdatedDelegate(UpdateInfoListView);
        //                            infoListView.BeginInvoke(d, info);
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //}


        //private void UpdateInfoPublishForm(object obj )
        //{
        //    infoPublishForm.AppendMsg(obj);
        //    if (AppContext.IsOpenSpeaker)
        //    {
        //        System.Media.SoundPlayer player = new SoundPlayer(Properties.Resources.Blip);
        //        player.LoadAsync();
        //        player.Play();
        //    }
        //}


        //private void UpdateInfoListView(object obj)
        //{
        //    PublishInfo pinfo = (PublishInfo)obj ?? new PublishInfo();
        //    if (!string.IsNullOrEmpty(pinfo.Content))
        //    {
        //        lock (AppContext.PublishInfos)
        //        {
        //            AppContext.PublishInfos.Add(pinfo);
        //            PublishInfo[] infos = new PublishInfo[AppContext.PublishInfos.Count];
        //            AppContext.PublishInfos.CopyTo(infos);
        //            Array.Sort(infos);
        //            Array.Reverse(infos);
        //            infoListView.Items.Clear();
        //            foreach (PublishInfo info in infos)
        //            {
        //                if (AppSetting.SYMBOLS.Contains(info.Symbol))
        //                {
        //                    ListViewItem item = new ListViewItem(
        //                      new string[] {info.DateTime.ToString(), info.Type.ToString(), info.Content.ToString() }, 0);
        //                    infoListView.Items.Add(item);
        //                }
        //            }
        //        }
        //    }
        //}

        //public override void Execute()
        //{

        //}

    }
}
