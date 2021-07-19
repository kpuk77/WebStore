using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;

namespace WebStore.TagHelpers
{
    public class Paging : TagHelper
    {
        private readonly IUrlHelperFactory _UrlHelperFactory;

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public Paging(IUrlHelperFactory urlHelperFactory) => _UrlHelperFactory = urlHelperFactory;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            var urlHelper = _UrlHelperFactory.GetUrlHelper(ViewContext);
            for (int i = 1; i <= PageViewModel.TotalPages; i++)
                ul.InnerHtml.AppendHtml(CreateElement(i, urlHelper));

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreateElement(int pageNumber, IUrlHelper url)
        {
            var li = new TagBuilder("li");
            var a = new TagBuilder("a");

            if(pageNumber == PageViewModel.Page)
                li.AddCssClass("active");
            else
            {
                PageUrlValues["page"] = pageNumber;
                a.Attributes["href"] = url.Action(PageAction, PageUrlValues);
            }

            a.InnerHtml.AppendHtml(pageNumber.ToString());
            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}
