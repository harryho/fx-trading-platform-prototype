using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fxpa
{
    public class PaymentHandler : MsgHandler
    {
        FXPA fxClient;

        public FXPA FXClient
        {
            get { return fxClient; }
            set { fxClient = value; }
        }


        public PaymentHandler(Client client)
            : base(client)
        {
        }
       
       public override void Send(object msg)
        {
            Console.WriteLine(" PaymentHandler  Send ~~~~~~~~~~~ ");
            LogUtil.Info(" PaymentHandler  Send ~~~~~~~~~~~ ");
            Client.Send(msg);
        }

      public  static string TOTAL_POINT_LABEL = "Total amount：";
       public static string REST_POINT_LABEL = "Credit: ";
       public static string END_DATE_LABEL = "Expiry Time：";
       public static string END_DATE_MSG = "Your account has been expired，please charge it to avoid any lost.!";
       public override void Receive(object msg)
       {
           string[] msgs = (string[])msg;

           Protocol protocol = AppUtil.ParseProtocol(msgs[1]);
           int paramAmount = AppUtil.StringToInt(msgs[2]);
           if (AppSetting.PAYMENT_TYPE != 0 || AppSetting.PAYMENT_TYPE!=4)
           {
               if (paramAmount == 4 && msgs.Length>=6)
               {
                   LogUtil.Info(" Payment Info content " + msgs[3] + msgs[4] + msgs[5] + msgs[6]);

                   PaymentType ptype = AppUtil.ParsePaymentType(msgs[3]);
                   string timeStr = msgs[4];
                   string totalStr = msgs[5];
                   string restStr = msgs[6];
                   MethodInvoker mi = null;
                       try
                       {
                           switch (ptype)
                           {
                               case PaymentType.Point: // Count points
                                   AppSetting.TotalPoints = int.Parse(totalStr);
                                   AppSetting.RestPoints = int.Parse(restStr);
                                   mi =new  MethodInvoker(PaymentLabelUpdate);
                                   AppContext.TradeExpertControl.BeginInvoke(mi);
                                   break;
                               case PaymentType.Month:  // 
                               case PaymentType.Year:  // 
                                   if (timeStr != NULL)
                                   {
                                       AppSetting.PAYMENT_END_DATE = timeStr;
                                       AppSetting.EndDate = DateTime.Parse(timeStr);
                                   }
                                   mi = new MethodInvoker(PaymentLabelUpdate);
                                   AppContext.TradeExpertControl.BeginInvoke(mi);
                                   break;
                           }
                       }
                       catch (Exception e)
                       {
                           LogUtil.Error(" Protocol " + protocol.ToString() + e.StackTrace);
                       }
                   } 
           }
       }

       private void PaymentLabelUpdate()
       {
           PaymentType pt = AppUtil.ParsePaymentType(AppSetting.PAYMENT_TYPE.ToString());

           if (pt == PaymentType.Point)
           {
               //AppContext.TotalLabel.Text = TOTAL_POINT_LABEL + "  " + AppSetting.TotalPoints.ToString();
               //AppContext.RestLabel.Text = REST_POINT_LABEL + "  " + AppSetting.RestPoints.ToString();
               AppContext.TotalLabel.Text = TOTAL_POINT_LABEL + AppSetting.TotalPoints.ToString()+"   " +REST_POINT_LABEL + AppSetting.RestPoints.ToString();
               AppContext.TotalLabel.Refresh();
               AppContext.RestLabel.Refresh();
           }
           else if (pt == PaymentType.Year || pt == PaymentType.Month)
           {
               if (AppSetting.PAYMENT_END_DATE != NULL)
               {
                   if (AppSetting.START_TIME.Subtract(AppSetting.EndDate) > TimeSpan.FromHours(1))
                            AppContext.TotalLabel.Text = PaymentHandler.END_DATE_MSG;
                   else
                            AppContext.TotalLabel.Text = PaymentHandler.END_DATE_LABEL + "  " + AppSetting.EndDate.ToShortDateString();
               }
               else
                   AppContext.TotalLabel.Text = PaymentHandler.END_DATE_MSG;
               AppContext.TotalLabel.Refresh();
           }
       }
       //int errCode = 0;

       public static void RefreshPaymentInfo()
       {
           AppContext.PaymentHandler.Execute();
       }

       public override void Execute()
       {
           string request = NetHelper.BuildMsg(Protocol.C0001_5, new string[] {});
           Send(request);
       }
    }
}
