using AutoMapper;
using Core.DTOs.Products;
using Core.Entities;
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
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _unitOfWork
                .Products
                .GetAllAsync();
            if (products == null) throw new KeyNotFoundException("Product not found");


            return _mapper.Map<List<ProductDto>>(products);
                
        }


        public async Task<ProductDetailsDto?> GetProductAsync(string id)
        {
            var product =  await _unitOfWork
               .Products
               .GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found");

            return _mapper.Map<ProductDetailsDto?>(product);
        }

       public async Task CreateProductAsync(CreateProductDto product)
        {
            //check if category is exist
            var category = await _unitOfWork.GenericRepository<Category>()
                .GetByIdAsync(product.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

                var seller = await _unitOfWork.GenericRepository<User>()
                .GetByIdAsync(product.SellerId);
            //Todo: make sure its role is a seller
            if (seller == null) throw new KeyNotFoundException("Seller not found");



            await _unitOfWork.Products
                .AddAsync(_mapper.Map<Product>(product));
        }

        public async Task UpdateProductAsync (UpdateProductDto product)
        {
            var exist = await _unitOfWork
                .GenericRepository<Product>()
                .GetByIdAsync(product.Id);

            if (exist == null) throw new KeyNotFoundException("Product not found");

            var category = await _unitOfWork.GenericRepository<Category>()
                .GetByIdAsync(product.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            var seller = await _unitOfWork.GenericRepository<User>()
                .GetByIdAsync(product.SellerId);
            if (seller == null) throw new KeyNotFoundException("Seller not found");

            if(product.Stock == 0)
                throw new InvalidOperationException("Cannot update a product with stock equal to zero.");


            
            exist.Name = product.Name;
            exist.Description = product.Description;
            exist.Price = product.Price;
            exist.Stock = product.Stock;
            exist.SellerId = product.SellerId;
            exist.CategoryId = product.CategoryId;

            var existingImages = exist.Images.ToList();

            // DTO image IDs
            var dtoImageIds = product.Images
                .Where(i => !string.IsNullOrEmpty(i.Id))
                .Select(i => i.Id)
                .ToList();

            // (A) DELETE IMAGES removed from DTO
            var imagesToDelete = existingImages
                .Where(img => !dtoImageIds.Contains(img.Id))
                .ToList();

            foreach (var img in imagesToDelete)
            {
                exist.Images.Remove(img);
                await _unitOfWork.GenericRepository<ProductImage>().DeleteAsync(img);
            }

            // (B) UPDATE existing images
            foreach (var dtoImg in product.Images
                .Where(i => !string.IsNullOrEmpty(i.Id)))
            {
                var existingImg = existingImages.FirstOrDefault(e => e.Id == dtoImg.Id);
                if (existingImg != null)
                {
                    existingImg.ImageUrl = dtoImg.ImageUrl;
                    existingImg.Position = dtoImg.Position;
                }
            }

            // (C) ADD new images (with no ID)
            var newImages = product.Images
                .Where(i => string.IsNullOrEmpty(i.Id))
                .Select(i => new ProductImage
                {
                    ImageUrl = i.ImageUrl,
                    Position = i.Position,
                    ProductId = exist.Id
                }).ToList();

            foreach (var img in newImages)
            {
                await _unitOfWork.GenericRepository<ProductImage>().AddAsync(img);
                exist.Images.Add(img);
            }



            await _unitOfWork
                .Products
                .EditAsync(exist);


        }

        public async Task DeleteProductAsync(UpdateProductDto product)
        {
            var exist = await _unitOfWork
                .GenericRepository<Product>()
                .GetByIdAsync(product.Id);
            if (exist == null) throw new KeyNotFoundException("Product not found");

            await _unitOfWork
                .Products
                .DeleteAsync(exist);
        }


     





    }
}
