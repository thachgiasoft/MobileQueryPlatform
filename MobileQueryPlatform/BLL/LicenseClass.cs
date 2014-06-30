using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace BLL
{
    internal class LicenseClass
    {
        internal static string GenerateLisense(string expDate, string serialNo, string reportNumber)
        {
            //string license = Des.EncryStrHex(expDate, reportNumber.ToString());
            //license = Des.EncryStrHex(license + reportNumber, serialNo);
            //return license;
            char[] date = Des.EncryStrHex(expDate, serialNo).ToCharArray();
            char[] number = Des.EncryStrHex(reportNumber, serialNo).ToCharArray();
            char[] license = new char[32];
            for (int i = 0; i < 16; i++)
            {
                license[i * 2] = date[i];
                license[i * 2 + 1] = number[i];
            }
            return new string(license);
        }
        internal static void AnalyzeLisense(string license, string serialNo, out string expDate, out string reportNumber)
        {
            char[] chars = license.ToCharArray();
            char[] date = new char[16];
            char[] number = new char[16];
            for (int i = 0; i < 16; i++)
            {
                date[i] = chars[i * 2];
                number[i] = chars[i * 2 + 1];
            }
            expDate = Des.DecryStrHex(new string(date), serialNo);
            reportNumber = Des.DecryStrHex(new string(number), serialNo);
        }

        ///<summary>
        /// 通过NetworkInterface读取网卡Mac
        ///</summary>
        ///<returns></returns>
        public static string GetMacByNetworkInterface()
        {
            List<string> macs = new List<string>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                {
                    continue;
                }
                macs.Add(ni.GetPhysicalAddress().ToString());
            }
            if (macs.Count > 0)
            {
                return macs[0];
            }
            else
            {
                return null;
            }
        }
   

    }
}
