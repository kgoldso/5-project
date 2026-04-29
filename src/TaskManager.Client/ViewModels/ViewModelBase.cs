using Prism.Mvvm;
using Prism.Commands;

namespace TaskManager.Client.ViewModels;

/// <summary>
/// Базовый ViewModel с поддержкой флага загрузки и сообщений об ошибках.
/// </summary>
public abstract class ViewModelBase : BindableBase
{
    private bool _isBusy;
    private string? _errorMessage;

    /// <summary>Признак выполнения асинхронной операции.</summary>
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    /// <summary>Сообщение об ошибке для отображения в UI.</summary>
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    /// <summary>
    /// Выполняет асинхронную операцию с обработкой ошибок и индикатором загрузки.
    /// </summary>
    protected async Task ExecuteAsync(Func<Task> action)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            await action();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
