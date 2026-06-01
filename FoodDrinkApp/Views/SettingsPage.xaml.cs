using Microsoft.Maui.Storage;

namespace FoodDrinkApp.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            bool isDarkMode = Preferences.Get("dark_mode", false);
            DarkModeSwitch.IsToggled = isDarkMode;

            double savedFontSize = Preferences.Get("font_size", 1.0);
            FontSizeSlider.Value = savedFontSize;
            UpdateFontSizeLabel(savedFontSize);

            bool isHighContrast = Preferences.Get("high_contrast", false);
            HighContrastSwitch.IsToggled = isHighContrast;
        }

        private void OnDarkModeToggled(object sender, ToggledEventArgs e)
        {
            App.ToggleTheme(e.Value);
        }

        private void OnFontSizeChanged(object sender, ValueChangedEventArgs e)
        {
            double newSize = Math.Round(e.NewValue, 1);
            FontSizeSlider.Value = newSize;
            UpdateFontSizeLabel(newSize);
            Preferences.Set("font_size", newSize);
            App.UpdateFontSizes(newSize);
        }

        private void UpdateFontSizeLabel(double value)
        {
            string sizeText = value switch
            {
                <= 0.85 => "Small",
                <= 1.05 => "Medium",
                <= 1.25 => "Large",
                _ => "Extra Large"
            };
            FontSizeLabel.Text = sizeText;
        }

        private void OnHighContrastToggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("high_contrast", e.Value);
            App.ToggleHighContrastMode(e.Value);
        }
    }
}
