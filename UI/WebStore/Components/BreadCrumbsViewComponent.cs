using Microsoft.AspNetCore.Mvc;

using WebStore.Interfaces.Services;
using WebStore.ViewModels;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BreadCrumbsViewComponent(IProductData productData) => _ProductData = productData;

        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbsViewModel();

            if (int.TryParse(Request.Query["SectionId"], out var sectionId))
            {
                model.Section = _ProductData.GetSection(sectionId);
                if (model.Section?.ParentId is { } parentId)
                    model.Section.Parent = _ProductData.GetSection(parentId);
            }

            if (int.TryParse(Request.Query["BrandId"], out var brandId))
                model.Brand = _ProductData.GetBrand(brandId);

            if (int.TryParse(ViewContext.RouteData.Values["id"]?.ToString(), out var productId))
                model.Product = _ProductData.GetProduct(productId);

            return View(model);
        }
    }
}
