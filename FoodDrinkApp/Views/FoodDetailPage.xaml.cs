using FoodDrinkApp.Models;
using FoodDrinkApp.Services;

namespace FoodDrinkApp.Views
{
    public partial class FoodDetailPage : ContentPage
    {
        private readonly IHardwareService _hardwareService;
        private readonly FoodItem _foodItem;

        public FoodDetailPage(FoodItem foodItem)
        {
            InitializeComponent();
            _foodItem = foodItem;
            BindingContext = foodItem;

            // Get hardware service from DI
            _hardwareService = IPlatformApplication.Current?.Services?.GetService<IHardwareService>();

            // Build ingredients display
            BuildIngredients();
        }

        private void BuildIngredients()
        {
            // Clear existing children
            IngredientsContainer.Children.Clear();

            if (_foodItem?.Ingredients != null && _foodItem.Ingredients.Count > 0)
            {
                foreach (var ingredient in _foodItem.Ingredients)
                {
                    var chip = new Frame
                    {
                        CornerRadius = 20,
                        Padding = new Thickness(14, 8),
                        Margin = new Thickness(0, 0, 8, 8),
                        HasShadow = false,
                        BackgroundColor = Color.FromArgb("#FFFFFF"),
                        BorderColor = Color.FromArgb("#C8E6C9"),
                        Content = new Label
                        {
                            Text = $"•  {ingredient}",
                            FontSize = 14,
                            TextColor = Color.FromArgb("#2E7D32")
                        }
                    };
                    IngredientsContainer.Children.Add(chip);
                }
            }
            else
            {
                IngredientsContainer.Children.Add(new Label
                {
                    Text = "No ingredients listed",
                    FontSize = 14,
                    TextColor = Color.FromArgb("#9E9E9E")
                });
            }
        }

        // ========== HARDWARE FEATURE 1: CAMERA ==========
        private async void OnCameraClicked(object sender, EventArgs e)
        {
            try
            {
                _hardwareService?.Vibrate(100);

                if (_hardwareService == null)
                {
                    await DisplayAlert("Error", "Camera service not available", "OK");
                    return;
                }

                var photo = await _hardwareService.TakePhotoAsync();

                if (photo != null)
                {
                    await DisplayAlert("📷 Photo Captured",
                        $"Your photo has been saved!\n\nFile: {photo.FileName}",
                        "OK");
                    await _hardwareService.SpeakAsync("Photo captured successfully");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Camera Error", ex.Message, "OK");
            }
        }

        // ========== HARDWARE FEATURE 2: GPS LOCATION ==========
        private async void OnLocationClicked(object sender, EventArgs e)
        {
            try
            {
                _hardwareService?.Vibrate(150);

                if (_hardwareService == null)
                {
                    await DisplayAlert("Error", "Location service not available", "OK");
                    return;
                }

                var location = await _hardwareService.GetCurrentLocationAsync();

                if (location != null)
                {
                    var searchQuery = $"{_foodItem?.Origin} food near me";
                    var mapsUrl = $"https://www.google.com/maps/search/{Uri.EscapeDataString(searchQuery)}/@{location.Latitude},{location.Longitude},14z";

                    await DisplayAlert("📍 Location Found",
                        $"Finding {_foodItem?.Origin} restaurants near you...",
                        "OK");

                    await Launcher.Default.OpenAsync(mapsUrl);
                    await _hardwareService.SpeakAsync($"Finding {_foodItem?.Origin} restaurants near your location");
                }
                else
                {
                    await DisplayAlert("Location Error",
                        "Could not get current location.\n\nPlease check GPS and permissions.",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Location Error", ex.Message, "OK");
            }
        }

        // ========== HARDWARE FEATURE 3: TEXT-TO-SPEECH ==========
        private async void OnTTSClicked(object sender, EventArgs e)
        {
            try
            {
                _hardwareService?.Vibrate(100);

                if (_hardwareService == null)
                {
                    await DisplayAlert("Error", "Text-to-Speech service not available", "OK");
                    return;
                }

                if (_foodItem != null)
                {
                    var recipeText = $"{_foodItem.Name}. {_foodItem.Description}. " +
                        $"Preparation time: {_foodItem.PrepTimeMinutes} minutes. " +
                        $"Calories: {_foodItem.Calories}. Origin: {_foodItem.Origin}. " +
                        $"Rating: {_foodItem.Rating} out of 5. ";

                    if (_foodItem.Ingredients?.Count > 0)
                    {
                        recipeText += $"Ingredients: {string.Join(", ", _foodItem.Ingredients)}. ";
                    }

                    recipeText += _foodItem.Details;

                    await _hardwareService.SpeakAsync(recipeText);
                    await DisplayAlert("🔊 Reading Aloud",
                        $"Now reading the recipe for {_foodItem.Name}!",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("TTS Error", ex.Message, "OK");
            }
        }
    }
}