using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using GoodNewsAggregator.Core.Services.Interfaces;

namespace GoodNewsAggregator.Services.Implementation
{
    public class S13Parser : IWebPageParser
    {
        public async Task<string> Parse(string url)
        {
            var web = new HtmlWeb();

            var htmlDoc = web.LoadFromWebAsync(url);

            var htmlDocResult = htmlDoc.Result;

            var node = htmlDocResult.DocumentNode.SelectSingleNode(".//div[@class='content']");

            int index = node.InnerHtml.IndexOf("<h2 class=\"comments\">Читайте по теме:</h2>");

            node.InnerHtml = node.InnerHtml.Substring(0, index);

            int index2 = node.InnerHtml.IndexOf("<div class=\"content\">");

            string htmlShouldBeRemoved = node.InnerHtml.Substring(0, index2);

            node.InnerHtml = node.InnerHtml.Replace(htmlShouldBeRemoved, "").
                Replace("<ul class=\"cols top\">", "<ul class=\"cols top\"  style=\"display: none;\">")
                .Replace("<img class=\"main lazyload\"", "<img class=\"main lazyload\" style=\"display: none;\"")
                .Replace("<iframe", "<iframe style=\"display: none;\"")
                .Replace("<div class=\"swiper-container slides\">", "<div class=\"swiper-container slides\" style=\"display: none;\">");

            if (node != null)
            {
                return node.InnerHtml;
            }
            return null;
        }
    }
}