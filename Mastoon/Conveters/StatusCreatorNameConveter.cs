using System;
using System.Globalization;
using System.Windows.Data;
using Mastonet.Entities;

namespace Mastoon.Conveters
{
    public class StatusCreatorNameConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as Status;

            return status?.Reblog == null
                ? $"{status?.Account.AccountName}/{status?.Account.DisplayName}"
                : $"{status.Reblog.Account.AccountName}/{status.Reblog.Account.DisplayName} (RT : {status.Account.DisplayName})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}