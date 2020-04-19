using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace LIFXGUI.Models
{
    class DataProtection
    {
        public static string Protect(string str)
        {
            try
            {
                byte[] entropy = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().FullName);
                byte[] data = Encoding.ASCII.GetBytes(str);
                string protectedData = Convert.ToBase64String(ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser));
                return protectedData;
            }
            catch
            {
                return "";
            }
        }

        public static string Unprotect(string str)
        {
            try
            {
                byte[] protectedData = Convert.FromBase64String(str);
                byte[] entropy = Encoding.ASCII.GetBytes(Assembly.GetExecutingAssembly().FullName);
                string data = Encoding.ASCII.GetString(ProtectedData.Unprotect(protectedData, entropy, DataProtectionScope.CurrentUser));
                return data;
            }
            catch
            {
                return "";
            }
        }
    }
}
