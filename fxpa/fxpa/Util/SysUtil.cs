using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Management;
using System.Windows.Forms;

namespace fxpa
{
    public enum Win32Stuff
    {
        Win32_BaseBoard,
        Win32_DiskDrive,
        Win32_OperatingSystem,
        Win32_Processor,
        Win32_ComputerSystem,
        Win32_StartupCommand,
        Win32_ProgramGroup,
        Win32_SystemDevices,
	   	Win32_NetworkAdapterConfiguration
    }

    public class SysUtil
    {


        public  static ArrayList GetFullStuffInfo(string queryObject)
        {
            ManagementObjectSearcher searcher;
            int i = 0;
            ArrayList hd = new ArrayList();
            try
            {
                searcher = new ManagementObjectSearcher("SELECT * FROM " + queryObject);
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    i++;
                    PropertyDataCollection searcherProperties =
                      wmi_HD.Properties;
                    foreach (PropertyData sp in searcherProperties)
                    {
                        hd.Add(sp);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return hd;
        }

        public static PropertyData GetSpecialStuffPairInfo(string queryObject, string target)
        {
            ManagementObjectSearcher searcher;
            int i = 0;
            ArrayList hd = new ArrayList();
            PropertyData pd = null;
            try
            {
                StringBuilder sb = new StringBuilder(" SELECT ");
                sb.Append(target).Append("  FROM ").Append(queryObject);

                searcher = new ManagementObjectSearcher(sb.ToString());
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    //i++;
                    PropertyDataCollection searcherProperties =
                      wmi_HD.Properties;
                    pd = searcherProperties[target];
                    break;
                }
                searcher.Dispose();
            }
            catch (Exception ex)
            {
                return null;
            }
            return pd;
        }

        public static string GetSpecialStuffValueInfo(string queryObject, string target)
        {
            ManagementObjectSearcher searcher;
            int i = 0;
            ArrayList hd = new ArrayList();
            PropertyData pd = null;
            try
            {
                StringBuilder sb = new StringBuilder(" SELECT ");
                sb.Append(target).Append("  FROM ").Append(queryObject);

                searcher = new ManagementObjectSearcher(sb.ToString());
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    //i++;
                    PropertyDataCollection searcherProperties =wmi_HD.Properties;
                    pd = searcherProperties[target];
                    break;
                }
                searcher.Dispose();
            }
            catch (Exception ex)
            {
                return null;
            }
            if (pd != null && pd.Value != null)
                return pd.Value.ToString();
            else
                return null;
        }

        public static List<PropertyData> GetPartialStuffInfo(string stuff, string[] targets)
        {
            List<PropertyData> hd = new List<PropertyData>();
            try
            {
                StringBuilder sb = new StringBuilder(" SELECT ");
                foreach (string t in targets)
                {
                    sb.Append(t).Append("  ");
                }
                sb.Append("  FROM ").Append(stuff);

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(sb.ToString());
                ManagementObjectCollection information = searcher.Get();
                foreach (ManagementObject obj in information)
                {
                    // Retrieving the properties (columns)    
                    // Writing column name then its value    

                    foreach (PropertyData data in obj.Properties)
                    {
                        Console.WriteLine("{0} = {1}", data.Name, data.Value);
                        hd.Add(data);
                    }
                }
                searcher.Dispose();
            }
            catch (Exception e)
            {
                return null;
            }
            return hd;
        }

    }
}
