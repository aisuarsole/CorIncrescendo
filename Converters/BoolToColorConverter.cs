using System.Globalization;

namespace CorIncrescendo.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isActive = value is bool b && b;
            return isActive
                ? Color.FromArgb("#CC0000")   // IncRed quan actiu
                : Color.FromArgb("#AAAAAA");  // Gris quan inactiu
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}


