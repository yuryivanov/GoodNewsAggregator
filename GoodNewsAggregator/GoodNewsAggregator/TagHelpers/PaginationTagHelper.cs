using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using GoodNewsAggregator.Models;
using Serilog;

namespace GoodNewsAggregator.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        //standard for any pagination:
        private readonly IUrlHelperFactory _urlHelperFactory; 

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public PageInfo PageModel { get; set; }

        //standard for any pagination starts:
        public string PageAction { get; set; }
        public string SourseId { get; set; }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        //standard for any pagination ends

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            try
            {
                var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext); //urlHelper created
                var result = new TagBuilder("div"); //div tags created: <div></div>

                for (int i = 1; i <= PageModel.TotalPages; i++)
                {
                    var tag = new TagBuilder("a"); //a tags created: <a></a>
                    var anchorInnerHtml = i.ToString(); //url for each page of 1 2 3 4 5 6 ... created
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i }); //href attribute for <a></a> created
                    tag.InnerHtml.Append(anchorInnerHtml); //url placed in tag <a>
                    result.InnerHtml.AppendHtml(tag); //url with tag <a> placed in tag <div>
                }

                output.Content.AppendHtml(result.InnerHtml); //pagination created
            }
            catch (Exception e)
            {
                Log.Error(e, "Process was not successful");
                throw;
            }            
        }

        //public string GetAnchorInnerHtml(int i, PageInfo info)
        //{
        //    var anchorInnerHtml = "";
        //    if (info.TotalPages<=10)
        //    {
        //        anchorInnerHtml = i.ToString();
        //    }
        //    else
        //    {
        //        if ((i-info.PageNumber >= 2 || info.PageNumber - i >= 2) && i != 0 && i != info.TotalPages)
        //        {
        //            anchorInnerHtml = "..";
        //        }
        //        else
        //        {
        //            anchorInnerHtml = i.ToString();
        //        }
        //    }
        //}
    }
}