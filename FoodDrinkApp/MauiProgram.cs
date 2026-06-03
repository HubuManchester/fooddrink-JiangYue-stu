using FoodDrinkApp.Services;
using FoodDrinkApp.ViewModels;
using FoodDrinkApp.Views;
using Microsoft.Extensions.Logging;

namespace FoodDrinkApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register Hardware Service as Singleton
            builder.Services.AddSingleton<IHardwareService, HardwareService>();

            // Singleton ViewModel registration
            builder.Services.AddSingleton<HomeViewModel>();

            // Register Pages as Transient
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<FavoritesPage>();
            builder.Services.AddTransient<FoodDetailPage>();
            builder.Services.AddTransient<SettingsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}