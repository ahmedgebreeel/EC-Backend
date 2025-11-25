using AutoMapper;
using Core.DTOs.ShoppingCart;
using Core.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ShoppingCartService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShoppingCartService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        //Cancellation Token to be used later to handle CPU intensive tasks or long running tasks
        public async Task<IEnumerable<ShoppingCartDto>> GetAllCartsAsync(CancellationToken cancellationToken = default)
        {
            // adding pagination for production use later
            var carts = await _unitOfWork.Repository<ShoppingCart>().GetAllAsync();
            return _mapper.Map<List<ShoppingCartDto>>(carts);
        }

        public async Task<ShoppingCartDto?> GetCartByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Shopping cart ID cannot be null or empty", nameof(id));

            // Eager Loading CartItems
            var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(id);

            if (shoppingCart == null)
                return null;

            return _mapper.Map<ShoppingCartDto>(shoppingCart);
        }
        //Still need to handle cancellation token in long running tasks
        public async Task<ShoppingCartDto> GetCartByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

            var carts = await _unitOfWork.Repository<ShoppingCart>().GetAllAsync();
            var userCart = carts.FirstOrDefault(c => c.UserId == userId);

            if (userCart == null)
            {
                // Create new cart for user if doesn't exist
                return await CreateCartForUserAsync(userId);
            }

            return _mapper.Map<ShoppingCartDto>(userCart);
        }

        public async Task<ShoppingCartDto> AddCartAsync(AddShoppingCartDto addShoppingCartDto, CancellationToken cancellationToken = default)
        {
            if (addShoppingCartDto == null)
                throw new ArgumentNullException(nameof(addShoppingCartDto));

            if (string.IsNullOrWhiteSpace(addShoppingCartDto.UserId))
                throw new ArgumentException("User ID cannot be null or empty", nameof(addShoppingCartDto.UserId));

            // Check if user already has a cart
            var carts = await _unitOfWork.Repository<ShoppingCart>().GetAllAsync();
            var existingCart = carts.FirstOrDefault(c => c.UserId == addShoppingCartDto.UserId);

            if (existingCart != null)
            {
                return _mapper.Map<ShoppingCartDto>(existingCart);
            }

            var cart = _mapper.Map<ShoppingCart>(addShoppingCartDto);
            cart.CreatedAt = DateTime.UtcNow;
            cart.UpdatedAt = DateTime.UtcNow;
            cart.CartItems = new List<CartItem>(); // Initialize empty collection

            await _unitOfWork.Repository<ShoppingCart>().AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ShoppingCartDto>(cart);
        }

        public async Task DeleteCartAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Shopping cart ID cannot be null or empty", nameof(id));

            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(id);
            if (cart == null)
                throw new KeyNotFoundException($"Shopping cart with ID '{id}' not found");

            _unitOfWork.Repository<ShoppingCart>().Delete(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<ShoppingCartDto> CreateCartForUserAsync(string userId)
        {
            var cart = new ShoppingCart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CartItems = new List<CartItem>()
            };

            await _unitOfWork.Repository<ShoppingCart>().AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ShoppingCartDto>(cart);
        }
    }
}
