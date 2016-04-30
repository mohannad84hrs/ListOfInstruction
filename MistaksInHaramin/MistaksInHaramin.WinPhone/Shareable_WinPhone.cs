using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

[assembly:Xamarin.Forms.Dependency(typeof(MistaksInHaramin.WinPhone.Shareable_WinPhone))]
namespace MistaksInHaramin.WinPhone
{
    class Shareable_WinPhone : MistaksInHaramin.HelperClasses.IShareable
    {

        string text, title, url;
        DataTransferManager dataTransferManager;

        public void ShareText(string textToShare, string shareTitle)
        {
            ShareLink(null, textToShare, shareTitle);
        }

        public void ShareLink(string url, string message = null, string shareTitle = null)
        {
            this.text = message ?? string.Empty;
            this.title = title ?? string.Empty;
            this.url = url;
            if (dataTransferManager == null)
            {
                dataTransferManager = DataTransferManager.GetForCurrentView();
                dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareTextHandler);
            }
            DataTransferManager.ShowShareUI();
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            try
            {
                DataRequest request = e.Request;
                // The Title is mandatory
#if WINDOWS_UWP
                request.Data.Properties.Title = title ?? Windows.ApplicationModel.Package.Current.DisplayName;
#elif WINDOWS_APP
                request.Data.Properties.Title = title ?? Windows.ApplicationModel.Package.Current.DisplayName;
#else
                request.Data.Properties.Title = title ?? string.Empty;

#endif

                if (!string.IsNullOrWhiteSpace(url))
                {

                    request.Data.SetWebLink(new Uri(url));

                }
                request.Data.SetText(text ?? string.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to share text: " + ex);
            }
        }
    }
}
