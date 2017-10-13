using System;
using System.Diagnostics;
using Mastonet;
using Mastonet.Entities;
using Mastoon.Entities;
using Mastoon.Models;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Mastoon.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly HomeTimelineModel _homeTimelineModel = new HomeTimelineModel();
        public ReadOnlyReactiveCollection<Status> HomeTimelineStatuses { get; }
        public ReactiveProperty<int> SelectedHomeTimelineStatusIndex { get; set; }

        private readonly FavouriteTimelineModel _favouriteTimelineModel = new FavouriteTimelineModel();
        public ReadOnlyReactiveCollection<Status> FavouriteTimelineStatuses { get; }
        public ReactiveProperty<int> SelectedFavoriteTimelineStatusIndex { get; set; }

        private readonly PublimeTimelineModel _publicTimelineModel = new PublimeTimelineModel();
        public ReadOnlyReactiveCollection<Status> PubliceTimelineStatuses { get; }
        public ReactiveProperty<int> SelectedStatusIndex { get; set; } = new ReactiveProperty<int>();

        public ReactiveProperty<Status> SelectedStatus { get; set; } = new ReactiveProperty<Status>();

        private readonly ReblogModel _reblogModel = new ReblogModel();

        public ReactiveCommand GetPrevPageHomeTimelineCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ToggleReblogCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ToggleFavouriteCommand { get; } = new ReactiveCommand();

        private readonly StatusPostModel _statusPostModel = new StatusPostModel();

        public DelegateCommand CustomCommand { get; }
        public ReactiveCommand SelectedStatusIncrementCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SelectedStatusDecrementCommand { get; } = new ReactiveCommand();

        private readonly StatusDetailsModel _statusDetailsModel = new StatusDetailsModel();

        public ReadOnlyReactiveCollection<ContentPart> ContentParts { get; set; }
        public ReactiveCollection<Status> Statuses { get; } = new ReactiveCollection<Status>();

        public ReactiveProperty<string> PostStatusContent { get; set; }
        public ReadOnlyReactiveCollection<string> VisibilityTexts { get; }
        public ReactiveProperty<int> SelectedVisibilityIndex { get; }
        public ReactiveCommand PostStatusCommand { get; } = new ReactiveCommand();

        private MastodonClient _mastodonClient;

        public MainWindowViewModel()
        {
            this.SetupMastodonClient();

            this.HomeTimelineStatuses = this._homeTimelineModel.HomeTimelineStatuses.ToReadOnlyReactiveCollection();
            this.FavouriteTimelineStatuses =
                this._favouriteTimelineModel.FavouriteTimelineStatuses.ToReadOnlyReactiveCollection();
            this.PubliceTimelineStatuses =
                this._publicTimelineModel.PublicTimelineStatuses.ToReadOnlyReactiveCollection();
            this.PostStatusContent = this._statusPostModel.ToReactivePropertyAsSynchronized(x => x.Content);
            this.VisibilityTexts = this._statusPostModel.VisibilityTexts.ToReadOnlyReactiveCollection();
            this.SelectedVisibilityIndex =
                this._statusPostModel.ToReactivePropertyAsSynchronized(x => x.SelectedVisibilityIndex);

            this.GetPrevPageHomeTimelineCommand.Subscribe(this._homeTimelineModel.GetPrevPageTimelineAsync);

            this.ToggleReblogCommand.Subscribe(() =>
                this._reblogModel.ToggleReblog(
                    this.SelectedStatus.Value.Id,
                    this.SelectedStatus.Value.Reblogged ?? false)
            );

            this.CustomCommand = new DelegateCommand(this.OpenStatus);
            this.ToggleFavouriteCommand.Subscribe(() =>
                this._favouriteTimelineModel.ToggleFavourite(this.SelectedStatus.Value));
            this.SelectedStatusIncrementCommand.Subscribe(this.SelectedStatusIndexIncrement);
            this.SelectedStatusDecrementCommand.Subscribe(this.SelectedStatusIndexDecrement);
            this.PostStatusCommand.Subscribe(this.PostStatus);

            this.SelectedStatus.PropertyChanged += (sender, e) => this.ShowSelectedStatus();

            this.ContentParts = this._statusDetailsModel.ContentParts;
        }

        private async void SetupMastodonClient()
        {
            var authClient = new AuthenticationClient("");
            var appRegistration = await authClient.CreateApp("Mastdoon", Scope.Read | Scope.Write | Scope.Follow);

            var auth = await authClient.ConnectWithPassword("",
                "");
            this._mastodonClient = new MastodonClient(appRegistration, auth);

            this._homeTimelineModel.SetupTimelineModel(this._mastodonClient);
            this._favouriteTimelineModel.SetupTimelineModel(this._mastodonClient);
            this._publicTimelineModel.SetupTimelineModel(this._mastodonClient);
            this._statusPostModel.SetupStatusPostModel(this._mastodonClient);
            this._reblogModel.SetupMastodonModel(this._mastodonClient);

            this.ReblogModelPropetyChanged();
        }

        public void PostStatus() => this._statusPostModel.PostStatusAsync();

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

        private void ShowSelectedStatus() =>
            this._statusDetailsModel.SetNewContentParts(this.SelectedStatus.Value.Content);

        private void ReblogModelPropetyChanged()
        {
            this._reblogModel.PropertyChanged += (sender, args) =>
            {
                var reblogModel = (ReblogModel) sender;
                Console.WriteLine(this._reblogModel.Number);
                this.UpdateAllTimelineStatus(reblogModel.NewStatus);
            };
        }

        private void UpdateAllTimelineStatus(Status status)
        {
            this._homeTimelineModel.UpdateStatus(status);
        }
    }
}