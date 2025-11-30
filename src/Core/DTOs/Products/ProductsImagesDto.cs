using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Products
{
    public class ProductsImagesDto
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public int Position { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


    }

    public class CreateProductsImagesDto
    {
        [Required]
        [MaxLength(1000)]
        public string ImageUrl { get; set; }
        [Required]
        public int Position { get; set; }
        [Required]
        public string ProductId { get; set; }

    }

}
