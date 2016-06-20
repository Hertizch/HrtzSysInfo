using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Models;
using HrtzSysInfo.Tools;

namespace HrtzSysInfo.Utilities
{
    public class DriveTicker : ObservableObject
    {
        public DriveTicker()
        {
            Debug.WriteLine("Created Utility Class: DriveTicker");

            Initialize();
        }

        private void Initialize()
        {
            Drives = new MtObservableCollection<Drive>();

            var timerDriveInfo = new Timer { Interval = UserSettings.GlobalSettings.PollingRateDrives };
            timerDriveInfo.Elapsed += timerDriveInfo_Elapsed;
            timerDriveInfo.Start();

            // Get the value at init
            EventExtensions.FireEvent(timerDriveInfo, "onIntervalElapsed", this, null);
        }

        // Private fields
        private MtObservableCollection<Drive> _drives; 

        // Public properties
        public MtObservableCollection<Drive> Drives
        {
            get { return _drives; }
            set { SetField(ref _drives, value); }
        }

        // Methods
        private void timerDriveInfo_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!UserSettings.GlobalSettings.VisibilityDrives) return;

            IEnumerable<DriveInfo> driveInfos = null;

            try
            {
                driveInfos = DriveInfo.GetDrives().Where(x => x.IsReady);
            }
            catch (Exception ex)
            {
                Logger.Write("DriveTicker() - DriveInfo.GetDrives() - Exception raised", true, ex);
            }

            if (driveInfos == null) return;

            foreach (var driveInfo in driveInfos)
            {
                // If new
                if (!Drives.Any(x => x.Name.Equals(driveInfo.Name)))
                    Drives.Add(new Drive
                    {
                        Name = driveInfo.Name,
                        TotalSize = driveInfo.TotalSize,
                        AvailableFreeSpace = driveInfo.AvailableFreeSpace
                    });

                // If existing
                foreach (var drive in Drives.Where(x => x.Name.Equals(driveInfo.Name)))
                {
                    drive.TotalSize = driveInfo.TotalSize;
                    drive.AvailableFreeSpace = driveInfo.AvailableFreeSpace;
                }
            }
        }
    }
}
