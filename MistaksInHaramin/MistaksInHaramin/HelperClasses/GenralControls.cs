using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MistaksInHaramin.HelperClasses
{
    public  class GenralControls:ContentPage
    {
        public  StackLayout SideMenu { get { return _panel; } }
        private StackLayout _panel;
        public  GenralControls()
        {
            _panel = new StackLayout();

                Image home = new Image
                {
                    Source = "home_icon_new.png"
                   ,
                    WidthRequest = 30
                   ,
                    HeightRequest = 30
                };

                Image setting = new Image
                {
                    Source = "gear_icon.png"
                    ,
                    WidthRequest = 30
                    ,
                    HeightRequest = 30
                };
                Image info = new Image
                {
                    Source = "info_icon.png"
                         ,
                    WidthRequest = 30
                    ,
                    HeightRequest = 30
                };
                setting.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async (o) =>
                    {
                        await setting.ScaleTo(0.95, 50, Easing.CubicOut);
                        await setting.ScaleTo(1, 50, Easing.CubicIn);
                        await base.Navigation.PushAsync(new SettingPage());
                    })
                });
                home.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async (o) =>
                    {
                        await home.ScaleTo(0.95, 50, Easing.CubicOut);
                        await home.ScaleTo(1, 50, Easing.CubicIn);
                        await base.Navigation.PopToRootAsync();
                    })
                });

                info.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async (o) =>
                    {
                        await info.ScaleTo(0.95, 50, Easing.CubicOut);
                        await info.ScaleTo(1, 50, Easing.CubicIn);
                        await base.Navigation.PushAsync(new Views.InfoPage());
                    })
                });


                _panel = new StackLayout
                {
                    Padding = new Thickness(10, 45, 0, 10),
                    Spacing = 30,
                    WidthRequest = 60,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    BackgroundColor = Color.FromHex("#222")
                };
                _panel.Children.Add(home);
                _panel.Children.Add(setting);
                _panel.Children.Add(info);
            }


      
    }
}
