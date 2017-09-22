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
        public DelegateCommand PostCommand { get; }
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
            this.PostCommand = new DelegateCommand(this.PostAsync);

            this.SetupMastodonClient();
        }

        private async void SetupMastodonClient()
        {
            var authClient = new AuthenticationClient("");
            var appRegistration = await authClient.CreateApp("Mastdoon", Scope.Read | Scope.Write | Scope.Follow);

            var auth = await authClient.ConnectWithPassword("",
                "");

            this._mastodonClient = new MastodonClient(appRegistration, auth);
            this.HogeAsync();
        }

        public async void HogeAsync()
        {
            var streaming = this._mastodonClient.GetPublicStreaming();
            streaming.OnUpdate += (sender, e) => this.Statuses.Insert(0, e.Status);
            await streaming.Start();
        }

        public async void PostAsync()
        {
            await this._mastodonClient.PostStatus(
                "テスト",
                Visibility.Public
            );
        }
    }
}