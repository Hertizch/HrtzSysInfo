using System;
using System.Globalization;
using System.Windows.Data;
using HrtzSysInfo.Properties;

namespace HrtzSysInfo.Converters
{
    public class DateTimeToStringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return DateTime.Parse(value.ToString()).ToString(Settings.Default.Formatting_Time);

            return DateTime.Parse(value.ToString()).ToString(parameter.Equals("date") ? Settings.Default.Formatting_Date : Settings.Default.Formatting_Time);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
