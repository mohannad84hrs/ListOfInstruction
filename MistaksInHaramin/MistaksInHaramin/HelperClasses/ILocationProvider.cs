using Xamarin.Forms.Maps;

namespace MistaksInHaramin.HelperClasses
{
    /// <summary>
    /// LocationProvider Interfece that Implemented on all platform (Droid, IOS, WinPhone).
    /// Get the current locatoin of the device when GPS enabled
    /// </summary>
    public interface ILocationProvider
    {
        /// <summary>
        /// Get the current Location of the deviec if GPS Location is enabled
        /// </summary>
        /// <returns>return the location of device position and return Postion(0,0) if the gps couldn't be notified</returns>
        Position GetCurrentLocation();

        /// <summary>
        /// General Property: to indicate whether the status of GPS access location is enabled or disabled.
        /// set: put the Enabled to true to enable GPS Locatin access, otherwise put to false to disable it.
        /// get: return true when GPS location access enabled in the device(by user) and in the app(settings page). otherwise return false.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// GPSStatusy: return true when GPS is enabled in device otherwise return false. 
        /// </summary>
        bool GPSStatus { get; }
    }
}
