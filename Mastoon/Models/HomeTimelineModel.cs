﻿using System.Collections.ObjectModel;
using System.Linq;
using Mastonet;
using Mastonet.Entities;
using Mastoon.Entities;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Mvvm;

namespace Mastoon.Models
{
    public class HomeTimelineModel : BindableBase, ITimelineModel
    {
        private MastodonClient _mastodonClient;

        public ObservableCollection<StatusWithMeta> HomeTimelineStatuses = new ObservableCollection<StatusWithMeta>();

        public void SetupTimelineModel(MastodonClient mastodonClient)
        {
            this._mastodonClient = mastodonClient;

            this.GetFirstPageTimelineAsync();
            this.StartStreamingTimelineAsync();
        }

        public async void GetFirstPageTimelineAsync()
        {
            var result = await this._mastodonClient.GetHomeTimeline();
            result.Reverse().ForEach(r => this.HomeTimelineStatuses.Add(new StatusWithMeta(r)));
        }

        public async void StartStreamingTimelineAsync()
        {
            var streaming = this._mastodonClient.GetUserStreaming();
            streaming.OnUpdate += (sender, e) =>
                this.HomeTimelineStatuses.Insert(this.HomeTimelineStatuses.Count, new StatusWithMeta(e.Status));
            await streaming.Start();
        }

        public async void GetPrevPageTimelineAsync()
        {
            var result = await this._mastodonClient.GetHomeTimeline(maxId: this.HomeTimelineStatuses[0].Id);
            result.ForEach(r => this.HomeTimelineStatuses.Insert(0, new StatusWithMeta(r)));
        }

        public void UpdateStatus(Status status)
        {
            var statusId = status.Reblog.Id;

            var indexOf = this.HomeTimelineStatuses.IndexOf(this.HomeTimelineStatuses.Single(s => s.Id == statusId));
            //this.HomeTimelineStatuses[indexOf] = status;
        }
    }
}