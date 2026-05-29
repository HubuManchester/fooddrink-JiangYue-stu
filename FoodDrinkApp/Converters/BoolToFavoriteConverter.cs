using System.Globalization;

namespace FoodDrinkApp.Converters
{
    public class BoolToFavoriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFavorite) return isFavorite ? "Favorited" : "Not Favorited";
            return "Not Favorited";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
