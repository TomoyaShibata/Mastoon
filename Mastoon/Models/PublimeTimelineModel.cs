using System.Collections.ObjectModel;
using System.Linq;
using Mastonet;
using Mastonet.Entities;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Mvvm;

namespace Mastoon.Models
{
    public class PublimeTimelineModel : BindableBase, ITimelineModel
    {
        private MastodonClient _mastodonClient;

        public ObservableCollection<Status> PublicTimelineStatuses = new ObservableCollection<Status>();

        public void SetupTimelineModel(MastodonClient mastodonClient)
        {
            this._mastodonClient = mastodonClient;

            this.GetFirstPageTimelineAsync();
            this.GetFirstPageTimelineAsync();
        }

        public async void GetFirstPageTimelineAsync()
        {
            var result = await this._mastodonClient.GetPublicTimeline();
            result.Reverse().ForEach(r => this.PublicTimelineStatuses.Add(r));
        }

        public async void StartStreamingTimelineAsync()
        {
            var streaming = this._mastodonClient.GetPublicStreaming();
            streaming.OnUpdate += (sender, e) =>
                this.PublicTimelineStatuses.Insert(this.PublicTimelineStatuses.Count, e.Status);
            await streaming.Start();
        }
    }
}