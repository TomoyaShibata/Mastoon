using System.Windows;

namespace Mastoon.Controls
{
    /// <summary>
    /// TimelineControl.xaml の相互作用ロジック
    /// </summary>
    public partial class TimelineControl
    {
        public object ItemsSource
        {
            get => (object) this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(TimelineControl),
                new PropertyMetadata(null));

        public object SelectedItem
        {
            get => (object) this.GetValue(SelectedItemProperty);
            set => this.SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(TimelineControl),
                new PropertyMetadata());

        public TimelineControl()
        {
            InitializeComponent();
        }
    }
}