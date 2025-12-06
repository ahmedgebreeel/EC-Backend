
namespace Core.DTOs.Category
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ParentCategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
    }
}