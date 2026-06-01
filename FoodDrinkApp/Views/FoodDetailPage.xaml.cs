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

        // ========== PINCH TO ZOOM ==========
        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (FoodImage == null)
                return;

            if (e.Status == GestureStatus.Started)
            {
                _startScale = _currentScale;
            }
            else if (e.Status == GestureStatus.Running)
            {
                _currentScale = _startScale * e.Scale;
                _currentScale = Math.Max(1.0, Math.Min(_currentScale, 5.0));
                
                FoodImage.Scale = _currentScale;
            }
            else if (e.Status == GestureStatus.Completed)
            {
                _hardwareService?.Vibrate(20);
                
                if (_currentScale <= 1.0)
                {
                    ResetImagePosition();
                }
            }
        }

        // ========== PAN TO MOVE ==========
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (FoodImage == null || _currentScale <= 1.0)
                return;

            if (e.StatusType == GestureStatus.Started)
            {
                _xOffset = _currentX;
                _yOffset = _currentY;
            }
            else if (e.StatusType == GestureStatus.Running)
            {
                var newX = _xOffset + e.TotalX;
                var newY = _yOffset + e.TotalY;
                
                var maxX = (FoodImage.Width * (_currentScale - 1)) / 2;
                var maxY = (FoodImage.Height * (_currentScale - 1)) / 2;
                
                _currentX = Math.Max(-maxX, Math.Min(newX, maxX));
                _currentY = Math.Max(-maxY, Math.Min(newY, maxY));
                
                FoodImage.TranslationX = _currentX;
                FoodImage.TranslationY = _currentY;
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                _hardwareService?.Vibrate(20);
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
