namespace FoodDrinkApp.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int PrepTimeMinutes { get; set; }
        public int Calories { get; set; }
        public bool IsFavorite { get; set; }
        public string Origin { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
        public double Rating { get; set; }
    }
}
