using System.IO;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Models;

namespace HrtzSysInfo.ViewModels
{
    public class GlobalSettingsVm : ObservableObject
    {
        public GlobalSettingsVm()
        {
            GlobalSettings = new GlobalSettings();
        }

        // Private fields
        private RelayCommand _cmdSaveSettings;
        private RelayCommand _cmdLoadSettings;
        private GlobalSettings _globalSettings;
        private string _settingsFilename = "settings.xml";

        // Public properties
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
                       (_cmdLoadSettings = new RelayCommand(ExecuteCmd_LoadSettings, p => File.Exists(_settingsFilename)));
            }
        }

        // Methods
        private void ExecuteCmd_SaveSettings(object obj)
        {
            XmlExtensions.SerializeToXml(GlobalSettings, _settingsFilename);
        }

        private void ExecuteCmd_LoadSettings(object obj)
        {
            GlobalSettings = XmlExtensions.DeserializeFromXml<GlobalSettings>(_settingsFilename);
        }
    }
}
