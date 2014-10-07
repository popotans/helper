using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Instrumentation;
using System.Management;
using Microsoft.Win32;
namespace Helper
{
    public class Win32Helper
    {
        public static string GetHardID()
        {
            string HDInfo = "";
            ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            foreach (ManagementObject mo in moc1)
            {
                HDInfo = (string)mo.Properties["Model"].Value;
            }
            return HDInfo;
        }

        /// <summary>

        /// 读注册表中指定键的值

        /// </summary>

        /// <param name="key">键名</param>

        /// <returns>返回键值</returns>

        private string ReadReg(string key)
        {
            string temp = "";
            try
            {
                RegistryKey myKey = Registry.LocalMachine;
                RegistryKey subKey = myKey.OpenSubKey(@"SOFTWARE/JX/Register");
                temp = subKey.GetValue(key).ToString();
                subKey.Close();
                myKey.Close();
                return temp;
            }
            catch { }
            return "";
        }

        /// <summary>

        /// 创建注册表中指定的键和值

        /// </summary>

        /// <param name="key">键名</param>

        /// <param name="value">键值</param>

        private void WriteReg(string key, string value)
        {
            try
            {
                RegistryKey rootKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE/JX/Register");
                rootKey.SetValue(key, value);
                rootKey.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }




    }
}
