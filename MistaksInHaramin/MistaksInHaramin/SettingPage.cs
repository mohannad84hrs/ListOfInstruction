using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MistaksInHaramin
{

    public class SettingPage : ContentPage
    {
        Label lblLocation;
        public static bool LocationEnabled;
        Switch gpsSwitcher;
        Picker languagePicker;

       

        public SettingPage()
        {
            this.BackgroundColor = ZadSpecialDesigen.ZadWhite;
            this.Padding = 0;
            Label lblHeader = new Label
            {
                Text = "الاعدادات",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
                ,TextColor=ZadSpecialDesigen.ZadGreen
            };
            StackLayout HeaderStackLayout = new StackLayout
            {
                Spacing = 10,
                Padding = 10,
                Children = { lblHeader }
            };

            /////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////  GPS Location Area  //////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////
            gpsSwitcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand
                
            };         
            gpsSwitcher.Toggled += switcher_Toggled;

            lblLocation = new Label
            {
                Text = "تحديد المكان",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions= LayoutOptions.Center,
                XAlign=TextAlignment.End
                , TextColor = Color.Black
            };

            var gpsLocationGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(35, GridUnitType.Star) },
                },
                BackgroundColor = ZadSpecialDesigen.ZadWhite
            };
            gpsLocationGrid.Children.Add(gpsSwitcher, 0, 0);
            gpsLocationGrid.Children.Add(new BoxView { WidthRequest = 140 }, 1, 0);
            gpsLocationGrid.Children.Add(lblLocation, 2, 0);

            /////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////// Language Picker Area //////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////
            var lblLanguage = new Label
            {
                Text = "اختيار اللغة",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                XAlign = TextAlignment.End,
                TextColor = Color.Black
            };

            // Initilize picker with its values(Languages Name)                   
            languagePicker = new Picker
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ZadSpecialDesigen.ZadGreen,             
            };           

            languagePicker.Title = HelperClasses.Helper.GetCurrentLanguageTitle();
             
            /// <summary>
            /// Keep in the same sequence of the Languages enum.
            /// First is Arabic with index 0.
            /// Second is English with index 1.
            /// </summary>
            languagePicker.Items.Add("عربي");// 0 sequence
            languagePicker.Items.Add("انكليزي");// 1 sequence
            languagePicker.SelectedIndexChanged += LanguagePicker_SelectedIndexChanged;
            languagePicker.SelectedIndex = (int)App.CurrentLanguage;

            var langsGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) },
                },
            };

            langsGrid.Children.Add(lblLanguage, 1, 0);
            langsGrid.Children.Add(languagePicker, 0, 0);
            
            /////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////// Main StackLayout Area ////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////////
            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            Content = new StackLayout
            {       
                Children =
                {
                    HeaderStackLayout,
                    gpsLocationGrid,
                    new StackLayout { HeightRequest=5, BackgroundColor =ZadSpecialDesigen.ZadGreen },
                    langsGrid,
                }         
            };
        }

        private async void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (languagePicker.SelectedIndex == -1)
                return;
            else
            {
                Language chosenLang = (Language)languagePicker.SelectedIndex;
                App.ChangeLanguage(chosenLang);
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            gpsSwitcher.IsToggled = await GetTheSwithValue();
        }
        HelperClasses.ILocationProvider locationService = DependencyService.Get<HelperClasses.ILocationProvider>();
        ISaveAndLoad fileService = DependencyService.Get<ISaveAndLoad>();
        async void switcher_Toggled(object sender, ToggledEventArgs e)
        {
          
			if (!locationService.GPSStatus)
            {
                await DisplayAlert("تنبيه","خدمة تحديد المكان غير مفعلة في هاتفك يرجى تفعيلها للاستفادة من هذه الخدمة", "اغلاق");
            }
            LocationEnabled = gpsSwitcher.IsToggled;
       
  
            await fileService.SaveTextAsync("setting.txt", gpsSwitcher.IsToggled.ToString());
            locationService.Enabled = gpsSwitcher.IsToggled;
        }
        async private System.Threading.Tasks.Task< bool> GetTheSwithValue()
        {

            return bool.TryParse(await fileService.LoadTextAsync("setting.txt"), out LocationEnabled) ? LocationEnabled : false;

        }
    }
}
