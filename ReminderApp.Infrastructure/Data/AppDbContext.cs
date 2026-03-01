using ReminderApp.Core.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ReminderApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ToDoItem>(entity => 
            { 
                entity.HasKey(e => e.Id); 
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });
        }
    }
}
