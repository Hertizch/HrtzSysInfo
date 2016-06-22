using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace HrtzSysInfo.Counters
{
    public static class RamCounter
    {
        public static double GetRamUsageValue()
        {
            double ramUsageValue;

            using (var pc = new PerformanceCounter("Memory", "Available MBytes"))
            {
                pc.NextValue();
                Thread.Sleep(500);
                var ramUsageValueString = pc.NextValue().ToString(CultureInfo.InvariantCulture);
                double.TryParse(ramUsageValueString, out ramUsageValue);
            }

            return ramUsageValue;
        }
    }
}
