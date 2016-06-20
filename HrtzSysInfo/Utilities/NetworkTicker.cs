using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Timers;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Tools;

namespace HrtzSysInfo.Utilities
{
    public class NetworkTicker : ObservableObject
    {
        public NetworkTicker()
        {
            Debug.WriteLine("Created Utility Class: NetworkTicker");

            Initialize();
        }

        private void Initialize()
        {
            var deviceName = string.Empty;

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces().Where(x => x.OperationalStatus.Equals(OperationalStatus.Up)).Where(x => x.NetworkInterfaceType.Equals(NetworkInterfaceType.Wireless80211) || x.NetworkInterfaceType.Equals(NetworkInterfaceType.Ethernet)))
                deviceName = ni.Description.Replace('(', '[').Replace(')', ']');

            if (string.IsNullOrEmpty(deviceName))
            {
                Logger.Write("NetworkTicker() - NetworkInterface.GetAllNetworkInterfaces() - string.IsNullOrEmpty(deviceName)", true);
            }
            else
            {
                _pcSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", deviceName);
                _pcRecieved = new PerformanceCounter("Network Interface", "Bytes Received/sec", deviceName);
            }

            // Traffic timer
            var timerTraffic = new Timer { Interval = UserSettings.GlobalSettings.PollingRateNetwork };
            timerTraffic.Elapsed += timerTraffic_Elapsed;
            timerTraffic.Start();

            // External IP timer
            var timerExternalIp = new Timer { Interval = UserSettings.GlobalSettings.PollingRateIpExternal };
            timerExternalIp.Elapsed += timerExternalIp_Elapsed;
            timerExternalIp.Start();

            // Internal IP timer
            var timerInternalIp = new Timer { Interval = UserSettings.GlobalSettings.PollingRateIpInternal };
            timerInternalIp.Elapsed += timerInternalIp_Elapsed;
            timerInternalIp.Start();

            // Get the values at init
            EventExtensions.FireEvent(timerTraffic, "onIntervalElapsed", this, null);
            EventExtensions.FireEvent(timerExternalIp, "onIntervalElapsed", this, null);
            EventExtensions.FireEvent(timerInternalIp, "onIntervalElapsed", this, null);
        }

        // Private fields
        private PerformanceCounter _pcSent;
        private PerformanceCounter _pcRecieved;
        private double _sent;
        private double _recieved;
        private string _externalIpAddress;
        private string _interalIpAddress;

        // Public properties
        public double Sent
        {
            get { return _sent; }
            set { SetField(ref _sent, value); }
        }

        public double Recieved
        {
            get { return _recieved; }
            set { SetField(ref _recieved, value); }
        }

        public string ExternalIpAddress
        {
            get { return _externalIpAddress; }
            set { SetField(ref _externalIpAddress, value); }
        }

        public string InternalIpAddress
        {
            get { return _interalIpAddress; }
            set { SetField(ref _interalIpAddress, value); }
        }

        // Methods
        private void timerTraffic_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!UserSettings.GlobalSettings.VisibilityNetwork) return;

            // Up
            if (UserSettings.GlobalSettings.VisibilityNetworkUpload)
            {
                double sentValue = 0;
                string sentValueString = null;

                if (_pcSent != null)
                    sentValueString = _pcSent.NextValue().ToString(CultureInfo.InvariantCulture);

                if (sentValueString != null)
                    double.TryParse(sentValueString, out sentValue);

                Sent = sentValue;
            }

            // Down
            if (UserSettings.GlobalSettings.VisibilityNetworkDownload)
            {
                double recievedValue = 0;
                string recievedValueString = null;

                if (_pcRecieved != null)
                    recievedValueString = _pcRecieved.NextValue().ToString(CultureInfo.InvariantCulture);

                if (recievedValueString != null)
                    double.TryParse(recievedValueString, out recievedValue);

                Recieved = recievedValue;
            }
        }

        private async void timerExternalIp_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!UserSettings.GlobalSettings.VisibilityNetworkExternalIp) return;

            string ip = null;

            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    ip = await webClient.DownloadStringTaskAsync("http://icanhazip.com/");
                }
            }
            catch (Exception ex)
            {
                Logger.Write("NetworkTicker() - webClient.DownloadStringTaskAsync(\"http://icanhazip.com/\") - Exception raised", true, ex);
            }
            finally
            {
                ExternalIpAddress = !string.IsNullOrWhiteSpace(ip)
                    ? ip.Trim()
                    : "N/A";
            }
        }

        private void timerInternalIp_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!UserSettings.GlobalSettings.VisibilityNetworkInternalIp) return;

            string ip = null;

            try
            {
                ip = NetworkExtensions.GetLocalIpAddress();
            }
            catch (Exception ex)
            {
                Logger.Write("NetworkTicker() - NetworkExtensions.GetLocalIpAddress() - Exception raised", true, ex);
            }
            finally
            {
                InternalIpAddress = !string.IsNullOrEmpty(ip)
                    ? ip
                    : "N/A";
            }
        }
    }
}
