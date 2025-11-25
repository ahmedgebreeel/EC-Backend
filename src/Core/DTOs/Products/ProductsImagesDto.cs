using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Products
{
    public class ProductsImagesDto
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public int Position { get; set; } = 0; // For ordering images (1st, 2nd, 3rd, etc.)


    }

    public class CreateProductsImagesDto
    {
        [Required]
        [MaxLength(1000)]
        public string ImageUrl { get; set; }
        [Required]
        public int Position { get; set; } = 0; // For ordering images (1st, 2nd, 3rd, etc.)
        [Required]
        public string ProductId { get; set; }

    }

}
