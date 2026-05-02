using Prism.Commands;
using Prism.Regions;
using TaskManager.Client.Services.Interfaces;

namespace TaskManager.Client.ViewModels;

/// <summary>
/// ViewModel для экрана авторизации.
/// </summary>
public class LoginViewModel : ViewModelBase, INavigationAware
{
    private readonly IAuthApiService _authService;
    private readonly IRegionManager _regionManager;
    private string _username = string.Empty;
    private string _password = string.Empty;

    public string Username
    {
        get => _username;
        set { SetProperty(ref _username, value); LoginCommand.RaiseCanExecuteChanged(); }
    }

    public string Password
    {
        get => _password;
        set { SetProperty(ref _password, value); LoginCommand.RaiseCanExecuteChanged(); }
    }

    public DelegateCommand LoginCommand { get; }
    public DelegateCommand GoToRegisterCommand { get; }

    public LoginViewModel(IAuthApiService authService, IRegionManager regionManager)
    {
        _authService = authService;
        _regionManager = regionManager;

        LoginCommand = new DelegateCommand(OnLogin, CanLogin);
        GoToRegisterCommand = new DelegateCommand(OnGoToRegister);
    }

    // Простая проверка для активации кнопки входа
    private bool CanLogin()
        => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

    // Выполняем вход и уведомляем главную модель для смены интерфейса
    private async void OnLogin()
    {
        await ExecuteAsync(async () =>
        {
            await _authService.LoginAsync(Username, Password);
            
            var mainVm = System.Windows.Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            mainVm?.OnLoginSuccess();
        });
    }

    private void OnGoToRegister()
    {
        _regionManager.RequestNavigate("ContentRegion", "RegisterView");
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        Username = string.Empty;
        Password = string.Empty;
        ErrorMessage = null;
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) => true;
    public void OnNavigatedFrom(NavigationContext navigationContext) { }
}
