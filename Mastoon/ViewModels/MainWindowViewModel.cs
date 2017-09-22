using System;
using System.Linq;
using Mastonet;
using Mastonet.Entities;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Reactive.Bindings;

namespace Mastoon.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        public DelegateCommand CustomCommand { get; }
        public DelegateCommand PostStatusCommand { get; }
        public InteractionRequest<INotification> PostRequest { get; } = new InteractionRequest<INotification>();

        private string _input;

        public string Input
        {
            get => this._input;
            set => this.SetProperty(ref this._input, value);
        }

        public ReactiveProperty<Object> SelectedItem { get; set; } = new ReactiveProperty<Object>();
        public ReactiveCollection<Status> Statuses { get; } = new ReactiveCollection<Status>();
        public ReactiveProperty<string> PostStatus { get; set; } = new ReactiveProperty<string>();

        private MastodonClient _mastodonClient;

        public MainWindowViewModel()
        {
            this.CustomCommand = new DelegateCommand(this.OpenStatus);
            this.PostStatusCommand = new DelegateCommand(this.PostStatusAsync);

            this.SetupMastodonClient();
        }

        private async void SetupMastodonClient()
        {
            var authClient = new AuthenticationClient("");
            var appRegistration = await authClient.CreateApp("Mastdoon", Scope.Read | Scope.Write | Scope.Follow);

            var auth = await authClient.ConnectWithPassword("",
                "");
            this._mastodonClient = new MastodonClient(appRegistration, auth);

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
                Visibility.Public
            );

            this.PostStatus.Value = "";
        }

        public void OpenStatus()
        {
            Console.WriteLine("");
        }
    }
}