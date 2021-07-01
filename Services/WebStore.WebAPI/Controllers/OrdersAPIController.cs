using Microsoft.AspNetCore.Mvc;

using System.Linq;
using System.Threading.Tasks;

using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping.DTO.Orders;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route(APIAddress.ORDERS)]
    public class OrdersAPIController : ControllerBase
    {
        private readonly IOrderService _OrderService;

        public OrdersAPIController(IOrderService orderService) => _OrderService = orderService;

        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetOrders(string userName)
        {
            var orders = await _OrderService.GetOrders(userName);
            return Ok(orders.ToDTO());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _OrderService.GetOrderById(id);
            return Ok(order.ToDTO());
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> CreateOrder(string userName, [FromBody]CreateOrderDTO orderDTO)
        {
            var order = await _OrderService.CreateOrder(userName, orderDTO.Items.ToViewModel(), orderDTO.Order);
            return Ok(order.ToDTO());
        }
    }
}
