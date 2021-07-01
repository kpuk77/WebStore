using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping.DTO.Orders;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient client) : base(client, APIAddress.ORDERS) { }
        public async Task<IEnumerable<Order>> GetOrders(string userName)
        {
            var response = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{userName}").ConfigureAwait(false);
            return response.FromDTO();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var response = await GetAsync<OrderDTO>($"{Address}/{id}").ConfigureAwait(false);
            return response.FromDTO();
        }

        public async Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel)
        {
            var orderDTO = new CreateOrderDTO
            {
                Items = cart.ToDTO(),
                Order = orderModel
            };

            var response = await PostAsync($"{Address}/{userName}", orderDTO).ConfigureAwait(false);
            var order = await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<OrderDTO>().ConfigureAwait(false);

            return order.FromDTO();
        }
    }
}
