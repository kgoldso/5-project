using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using TaskManager.Client.Services;
using TaskManager.Client.Services.Interfaces;
using TaskManager.Client.ViewModels;
using TaskManager.Client.Views;

namespace TaskManager.Client;

/// <summary>
/// Точка входа WPF-приложения.
/// Настройка Prism DI-контейнера и регистрация представлений.
/// </summary>
public partial class App : PrismApplication
{
    /// <summary>Создаёт главное окно приложения.</summary>
    protected override Window CreateShell()
        => Container.Resolve<MainWindow>();

    /// <summary>Регистрация зависимостей в DI-контейнере.</summary>
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Сервисы (Singleton — один экземпляр на всё приложение)
        containerRegistry.RegisterSingleton<ITokenStorageService, TokenStorageService>();
        containerRegistry.RegisterSingleton<IApiClient, ApiClient>();
        containerRegistry.RegisterSingleton<IAuthApiService, AuthApiService>();
        containerRegistry.Register<IProcessApiService, ProcessApiService>();
        containerRegistry.Register<ITaskApiService, TaskApiService>();

        // Представления для навигации через Prism RegionManager
        containerRegistry.RegisterForNavigation<LoginView>();
        containerRegistry.RegisterForNavigation<RegisterView>();
        containerRegistry.RegisterForNavigation<ProcessListView>();
        containerRegistry.RegisterForNavigation<TaskListView>();
        containerRegistry.RegisterForNavigation<SettingsView>();
    }

    /// <summary>Начальная навигация при запуске.</summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var regionManager = Container.Resolve<Prism.Regions.IRegionManager>();
        regionManager.RequestNavigate("ContentRegion", "LoginView", result =>
        {
            if (result.Error != null)
            {
                System.IO.File.WriteAllText("error.log", $"Navigation Error: {result.Error.Message}\n{result.Error.StackTrace}");
                System.Windows.MessageBox.Show($"Navigation Error: {result.Error.Message}", "Prism Error");
            }
        });
    }
}
