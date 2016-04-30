using System;

using Xamarin.Forms;

namespace MistaksInHaramin
{
	public class VideosViewPage : ContentPage
	{
        public VideosViewPage()
        {

            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
            var a = new CustomControl.PopupLayout
            {
                Content = new Label { Text = "I'm Content!" },
                WidthRequest = 100
                ,BackgroundColor=ZadSpecialDesigen.ZadGreen
            };
           
            this.WidthRequest = 100;
            this.Content = a;


        }
    }
}


