using Core.DTOs.Category;
using Core.Entities;
using Data.Repositories;

namespace Business.Services
{
    public class CategoryService
    {
        private readonly UnitOfWork unitOfWork;

        public CategoryService(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
           var categories = await unitOfWork.Repository<Category>().GetAllAsync();

            var categoryDTOs = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ParentCategoryId = c.ParentCategoryId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });

            return categoryDTOs;
        }

        public async Task<CategoryDto?> GetByIdAsync(string id)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
            return categoryDto; 
        }

        public async Task AddAsync(AddCategoryDto addCategoryDto)
        {
            var existingCategory = await unitOfWork.Repository<Category>()
                .FindAsync(c => c.Name.ToLower() == addCategoryDto.Name.Trim().ToLower());

            if (existingCategory.Any())
            {
                throw new InvalidOperationException("A category with the same name already exists.");
            }
            
            await unitOfWork.Repository<Category>().AddAsync(new Category
            {
                Name = addCategoryDto.Name.Trim().ToLower(),
                Description = addCategoryDto.Description,
                ParentCategoryId = addCategoryDto.ParentCategoryId
            });
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(id);
            if(category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            category.Name = updateCategoryDto.Name;

            if (updateCategoryDto.Description != null)
                category.Description = updateCategoryDto.Description;

            if (updateCategoryDto.ParentCategoryId != null)
                category.ParentCategoryId = updateCategoryDto.ParentCategoryId;

            category.UpdatedAt = DateTime.UtcNow;

            unitOfWork.Repository<Category>().Update(category);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var category = await unitOfWork.Repository<Category>().GetByIdAsync(id);
            if(category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            unitOfWork.Repository<Category>().Delete(category);
            await unitOfWork.SaveChangesAsync();
        }
    }
};