using Business.Services;
using Core.DTOs.ShoppingCart;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly ShoppingCartService shoppingCartService;

    public ShoppingCartController(ShoppingCartService _shoppingCartService)
    {
        shoppingCartService = _shoppingCartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var carts = await shoppingCartService.GetAllAsync();
            return Ok(carts);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        try
        {
            var cart = await shoppingCartService.GetByIdAsync(id);
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

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserIdAsync(string userId)
    {
        try
        {
            var cart = await shoppingCartService.GetByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound("Shopping cart not found for this user");
            }

            return Ok(cart);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    // these 2 methods are not needed
    // [HttpPost]
    // public async Task<IActionResult> AddAsync(AddShoppingCartDto addShoppingCartDto)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }
    //     try
    //     {
    //         await shoppingCartService.AddAsync(addShoppingCartDto);
    //         return Created();
    //     }
    //     catch (InvalidOperationException ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         return StatusCode(500, "Internal server error");
    //     }
    // }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteAsync(string id)
    // {
    //     try
    //     {
    //         await shoppingCartService.DeleteAsync(id);
    //         return NoContent();
    //     }
    //     catch (KeyNotFoundException ex)
    //     {
    //         return NotFound(ex.Message);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         return StatusCode(500, "Internal server error");
    //     }
    // }
}
