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

            // ★★★ 从依赖注入容器获取同一个单例 ViewModel ★★★
            ViewModel = IPlatformApplication.Current?.Services.GetService<HomeViewModel>()
                ?? new HomeViewModel(new HardwareService());

            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // 每次显示收藏页时，强制显示收藏
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