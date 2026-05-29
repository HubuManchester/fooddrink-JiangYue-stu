using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Android;

namespace FoodDrinkApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        // Required permissions for hardware features
        private readonly string[] _requiredPermissions = new[]
        {
            Manifest.Permission.Camera,           // Changed: android.Manifest.Permission.Camera -> Manifest.Permission.Camera
            Manifest.Permission.AccessFineLocation,   // Changed
            Manifest.Permission.AccessCoarseLocation, // Changed
            Manifest.Permission.Vibrate                // Changed
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Request permissions at runtime for Android 6.0+ (API 23+)
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                RequestPermissions(_requiredPermissions, 100);
            }
        }

        // Handle permission request result
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == 100)
            {
                for (int i = 0; i < permissions.Length; i++)
                {
                    System.Diagnostics.Debug.WriteLine($"Permission {permissions[i]}: {grantResults[i]}");
                }
            }
        }
    }
}