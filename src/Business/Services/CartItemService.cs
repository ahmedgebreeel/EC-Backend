using Core.DTOs.CartItem;
using Core.DTOs.Products;
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

            var cartItemDtos = items.Select(item => new CartItemDto
            {
                Id = item.Id,
                CartId = item.CartId,
                product = new ProductDto
                {
                    Id = item.Product.Id,
                    Name = item.Product.Name,
                    Description = item.Product.Description,
                    CategoryId = item.Product.CategoryId,
                    Price = item.Product.Price,
                    Stock = item.Product.Stock,
                    Image = item.Product.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                    CreatedAt = item.Product.CreatedAt,
                    UpdatedAt = item.Product.UpdatedAt
                },
                Quantity = item.Quantity,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            });

            return cartItemDtos;
        }

        public async Task<CartItemDto?> GetByIdAsync(string id)
        {
            var cartItem = await _unitOfWork.Repository<CartItem>().GetByIdAsync(id);
            if (cartItem == null)
            {
                return null;
            }

            var cartItemDto = new CartItemDto
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                product = new ProductDto
                {
                    Id = cartItem.Product.Id,
                    Name = cartItem.Product.Name,
                    Description = cartItem.Product.Description,
                    CategoryId = cartItem.Product.CategoryId,
                    Price = cartItem.Product.Price,
                    Stock = cartItem.Product.Stock,
                    Image = cartItem.Product.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                    CreatedAt = cartItem.Product.CreatedAt,
                    UpdatedAt = cartItem.Product.UpdatedAt
                },
                Quantity = cartItem.Quantity,
                CreatedAt = cartItem.CreatedAt,
                UpdatedAt = cartItem.UpdatedAt
            };
            return cartItemDto;
        }

        public async Task AddAsync(AddCartItemDto addCartItemDto)
        {
            // Check if this CartId + ProductId combination already exists
            var existingCartItem = await _unitOfWork.Repository<CartItem>()
                .FindAsync(ci => ci.CartId == addCartItemDto.CartId && ci.ProductId == addCartItemDto.ProductId);

            if (existingCartItem.Any())
            {
                // Item already exists - increment quantity
                var item = existingCartItem.First();
                item.Quantity += addCartItemDto.Quantity;
                item.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Repository<CartItem>().Update(item);
                await _unitOfWork.SaveChangesAsync();
                return;
            }

            // Create new cart item
            await _unitOfWork.Repository<CartItem>().AddAsync(new CartItem
            {
                CartId = addCartItemDto.CartId,
                ProductId = addCartItemDto.ProductId,
                Quantity = addCartItemDto.Quantity,
            });
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, UpdateCartItemDto updateCartItemDto)
        {
            var cartItem = await _unitOfWork.Repository<CartItem>().GetByIdAsync(id);
            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found");
            }

            cartItem.Quantity = updateCartItemDto.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<CartItem>().Update(cartItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var cartItem = await _unitOfWork.Repository<CartItem>().GetByIdAsync(id);
            if (cartItem == null)
            {
                throw new KeyNotFoundException("Cart item not found");
            }

            _unitOfWork.Repository<CartItem>().Delete(cartItem);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
