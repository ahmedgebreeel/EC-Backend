using Core.DTOs.OrderItems;
using Core.Entities;
using Data.Repositories;

namespace Business.Services
{
    public class OrderItemService
    {
        private readonly UnitOfWork unitOfWork;
        public OrderItemService(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task AddAsync(AddOrderItemsDto addOrderItemDto)
        {
            var existingOrder = await unitOfWork.Orders.GetByIdAsync(addOrderItemDto.OrderId);
            if (existingOrder == null) { 
                throw new InvalidOperationException("Order does not exist.");
            }
            var existingProduct = await unitOfWork.Products.GetByIdAsync(addOrderItemDto.ProductId);
            if (existingProduct == null) {
                throw new InvalidOperationException("Product does not exist.");
            }
            var existingOrderItem = await unitOfWork.Repository<OrderItem>()
                .FindAsync(o=>o.OrderId==addOrderItemDto.OrderId && o.ProductId==addOrderItemDto.ProductId);
           
            var item = existingOrderItem.FirstOrDefault();
            if (item != null)
            {
                item.Quantity += addOrderItemDto.Quantity;
                item.UpdatedAt = DateTime.UtcNow;
                unitOfWork.Repository<OrderItem>().Update(item);
                await unitOfWork.SaveChangesAsync();
                return;
            }

            var newOrderItem = new OrderItem
            {
                OrderId = addOrderItemDto.OrderId,
                ProductId = addOrderItemDto.ProductId,
                Price = existingProduct.Price,
                Quantity = addOrderItemDto.Quantity,

            };
            await unitOfWork.Repository<OrderItem>().AddAsync(newOrderItem);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, UpdateOrderItemDto updateOrderItemDto)
        {
            var existingOrderItem = await unitOfWork.Repository<OrderItem>().GetByIdAsync(id);
            if (existingOrderItem == null) {
                throw new InvalidOperationException("Order item does not exist.");
            }
            existingOrderItem.Quantity = updateOrderItemDto.Quantity;
            existingOrderItem.Price = updateOrderItemDto.Price;
            existingOrderItem.UpdatedAt = DateTime.UtcNow;

            unitOfWork.Repository<OrderItem>().Update(existingOrderItem);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var orderItem = await unitOfWork.Repository<OrderItem>().GetByIdAsync(id);
            if (orderItem == null) {
                throw new InvalidOperationException("Order item does not exist.");
            }
            unitOfWork.Repository<OrderItem>().Delete(orderItem);
            await unitOfWork.SaveChangesAsync();
        }


    }
}
