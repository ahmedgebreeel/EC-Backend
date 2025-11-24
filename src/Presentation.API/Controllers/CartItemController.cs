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

        public CartItemController(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        // Add Cart Item
        [HttpPost("add")]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemDto addCartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdItem = await _cartItemService.AddCartItemAsync(addCartItemDto);
                return CreatedAtAction(nameof(GetCartItemById), new { id = createdItem.Id }, createdItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Get Cart Item by Id (id from route)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Update Cart Item (id from route, DTO from body)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(string id, [FromBody] UpdateCartItemDto updateCartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _cartItemService.UpdateCartItemAsync(id, updateCartItemDto);
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
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Delete Cart Item by Id (id from route)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(string id)
        {
            try
            {
                await _cartItemService.DeleteCartItemAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
