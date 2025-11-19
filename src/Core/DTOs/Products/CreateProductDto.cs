using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Products
{
    public class CreateProductDto
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
        public int Stock { get; set; } = 0;
        [Required]
        public string SellerId { get; set; }

        public ICollection<CreateProductsImagesDto> Images { get; set; }

    }
    public class UpdateProductDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
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
        public int Stock { get; set; } = 0;
        [Required]
        public string SellerId { get; set; }

        public ICollection<CreateProductsImagesDto> Images { get; set; }
    }
}
