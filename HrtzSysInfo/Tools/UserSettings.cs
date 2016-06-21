using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Models;

namespace HrtzSysInfo.Tools
{
    public class UserSettings
    {
        public UserSettings()
        {
            Debug.WriteLine("Created Class: UserSettings");

            GlobalSettings.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            GlobalSettings.PollingRateDateTime = 1000;
            GlobalSettings.PollingRateWeek = 60000;
            GlobalSettings.PollingRateCpu = 1000;
            GlobalSettings.PollingRateRam = 1000;
            GlobalSettings.PollingRateDrives = 30000;
            GlobalSettings.PollingRateTemps = 1000;
            GlobalSettings.PollingRateNetwork = 1000;
            GlobalSettings.PollingRateIpInternal = 60000;
            GlobalSettings.PollingRateIpExternal = 600000;
            GlobalSettings.FormattingDate = "dddd dd.MM.yyyy";
            GlobalSettings.FormattingTime = "HH:mm";
            GlobalSettings.FormattingTempUnit = "c";
            GlobalSettings.UiTop = 0;
            GlobalSettings.UiLeft = 0;
            GlobalSettings.UiWidth = 200;
            GlobalSettings.UiShowInTaskbar = false;
            GlobalSettings.UiRunAtStartup = true;
            GlobalSettings.UiSectionSeparator = 1;
            GlobalSettings.VisibilitySectionHeaders = true;
            GlobalSettings.VisibilityNetwork = true;
            GlobalSettings.VisibilitySystem = true;
            GlobalSettings.VisibilityDrives = true;
            GlobalSettings.VisibilityDateTime = true;
            GlobalSettings.VisibilityNetworkExternalIp = true;
            GlobalSettings.VisibilityNetworkInternalIp = true;
            GlobalSettings.VisibilityNetworkUpload = true;
            GlobalSettings.VisibilityNetworkDownload = true;
            GlobalSettings.VisibilitySystemCpuUsage = true;
            GlobalSettings.VisibilitySystemRamUsage = true;
            GlobalSettings.VisibilitySystemCpuTemp = true;
            GlobalSettings.VisibilitySystemGpuTemp = true;
            GlobalSettings.NetworkSentMaxValue = 4089446;
            GlobalSettings.NetworkRecieveMaxValue = 13631488;
            GlobalSettings.SystemTempCpuMaxValue = 90;
            GlobalSettings.SystemTempGpuMaxValue = 80;

            RemoveOutdatedSettingsFile();

            if (File.Exists(SettingsFilename))
            {
                LoadSettings();
                return;
            }

            SaveSettings();
        }

        public static GlobalSettings GlobalSettings { get; set; } = new GlobalSettings();

        public static readonly string SettingsFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.xml");

        public static void LoadSettings()
        {
            try
            {
                GlobalSettings = XmlExtensions.DeserializeFromXml<GlobalSettings>(SettingsFilename);
                Debug.WriteLine("Loaded settings from file: " + SettingsFilename);
            }
            catch (Exception ex)
            {
                Logger.Write("UserSettings() - LoadSettings() - Exception raised", true, ex);
            }
        }

        public static void SaveSettings()
        {
            try
            {
                XmlExtensions.SerializeToXml(GlobalSettings, SettingsFilename);
                Debug.WriteLine("Saved settings to file: " + SettingsFilename);
            }
            catch (Exception ex)
            {
                Logger.Write("UserSettings() - SaveSettings() - Exception raised", true, ex);
            }
        }

        public static void RemoveOutdatedSettingsFile()
        {
            var localVersion = Assembly.GetExecutingAssembly().GetName().Version;
            if (GlobalSettings.Version != null)
            {
                var remoteVersion = Version.Parse(GlobalSettings.Version);

                if (GlobalSettings.Version != null && (localVersion > remoteVersion && File.Exists(SettingsFilename)))
                {
                    File.Delete(SettingsFilename);
                    Debug.WriteLine("Deleted old settings file.");
                }
            }
        }
    }
}
