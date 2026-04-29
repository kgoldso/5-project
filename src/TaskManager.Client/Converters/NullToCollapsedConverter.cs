using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TaskManager.Client.Converters;

/// <summary>
/// Конвертирует null в Visibility.Collapsed, не-null в Visible.
/// </summary>
public class NullToCollapsedConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        => value is null ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
