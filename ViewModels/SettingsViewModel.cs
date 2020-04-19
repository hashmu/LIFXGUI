using LIFXGUI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LIFXGUI.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            UserData ud = new UserData();
            ud.LoadSettings();
            Name = ud.Name;
            Region = ud.Region;
            Token = ud.Token;
        }
        
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _region;
        public string Region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
                OnPropertyChanged("Region");
            }
        }

        private string _token;
        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
                OnPropertyChanged("Token");
            }
        }

        private string _status = "Settings";
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public ICommand SaveSettingsCommand { get { return new DelegateCommand(SaveSettings); } }

        public void SaveSettings()
        {
            UserData ud = new UserData
            {
                Name = _name,
                Region = _region,
                Token = _token
            };
            ud.SaveSettings(ud);
            Status = "Settings Saved.";
        }
    }
}
