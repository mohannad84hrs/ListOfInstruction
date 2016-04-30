using CoreLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MistaksInHaramin.iOS
{
    public class LocationUpdatedEventArgs : EventArgs
    {
        CLLocation location;

        public LocationUpdatedEventArgs(CLLocation location)
        {
            this.location = location;

            AppDelegate.CurrentLocation = location;

        }

        public CLLocation Location
        {
            get { return location; }
        }
    }
}
