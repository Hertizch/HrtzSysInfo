using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using HrtzSysInfo.Extensions;

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
            Application.Current.MainWindow.Close();
        }

        private static void ExecuteCmd_OpenSettings(object obj)
        {
            if (File.Exists(GlobalSettingsVm.Instance.SettingsFilename))
                try
                {
                    Process.Start(GlobalSettingsVm.Instance.SettingsFilename);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            /*var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();*/
        }
    }
}
