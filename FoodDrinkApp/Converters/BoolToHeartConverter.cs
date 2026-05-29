using System.Globalization;

namespace FoodDrinkApp.Converters
{
    /// <summary>
    /// Converts boolean favorite status to heart symbol.
    /// Returns filled heart (❤️) for true, empty heart (♡) for false.
    /// Used for favorite button display.
    /// </summary>
    public class BoolToHeartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
                return "❤️";     // Filled heart - item is favorited
            return "♡";           // Empty heart - item is not favorited
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}