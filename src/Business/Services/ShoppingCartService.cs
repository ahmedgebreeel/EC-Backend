using Core.DTOs.ShoppingCart;
using Core.Entities;
using Data.Repositories;

namespace Business.Services
{
    public class ShoppingCartService
    {
        private readonly UnitOfWork _unitOfWork;

        public ShoppingCartService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ShoppingCartDto>> GetAllAsync()
        {
            var carts = await _unitOfWork.Repository<ShoppingCart>().GetAllAsync();

            var shoppingCartDtos = carts.Select(cart => new ShoppingCartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cart.CartItems?.Select(ci => new Core.DTOs.CartItem.CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt
                }).ToList() ?? new List<Core.DTOs.CartItem.CartItemDto>()
            });

            return shoppingCartDtos;
        }

        public async Task<ShoppingCartDto?> GetByIdAsync(string id)
        {
            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(id);
            if (cart == null)
            {
                return null;
            }

            var shoppingCartDto = new ShoppingCartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cart.CartItems?.Select(ci => new Core.DTOs.CartItem.CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt
                }).ToList() ?? new List<Core.DTOs.CartItem.CartItemDto>()
            };
            return shoppingCartDto;
        }

        public async Task<ShoppingCartDto?> GetByUserIdAsync(string userId)
        {
            var carts = await _unitOfWork.Repository<ShoppingCart>().GetAllAsync();
            var userCart = carts.FirstOrDefault(c => c.UserId == userId);

            if (userCart == null)
            {
                return null;
            }

            var shoppingCartDto = new ShoppingCartDto
            {
                Id = userCart.Id,
                UserId = userCart.UserId,
                CreatedAt = userCart.CreatedAt,
                UpdatedAt = userCart.UpdatedAt,
                CartItems = userCart.CartItems?.Select(ci => new Core.DTOs.CartItem.CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt
                }).ToList() ?? new List<Core.DTOs.CartItem.CartItemDto>()
            };
            return shoppingCartDto;
        }

        public async Task AddAsync(AddShoppingCartDto addShoppingCartDto)
        {
            // Check if user already has a cart
            var existingCart = await _unitOfWork.Repository<ShoppingCart>()
                .FindAsync(c => c.UserId == addShoppingCartDto.UserId);

            if (existingCart.Any())
            {
                throw new InvalidOperationException("User already has a shopping cart.");
            }

            await _unitOfWork.Repository<ShoppingCart>().AddAsync(new ShoppingCart
            {
                UserId = addShoppingCartDto.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CartItems = new List<CartItem>()
            });
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(id);
            if (cart == null)
            {
                throw new KeyNotFoundException("Shopping cart not found");
            }

            _unitOfWork.Repository<ShoppingCart>().Delete(cart);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
