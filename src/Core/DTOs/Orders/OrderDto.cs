using Core.DTOs.OrderItems;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
