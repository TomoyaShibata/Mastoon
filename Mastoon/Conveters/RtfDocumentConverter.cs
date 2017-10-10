using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using Mastoon.Entities;

namespace Mastoon.Conveters
{
    public class RtfDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flowDocument = new FlowDocument();
            flowDocument.Blocks.Add(this.GenerateParagraph(value as List<ContentPart>));
            return flowDocument;
        }

        private Paragraph GenerateParagraph(List<ContentPart> contentParts)
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
                        hyperlink.Click += Hyperlink_Click;
                        paragraph.Inlines.Add(hyperlink);
                        break;
                }
            });
            return paragraph;
        }

        private static void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = (Hyperlink) sender;
            Process.Start(hyperlink.NavigateUri.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}