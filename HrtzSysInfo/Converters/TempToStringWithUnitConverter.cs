using System;
using System.Globalization;
using System.Windows.Data;
using HrtzSysInfo.Tools;

namespace HrtzSysInfo.Converters
{
    public class TempToStringWithUnitConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return "N/A";

            if (values.Length <= 0)
                return "N/A";

            var temp = (double) values[0];
            string unit;

            if (values[1].Equals("f"))
            {
                unit = "°F";
                temp = ConvertTemp.ConvertCelsiusToFahrenheit(temp);
            }
            else
                unit = "°C";

            return temp + " " + unit;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
