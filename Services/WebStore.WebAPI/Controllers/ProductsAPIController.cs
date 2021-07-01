using Microsoft.AspNetCore.Mvc;

using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping.DTO.Products;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route(APIAddress.PRODUCTS)]
    public class ProductsAPIController : ControllerBase
    {
        private readonly IProductData _ProductData;

        public ProductsAPIController(IProductData productData) => _ProductData = productData;

        [HttpGet("sections")]
        public IActionResult GetSections() => Ok(_ProductData.GetSections().ToDTO());

        [HttpGet("sections/{id}")]
        public IActionResult GetSection(int id) => Ok(_ProductData.GetSection(id).ToDTO());

        [HttpGet("brands")]
        public IActionResult GetBrands() => Ok(_ProductData.GetBrands().ToDTO());

        [HttpGet("brands/{id}")]
        public IActionResult GetBrand(int id) => Ok(_ProductData.GetBrand(id).ToDTO());

        [HttpPost]
        public IActionResult GetProducts(ProductFilter filter = null) => Ok(_ProductData.GetProducts(filter).ToDTO());

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) => Ok(_ProductData.GetProduct(id).ToDTO());

        [HttpPost("add")]
        public IActionResult Add(Product product) => Ok(_ProductData.Add(product));

        [HttpDelete("{id}")]
        public IActionResult Remove(int id) => Ok(_ProductData.Remove(id));

        [HttpPut]
        public IActionResult Update(Product product)
        {
            _ProductData.Update(product);
            return Ok(true);
        }
    }
}
