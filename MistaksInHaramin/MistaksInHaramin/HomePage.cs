using MistaksInHaramin.CustomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;
using Xamarin.Forms;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace MistaksInHaramin
{
    public class HomePage : BasicLayout
    {

        public HomePage()
        {
			
            Grid grid = new Grid
            {
                Padding = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(220,GridUnitType.Absolute) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star)  },
                }
                ,
                BackgroundColor = Color.White
            };
            Image Makka = new Image
            {
                Source = "Makkah.jpg"
                ,
                Aspect = Aspect.Fill
                //var location = DependencyService.Get<ILocationProvider>();

            };
            StackLayout makkaIconImageBox = new StackLayout
            {
                Padding = new Thickness(25, 45, 0, 25)
            };
            StackLayout madinaIconImageBox = new StackLayout
            {
                Padding = new Thickness(25, 45, 0, 25)
            };
            Image MakkaIcon = new Image
            {
                Source = "kaaba_green_bg.png"
                ,
                HorizontalOptions = LayoutOptions.Center

            };
            Image MadinaIcon = new Image
            {
                Source = "madinah_icon_green.png"

                ,
                HorizontalOptions = LayoutOptions.Center
            };


            NavigationPage n = new NavigationPage();


            Image Madina = new Image
            {
                Source = "Almadina.jpg"
                ,
                Aspect = Aspect.Fill
                ,
            };


            makkaIconImageBox.Children.Add(MakkaIcon);
            madinaIconImageBox.Children.Add(MadinaIcon);

            var titleLablebox = new StackLayout
            {
                BackgroundColor = ZadSpecialDesigen.ZadGreen
                ,
                Padding = 15,               
           
            };
            var title = new Label
            {
                Text = "الحرمين الشريفين الحرم المكي والحرم المدني وسميا بالحرمين لحرمة الاقتتال فيهما"
               ,
                TextColor = Color.White
                ,
                XAlign = TextAlignment.Center
                ,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
            var MainStackLayout = new StackLayout
            {
                Padding = 0
                ,
                Spacing = 0
            };
            Grid g1 = new Grid();
            g1.Children.Add(Makka);
            g1.Children.Add(makkaIconImageBox);
            Grid g2 = new Grid();
            g2.Children.Add(Madina);
            g2.Children.Add(madinaIconImageBox);
            Models.Cateogry makka = new Models.Cateogry(), madina = new Models.Cateogry();
            if (App.Categroies != null)
            {
                foreach (var area in App.Categroies.CategoryList)
                {
                    if (area.Type == "area")
                    {
                        if (area.Contents.ar.Title == "مكة")
                        {
                            makka = area;
                        }
                        else
                        {
                            madina = area;
                        }
                    }
                }
            }


            g2.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async (o) =>
                {

                    //  MadinaIcon.Source = "madinah_icon_green.png";
                    await Madina.ScaleTo(0.95, 50, Easing.CubicOut);
							await base.Navigation.PushAsync(new CityTabPage(madina.Id));
                    await Madina.ScaleTo(1, 50, Easing.CubicIn);
                    //   MadinaIcon.Source = "madinah_icon_faded.png";
      
                })
            });
            g1.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async (o) =>
                {

                    //    MakkaIcon.Source = "kaaba_green_bg.png";
                    await Makka.ScaleTo(0.95, 50, Easing.CubicOut);
							await base.Navigation.PushAsync(new CityTabPage(makka.Id));
                    await Makka.ScaleTo(1, 50, Easing.CubicIn);
                    //    MakkaIcon.Source = "kaaba_green_faded.png";
          
                })
            });

            grid.Children.Add(g2, 0, 0);
            grid.Children.Add(g1, 1, 0);
            titleLablebox.Children.Add(title);
            MainStackLayout.Children.Add(grid);
            MainStackLayout.Children.Add(titleLablebox);
            bool switching = true;
            if (App.Data != null)
            {
                foreach (Models.Post p in App.Data.Data)
                {
                    if ((p.In_home) && (p.Content != null))
                    {
                        MainStackLayout.Children.Add(new FullDetailStackLayout(p.Id, true, switching));
                        switching = !switching;
                    }

                }
            }

            this.MainBodyScrollView.Content = MainStackLayout;

        }
        protected override void OnAppearing()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                this.Title = "اتقن زيارتك";
            }
            else
            {
                this.Title = "";
            }
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                this.Title = "اتقن زيارتك";
            }
            else
            {
                this.Title = "";
            }
            base.OnAppearing();
            base.OnDisappearing();
        }



    }

}

