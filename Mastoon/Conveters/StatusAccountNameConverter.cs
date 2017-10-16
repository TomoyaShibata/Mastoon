using System;
using System.Globalization;
using System.Windows.Data;
using Mastonet.Entities;

namespace Mastoon.Conveters
{
    public class StatusAccountNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as Status;

            return status?.Reblog?.Account?.AccountName ??
                   status?.Account.AccountName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}