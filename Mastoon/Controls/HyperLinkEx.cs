using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

// Original Source Code
// https://github.com/Grabacr07/MetroTrilithon/blob/bd31e38ce976fdc7a1689057f3b775fa9cec0f51/source/MetroTrilithon.Desktop/UI/Controls/HyperlinkEx.cs
namespace Mastoon.Controls
{
    public class HyperlinkEx : Hyperlink
    {
        public Uri Uri
        {
            get => (Uri) this.GetValue(UriProperty);
            set => this.SetValue(UriProperty, value);
        }

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register(nameof(Uri), typeof(Uri), typeof(HyperlinkEx), new UIPropertyMetadata(null));

        protected override void OnClick()
        {
            base.OnClick();

            if (this.Uri == null) return;
            try
            {
                Process.Start(this.Uri.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}