using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace HrtzSysInfo.Counters
{
    public static class NetworkCounter
    {
        public static double GetNetworkSentValue(string networkInterfaceName)
        {
            double sentValue;
            string sentValueString;

            using (var pc = new PerformanceCounter("Network Interface", "Bytes Sent/sec", networkInterfaceName))
            {
                pc.NextValue();
                Thread.Sleep(500);
                sentValueString = pc.NextValue().ToString(CultureInfo.InvariantCulture);
            }

            double.TryParse(sentValueString, out sentValue);
            return sentValue;
        }

        public static double GetNetworkRecievedValue(string networkInterfaceName)
        {
            double recievedValue;
            string recievedValueString;

            using (var pc = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkInterfaceName))
            {
                pc.NextValue();
                Thread.Sleep(500);
                recievedValueString = pc.NextValue().ToString(CultureInfo.InvariantCulture);
            }

            double.TryParse(recievedValueString, out recievedValue);
            return recievedValue;
        }
    }
}
