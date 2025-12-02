using Business.Services;
using Core.DTOs.Products;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;
        public ProductController(ProductService _productService) { 
            productService = _productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]int? pageNum , [FromQuery] int? pageSize) {
            try
            {
                var produts = await productService.GetAllAsync(pageNum,pageSize);
                return Ok(produts);
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
                var product = await productService.GetByIdAsync(id);
                if (product == null)
                {
                  return NotFound("Product not found");  
                } 
                return Ok(product);
            }
            catch (Exception ex)
            {   
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] AddProductDto productDto)
        {
            if(!ModelState.IsValid) return BadRequest();
            try
            {
                await productService.AddAsync(productDto);
                return Created();
            }
            catch (Exception ex) when (ex is KeyNotFoundException or InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error" + ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(string id, [FromForm] UpdateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await productService.UpdateAsync(id, productDto);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                await productService.DeleteAsync(id);
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
