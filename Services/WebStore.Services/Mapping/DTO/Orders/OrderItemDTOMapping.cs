using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping.DTO.Orders
{
    public static class OrderItemDTOMapping
    {
        public static OrderItemDTO ToDTO(this OrderItem orderItem) =>
            orderItem is null
                ? null
                : new OrderItemDTO
                {
                    Id = orderItem.Id,
                    ProductId = orderItem.Product.Id,
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity
                };

        public static OrderItem FromDTO(this OrderItemDTO orderItem) =>
            orderItem is null
                ? null
                : new OrderItem
                {
                    Id = orderItem.Id,
                    Product = new Product { Id = orderItem.ProductId },
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity
                };

        public static IEnumerable<OrderItemDTO> ToDTO(this IEnumerable<OrderItem> items) =>
            items.Select(ToDTO);

        public static IEnumerable<OrderItem> FromDTO(this IEnumerable<OrderItemDTO> items) =>
            items.Select(FromDTO);
    }
}
