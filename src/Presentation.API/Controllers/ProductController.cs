using Business.Services;
using Core.DTOs.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService) { 
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            try
            {
                var produts = await _productService.GetAllAsync();
                return Ok(produts);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id);
                if (product == null) return NotFound();
                return Ok(product);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]CreateProductDto productDto)
        {
            if(!ModelState.IsValid) return BadRequest();
            try
            {
                await _productService.CreateProductAsync(productDto);
                return Created();
            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]UpdateProductDto productDto)
        {
            try
            {
                var exist = await _productService.GetProductAsync(productDto.Id);
                if (exist == null) return NotFound();
                if (!ModelState.IsValid) return BadRequest();
                await _productService.UpdateProductAsync(productDto);
                return Ok(new { message = "Updated Successfully" });
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var exist = await _productService.GetProductAsync(id);
                if (exist == null) return NotFound();
                return Ok(new { message = "Deleted Successfully" });
            }
            catch
            {
                return BadRequest();
            }

        }

    }
}
