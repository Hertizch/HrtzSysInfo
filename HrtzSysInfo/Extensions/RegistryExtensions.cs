using System.Reflection;
using Microsoft.Win32;

namespace HrtzSysInfo.Extensions
{
    public static class RegistryExtensions
    {
        public static void RegisterInStartup(string applicationName, bool register)
        {
            var registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (register)
            {
                registryKey?.SetValue(applicationName, Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                registryKey?.DeleteValue(applicationName);
            }
        }
    }
}
