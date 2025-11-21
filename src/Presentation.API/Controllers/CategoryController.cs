using Business.Services;
using Core.DTOs.Category;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController: ControllerBase
{
    private readonly CategoryService categoryService;
    public CategoryController(CategoryService _categoryService)
    {
        categoryService = _categoryService;
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
        Console.WriteLine(ex);
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
        Console.WriteLine(ex);
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
        return BadRequest(ex.Message);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
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
        await categoryService.DeleteAsync(id);
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