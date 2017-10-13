using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using Mastoon.Entities;
using Microsoft.Practices.ObjectBuilder2;

namespace Mastoon.Conveters
{
    public class RtfDocumentConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var contentParts = values[0] as IReadOnlyCollection<ContentPart>;
            var paragraph = GenerateParagraph(contentParts);

            var flowDocument = new FlowDocument();
            flowDocument.Blocks.Add(paragraph);

            return flowDocument;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static Paragraph GenerateParagraph(IEnumerable<ContentPart> contentParts)
        {
            var paragraph = new Paragraph();
            contentParts?.ForEach(c =>
            {
                switch (c.Type)
                {
                    case "text":
                        paragraph.Inlines.Add(c.Text);
                        break;
                    case "url":
                        var hyperlink = new Hyperlink
                        {
                            Inlines = {new Run(c.Text)},
                            NavigateUri = new Uri(c.Url)
                        };
                        hyperlink.Click += OnClickHyperlink;
                        paragraph.Inlines.Add(hyperlink);
                        break;
                }
            });
            return paragraph;
        }

        private static void OnClickHyperlink(object sender, RoutedEventArgs e)
        {
            var hyperlink = (Hyperlink) sender;
            Process.Start(hyperlink.NavigateUri.ToString());
        }
    }
}