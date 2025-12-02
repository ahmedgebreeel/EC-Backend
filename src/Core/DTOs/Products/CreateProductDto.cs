using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Core.DTOs.Products
{
    public class AddProductDto
    {
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public string CategoryId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public string SellerId { get; set; }

        public ICollection<IFormFile> Images { get; set; }

    }
    public class UpdateProductDto
    {
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public string CategoryId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public string SellerId { get; set; }

        public ICollection<IFormFile> Images { get; set; }
    }
}
