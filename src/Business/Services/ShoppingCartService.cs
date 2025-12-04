using Core.DTOs.CartItem;
using Core.DTOs.Products;
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
            var carts = await _unitOfWork.Cart.GetAllAsync();

            var shoppingCartDtos = carts.Select(cart => new ShoppingCartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cart.CartItems?.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    product = new ProductDto
                    {
                        Id = ci.Product.Id,
                        Name = ci.Product.Name,
                        Description = ci.Product.Description,
                        CategoryId = ci.Product.CategoryId,
                        Price = ci.Product.Price,
                        Stock = ci.Product.Stock,
                        Image = ci.Product.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                        CreatedAt = ci.Product.CreatedAt,
                        UpdatedAt = ci.Product.UpdatedAt
                    },
                    Quantity = ci.Quantity,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt
                }).ToList() ?? new List<CartItemDto>()
            });

            return shoppingCartDtos;
        }

        public async Task<ShoppingCartDto?> GetByIdAsync(string id)
        {
            var cart = await _unitOfWork.Cart.GetByIdAsync(id);
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
                CartItems = cart.CartItems?.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    product = new ProductDto
                    {
                        Id = ci.Product.Id,
                        Name = ci.Product.Name,
                        Description = ci.Product.Description,
                        CategoryId = ci.Product.CategoryId,
                        Price = ci.Product.Price,
                        Stock = ci.Product.Stock,
                        Image = ci.Product.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                        CreatedAt = ci.Product.CreatedAt,
                        UpdatedAt = ci.Product.UpdatedAt
                    },
                    Quantity = ci.Quantity,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt
                }).ToList() ?? new List<CartItemDto>()
            };
            return shoppingCartDto;
        }

        public async Task<ShoppingCartDto?> GetByUserIdAsync(string userId)
        {
            var userCart = await _unitOfWork.Cart.GetByUserIdAsync(userId);

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
                CartItems = userCart.CartItems?.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    product = new ProductDto
                    {
                        Id = ci.Product.Id,
                        Name = ci.Product.Name,
                        Description = ci.Product.Description,
                        CategoryId = ci.Product.CategoryId,
                        Price = ci.Product.Price,
                        Stock = ci.Product.Stock,
                        Image = ci.Product.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                        CreatedAt = ci.Product.CreatedAt,
                        UpdatedAt = ci.Product.UpdatedAt
                    },
                    Quantity = ci.Quantity,
                    CreatedAt = ci.CreatedAt,
                    UpdatedAt = ci.UpdatedAt
                }).ToList() ?? new List<CartItemDto>()
            };
            return shoppingCartDto;
        }

        public async Task AddAsync(AddShoppingCartDto addShoppingCartDto)
        {
            // Check if user already has a cart
            var existingCart = await _unitOfWork.Cart
                .FindAsync(c => c.UserId == addShoppingCartDto.UserId);

            if (existingCart.Any())
            {
                throw new InvalidOperationException("User already has a shopping cart.");
            }

            await _unitOfWork.Cart.AddAsync(new ShoppingCart
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
            var cart = await _unitOfWork.Cart.GetByIdAsync(id);
            if (cart == null)
            {
                throw new KeyNotFoundException("Shopping cart not found");
            }

            _unitOfWork.Cart.Delete(cart);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
