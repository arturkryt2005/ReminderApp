using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReminderApp.Infrastructure.Data;
using ReminderApp.Infrastructure.Repositories.Implementations;
using ReminderApp.Core.Repositories.Interfaces;

namespace ReminderApp 
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ReminderApp");

            Directory.CreateDirectory(appDataPath);
            var dbPath = Path.Combine(appDataPath, "app_data.db");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<MainWindow>();
        }
    }
}