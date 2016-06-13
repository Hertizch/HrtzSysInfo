using HrtzSysInfo.Extensions;

namespace HrtzSysInfo.Models
{
    public class GlobalSettings : ObservableObject
    {
        // Private fields
        private int _pollingRateDateTime = 1000;
        private int _pollingRateCpu = 1000;
        private int _pollingRateRam = 1000;
        private int _pollingRateDrives = 30000;
        private int _pollingRateTemps = 1000;
        private int _pollingRateNetwork = 1000;
        private string _formattingDate = "dddd dd.MM.yyyy";
        private string _formattingTime = "HH:mm";
        private double _uiTop;
        private double _uiLeft;
        private double _uiWidth = 200;
        private double _uiHeight = 360;
        private bool _visibilityNetwork = true;
        private bool _visibilitySystem = true;
        private bool _visibilityDrives = true;
        private bool _visibilityDateTime = true;
        private long _networkSentMaxValue = 4089446;
        private long _networkRecieveMaxValue = 13631488;
        private int _systemTempCpuMaxValue = 90;
        private int _systemTempGpuMaxValue = 80;

        // Public properties
        public int PollingRateDateTime
        {
            get { return _pollingRateDateTime; }
            set { SetField(ref _pollingRateDateTime, value); }
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

        public double UiHeight
        {
            get { return _uiHeight; }
            set { SetField(ref _uiHeight, value); }
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
