using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using GoodNewsAggregator.Core.Services.Interfaces;
using Serilog;

namespace GoodNewsAggregator.Services.Implementation
{
    public class S13Parser : IWebPageParser
    {
        public async Task<string> Parse(string url)
        {
            try
            {
                var web = new HtmlWeb();

                var htmlDoc = web.LoadFromWebAsync(url);

                var htmlDocResult = htmlDoc.Result;

                var node = htmlDocResult.DocumentNode.SelectSingleNode("//div[@class='content']");

                if (node.InnerHtml.IndexOf("<h2 class=\"comments\">Читайте по теме:</h2>") >= 0)
                {
                    int index = node.InnerHtml.IndexOf("<h2 class=\"comments\">Читайте по теме:</h2>");

                    node.InnerHtml = node.InnerHtml.Substring(0, index);
                }

                if (node.InnerHtml.IndexOf("<div class=\"content\">") >= 0)
                {
                    int index2 = node.InnerHtml.IndexOf("<div class=\"content\">");

                    string htmlShouldBeRemoved = node.InnerHtml.Substring(0, index2);

                    node.InnerHtml = node.InnerHtml.Replace(htmlShouldBeRemoved, "");
                }

                node.InnerHtml = node.InnerHtml.
                    Replace("<ul class=\"cols top\">", "<ul class=\"cols top\"  style=\"display: none;\">")
                    .Replace("<img class=\"main lazyload\"", "<img class=\"main lazyload\" style=\"display: none;\"")
                    .Replace("<iframe", "<iframe style=\"display: none;\"")
                    .Replace("<div class=\"swiper-container slides\">", "<div class=\"swiper-container slides\" style=\"display: none;\">")
                    .Replace("<ul", "<text")
                    .Replace("</ul", "</text");


                if (node != null)
                {
                    return node.InnerHtml;
                }
                return null;
            }
            catch (Exception e)
            {
                Log.Error(e, "S13Parser was not successful");
                throw;
            }            
        }
    }
}