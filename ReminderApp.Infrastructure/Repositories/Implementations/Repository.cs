using Microsoft.EntityFrameworkCore;
using ReminderApp.Core.Entities.Models;
using ReminderApp.Core.Repositories.Interfaces;
using ReminderApp.Infrastructure.Data;

namespace ReminderApp.Infrastructure.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        async Task<IEnumerable<T>> IRepository<T>.GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException($"Сущность '{typeof(T).Name}' с Id={id} не найдена в базе данных.");
            }

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}