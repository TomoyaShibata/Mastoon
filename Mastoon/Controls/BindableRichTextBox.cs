using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Mastoon.Controls
{
    public class BindableRichTextBox : RichTextBox
    {
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(
            "Document", typeof(FlowDocument), typeof(BindableRichTextBox),
            new FrameworkPropertyMetadata(null, OnDocumentChanged));

        public new FlowDocument Document
        {
            get => (FlowDocument) this.GetValue(DocumentProperty);
            set => this.SetValue(DocumentProperty, value);
        }

        public static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var richTextBox = (RichTextBox) obj;
            richTextBox.Document = (FlowDocument) args.NewValue;
        }
    }
}