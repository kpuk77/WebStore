using System.Collections.Generic;
using System.Threading.Tasks;

using WebStore.Domain.Entities;
using WebStore.ViewModels;

namespace WebStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrders(string userName);

        Task<Order> GetOrderById(int id);

        Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel);
    }
}
