using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

[assembly:Xamarin.Forms.Dependency(typeof(MistaksInHaramin.iOS.Shareable_IOS))]
namespace MistaksInHaramin.iOS
{
    class Shareable_IOS : MistaksInHaramin.HelperClasses.IShareable
    {

        public async void ShareText(string textToShare, string shareTitle = null)
        {
            try
            {
                var items = new NSObject[] { new NSString(textToShare ?? string.Empty) };
                var activityController = new UIActivityViewController(items, null);

                var vc = GetVisibleViewController();

                if (activityController.PopoverPresentationController != null)
                {
                    activityController.PopoverPresentationController.SourceView = vc.View;
                }

                await vc.PresentViewControllerAsync(activityController, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to share text" + ex.Message);
            }
        }

        public async void ShareLink(string url, string message, string shareTitle = null)
        {
            try
            {
                var items = new NSObject[] { new NSString(message ?? string.Empty), NSUrl.FromString(url) };
                var activityController = new UIActivityViewController(items, null);
                var vc = GetVisibleViewController();

                if (activityController.PopoverPresentationController != null)
                {
                    activityController.PopoverPresentationController.SourceView = vc.View;
                }

                await vc.PresentViewControllerAsync(activityController, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to share text" + ex.Message);
            }
        }

        /// <summary>
        /// Gets the shareable application.
        /// </summary>
        /// <returns>The visible view controller.</returns>
        UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).TopViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }

    }
}
