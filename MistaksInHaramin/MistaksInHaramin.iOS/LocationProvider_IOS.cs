using CoreLocation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms.Maps;

[assembly:Xamarin.Forms.Dependency(typeof(MistaksInHaramin.iOS.LocationProvider_IOS))]
namespace MistaksInHaramin.iOS
{
    class LocationProvider_IOS : HelperClasses.ILocationProvider
    {
        public bool Enabled
        {
            get
            {
                return AppDelegate.LocationEnabled;
            }

            set
            {
                AppDelegate.LocationEnabled = value;
                if (AppDelegate.LocationEnabled == true)
                {
                    if (AppDelegate.Manager == null)
                        AppDelegate.Manager = new LocationManager();

                    AppDelegate.Manager.StartLocationUpdates();
                }else
                    AppDelegate.CurrentLocation = null;
            }
        }

        public bool GPSStatus
        {
            get
            {
                if (CLLocationManager.LocationServicesEnabled)
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
            if (AppDelegate.CurrentLocation == null)
            {
                return loc;
            }
            else
            {
                loc = new Position(AppDelegate.CurrentLocation.Coordinate.Latitude, AppDelegate.CurrentLocation.Coordinate.Longitude);
            }

            return loc;
        }
      
    }
}
