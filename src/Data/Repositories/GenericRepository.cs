using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class GenericRepository<T>  where T : class
    {
        protected readonly AppDbContext context;

        public GenericRepository(AppDbContext _context)
        {
            context = _context;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
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
