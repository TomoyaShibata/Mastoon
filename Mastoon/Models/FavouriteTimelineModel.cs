using System.Collections.ObjectModel;
using System.Linq;
using Mastonet;
using Mastonet.Entities;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Mvvm;

namespace Mastoon.Models
{
    public class FavouriteTimelineModel : BindableBase, ITimelineModel
    {
        private MastodonClient _mastodonClient;

        public ObservableCollection<Status> FavouriteTimelineStatuses = new ObservableCollection<Status>();

        public void SetupTimelineModel(MastodonClient mastodonClient)
        {
            this._mastodonClient = mastodonClient;

            this.GetFirstPageTimelineAsync();
        }

        public async void GetFirstPageTimelineAsync()
        {
            var result = await this._mastodonClient.GetFavourites();
            result.Reverse().ForEach(r => this.FavouriteTimelineStatuses.Add(r));
        }

        public void ToggleFavourite(Status status)
        {
            var favourited = status.Favourited ?? false;
            if (favourited)
            {
                this.Unfavourite(status.Id);
            }
            else
            {
                this.Favourite(status.Id);
            }
        }

        private async void Favourite(int statusId)
        {
            var result = await this._mastodonClient.Favourite(statusId);
            this.FavouriteTimelineStatuses.Add(result);
        }

        private async void Unfavourite(int statusId)
        {
            var status = await this._mastodonClient.Unfavourite(statusId);
            this.FavouriteTimelineStatuses.Remove(status);
        }
    }
}