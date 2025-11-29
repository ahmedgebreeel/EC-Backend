using AutoMapper;
using Core.DTOs.Products;
using Core.Entities;
using Core.Enums;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(UnitOfWork _unitOfWork, IMapper mapper)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(int pageNum = 1, int pageSize = 10)
        {
            const int maxPageSize = 50;
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var products = await unitOfWork
                .Products
                .GetAllAsync(pageNum,pageSize);

            return _mapper.Map<List<ProductDto>>(products);
                
        }


        public async Task<ProductDetailsDto?> GetByIdAsync(string id)
        {
            var product =  await unitOfWork
               .Products
               .GetByIdAsync(id);

            if (product == null) return null;

            return _mapper.Map<ProductDetailsDto?>(product);
        }

       public async Task AddAsync(CreateProductDto product)
        {
            //check if category is exist
            var category = await unitOfWork.Repository<Category>()
                .GetByIdAsync(product.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            var seller = await unitOfWork.Users.GetByIdAsync(product.SellerId);
            if (seller == null) throw new KeyNotFoundException("Seller not found");

            //make sure its role is a seller
            if(seller.Role.Name != RoleType.Seller.ToString() && seller.Role.Name != RoleType.Admin.ToString()) throw new InvalidOperationException("User is not a seller");

            //ToDo: saving the images !!
            await unitOfWork.Products
                .AddAsync(_mapper.Map<Product>(product));

            await unitOfWork.SaveChangesAsync();    
        }

        public async Task UpdateAsync (string id, UpdateProductDto updateProductDto)
        {
            var product = await unitOfWork
                .Repository<Product>()
                .GetByIdAsync(id);

            if (product == null) throw new KeyNotFoundException("Product not found");

            var category = await unitOfWork.Repository<Category>()
                .GetByIdAsync(updateProductDto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            var seller = await unitOfWork.Repository<User>()
                .GetByIdAsync(updateProductDto.SellerId);
            if (seller == null) throw new KeyNotFoundException("Seller not found");
            
            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.Stock = updateProductDto.Stock;
            product.SellerId = updateProductDto.SellerId;
            product.CategoryId = updateProductDto.CategoryId;

            //ToDo: Updating the images !!

            unitOfWork.Products.Update(product);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var product = await unitOfWork
                .Repository<Product>()
                .GetByIdAsync(id);

            if (product == null) throw new KeyNotFoundException("Product not found");

            unitOfWork.Products.Delete(product);
            await unitOfWork.SaveChangesAsync();
        }

    }
}
