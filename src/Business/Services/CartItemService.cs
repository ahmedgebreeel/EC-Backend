using Core.DTOs.CartItem;
using Core.Entities;
using Data.Repositories;

namespace Business.Services
{
    public class CartItemService
    {
        private readonly UnitOfWork _unitOfWork;

        public CartItemService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CartItemDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<CartItem>().GetAllAsync();

            var CartItemDTOs = items.Select(i => new CartItemDto
            {
                Id = i.Id,
                CartId = i.CartId,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            });

            return CartItemDTOs;
        }

        public async Task<CartItemDto?> GetCartItemByIdAsync(string id)
        {
            var cartitem = await _unitOfWork.Repository<CartItem>().GetByIdAsync(id);
            if (cartitem == null) return null;

            return new CartItemDto
            {
                Id = cartitem.Id,
                CartId = cartitem.CartId,
                ProductId = cartitem.ProductId,
                Quantity = cartitem.Quantity,
                CreatedAt = cartitem.CreatedAt,
                UpdatedAt = cartitem.UpdatedAt
            };
        }

        public async Task<CartItemDto> AddCartItemAsync(AddCartItemDto addCartItemDto)
        {
            if (addCartItemDto == null)
                throw new ArgumentNullException(nameof(addCartItemDto));

            // Validate cart exists
            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(addCartItemDto.CartId);
            if (cart == null)
                throw new ArgumentException("Cart does not exist");

            // Validate product exists before adding
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(addCartItemDto.ProductId);
            if (product == null)
                throw new ArgumentException("Product does not exist");

            if (addCartItemDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var cartitem = new CartItem
            {
                CartId = addCartItemDto.CartId,
                ProductId = addCartItemDto.ProductId,
                Quantity = addCartItemDto.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<CartItem>().AddAsync(cartitem);
            await _unitOfWork.SaveChangesAsync();

            return new CartItemDto
            {
                Id = cartitem.Id,
                CartId = cartitem.CartId,
                ProductId = cartitem.ProductId,
                Quantity = cartitem.Quantity,
                CreatedAt = cartitem.CreatedAt,
                UpdatedAt = cartitem.UpdatedAt
            };
        }

        public async Task UpdateCartItemAsync(string id, UpdateCartItemDto updateCartItemDto)
        {
            if (updateCartItemDto == null)
                throw new ArgumentNullException(nameof(updateCartItemDto));

            var item = await _unitOfWork.Repository<CartItem>().GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Cart item not found");

            if (updateCartItemDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be positive");

            item.Quantity = updateCartItemDto.Quantity;
            item.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<CartItem>().Update(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(string id)
        {
            var item = await _unitOfWork.Repository<CartItem>().GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Cart item not found");

            _unitOfWork.Repository<CartItem>().Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
