using System.Collections.ObjectModel;
using Mastonet;
using Prism.Mvvm;

namespace Mastoon.Models
{
    public class StatusPostModel : BindableBase
    {
        private MastodonClient _mastodonClient;

        private string _content = "";

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ObservableCollection<string> VisibilityTexts = new ObservableCollection<string>
        {
            "公開",
            "未収録",
            "非公開",
            "ダイレクト"
        };

        private int _selectedVisibilityIndex = 0;

        public int SelectedVisibilityIndex
        {
            get => _selectedVisibilityIndex;
            set => SetProperty(ref _selectedVisibilityIndex, value);
        }

        public void SetupStatusPostModel(MastodonClient mastodonClient)
        {
            this._mastodonClient = mastodonClient;
        }

        public async void PostStatusAsync()
        {
            if (string.IsNullOrWhiteSpace(this._content)) return;

            await this._mastodonClient.PostStatus(
                this._content,
                this.GetStatusVisibility()
            );

            this.ClearStatusContent();
        }

        private Visibility GetStatusVisibility()
        {
            switch (this._selectedVisibilityIndex)
            {
                case 0: return Visibility.Public;
                case 1: return Visibility.Unlisted;
                case 2: return Visibility.Private;
                case 3: return Visibility.Direct;
                default: return Visibility.Public;
            }
        }

        private void ClearStatusContent() => this.Content = "";
    }
}