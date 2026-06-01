using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using System.Threading.Tasks;

namespace FoodDrinkApp.Services
{
	public interface IHardwareService
	{
		// Vibration
		void Vibrate(short duration = 100);

		// TTS
		Task SpeakAsync(string text);

		// Camera
		Task<FileResult> TakePhotoAsync();
		Task<FileResult> PickPhotoAsync();

		// GPS
		Task<Location> GetCurrentLocationAsync();
		Task<Location> GetLastKnownLocationAsync();

		// Flashlight
		Task TurnOnFlashlightAsync();
		Task TurnOffFlashlightAsync();

		// Accelerometer
		void StartAccelerometer(Action<AccelerometerData> onReading);
		void StopAccelerometer();
	}

	public class HardwareService : IHardwareService
	{
		// ========== VIBRATION ==========
		public void Vibrate(short duration = 100)
		{
			try
			{
				Vibration.Vibrate(duration);
			}
			catch (FeatureNotSupportedException)
			{
				// Vibration not supported on this device
			}
		}

		// ========== TTS (Text-to-Speech) ==========
		public async Task SpeakAsync(string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				await TextToSpeech.Default.SpeakAsync(text);
			}
		}

		// ========== CAMERA ==========
		public async Task<FileResult> TakePhotoAsync()
		{
			if (MediaPicker.Default.IsCaptureSupported)
			{
				var photo = await MediaPicker.Default.CapturePhotoAsync();
				return photo;
			}
			return null;
		}

		public async Task<FileResult> PickPhotoAsync()
		{
			var photo = await MediaPicker.Default.PickPhotoAsync();
			return photo;
		}

		// ========== GPS / LOCATION ==========
		public async Task<Location> GetCurrentLocationAsync()
		{
			try
			{
				var request = new GeolocationRequest(GeolocationAccuracy.Best);
				var location = await Geolocation.Default.GetLocationAsync(request);
				return location;
			}
			catch (FeatureNotSupportedException)
			{
				return null;
			}
			catch (PermissionException)
			{
				return null;
			}
		}

		public async Task<Location> GetLastKnownLocationAsync()
		{
			try
			{
				var location = await Geolocation.Default.GetLastKnownLocationAsync();
				return location;
			}
			catch (FeatureNotSupportedException)
			{
				return null;
			}
		}

		// ========== FLASHLIGHT ==========
		public async Task TurnOnFlashlightAsync()
		{
			try
			{
				await Flashlight.Default.TurnOnAsync();
			}
			catch (FeatureNotSupportedException)
			{
				// Flashlight not supported
			}
		}

		public async Task TurnOffFlashlightAsync()
		{
			try
			{
				await Flashlight.Default.TurnOffAsync();
			}
			catch (FeatureNotSupportedException)
			{
				// Flashlight not supported
			}
		}

		// ========== ACCELEROMETER ==========
		public void StartAccelerometer(Action<AccelerometerData> onReading)
		{
			try
			{
				Accelerometer.Default.ReadingChanged += (s, e) =>
				{
					onReading?.Invoke(e.Reading);
				};
				Accelerometer.Default.Start(SensorSpeed.UI);
			}
			catch (FeatureNotSupportedException)
			{
				// Accelerometer not supported
			}
		}

		public void StopAccelerometer()
		{
			try
			{
				Accelerometer.Default.Stop();
			}
			catch (FeatureNotSupportedException)
			{
				// Accelerometer not supported
			}
		}
	}
}
