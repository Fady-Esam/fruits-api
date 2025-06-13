using FruitsAppBackEnd.BL.Interfaces;
using FruitsAppBackEnd.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FruitsAppBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService _dataService;
        public HomeController(IHomeService dataService)
        {
            _dataService = dataService;
        }
        [HttpPost("add-order")]
        public async Task<IActionResult> AddOrder(OrderDto dtoOrder)
        {
            var res = await _dataService.MakeOrder(dtoOrder);

            if (!ModelState.IsValid || res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory(CategoryDto dtoCategory)
        {
            var res = await _dataService.AddCategory(dtoCategory);
            if (!ModelState.IsValid || res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct(ProductDto dtoProduct)
        {
            var res = await _dataService.AddProduct(dtoProduct);
            if (!ModelState.IsValid || res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var res = await _dataService.GetAllProduct();
            if (res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("get-best-selling-products")]
        public async Task<IActionResult> GetBestSellingProducts()
        {
            var res = await _dataService.GetBestSellingProducts();
            if (res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("get-categories")]
        public async Task<IActionResult> GetAllCategory()
        {
            var res = await _dataService.GetAllCategory();
            if (res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("get-products-by-categoryId/{CategoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(int CategoryId)
        {
            var res = await _dataService.GetProductsByCategoryId(CategoryId);
            if (res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("get-products-by-orderId/{OrderId}")]
        public async Task<IActionResult> GetProductsByOrderId(int OrderId)
        {
            var res = await _dataService.GetProductsByOrderId(OrderId);
            if (res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("get-orders-by-productId/{ProductId}")]
        public async Task<IActionResult> GetOrdersByProductId(int ProductId)
        {
            var res = await _dataService.GetOrdersByProductId(ProductId);
            if (res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("add-orderProduct")]
        public async Task<IActionResult> GetOrdersByProductId(OrderProductDto orderProductDto)
        {
            var res = await _dataService.AddOrderProduct(orderProductDto);
            if (!ModelState.IsValid || res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("get-product-by-id/{Id}")]
        public async Task<IActionResult> GetProductById(int Id)
        {
            var res = await _dataService.GetProductById(Id);
            if (res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpGet("get-user-orders-by-user-id/{UserId}")]
        public async Task<IActionResult> GetUserOrders(string UserId)
        {
            var res = await _dataService.GetUserOrders(UserId);
            if (res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpDelete("cancel-order/{OrderId}")]
        public async Task<IActionResult> CancelOrder(int OrderId)
        {
            var res = await _dataService.CancelOrder(OrderId);
            if (res.Errors is not null || res.data is null)
                return BadRequest(res);
            return Ok(res);
        }

    }
} 

