
using Core.DTOs.OrderItems;
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


        }


    }
}
