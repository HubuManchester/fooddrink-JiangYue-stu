using FoodDrinkApp.Models;
using FoodDrinkApp.Services;
using FoodDrinkApp.ViewModels;

namespace FoodDrinkApp.Views
{
    public partial class FavoritesPage : ContentPage
    {
        public HomeViewModel ViewModel { get; }

        public FavoritesPage()
        {
            InitializeComponent();

            // Get the same singleton ViewModel from DI container
            ViewModel = IPlatformApplication.Current?.Services.GetService<HomeViewModel>()
                ?? new HomeViewModel(new HardwareService());

            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Always show only favorites when this page appears
            ViewModel.ShowOnlyFavorites = true;
            ViewModel.ApplyFilterCommand.Execute(null);
        }

        private void OnItemTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is FoodItem selectedItem)
            {
                Navigation.PushAsync(new FoodDetailPage(selectedItem));
            }
        }
    }
}