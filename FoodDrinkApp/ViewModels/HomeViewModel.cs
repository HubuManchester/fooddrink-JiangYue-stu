using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDrinkApp.Models;
using FoodDrinkApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Storage;

namespace FoodDrinkApp.ViewModels
{
    /// <summary>
    /// Home page ViewModel - Manages food list, filtering, favorites, and hardware features
    /// </summary>
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IHardwareService _hardwareService;

        // ========== BINDABLE PROPERTIES ==========

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

        // Status message properties for hardware operation feedback
        [ObservableProperty]
        private string _photoStatus = string.Empty;

        [ObservableProperty]
        private string _locationInfo = string.Empty;

        [ObservableProperty]
        private bool _hasStatusMessage;

        [ObservableProperty]
        private bool _hasLocationInfo;

        // Favorite filter toggle
        [ObservableProperty]
        private bool _showOnlyFavorites;

        // Floating Action Button (FAB) menu state
        [ObservableProperty]
        private bool _isMenuOpen;

        // ========== COMMANDS ==========

        // Hardware feature commands
        public ICommand VibrateCommand { get; }
        public ICommand SpeakRecipeCommand { get; }
        public ICommand TakePhotoCommand { get; }
        public ICommand GetNearbyLocationCommand { get; }

        // UI commands
        public ICommand ToggleFavoriteCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand ToggleShowFavoritesCommand { get; }
        public ICommand ToggleMenuCommand { get; }

        /// <summary>
        /// Command for category selection filtering
        /// </summary>
        public ICommand SelectCategoryCommand { get; }

        /// <summary>
        /// List of available categories for filtering
        /// </summary>
        public List<string> Categories { get; } = new()
        {
            "All", "Appetizer", "Main Course", "Dessert", "Drink", "Snack", "Breakfast"
        };

        // ========== CONSTRUCTOR ==========

        public HomeViewModel(IHardwareService hardwareService)
        {
            _hardwareService = hardwareService;

            // Initialize commands
            VibrateCommand = new Command(OnVibrate);
            SpeakRecipeCommand = new Command(OnSpeakRecipe);
            TakePhotoCommand = new Command(async () => await OnTakePhotoAsync());
            GetNearbyLocationCommand = new Command(async () => await OnGetNearbyLocationAsync());
            ToggleFavoriteCommand = new Command<FoodItem>(OnToggleFavorite);
            ClearSearchCommand = new Command(OnClearSearch);
            ToggleShowFavoritesCommand = new Command(OnToggleShowFavorites);
            ToggleMenuCommand = new Command(OnToggleMenu);
            SelectCategoryCommand = new Command<string>(OnSelectCategory);

            LoadSampleData();
            LoadSavedFavorites();
        }

        // ========== FLOATING ACTION BUTTON (FAB) MENU ==========

        /// <summary>
        /// Toggles the hardware feature floating menu visibility
        /// </summary>
        private void OnToggleMenu()
        {
            IsMenuOpen = !IsMenuOpen;
        }

        // ========== CATEGORY FILTERING ==========

