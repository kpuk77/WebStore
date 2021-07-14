using System.Linq;

using Microsoft.AspNetCore.Mvc;

using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData productData) => _ProductData = productData;

        public IActionResult Index(int? brandId, int? sectionId)
        {
            var filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
            };

            var products = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                Products = products.OrderBy(p => p.Order).ToViewModels()
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
