using System.Windows;

namespace TaskManager.Client.Helpers;

/// <summary>
/// Централизованная обработка ошибок в WPF-клиенте.
/// </summary>
public static class ErrorHandler
{
    /// <summary>Показывает диалог ошибки.</summary>
    public static void ShowError(string message)
    {
        var title = Application.Current.TryFindResource("Error") as string ?? "Error";
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <summary>Показывает диалог информации.</summary>
    public static void ShowInfo(string message)
    {
        var title = Application.Current.TryFindResource("Success") as string ?? "Success";
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>Показывает диалог подтверждения.</summary>
    public static bool Confirm(string message)
    {
        var title = Application.Current.TryFindResource("Confirm") as string ?? "Confirm";
        var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }

    /// <summary>Обрабатывает исключение с отображением ошибки.</summary>
    public static void Handle(Exception ex)
    {
        var message = ex is System.Net.Http.HttpRequestException httpEx
            ? httpEx.Message
            : $"Произошла ошибка: {ex.Message}";

        ShowError(message);
    }
}
