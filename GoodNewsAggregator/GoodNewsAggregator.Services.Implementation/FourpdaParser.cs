using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using GoodNewsAggregator.Core.Services.Interfaces;

namespace GoodNewsAggregator.Services.Implementation
{
    public class FourPdaParser : IWebPageParser
    {
        public async Task<string> Parse(string url)
        {
            var web = new HtmlWeb();

            var htmlDoc = web.LoadFromWebAsync(url);

            var htmlDocResult = htmlDoc.Result;

            var node = htmlDocResult.DocumentNode.SelectSingleNode("//div[@class='article']");

            if (node == null)
            {
                node = htmlDocResult.DocumentNode.SelectSingleNode("//div[@class='content-box']");
            }

            if (node != null)
            {
                node.InnerHtml = node.InnerHtml.Replace("<div class=\"article-meta\">",
                    "<div class=\"article-meta\" style=\"display: none;\">");
                node.InnerHtml = node.InnerHtml.Replace("<ul", "<text");
                node.InnerHtml = node.InnerHtml.Replace("</ul", "</text");

                return node.InnerHtml;
            }
            return null;
        }
    }
}