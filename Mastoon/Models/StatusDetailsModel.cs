using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Mastoon.Entities;
using Microsoft.Practices.ObjectBuilder2;

namespace Mastoon.Models
{
    public class StatusDetailsModel
    {
        public ObservableCollection<ContentPart> ContentParts = new ObservableCollection<ContentPart>();

        public void SetNewContentParts(string content)
        {
            this.ParseContent(content);
        }

        public void ParseContent(string content)
        {
            ContentParts.Clear();

            var hyperlinkTargets = GetHyperlinkTargets(content);
            var splitedNormalizedContent = GetSplitedNormalizedContent(content);
            splitedNormalizedContent.ForEach(c =>
            {
                var (target, url) = hyperlinkTargets.Find(h => h.target == c);
                if (target == null)
                {
                    this.ContentParts.Add(new ContentPart {Text = c});
                    return;
                }

                this.ContentParts.Add(new ContentPart
                    {
                        Text = c,
                        Type = "url",
                        Url = url
                    }
                );
            });
        }

        public static List<(string target, string url)> GetHyperlinkTargets(string content)
        {
            var ankerRegex = new Regex("<a href=\"(.*?)\".*?>(.*?)</a>");
            var matches = ankerRegex.Matches(content);

            return matches.Cast<Match>()
                .Select(m => (
                    target: GetNormalizedContent(m.Groups[2].Value),
                    url: m.Groups[1].Value
                    )
                )
                .ToList();
        }

        public static IEnumerable<string> GetSplitedNormalizedContent(string content)
        {
            var normalizedContent = GetNormalizedContent(content);
            return new Regex(@"<.*?>").Split(normalizedContent).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        public static string GetNormalizedContent(string content)
            => content.Replace("<span class=\"invisible\">", "")
                .Replace("<span class=\"ellipsis\">", "")
                .Replace("<span class=\"\">", "")
                .Replace("<span>", "")
                .Replace("</span>", "");
    }
}