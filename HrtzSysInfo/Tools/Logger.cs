﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HrtzSysInfo.Tools
{
    public static class Logger
    {
        private static readonly string LogFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hrtzsysinfo.log");

        public static void Write(string value, bool writeToFile = false, Exception exception = null)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(LogFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    fileStream = null;

                    if (exception != null)
                    {
                        Debug.WriteLine($"[{DateTime.Now}]: {value} [Exception:\n{exception}]");

                        if (writeToFile)
                            streamWriter.WriteLine($"[{DateTime.Now}]: {value} [Exception:\n{exception}]");
                    }
                    else
                    {
                        Debug.WriteLine($"[{DateTime.Now}]: {value}");

                        if (writeToFile)
                            streamWriter.WriteLine($"[{DateTime.Now}]: {value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Logger error: {ex}");
            }
            finally
            {
                fileStream?.Dispose();
            }
        }
    }
}
