using System.ComponentModel.DataAnnotations;
using Core.DTOs.CartItem;

namespace Core.DTOs.ShoppingCart
{
    public class ShoppingCartDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }

        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
