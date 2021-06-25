using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping
{
    public static class ProductMapping
    {
        public static ProductViewModel ToViewModel(this Product product) =>
            product is null
                ? null
                : new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Brand = product.Brand?.Name,
                    Section = product.Section.Name
                };

        public static IEnumerable<ProductViewModel> ToViewModels(this IEnumerable<Product> products) => products.Select(p => p.ToViewModel());
    }
}
