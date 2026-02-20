using ReminderApp.Core.Entities.Models;

namespace ReminderApp.Core.Repositories.Interfaces
{
    /// <summary>
    /// Интерфес для репозиториев сущностей (обобщенная)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
