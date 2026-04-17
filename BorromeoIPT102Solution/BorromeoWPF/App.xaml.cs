using BorromeoWPF.Data;
using BorromeoWPF.Services;
using BorromeoWPF.Stores;
using BorromeoWPF.ViewModels;
using BorromeoWPF.Views;
using Domain.Interfaces;
using Framework.Commands;
using Framework.Queries;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;

namespace BorromeoWPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Read connection string from appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        var connStr = config.GetConnectionString("DefaultConnection")!;

        // Auto-create database, table, and stored procedures if not yet existing
        var dbInit = new DatabaseInitializer(connStr);
        dbInit.Initialize();

        // Wire up navigation
        var store = new NavigationStore();

        ICreateCommand create = new CreateCommand(connStr);
        IUpdateCommand update = new UpdateCommand(connStr);
        IDeleteCommand delete = new DeleteCommand(connStr);
        IGetTshirtAll getAll = new GetAllTshirt(connStr);

        NavigationService<HomeViewModel>? homeNav = null;
        NavigationService<TshirtViewModel>? tshirtNav = null;

        homeNav = new NavigationService<HomeViewModel>(store,
            () => new HomeViewModel(tshirtNav!));

        tshirtNav = new NavigationService<TshirtViewModel>(store,
            () => new TshirtViewModel(getAll, create, update, delete, homeNav!));

        homeNav.Navigate();

        var mainWindow = new MainView
        {
            DataContext = new MainViewModel(store)
        };
        mainWindow.Show();
    }
}
