using ReminderApp.Core.Entities.Models;
using ReminderApp.Core.Repositories.Interfaces;

namespace ReminderApp.Core.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public Task<T> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAll()
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
