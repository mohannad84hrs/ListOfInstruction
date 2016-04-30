using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MistaksInHaramin
{
    public static class ZadSpecialDesigen
    {
       public static Color ZadGreen =  Color.FromHex("#00645c");
        public static Color ZadGreenNavBar = Color.FromHex("#008a8b");
        public static Color ZadWhite = Color.FromRgb(228, 227, 222);
       
        public static string ImageUrl ="http://zad-test.s3.amazonaws.com/cms-test/";

        public static Color DefaultNoneImageColor { get { return Color.FromHex("D8D8D8"); } }

        /// <summary>
        /// static methods: return the sizes of titles in all application
        /// </summary>
        /// <returns></returns>
        public static double GetFontSizeOfTitle()
        {
            return Device.GetNamedSize(NamedSize.Large, typeof(Label));
        }

        /// <summary>
        /// static methods: return the sizes of descriptions in all application
        /// </summary>
        /// <returns></returns>
        public static double GetFontSizeOfDescription()
        {
            return Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        }

        /// <summary>
        /// Get default padding of description for all places and details pages
        /// </summary>
        /// <returns></returns>
        public static Thickness GetPaddingOfDescription()
        {
            return new Thickness(25);
        }

        /// <summary>
        /// Return the distance of coordinate in meters
        /// </summary>
        public static double DefaultCoordinateDistance { get { return 289; } }

        public static Color GetTextColorOfDescription()
        {
            return Color.FromHex("#888888");
        }
    }
}
