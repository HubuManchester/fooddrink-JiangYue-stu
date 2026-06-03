using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using System.Threading.Tasks;

namespace FoodDrinkApp.Services
{
	/// <summary>
	/// Interface for accessing device hardware features.
	/// Provides abstraction over device-specific implementations for:
	/// vibration, text-to-speech, camera, GPS, flashlight, and accelerometer.
	/// </summary>
	public interface IHardwareService
	{
		/// <summary>
		/// Vibrates the device for haptic feedback.
		/// </summary>
		/// <param name="duration">Duration in milliseconds. Default is 100ms.</param>
		void Vibrate(short duration = 100);

		/// <summary>
		/// Speaks the specified text using text-to-speech.
		/// </summary>
		/// <param name="text">The text to speak.</param>
		Task SpeakAsync(string text);

		/// <summary>
		/// Stops any ongoing text-to-speech.
		/// </summary>
		void StopSpeaking();

		/// <summary>
		/// Gets whether text-to-speech is currently speaking.
		/// </summary>
		bool IsSpeaking { get; }

		/// <summary>
		/// Captures a photo using the device camera.
		/// </summary>
		/// <returns>The captured photo as FileResult, or null if cancelled.</returns>
		Task<FileResult> TakePhotoAsync();

		/// <summary>
		/// Picks a photo from the device gallery.
		/// </summary>
		/// <returns>The selected photo as FileResult, or null if cancelled.</returns>
		Task<FileResult> PickPhotoAsync();

		/// <summary>
		/// Gets the current GPS location with high accuracy.
		/// </summary>
		/// <returns>The current location, or null if unavailable.</returns>
		Task<Location> GetCurrentLocationAsync();

		/// <summary>
		/// Gets the last known cached GPS location.
		/// </summary>
		/// <returns>The last known location, or null if unavailable.</returns>
		Task<Location> GetLastKnownLocationAsync();

		/// <summary>
		/// Turns on the device flashlight/flash.
		/// </summary>
		Task TurnOnFlashlightAsync();

		/// <summary>
		/// Turns off the device flashlight/flash.
		/// </summary>
		Task TurnOffFlashlightAsync();

		/// <summary>
		/// Starts the accelerometer to receive motion data.
		/// </summary>
		/// <param name="onReading">Callback invoked when new data is available.</param>
		void StartAccelerometer(Action<AccelerometerData> onReading);

		/// <summary>
		/// Stops the accelerometer.
		/// </summary>
		void StopAccelerometer();
	}

	/// <summary>
	/// Implementation of IHardwareService using .NET MAUI device APIs.
	/// All hardware operations include error handling for unsupported features.
	/// </summary>
	public class HardwareService : IHardwareService
	{
		// ========== VIBRATION ==========
		/// <summary>
		/// Vibrates the device for haptic feedback.
		/// Silently ignores if vibration is not supported on the device.
		/// </summary>
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
		private bool _isSpeaking = false;
		private CancellationTokenSource? _ttsCts;
		
		/// <summary>
		/// Gets whether text-to-speech is currently speaking.
		/// </summary>
		public bool IsSpeaking => _isSpeaking;

		/// <summary>
		/// Speaks the specified text using system text-to-speech engine.
		/// </summary>
		public async Task SpeakAsync(string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				_isSpeaking = true;
				try
				{
					_ttsCts = new CancellationTokenSource();
					var options = new SpeechOptions();
					await TextToSpeech.Default.SpeakAsync(text, options, _ttsCts.Token);
				}
				finally
				{
					_isSpeaking = false;
					_ttsCts = null;
				}
			}
		}

		/// <summary>
		/// Stops any ongoing text-to-speech.
		/// </summary>
		public void StopSpeaking()
		{
			_ttsCts?.Cancel();
			_ttsCts = null;
			_isSpeaking = false;
		}

		// ========== CAMERA ==========
		/// <summary>
		/// Captures a photo using the native camera app.
		/// </summary>
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
