using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace HrtzSysInfo.Counters
{
    public static class CpuCounter
    {
        public static double GetSystemCpuUsageValue()
        {
            double cpuUsageValue;
            string cpuUsageValueString;

            using (var pc = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            })
            {
                pc.NextValue();
                Thread.Sleep(500);
                cpuUsageValueString = pc.NextValue().ToString(CultureInfo.InvariantCulture);
            }

            double.TryParse(cpuUsageValueString, out cpuUsageValue);
            return cpuUsageValue;
        }
    }
}
