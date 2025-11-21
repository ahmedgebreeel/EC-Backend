using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Category;
public class AddCategoryDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; }

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    public string? ParentCategoryId { get; set; }
} 