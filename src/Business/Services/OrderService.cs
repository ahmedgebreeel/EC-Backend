using Core.DTOs.OrderItems;
using Core.DTOs.Orders;
using Core.Entities;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class OrderService
    {
        UnitOfWork unitOfWork;
        
        public OrderService(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = await unitOfWork.Orders.GetAllAsync();
            var orderDto = orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                UserId = o.UserId,
                UserName = o.User.Name,
                Status = o.Status,
                Total = o.Total,
                AddressId = o.AddressId,
                Address = o.Address,
                OrderItems = o.OrderItems.Select(oi => new OrderItemsDto
                {
                    Id = oi.Id,
                    OrderId = oi.Id,
                    ProductId = oi.ProductId,
                    Product = oi.Product,   
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                }).ToList()
            }).ToList();
            return orderDto;
        }

        public async Task<OrderDTO?> GetByIdAsync(string id)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(id);
            if(order == null)
            {
                return null;
            }
            var orderDto = new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User.Name,
                Status = order.Status,
                Total = order.Total,
                AddressId = order.AddressId,
                Address = order.Address,
                OrderItems = order.OrderItems.Select(oi => new OrderItemsDto
                {
                    Id = oi.Id,
                    OrderId = oi.Id,
                    ProductId = oi.ProductId,
                    Product = oi.Product,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                }).ToList()

            };
            return orderDto;
        }

        public async Task<OrderDTO?> GetByUserIdAsync(string userId)
        {
            var order = await unitOfWork.Orders.GetByUserIdAsync(userId);
            if (order == null)
            {
                return null;
            }
            var orderDto = new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User.Name,
                Status = order.Status,
                Total = order.Total,
                AddressId = order.AddressId,
                Address = order.Address,
                OrderItems = order.OrderItems.Select(oi => new OrderItemsDto
                {
                    Id = oi.Id,
                    OrderId = oi.Id,
                    ProductId = oi.ProductId,
                    Product = oi.Product,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                }).ToList()

            };
            return orderDto;

        }
    }
}
