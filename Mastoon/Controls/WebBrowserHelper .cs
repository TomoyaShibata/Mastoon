using System.Windows;
using WebBrowser = System.Windows.Controls.WebBrowser;

namespace Mastoon.Controls
{
    public class WebBrowserHelper
    {
        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.RegisterAttached("Body", typeof(string), typeof(WebBrowserHelper),
                new PropertyMetadata(OnBodyChanged));

        public static string GetBody(DependencyObject dependencyObject)
        {
            return (string) dependencyObject.GetValue(BodyProperty);
        }

        public static void SetBody(DependencyObject dependencyObject, string body)
        {
            var b = $"<head><meta charset=\"UTF-8\"></head><body>{body}</body>";
            dependencyObject.SetValue(BodyProperty, b);
        }

        private static void OnBodyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = (WebBrowser) d;
            var b = $"<head><meta charset=\"UTF-8\"></head><body style=\"font-family: Meiryo; font-size: 14px; margin=0\" oncontextmenu=\"return false;\" >{e.NewValue}</body>";
            webBrowser.NavigateToString((string) b);

        }
    }
}