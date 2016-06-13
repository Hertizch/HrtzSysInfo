using System;
using System.Globalization;
using System.Windows.Data;

namespace HrtzSysInfo.Converters
{
    public class DiskUsedSpaceToPercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)((long)values[0] - (long)values[1]) / (long)values[0] * 100;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
