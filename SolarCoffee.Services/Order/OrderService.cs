using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Services.Product;

namespace SolarCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(
            SolarDbContext db,
            ILogger<OrderService> logger,
            IProductService productService,
            IInventoryService inventoryService)
        {
            _db = db;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        public List<SalesOrder> GetOrders()
        {
            return _db.SalesOrders
                .Include(order => order.Customer)
                .ThenInclude(customer => customer.PrimaryAddress)
                .Include(order => order.SalesOrderItems)
                .ThenInclude(item => item.Product)
                .ToList();
        }

        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            try
            {
                _logger.LogInformation("Generate invoice for order");

                foreach (var item in order.SalesOrderItems)
                {
                    item.Product = _productService.GetProductById(item.Product.Id);
                    item.Quantity = item.Quantity;

                    var inventoryId = _inventoryService.GetByProductId(item.Product.Id).Id;

                    _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
                }

                _db.SalesOrders.Add(order);
                _db.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Order Created",
                    Time = DateTime.UtcNow,
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = e.Message,
                    Time = DateTime.UtcNow,
                };
            }
        }

        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            var now = DateTime.UtcNow;
            var order = _db.SalesOrders.Find(id);

            order.IsPaid = true;
            order.UpdatedOn = now;

            try
            {
                _db.SalesOrders.Update(order);
                _db.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Order closed successfully",
                    Time = DateTime.UtcNow,
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = e.Message,
                    Time = DateTime.UtcNow,
                };
            }
        }
    }
}