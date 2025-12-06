using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ShoppingCartRepo : GenericRepository<ShoppingCart>
    {
        public ShoppingCartRepo(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ShoppingCart>> GetAllAsync(int? pageNum = null, int? pageSize = null)
        {
            if(pageNum is not null && pageSize is not null)
            {
                return await context.ShoppingCarts
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                .Skip((pageNum - 1) * pageSize ?? 0)
                .Take(pageSize ?? 10)
                .ToListAsync();
            }
            return await context.ShoppingCarts
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(ci => ci.Category)
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .ToListAsync();
        }
        public override async Task<ShoppingCart?> GetByIdAsync(string id)
        {
            return await context.ShoppingCarts
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(ci => ci.Category)
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<ShoppingCart?> GetByUserIdAsync(string userId)
        {
            return await context.ShoppingCarts
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(ci => ci.Category)
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);
        }

    }
}

