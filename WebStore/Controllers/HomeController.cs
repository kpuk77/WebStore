﻿using Microsoft.AspNetCore.Mvc;

using System.Linq;

using WebStore.Infrastructure.Mapping;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices]IProductData productData)
        {
            var products = productData.GetProducts()
                .Take(9)
                .Select(p => p.ToViewModel());

            return View(products);
        }

        public IActionResult Blog() => View();
        public IActionResult BlogSingle() => View();
        public IActionResult Cart() => View();
        public IActionResult Checkout() => View();
        public IActionResult ContactUs() => View();
        public IActionResult ProductDetails() => View();
    }
}
