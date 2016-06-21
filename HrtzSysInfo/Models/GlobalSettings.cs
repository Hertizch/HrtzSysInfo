using System;
using System.Windows.Media;
using HrtzSysInfo.Extensions;

namespace HrtzSysInfo.Models
{
    public class GlobalSettings : ObservableObject
    {
        // Private fields
        private string _version;

        private int _pollingRateDateTime;
        private int _pollingRateWeek;
        private int _pollingRateCpu;
        private int _pollingRateRam;
        private int _pollingRateDrives;
        private int _pollingRateTemps;
        private int _pollingRateNetwork;
        private int _pollingRateIpInternal;
        private int _pollingRateIpExternal;

        private string _formattingDate;
        private string _formattingTime;
        private string _formattingTempUnit;

        private double _uiTop;
        private double _uiLeft;
        private double _uiWidth;
        private bool _uiShowInTaskbar;
        private bool _uiRunAtStartup;
        private double _uiSectionSeparator;

        private bool _visibilitySectionHeaders;
        private bool _visibilityNetwork;
        private bool _visibilitySystem;
        private bool _visibilityDrives;
        private bool _visibilityDateTime;

        private bool _visibilityNetworkExternalIp;
        private bool _visibilityNetworkInternalIp;
        private bool _visibilityNetworkUpload;
        private bool _visibilityNetworkDownload;

        private bool _visibilitySystemCpuUsage;
        private bool _visibilitySystemRamUsage;
        private bool _visibilitySystemCpuTemp;
        private bool _visibilitySystemGpuTemp;

        private long _networkSentMaxValue;
        private long _networkRecieveMaxValue;

        private int _systemTempCpuMaxValue;
        private int _systemTempGpuMaxValue;

        // Public properties
        public string Version
        {
            get { return _version; }
            set { SetField(ref _version, value); }
        }

        public int PollingRateDateTime
        {
            get { return _pollingRateDateTime; }
            set { SetField(ref _pollingRateDateTime, value); }
        }

        public int PollingRateWeek
        {
            get { return _pollingRateWeek; }
            set { SetField(ref _pollingRateWeek, value); }
        }

        public int PollingRateCpu
        {
            get { return _pollingRateCpu; }
            set { SetField(ref _pollingRateCpu, value); }
        }

        public int PollingRateRam
        {
            get { return _pollingRateRam; }
            set { SetField(ref _pollingRateRam, value); }
        }

        public int PollingRateDrives
        {
            get { return _pollingRateDrives; }
            set { SetField(ref _pollingRateDrives, value); }
        }

        public int PollingRateTemps
        {
            get { return _pollingRateTemps; }
            set { SetField(ref _pollingRateTemps, value); }
        }

        public int PollingRateNetwork
        {
            get { return _pollingRateNetwork; }
            set { SetField(ref _pollingRateNetwork, value); }
        }

        public int PollingRateIpInternal
        {
            get { return _pollingRateIpInternal; }
            set { SetField(ref _pollingRateIpInternal, value); }
        }

        public int PollingRateIpExternal
        {
            get { return _pollingRateIpExternal; }
            set { SetField(ref _pollingRateIpExternal, value); }
        }

        public string FormattingDate
        {
            get { return _formattingDate; }
            set { SetField(ref _formattingDate, value); }
        }

        public string FormattingTime
        {
            get { return _formattingTime; }
            set { SetField(ref _formattingTime, value); }
        }

        public string FormattingTempUnit
        {
            get { return _formattingTempUnit; }
            set { SetField(ref _formattingTempUnit, value); }
        }

        public double UiTop
        {
            get { return _uiTop; }
            set { SetField(ref _uiTop, value); }
        }

        public double UiLeft
        {
            get { return _uiLeft; }
            set { SetField(ref _uiLeft, value); }
        }

        public double UiWidth
        {
            get { return _uiWidth; }
            set { SetField(ref _uiWidth, value); }
        }

        public bool UiShowInTaskbar
        {
            get { return _uiShowInTaskbar; }
            set { SetField(ref _uiShowInTaskbar, value); }
        }

        public bool UiRunAtStartup
        {
            get { return _uiRunAtStartup; }
            set { SetField(ref _uiRunAtStartup, value); }
        }

        public double UiSectionSeparator
        {
            get { return _uiSectionSeparator; }
            set { SetField(ref _uiSectionSeparator, value); }
        }

        public bool VisibilitySectionHeaders
        {
            get { return _visibilitySectionHeaders; }
            set { SetField(ref _visibilitySectionHeaders, value); }
        }

        public bool VisibilityNetwork
        {
            get { return _visibilityNetwork; }
            set { SetField(ref _visibilityNetwork, value); }
        }

        public bool VisibilitySystem
        {
            get { return _visibilitySystem; }
            set { SetField(ref _visibilitySystem, value); }
        }

        public bool VisibilityDrives
        {
            get { return _visibilityDrives; }
            set { SetField(ref _visibilityDrives, value); }
        }

        public bool VisibilityDateTime
        {
            get { return _visibilityDateTime; }
            set { SetField(ref _visibilityDateTime, value); }
        }

        public bool VisibilityNetworkExternalIp
        {
            get { return _visibilityNetworkExternalIp; }
            set { SetField(ref _visibilityNetworkExternalIp, value); }
        }

        public bool VisibilityNetworkInternalIp
        {
            get { return _visibilityNetworkInternalIp; }
            set { SetField(ref _visibilityNetworkInternalIp, value); }
        }

        public bool VisibilityNetworkUpload
        {
            get { return _visibilityNetworkUpload; }
            set { SetField(ref _visibilityNetworkUpload, value); }
        }

        public bool VisibilityNetworkDownload
        {
            get { return _visibilityNetworkDownload; }
            set { SetField(ref _visibilityNetworkDownload, value); }
        }

        public bool VisibilitySystemCpuUsage
        {
            get { return _visibilitySystemCpuUsage; }
            set { SetField(ref _visibilitySystemCpuUsage, value); }
        }

        public bool VisibilitySystemRamUsage
        {
            get { return _visibilitySystemRamUsage; }
            set { SetField(ref _visibilitySystemRamUsage, value); }
        }

        public bool VisibilitySystemCpuTemp
        {
            get { return _visibilitySystemCpuTemp; }
            set { SetField(ref _visibilitySystemCpuTemp, value); }
        }

        public bool VisibilitySystemGpuTemp
        {
            get { return _visibilitySystemGpuTemp; }
            set { SetField(ref _visibilitySystemGpuTemp, value); }
        }

        public long NetworkSentMaxValue
        {
            get { return _networkSentMaxValue; }
            set { SetField(ref _networkSentMaxValue, value); }
        }

        public long NetworkRecieveMaxValue
        {
            get { return _networkRecieveMaxValue; }
            set { SetField(ref _networkRecieveMaxValue, value); }
        }

        public int SystemTempCpuMaxValue
        {
            get { return _systemTempCpuMaxValue; }
            set { SetField(ref _systemTempCpuMaxValue, value); }
        }

        public int SystemTempGpuMaxValue
        {
            get { return _systemTempGpuMaxValue; }
            set { SetField(ref _systemTempGpuMaxValue, value); }
        }
    }
}
