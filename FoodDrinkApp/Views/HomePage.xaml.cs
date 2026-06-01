using FoodDrinkApp.Models;
using FoodDrinkApp.Services;
using FoodDrinkApp.ViewModels;

namespace FoodDrinkApp.Views
{
    public partial class HomePage : ContentPage
    {
        public HomeViewModel ViewModel { get; }

        public HomePage()
        {
            InitializeComponent();

            ViewModel = IPlatformApplication.Current?.Services.GetService<HomeViewModel>()
                ?? new HomeViewModel(new HardwareService());

            BindingContext = ViewModel;
        }

        public HomePage(IHardwareService hardwareService)
        {
            InitializeComponent();
            ViewModel = IPlatformApplication.Current?.Services.GetService<HomeViewModel>()
                ?? new HomeViewModel(hardwareService);
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.ShowOnlyFavorites = false;
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