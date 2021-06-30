using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping.DTO
{
    public static class ProductDTOMapping
    {
        public static Product FromDTO(this ProductDTO productDTO) => productDTO is null
            ? null
            : new Product
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Order = productDTO.Order,
                Price = productDTO.Price,
                ImageUrl = productDTO.ImageUrl,
                Brand = productDTO.Brand.FromDTO(),
                Section = productDTO.Section.FromDTO()
            };

        public static ProductDTO ToDTO(this Product product) => product is null
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

        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> productsDTO) =>
            productsDTO.Select(FromDTO);

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> products) =>
            products.Select(ToDTO);
    }
}