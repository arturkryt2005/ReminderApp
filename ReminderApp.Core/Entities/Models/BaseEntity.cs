namespace ReminderApp.Core.Entities.Models
{
    /// <summary>
    /// Сущность для автоматического Id и врмени создания
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
