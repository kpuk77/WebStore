using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InSQL
{
    public class SqlOrderData : IOrderService
    {
        private readonly WebStoreDB _Db;
        private readonly UserManager<User> _UserManager;
        private readonly ILogger<SqlOrderData> _Logger;

        public SqlOrderData(WebStoreDB db, UserManager<User> userManager, ILogger<SqlOrderData> logger)
        {
            _Db = db;
            _UserManager = userManager;
            _Logger = logger;
        }

        public async Task<IEnumerable<Order>> GetOrders(string userName) => await _Db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .Where(o => o.User.UserName == userName)
                .ToArrayAsync();

        public async Task<Order> GetOrderById(int id)
        {
            return await _Db.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel)
        {
            var user = await _UserManager.FindByNameAsync(userName);
            if (user is null)
                throw new InvalidOperationException($"Пользователь {userName} не найден");

            await using var transaction = await _Db.Database.BeginTransactionAsync();

            var order = new Order
            {
                User = user,
                Address = orderModel.Address,
                Phone = orderModel.Phone,
                Name = orderModel.Name,
            };

            var productIds = cart.Items.Select(item => item.product.Id).ToArray();

            var cartProducts = await _Db.Products
                .Where(p => productIds.Contains(p.Id))
                .ToArrayAsync();

            order.Items = cart.Items.Join(
                cartProducts,
                cartItem => cartItem.product.Id,
                cartProduct => cartProduct.Id,
                (cartItem, cartProduct) => new OrderItem
                {
                    Order = order,
                    Product = cartProduct,
                    Price = cartProduct.Price, // сюда можно применить скидки...
                    Quantity = cartItem.quantity,
                }).ToArray();

            await _Db.Orders.AddAsync(order);
            await _Db.SaveChangesAsync();

            await transaction.CommitAsync();

            return order;
        }
    }
}
