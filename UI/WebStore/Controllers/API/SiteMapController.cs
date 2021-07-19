using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using SimpleMvcSitemap;

using WebStore.Interfaces.Services;

namespace WebStore.Controllers.API
{
    public class SiteMapController : ControllerBase
    {
        public IActionResult Index([FromServices] IProductData productData)
        {
            var nodes = new List<SitemapNode>
            {
                new(Url.Action("Index", "Home")),
                new(Url.Action("ContactUs", "Home")),
                new(Url.Action("Index", "Catalog")),
                new(Url.Action("Index", "WebAPI")),
                new(Url.Action("Index", "Blog")),
                new(Url.Action("Single", "Blog")),
            };

            nodes.AddRange(productData.GetSections()
                .Select(s => new SitemapNode(Url.Action("Index", "Catalog", new {sectionId = s.Id}))));

            nodes.AddRange(productData.GetBrands()
                .Select(b => new SitemapNode(Url.Action("Index", "Catalog", new {brandId = b.Id}))));

            nodes.AddRange(productData.GetProducts().Products
                .Select(p => new SitemapNode(Url.Action("Index", "Catalog", new {productId = p.Id}))));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
