using ReminderApp.Core.Entities.Enums;

namespace ReminderApp.Core.Entities.Models
{
    /// <summary>
    /// Сущность напоминания
    /// </summary>
    public class Reminder
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        /// <summary>
        /// Время напоминания
        /// </summary>
        public DateTime ReminderTime { get; set; }

        /// <summary>
        /// Статус напоминания
        /// </summary>
        public ReminderStatus Status { get; set; } = ReminderStatus.Pending;

        /// <summary>
        /// Отложенное время напоминания
        /// </summary>
        public DateTime? SnoozeUntil { get; set; }

        /// <summary>
        /// Момент выполнения
        /// </summary>
        public DateTime? CompletedAt { get; set; }
    }
}
