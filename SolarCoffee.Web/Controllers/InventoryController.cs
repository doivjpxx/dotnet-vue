using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryService _inventoryService;

        public InventoryController(ILogger<InventoryController> logger, IInventoryService inventoryService)
        {
            _logger = logger;
            _inventoryService = inventoryService;
        }

        [HttpGet("/api/inventory")]
        public ActionResult GetCurrentInventory()
        {
            _logger.LogInformation("Get all inventory");
            var inventory = _inventoryService.GetCurrentInventory()
                .Select(pi => new ProductInventoryModel
                {
                    Id = pi.Id,
                    CreatedOn = pi.CreatedOn,
                    UpdatedOn = pi.UpdatedOn,
                    QuantityOnHand = pi.QuantityOnHand,
                    Product = ProductMapper.SerializeProductModel(pi.Product)
                })
                .OrderBy(inv => inv.CreatedOn)
                .ToList();

            return Ok(inventory);
        }

        [HttpPatch("/api/inventory")]
        public ActionResult UpdateInventory([FromBody] ShipmentModel shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = shipment.ProductId;
            var adjustment = shipment.Adjustment;
            _logger.LogInformation("Updating Inventory: " +
                                   $"for product Id: {id}" +
                                   $" with adjustment: {adjustment}");

            var inventory = _inventoryService.UpdateUnitsAvailable(id, adjustment);
            return Ok(inventory);
        }
    }
}