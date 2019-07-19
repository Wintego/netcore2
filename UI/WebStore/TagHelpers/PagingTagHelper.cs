using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels.Product;

namespace WebStore.TagHelpers
{
    public class PagingTagHelper: TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }
        [HtmlAttributeName(DictionaryAttributePrefix ="page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public PagingTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var url_helper = urlHelperFactory.GetUrlHelper(ViewContext);
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");
            for (int i = 1; i < PageModel.TotalPages; i++)
            {
                ul.InnerHtml.AppendHtml(CreateItem(i, url_helper));
            }
            base.Process(context, output);
            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreateItem(int page_number, IUrlHelper url_helper)
        {
            var li = new TagBuilder("li");
            var a = new TagBuilder("a");
            if (page_number == PageModel.PageNumber)
                li.AddCssClass("active");
            else
            {
                PageUrlValues["pages"] = page_number;
                a.Attributes["href"] = UrlHelper.Action(PageAction, PageUrlValues);
            }
            a.InnerHtml.AppendHtml(page_number.ToString());
            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}
