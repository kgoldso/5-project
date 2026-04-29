using Prism.Commands;
using TaskManager.Client.Resources;

namespace TaskManager.Client.ViewModels;

/// <summary>
/// ViewModel настроек приложения: язык интерфейса.
/// </summary>
public class SettingsViewModel : ViewModelBase
{
    private string _selectedLanguage;

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (SetProperty(ref _selectedLanguage, value))
                LocalizationManager.SwitchLanguage(value);
        }
    }

    /// <summary>Список доступных языков.</summary>
    public string[] AvailableLanguages => ["ru", "en"];

    public SettingsViewModel()
    {
        _selectedLanguage = LocalizationManager.CurrentLanguage;
    }
}
