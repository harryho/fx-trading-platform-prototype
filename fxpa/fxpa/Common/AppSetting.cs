using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
    class AppSetting
    {
        public static string PROTOCOL_VERSION = "01";
        public static string VERSION = "1.0";
        public static string A_OR_C = "A";// admin or client 
        public static string NORMAL_ACCOUNT= "0";
        public static string TEST_ACCOUNT = "1";
        public static string ADMIN_ACCOUNT = "-99";


        public static DateTime RELEASE_DATE = DateTime.Parse("2010-12-12 16:30");

        public static string USER;
        public static string PWD;
        public static string RECONN_USER;
        public static string RECONN_PWD;
        public static string SOFTWARE_TYPE;
        public static bool IS_PERMANENT=false;
        public static string STATUS;
        public static string LATEST_VERSION;
        public static DateTime END_TIME;
        public static DateTime START_TIME;

        public static string DS_SERVER_IP;
        public static int DS_SERVER_PORT;
        public static string LOGIN_TOKEN;
        public static string MACHINE_INFO;
        public static Interval[] INTEVALS = null;
        public static Symbol[] SYMBOLS;

        public static int PAYMENT_TYPE = 0;
        public static int TotalPoints = 0;
        public static int RestPoints = 0;
        public static DateTime EndDate;
        public static string PAYMENT_END_DATE = MsgHandler.NULL;

    }
}
