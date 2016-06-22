using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Timers;
using HrtzSysInfo.Counters;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Models;
using HrtzSysInfo.Tools;
using Microsoft.VisualBasic.Devices;
using OpenHardwareMonitor.Hardware;
using Computer = OpenHardwareMonitor.Hardware.Computer;

namespace HrtzSysInfo.Utilities
{
    public class SystemTicker : ObservableObject
    {
        public SystemTicker()
        {
            Debug.WriteLine("Created Utility Class: SystemTicker");

            Initialize();
        }

        private void Initialize()
        {
            Gpus = new MtObservableCollection<Gpu>();
            for (var i = 0; i < GetTotalNumberOfGpus(); i++)
            {
                var id = i;
                id++;
                Gpus.Add(new Gpu
                {
                    Identifier = id
                });
            }

            var gpuVisibility = new[] { UserSettings.GlobalSettings.VisibilitySystemGpuTemp, UserSettings.GlobalSettings.VisibilitySystemGpuLoad };

            _computer = new Computer
            {
                CPUEnabled = UserSettings.GlobalSettings.VisibilitySystemCpuTemp,
                GPUEnabled = BooleanExtensions.ExceedsThreshold(0, gpuVisibility)
            };

            _computer.Open();

            // Cpu timer
            var timerCpu = new Timer { Interval = UserSettings.GlobalSettings.PollingRateCpu };
            timerCpu.Elapsed += timerCpu_Elapsed;
            timerCpu.Start();

            // Ram timer
            var timerRam = new Timer { Interval = UserSettings.GlobalSettings.PollingRateRam };
            timerRam.Elapsed += timerRam_Elapsed;
            timerRam.Start();

            // Temp timer
            var timerTemp = new Timer { Interval = UserSettings.GlobalSettings.PollingRateTemps };
            timerTemp.Elapsed += timerTemp_Elapsed;
            timerTemp.Start();

            // Get the value at init
            EventExtensions.FireEvent(timerCpu, "onIntervalElapsed", this, null);
            EventExtensions.FireEvent(timerRam, "onIntervalElapsed", this, null);
            EventExtensions.FireEvent(timerTemp, "onIntervalElapsed", this, null);
        }

        // Private fields
        private Computer _computer;
        private double _cpuUsage;
        private double _cpuClock;
        private double _ramUsage;
        private double _cpuTemp;
        private MtObservableCollection<Gpu> _gpus;

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

        public double CpuTemp
        {
            get { return _cpuTemp; }
            set { SetField(ref _cpuTemp, value); }
        }

        public MtObservableCollection<Gpu> Gpus
        {
            get { return _gpus; }
            set { SetField(ref _gpus, value); }
        }

        // Methods
        private void timerCpu_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!UserSettings.GlobalSettings.VisibilitySystemCpuUsage) return;

            try
            {
                CpuUsage = CpuCounter.GetSystemCpuUsageValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void timerRam_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!UserSettings.GlobalSettings.VisibilitySystemRamUsage) return;

            try
            {
                var max = (new ComputerInfo().TotalPhysicalMemory/1024)/1024;
                var available = RamCounter.GetRamUsageValue();
                RamUsage = (max - available)/max*100;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void timerTemp_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_computer?.Hardware != null)
                foreach (var hardware in _computer.Hardware)
                {
                    hardware.Update();
                    hardware.GetReport();

                    // CPU
                    if (UserSettings.GlobalSettings.VisibilitySystemCpuTemp)
                    {
                        if (hardware.HardwareType.Equals(HardwareType.CPU))
                        {
                            foreach (
                                var sensor in
                                    hardware.Sensors.Where(
                                        x => x.SensorType.Equals(SensorType.Temperature) && x.Name.Contains("Package")))
                            {
                                double cpuTemp;
                                double.TryParse(sensor.Value.ToString(), out cpuTemp);

                                CpuTemp = cpuTemp;
                            }
                        }
                    }

                    // GPU
                    if ((hardware.HardwareType.Equals(HardwareType.GpuNvidia) ||
                         hardware.HardwareType.Equals(HardwareType.GpuAti)))
                    {
                        var hardwareIdRaw = hardware.Identifier.ToString();
                        var hardwareId = hardwareIdRaw.Remove(0, hardwareIdRaw.Length - 1);

                        int id;
                        int.TryParse(hardwareId, out id);
                        id++;

                        double temp = 0;
                        double load = 0;

                        if (UserSettings.GlobalSettings.VisibilitySystemGpuTemp)
                            temp = GetHardwareValue(hardware, SensorType.Temperature);

                        if (UserSettings.GlobalSettings.VisibilitySystemGpuLoad)
                            load = GetHardwareValue(hardware, SensorType.Load);

                        if (Gpus.Count <= 0) continue;

                        foreach (var gpu in Gpus.Where(x => x.Identifier == id))
                        {
                            gpu.Temperature = temp;
                            gpu.Load = load;
                        }
                    }
                }
        }

        private static int GetTotalNumberOfGpus()
        {
            using (var objSearcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
            {
                var count = objSearcher.Get().OfType<ManagementObject>().Count(x => x["VideoProcessor"] != null);
                return count;
            }
        }

        private static double GetHardwareValue(IHardware hardware, SensorType sensorType)
        {
            double value = 0;

            var sensor = hardware.Sensors?.First(x => x.SensorType.Equals(sensorType));
            if (sensor != null)
                double.TryParse(sensor.Value.ToString(), out value);

            return value;
        }
    }
}
