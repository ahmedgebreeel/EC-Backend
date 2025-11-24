using System.ComponentModel.DataAnnotations;


namespace Core.DTOs.CartItem
{
    public class AddCartItemDto
    {
        [Required]
        public string CartId { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
