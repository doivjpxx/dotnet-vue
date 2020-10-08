using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Product;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpPost("/api/product")]
        public ActionResult CreateProduct(ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Create new product");
            var newProduct = ProductMapper.SerializeProductModel(product);
            var productRepsonse = _productService.CreateProduct(newProduct);
            return Ok(productRepsonse);
        }
        
        
        [HttpGet("/api/product")]
        public ActionResult GetProducts()
        {
            _logger.LogInformation("Getting all products");
            var products = _productService.GetAllProducts();
            var productViewModel = products.Select(ProductMapper.SerializeProductModel);
            return Ok(productViewModel);
        }

        [HttpPatch("/api/product/{id}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation("Archiving product");
            var archiveProduct = _productService.ArchiveProduct(id);
            return Ok(archiveProduct);
        }
    }
}