

using Core.Entities;

namespace Core.DTOs.CartItem
{
    public class CartItemDto
    {
        public string Id { get; set; }
        public string CartId { get; set; }
        public Product product { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
