using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;
public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    // Self-referencing foreign key for hierarchical structure
    public string? ParentCategoryId { get; set; }

    // Navigation property for parent category
    [ForeignKey("ParentCategoryId")]
    public virtual Category? ParentCategory { get; set; }

    // Navigation property for child categories
    public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();

    // Navigation property for products in this category
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}