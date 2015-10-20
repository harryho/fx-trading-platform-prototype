using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fxpa
{
    class NetHelper
    {
        static string MSG_START = "#";
        static string MSG_DELIMITER = "|-|";
        static string _NULL_ = "_NULL_";

        public static string BuildMsg(Protocol p, string[] contents)
        {
            StringBuilder sb = new StringBuilder(MSG_START);
            sb.Append(MSG_DELIMITER).Append(AppSetting.PROTOCOL_VERSION);
            sb.Append(MSG_DELIMITER).Append(p.ToString());
            int count = 0;
            StringBuilder content = new StringBuilder();
            foreach (string s in contents)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    content.Append(MSG_DELIMITER).Append(s);
                    count += 1;
                }
            }
            sb.Append(MSG_DELIMITER).Append(count.ToString());
            if(!string.IsNullOrEmpty(content.ToString()))
                  sb.Append(content.ToString());
            sb.Append(MSG_DELIMITER);
            return sb.ToString();
        }

        public static string BuildRawMsg(Protocol p, string[] contents)
        {
            StringBuilder sb = new StringBuilder(MSG_START);
            sb.Append(MSG_DELIMITER).Append(AppSetting.PROTOCOL_VERSION);
            sb.Append(MSG_DELIMITER).Append(p.ToString());
            int count = 0;
            StringBuilder content = new StringBuilder();
            foreach (string s in contents)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    content.Append(MSG_DELIMITER).Append(s);
                    count += 1;
                }
            }
            sb.Append(MSG_DELIMITER).Append(count.ToString());
            if (!string.IsNullOrEmpty(content.ToString()))
                sb.Append(content.ToString());
            sb.Append(MSG_DELIMITER);
            return sb.ToString();
        }

      


    }
}
