using Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DTOs.Orders
{
    public class UpdateOrderDto
    {
        [Required]
        public string AddressId { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
    }
}
