using FoodDrinkApp.Models;
using FoodDrinkApp.Services;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FoodDrinkApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly IHardwareService _hardwareService;

        // Properties
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private string _selectedCategory = "All";
        public string SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private bool _showOnlyFavorites;
        public bool ShowOnlyFavorites
        {
            get => _showOnlyFavorites;
            set { _showOnlyFavorites = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private bool _isMenuOpen;
        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set { _isMenuOpen = value; OnPropertyChanged(); }
        }

        private bool _isAccelerometerActive;
        public bool IsAccelerometerActive
        {
            get => _isAccelerometerActive;
            set { _isAccelerometerActive = value; OnPropertyChanged(); }
        }

        private double _accelerometerX;
        public double AccelerometerX
        {
            get => _accelerometerX;
            set { _accelerometerX = value; OnPropertyChanged(); }
        }

        private double _accelerometerY;
        public double AccelerometerY
        {
            get => _accelerometerY;
            set { _accelerometerY = value; OnPropertyChanged(); }
        }

        private double _accelerometerZ;
        public double AccelerometerZ
        {
            get => _accelerometerZ;
            set { _accelerometerZ = value; OnPropertyChanged(); }
        }

        public ObservableCollection<FoodItem> FoodItems { get; set; } = new();
        public ObservableCollection<FoodItem> FilteredItems { get; set; } = new();

        // Commands
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand ToggleShowFavoritesCommand { get; }
        public ICommand ToggleMenuCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand ShareItemCommand { get; }
        public ICommand SwipeFavoriteCommand { get; }
        public ICommand ToggleAccelerometerCommand { get; }
        public ICommand VibrateCommand { get; }
        public ICommand SpeakRecipeCommand { get; }
        public ICommand TakePhotoCommand { get; }
        public ICommand GetNearbyLocationCommand { get; }

        // Accelerometer fields
        private bool _isAccelerometerRunning;
        private const double ShakeThreshold = 2.5;
        private const int ShakeCooldownMilliseconds = 1500;
        private DateTime _lastShakeTime = DateTime.MinValue;

        public HomeViewModel(IHardwareService hardwareService)
        {
            _hardwareService = hardwareService;

            ToggleFavoriteCommand = new Command<FoodItem>(OnToggleFavorite);
            ClearSearchCommand = new Command(OnClearSearch);
            SelectCategoryCommand = new Command<string>(OnSelectCategory);
            ToggleShowFavoritesCommand = new Command(OnToggleShowFavorites);
            ToggleMenuCommand = new Command(OnToggleMenu);
            ApplyFilterCommand = new Command(ApplyFilter);
            DeleteItemCommand = new Command<FoodItem>(OnDeleteItem);
            ShareItemCommand = new Command<FoodItem>(OnShareItem);
            SwipeFavoriteCommand = new Command<FoodItem>(OnSwipeFavorite);
            ToggleAccelerometerCommand = new Command(OnToggleAccelerometer);
            VibrateCommand = new Command(OnVibrate);
            SpeakRecipeCommand = new Command(OnSpeakRecipe);
            TakePhotoCommand = new Command(OnTakePhoto);
            GetNearbyLocationCommand = new Command(OnGetNearbyLocation);

            LoadSampleData();
            LoadSavedFavorites();
        }

        private void LoadSampleData()
        {
            FoodItems.Add(new FoodItem
            {
                Id = 1,
                Name = "Tiramisu",
                Category = "Dessert",
                Description = "A classic Italian dessert made with ladyfingers soaked in coffee and mascarpone cream.",
                Details = "Tiramisu means 'pick me up' in Italian, referring to the energy boost from espresso and cocoa. Originating in Veneto in the 1960s, this no-bake dessert has become Italy's most iconic sweet treat worldwide.",
                ImageFileName = "tiramisu",
                PrepTimeMinutes = 45,
                Calories = 350,
                Origin = "Italy",
                Rating = 4.8,
                Ingredients = new List<string> { "Ladyfinger biscuits", "Mascarpone cheese", "Fresh espresso", "Egg yolks", "Cocoa powder", "Marsala wine", "Dark chocolate" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 2,
                Name = "Sushi",
                Category = "Main Course",
                Description = "Traditional Japanese dish consisting of vinegared rice with various toppings.",
                Details = "Sushi originated in Southeast Asia as a way to preserve fish. Over centuries, it evolved into the refined art form we know today. The key to good sushi is fresh ingredients and precise preparation.",
                ImageFileName = "sushiroll",
                PrepTimeMinutes = 60,
                Calories = 280,
                Origin = "Japan",
                Rating = 4.9,
                Ingredients = new List<string> { "Sushi rice", "Fresh fish (salmon, tuna)", "Nori seaweed", "Wasabi", "Soy sauce", "Pickled ginger", "Avocado" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 3,
                Name = "Matcha Latte",
                Category = "Drink",
                Description = "Creamy green tea latte made with high-quality matcha powder.",
                Details = "Matcha is finely ground green tea leaves that have been shade-grown for several weeks before harvest. This process increases chlorophyll and amino acids, giving matcha its vibrant green color and unique umami flavor.",
                ImageFileName = "matchalatte",
                PrepTimeMinutes = 5,
                Calories = 180,
                Origin = "Japan",
                Rating = 4.6,
                Ingredients = new List<string> { "Ceremonial grade matcha powder", "Steamed whole milk (or oat milk)", "Hot water (80°C)", "Honey or simple syrup", "Bamboo whisk (chasen)" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 4,
                Name = "Tacos",
                Category = "Main Course",
                Description = "Traditional Mexican dish with seasoned meat in a corn or flour tortilla.",
                Details = "Tacos have a rich history dating back to pre-Columbian times. The word 'taco' comes from the Nahuatl word 'tlahco' meaning 'half' or 'in the middle'.",
                ImageFileName = "tacos",
                PrepTimeMinutes = 30,
                Calories = 250,
                Origin = "Mexico",
                Rating = 4.7,
                Ingredients = new List<string> { "Corn tortillas", "Ground beef or chicken", "Fresh cilantro", "Onions", "Lime wedges", "Salsa", "Guacamole", "Cheese" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 5,
                Name = "Caesar Salad",
                Category = "Appetizer",
                Description = "Classic salad with romaine lettuce, croutons, parmesan, and Caesar dressing.",
                Details = "The Caesar salad was invented in 1924 by Caesar Cardini, an Italian-American restaurateur in Tijuana, Mexico.",
                ImageFileName = "caesarsalad",
                PrepTimeMinutes = 15,
                Calories = 320,
                Origin = "Mexico",
                Rating = 4.5,
                Ingredients = new List<string> { "Romaine lettuce", "Croutons", "Parmesan cheese", "Caesar dressing", "Anchovies", "Black pepper", "Lemon juice" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 6,
                Name = "Pho",
                Category = "Main Course",
                Description = "Vietnamese noodle soup with beef or chicken and aromatic herbs.",
                Details = "Pho is Vietnam's national dish, with a history dating back to the early 20th century in northern Vietnam.",
                ImageFileName = "springrolls",
                PrepTimeMinutes = 180,
                Calories = 220,
                Origin = "Vietnam",
                Rating = 4.8,
                Ingredients = new List<string> { "Rice noodles", "Beef bones or chicken", "Star anise", "Cinnamon stick", "Ginger", "Cilantro", "Lime", "Bean sprouts", "Thai basil" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 7,
                Name = "Croissant",
                Category = "Snack",
                Description = "Buttery, flaky French pastry with a crescent shape.",
                Details = "The croissant originated in Austria as 'kipferl', but was popularized in France in the 19th century.",
                ImageFileName = "frenchfries",
                PrepTimeMinutes = 180,
                Calories = 300,
                Origin = "France",
                Rating = 4.7,
                Ingredients = new List<string> { "All-purpose flour", "Butter", "Yeast", "Sugar", "Salt", "Milk", "Eggs" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 8,
                Name = "Pad Thai",
                Category = "Main Course",
                Description = "Thai stir-fried noodles with shrimp, peanuts, and lime.",
                Details = "Pad Thai is Thailand's most famous export dish. Despite its popularity, it's actually a relatively modern creation.",
                ImageFileName = "butterchicken",
                PrepTimeMinutes = 25,
                Calories = 400,
                Origin = "Thailand",
                Rating = 4.6,
                Ingredients = new List<string> { "Rice noodles", "Shrimp or chicken", "Tamarind paste", "Fish sauce", "Lime", "Peanuts", "Bean sprouts", "Scallions", "Eggs" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 9,
                Name = "Chocolate Fondue",
                Category = "Dessert",
                Description = "Melted chocolate served with various dippers.",
                Details = "Chocolate fondue originated in Switzerland in the 1930s as a way to use up leftover chocolate.",
                ImageFileName = "chocolatelavacake",
                PrepTimeMinutes = 10,
                Calories = 450,
                Origin = "Switzerland",
                Rating = 4.5,
                Ingredients = new List<string> { "Dark chocolate", "Heavy cream", "Brandy or liqueur", "Fruit (strawberries, bananas)", "Marshmallows", "Pretzels", "Cookies" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 10,
                Name = "Greek Salad",
                Category = "Appetizer",
                Description = "Fresh salad with tomatoes, cucumber, olives, feta, and olive oil.",
                Details = "The Greek salad, or 'horiatiki', is a staple of Mediterranean cuisine.",
                ImageFileName = "bruschetta",
                PrepTimeMinutes = 10,
                Calories = 280,
                Origin = "Greece",
                Rating = 4.4,
                Ingredients = new List<string> { "Ripe tomatoes", "Cucumber", "Red onion", "Kalamata olives", "Feta cheese", "Extra virgin olive oil", "Oregano", "Lemon juice" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 11,
                Name = "Churros",
                Category = "Snack",
                Description = "Spanish fried dough pastry dusted with cinnamon sugar.",
                Details = "Churros have a long history, with origins in both Spain and Portugal. They were traditionally made by Spanish shepherds.",
                ImageFileName = "mangolassi",
                PrepTimeMinutes = 30,
                Calories = 180,
                Origin = "Spain",
                Rating = 4.6,
                Ingredients = new List<string> { "All-purpose flour", "Water", "Butter", "Salt", "Sugar", "Cinnamon", "Oil for frying" }
            });

            FoodItems.Add(new FoodItem
            {
                Id = 12,
                Name = "Mojito",
                Category = "Drink",
                Description = "Refreshing Cuban cocktail with mint, lime, rum, and sugar.",
                Details = "The mojito originated in Cuba in the early 20th century. Its name comes from the Spanish word 'mojadito' meaning 'little wet one'.",
                ImageFileName = "margheritapizza",
                PrepTimeMinutes = 5,
                Calories = 150,
                Origin = "Cuba",
                Rating = 4.5,
                Ingredients = new List<string> { "White rum", "Fresh mint leaves", "Lime wedges", "Sugar or simple syrup", "Club soda", "Ice" }
            });

            ApplyFilter();
        }

        private void LoadSavedFavorites()
        {
            try
            {
                string favoritesJson = Preferences.Get("favorites", "[]");
                var favoriteIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(favoritesJson) ?? new List<int>();

                foreach (var item in FoodItems)
                {
                    item.IsFavorite = favoriteIds.Contains(item.Id);
                }
            }
            catch
            {
                // Ignore errors loading favorites
            }
        }

        private void SaveFavorites()
        {
            var favoriteIds = FoodItems.Where(i => i.IsFavorite).Select(i => i.Id).ToList();
            string favoritesJson = System.Text.Json.JsonSerializer.Serialize(favoriteIds);
            Preferences.Set("favorites", favoritesJson);
        }

        private void ApplyFilter()
        {
            var filtered = FoodItems.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string search = SearchText.ToLower();
                filtered = filtered.Where(item =>
                    item.Name.ToLower().Contains(search) ||
                    item.Description.ToLower().Contains(search) ||
                    item.Origin.ToLower().Contains(search));
            }

            if (SelectedCategory != "All")
            {
                filtered = filtered.Where(item => item.Category == SelectedCategory);
            }

            if (ShowOnlyFavorites)
            {
                filtered = filtered.Where(item => item.IsFavorite);
            }

            FilteredItems.Clear();
            foreach (var item in filtered)
            {
                FilteredItems.Add(item);
            }
        }

        private void OnToggleFavorite(FoodItem? item)
        {
            if (item == null) return;

            item.IsFavorite = !item.IsFavorite;
            SaveFavorites();
            _hardwareService.Vibrate(30);

            string message = item.IsFavorite
                ? $"❤️ {item.Name} added to favorites!"
                : $"💔 {item.Name} removed from favorites.";

            ShowAlert(item.IsFavorite ? "Added to Favorites" : "Removed from Favorites", message);
            ApplyFilter();
        }

        private void OnSwipeFavorite(FoodItem? item)
        {
            if (item == null)
                return;

            if (item.IsFavorite)
            {
                ShowAlert("Already in Favorites", $"❤️ {item.Name} is already in your favorites!");
                _hardwareService.Vibrate(50);
                return;
            }

            item.IsFavorite = true;
            SaveFavorites();
            _hardwareService.Vibrate(30);
            ShowAlert("Added to Favorites", $"❤️ {item.Name} added to favorites!");
            ApplyFilter();
        }

        private void OnClearSearch()
        {
            SearchText = string.Empty;
        }

        private void OnSelectCategory(string? category)
        {
            SelectedCategory = category ?? "All";
        }

        private void OnToggleShowFavorites()
        {
            ShowOnlyFavorites = !ShowOnlyFavorites;
        }

        private void OnToggleMenu()
        {
            IsMenuOpen = !IsMenuOpen;
        }

        private void OnDeleteItem(FoodItem? item)
        {
            if (item == null) return;

            bool wasFavorite = item.IsFavorite;
            FoodItems.Remove(item);
            SaveFavorites();
            _hardwareService.Vibrate(50);
            ShowAlert("Deleted", $"🗑️ {item.Name} has been deleted.");
            ApplyFilter();
        }

        private async void OnShareItem(FoodItem? item)
        {
            if (item == null) return;

            string text = $"Check out this delicious {item.Category}: {item.Name}!\\n\\nOrigin: {item.Origin}\\nRating: {item.Rating}/5\\n\\nDescription: {item.Description}";

            try
            {
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = text,
                    Title = $"Share {item.Name}"
                });
            }
            catch
            {
                ShowAlert("Share Failed", "Unable to share at this time.");
            }
        }

        private void OnToggleAccelerometer()
        {
            IsMenuOpen = false;

            if (IsAccelerometerActive)
            {
                StopAccelerometerMonitoring();
                IsAccelerometerActive = false;
                AccelerometerX = 0;
                AccelerometerY = 0;
                AccelerometerZ = 0;
                ShowAlert("Accelerometer", "Accelerometer monitoring stopped.");
            }
            else
            {
                StartAccelerometerMonitoring();
                IsAccelerometerActive = true;
                ShowAlert("Accelerometer", "🎢 Accelerometer monitoring started! Shake your device for a surprise.");
            }
        }

        private void StartAccelerometerMonitoring()
        {
            try
            {
                _hardwareService.StartAccelerometer(OnAccelerometerReading);
                _isAccelerometerRunning = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to start accelerometer: {ex.Message}");
            }
        }

        private void StopAccelerometerMonitoring()
        {
            if (_isAccelerometerRunning)
            {
                _hardwareService.StopAccelerometer();
                _isAccelerometerRunning = false;
            }
        }

        private void OnAccelerometerReading(Microsoft.Maui.Devices.Sensors.AccelerometerData data)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                AccelerometerX = data.Acceleration.X;
                AccelerometerY = data.Acceleration.Y;
                AccelerometerZ = data.Acceleration.Z;

                double acceleration = Math.Abs(data.Acceleration.X) +
                                     Math.Abs(data.Acceleration.Y) +
                                     Math.Abs(data.Acceleration.Z);

                if (acceleration > ShakeThreshold)
                {
                    if ((DateTime.Now - _lastShakeTime).TotalMilliseconds > ShakeCooldownMilliseconds)
                    {
                        _lastShakeTime = DateTime.Now;
                        OnDeviceShaken();
                    }
                }
            });
        }

        private void OnDeviceShaken()
        {
            _hardwareService.Vibrate(50);

            if (FilteredItems.Any())
            {
                var randomIndex = Random.Shared.Next(FilteredItems.Count);
                var randomItem = FilteredItems[randomIndex];

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    ShowAlert("🎲 Shake to Discover", $"Today's recommendation: {randomItem.Name}");
                    await _hardwareService.SpeakAsync($"Today's recommendation is {randomItem.Name}, a delicious {randomItem.Category} from {randomItem.Origin}.");
                });
            }
        }

        private void OnVibrate()
        {
            IsMenuOpen = false;
            _hardwareService.Vibrate(100);
            ShowAlert("Vibration", "📳 Vibration test complete!");
        }

        private async void OnSpeakRecipe()
        {
            IsMenuOpen = false;

            if (FilteredItems.Any())
            {
                var item = FilteredItems.First();
                string text = $"Let me read you about {item.Name}. This is a {item.Category} from {item.Origin}. {item.Description}";
                await _hardwareService.SpeakAsync(text);
            }
            else
            {
                ShowAlert("No Items", "Please select a food item first.");
            }
        }

        private async void OnTakePhoto()
        {
            IsMenuOpen = false;

            try
            {
                var photo = await _hardwareService.TakePhotoAsync();
                if (photo != null)
                {
                    ShowAlert("Photo Taken", "📷 Photo captured successfully!");
                }
            }
            catch
            {
                ShowAlert("Camera Error", "Unable to access camera.");
            }
        }

        private async void OnGetNearbyLocation()
        {
            IsMenuOpen = false;

            try
            {
                var location = await _hardwareService.GetCurrentLocationAsync();
                if (location != null)
                {
                    ShowAlert("📍 Location Found",
                        $"Latitude: {location.Latitude:F4}\\nLongitude: {location.Longitude:F4}\\n\\nSearching for nearby restaurants...");
                }
                else
                {
                    ShowAlert("Location Unavailable", "Unable to get current location.");
                }
            }
            catch
            {
                ShowAlert("Location Error", "Unable to access location services.");
            }
        }

        private void ShowAlert(string title, string message)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await App.Current?.MainPage?.DisplayAlert(title, message, "OK");
            });
        }
    }
}
