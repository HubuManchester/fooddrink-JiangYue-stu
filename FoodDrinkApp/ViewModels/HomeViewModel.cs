using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDrinkApp.Models;

namespace FoodDrinkApp.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<FoodItem> _foodItems = new();

        [ObservableProperty]
        private ObservableCollection<FoodItem> _filteredItems = new();

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private string _selectedCategory = "All";

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private FoodItem? _selectedItem;

        public List<string> Categories { get; } = new()
        {
            "All", "Appetizer", "Main Course", "Dessert", "Drink", "Snack"
        };

        public HomeViewModel()
        {
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            var items = new List<FoodItem>
            {
                new FoodItem { Id = 1, Name = "Margherita Pizza", Category = "Main Course", Description = "Classic Italian pizza with tomato, mozzarella, and basil", Details = "A traditional Neapolitan pizza topped with fresh tomatoes, mozzarella cheese, fresh basil, and olive oil.", Origin = "Italy", PrepTimeMinutes = 25, Calories = 285, Rating = 4.8, Ingredients = new List<string> { "Pizza dough", "Tomato sauce", "Mozzarella", "Fresh basil", "Olive oil" } },
                new FoodItem { Id = 2, Name = "Sushi Roll", Category = "Main Course", Description = "Japanese rice rolls with fresh fish and vegetables", Details = "Assorted sushi rolls made with seasoned sushi rice and fresh ingredients, wrapped in nori seaweed.", Origin = "Japan", PrepTimeMinutes = 40, Calories = 200, Rating = 4.6, Ingredients = new List<string> { "Sushi rice", "Nori seaweed", "Fresh salmon", "Cucumber", "Avocado" } },
                new FoodItem { Id = 3, Name = "Chocolate Lava Cake", Category = "Dessert", Description = "Warm chocolate cake with a molten center", Details = "Individual chocolate cakes with a gooey molten chocolate center, served warm with vanilla ice cream.", Origin = "France", PrepTimeMinutes = 20, Calories = 350, Rating = 4.9, Ingredients = new List<string> { "Dark chocolate", "Butter", "Eggs", "Sugar", "Flour" } },
                new FoodItem { Id = 4, Name = "Caesar Salad", Category = "Appetizer", Description = "Fresh romaine lettuce with Caesar dressing and croutons", Details = "Crisp romaine lettuce tossed in creamy Caesar dressing, topped with parmesan and crunchy croutons.", Origin = "United States", PrepTimeMinutes = 15, Calories = 150, Rating = 4.3, Ingredients = new List<string> { "Romaine lettuce", "Caesar dressing", "Parmesan", "Croutons", "Anchovies" } },
                new FoodItem { Id = 5, Name = "Matcha Latte", Category = "Drink", Description = "Japanese green tea latte with creamy milk", Details = "Traditional matcha green tea whisked with steamed milk and sweetened to taste.", Origin = "Japan", PrepTimeMinutes = 10, Calories = 120, Rating = 4.5, Ingredients = new List<string> { "Matcha powder", "Steamed milk", "Simple syrup", "Hot water" } },
                new FoodItem { Id = 6, Name = "Tacos", Category = "Main Course", Description = "Mexican street tacos with seasoned meat and fresh toppings", Details = "Authentic corn tortillas filled with seasoned beef, fresh cilantro, onions, and lime.", Origin = "Mexico", PrepTimeMinutes = 30, Calories = 220, Rating = 4.7, Ingredients = new List<string> { "Corn tortillas", "Ground beef", "Cilantro", "Onion", "Lime" } }
            };
            FoodItems = new ObservableCollection<FoodItem>(items);
            FilteredItems = new ObservableCollection<FoodItem>(items);
        }

        [RelayCommand]
        private void ApplyFilter()
        {
            var filtered = FoodItems.AsEnumerable();
            if (SelectedCategory != "All")
                filtered = filtered.Where(item => item.Category == SelectedCategory);
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                filtered = filtered.Where(item => item.Name.ToLower().Contains(searchLower) || item.Description.ToLower().Contains(searchLower) || item.Origin.ToLower().Contains(searchLower));
            }
            FilteredItems = new ObservableCollection<FoodItem>(filtered);
        }

        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = string.Empty;
            SelectedCategory = "All";
            FilteredItems = new ObservableCollection<FoodItem>(FoodItems);
        }

        [RelayCommand]
        private async Task RefreshData()
        {
            IsLoading = true;
            await Task.Delay(500);
            LoadSampleData();
            IsLoading = false;
        }

        partial void OnSearchTextChanged(string value) => ApplyFilter();
        partial void OnSelectedCategoryChanged(string value) => ApplyFilter();
    }
}
