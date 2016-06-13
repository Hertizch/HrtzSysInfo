﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Timers;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Properties;

namespace HrtzSysInfo.Utilities
{
    public class NetworkTicker : ObservableObject
    {
        public NetworkTicker()
        {
            Debug.WriteLine("Created Utility Class: NetworkTicker");

            if (Settings.Default.SectionVisibility_Network)
                Initialize();
        }

        private void Initialize()
        {
            var pcc = new PerformanceCounterCategory("Network Interface");
            var instanceNames = pcc.GetInstanceNames();

            /*string instance = null;
            foreach (var instanceName in instanceNames.Where(instanceName => instanceName.Contains("Intel")))
                instance = instanceName;*/

            Debug.WriteLine(instanceNames[0]);

            _pcSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instanceNames[0]);
            _pcRecieved = new PerformanceCounter("Network Interface", "Bytes Received/sec", instanceNames[0]);

            // Traffic timer
            var timerTraffic = new Timer { Interval = Settings.Default.PollingRate_Network };
            timerTraffic.Elapsed += timerTraffic_Elapsed;
            timerTraffic.Start();

            // External IP timer
            var timerExternalIp = new Timer { Interval = Settings.Default.PollingRate_ExternalIp };
            timerExternalIp.Elapsed += timerExternalIp_Elapsed;
            timerExternalIp.Start();

            // Internal IP timer
            var timerInternalIp = new Timer { Interval = Settings.Default.PollingRate_InternalIp };
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
            double sentValue;
            double.TryParse(_pcSent.NextValue().ToString(CultureInfo.InvariantCulture), out sentValue);
            Sent = sentValue;

            double recievedValue;
            double.TryParse(_pcRecieved.NextValue().ToString(CultureInfo.InvariantCulture), out recievedValue);
            Recieved = recievedValue;
        }

        private async void timerExternalIp_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    ExternalIpAddress = await webClient.DownloadStringTaskAsync("http://icanhazip.com/");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                ExternalIpAddress = !string.IsNullOrWhiteSpace(ExternalIpAddress) ? ExternalIpAddress.Trim() : "N/A";
            }
        }

        private void timerInternalIp_Elapsed(object sender, ElapsedEventArgs e)
        {
            InternalIpAddress = NetworkExtensions.GetLocalIpAddress().ToString();
        }
    }
}