using Core.DTOs.CartItem;
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

        public async Task<IEnumerable<ShoppingCartDto>> GetAllCartsAsync()
        {
            var carts = await _unitOfWork.Repository<ShoppingCart>().GetAllAsync();
            var shoppingCarts = carts.Select(c => new ShoppingCartDto
            {
                Id = c.Id,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                CartItems = new List<CartItemDto>() // Populate cart items as needed
            });

            return shoppingCarts;
        }

        public async Task<ShoppingCartDto> GetCartByIdAsync(string id)
        {
            var Shoppingcart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(id);
            if (Shoppingcart == null) return null;

            // Load CartItems here if needed

            return new ShoppingCartDto
            {
                Id = Shoppingcart.Id,
                UserId = Shoppingcart.UserId,
                CreatedAt = Shoppingcart.CreatedAt,
                UpdatedAt = Shoppingcart.UpdatedAt,
                CartItems = new List<CartItemDto>() // Populate cart items as needed
            };
        }

        public async Task<ShoppingCartDto> AddCartAsync(ShoppingCartDto ShoppingcartDto)
        {
            if (ShoppingcartDto == null)
                throw new ArgumentNullException(nameof(ShoppingcartDto));

            var cart = new ShoppingCart
            {
                UserId = ShoppingcartDto.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<ShoppingCart>().AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            ShoppingcartDto.Id = cart.Id;
            ShoppingcartDto.CreatedAt = cart.CreatedAt;
            ShoppingcartDto.UpdatedAt = cart.UpdatedAt;
            ShoppingcartDto.CartItems = new List<CartItemDto>();

            return ShoppingcartDto;
        }

        public async Task DeleteCartAsync(string id)
        {
            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(id);
            if (cart == null)
                throw new KeyNotFoundException("Shopping cart not found");

            _unitOfWork.Repository<ShoppingCart>().Delete(cart);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
