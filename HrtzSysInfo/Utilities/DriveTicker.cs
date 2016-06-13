using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Properties;

namespace HrtzSysInfo.Utilities
{
    public class DriveTicker : ObservableObject
    {
        public DriveTicker()
        {
            Debug.WriteLine("Created Utility Class: DriveTicker");

            if (Settings.Default.SectionVisibility_Drives)
                Initialize();
        }

        private void Initialize()
        {
            var timerDriveInfo = new Timer { Interval = Settings.Default.PollingRate_Drives };
            timerDriveInfo.Elapsed += timerDriveInfo_Elapsed;
            timerDriveInfo.Start();

            // Get the value at init
            EventExtensions.FireEvent(timerDriveInfo, "onIntervalElapsed", this, null);
        }

        // Private fields
        private IEnumerable<DriveInfo> _driveInfos; 

        // Public properties
        public IEnumerable<DriveInfo> DriveInfos
        {
            get { return _driveInfos; }
            set { SetField(ref _driveInfos, value); }
        }

        // Methods
        private void timerDriveInfo_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                DriveInfos = DriveInfo.GetDrives();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
