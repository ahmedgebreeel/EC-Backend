using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.OrderItems
{
    public class UpdateOrderItemDto
    {
        [Required]
        public int Quantity { get; set; } = 1;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
