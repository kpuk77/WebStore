using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;

namespace WebStore.Services.Mapping.DTO.Products
{
    public static class ProductDTOMapping
    {
        public static Domain.Entities.Product FromDTO(this ProductDTO productDTO) => productDTO is null
            ? null
            : new Domain.Entities.Product
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Order = productDTO.Order,
                Price = productDTO.Price,
                ImageUrl = productDTO.ImageUrl,
                Brand = productDTO.Brand.FromDTO(),
                Section = productDTO.Section.FromDTO()
            };

        public static ProductDTO ToDTO(this Domain.Entities.Product product) => product is null
            ? null
            : new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand.ToDTO(),
                Section = product.Section.ToDTO()
            };

        public static IEnumerable<Domain.Entities.Product> FromDTO(this IEnumerable<ProductDTO> productsDTO) =>
            productsDTO.Select(FromDTO);

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Domain.Entities.Product> products) =>
            products.Select(ToDTO);

        public static ProductsPageDTO ToDTO(this ProductsPage productsPage) => new ProductsPageDTO(productsPage.Products.ToDTO(), productsPage.TotalCount);

        public static ProductsPage FromDTO(this ProductsPageDTO productsPageDTO) =>
            new ProductsPage(productsPageDTO.Products.FromDTO(), productsPageDTO.TotalCount);
    }
}