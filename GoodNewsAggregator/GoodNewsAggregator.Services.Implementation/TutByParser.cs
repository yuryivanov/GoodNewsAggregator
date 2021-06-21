using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using GoodNewsAggregator.Core.Services.Interfaces;
using Serilog;

namespace GoodNewsAggregator.Services.Implementation
{
    public class TutByParser : IWebPageParser
    {
        public async Task<string> Parse(string url)
        {
            try
            {
                var web = new HtmlWeb();

                var htmlDoc = web.LoadFromWebAsync(url);

                var htmlDocResult = htmlDoc.Result;

                var node = htmlDocResult.DocumentNode.SelectSingleNode("//div[@class='b-article']");

                if (node == null)
                {
                    node = htmlDocResult.DocumentNode.SelectSingleNode("//div[@id='article_body']");
                }

                if (node == null)
                {
                    node = htmlDocResult.DocumentNode.SelectSingleNode("//div[@class='news-one-data']");
                }

                if (node == null)
                {
                    node = htmlDocResult.DocumentNode.SelectSingleNode("//div[@class='stk-post stk-layout_4col_4203 stk-theme_9740 wp-exclude-emoji']");
                }

                if (node != null)
                {
                    node.InnerHtml = node.InnerHtml.Replace("<time itemprop=\"datePublished\"", "<time itemprop=\"datePublished\" style=\"display: none;\">");
                    node.InnerHtml = node.InnerHtml.Replace("<span itemprop=\"commentCount\">", "<span itemprop=\"commentCount\" style=\"display: none;\">>");
                    node.InnerHtml = node.InnerHtml.Replace("<span itemprop=\"name\">", "<span itemprop=\"name\" style=\"display: none;\">");

                    return node.InnerHtml;
                }
                return null;
            }
            catch (Exception e)
            {
                Log.Error(e, "TutByParser was not successful");
                throw;
            }            
        }
    }
}