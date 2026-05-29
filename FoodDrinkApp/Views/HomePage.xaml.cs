using FoodDrinkApp.Models;
using FoodDrinkApp.ViewModels;

namespace FoodDrinkApp.Views
{
    public partial class HomePage : ContentPage
    {
        public HomeViewModel ViewModel { get; }

        public HomePage()
        {
            InitializeComponent();
            ViewModel = new HomeViewModel();
            BindingContext = ViewModel;
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