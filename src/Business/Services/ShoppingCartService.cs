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

        // Get all carts (with DTO)
        public async Task<List<ShoppingCartDTO>> GetAllCartsAsync()
        {
            // Placeholder for future logic
            var carts = await _unitOfWork.Repository<ShoppingCart>().GetAllAsync();

            // Map entities to DTOs (just basic props, extend later)
            return carts.Select(c => new ShoppingCartDTO
            {
                Id = c.Id,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                CartItems = new List<CartItemDTO>() // Load as needed later
            }).ToList();
        }

        // Get cart by Id (with DTO)
        public async Task<ShoppingCartDTO?> GetCartByIdAsync(string cartId)
        {
            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(cartId);
            if (cart == null) return null;
            // Need to check on user once auth is in place
            // Load CartItems and Map to DTOs later as needed
            return new ShoppingCartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = new List<CartItemDTO>() // populate later
            };
        }

        // Add new cart with DTO
        public async Task<ShoppingCartDTO> AddCartAsync(ShoppingCartDTO cartDTO)
        {
            var cart = new ShoppingCart
            {
                UserId = cartDTO.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
                // Id will be auto-generated
            };

            await _unitOfWork.Repository<ShoppingCart>().AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            cartDTO.Id = cart.Id;
            cartDTO.CreatedAt = cart.CreatedAt;
            cartDTO.UpdatedAt = cart.UpdatedAt;
            cartDTO.CartItems = new List<CartItemDTO>();

            return cartDTO;
        }

        // Update cart (e.g., update timestamps or userId)
        public async Task UpdateCartAsync(ShoppingCartDTO cartDTO)
        {
            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(cartDTO.Id);
            if (cart == null) throw new ArgumentException("Cart not found");

            cart.UserId = cartDTO.UserId;
            cart.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<ShoppingCart>().Update(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        // Remove cart by Id
        public async Task DeleteCartAsync(string cartId)
        {
            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(cartId);
            if (cart == null) return;

            _unitOfWork.Repository<ShoppingCart>().Delete(cart);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