        /// <summary>
        /// Handles category selection from UI filter buttons
        /// </summary>
        /// <param name="category">Selected category name</param>
        private void OnSelectCategory(string? category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                SelectedCategory = category;
                ApplyFilter();
            }
        }

        // ========== SAMPLE DATA ==========

        /// <summary>
        /// Loads sample food data with comprehensive nutritional and cultural information
        /// </summary>
        private void LoadSampleData()
        {
            var items = new List<FoodItem>
            {
                // ========== Appetizers ==========
                new FoodItem
                {
                    Id = 1,
                    Name = "Caesar Salad",
                    Category = "Appetizer",
                    Description = "Crisp romaine lettuce tossed in creamy homemade Caesar dressing with golden croutons",
                    Details = "Contrary to popular belief, Caesar salad was invented in Tijuana, Mexico by Italian immigrant Caesar Cardini in 1924. During a busy Fourth of July rush, Cardini threw together remaining ingredients in dramatic tableside fashion. The original recipe contains no anchovy fillets — the anchovy flavor comes from Worcestershire sauce.",
                    Origin = "United States",
                    PrepTimeMinutes = 15,
                    Calories = 150,
                    Rating = 4.3,
                    Ingredients = new List<string> { "Crisp romaine lettuce hearts", "Parmigiano-Reggiano cheese", "Homemade garlic croutons", "Egg yolk (coddled)", "Dijon mustard", "Fresh lemon juice", "Worcestershire sauce", "Extra virgin olive oil" },
                    ImageFileName = "caesarsalad.png",
                    IsFavorite = false
                },
                new FoodItem
                {
                    Id = 2,
                    Name = "Bruschetta",
                    Category = "Appetizer",
                    Description = "Toasted Italian bread topped with fresh tomatoes, basil, garlic, and balsamic glaze",
                    Details = "Bruschetta originated in central Italy during the 15th century as a way to test freshly pressed olive oil. Farmers would grill bread over coals and rub it with garlic before drizzling new oil on top. Today it's a beloved antipasto served at Italian tables worldwide.",
                    Origin = "Italy",
                    PrepTimeMinutes = 12,
                    Calories = 180,
                    Rating = 4.5,
                    Ingredients = new List<string> { "Crusty Italian bread", "Ripe Roma tomatoes", "Fresh basil", "Garlic cloves", "Balsamic vinegar", "Extra virgin olive oil", "Sea salt", "Black pepper" },
                    ImageFileName = "bruschetta.png",
                    IsFavorite = false
                },

                // ========== Main Courses ==========
                new FoodItem
                {
                    Id = 3,
                    Name = "Margherita Pizza",
                    Category = "Main Course",
                    Description = "Classic Neapolitan pizza with San Marzano tomatoes, fresh mozzarella, and garden basil",
                    Details = "The authentic Margherita pizza was created in 1889 by chef Raffaele Esposito in honor of Queen Margherita of Italy. The red tomatoes, white mozzarella, and green basil represent the Italian flag colors. This thin-crust pizza is baked in a wood-fired oven at 485°C for just 90 seconds, resulting in a perfectly crispy yet chewy crust.",
                    Origin = "Italy",
                    PrepTimeMinutes = 25,
                    Calories = 285,
                    Rating = 4.8,
                    Ingredients = new List<string> { "Pizza dough", "San Marzano tomato sauce", "Fresh mozzarella di bufala", "Fresh basil leaves", "Extra virgin olive oil", "Sea salt" },
                    ImageFileName = "margheritapizza.png",
                    IsFavorite = false
                },
                new FoodItem
                {
                    Id = 4,
                    Name = "Sushi Roll",
                    Category = "Main Course",
                    Description = "Fresh Japanese maki rolls with premium salmon, crisp cucumber, and creamy avocado",
                    Details = "Maki sushi originated in the Edo period of Japan as a way to preserve fish in fermented rice. Today's modern sushi rolls combine vinegared sushi rice with fresh seafood and vegetables, wrapped in crisp nori seaweed. Each roll is carefully sliced into 6-8 pieces using a sharp knife dipped in water for clean cuts.",
                    Origin = "Japan",
                    PrepTimeMinutes = 40,
                    Calories = 200,
                    Rating = 4.6,
                    Ingredients = new List<string> { "Sushi rice", "Nori seaweed sheets", "Fresh Atlantic salmon", "Japanese cucumber", "Ripe avocado", "Rice vinegar", "Wasabi", "Pickled ginger" },
                    ImageFileName = "sushiroll.png",
                    IsFavorite = false
                },
                new FoodItem
                {
                    Id = 5,
                    Name = "Street Tacos",
                    Category = "Main Course",
                    Description = "Authentic Mexican street tacos with seasoned carne asada, fresh cilantro, and zesty lime",
                    Details = "Mexican street tacos (tacos callejeros) are the heart of Mexico's vibrant street food culture. Served on small, warm corn tortillas, they're topped simply with diced onion, fresh cilantro, and a squeeze of lime. The secret is in the marinade: citrus juice, garlic, and spices that tenderize the meat.",
                    Origin = "Mexico",
                    PrepTimeMinutes = 30,
                    Calories = 220,
                    Rating = 4.7,
                    Ingredients = new List<string> { "Small corn tortillas", "Flank steak (carne asada)", "Fresh cilantro", "White onion (diced)", "Fresh lime wedges", "Salsa roja", "Guacamole", "Garlic and cumin marinade" },
                    ImageFileName = "tacos.png",
                    IsFavorite = false
                },
                new FoodItem
                {
                    Id = 6,
                    Name = "Butter Chicken",
                    Category = "Main Course",
                    Description = "Creamy Indian curry with tender tandoori chicken in a rich tomato-butter sauce",
                    Details = "Butter Chicken (Murgh Makhani) was invented in Delhi in the 1950s by Kundan Lal Gujral. To avoid wasting leftover tandoori chicken, he simmered it in a creamy tomato gravy with butter and cream, creating India's most beloved curry.",
                    Origin = "India",
                    PrepTimeMinutes = 45,
                    Calories = 420,
                    Rating = 4.9,
                    Ingredients = new List<string> { "Chicken thighs", "Greek yogurt", "Tomato puree", "Heavy cream", "Butter", "Garam masala", "Fenugreek leaves", "Garlic and ginger paste" },
                    ImageFileName = "butterchicken.png",
                    IsFavorite = false
                },

                // ========== Desserts ==========
                new FoodItem
                {
                    Id = 7,
                    Name = "Chocolate Lava Cake",
                    Category = "Dessert",
                    Description = "Decadent warm chocolate cake with an irresistibly gooey molten center",
                    Details = "This heavenly dessert was accidentally invented by French chef Jean-Georges Vongerichten in 1987 when he removed a chocolate sponge cake from the oven too early. The outer layer was perfectly baked while the center remained deliciously molten. Serve immediately with vanilla ice cream.",
                    Origin = "France",
                    PrepTimeMinutes = 20,
                    Calories = 350,
                    Rating = 4.9,
                    Ingredients = new List<string> { "Dark chocolate (70% cocoa)", "Unsalted butter", "Free-range eggs", "Caster sugar", "All-purpose flour", "Cocoa powder", "Vanilla extract", "Pinch of sea salt" },
                    ImageFileName = "chocolatelavacake.png",
                    IsFavorite = false
                },
                new FoodItem
                {
                    Id = 8,
                    Name = "Tiramisu",
                    Category = "Dessert",
                    Description = "Classic Italian layered dessert with espresso-soaked ladyfingers and mascarpone cream",
                    Details = "Tiramisu means 'pick me up' in Italian, referring to the energy boost from espresso and cocoa. Originating in Veneto in the 1960s, this no-bake dessert has become Italy's most iconic sweet treat worldwide.",
                    Origin = "Italy",
                    PrepTimeMinutes = 30,
                    Calories = 380,
                    Rating = 4.7,
                    Ingredients = new List<string> { "Ladyfinger biscuits", "Mascarpone cheese", "Fresh espresso", "Egg yolks", "Cocoa powder", "Marsala wine", "Sugar", "Dark chocolate shavings" },
                    ImageFileName = "tiramisu.png",
                    IsFavorite = false
                },

                // ========== Drinks ==========
                new FoodItem
                {
                    Id = 9,
                    Name = "Matcha Latte",
                    Category = "Drink",
                    Description = "Vibrant Japanese green tea latte with ceremonial grade matcha and silky steamed milk",
                    Details = "Matcha has been central to Japanese tea ceremonies since the 12th century when Zen monks discovered its meditative properties. Unlike regular green tea, matcha involves consuming the entire shade-grown leaf in powdered form, providing 10x the antioxidants.",
                    Origin = "Japan",
                    PrepTimeMinutes = 10,
                    Calories = 120,
                    Rating = 4.5,
                    Ingredients = new List<string> { "Ceremonial grade matcha powder", "Steamed whole milk (or oat milk)", "Hot water (80°C)", "Honey or simple syrup", "Bamboo whisk (chasen)" },
                    ImageFileName = "matchalatte.png",
                    IsFavorite = false
                },
                new FoodItem
                {
                    Id = 10,
                    Name = "Mango Lassi",
                    Category = "Drink",
                    Description = "Refreshing Indian yogurt drink blended with sweet Alphonso mangoes and cardamom",
                    Details = "Lassi originated in Punjab, India as a cooling summer beverage. The addition of mango creates a tropical twist on the classic yogurt drink. Best served chilled in a copper cup for authentic Indian restaurant experience.",
                    Origin = "India",
                    PrepTimeMinutes = 8,
                    Calories = 180,
                    Rating = 4.6,
                    Ingredients = new List<string> { "Alphonso mango pulp", "Plain yogurt", "Cold milk", "Sugar", "Ground cardamom", "Ice cubes", "Saffron strands", "Chopped pistachios" },
                    ImageFileName = "mangolassi.png",
                    IsFavorite = false
                },

                // ========== Snacks ==========
                new FoodItem
                {
                    Id = 11,
                    Name = "Spring Rolls",
                    Category = "Snack",
                    Description = "Crispy golden Vietnamese spring rolls filled with pork, shrimp, and vermicelli noodles",
                    Details = "Vietnamese spring rolls (Chả giò) are a staple of Vietnamese cuisine. Unlike Chinese spring rolls, they use rice paper wrappers which fry up extra crispy and bubbly. Served with sweet chili fish sauce (nước chấm) for dipping.",
                    Origin = "Vietnam",
                    PrepTimeMinutes = 35,
                    Calories = 150,
                    Rating = 4.4,
                    Ingredients = new List<string> { "Rice paper wrappers", "Ground pork", "Shrimp", "Vermicelli noodles", "Wood ear mushrooms", "Carrots", "Bean sprouts", "Fish sauce" },
                    ImageFileName = "springrolls.png",
                    IsFavorite = false
                },
                new FoodItem
                {
                    Id = 12,
                    Name = "French Fries",
                    Category = "Snack",
                    Description = "Double-fried Belgian style golden fries, crispy outside and fluffy inside",
                    Details = "Despite the name, French fries likely originated in Belgium in the late 1600s. The secret to perfect fries is double frying: first at a lower temperature to cook through, then at high heat for that golden crispy exterior.",
                    Origin = "Belgium",
                    PrepTimeMinutes = 35,
                    Calories = 320,
                    Rating = 4.2,
                    Ingredients = new List<string> { "Russet potatoes", "Vegetable oil", "Sea salt", "Truffle oil", "Parmesan cheese", "Fresh parsley", "Garlic powder", "Truffle mayonnaise" },
                    ImageFileName = "frenchfries.png",
                    IsFavorite = false
                }
            };
            FoodItems = new ObservableCollection<FoodItem>(items);
            FilteredItems = new ObservableCollection<FoodItem>(items);
        }

        // ========== FAVORITE FUNCTIONS ==========

        /// <summary>
        /// Loads saved favorite food IDs from device preferences
        /// </summary>
        private void LoadSavedFavorites()
        {
            var favoriteIdsString = Preferences.Get("favorite_ids", "");
            if (!string.IsNullOrEmpty(favoriteIdsString))
            {
                var favoriteIds = favoriteIdsString.Split(',').Select(int.Parse).ToList();
                foreach (var item in FoodItems)
                {
                    item.IsFavorite = favoriteIds.Contains(item.Id);
                }
            }
            else
            {
                // Ensure all items have IsFavorite = false when no favorites saved
                foreach (var item in FoodItems)
                {
                    item.IsFavorite = false;
                }
            }
            ApplyFilter();
        }

        /// <summary>
        /// Persists favorite food IDs to device preferences
        /// </summary>
        private void SaveFavorites()
        {
            var favoriteIds = string.Join(",", FoodItems.Where(x => x.IsFavorite).Select(x => x.Id));
            Preferences.Set("favorite_ids", favoriteIds);
            System.Diagnostics.Debug.WriteLine($"Saved favorites: {favoriteIds}");
        }

        /// <summary>
        /// Toggles favorite status for a food item
        /// </summary>
        /// <param name="item">The food item to toggle favorite status for</param>
        private void OnToggleFavorite(FoodItem? item)
        {
            if (item == null)
            {
                ShowAlert("Error", "No food item selected to favorite.");
                return;
            }

            item.IsFavorite = !item.IsFavorite;
            SaveFavorites();

            string message = item.IsFavorite
                ? $"❤️ {item.Name} added to favorites!"
                : $"💔 {item.Name} removed from favorites.";

            ShowAlert("Favorite Updated", message);
            ApplyFilter();
        }

        /// <summary>
        /// Toggles the "show only favorites" filter
        /// </summary>
        private void OnToggleShowFavorites()
        {
            ShowOnlyFavorites = !ShowOnlyFavorites;
            ApplyFilter();

            string message = ShowOnlyFavorites
                ? "Showing only your favorite items."
                : "Showing all items.";
            ShowAlert("Filter Updated", message);
        }

        /// <summary>
        /// Clears the search text and resets the view
        /// </summary>
        private void OnClearSearch()
        {
            SearchText = string.Empty;
            ShowAlert("Search Cleared", "Search text has been cleared. Showing all items.");
        }

        // ========== FILTER METHODS ==========

        /// <summary>
        /// Applies category, search text, and favorites filters to the food list
        /// </summary>
        [RelayCommand]
        private void ApplyFilter()
        {
            var filtered = FoodItems.AsEnumerable();

            // Apply category filter
            if (SelectedCategory != "All")
                filtered = filtered.Where(item => item.Category == SelectedCategory);

            // Apply search text filter
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                filtered = filtered.Where(item =>
                    item.Name.ToLower().Contains(searchLower) ||
                    item.Description.ToLower().Contains(searchLower) ||
                    item.Origin.ToLower().Contains(searchLower));
            }

            // Apply favorites filter
            if (ShowOnlyFavorites)
            {
                filtered = filtered.Where(item => item.IsFavorite);

                // Show friendly message if no favorites
                if (!filtered.Any())
                {
                    ShowAlert("No Favorites", "You haven't added any favorites yet. Tap the heart icon on any food item to add it.");
                }
            }

            FilteredItems = new ObservableCollection<FoodItem>(filtered);
        }

        /// <summary>
        /// Clears all active filters (search, category, favorites)
        /// </summary>
        [RelayCommand]
        private void ClearFilters()
        {
            SearchText = string.Empty;
            SelectedCategory = "All";
            ShowOnlyFavorites = false;
            ApplyFilter();
            ShowAlert("Filters Cleared", "All filters have been cleared.");
        }

        /// <summary>
        /// Refreshes the food data and reloads favorites
        /// </summary>
        [RelayCommand]
        private async Task RefreshData()
        {
            IsLoading = true;
            await Task.Delay(500);
            LoadSampleData();
            LoadSavedFavorites();
            IsLoading = false;
            ShowAlert("Refreshed", "Food list has been refreshed.");
        }

        /// <summary>
        /// Automatically applies filters when search text changes
        /// </summary>
        partial void OnSearchTextChanged(string value) => ApplyFilter();

        /// <summary>
        /// Automatically applies filters when selected category changes
        /// </summary>
        partial void OnSelectedCategoryChanged(string value) => ApplyFilter();

        // ========== HELPER ==========

        /// <summary>
        /// Displays a user-friendly alert dialog
        /// </summary>
        /// <param name="title">Alert title</param>
        /// <param name="message">Alert message</param>
        private async void ShowAlert(string title, string message)
        {
            try
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(title, message, "OK");
                }
            }
            catch
            {
                // Silently fail if alert cannot be shown
            }
        }

        // ========== HARDWARE FEATURE 1: VIBRATION ==========

        /// <summary>
        /// Triggers device vibration for haptic feedback
        /// </summary>
        private void OnVibrate()
        {
            try
            {
                _hardwareService.Vibrate(100);
                PhotoStatus = "✨ Vibration triggered!";
                HasStatusMessage = true;

                // Auto-clear status after 2 seconds
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (PhotoStatus == "✨ Vibration triggered!")
                            HasStatusMessage = false;
                    });
                });
            }
            catch
            {
                PhotoStatus = "❌ Vibration not supported on this device.";
                HasStatusMessage = true;
                ShowAlert("Vibration Error", "Your device does not support vibration, or vibration permissions are not granted.");
            }
        }

        // ========== HARDWARE FEATURE 2: TEXT-TO-SPEECH ==========

        /// <summary>
        /// Reads the selected recipe aloud using text-to-speech
        /// </summary>
        private async void OnSpeakRecipe()
        {
            try
            {
                // Close FAB menu when action is executed
                IsMenuOpen = false;

                if (SelectedItem == null)
                {
                    await _hardwareService.SpeakAsync("Please select a food item from the list first.");
                    ShowAlert("No Selection", "Please tap on a food item first before using the Read Recipe feature.");
                    PhotoStatus = "⚠️ Please select a food item first";
                    HasStatusMessage = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedItem.Name))
                {
                    await _hardwareService.SpeakAsync("This recipe does not have a valid name.");
                    ShowAlert("Invalid Recipe", "This recipe information is incomplete.");
                    return;
                }

                string message = $"{SelectedItem.Name}. {SelectedItem.Description}";
                await _hardwareService.SpeakAsync(message);
                PhotoStatus = $"🔊 Reading aloud: {SelectedItem.Name}";
                HasStatusMessage = true;
            }
            catch
            {
                PhotoStatus = "❌ Text-to-Speech failed. Please check your device settings.";
                HasStatusMessage = true;
                ShowAlert("TTS Error", "Unable to read aloud. Text-to-Speech may not be available on this device.");
            }
        }

        // ========== HARDWARE FEATURE 3: CAMERA ==========

        /// <summary>
        /// Opens the device camera to take a photo
        /// </summary>
        private async Task OnTakePhotoAsync()
        {
            try
            {
                // Close FAB menu when action is executed
                IsMenuOpen = false;

                if (_hardwareService == null)
                {
                    ShowAlert("Camera Error", "Camera service is not available.");
                    return;
                }

                var photo = await _hardwareService.TakePhotoAsync();

                if (photo != null)
                {
                    PhotoStatus = "📷 Photo captured successfully!";
                    HasStatusMessage = true;
                    await _hardwareService.SpeakAsync("Photo captured successfully");
                    ShowAlert("Photo Captured", $"Your photo has been saved. Filename: {photo.FileName}");
                }
                else
                {
                    PhotoStatus = "📷 Photo capture cancelled.";
                    HasStatusMessage = true;
                    ShowAlert("Cancelled", "Photo capture was cancelled. Please try again if you'd like to take a photo.");
                }
            }
            catch (UnauthorizedAccessException)
            {
                PhotoStatus = "❌ Camera permission denied.";
                HasStatusMessage = true;
                ShowAlert("Permission Required", "Camera access is required to take photos. Please grant camera permission in device settings.");
            }
            catch
            {
                PhotoStatus = "❌ Camera error. Please check device camera.";
                HasStatusMessage = true;
                ShowAlert("Camera Error", "Unable to access camera. Please make sure your device has a working camera.");
            }
        }

        // ========== HARDWARE FEATURE 4: GPS LOCATION ==========

        /// <summary>
        /// Retrieves the device's current GPS location
        /// </summary>
        private async Task OnGetNearbyLocationAsync()
        {
            try
            {
                // Close FAB menu when action is executed
                IsMenuOpen = false;

                if (_hardwareService == null)
                {
                    ShowAlert("Location Error", "Location service is not available.");
                    return;
                }

                PhotoStatus = "📍 Getting your location...";
                HasStatusMessage = true;

                var location = await _hardwareService.GetCurrentLocationAsync();

                if (location != null)
                {
                    LocationInfo = $"📍 Latitude: {location.Latitude:F4}, Longitude: {location.Longitude:F4}";
                    HasLocationInfo = true;
                    PhotoStatus = "📍 Location retrieved! Use this for nearby restaurant search";
                    HasStatusMessage = true;

                    await _hardwareService.SpeakAsync($"Your current location is latitude {location.Latitude:F1}, longitude {location.Longitude:F1}");
                    ShowAlert("Location Found", $"Your coordinates:\nLatitude: {location.Latitude:F4}\nLongitude: {location.Longitude:F4}\n\nUse this for finding nearby restaurants.");
                }
                else
                {
                    LocationInfo = "❌ Unable to get location.";
                    HasLocationInfo = true;
                    PhotoStatus = "❌ GPS location failed. Please check permissions and GPS signal.";
                    HasStatusMessage = true;
                    ShowAlert("Location Unavailable", "Could not get your current location.\n\nPlease check:\n• Location permissions are granted\n• GPS is enabled on your device\n• You are in an area with good GPS signal");
                }
            }
            catch (PermissionException)
            {
                LocationInfo = "❌ Location permission denied.";
                HasLocationInfo = true;
                PhotoStatus = "❌ Please grant location permission in device settings.";
                HasStatusMessage = true;
                ShowAlert("Permission Required", "Location access is required to find nearby restaurants. Please grant location permission in your device settings.");
            }
            catch (FeatureNotSupportedException)
            {
                LocationInfo = "❌ GPS not supported on this device.";
                HasLocationInfo = true;
                ShowAlert("Not Supported", "Your device does not support GPS location services.");
            }
            catch
            {
                LocationInfo = "❌ Location error occurred.";
                HasLocationInfo = true;
                PhotoStatus = "❌ Unexpected error occurred.";
                HasStatusMessage = true;
                ShowAlert("Location Error", "An unexpected error occurred while trying to get your location. Please try again.");
            }
        }
    }
} 