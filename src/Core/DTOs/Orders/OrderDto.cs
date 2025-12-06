using Core.DTOs.OrderItems;
using Core.Entities;

namespace Core.DTOs.Orders
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public Address Address { get; set; }

        public ICollection<OrderItemsDto> OrderItems { get; set; } = new List<OrderItemsDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
