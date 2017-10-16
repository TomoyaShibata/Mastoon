using System;
using System.Globalization;
using System.Windows.Data;
using Mastonet.Entities;

namespace Mastoon.Conveters
{
    public class StatusDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as Status;

            return status?.Reblog?.Account?.DisplayName ??
                   status?.Account.DisplayName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}