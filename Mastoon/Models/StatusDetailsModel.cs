using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mastoon.Entities;
using Microsoft.Practices.ObjectBuilder2;
using Prism.Mvvm;

namespace Mastoon.Models
{
    public class StatusDetailsModel : BindableBase
    {
        private List<ContentPart> _contentParts = new List<ContentPart>();

        public List<ContentPart> ContentParts
        {
            get => _contentParts;
            set => SetProperty(ref _contentParts, value);
        }

        public void SetNewContentParts(string content)
        {
            this.ParseContent(content);
        }

        public void ParseContent(string content)
        {
            ContentParts = new List<ContentPart>();

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
                    target: m.Groups[0].Value,
                    url: m.Groups[1].Value
                    )
                )
                .ToList();
        }

        public static IEnumerable<string> GetSplitedNormalizedContent(string content)
        {
            var normalizedContent = content.Replace("<span class=\"invisible\">", "")
                .Replace("<span class=\"ellipsis\">", "")
                .Replace("<span class=\"\">", "")
                .Replace("</span>", "");
            return new Regex(@"<.*?>").Split(normalizedContent).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
    }
}