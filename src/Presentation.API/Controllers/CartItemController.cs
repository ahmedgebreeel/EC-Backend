using Business.Services;
using Core.DTOs.CartItem;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartItemController : ControllerBase
{
    private readonly CartItemService cartItemService;
    private readonly ILogger<CartItemController> logger;

    public CartItemController(CartItemService _cartItemService, ILogger<CartItemController> _logger)
    {
        cartItemService = _cartItemService;
        logger = _logger;
    }

    // these 2 methods are not needed
    // [HttpGet]
    // public async Task<IActionResult> GetAllAsync()
    // {
    //     try
    //     {
    //         var cartItems = await cartItemService.GetAllAsync();
    //         return Ok(cartItems);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         return StatusCode(500, "Internal server error");
    //     }
    // }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetByIdAsync(string id)
    // {
    //     try
    //     {
    //         var cartItem = await cartItemService.GetByIdAsync(id);
    //         if (cartItem == null)
    //         {
    //             return NotFound("Cart item not found");
    //         }

    //         return Ok(cartItem);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         return StatusCode(500, "Internal server error");
    //     }
    // }

    [HttpPost]
    public async Task<IActionResult> AddAsync(AddCartItemDto addCartItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await cartItemService.AddAsync(addCartItemDto);
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
    public async Task<IActionResult> UpdateAsync(string id, UpdateCartItemDto updateCartItemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await cartItemService.UpdateAsync(id, updateCartItemDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(ex.Message);
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
            await cartItemService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}
