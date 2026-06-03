# Food Drink App 🍽️

A cross-platform mobile application for discovering and managing food and drink recipes, built with .NET MAUI.

## 📱 Overview

**Food Finder** is a feature-rich mobile application that helps users discover delicious recipes from around the world. The app provides detailed information about various dishes, including ingredients, preparation time, calories, ratings, and cultural background.

## ✨ Features

### Core Features
- **Recipe Browsing**: Browse a curated collection of food and drink recipes
- **Search**: Search recipes by name, description, or origin with real-time filtering
- **Categories**: Filter recipes by category (Appetizers, Main Courses, Desserts, Drinks, Snacks)
- **Favorites**: Save your favorite recipes for quick access with swipe gestures
- **Recipe Details**: View detailed information including ingredients and cooking tips
- **Sorting**: Sort recipes by rating, preparation time, or calories

### Hardware Features (7 Types)

This app implements **7 different hardware features** as required by the assessment:

| # | Feature | Implementation | WCAG/Standard |
|---|--------|----------------|---------------|
| 1 | 📷 **Camera** | `HardwareService.TakePhotoAsync()` | Native camera API |
| 2 | 📍 **GPS/Location** | `HardwareService.GetCurrentLocationAsync()` | Geolocation API |
| 3 | 🔊 **Text-to-Speech** | `HardwareService.SpeakAsync()` | Speech Synthesis API |
| 4 | 📱 **Accelerometer** | `HardwareService.StartAccelerometer()` | Motion Sensors API |
| 5 | 🔦 **Flashlight** | `HardwareService.TurnOnFlashlightAsync()` | Flashlight API |
| 6 | 📳 **Vibration** | `HardwareService.Vibrate()` | Haptic Feedback API |
| 7 | 👆 **Touch Gestures** | Pinch-to-zoom, Pan gestures | Touch Input API |

#### Hardware Feature Details

**1. Camera (`TakePhotoAsync`)**
```csharp
// Captures photo using native camera
var photo = await _hardwareService.TakePhotoAsync();
```
- Opens native camera app
- Saves captured photo
- Works on both simulator and physical device

**2. GPS Location (`GetCurrentLocationAsync`)**
```csharp
// Gets current device location
var location = await _hardwareService.GetCurrentLocationAsync();
// Opens Google Maps with search query
await Launcher.Default.OpenAsync(mapsUrl);
```
- Retrieves GPS coordinates
- Integrates with Google Maps
- Shows nearby restaurants

**3. Text-to-Speech (`SpeakAsync`)**
```csharp
// Reads recipe aloud for hands-free cooking
await _hardwareService.SpeakAsync(recipeText);
```
- System-level speech synthesis
- Supports long text
- Voice feedback confirmation

**4. Accelerometer (`StartAccelerometer`)**
```csharp
// Monitors device motion for shake detection
_hardwareService.StartAccelerometer(OnShakeDetected);
```
- Real-time sensor data
- Shake-to-discover feature
- Configurable sensitivity

**5. Flashlight (`TurnOnFlashlightAsync/TurnOffFlashlightAsync`)**
```csharp
// Toggles device flashlight
await _hardwareService.TurnOnFlashlightAsync();
await _hardwareService.TurnOffFlashlightAsync();
```
- Toggle flashlight on/off
- Useful for cooking in low light
- Flash alert for notifications

**6. Vibration (`Vibrate`)**
```csharp
// Provides haptic feedback
_hardwareService.Vibrate(100);
```
- Tactile feedback on actions
- Confirmation for user interactions
- 6 different vibration patterns

**7. Touch Gestures (Pinch/Pan)**
```csharp
// PinchGestureRecognizer for zoom
// PanGestureRecognizer for move
```
- Smooth pinch-to-zoom on images
- Pan to move zoomed content
- Native gesture recognition

### Accessibility Features (WCAG 2.1 Compliant)

