using Prism.Commands;
using Prism.Regions;
using TaskManager.Client.Services.Interfaces;

namespace TaskManager.Client.ViewModels;

/// <summary>
/// ViewModel экрана регистрации.
/// </summary>
public class RegisterViewModel : ViewModelBase, INavigationAware
{
    private readonly IAuthApiService _authService;
    private readonly IRegionManager _regionManager;
    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;

    public string Username
    {
        get => _username;
        set { SetProperty(ref _username, value); RegisterCommand.RaiseCanExecuteChanged(); }
    }

    public string Email
    {
        get => _email;
        set { SetProperty(ref _email, value); RegisterCommand.RaiseCanExecuteChanged(); }
    }

    public string Password
    {
        get => _password;
        set { SetProperty(ref _password, value); RegisterCommand.RaiseCanExecuteChanged(); }
    }

    public DelegateCommand RegisterCommand { get; }
    public DelegateCommand GoToLoginCommand { get; }

    public RegisterViewModel(IAuthApiService authService, IRegionManager regionManager)
    {
        _authService = authService;
        _regionManager = regionManager;

        RegisterCommand = new DelegateCommand(OnRegister, CanRegister);
        GoToLoginCommand = new DelegateCommand(OnGoToLogin);
    }

    private bool CanRegister()
        => !string.IsNullOrWhiteSpace(Username) &&
           !string.IsNullOrWhiteSpace(Email) &&
           !string.IsNullOrWhiteSpace(Password) &&
           Password.Length >= 6;

    /// <summary>Выполняет регистрацию нового пользователя.</summary>
    private async void OnRegister()
    {
        await ExecuteAsync(async () =>
        {
            await _authService.RegisterAsync(Username, Email, Password);
            var mainVm = System.Windows.Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            mainVm?.OnLoginSuccess();
        });
    }

    private void OnGoToLogin()
    {
        _regionManager.RequestNavigate("ContentRegion", "LoginView");
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        ErrorMessage = null;
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) => true;
    public void OnNavigatedFrom(NavigationContext navigationContext) { }
}
