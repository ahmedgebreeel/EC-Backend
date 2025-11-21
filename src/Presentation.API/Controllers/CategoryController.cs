using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController: ControllerBase
{
    private readonly CategoryService _categoryService;
    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult GetCategories()
    {
      try
      {
        return Ok();
      }
      catch (System.Exception ex)
      {
        System.Console.WriteLine(ex);
        return StatusCode(500, "Internal server error");
      }   
    }
}