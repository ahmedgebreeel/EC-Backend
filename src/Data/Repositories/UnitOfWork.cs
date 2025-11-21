namespace Data.Repositories
{
    public class UnitOfWork
    {
        private readonly AppDbContext context;
        private readonly Dictionary<Type, object> repositories = new();

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

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

    }
}