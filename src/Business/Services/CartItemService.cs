using Core.DTOs.CartItem;
using Core.Entities;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Services
{
    public class CartItemService
    {
        private readonly UnitOfWork _unitOfWork;

        public CartItemService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CartItemDTO> AddCartItemAsync(CartItemDTO cartItemDTO)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(cartItemDTO.ProductId);
            if (product == null)
                throw new ArgumentException("Product does not exist");

            if (cartItemDTO.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");
            var item = new CartItem
            {
                CartId = cartItemDTO.CartId,
                ProductId = cartItemDTO.ProductId,
                Quantity = cartItemDTO.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<CartItem>().AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            cartItemDTO.Id = item.Id;
            cartItemDTO.CreatedAt = item.CreatedAt;
            cartItemDTO.UpdatedAt = item.UpdatedAt;

            return cartItemDTO;
        }

        // Update cart item DTO quantity
        public async Task UpdateCartItemAsync(CartItemDTO cartItemDTO)
        {
            var item = await _unitOfWork.Repository<CartItem>().GetByIdAsync(cartItemDTO.Id);
            if (item == null) throw new ArgumentException("Cart item not found");

            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(item.CartId);
            if (cart == null)
                throw new ArgumentException("Cart not found");

            if (cartItemDTO.Quantity <= 0)
                throw new ArgumentException("Quantity must be positive");

            item.Quantity = cartItemDTO.Quantity;
            item.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<CartItem>().Update(item);
            await _unitOfWork.SaveChangesAsync();
        }

        // Delete cart item by Id
        public async Task DeleteCartItemAsync(string cartItemId)
        {
            var item = await _unitOfWork.Repository<CartItem>().GetByIdAsync(cartItemId);
            if (item == null) return;

            var cart = await _unitOfWork.Repository<ShoppingCart>().GetByIdAsync(item.CartId);
            if (cart == null)
                throw new ArgumentException("Cart not found");

            _unitOfWork.Repository<CartItem>().Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
