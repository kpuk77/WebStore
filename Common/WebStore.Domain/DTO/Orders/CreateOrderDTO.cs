using System.Collections.Generic;

using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTO.Orders
{
    public class CreateOrderDTO
    {
        public OrderViewModel Order { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
