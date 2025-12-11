using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public override Task<User?> GetByIdAsync(string id)
        {
            return context.Users
                .Include(u => u.Addresses)
                .Include(u => u.Orders)
                .Include(u => u.ShoppingCart!)
                    .ThenInclude(sc => sc.CartItems)
                        .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}