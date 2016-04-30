using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace MistaksInHaramin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public static LocationManager Manager = null;

        /// <summary>
        /// General public Location to save the current location of the device when the GPS is enabled and location can be accessed
        /// </summary>
        public static CoreLocation.CLLocation CurrentLocation = null;

        public static bool LocationEnabled { get; set; } = false; // the default value is disable GPS

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();
            LoadApplication(new App());

            // as soon as the app is done launching, begin generating location updates in the location manager
            if(AppDelegate.LocationEnabled == true)
            { 
                Manager = new LocationManager();
                Manager.StartLocationUpdates();
            }

            return base.FinishedLaunching(app, options);
        }     

    }
}
