using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class GenericRepository<T>  where T : class
    {
        protected readonly AppDbContext context;

        public GenericRepository(AppDbContext _context)
        {
            context = _context;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(int? pageNum =null , int? pageSize=null)
        {
            if(pageNum is not null && pageSize is not null )
            {
                return await context.Set<T>()
                    .Skip((pageNum - 1) * pageSize??0)
                    .Take(pageSize??10)
                    .ToListAsync();
            }
            return await context.Set<T>().ToListAsync();
        }
        
        public virtual async Task<T?> GetByIdAsync(string id)
        {
            return await context.Set<T>().FindAsync(id);
        }


        public virtual async Task AddAsync(T entity )
        {
            await context.Set<T>().AddAsync(entity );
        }

       
        public virtual void Update(T entity )
        {
            context.Set<T>().Update(entity);
        }

        public virtual void Delete(T entity )
        {
            context.Set<T>().Remove(entity);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

       
    }
}
