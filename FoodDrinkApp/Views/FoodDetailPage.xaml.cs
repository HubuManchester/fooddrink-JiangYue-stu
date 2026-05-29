using FoodDrinkApp.Models;

namespace FoodDrinkApp.Views
{
    public partial class FoodDetailPage : ContentPage
    {
        public FoodItem FoodItem { get; }

        public FoodDetailPage(FoodItem foodItem)
        {
            InitializeComponent();
            FoodItem = foodItem;
            BindingContext = foodItem;
            UpdateIngredients();
        }

        private void UpdateIngredients()
        {
            if (FoodItem?.Ingredients != null && FoodItem.Ingredients.Count > 0)
                IngredientsLabel.Text = "- " + string.Join("\n- ", FoodItem.Ingredients);
            else
                IngredientsLabel.Text = "No ingredients listed";
        }

        private async void OnCameraClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Camera", "Camera feature will be implemented with hardware integration.", "OK");
        }

        private async void OnLocationClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Location", "Location feature will be implemented with hardware integration.", "OK");
        }

        private async void OnTTSClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Text-to-Speech", "TTS feature will be implemented with hardware integration.", "OK");
        }
    }
}
