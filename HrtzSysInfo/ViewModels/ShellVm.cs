using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Tools;

namespace HrtzSysInfo.ViewModels
{
    public class ShellVm
    {
        private RelayCommand _cmdCloseApp;
        private RelayCommand _cmdOpenSettings;

        public RelayCommand CmdCloseApp
        {
            get
            {
                return _cmdCloseApp ??
                       (_cmdCloseApp = new RelayCommand(ExecuteCmd_CloseApp, p => true));
            }
        }

        public RelayCommand CmdOpenSettings
        {
            get
            {
                return _cmdOpenSettings ??
                       (_cmdOpenSettings = new RelayCommand(ExecuteCmd_OpenSettings, p => true));
            }
        }

        private static void ExecuteCmd_CloseApp(object obj)
        {
            Application.Current.Shutdown(0);
        }

        private static void ExecuteCmd_OpenSettings(object obj)
        {
            if (!File.Exists(UserSettings.SettingsFilename)) return;

            try
            {
                Process.Start(UserSettings.SettingsFilename);
            }
            catch (Exception ex)
            {
                Logger.Write("ShellVm - ExecuteCmd_OpenSettings() - Exception raised", true, ex);
            }
        }
    }
}
