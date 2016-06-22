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

            if (File.Exists(SettingsFilename))
            {
                LoadSettings();

                // Delete settings file if unmatched versions
                var runningVersion = Assembly.GetExecutingAssembly().GetName().Version;
                if (runningVersion > Version.Parse(GlobalSettings.Version))
                {
                    File.Delete(SettingsFilename);
                    Debug.WriteLine("Deleted old settings file.");

                    GlobalSettings = GetDefaultSettings();
                }
            }
            else
            {
                GlobalSettings = GetDefaultSettings();
            }

            SaveSettings();
        }

        private static GlobalSettings GetDefaultSettings()
        {
            Debug.WriteLine("Applied default settings");

            return new GlobalSettings
            {
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
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
                FormattingTempUnit = "c",
                UiTop = 0,
                UiLeft = 0,
                UiWidth = 200,
                UiShowInTaskbar = false,
                UiRunAtStartup = true,
                UiSectionSeparator = 1,
                VisibilitySectionHeaders = true,
                VisibilityNetwork = true,
                VisibilitySystem = true,
                VisibilityDrives = true,
                VisibilityDateTime = true,
                VisibilityNetworkExternalIp = true,
                VisibilityNetworkInternalIp = true,
                VisibilityNetworkUpload = true,
                VisibilityNetworkDownload = true,
                VisibilitySystemCpuUsage = true,
                VisibilitySystemRamUsage = true,
                VisibilitySystemCpuTemp = true,
                VisibilitySystemGpuTemp = true,
                VisibilitySystemGpuLoad = true,
                NetworkSentMaxValue = 4089446,
                NetworkRecieveMaxValue = 13631488,
                SystemTempCpuMaxValue = 90,
                SystemTempGpuMaxValue = 80
            };
        }

        public static GlobalSettings GlobalSettings { get; set; }

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

        public static void MergeGlobalSettings()
        {
            /*
            var runningVersion = Assembly.GetExecutingAssembly().GetName().Version;

            if (GlobalSettings.Version != null)
            {
                var savedVersion = Version.Parse(GlobalSettings.Version);

                if (GlobalSettings.Version != null && (runningVersion > savedVersion && File.Exists(SettingsFilename)))
                {
                    File.Delete(SettingsFilename);
                    Debug.WriteLine("Deleted old settings file.");
                }
            }
            */
        }
    }
}
