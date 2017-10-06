using System.Windows;
using System.Windows.Navigation;
using mshtml;

namespace Mastoon.Views
{
    /// <summary>
    /// ViewModel.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.webbrowser.LoadCompleted += webBrowser_LoadCompleted;
        }

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var document = (HTMLDocument) webbrowser.Document;
            var iEvent = (HTMLDocumentEvents2_Event) document;
            iEvent.onclick += ClickEventHandler;
        }

        private bool ClickEventHandler(IHTMLEventObj e)
        {
            var srcElement = e.srcElement;
            var tagClassName = srcElement.className;

            if (tagClassName == "ellipsis")
            {
                // TODO:通常リンクをクリックしたときの処理を実装する
            }

            return false;
        }
    }
}