using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class OrderRepo : GenericRepository<Order>
    {
        public OrderRepo(AppDbContext _context) : base(_context)
        {
        }

        public override async Task<IEnumerable<Order>> GetAllAsync(int? pageNum =  null, int? pageSize = null)
        {
            if (pageNum is not null && pageSize is not null) {
                return await context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Skip((pageNum - 1) * pageSize ??0)
                .Take(pageSize ?? 20)
                .ToListAsync();

            }

           return await context.Orders
                .Include(o => o.User)
                .Include(o=>o.Address)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public override async Task<Order?> GetByIdAsync(string id)
        {
            return await context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetByUserIdAsync(string userId)
        {
            return await context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.UserId == userId);
        }
    }
}
