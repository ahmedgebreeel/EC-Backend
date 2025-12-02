using Business.Services;
using Core.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController: ControllerBase
{
    private readonly CategoryService categoryService;
    private readonly ILogger<CategoryController> logger;
    public CategoryController(CategoryService _categoryService, ILogger<CategoryController> _logger)
    {
        categoryService = _categoryService;
        logger = _logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      try
      {
        var categories =  await categoryService.GetAllAsync();
        return Ok(categories);
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
        var category =  await categoryService.GetByIdAsync(id);
        if(category == null)
        {
            return NotFound("Category not found");
        }
        
        return Ok(category);
      }
      catch (Exception ex)
      {
        logger.LogError(ex.Message);
        return StatusCode(500, "Internal server error");
      }   
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(AddCategoryDto addCategoryDto)
    {
      if(!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      try
      {
        await categoryService.AddAsync(addCategoryDto);
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
    public async Task<IActionResult> UpdateAsync(string id, UpdateCategoryDto updateCategoryDto)
    {
      if(!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      try
      {
        await categoryService.UpdateAsync(id, updateCategoryDto);
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
        await categoryService.DeleteAsync(id);
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