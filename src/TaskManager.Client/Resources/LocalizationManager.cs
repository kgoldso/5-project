using System.Windows;

namespace TaskManager.Client.Resources;

/// <summary>
/// Управление локализацией — загрузка и смена языковых словарей «на лету».
/// </summary>
public static class LocalizationManager
{
    private const string LangDictionaryPrefix = "Resources/Lang/Strings.";
    private static string _currentLanguage = "ru";

    /// <summary>Текущий язык интерфейса.</summary>
    public static string CurrentLanguage => _currentLanguage;

    /// <summary>
    /// Переключает язык интерфейса и обновляет ResourceDictionary.
    /// </summary>
    public static void SwitchLanguage(string languageCode)
    {
        _currentLanguage = languageCode.ToLowerInvariant();

        var dictUri = new Uri($"{LangDictionaryPrefix}{_currentLanguage}.xaml", UriKind.Relative);
        var newDict = new ResourceDictionary { Source = dictUri };

        // Удаляем старый языковой словарь
        var existingDict = Application.Current.Resources.MergedDictionaries
            .FirstOrDefault(d => d.Source?.OriginalString.Contains(LangDictionaryPrefix) == true);

        if (existingDict is not null)
            Application.Current.Resources.MergedDictionaries.Remove(existingDict);

        // Добавляем новый
        Application.Current.Resources.MergedDictionaries.Add(newDict);
    }
}
