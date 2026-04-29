using System.Windows;
using System.Windows.Markup;

namespace TaskManager.Client.Helpers;

/// <summary>
/// Расширение разметки для получения локализованной строки из ресурсов.
/// Использование в XAML: {helpers:Loc Key=SomeKey}
/// </summary>
[MarkupExtensionReturnType(typeof(string))]
public class LocExtension(string key) : MarkupExtension
{
    public string Key { get; set; } = key;

    public LocExtension() : this(string.Empty) { }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(Key))
            return string.Empty;

        return Application.Current.TryFindResource(Key) as string ?? $"[{Key}]";
    }
}
