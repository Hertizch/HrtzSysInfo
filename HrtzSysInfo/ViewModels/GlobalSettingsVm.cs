using System;
using System.Diagnostics;
using System.IO;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Models;

namespace HrtzSysInfo.ViewModels
{
    public class GlobalSettingsVm : ObservableObject
    {
        public GlobalSettingsVm()
        {
            Debug.WriteLine("Created ViewModel: GlobalSettingsVm");

            // Init default values
            GlobalSettings = new GlobalSettings
            {
                PollingRateDateTime = 1000,
                PollingRateWeek = 60000,
                PollingRateCpu = 1000,
                PollingRateRam = 1000,
                PollingRateDrives = 30000,
                PollingRateTemps = 1000,
                PollingRateNetwork = 1000,
                PollingRateIpInternal = 60000,
                PollingRateIpExternal = 600000,
                FormattingDate = "dddd dd.MM.yyyy",
                FormattingTime = "HH:mm",
                UiTop = 0,
                UiLeft = 0,
                UiWidth = 200,
                UiShowInTaskbar = false,
                UiRunAtStartup = true,
                VisibilityNetwork = true,
                VisibilitySystem = true,
                VisibilityDrives = true,
                VisibilityDateTime = true,
                NetworkSentMaxValue = 4089446,
                NetworkRecieveMaxValue = 13631488,
                SystemTempCpuMaxValue = 90,
                SystemTempGpuMaxValue = 80
            };

            // Create global settings file if not exist
            if (!File.Exists(SettingsFilename))
                if (CmdSaveSettings.CanExecute(null))
                    CmdSaveSettings.Execute(null);

            // Load global settings
            if (CmdLoadSettings.CanExecute(null))
                CmdLoadSettings.Execute(null);
        }

        // Private fields
        private RelayCommand _cmdSaveSettings;
        private RelayCommand _cmdLoadSettings;
        private GlobalSettings _globalSettings;

        // Public fields
        public string SettingsFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.xml");

        // Public properties
        public static GlobalSettingsVm Instance { get; } = new GlobalSettingsVm();

        public GlobalSettings GlobalSettings
        {
            get { return _globalSettings; }
            set { SetField(ref _globalSettings, value); }
        }

        // Commands
        public RelayCommand CmdSaveSettings
        {
            get
            {
                return _cmdSaveSettings ??
                       (_cmdSaveSettings = new RelayCommand(ExecuteCmd_SaveSettings, p => true));
            }
        }

        public RelayCommand CmdLoadSettings
        {
            get
            {
                return _cmdLoadSettings ??
                       (_cmdLoadSettings = new RelayCommand(ExecuteCmd_LoadSettings, p => File.Exists(SettingsFilename)));
            }
        }

        // Methods
        private void ExecuteCmd_SaveSettings(object obj)
        {
            XmlExtensions.SerializeToXml(GlobalSettings, SettingsFilename);
            Debug.WriteLine("Saved settings to file: " + SettingsFilename);
        }

        private void ExecuteCmd_LoadSettings(object obj)
        {
            GlobalSettings = XmlExtensions.DeserializeFromXml<GlobalSettings>(SettingsFilename);
            Debug.WriteLine("Loaded settings from file: " + SettingsFilename);
        }
    }
}
