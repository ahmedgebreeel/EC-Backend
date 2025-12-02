using Business.Services;
using Core.DTOs.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly ILogger<OrderController> logger;

        public OrderController(OrderService _orderService, ILogger<OrderController> _logger)
        {
            orderService = _orderService;
            logger = _logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var order = await orderService.GetAllAsync();
                return Ok(order);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");

            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            try
            {
                var order = await orderService.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound("Order not found");
                }
                return Ok(order);

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");

            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserIdAsync(string userId)
        {
            try
            {
                var order = await orderService.GetByUserIdAsync(userId);
                if(order == null)
                {
                    return NotFound("Order not found");

                }
                return Ok(order);
            }
            catch (Exception ex) {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddOrderDto order)
        {
            try {

                await orderService.AddAsync(order);
                return Ok("Order created successfully ");

            }
            catch(InvalidOperationException e)
            {
                logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
            
        }
    }
}
