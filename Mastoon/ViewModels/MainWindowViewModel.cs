using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Mastonet;
using Mastonet.Entities;
using Mastoon.Models;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;

namespace Mastoon.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private HomeTimelineModel _homeTimelineModel;
        public ReadOnlyReactiveCollection<Status> HomeTimelineStatuses { get; private set; }

        public DelegateCommand CustomCommand { get; }

        public ReactiveProperty<int> SelectedStatusIndex { get; set; } = new ReactiveProperty<int>();
        public ReactiveProperty<Status> SelectedStatus { get; set; } = new ReactiveProperty<Status>();
        public ReactiveCommand SelectedStatusIncrementCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SelectedStatusDecrementCommand { get; } = new ReactiveCommand();

        public ReactiveCollection<BindableTextViewModel> Contents { get; set; } =
            new ReactiveCollection<BindableTextViewModel>();

        public ReactiveCollection<Status> Statuses { get; } = new ReactiveCollection<Status>();

        public ReactiveProperty<string> PostStatus { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> SelectedItemVisibility { get; set; } = new ReactiveProperty<int>();
        public ReactiveCommand PostStatusCommand { get; } = new ReactiveCommand();

        private MastodonClient _mastodonClient;

        public MainWindowViewModel()
        {
            this.CustomCommand = new DelegateCommand(this.OpenStatus);
            this.SelectedStatusIncrementCommand.Subscribe(this.SelectedStatusIndexIncrement);
            this.SelectedStatusDecrementCommand.Subscribe(this.SelectedStatusIndexDecrement);
            this.PostStatusCommand.Subscribe(this.PostStatusAsync);

            this.SelectedStatus.PropertyChanged += (sender, e) => this.ShowSelectedStatus();

            this.SetupMastodonClient();
        }

        private async void SetupMastodonClient()
        {
            var authClient = new AuthenticationClient("");
            var appRegistration = await authClient.CreateApp("Mastdoon", Scope.Read | Scope.Write | Scope.Follow);

            var auth = await authClient.ConnectWithPassword("",
                "");
            this._mastodonClient = new MastodonClient(appRegistration, auth);

            this._homeTimelineModel = new HomeTimelineModel(this._mastodonClient);
            this.HomeTimelineStatuses = this._homeTimelineModel.HomeTimelineStatuses.ToReadOnlyReactiveCollection();

            this.GetFirstPageTimelineAsync();
            this.StartGetPublicStreamingAsync();
        }

        public async void GetFirstPageTimelineAsync()
        {
            var result = await this._mastodonClient.GetPublicTimeline();
            result.Reverse().ForEach(r => this.Statuses.Add(r));
        }

        public async void StartGetPublicStreamingAsync()
        {
            var streaming = this._mastodonClient.GetPublicStreaming();
            streaming.OnUpdate += (sender, e) => this.Statuses.Insert(this.Statuses.Count, e.Status);
            await streaming.Start();
        }

        public async void PostStatusAsync()
        {
            if (string.IsNullOrWhiteSpace(this.PostStatus.Value)) return;


            await this._mastodonClient.PostStatus(
                this.PostStatus.Value,
                this.GetPostStatusVisibility()
            );

            this.PostStatus.Value = "";
        }

        public void OpenStatus()
        {
            if (this.SelectedStatus.Value == null) return;
            Process.Start(this.SelectedStatus.Value.Url);
        }

        private void SelectedStatusIndexIncrement()
        {
            if (this.Statuses.Count - 1 > this.SelectedStatusIndex.Value) this.SelectedStatusIndex.Value++;
        }

        private void SelectedStatusIndexDecrement()
        {
            if (0 < this.SelectedStatusIndex.Value) this.SelectedStatusIndex.Value--;
        }

        private void ShowSelectedStatus()
        {
            this.Contents.Clear();

            var html = this.SelectedStatus.Value.Content;
            html = html.Replace("<br />", Environment.NewLine);
            var regex = new Regex("<.*?>");
            var splitedHtml = regex.Split(html);

            var hoge = new ReactiveCollection<BindableTextViewModel>();
            splitedHtml.ForEach(s =>
            {
                this.Contents.Add(new BindableTextViewModel {Text = new ReactiveProperty<string>(s)});
            });
        }

        private Visibility GetPostStatusVisibility()
        {
            switch (this.SelectedItemVisibility.Value)
            {
                case 0: return Visibility.Public;
                case 1: return Visibility.Unlisted;
                case 2: return Visibility.Private;
                case 3: return Visibility.Direct;
                default: return Visibility.Public;
            }
        }
    }
}