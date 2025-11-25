using Core.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Repositories
{
    public class UnitOfWork 
    {
        private readonly AppDbContext context;
        private readonly Dictionary<Type, object> repositories = new();

        private ProductRepo products ;
        private UserRepository users;

        public UnitOfWork(AppDbContext _context)
        {
            context = _context;

        }

        public GenericRepository<T> Repository<T>() where T : class
        {
            if(!repositories.ContainsKey(typeof(T)))
            {
                var repositoryInstance = new GenericRepository<T>(context);
                repositories.Add(typeof(T), repositoryInstance);
            }
            return (GenericRepository<T>)repositories[typeof(T)];
        }

        public ProductRepo Products {
            get
            {
                if (products == null)
                {
                    products = new ProductRepo(context);
                }
                return products;
            }
        }

        public UserRepository Users {
            get
            {
                if (users == null)
                {
                    users = new UserRepository(context);
                }
                return users;
            }
        }
 

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        } 
    }
}
