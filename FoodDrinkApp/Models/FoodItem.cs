using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FoodDrinkApp.Models
{
    public class FoodItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _category = string.Empty;
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private string _details = string.Empty;
        public string Details
        {
            get => _details;
            set { _details = value; OnPropertyChanged(); }
        }

        private string _imageUrl = string.Empty;
        public string ImageUrl
        {
            get => _imageUrl;
            set { _imageUrl = value; OnPropertyChanged(); }
        }

        private int _prepTimeMinutes;
        public int PrepTimeMinutes
        {
            get => _prepTimeMinutes;
            set { _prepTimeMinutes = value; OnPropertyChanged(); }
        }

        private int _calories;
        public int Calories
        {
            get => _calories;
            set { _calories = value; OnPropertyChanged(); }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite != value)
                {
                    _isFavorite = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _origin = string.Empty;
        public string Origin
        {
            get => _origin;
            set { _origin = value; OnPropertyChanged(); }
        }

        private List<string> _ingredients = new();
        public List<string> Ingredients
        {
            get => _ingredients;
            set { _ingredients = value; OnPropertyChanged(); }
        }

        private double _rating;
        public double Rating
        {
            get => _rating;
            set { _rating = value; OnPropertyChanged(); }
        }

        private string _imageFileName = string.Empty;
        public string ImageFileName
        {
            get => _imageFileName;
            set { _imageFileName = value; OnPropertyChanged(); }
        }
    }
}