using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DTOs.OrderItems
{
    public class AddOrderItemsDto
    {
        [Required]
        public string OrderId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public int Quantity { get; set; } = 1;

    }

}
