using System;
using System.Globalization;
using System.Windows.Data;

namespace Mastoon.Conveters
{
    public class StatusCreatedAtConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var createdAt = (DateTime) value;
            return TimeZoneInfo.ConvertTimeFromUtc(createdAt, TimeZoneInfo.Local).ToString("yyyy/MM/dd HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}