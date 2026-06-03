using FoodDrinkApp.Models;
using FoodDrinkApp.Services;

namespace FoodDrinkApp.Views
{
    public partial class FoodDetailPage : ContentPage
    {
        private readonly IHardwareService? _hardwareService;
        private readonly FoodItem _foodItem;
        
        private double _currentScale = 1.0;
        private double _startScale = 1.0;
        private double _xOffset = 0;
        private double _yOffset = 0;
        private double _currentX = 0;
        private double _currentY = 0;

        public FoodDetailPage(FoodItem foodItem)
        {
            InitializeComponent();
            _foodItem = foodItem;
            BindingContext = new FoodDetailViewModel(foodItem);
            _hardwareService = IPlatformApplication.Current?.Services?.GetService<IHardwareService>() ?? new HardwareService();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        // ========== DOUBLE TAP TO ZOOM ==========
        private void OnDoubleTapped(object sender, TappedEventArgs e)
        {
            if (FoodImage == null)
                return;

            _hardwareService?.Vibrate(20);

            if (_currentScale > 1.0)
            {
                // Zoom out
                ResetImagePosition();
            }
            else
            {
                // Zoom in to 2.5x
                _currentScale = 2.5;
                FoodImage.Scale = _currentScale;
                FoodImage.AnchorX = 0.5;
                FoodImage.AnchorY = 0.5;
            }
        }

        private void ResetImagePosition()
        {
            _currentScale = 1.0;
            _currentX = 0;
            _currentY = 0;
            
            FoodImage.Scale = 1.0;
            FoodImage.TranslationX = 0;
            FoodImage.TranslationY = 0;
        }

        // ========== HARDWARE FEATURE 1: CAMERA ==========
        /// <summary>
        /// Captures a photo using the native camera API.
        /// </summary>
        /// <returns>The captured photo as FileResult, or null if cancelled.</returns>
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
        private async void OnSpeakClicked(object sender, EventArgs e)
        {
            try
            {
                _hardwareService?.Vibrate(100);

                if (_hardwareService == null)
                {
                    await DisplayAlert("Error", "Text-to-Speech service not available", "OK");
                    return;
                }

                // If currently speaking, stop
                if (_hardwareService.IsSpeaking)
                {
                    _hardwareService.StopSpeaking();
                    await DisplayAlert("🔇 Stopped", "Text-to-speech has been stopped.", "OK");
                    return;
                }

                // Otherwise, start speaking
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

        // ========== HARDWARE FEATURE 4: FLASHLIGHT ==========
        private bool _isFlashlightOn = false;
        private async void OnFlashlightClicked(object sender, EventArgs e)
        {
            try
            {
                _hardwareService?.Vibrate(50);

                if (_hardwareService == null)
                {
                    await DisplayAlert("Error", "Flashlight service not available", "OK");
                    return;
                }

                _isFlashlightOn = !_isFlashlightOn;

                if (_isFlashlightOn)
                {
                    await _hardwareService.TurnOnFlashlightAsync();
                    await DisplayAlert("🔦 Flashlight On", 
                        "Flashlight is now ON. Useful for cooking in low light!", 
                        "OK");
                }
                else
                {
                    await _hardwareService.TurnOffFlashlightAsync();
                    await DisplayAlert("🔦 Flashlight Off", 
                        "Flashlight is now OFF.", 
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Flashlight Error", ex.Message, "OK");
            }
        }
    }

    public class FoodDetailViewModel
    {
        public FoodItem FoodItem { get; }
        public double ZoomLevel => 1.0;

        public FoodDetailViewModel(FoodItem foodItem)
        {
            FoodItem = foodItem;
        }
    }
}
