using LIFXGUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LIFXGUI.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region Members
        private UserData userData;
        private LIFXAPI lifx;
        //1500 to 6500 kelvin
        private int _colour = 4000;
        public int Colour
        {
            get
            {
                return _colour;
            }
            set
            {
                _colour = value;
                OnPropertyChanged("Colour");
                OnPropertyChanged("ColourAndBrightnessString");
            }
        }
        //0.0 to 1.0
        private int _brightness = 1;
        public int Brightness
        {
            get
            {
                return _brightness;
            }
            set
            {
                _brightness = value;
                OnPropertyChanged("Brightness");
                OnPropertyChanged("ColourAndBrightnessString");
            }
        }
        public string ColourAndBrightnessString
        {
            get
            {
                return "Colour: " + Colour + "K, Brightness: " + ((Brightness == 10) ? "1.0" : "0." + Brightness);
            }
        }
        //status bar text
        private string _status = "";
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

        List<LightPreset> presets = new List<LightPreset>();
        #endregion


        #region Constructors
        public MainViewModel()
        {
            userData = new UserData();
            userData.LoadSettings();
            lifx = new LIFXAPI(userData.Token);
            try
            {
                GetLightSettings();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error");
            }
            //low light
            presets.Add(new LightPreset(1500, 1));
            //medium light
            presets.Add(new LightPreset(2500, 5));
            //bright light
            presets.Add(new LightPreset(6500, 10));
        }

        private async void GetLightSettings()
        {
            string json = await lifx.CurrentSettings();
            try
            {
                IEnumerable<Light> lights = JsonConvert.DeserializeObject<IEnumerable<Light>>(json);
                if (lights.Count() > 0)
                {
                    Light light = lights.First();
                    Brightness = (int)(light.brightness * 10);
                    Colour = light.color.kelvin;
                    Status = "Light Settings Loaded";
                }
            }
            catch
            {
                Status = "Failed To Get Settings";
            }
            
        }
        #endregion

        #region Methods
        async void ToggleLight()
        {
            Status = await lifx.Toggle();
        }

        async void ToggleLight(bool toggle)
        {
            Status = await lifx.Toggle(toggle);
        }

        async void SetLight()
        {
            string brightnessString = (Brightness == 10) ? "1.0" : "0." + Brightness;
            Status = await lifx.Set(Colour, brightnessString);
        }

        async void GetSettings()
        {
            string settings = await lifx.CurrentSettings();
        }

        void Preset(int idx)
        {
            Brightness = presets[idx].Brightness;
            Colour = presets[idx].Colour;
            SetLight();
        }

        void LoadSettings()
        {
            userData.LoadSettings();
            lifx = new LIFXAPI(userData.Token);
            GetLightSettings();
        }

        void SaveSettings()
        {
            userData.SaveSettings();
        }

        void OpenSettings()
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Closed += (sender, e) => LoadSettings();
            settingsWindow.ShowDialog();
        }
        #endregion


        #region Commands
        public ICommand ToggleLightCommand { get { return new DelegateCommand(ToggleLight); } }
        public ICommand LightOnCommand { get { return new DelegateCommand(()=>ToggleLight(true)); } }
        public ICommand LightOffCommand { get { return new DelegateCommand(()=>ToggleLight(false)); } }
        public ICommand SetLightCommand { get { return new DelegateCommand(SetLight); } }
        public ICommand GetSettingsCommand { get { return new DelegateCommand(GetSettings); } }
        public ICommand QuitCommand { get { return new DelegateCommand(() => App.Current.Shutdown()); } }
        public ICommand PresetCommand { get { return new ParameteredCommand<string>((x) => Preset((int.Parse(x)))); } }
        public ICommand OpenSettingsCommand { get { return new DelegateCommand(OpenSettings); } }
        #endregion
    }

    public class LightPreset
    {
        public int Colour { get; set; }
        public int Brightness { get; set; }

        public LightPreset(int colour, int brightness)
        {
            Colour = colour;
            Brightness = brightness;
        }
    }
}
