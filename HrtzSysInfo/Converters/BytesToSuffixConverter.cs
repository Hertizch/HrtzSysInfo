using System;
using System.Globalization;
using System.Windows.Data;

namespace HrtzSysInfo.Converters
{
    class BytesToSuffixConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return SizeSuffix((long) value);

            if (!parameter.Equals("double"))
                return SizeSuffix((long) value);

            value = Math.Round((double)value);
            long parsedValue;
            long.TryParse(value.ToString(), out parsedValue);

            return SizeSuffix(parsedValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(long value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0 bytes"; }

            var mag = (int)Math.Log(value, 1024);
            var adjustedSize = (decimal)value / (1L << (mag * 10));

            return $"{adjustedSize:n1} {SizeSuffixes[mag]}";
        }
    }
}
