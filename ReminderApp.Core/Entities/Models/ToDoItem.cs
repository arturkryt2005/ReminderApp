using ReminderApp.Core.Entities.Enums;

namespace ReminderApp.Core.Entities.Models
{
    public class ToDoItem : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TodoStatus Status { get; set; } = TodoStatus.Pending;

        public DateTime? CompletedAt { get; set; }
    }
}
