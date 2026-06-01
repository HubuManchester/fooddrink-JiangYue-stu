using Microsoft.Maui.Storage;

namespace FoodDrinkApp
{
    public partial class App : Application
    {
        public static bool IsHighContrastMode { get; private set; }

        public App()
        {
            InitializeComponent();
            LoadSettings();
            MainPage = new AppShell();
        }

        private void LoadSettings()
        {
            bool isDarkMode = Preferences.Get("dark_mode", false);
            UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;

            double savedScale = Preferences.Get("font_size", 1.0);
            UpdateFontSizes(savedScale);

            IsHighContrastMode = Preferences.Get("high_contrast", false);
            UpdateThemeColors();
        }

        public static void UpdateFontSizes(double scale)
        {
            if (Current == null) return;

            var resources = Current.Resources;

            resources["FontSizeMicro"] = 10.0 * scale;
            resources["FontSizeCaption"] = 12.0 * scale;
            resources["FontSizeBody"] = 14.0 * scale;
            resources["FontSizeSubtitle"] = 16.0 * scale;
            resources["FontSizeTitle"] = 20.0 * scale;
            resources["FontSizeLarge"] = 28.0 * scale;
            resources["FontSizeHuge"] = 36.0 * scale;

            resources["ShellTitleFontSize"] = 18.0 * scale;
            resources["ShellTabFontSize"] = 12.0 * scale;
        }

        public static void ToggleTheme(bool isDark)
        {
            Current.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
            Preferences.Set("dark_mode", isDark);
            UpdateThemeColors();
        }

        public static void ToggleHighContrastMode(bool enable)
        {
            IsHighContrastMode = enable;
            Preferences.Set("high_contrast", enable);
            UpdateThemeColors();
        }

        private static void UpdateThemeColors()
        {
            if (Current == null) return;

            var resources = Current.Resources;

            if (IsHighContrastMode)
            {
                resources["BackgroundColor"] = resources["BackgroundHighContrast"];
                resources["CardColor"] = resources["CardHighContrast"];
                resources["TextPrimaryColor"] = resources["TextPrimaryHighContrast"];
                resources["TextSecondaryColor"] = resources["TextSecondaryHighContrast"];
                resources["BorderColor"] = resources["BorderHighContrast"];
                resources["CurrentPrimaryColor"] = resources["PrimaryHighContrast"];
            }
            else if (Current.UserAppTheme == AppTheme.Dark)
            {
                resources["BackgroundColor"] = resources["BackgroundDark"];
                resources["CardColor"] = resources["CardDark"];
                resources["TextPrimaryColor"] = resources["TextPrimaryDark"];
                resources["TextSecondaryColor"] = resources["TextSecondaryDark"];
                resources["BorderColor"] = resources["BorderDark"];
                resources["CurrentPrimaryColor"] = resources["PrimaryColor"];
            }
            else
            {
                resources["BackgroundColor"] = resources["BackgroundLight"];
                resources["CardColor"] = resources["CardLight"];
                resources["TextPrimaryColor"] = resources["TextPrimaryLight"];
                resources["TextSecondaryColor"] = resources["TextSecondaryLight"];
                resources["BorderColor"] = resources["BorderLight"];
                resources["CurrentPrimaryColor"] = resources["PrimaryColor"];
            }
        }
    }
}
