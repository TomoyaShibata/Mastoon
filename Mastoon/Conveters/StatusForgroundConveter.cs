using System;
using System.Globalization;
using System.Windows.Data;

namespace Mastoon.Conveters
{
    public class StatusForgroundConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var reblogged = (bool) (value ?? false);
            return reblogged ? "#2aa198" : "#fff";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}