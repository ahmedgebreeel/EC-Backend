using System.ComponentModel.DataAnnotations;
using Core.DTOs.CartItem;

namespace Core.DTOs.ShoppingCart
{
    public class ShoppingCartDto
    {
        [StringLength(40, ErrorMessage = "Id cannot be longer than 40 characters.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "UserId must be between 1 and 40 characters.")]
        public string UserId { get; set; }

        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
