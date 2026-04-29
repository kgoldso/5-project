using System.Globalization;
using System.Windows.Data;

namespace TaskManager.Client.Converters;

/// <summary>
/// Инвертирует значение bool (true → false, false → true).
/// </summary>
public class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b ? !b : value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b ? !b : value;
}
