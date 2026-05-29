using System.Globalization;

namespace FoodDrinkApp.Converters
{
    /// <summary>
    /// Converts boolean value to Color.
    /// Returns Red for true, Gray for false.
    /// Used for favorite button and filter button colors.
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
                return Colors.Red;      // Active/selected state - Red
            return Colors.Gray;          // Inactive/unselected state - Gray
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}