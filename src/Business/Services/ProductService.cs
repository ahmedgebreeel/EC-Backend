using AutoMapper;
using Core.DTOs.Products;
using Core.Entities;
using Core.Enums;
using Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Linq.Expressions;


namespace Business.Services
{
    public class ProductService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment env;

        public ProductService(UnitOfWork _unitOfWork, IMapper mapper, IWebHostEnvironment _env)
        {
            unitOfWork = _unitOfWork;
            _mapper = mapper;
            env = _env;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(
            int? pageNum, int? pageSize,
            string? search,
            string? category,
            decimal? minPrice,
            decimal? maxPrice)
        {
            bool hasSearch = 
                !string.IsNullOrEmpty(search)||
                !string.IsNullOrEmpty(category)||
                minPrice.HasValue||
                maxPrice.HasValue;
            if (hasSearch)
            {
                var existCategory = (await unitOfWork.Repository<Category>()
                    .FindAsync(c=>c.Name==category)).FirstOrDefault();

                Expression<Func<Product, bool>> predicate = p=>
                (string.IsNullOrEmpty(search) || p.Name.Contains(search)) &&
                (string.IsNullOrEmpty(category) || p.CategoryId == existCategory.Id) &&
                (!minPrice.HasValue || p.Price >= minPrice) &&
                (!maxPrice.HasValue || p.Price <= maxPrice);

                var res = await unitOfWork.Products.FindAsync(predicate);
                return res.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Price = p.Price,
                    Stock = p.Stock,
                    Image = p.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt

                }
                    ).ToList();
            }
            var products = await unitOfWork
                .Products
                .GetAllAsync(pageNum, pageSize);


            return _mapper.Map<List<ProductDto>>(products);

        }


        public async Task<ProductDetailsDto?> GetByIdAsync(string id)
        {
            var product = await unitOfWork
               .Products
               .GetByIdAsync(id);

            if (product == null) return null;

            return _mapper.Map<ProductDetailsDto?>(product);
        }

        public async Task AddAsync(AddProductDto product)
        {
            //check if category is exist
            var category = await unitOfWork.Repository<Category>()
                .GetByIdAsync(product.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            var seller = await unitOfWork.Users.GetByIdAsync(product.SellerId);
            if (seller == null) throw new KeyNotFoundException("Seller not found");

            //make sure its role is a seller
            if (seller.Role.Name != RoleType.Seller.ToString() && seller.Role.Name != RoleType.Admin.ToString()) throw new InvalidOperationException("User is not a seller");

            var productToAdd = _mapper.Map<Product>(product);
            await unitOfWork.Products
                .AddAsync(productToAdd);

            //ToDo: saving the images !!
            string uploadPath = Path.Combine(env.WebRootPath, "images");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            var position = 0;
            Console.WriteLine("Images count: " + product.Images?.Count);

            foreach (var file in product.Images!)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);

                var imageToAdd = new ProductImage
                {
                    ProductId = productToAdd.Id,
                    ImageUrl = "/images/" + fileName,
                    Position = position++,
                };

                await unitOfWork.Repository<ProductImage>().AddAsync(imageToAdd);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, UpdateProductDto updateProductDto)
        {
            var product = await unitOfWork
                .Products
                .GetByIdAsync(id);

            if (product == null) throw new KeyNotFoundException("Product not found");

            var category = await unitOfWork.Repository<Category>()
                .GetByIdAsync(updateProductDto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found.");

            var seller = await unitOfWork.Users
                .GetByIdAsync(updateProductDto.SellerId);
            if (seller == null) throw new KeyNotFoundException("Seller not found");

            //make sure its role is a seller
            if (seller.Role.Name != RoleType.Seller.ToString() && seller.Role.Name != RoleType.Admin.ToString()) throw new InvalidOperationException("User is not a seller");


            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.Stock = updateProductDto.Stock;
            product.SellerId = updateProductDto.SellerId;
            product.CategoryId = updateProductDto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            unitOfWork.Products.Update(product);

            //Updating the images !!
            string uploadPath = Path.Combine(env.WebRootPath, "images");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            var existingImages = product.Images.ToList();
            var newImages = updateProductDto.Images.ToList();

            var position = 0;

            foreach (var oldImage in existingImages)
            {
                if (!newImages.Any(i => i.FileName == oldImage.ImageUrl))
                {
                    // Delete the file from the server
                    var fullImagePath = Path.Combine(env.WebRootPath, oldImage.ImageUrl.TrimStart('/'));

                    if (File.Exists(fullImagePath))
                    {
                        File.Delete(fullImagePath);
                    }
                    unitOfWork.Repository<ProductImage>().Delete(oldImage);
                }
            }

            foreach (var file in newImages)
            {

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);

                var imageToAdd = new ProductImage
                {
                    ProductId = product.Id,
                    ImageUrl = "/images/" + fileName,
                    Position = position++
                };

                await unitOfWork.Repository<ProductImage>().AddAsync(imageToAdd);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var product = await unitOfWork
                .Products
                .GetByIdAsync(id);

            if (product == null) throw new KeyNotFoundException("Product not found");

            unitOfWork.Products.Delete(product);
            await unitOfWork.SaveChangesAsync();
        }

    }
}
