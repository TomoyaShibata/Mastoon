using Mastonet;
using Mastonet.Entities;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Reactive.Bindings;

namespace Mastoon.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        public DelegateCommand PostStatusCommand { get; }
        public InteractionRequest<INotification> PostRequest { get; } = new InteractionRequest<INotification>();

        private string _input;

        public string Input
        {
            get => this._input;
            set => this.SetProperty(ref this._input, value);
        }

        public ReactiveCollection<Status> Statuses { get; } = new ReactiveCollection<Status>();

        private MastodonClient _mastodonClient;

        public MainWindowViewModel()
        {
            this.PostStatusCommand = new DelegateCommand(this.PostStatusAsync);

            this.SetupMastodonClient();
        }

        private async void SetupMastodonClient()
        {
            var authClient = new AuthenticationClient("m6n.onsen.tech");
            var appRegistration = await authClient.CreateApp("Mastdoon", Scope.Read | Scope.Write | Scope.Follow);

            var auth = await authClient.ConnectWithPassword("wind.of.hometown+m6n.onsen.tech@gmail.com",
                "Bn0Qi5FbEH7pdjTc");
            this._mastodonClient = new MastodonClient(appRegistration, auth);
            this.StartGetPublicStreamingAsync();
        }

        public async void StartGetPublicStreamingAsync()
        {
            var streaming = this._mastodonClient.GetPublicStreaming();
            streaming.OnUpdate += (sender, e) => this.Statuses.Insert(0, e.Status);
            await streaming.Start();
        }

        public async void PostStatusAsync()
        {
            await this._mastodonClient.PostStatus(
                "テスト",
                Visibility.Public
            );
        }
    }
}