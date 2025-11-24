using Business.Services;
using Core.DTOs.ShoppingCart;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // Get all shopping carts
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var carts = await _shoppingCartService.GetAllCartsAsync();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Get Shopping Cart by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            try
            {
                var cart = await _shoppingCartService.GetCartByIdAsync(id);
                if (cart == null)
                {
                    return NotFound("Shopping cart not found");
                }
                return Ok(cart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Create Shopping Cart
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddShoppingCartDto addCartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Map AddShoppingCartDto to ShoppingCartDto (assuming AddCartItems if needed)
                var cartDto = new ShoppingCartDto
                {
                    UserId = addCartDto.UserId
                    // You can map CartItems here if AddShoppingCartDto includes any
                };

                var createdCart = await _shoppingCartService.AddCartAsync(cartDto);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = createdCart.Id }, createdCart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Delete Shopping Cart
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await _shoppingCartService.DeleteCartAsync(id);
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
