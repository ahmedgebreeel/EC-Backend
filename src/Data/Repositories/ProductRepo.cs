using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepo : GenericRepository<Product>
    {

        public ProductRepo(AppDbContext context) : base(context) 
        {
           
        }

        public override async Task<IEnumerable<Product>> GetAllAsync(int? pageNum, int? pageSize)
        {
            if(pageNum is not null && pageSize is not null)
            {
                return await context.Products
                //.Include(p => p.Category)
                .Include(p => p.Images)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    Category = p.Category,
                    SellerId = p.SellerId,
                    Seller = p.Seller,
                    CartItems = p.CartItems,
                    OrderItems = p.OrderItems,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    //take only the main image (position 0)
                    Images = p.Images
                    .Where(i => i.Position == 0)
                    .ToList()
                })
                .Skip((pageNum - 1) * pageSize ??0)
                .Take(pageSize??10)
                .ToListAsync();

            }
                return await context.Products
                .Include(p=>p.Category)
                .Include(p=>p.Images)
                .Select(p=> new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    Category = p.Category,
                    SellerId = p.SellerId,
                    Seller = p.Seller,
                    CartItems = p.CartItems,
                    OrderItems = p.OrderItems,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    //take only the main image (position 0)
                    Images = p.Images
                    .Where(i=>i.Position==0)
                    .ToList()
                })
                .ToListAsync();           
        }

        public override async Task<Product?> GetByIdAsync(string id)
        {
            return await context.Products
                .Include(p=>p.Category)
                .Include(p=>p.Seller)
                .Include(p=>p.Images)
                .FirstOrDefaultAsync(p=>p.Id == id);
               
        }

        public async Task<int> CountAsync()
        {
            return await context.Products.CountAsync();
        }
    }
}
