using System.Windows.Controls;

namespace TaskManager.Client.Views;

/// <summary>
/// Экран авторизации пользователя.
/// </summary>
public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    /// <summary>Передаёт пароль во ViewModel (PasswordBox не поддерживает привязку).</summary>
    private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is ViewModels.LoginViewModel vm)
            vm.Password = ((PasswordBox)sender).Password;
    }
}
