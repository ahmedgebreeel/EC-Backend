

using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.CartItem
{
    public class UpdateCartItemDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
