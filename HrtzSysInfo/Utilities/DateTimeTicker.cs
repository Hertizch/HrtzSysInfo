﻿using System;
using System.Diagnostics;
using System.Timers;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Properties;

namespace HrtzSysInfo.Utilities
{
    public class DateTimeTicker : ObservableObject
    {
        public DateTimeTicker()
        {
            Debug.WriteLine("Created Utility Class: DateTimeTicker");

            if (Settings.Default.SectionVisibility_DateTime)
                Initialize();
        }

        private void Initialize()
        {
            var timerNow = new Timer { Interval = Settings.Default.PollingRate_DateTime };
            timerNow.Elapsed += timerNow_Elapsed;
            timerNow.Start();

            var timerWeek = new Timer { Interval = Settings.Default.PollingRate_Week };
            timerWeek.Elapsed += timerWeek_Elapsed;
            timerWeek.Start();

            // Get the values at init
            EventExtensions.FireEvent(timerNow, "onIntervalElapsed", this, null);
            EventExtensions.FireEvent(timerWeek, "onIntervalElapsed", this, null);
        }

        // Private fields
        private DateTime _now;
        private int _week;

        // Public properties
        public DateTime Now
        {
            get { return _now; }
            set { SetField(ref _now, value); }
        }

        public int Week
        {
            get { return _week; }
            set { SetField(ref _week, value); }
        }

        private void timerNow_Elapsed(object sender, ElapsedEventArgs e)
        {
            Now = DateTime.Now;
        }

        private void timerWeek_Elapsed(object sender, ElapsedEventArgs e)
        {
            Week = DateTimeExtensions.GetIso8601WeekOfYear(DateTime.Now);
        }
    }
}
