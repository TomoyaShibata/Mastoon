using System.Linq;
using Mastoon.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MastoonTest.Models
{
    [TestClass]
    public class StatusDetailsModelTest
    {
        private const string Content =
                @"<p>メソッド名が思いつかなくてカッとなってやった <a href=""https://m6n.onsen.tech/media/8ADn-yJ3AVPBzq-uV0k"" rel=""nofollow noopener"" target=""_blank""><span class=""invisible""> https://</span><span class=""ellipsis"">m6n.onsen.tech/media/8ADn-yJ3A</span><span class=""invisible"">VPBzq-uV0k</span></a></p>"
            ;

        [Ignore("テスト対象の System.ValueTuple の解決に失敗するため")]
        [TestMethod]
        public void ハイパーリンク化対象文字列とurlを紐付けたListを取得できる()
        {
            var result = StatusDetailsModel.GetHyperlinkTargets(Content);
            var expected = (
                target: "https://m6n.onsen.tech/media/8ADn-yJ3AVPBzq-uV0k",
                url: "https://m6n.onsen.tech/media/8ADn-yJ3AVPBzq-uV0k"
                );
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Contentを正規化して配列した結果を取得できる()
        {
            var splitedNormalizedContent = StatusDetailsModel.GetSplitedNormalizedContent(Content);
            var expected = new string[2]
            {
                "メソッド名が思いつかなくてカッとなってやった ",
                " https://m6n.onsen.tech/media/8ADn-yJ3AVPBzq-uV0k"
            };
            CollectionAssert.AreEqual(expected, splitedNormalizedContent.ToArray());
        }
    }
}