using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;

namespace MistaksInHaramin.Droid
{
    [Activity(Label = "اخطاء في الحرمين الشريفين", MainLauncher = false,  Icon = "@drawable/ahkam", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, Android.Locations.ILocationListener
    {

        /// <summary>
        /// General public Locatin to save the current location of the device when the GPS is enabled and location can be accessed
        /// </summary>
        public static Location CurrentLocation;
        public static bool LocationEnabled { get; set; } = false; // default value is to disable GPS

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.FormsMaps.Init(this, bundle);
            LoadApplication(new App());

            this.ActionBar.SetIcon(Android.Resource.Color.Transparent);

            if(MainActivity.LocationEnabled)
                MainActivity.InitializeLocationManager();
        }


        /// <summary>
        /// GPS provider name
        /// </summary>
        static string locationProvider;
        public static LocationManager locationManager;
        public static void InitializeLocationManager()
        {
            locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
                MainActivity.LocationEnabled = false;
                MainActivity.CurrentLocation = null;
            }
            Android.Util.Log.Debug("GPS", "Using " + locationProvider + ".");
        }

        /// <summary>
        /// Override OnResume so that MainActivity will begin listening to the LocationManager when the activity comes into the foreground
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            if(locationManager != null && MainActivity.LocationEnabled)
                locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }

        /// <summary>
        ///  unsubscribe MainActivity from the LocationManager when the activity goes into the background
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();

            if(locationManager != null)
                locationManager.RemoveUpdates(this);
        }

        /// <summary>
        /// Update the current latitude and longitude when GPS Location is changed
        /// </summary>
        /// <param name="location"></param>
        void ILocationListener.OnLocationChanged(Location location)
        {
            CurrentLocation = location;

            if (CurrentLocation == null)
                MainActivity.LocationEnabled = false;
        }

        void ILocationListener.OnProviderDisabled(string provider) {
            MainActivity.LocationEnabled = false;
        }

        void ILocationListener.OnProviderEnabled(string provider) { }

        void ILocationListener.OnStatusChanged(string provider, Availability status, Bundle extras){ }
    }
}

