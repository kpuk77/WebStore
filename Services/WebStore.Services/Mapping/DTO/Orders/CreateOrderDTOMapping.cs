using System.Collections.Generic;
using System.Linq;

using WebStore.Domain.DTO.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping.DTO.Orders
{
    public static class CreateOrderDTOMapping
    {
        public static IEnumerable<OrderItemDTO> ToDTO(this CartViewModel cart) =>
            cart.Items.Select(p => new OrderItemDTO
            {
                ProductId = p.product.Id,
                Price = p.product.Price,
                Quantity = p.quantity
            });

        public static CartViewModel ToViewModel(this IEnumerable<OrderItemDTO> items) =>
            new CartViewModel
            {
                Items = items.Select(p => (new ProductViewModel {Id = p.ProductId}, p.Quantity))
            };
    }
}
