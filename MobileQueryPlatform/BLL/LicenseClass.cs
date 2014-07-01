using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace BLL
{
    internal class LicenseClass
    {
        internal static void AnalyzeLisense(string license, string serialNo, out string expDate, out string reportNumber)
        {
            char[] chars = license.ToCharArray();
            char[] date = new char[16];
            char[] keys = new char[16];
            char[] number = new char[16];

            for (int i = 0; i < 16; i++)
            {
                date[i] = chars[i * 3];
                number[i] = chars[i * 3 + 1];
                keys[i] = chars[i * 3 + 2];
            }
            string key = Des.DecryStrHex(new string(keys), serialNo);
            expDate = Des.DecryStrHex(new string(date), key);
            reportNumber = Des.DecryStrHex(new string(number), key);
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
