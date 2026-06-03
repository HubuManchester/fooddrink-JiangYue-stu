namespace FoodDrinkApp.Controls;

public partial class LoadingControl : ContentView
{
    public static readonly BindableProperty IsLoadingProperty =
        BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(LoadingControl),
            false);

    public static readonly BindableProperty LoadingTextProperty =
        BindableProperty.Create(
            nameof(LoadingText),
            typeof(string),
            typeof(LoadingControl),
            "Loading...");

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public string LoadingText
    {
        get => (string)GetValue(LoadingTextProperty);
        set => SetValue(LoadingTextProperty, value);
    }

    public LoadingControl()
    {
        InitializeComponent();
    }
}
