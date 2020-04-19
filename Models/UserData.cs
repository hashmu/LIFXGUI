using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LIFXGUI.Models
{
    class UserData
    {
        #region Members
        private readonly string _filePath = "userdata";
        public string Name;
        public string Region;
        public string Token;
        #endregion

        #region Constructors
        public UserData()
        {

        }
        #endregion


        #region Methods
        private string GetLocalFilePath(string fileName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData + "/LIFXGUI", fileName);
        }

        public void LoadSettings()
        {
            string path = GetLocalFilePath(_filePath);
            if (File.Exists(path))
            {
                UserData ud = JsonConvert.DeserializeObject<UserData>(File.ReadAllText(path));
                Name = DataProtection.Unprotect(ud.Name);
                Region = DataProtection.Unprotect(ud.Region);
                Token = DataProtection.Unprotect(ud.Token);
            }
        }
        public void SaveSettings()
        {
            UserData ud = new UserData()
            {
                Name = DataProtection.Protect(this.Name),
                Region = DataProtection.Protect(this.Region),
                Token = DataProtection.Protect(this.Token)
            };
            string json = JsonConvert.SerializeObject(ud);
            string path = GetLocalFilePath(_filePath);
            File.WriteAllText(path, json);
        }


        public void SaveSettings(UserData userData)
        {
            UserData ud = new UserData()
            {
                Name = DataProtection.Protect(userData.Name),
                Region = DataProtection.Protect(userData.Region),
                Token = DataProtection.Protect(userData.Token)
            };
            string json = JsonConvert.SerializeObject(ud);
            string path = GetLocalFilePath(_filePath);
            File.WriteAllText(path, json);
        }
        #endregion
    }
}