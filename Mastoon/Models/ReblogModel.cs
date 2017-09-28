using Mastonet;
using Mastonet.Entities;
using Prism.Mvvm;

namespace Mastoon.Models
{
    public class ReblogModel : BindableBase
    {
        private MastodonClient _mastodonClient;

        private Status _newStatus;

        public Status NewStatus
        {
            get => this._newStatus;
            set => this.SetProperty(ref this._newStatus, value);
        }

        private int _number;

        public int Number
        {
            get => this._number;
            set => this.SetProperty(ref this._number, value);
        }

        public void SetupMastodonModel(MastodonClient mastodonClient)
        {
            this._mastodonClient = mastodonClient;
        }

        public void ToggleReblog(int statusId, bool reblogged = false)
        {
            if (reblogged)
            {
                this.Unreblog(statusId);
            }
            else
            {
                this.Reblog(statusId);
            }
        }

        private async void Reblog(int statusId)
        {
            var result = await this._mastodonClient.Reblog(statusId);
            this.NewStatus = result;
        }

        private async void Unreblog(int statusId)
        {
            var result = await this._mastodonClient.Unreblog(statusId);
            this.NewStatus = result;
        }
    }
}