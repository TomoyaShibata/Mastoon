using System;
using System.Globalization;
using System.Windows.Data;
using Mastonet.Entities;

namespace Mastoon.Conveters
{
    public class StatusForgroundConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as Status;

            if (status?.Reblog != null)
            {
                return "#2aa198";
            }

            return "#fff";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}