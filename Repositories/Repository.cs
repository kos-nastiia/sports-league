using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsLeague.Data;

namespace SportsLeague.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? query = null)
        {
            IQueryable<T> dbQuery = _context.Set<T>();
            if (query is not null)
            {
                dbQuery = query(dbQuery);
            }

            return await dbQuery.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? query = null)
        {
            if (query is null)
            {
                return await _context.Set<T>().FindAsync(id);
            }

            IQueryable<T> dbQuery = query(_context.Set<T>());
            return await dbQuery.FirstOrDefaultAsync(entity => EF.Property<int>(entity, "Id") == id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity is null)
            {
                return;
            }

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
