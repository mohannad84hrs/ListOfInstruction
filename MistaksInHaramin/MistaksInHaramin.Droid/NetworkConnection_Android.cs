using Android.App;
using Android.Content;
using Android.Net;
using MistaksInHaramin.HelperClasses;

[assembly:Xamarin.Forms.Dependency(typeof(MistaksInHaramin.Droid.NetworkConnection_Android))]
namespace MistaksInHaramin.Droid
{
    class NetworkConnection_Android : INetworkConnection
    {
        public bool IsConnected {
            get
            {
                var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
                if (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}