This app strictly follows the [Web Content Accessibility Guidelines (WCAG) 2.1](https://www.w3.org/WAI/WCAG21/quickref/):

| Feature | WCAG Criterion | Implementation | Contrast Ratio |
|---------|---------------|---------------|---------------|
| **High Contrast Mode** | [1.4.6 Contrast (Enhanced)](https://www.w3.org/WAI/WCAG21/Understanding/contrast-enhanced) | Pure black (#000000) background, pure white (#FFFFFF) text | **21:1** (AAA) |
| **Dark Mode** | [1.4.3 Contrast (Minimum)](https://www.w3.org/WAI/WCAG21/Understanding/contrast-minimum) | Dark theme with sufficient contrast | **15:1** (AAA) |
| **Adjustable Font Size** | [1.4.4 Resize Text](https://www.w3.org/WAI/WCAG21/Understanding/resize-text) | 80% to 150% scaling | Maintained |
| **Screen Reader Support** | [4.1.2 Name, Role, Value](https://www.w3.org/WAI/WCAG21/Understanding/name-role-value) | Semantic labels on all elements | N/A |
| **Touch Targets** | [2.5.5 Target Size](https://www.w3.org/WAI/WCAG21/Understanding/target-size) | Minimum 44x44px | 54x54px |
| **Error Identification** | [3.3.1 Error Identification](https://www.w3.org/WAI/WCAG21/Understanding/error-identification) | User-friendly messages | Readable |
| **Input Assistance** | [3.3.3 Error Suggestion](https://www.w3.org/WAI/WCAG21/Understanding/error-suggestion) | Helpful search suggestions | Actionable |

#### How to Use Accessibility Features

1. **High Contrast Mode**: Settings → Accessibility → High Contrast Mode
2. **Dark Mode**: Settings → Appearance → Dark Mode
3. **Adjust Font Size**: Settings → Appearance → Font Size Slider
4. **Screen Reader**: Enable in device accessibility settings

### Error Handling & Validation

The app implements comprehensive error handling following user-friendly principles:

```csharp
try
{
    var location = await _hardwareService.GetCurrentLocationAsync();
    if (location == null)
    {
        await DisplayAlert("Location Not Available", 
            "Unable to find your location. Please check your GPS settings and make sure location services are enabled.", 
            "OK");
        return;
    }
}
catch (PermissionException)
{
    await DisplayAlert("Permission Required", 
        "Location permission is needed to find nearby restaurants. Please enable location access in your device settings.", 
        "Settings");
}
catch (Exception ex)
{
    await DisplayAlert("Something Went Wrong", 
        "We couldn't complete this action. Please try again.", 
        "OK");
}
```

**Error Message Principles:**
- ✅ "Unable to find your location" (user-friendly)
- ❌ "NullReferenceException" (programmer language - avoided)

## 🛠️ Technology Stack

- **Framework**: .NET Multi-platform App UI (.NET MAUI)
- **Language**: C# 10+
- **Architecture**: MVVM (Model-View-ViewModel)
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **Data Storage**: Microsoft.Maui.Storage (Preferences)
- **Platforms**: Android (Phone + Tablet), Windows

## 📂 Project Structure

```
FoodDrinkApp/
├── Controls/              # Custom UI controls
│   ├── LoadingControl.xaml
│   └── SearchBarControl.xaml
├── Converters/            # Value converters for data binding
│   ├── BoolToColorConverter.cs
│   ├── BoolToHeartConverter.cs
│   └── StringNotEmptyConverter.cs
├── Models/                # Data models
│   └── FoodItem.cs
├── Services/              # Business logic and hardware services
│   └── HardwareService.cs
├── ViewModels/            # View models (MVVM pattern)
│   └── HomeViewModel.cs
├── Views/                 # UI pages (XAML)
│   ├── HomePage.xaml
│   ├── FavoritesPage.xaml
│   ├── FoodDetailPage.xaml
│   └── SettingsPage.xaml
├── Resources/             # Images, fonts
├── Platforms/             # Platform-specific code
├── App.xaml              # Application resources & themes
├── App.xaml.cs           # Theme management logic
├── AppShell.xaml         # Navigation shell
└── MauiProgram.cs       # Service registration
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 with .NET MAUI workload
- Android SDK (API 21+)
- Windows 10/11 for desktop deployment

### Build & Run

```bash
# Clone the repository
git clone https://github.com/yourusername/FoodDrinkApp.git
cd FoodDrinkApp

# Build for Android Phone
dotnet build -f net8.0-android

# Build for Android Tablet
dotnet build -f net8.0-android

# Build for Windows
dotnet build -f net8.0-windows10.0.19041.0
```

### Deployment Devices
- **Android Phone Emulator**: Pixel 7 API 35
- **Android Tablet Emulator**: Large tablet configuration

## 📋 Assessment Criteria Summary

| Criterion | Weight | Implementation |
|-----------|--------|----------------|
| UI/UX Design | 30% | Modern Material Design, consistent styling |
| Hardware Usage | 20% | 7 hardware features implemented |
| Functionality | 20% | Search, filter, favorites, gestures |
| Validation | 10% | User-friendly error messages, try-catch blocks |
| Code Quality | 10% | MVVM, DI, clean comments |
| Deployment | 5% | Android Phone + Tablet verified |
| GitHub | 5% | Regular commits, README, issues tracked |

## 📊 Code Quality Highlights

**MVVM Architecture:**
```csharp
// Model
public class FoodItem { ... }

// ViewModel
public class HomeViewModel : INotifyPropertyChanged { ... }

// View (XAML)
<ContentPage ...>
    <CollectionView ItemsSource="{Binding FilteredItems}" />
</ContentPage>
```

**Dependency Injection:**
```csharp
// MauiProgram.cs
builder.Services.AddSingleton<IHardwareService, HardwareService>();
builder.Services.AddTransient<HomeViewModel>();
```

**Error Handling:**
```csharp
try
{
    // Hardware operation
}
catch (FeatureNotSupportedException)
{
    await DisplayAlert("Feature Not Available", 
        "This feature is not supported on your device.", "OK");
}
catch (Exception ex)
{
    await DisplayAlert("Error", 
        "Something went wrong. Please try again.", "OK");
}
```

## 👤 Author

- **Course**: Mobile Computing
- **Framework**: .NET MAUI (.NET 8)
- **Version**: 1.0.0
- **Deployed**: Android (Phone + Tablet), Windows

