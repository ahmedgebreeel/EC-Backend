using Core.DTOs.CartItem;
using System.ComponentModel.DataAnnotations;


namespace Core.DTOs.ShoppingCart
{
    public class AddShoppingCartDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }

        public List<AddCartItemDto> CartItems { get; set; } = new List<AddCartItemDto>();
    }
}
