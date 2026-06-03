using System.Windows.Input;

namespace FoodDrinkApp.Controls;

public partial class SearchBarControl : ContentView
{
    public static readonly BindableProperty SearchTextProperty =
        BindableProperty.Create(
            nameof(SearchText),
            typeof(string),
            typeof(SearchBarControl),
            string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnSearchTextChanged);

    public static readonly BindableProperty ClearCommandProperty =
        BindableProperty.Create(
            nameof(ClearCommand),
            typeof(ICommand),
            typeof(SearchBarControl));

    public static readonly BindableProperty SearchCommandProperty =
        BindableProperty.Create(
            nameof(SearchCommand),
            typeof(ICommand),
            typeof(SearchBarControl));

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(SearchBarControl),
            "Search...");

    public string SearchText
    {
        get => (string)GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }

    public ICommand ClearCommand
    {
        get => (ICommand)GetValue(ClearCommandProperty);
        set => SetValue(ClearCommandProperty, value);
    }

    public ICommand SearchCommand
    {
        get => (ICommand)GetValue(SearchCommandProperty);
        set => SetValue(SearchCommandProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public bool ShowClearButton => !string.IsNullOrEmpty(SearchText);

    public event EventHandler<string>? SearchCompleted;

    public SearchBarControl()
    {
        InitializeComponent();
    }

    private static void OnSearchTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SearchBarControl control)
        {
            control.OnPropertyChanged(nameof(ShowClearButton));
        }
    }

    private void OnSearchCompleted(object? sender, EventArgs e)
    {
        SearchCompleted?.Invoke(this, SearchText);
        SearchCommand?.Execute(SearchText);
    }

    private void OnClearClicked(object? sender, EventArgs e)
    {
        SearchText = string.Empty;
        ClearCommand?.Execute(null);
    }
}
