using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Practices.Unity;
using Prism.Unity;

// ↓追加
using Android.Preferences;

namespace VoiceRecognitionSample.Droid
{
	[Activity(Label = "VoiceRecognitionSample.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		// ↓追加
		public event EventHandler<PreferenceManager.ActivityResultEventArgs> ActivityResult = delegate {};

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App(new AndroidInitializer()));
		}

		// ↓追加
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			var resultEventArgs = new PreferenceManager.ActivityResultEventArgs(true, requestCode, resultCode, data);
			ActivityResult(this, resultEventArgs);
		}
	}

	public class AndroidInitializer : IPlatformInitializer
	{
		public void RegisterTypes(IUnityContainer container)
		{

		}
	}
}
