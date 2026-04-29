using System.Windows.Controls;

namespace TaskManager.Client.Views;

/// <summary>
/// Экран регистрации нового пользователя.
/// </summary>
public partial class RegisterView : UserControl
{
    public RegisterView()
    {
        InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is ViewModels.RegisterViewModel vm)
            vm.Password = ((PasswordBox)sender).Password;
    }
}
