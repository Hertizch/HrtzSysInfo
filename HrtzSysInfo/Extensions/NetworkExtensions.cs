using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace HrtzSysInfo.Extensions
{
    public static class NetworkExtensions
    {
        public static string GetLocalIpAddress()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return null;

            var host = Dns.GetHostEntry(Dns.GetHostName());

            var ipAddress = host
                .AddressList
                .FirstOrDefault(x => x.AddressFamily.Equals(AddressFamily.InterNetwork));

            return ipAddress?.ToString();
        }

        public static async void CalculateMaxTranserSpeed()
        {
            /*
            FileStream fs = new FileStream("speedtest20mb.txt", FileMode.CreateNew);
            fs.Seek(1024 * 1024 * 20, SeekOrigin.Begin);
            fs.WriteByte(0);
            fs.Close();

            return;
            */
            
            WebClient wc = new WebClient();

            Uri URL = new Uri("http://www.hertizch.com/speedtest20mb.txt");

            // get current tickcount
            double starttime = Environment.TickCount;

            await wc.DownloadFileTaskAsync(URL, "speedtest.txt");

            double endtime = Environment.TickCount;

            // how many seconds did it take?
            // we are calculating this by subtracting starttime from endtime
            // and dividing by 1000 (since the tickcount is in milliseconds.. 1000 ms = 1 
            // sec)
            double secs = Math.Floor(endtime - starttime) / 1000;

            // we also need a variable that shows the number of seconds
            // without the decimal point. Therefore, we are rounding the value
            // of 'secs'. 0 means that zero decimal numbers should be shown.
            double secs2 = Math.Round(secs, 0);

            // calculate download rate in kb per sec.
            // this is done by dividing 1024 by the number of seconds it
            // took to download the file (1024 bytes = 1 kilobyte)
            double kbsec = Math.Round(1024 / secs);

            Debug.WriteLine("\nCompleted. Statistics:\n");

            Debug.WriteLine("1mb download: \t{0} secs ({1} secs)", secs2, secs);
            Debug.WriteLine("Download rate: \t{0} kb/sec", kbsec);

            Debug.WriteLine("Cleaning up... (deleting downloaded file)");

            try
            {
                // delete downloaded file
                System.IO.File.Delete("speedtest.txt");
                Debug.WriteLine("Done.");
            }
            catch
            {
                Debug.WriteLine("Couldn't delete download file.");
                Debug.WriteLine("To delete the file yourself, go to your C-drive and " + "look for the file 'speedtest.txt'.");
            }
        }
    }
}
