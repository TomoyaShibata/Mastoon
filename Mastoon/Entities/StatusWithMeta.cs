using System.ComponentModel;
using System.Runtime.CompilerServices;
using Mastonet.Entities;
using Mastoon.Annotations;

namespace Mastoon.Entities
{
    public class StatusWithMeta : Status, INotifyPropertyChanged
    {
        public StatusWithMeta(Status s)
        {
            this.Mentions = s.Mentions;
            this.MediaAttachments = s.MediaAttachments;
            this.Visibility = s.Visibility;
            this.SpoilerText = s.SpoilerText;
            this.Sensitive = s.Sensitive;
            this.Favourited = s.Favourited;
            this.Reblogged = s.Reblogged;
            this.FavouritesCount = s.FavouritesCount;
            this.Tags = s.Tags;
            this.ReblogCount = s.ReblogCount;
            this.Content = s.Content;
            this.Reblog = s.Reblog;
            this.InReplyToAccountId = s.InReplyToAccountId;
            this.InReplyToId = s.InReplyToId;
            this.Account = s.Account;
            this.Url = s.Url;
            this.Uri = s.Uri;
            this.Id = s.Id;
            this.CreatedAt = s.CreatedAt;
            this.Application = s.Application;
        }

        private bool isRead;

        public bool IsRead
        {
            get => this.isRead;
            set
            {
                this.isRead = value;
                OnPropertyChanged(nameof(IsRead));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}