using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepo : GenericRepository<Product>
    {

        public ProductRepo(AppDbContext context) : base(context) 
        {
           
        }

        public override async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products
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
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,

                    Images = p.Images
                    .Where(i=>i.Position==0)
                    
                    .ToList()

                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);
                
                
        }

        public override async Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(p=>p.Category)
                .Include(p=>p.Seller)
                .Include(p=>p.Images)
                .FirstOrDefaultAsync(p=>p.Id == id, cancellationToken);
               
        }
    }
}
