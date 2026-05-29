namespace FoodDrinkApp
{
    public partial class App : Application
    {
        // Global font scale property
        public static double CurrentFontScale { get; set; } = 1.0;

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}