using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly MVCAppDbContext _context;

        public GenericRepository(MVCAppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
                return  (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();

            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int? id) => await _context.Set<T>().FindAsync(id);

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
