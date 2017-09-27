using System.Collections.ObjectModel;
using System.Linq;
using Mastonet;
using Mastonet.Entities;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Mvvm;

namespace Mastoon.Models
{
    public class FavoriteTimelineModel : BindableBase, ITimelineModel
    {
        private MastodonClient _mastodonClient;

        public ObservableCollection<Status> FavoriteTimelineStatuses = new ObservableCollection<Status>();

        public void SetupTimelineModel(MastodonClient mastodonClient)
        {
            this._mastodonClient = mastodonClient;

            this.GetFirstPageTimelineAsync();
        }

        public async void GetFirstPageTimelineAsync()
        {
            var result = await this._mastodonClient.GetFavourites();
            result.Reverse().ForEach(r => this.FavoriteTimelineStatuses.Add(r));
        }
    }
}