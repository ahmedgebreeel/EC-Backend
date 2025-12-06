using Core.Entities;

namespace Core.DTOs.OrderItems
{
    public class OrderItemsDto
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
