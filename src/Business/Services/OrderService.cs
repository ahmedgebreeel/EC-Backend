using Core.DTOs.OrderItems;
using Core.DTOs.Orders;
using Core.Entities;
using Data.Repositories;

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
                Address = o.Address,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,
                OrderItems = o.OrderItems.Select(oi => new OrderItemsDto
                {
                    Id = oi.Id,
                    OrderId = oi.Id,
                    Product = oi.Product,   
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                    CreatedAt = oi.CreatedAt,
                    UpdatedAt = oi.UpdatedAt
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
                Address = order.Address,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems.Select(oi => new OrderItemsDto
                {
                    Id = oi.Id,
                    OrderId = oi.Id,
                    Product = oi.Product,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                    CreatedAt = oi.CreatedAt,
                    UpdatedAt = oi.UpdatedAt
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
                Address = order.Address,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems.Select(oi => new OrderItemsDto
                {
                    Id = oi.Id,
                    OrderId = oi.Id,
                    Product = oi.Product,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                    CreatedAt = oi.CreatedAt,
                    UpdatedAt = oi.UpdatedAt
                }).ToList()

            };
            return orderDto;

        }
    
        
        public async Task AddAsync(AddOrderDto order)
        {
            var exisitingUser = await unitOfWork.Users.GetByIdAsync(order.UserId);
            if (exisitingUser == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }
            var existingAddress = await unitOfWork.Repository<Address>().GetByIdAsync(order.AddressId);

            if (existingAddress == null)
            {
                throw new InvalidOperationException("Address does not exist.");
            }

            var newOrder = new Order
            {
                UserId = order.UserId,
                Status = order.Status.ToString(),
                Total = order.Total,
                AddressId = order.AddressId,
            };

            await unitOfWork.Orders.AddAsync(newOrder );
            foreach (var item in order.OrderItems)
            {
                await unitOfWork.Repository<OrderItem>().AddAsync(
                    new OrderItem
                    {
                        OrderId = newOrder.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }
                    );
            }
            await unitOfWork.SaveChangesAsync();
        }




    }
}
