using Business.Services;
using Core.DTOs.OrderItems;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly OrderItemService orderItemService;
        private readonly ILogger<OrderItemsController> logger;

        public OrderItemsController(OrderItemService _orderItemService, ILogger<OrderItemsController> _logger)
        {
            orderItemService = _orderItemService;
            logger = _logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddOrderItemsDto orderItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await orderItemService.AddAsync(orderItemDto);
                return Created();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateOrderItemDto updateOrderItemsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await orderItemService.UpdateAsync(id, updateOrderItemsDto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await orderItemService.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
