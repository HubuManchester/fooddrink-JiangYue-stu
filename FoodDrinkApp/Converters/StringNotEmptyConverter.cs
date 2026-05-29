using System.Globalization;

namespace FoodDrinkApp.Converters
{
    /// <summary>
    /// Checks if a string is not null or whitespace.
    /// Returns true for non-empty strings, false for empty/null/whitespace.
    /// Used to show/hide the clear search button.
    /// </summary>
    public class StringNotEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str))
                return true;     // String has content - show clear button
            return false;         // String is empty - hide clear button
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}