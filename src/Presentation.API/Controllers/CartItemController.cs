using Business.Services;
using Core.DTOs.CartItem;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly CartItemService _cartItemService;
        private readonly ILogger<CartItemController> _logger;

        public CartItemController(CartItemService cartItemService, ILogger<CartItemController> logger)
        {
            _cartItemService = cartItemService;
            _logger = logger;
        }

        // Get all cart items
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _cartItemService.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all cart items");
                return StatusCode(500, "Internal server error");
            }
        }

        // Get cart item by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartItemById(string id)
        {
            try
            {
                var item = await _cartItemService.GetCartItemByIdAsync(id);
                if (item == null)
                    return NotFound("Cart item not found");

                return Ok(item);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving cart item with ID: {CartItemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // Add cart item
        [HttpPost("add")]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemDto addCartItemDto)
        {
            try
            {
                var createdItem = await _cartItemService.AddCartItemAsync(addCartItemDto);
                _logger.LogInformation("Cart item added successfully. CartId: {CartId}, ProductId: {ProductId}",
                    addCartItemDto.CartId, addCartItemDto.ProductId);
                return CreatedAtAction(nameof(GetCartItemById), new { id = createdItem.Id }, createdItem);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Add cart item failed: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding cart item");
                return StatusCode(500, "Internal server error");
            }
        }

        // Update cart item
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(string id, [FromBody] UpdateCartItemDto updateCartItemDto)
        {
            try
            {
                await _cartItemService.UpdateCartItemAsync(id, updateCartItemDto);
                _logger.LogInformation("Cart item updated successfully. CartItemId: ", id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating cart item with ID: {CartItemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // Delete cart item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(string id)
        {
            try
            {
                await _cartItemService.DeleteCartItemAsync(id);
                _logger.LogInformation("Cart item deleted successfully. CartItemId: {CartItemId}", id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting cart item with ID: {CartItemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
