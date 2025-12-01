using Core.DTOs.OrderItems;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DTOs.Orders
{
    public class AddOrderDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AddressId { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        public ICollection<_orderItems> OrderItems { get; set; }
    }
    public record _orderItems
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public int Quantity { get; set; } = 1;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }


}

