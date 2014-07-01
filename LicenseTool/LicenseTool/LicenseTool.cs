using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Management;

namespace LicenseTool
{
    internal class LicenseTool
    {
        internal static string GenerateLisense(string expDate, string serialNo, string reportNumber)
        {
            //string license = Des.EncryStrHex(expDate, reportNumber.ToString());
            //license = Des.EncryStrHex(license + reportNumber, serialNo);
            //return license;
            int r = new Random().Next(255, 4095);
            string key =Convert.ToString(r, 16).ToUpper().PadLeft(3, '0');

            char[] keys=Des.EncryStrHex(key,serialNo).ToCharArray();
            char[] date = Des.EncryStrHex(expDate, key).ToCharArray();
            char[] number= Des.EncryStrHex(reportNumber, key).ToCharArray();
            char[] license = new char[48];

            for (int i = 0; i < 16; i++)
            {
                license[i * 3] = date[i];
                license[i * 3 + 1] = number[i];
                license[i * 3 + 2] = keys[i];
            }
            return new string(license);
        }
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
            string key =Des.DecryStrHex( new string(keys),serialNo);
            expDate = Des.DecryStrHex(new string(date), key);
            reportNumber = Des.DecryStrHex(new string(number), key);
        }

        ///<summary>
        /// 通过NetworkInterface读取网卡Mac
        ///</summary>
        ///<returns></returns>
        internal static string GetMacByNetworkInterface()
        {
          List<string> macs =new List<string>();
          NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
          foreach (NetworkInterface ni in interfaces)
          {
              if (ni.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
              {
                  continue;
              }
            macs.Add(ni.GetPhysicalAddress().ToString());
          }
          if (macs.Count > 0) {
              return macs[0];
          }
          else
          {
              return null;
          }
        }
        internal static string getCPUID()
        {
            string cpuInfo = "";//cpu序列号
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
            }
            return cpuInfo;
        }

        internal static string getMainboardID()
        {
            ManagementClass mc = new ManagementClass("Win32_BaseBoard");
            ManagementObjectCollection moc = mc.GetInstances();
            string strID = null;
            foreach (ManagementObject mo in moc)
            {
                strID = mo.Properties["SerialNumber"].Value.ToString();
                break;
            }
            return strID;
        }
        internal static string getHardDiskID()
        {
            ManagementClass mc = new ManagementClass("Win32_PhysicalMedia");
            //网上有提到，用Win32_DiskDrive，但是用Win32_DiskDrive获得的硬盘信息中并不包含SerialNumber属性。  
            ManagementObjectCollection moc = mc.GetInstances();
            string strID = null;
            foreach (ManagementObject mo in moc)
            {
                strID = mo.Properties["SerialNumber"].Value.ToString();
                break;
            }
            return strID;
        }
    }
}
