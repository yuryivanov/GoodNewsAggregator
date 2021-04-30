using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using GoodNewsAggregator.Core.Services.Interfaces;

namespace GoodNewsAggregator.Services.Implementation
{
    public class OnlinerParser : IWebPageParser
    {
        public async Task<string> Parse(string url)
        {
            var web = new HtmlWeb();

            var htmlDoc = web.LoadFromWebAsync(url);

            var htmlDocResult = htmlDoc.Result;

            var node = htmlDocResult.DocumentNode.SelectSingleNode("//div[@class='news-text']");

            if (node == null)
            {
                node = htmlDocResult.DocumentNode.SelectSingleNode("//div[@id='article_body']");
            }

            if (node != null)
            {
                node.InnerHtml = node.InnerHtml.Replace("<div class=\"news-incut news-incut_extended news-incut_position_right news-incut_shift_top news-helpers_hide_tablet\">",
                    "<div class=\"news-incut news-incut_extended news-incut_position_right news-incut_shift_top news-helpers_hide_tablet\" style=\"display: none;\">");

                return node.InnerHtml;
            }
            return null;
        }
    }
}