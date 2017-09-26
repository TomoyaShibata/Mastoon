using System;
using System.Globalization;
using System.Windows.Data;

namespace Mastoon.Conveters
{
    public class DataGridContentConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml((string) value);
            return doc.DocumentNode.InnerText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}