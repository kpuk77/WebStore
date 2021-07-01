using System.Collections.Generic;
using System.Linq;

using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping.DTO.Orders
{
    public static class OrderDTOMapping
    {
        public static OrderDTO ToDTO(this Order order) => order is null
            ? null
            : new OrderDTO
            {
                Id = order.Id,
                Name = order.Name,
                Phone = order.Phone,
                Address = order.Address,
                Date = order.TimeOrder,
                Items = order.Items.ToDTO()
            };

        public static Order FromDTO(this OrderDTO order) => order is null
            ? null
            : new Order
            {
                Id = order.Id,
                Name = order.Name,
                Phone = order.Phone,
                Address = order.Address,
                TimeOrder = order.Date,
                Items = order.Items.FromDTO().ToList()
            };

        public static IEnumerable<OrderDTO> ToDTO(this IEnumerable<Order> orders) => orders.Select(ToDTO);
        public static IEnumerable<Order> FromDTO(this IEnumerable<OrderDTO> orders) => orders.Select(FromDTO);
    }
}
