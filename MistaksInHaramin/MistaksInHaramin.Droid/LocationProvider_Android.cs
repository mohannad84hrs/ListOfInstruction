using System;
using MistaksInHaramin.HelperClasses;
using Xamarin.Forms.Maps;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Runtime;

[assembly:Xamarin.Forms.Dependency(typeof(MistaksInHaramin.Droid.LocationProvider_Android))]
namespace MistaksInHaramin.Droid
{
    class LocationProvider_Android : ILocationProvider
    {
        public bool Enabled
        {
            get
            {
                return MainActivity.LocationEnabled;
            }

            set
            {
                MainActivity.LocationEnabled = value;
                if (MainActivity.LocationEnabled == true)
                    MainActivity.InitializeLocationManager();
                else
                    MainActivity.CurrentLocation = null;    
            }
        }

        public bool GPSStatus
        {
            get
            {
                if (MainActivity.locationManager == null)
                    MainActivity.InitializeLocationManager();

                if (MainActivity.locationManager.IsProviderEnabled(LocationManager.GpsProvider))
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public Position GetCurrentLocation()
        {
           
            Position loc = new Position(0, 0);
            if (MainActivity.CurrentLocation == null)
            {
                return loc;
            }
            else
            {
               loc = new Position(MainActivity.CurrentLocation.Latitude, MainActivity.CurrentLocation.Longitude);
            }
                      
            return loc;
        }

    }
}