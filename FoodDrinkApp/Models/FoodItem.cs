using System.Collections.Generic;
using FoodDrinkApp.ViewModels;

namespace FoodDrinkApp.Models
{
    /// <summary>
    /// Represents a food or drink item with all its properties.
    /// Implements INotifyPropertyChanged through ViewModelBase for data binding support.
    /// </summary>
    public class FoodItem : ViewModelBase
    {
        private int _id;
        /// <summary>
        /// Unique identifier for the food item.
        /// </summary>
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name = string.Empty;
        /// <summary>
        /// Name of the food item.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _category = string.Empty;
        /// <summary>
        /// Category (e.g., "Main Course", "Dessert", "Drink").
        /// </summary>
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        private string _description = string.Empty;
        /// <summary>
        /// Short description of the food item.
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _details = string.Empty;
        /// <summary>
        /// Detailed information including cooking tips and history.
        /// </summary>
        public string Details
        {
            get => _details;
            set => SetProperty(ref _details, value);
        }

        private string _imageUrl = string.Empty;
        /// <summary>
        /// URL for the food image (for online images).
        /// </summary>
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        private int _prepTimeMinutes;
        /// <summary>
        /// Preparation time in minutes.
        /// </summary>
        public int PrepTimeMinutes
        {
            get => _prepTimeMinutes;
            set => SetProperty(ref _prepTimeMinutes, value);
        }

        private int _calories;
        /// <summary>
        /// Calorie count per serving.
        /// </summary>
        public int Calories
        {
            get => _calories;
            set => SetProperty(ref _calories, value);
        }

        private bool _isFavorite;
        /// <summary>
        /// Indicates whether this item is in the user's favorites.
        /// </summary>
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }

        private string _origin = string.Empty;
        /// <summary>
        /// Country or region of origin.
        /// </summary>
        public string Origin
        {
            get => _origin;
            set => SetProperty(ref _origin, value);
        }

        private List<string> _ingredients = new();
        /// <summary>
        /// List of ingredients required for this recipe.
        /// </summary>
        public List<string> Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }

        private double _rating;
        /// <summary>
        /// User rating from 0.0 to 5.0.
        /// </summary>
        public double Rating
        {
            get => _rating;
            set => SetProperty(ref _rating, value);
        }

        private string _imageFileName = string.Empty;
        /// <summary>
        /// Local filename for the food image (embedded resource).
        /// </summary>
        public string ImageFileName
        {
            get => _imageFileName;
            set => SetProperty(ref _imageFileName, value);
        }
    }
}
