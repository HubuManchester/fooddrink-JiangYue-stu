using Microsoft.Maui.Storage;

namespace FoodDrinkApp.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            // Load saved settings
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Load dark mode preference
            bool isDarkMode = Preferences.Get("dark_mode", false);
            DarkModeSwitch.IsToggled = isDarkMode;
            Application.Current.UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;

            // Load font size preference
            double savedFontSize = Preferences.Get("font_size", 1.0);
            FontSizeSlider.Value = savedFontSize;
            UpdateFontSizeLabel(savedFontSize);
            ApplyFontSize(savedFontSize);
        }

        private void OnDarkModeToggled(object sender, ToggledEventArgs e)
        {
            // Apply dark/light theme
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;

            // Save preference
            Preferences.Set("dark_mode", e.Value);
        }

        private void OnFontSizeChanged(object sender, ValueChangedEventArgs e)
        {
            double newSize = Math.Round(e.NewValue, 1);
            FontSizeSlider.Value = newSize;

            // Update label
            UpdateFontSizeLabel(newSize);

            // Save preference
            Preferences.Set("font_size", newSize);

            // Apply font size globally
            ApplyFontSize(newSize);
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

        private void ApplyFontSize(double scale)
        {
            // Apply to all pages via a static property
            App.CurrentFontScale = scale;
        }
    }
}