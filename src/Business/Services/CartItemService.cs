using AutoMapper;
using Core.DTOs.CartItem;
using Core.Entities;
using Data.Repositories;

namespace Business.Services
{
    public class CartItemService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartItemService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CartItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var items = await _unitOfWork.Repository<CartItem>().GetAllAsync();
            return _mapper.Map<List<CartItemDto>>(items);
        }

        public async Task<CartItemDto?> GetCartItemByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Cart item ID cannot be null or empty", nameof(id));

            var cartItem = await _unitOfWork.Repository<CartItem>().GetByIdAsync(id);
            return cartItem == null ? null : _mapper.Map<CartItemDto>(cartItem);
        }

        public async Task<CartItemDto> AddCartItemAsync(AddCartItemDto addCartItemDto, CancellationToken cancellationToken = default)
        {
            if (addCartItemDto == null)
                throw new ArgumentNullException(nameof(addCartItemDto));

            if (string.IsNullOrWhiteSpace(addCartItemDto.CartId))
                throw new ArgumentException("Cart ID cannot be null or empty", nameof(addCartItemDto.CartId));

            if (string.IsNullOrWhiteSpace(addCartItemDto.ProductId))
                throw new ArgumentException("Product ID cannot be null or empty", nameof(addCartItemDto.ProductId));

            if (addCartItemDto.Quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(addCartItemDto.Quantity), "Quantity must be greater than zero");

            // Validate cart exists
            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(addCartItemDto.CartId);
            if (cart == null)
                throw new KeyNotFoundException($"Shopping cart with ID '{addCartItemDto.CartId}' not found");

            // Validate product exists
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(addCartItemDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID '{addCartItemDto.ProductId}' not found");

            // Check if this exact CartItem ID already exists (user clicked add again)
            if (!string.IsNullOrWhiteSpace(addCartItemDto.Id))
            {
                var existingItemById = await _unitOfWork.Repository<CartItem>().GetByIdAsync(addCartItemDto.Id);
                if (existingItemById != null)
                {
                    // CartItem exists - just increment quantity
                    existingItemById.Quantity += addCartItemDto.Quantity;
                    existingItemById.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.Repository<CartItem>().Update(existingItemById);
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<CartItemDto>(existingItemById);
                }
            }

            // Create new cart item
            var cartItem = _mapper.Map<CartItem>(addCartItemDto);
            cartItem.CreatedAt = DateTime.UtcNow;
            cartItem.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<CartItem>().AddAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CartItemDto>(cartItem);
        }

        public async Task UpdateCartItemAsync(string id, UpdateCartItemDto updateCartItemDto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Cart item ID cannot be null or empty", nameof(id));

            if (updateCartItemDto == null)
                throw new ArgumentNullException(nameof(updateCartItemDto));

            if (updateCartItemDto.Quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(updateCartItemDto.Quantity), "Quantity must be greater than zero");

            var item = await _unitOfWork.Repository<CartItem>().GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Cart item with ID '{id}' not found");

            item.Quantity = updateCartItemDto.Quantity;
            item.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<CartItem>().Update(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Cart item ID cannot be null or empty", nameof(id));

            var item = await _unitOfWork.Repository<CartItem>().GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Cart item with ID '{id}' not found");

            _unitOfWork.Repository<CartItem>().Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
