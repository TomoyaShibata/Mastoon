using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace Mastoon.Conveters
{
    public class RtfDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flowDocument = new FlowDocument();

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            {
                var range = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                sw.Write(value as string);
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                range.Load(ms, DataFormats.Rtf);
            }

            return flowDocument;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}