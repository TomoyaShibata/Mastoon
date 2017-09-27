using Mastonet;

namespace Mastoon.Models
{
    public interface ITimelineModel
    {
        void SetupTimelineModel(MastodonClient mastodonClient);
        void GetFirstPageTimelineAsync();
        void StartStreamingTimelineAsync();
    }
}