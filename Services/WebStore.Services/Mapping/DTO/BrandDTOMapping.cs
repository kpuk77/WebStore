using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping.DTO
{
    public static class BrandDTOMapping
    {
        public static Brand FromDTO(this BrandDTO brandDTO) => brandDTO is null
            ? null
            : new Brand
            {
                Id = brandDTO.Id,
                Name = brandDTO.Name,
                Order = brandDTO.Order,
            };

        public static BrandDTO ToDTO(this Brand brand) => brand is null
            ? null
            : new BrandDTO
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order,
            };

        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO> brandsDTO) =>
            brandsDTO.Select(FromDTO);

        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> brands) =>
            brands.Select(ToDTO);
    }
}