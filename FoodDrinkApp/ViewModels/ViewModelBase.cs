using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FoodDrinkApp.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels providing INotifyPropertyChanged implementation.
    /// This eliminates code duplication across ViewModels.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event raised when a property value changes.
        /// Used by data binding to update the UI.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// This method is called automatically by property setters.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that changed.
        /// This is automatically provided by [CallerMemberName], so callers don't need to specify it.
        /// </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the property value and raises PropertyChanged if the value has changed.
        /// This helper method reduces boilerplate code in property setters.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="field">Reference to the backing field.</param>
        /// <param name="value">The new value to set.</param>
        /// <param name="propertyName">The name of the property (automatically provided).</param>
        /// <returns>True if the value was changed, false otherwise.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
