using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Customer;
using SolarCoffee.Services.Order;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(ICustomerService customerService, IOrderService orderService,
            ILogger<OrderController> logger)
        {
            _logger = logger;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost("/api/invoice")]
        public ActionResult GenerateNewOrder([FromBody] InvoiceModel invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Generating invoice");
            var order = OrderMapper.SerializeInvoiceToOrder(invoice);
            order.Customer = _customerService.GetById(invoice.CustomerId);
            _orderService.GenerateOpenOrder(order);
            return Ok(order);
        }

        [HttpGet("/api/order")]
        public ActionResult GetOrders()
        {
            _logger.LogInformation("Getting orders");
            var orders = _orderService.GetOrders();
            var ordersViewModel = OrderMapper.SerializeOrdersToViewModels(orders);
            return Ok(ordersViewModel);
        }

        [HttpPatch("/api/order/complete/{id}")]
        public ActionResult MarkOrderCompleted(int id)
        {
            _logger.LogInformation("mark full order completed");
            _orderService.MarkFulfilled(id);
            return Ok();
        }
        
    }
}