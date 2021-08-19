using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _ProductData = productData;
            _Configuration = configuration;
        }

        public IActionResult Index(int? brandId, int? sectionId, int page = 1, int? pageSize = null)
        {
            pageSize ??= int.TryParse(_Configuration["CatalogPageSize"], out var size) ? size : 3;

            var filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = page,
                PageSize = pageSize
            };

            var (products, totalCount) = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                Products = products.OrderBy(p => p.Order).ToViewModels(),
                PageViewModel = new PageViewModel
                {
                    Page = page,
                    PageSize = pageSize ?? 0,
                    TotalItems = totalCount
                }
            });
        }

        public IActionResult Details(int id)
        {
            var product = _ProductData.GetProduct(id);

            if (product is null)
                return NotFound();

            return View(product.ToViewModel());
        }
    }
}
