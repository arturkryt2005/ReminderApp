using ReminderApp.Core.Entities.Models;
using ReminderApp.Core.Repositories.Interfaces;

namespace ReminderApp.Infrastructure.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> IRepository<T>.GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}