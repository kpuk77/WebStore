using System.Collections.Immutable;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Mapping;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly ILogger<ProductsController> _Logger;

        public ProductsController(IProductData productData, ILogger<ProductsController> logger)
        {
            _ProductData = productData;
            _Logger = logger;
        }
        public IActionResult Index() => View(_ProductData.GetProducts()/*.Take(10)*/);

        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            if (_ProductData.GetProductById(id) is not { } product)
                return NotFound();

            return View(product.ToViewModel());
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = _ProductData.GetProductById(model.Id);

            if (product is null)
                return BadRequest();

            var section = _ProductData.GetSections().FirstOrDefault(s => s.Name == model.Section);
            var brand = _ProductData.GetBrands().FirstOrDefault(b => b.Name == model.Brand);

            product.Name = model.Name;
            product.Section = section ??= new Section { Name = model.Section };
            product.Brand = brand ??= new Brand { Name = model.Brand };
            product.ImageUrl = model.ImageUrl;
            product.Price = model.Price;

            _ProductData.Update(product);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            if (_ProductData.GetProductById(id) is not { } product)
                return NotFound();

            return View(product.ToViewModel());
        }


        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _ProductData.GetProductById(id);

            _ProductData.Remove(product);

            return RedirectToAction("Index");
        }
    }
}
