﻿using CoreLocation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MistaksInHaramin.iOS
{
    public class LocationManager
    {
        protected CLLocationManager locMgr;
        public CLLocationManager LocMgr
        {
            get { return this.locMgr; }
        }

        // event for the location changing
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

        public LocationManager()
        {
            this.locMgr = new CLLocationManager();
            this.locMgr.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization(); // works in background
                //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }
        }

        public void StartLocationUpdates()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                //set the desired accuracy, in meters
                LocMgr.DesiredAccuracy = 1;
                LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    // fire our custom Location Updated event
                    LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
                };
                LocMgr.StartUpdatingLocation();
            }
            else // false if the user has denied the application access to location.
            {
                AppDelegate.LocationEnabled = false;
                AppDelegate.CurrentLocation = null;
            }
        }

    }
}
