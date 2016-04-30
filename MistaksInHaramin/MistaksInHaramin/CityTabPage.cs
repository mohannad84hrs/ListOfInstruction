using MistaksInHaramin.CustomControl;
using MistaksInHaramin.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace MistaksInHaramin
{
    public class CityTabPage : BasicLayout
    {
        Image titleImage;
        Grid tabButtons;
        Button btnPageMistakes, btnPagePlace;
        StackLayout MistaskContent, PlaceContent;
        Models.Cateogry CurrentArea;
        public CityTabPage(int currentAreaID)
        {
        //    this.CurrentArea = currentArea;
			this.CurrentArea = App.Categroies.CategoryList.Find (x => x.Id.Equals (currentAreaID));
			this.Title = this.CurrentArea.Contents.ar.Title;
			titleImage = new Image
			{
				Source = Title == "مكة" ? "Makkah_Full.jpg" : "Almadina.jpg"
					,
				Aspect = Aspect.AspectFill
					,
				HeightRequest = 200
			};
			///////////////////////////////
			tabButtons = new Grid
			{
				RowSpacing = 0,
				ColumnSpacing = 0
					,
				Padding = new Thickness(0, 0, 0, 4)
					,
				BackgroundColor = ZadSpecialDesigen.ZadGreen
			};
			btnPageMistakes = new Button
			{
				Text = "إستعراض بالأخطاء",
				BackgroundColor=Color.White,
				BorderRadius = 0
			};
			btnPagePlace = new Button
			{
				Text = "إستعراض بالأماكن",
				BackgroundColor = Color.White,
				BorderRadius = 0
			};
			tabButtons.Children.Add(btnPageMistakes, 0, 0);
			tabButtons.Children.Add(btnPagePlace, 1, 0);
			btnPageMistakes.Clicked += btnPageMistakes_clicked;
			btnPagePlace.Clicked += btnPagePlace_clicked;

			PlaceContent = GetPageContentForPlaces();
			btnPagePlace_clicked(btnPagePlace, new EventArgs());
			MistaskContent = GetPageContentForMistaks();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
          
        }


        private void btnPageMistakes_clicked(object sender , EventArgs e )
        {
            var current = sender as Button;
            current.TextColor = Color.White;
            current.BackgroundColor = ZadSpecialDesigen.ZadGreen;      
            MainBodyScrollView.Content = MistaskContent;
            btnPagePlace.BackgroundColor = Color.White;
            btnPagePlace.TextColor = ZadSpecialDesigen.ZadGreen;
        }
        private void btnPagePlace_clicked(object sender, EventArgs e)
        {
            var current = sender as Button;
            current.TextColor = Color.White;
            current.BackgroundColor = ZadSpecialDesigen.ZadGreen;
        
            MainBodyScrollView.Content = PlaceContent;

            btnPageMistakes.BackgroundColor = Color.White;
            btnPageMistakes.TextColor = ZadSpecialDesigen.ZadGreen;
        }
      
        private StackLayout GetPageContentForPlaces()
        {
            var mainStackLayout = new StackLayout
            {
                Spacing = 0
            };
            mainStackLayout.Children.Add(tabButtons);
            mainStackLayout.Children.Add(titleImage);
            StackLayout title = new StackLayout
            {
                BackgroundColor = ZadSpecialDesigen.ZadGreen
                ,
                TranslationY = -0
                ,
                Padding = new Thickness(10, 10, 10, 0),

            };
            string noHTML = Regex.Replace(CurrentArea.Contents.ar.Description, @"<[^>]+>|&nbsp;", "").Trim();
            int count = noHTML.Length;
            var TitleText = new Label
            {
                XAlign = TextAlignment.Center,
                TextColor = Color.White
                 ,
                Text = noHTML.CutString(150)
                 ,
                HorizontalOptions = LayoutOptions.Center,

                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))+3,
            };
            title.Children.Add(TitleText);
            if (count > 150)
            {
                Image moreButton = new Image
                {
                    Source = "more_icon.png",
                    BackgroundColor = ZadSpecialDesigen.ZadGreen
                };
                bool isMaxed = false;
                var mini = new TapGestureRecognizer
                {
                    Command = new Command(async (o) =>
                    {

                        if (!isMaxed)
                        {
                            TitleText.Text = noHTML;
                            isMaxed = !isMaxed;
                        }
                        else
                        {
                            TitleText.Text = noHTML.CutString(150);
                            isMaxed = !isMaxed;
                        }

                    })
                };
                moreButton.GestureRecognizers.Add(mini);
                var grid = new Grid { HeightRequest = 20, };
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(18, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });

                grid.Children.Add(moreButton, 1, 0);
                title.Children.Add(grid);
            }



            mainStackLayout.Children.Add(title);
            List <Models.Post> list= new List<Models.Post>();
            if (App.Data != null)
            {
            
                foreach (var place in App.Data.Data)
                {
                    if (place.Taxonomies.Area.Length> 0)
                    {
                        if ((place.Content != null) && (place.Content.ar.Attachments != null) && (place.Type == "place") && (place.Taxonomies.Area[0] == CurrentArea.Id))
                        {
                            list.Add(place);
                        }
                    }
                }
            }
            int heigh = 0;
            if(list.Count%2==0)
            {
                heigh = list.Count * 84;
            }
            else
            {
                heigh =( list.Count+1 )* 84;
            }
            

            PlacesViewer placeViewer = new PlacesViewer()
            {
                Padding = 0,
               HeightRequest= heigh,
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star)  },
                },
        
            };
            int top = 0;
            int left = 0;
            if (App.Data != null)
            {
                int cout = list.Count % 2 == 0 ? list.Count : list.Count + 1;
                foreach (var place in App.Data.Data)
                {
                    if (place.Taxonomies.Area.Length > 0)
                    {
                        if ((place.Content != null) && (place.Content.ar.Attachments != null) && (place.Type == "place") && (place.Taxonomies.Area[0] == CurrentArea.Id))
                        {
                            placeViewer.AddPlacetoStackLayout(place.Id, left, top);
                            
                            top++;
                            if (top == (cout / 2))
                            {
                                top = 0;
                                left = 1;

                            }

                        }
                    }
                }
            }
            mainStackLayout.Children.Add(tabButtons);
            mainStackLayout.Children.Add(placeViewer);
            return mainStackLayout;
        }
        private void onTapOnPlace(View v, object o)
        {
            base.Navigation.PushAsync(new Views.PlaceDetailsPage());
        }


        private StackLayout GetPageContentForMistaks()
        {
            var mainStackLayout = new StackLayout
            {
                Spacing = 0
            };
            mainStackLayout.Children.Add(tabButtons);
            mainStackLayout.Children.Add(titleImage);
            StackLayout title = new StackLayout
            {
                BackgroundColor = ZadSpecialDesigen.ZadGreen

                ,
                TranslationY = -2
                ,
                Padding = new Thickness(10, 10, 10, 0),
       
            };
            string noHTML = Regex.Replace(CurrentArea.Contents.ar.Description, @"<[^>]+>|&nbsp;", "").Trim();
            int count = noHTML.Length;
            var TitleText = new Label
            {
                XAlign = TextAlignment.Center,
                TextColor = Color.White
                 ,
                Text = noHTML.CutString(150)
                 ,
                HorizontalOptions = LayoutOptions.Center,

                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))+3,
            };
            title.Children.Add(TitleText);
            if (count > 150)
            {
                Image moreButton = new Image
                {
                    Source = "more_icon.png",
                    BackgroundColor = ZadSpecialDesigen.ZadGreen
                };
                bool isMaxed = false;
                var mini = new TapGestureRecognizer
                {
                    Command = new Command(async (o) =>
                    {

                        if (!isMaxed)
                        {
                            TitleText.Text = noHTML;
                            isMaxed = !isMaxed;
                        }
                        else
                        {
                            TitleText.Text = noHTML.CutString(150);
                            isMaxed = !isMaxed;
                        }

                    })
                };
                moreButton.GestureRecognizers.Add(mini);
                var grid = new Grid { HeightRequest=20,  };
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(18, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });

                grid.Children.Add(moreButton,1, 0);
                title.Children.Add(grid);
            }
            StackLayout mistakesStack = new StackLayout
            {
                Padding = 0,
                Spacing = 0
            };
            bool switching = true;
            if (App.Data != null)
            {
                foreach (Models.Cateogry p in App.Categroies.CategoryList)
                {
                  
                        if ((p.Type == "category") && (p.Contents != null) )
                        {
                            mistakesStack.Children.Add(new FullDetailStackLayout(p.Id, true, switching , CurrentArea.Id) );
                            switching = !switching;
                        }
                 
                }
            }
 
            mainStackLayout.Children.Add(title);
            mainStackLayout.Children.Add(mistakesStack);
            return mainStackLayout;
        }

    
        
    }
    


}
