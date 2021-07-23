using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.IO;
using System.Linq;

using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly ILogger<ProductsController> _Logger;
        private readonly IWebHostEnvironment _Environment;

        public ProductsController(IProductData productData, ILogger<ProductsController> logger, IWebHostEnvironment environment)
        {
            _ProductData = productData;
            _Logger = logger;
            _Environment = environment;
        }

        public IActionResult Index() => View(_ProductData.GetProducts().Products);

        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            if (_ProductData.GetProduct(id) is not { } product)
                return NotFound();

            return View(product.ToViewModel());
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = _ProductData.GetProduct(model.Id);

            if (product is null)
                return BadRequest();

            var section = _ProductData.GetSections().FirstOrDefault(s => s.Name == model.Section);
            var brand = _ProductData.GetBrands().FirstOrDefault(b => b.Name == model.Brand);

            product.Name = model.Name.Trim();
            product.Section = section ??= new Section { Name = model.Section.Trim() };
            product.Brand = model.Brand is null ? null : brand ??= new Brand { Name = model.Brand.Trim() };
            product.Price = model.Price;

            if (file is { })
            {
                var filePath = "/images/" + file.FileName;
                using var fs = new FileStream(_Environment.WebRootPath + filePath, FileMode.Create);
                file.CopyTo(fs);

                product.ImageUrl = filePath;
            }
            else
                product.ImageUrl = model.ImageUrl.Trim();

            _ProductData.Update(product);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            if (_ProductData.GetProduct(id) is not { } product)
                return NotFound();

            return View(product.ToViewModel());
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _ProductData.Remove(id);

            return RedirectToAction("Index");
        }
    }
}