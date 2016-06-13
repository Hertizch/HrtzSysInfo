﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Properties;
using Microsoft.VisualBasic.Devices;
using OpenHardwareMonitor.Hardware;

namespace HrtzSysInfo.Utilities
{
    public class SystemTicker : ObservableObject
    {
        public SystemTicker()
        {
            Debug.WriteLine("Created Utility Class: SystemTicker");

            if (Settings.Default.SectionVisibility_System)
                Initialize();
        }

        private void Initialize()
        {
            // Cpu Pc
            _pcCpu = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            // Ram Pc
            _pcRam = new PerformanceCounter("Memory", "Available MBytes");

            // Temp
            _computer = new OpenHardwareMonitor.Hardware.Computer
            {
                CPUEnabled = true,
                GPUEnabled = true
            };

            _computer.Open();

            // Cpu timer
            var timerCpu = new Timer { Interval = Settings.Default.PollingRate_Cpu };
            timerCpu.Elapsed += timerCpu_Elapsed;
            timerCpu.Start();

            // Ram timer
            var timerRam = new Timer { Interval = Settings.Default.PollingRate_Ram };
            timerRam.Elapsed += timerRam_Elapsed;
            timerRam.Start();

            // Temp timer
            var timerTemp = new Timer { Interval = Settings.Default.PollingRate_Temp };
            timerTemp.Elapsed += timerTemp_Elapsed;
            timerTemp.Start();

            // Get the value at init
            EventExtensions.FireEvent(timerCpu, "onIntervalElapsed", this, null);
            EventExtensions.FireEvent(timerRam, "onIntervalElapsed", this, null);
            EventExtensions.FireEvent(timerTemp, "onIntervalElapsed", this, null);
        }

        // Private fields
        private PerformanceCounter _pcCpu;
        private PerformanceCounter _pcRam;
        private OpenHardwareMonitor.Hardware.Computer _computer;
        private double _cpuUsage;
        private double _cpuClock;
        private double _ramUsage;
        private int _cpuTemp;
        private int _gpuTemp;

        // Public properties
        public double CpuUsage
        {
            get { return _cpuUsage; }
            set { SetField(ref _cpuUsage, value); }
        }

        public double CpuClock
        {
            get { return _cpuClock; }
            set { SetField(ref _cpuClock, value); }
        }

        public double RamUsage
        {
            get { return _ramUsage; }
            set { SetField(ref _ramUsage, value); }
        }

        public int CpuTemp
        {
            get { return _cpuTemp; }
            set { SetField(ref _cpuTemp, value); }
        }

        public int GpuTemp
        {
            get { return _gpuTemp; }
            set { SetField(ref _gpuTemp, value); }
        }

        // Methods
        private void timerCpu_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                CpuUsage = _pcCpu.NextValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void timerRam_Elapsed(object sender, ElapsedEventArgs e)
        {
            var max = (new ComputerInfo().TotalPhysicalMemory / 1024) / 1024;

            float available = 0;

            try
            {
                available = _pcRam.NextValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            RamUsage = (max - available)/max*100;
        }

        private void timerTemp_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var hardware in _computer.Hardware)
            {
                hardware.Update();
                hardware.GetReport();

                // CPU
                if (hardware.HardwareType.Equals(HardwareType.CPU))
                {
                    foreach (
                        var sensor in
                            hardware.Sensors.Where(
                                x => x.SensorType.Equals(SensorType.Temperature) && x.Name.Contains("Package")))
                    {
                        int cpuTemp;
                        int.TryParse(sensor.Value.ToString(), out cpuTemp);
                        CpuTemp = cpuTemp;
                    }
                }

                // GPU
                if (hardware.HardwareType.Equals(HardwareType.GpuNvidia) | hardware.HardwareType.Equals(HardwareType.GpuAti))
                {
                    foreach (
                        var sensor in
                            hardware.Sensors.Where(
                                x =>
                                    x.SensorType.Equals(SensorType.Temperature) &&
                                    !x.Identifier.ToString().Contains("1")))
                    {
                        int gpuTemp;
                        int.TryParse(sensor.Value.ToString(), out gpuTemp);
                        GpuTemp = gpuTemp;
                    }
                }
            }
        }
    }
}