using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
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
            _computer = new Computer
            {
                CPUEnabled = true,
                GPUEnabled = true
            };

            _computer.Open();

            Gpus = new MtObservableCollection<Gpu>();

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
        private PerformanceCounter _pcCpu;
        private PerformanceCounter _pcRam;
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
                CpuUsage = _pcCpu.NextValue();
            }
            catch (Exception ex)
            {
                Logger.Write("SystemTicker() - _pcCpu.NextValue() - Exception raised", true, ex);
            }
        }

        private void timerRam_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!UserSettings.GlobalSettings.VisibilitySystemRamUsage) return;

            var max = (new ComputerInfo().TotalPhysicalMemory / 1024) / 1024;

            float available = 0;

            try
            {
                available = _pcRam.NextValue();
            }
            catch (Exception ex)
            {
                Logger.Write("SystemTicker() - _pcRam.NextValue() - Exception raised", true, ex);
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
                if (UserSettings.GlobalSettings.VisibilitySystemCpuTemp)
                {
                    if (hardware.HardwareType.Equals(HardwareType.CPU))
                    {
                        foreach (var sensor in hardware.Sensors.Where(x => x.SensorType.Equals(SensorType.Temperature) && x.Name.Contains("Package")))
                        {
                            double cpuTemp;
                            double.TryParse(sensor.Value.ToString(), out cpuTemp);
                            CpuTemp = cpuTemp;
                        }
                    }
                }

                // GPU
                if (UserSettings.GlobalSettings.VisibilitySystemGpuTemp)
                {
                    if ((hardware.HardwareType.Equals(HardwareType.GpuNvidia) | hardware.HardwareType.Equals(HardwareType.GpuAti)))
                    {
                        var hardwareId = hardware.Identifier.ToString();
                        int id;
                        int.TryParse(hardwareId.Substring(hardwareId.Length - 1), out id);

                        var temp = 0;
                        var sensor = hardware.Sensors.FirstOrDefault(x => x.SensorType.Equals(SensorType.Temperature));

                        if (sensor != null)
                            int.TryParse(sensor.Value.ToString(), out temp);

                        // If new, add
                        if (!Gpus.Any(x => x.Identifier.Equals(id)))
                            Gpus.Add(new Gpu
                            {
                                Identifier = id,
                                Temperature = temp
                            });

                        foreach (var gpu in Gpus.Where(x => x.Identifier.Equals(id)))
                            gpu.Temperature = temp;
                    }
                }
            }
        }
    }
}
