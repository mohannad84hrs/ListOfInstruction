using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;


namespace MistaksInHaramin.Droid
{
    [Activity(Label = "أماكن الحرمين", MainLauncher = true, NoHistory =false,  Theme = "@style/Theme.Splash" ,Icon = "@drawable/ic_launcher", ScreenOrientation =ScreenOrientation.Portrait , ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
   
            var intent = new Intent(this, typeof(MainActivity));


            StartActivity(intent);

            Finish();
        }
    }
}