using TShirtManagement.Views;
using TShirtManagement.ViewModels;
using Framework.Data;
using Framework.Repositories;
using Framework.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace TShirtManagement
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            // Auto-create database, table, and stored procedures on startup
            var config = ServiceProvider.GetRequiredService<IConfiguration>();
            new Framework.Data.DatabaseInitializer(config).Initialize();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<AppDbContext>();
            services.AddSingleton<ITShirtRepository, TShirtRepository>();
            services.AddSingleton<ITShirtService, TShirtService>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }
}