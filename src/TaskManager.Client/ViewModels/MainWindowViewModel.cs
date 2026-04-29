using System.Windows;
using Prism.Commands;
using Prism.Regions;
using TaskManager.Client.Resources;
using TaskManager.Client.Services.Interfaces;

namespace TaskManager.Client.ViewModels;

/// <summary>
/// Главный ViewModel оболочки приложения: навигация, выход, смена языка.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    private readonly IRegionManager _regionManager;
    private readonly IAuthApiService _authService;
    private bool _isLoggedIn;
    private string _currentLanguage = "ru";

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set => SetProperty(ref _isLoggedIn, value);
    }

    public string CurrentLanguage
    {
        get => _currentLanguage;
        set => SetProperty(ref _currentLanguage, value);
    }

    public DelegateCommand LogoutCommand { get; }
    public DelegateCommand<string> NavigateCommand { get; }
    public DelegateCommand<string> SwitchLanguageCommand { get; }

    public MainWindowViewModel(IRegionManager regionManager, IAuthApiService authService)
    {
        _regionManager = regionManager;
        _authService = authService;

        LogoutCommand = new DelegateCommand(OnLogout);
        NavigateCommand = new DelegateCommand<string>(OnNavigate);
        SwitchLanguageCommand = new DelegateCommand<string>(OnSwitchLanguage);

        // По умолчанию показываем экран входа
        IsLoggedIn = false;
    }

    /// <summary>Навигация к указанному представлению.</summary>
    private void OnNavigate(string viewName)
    {
        _regionManager.RequestNavigate("ContentRegion", viewName);
    }

    /// <summary>Выполняет выход из системы.</summary>
    private void OnLogout()
    {
        _authService.Logout();
        IsLoggedIn = false;
        _regionManager.RequestNavigate("ContentRegion", "LoginView");
    }

    /// <summary>Переключает язык интерфейса.</summary>
    private void OnSwitchLanguage(string lang)
    {
        CurrentLanguage = lang;
        LocalizationManager.SwitchLanguage(lang);

        // Принудительно обновляем все привязки в текущем окне
        if (Application.Current.MainWindow is not null)
        {
            var currentView = _regionManager.Regions["ContentRegion"].ActiveViews.FirstOrDefault();
            if (currentView is FrameworkElement fe)
            {
                var viewName = fe.GetType().Name;
                _regionManager.RequestNavigate("ContentRegion", viewName);
            }
        }
    }

    /// <summary>Вызывается при успешной аутентификации.</summary>
    public void OnLoginSuccess()
    {
        IsLoggedIn = true;
        _regionManager.RequestNavigate("ContentRegion", "ProcessListView");
    }
}
