using System.Collections.Generic;
using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO.Products
{
    public record ProductsPageDTO(IEnumerable<ProductDTO> Products, int TotalCount);
}
