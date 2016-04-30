using System;
using Xamarin.Forms.Maps;

namespace MistaksInHaramin.HelperClasses
{
    /// <summary>
    /// Sharable Interfece that Implemented on all platform (Droid, IOS, WinPhone).
    ///  share the content of text with social and messaging app in the phone
    /// </summary>
    public interface IShareable
    {
        /// <summary>
        /// Share message with compatible apps
        /// </summary>
        /// <param name="textToShare">message to share</param>
        /// <param name="shareTitle">title of popup share (for ex: 'choose the app to share')</param>
        void ShareText(string textToShare, string shareTitle);

        /// <summary>
        /// Share link and message with compatible apps
        /// </summary>
        /// <param name="url">link to share</param>
        /// <param name="message">message with link to share</param>
        /// <param name="shareTitle">title of popup share (for ex: 'choose the app to share')</param>
        void ShareLink(string url, string message, string shareTitle);
    }

}
