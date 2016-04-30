using MistaksInHaramin.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace MistaksInHaramin.CustomControl
{
    public class FullDetailStackLayout : ContentView
    {
        public Label Title { set; get; }
        public Label Details { set; get; }
        public Image RightImage { set; get; }
        public Button GoButton { set; get; }

        StackLayout _layout;
        Models.Post post;
        Models.Cateogry catgory;

        public FullDetailStackLayout(int PostId, bool WithIcon, bool even)
        {
            this.post = App.Data.Data.Find(x=>x.Id.Equals(PostId));
            
            _layout = new StackLayout
            {
                HeightRequest = 100,
                Spacing = 0,
                Padding = 0
            };

            Grid grid = new Grid
            {
                Padding = new Thickness(0, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 5,
                RowDefinitions =
                {
                    new RowDefinition { Height = 70},
                },                        
                BackgroundColor =even ? Color.White :ZadSpecialDesigen.ZadWhite 
            };
            if (WithIcon)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
            }
            else
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
            }
            this.Title = new Label
            {
                Text = cutString( post.Content.ar.Title,17),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Start,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = ZadSpecialDesigen.ZadGreen
                ,HorizontalOptions=LayoutOptions.End             };
            this.Details = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.End,
                Text = cutString (post.Content.ar.Description,45),
                TextColor = Color.FromHex("#555")
                         ,
                HorizontalOptions = LayoutOptions.End
            };
          

            this.RightImage = new Image
            {

                HeightRequest = 40,
                WidthRequest = 40,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center
                , VerticalOptions = LayoutOptions.Center
            };
            switch(post.Type)
            {
                case "mistake":
                    RightImage.Source = "x_icon_clear_green.png";
                    break;
                case "ahkam":
                    RightImage.Source = "book_icon_green_bg.png";
                    break;
                case "azkar":
                    RightImage.Source = "hands_icon_clear_green.png";
                    break;
                case "place":
                    RightImage.Source = "location_icon_green_bg.png";
                    break;

            }
            var leftArrowImage = new Image
            {
                Source = "gree_arrow_icon.png"
                ,
                HeightRequest = 25
                ,
                WidthRequest = 25
                ,
                Aspect = Aspect.Fill
                ,
                 HorizontalOptions = LayoutOptions.Center
                ,
                VerticalOptions = LayoutOptions.Center
            };
            
        
        
            var TextBoxstackLayout = new StackLayout
            {
                Padding = 10
                ,
                Spacing = 10
                ,
                Orientation = StackOrientation.Vertical
            };
       
            TextBoxstackLayout.Children.Add(Title);
            TextBoxstackLayout.Children.Add(Details);
            grid.Children.Add(leftArrowImage, 0, 0);
            grid.Children.Add(TextBoxstackLayout, 1, 0);
            if(WithIcon)
                grid.Children.Add(RightImage, 2, 0);
           
         
            _layout.Children.Add(grid);
            grid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async (o) =>
                {
                    await this.ScaleTo(0.95, 50, Easing.CubicOut);
                    await this.ScaleTo(1, 50, Easing.CubicIn);
                     new PostDataType(post.Id);
                })
            });

            this.Content = _layout;
        }
        public FullDetailStackLayout(int PostId, bool WithIcon, bool even ,bool withDeleteButton ,Action delete)
        {
            this.post = App.Data.Data.Find(x => x.Id.Equals(PostId));
            ;
            _layout = new StackLayout
            {
                HeightRequest = 100,
                Spacing = 0,
                Padding = 0
            };

            Grid grid = new Grid
            {
                Padding = new Thickness(0, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 5,
                RowDefinitions =
                {
                    new RowDefinition { Height = 70},
                },
                BackgroundColor = even ? Color.White : ZadSpecialDesigen.ZadWhite
            };
            if (WithIcon)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
            }
            else
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
            }
            this.Title = new Label
            {
                Text = cutString(post.Content.ar.Title, 17),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Start,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = ZadSpecialDesigen.ZadGreen
                ,
                HorizontalOptions = LayoutOptions.End
            };
            this.Details = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.End,
                Text = cutString(post.Content.ar.Description,45),
                TextColor = Color.FromHex("#555")
                         ,
                HorizontalOptions = LayoutOptions.End
            };


            this.RightImage = new Image
            {

                HeightRequest = 40,
                WidthRequest = 40,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center
                ,
                VerticalOptions = LayoutOptions.Center
            };
            switch (post.Type)
            {
                case "mistake":
                    RightImage.Source = "x_icon_clear_green.png";
                    break;
                case "ahkam":
                    RightImage.Source = "book_icon_green_bg.png";
                    break;
                case "azkar":
                    RightImage.Source = "hands_icon_clear_green.png";
                    break;
                case "place":
                    RightImage.Source = "location_icon_green_bg.png";
                    break;

            }
            var leftArrowImage = new Image
            {
                Source = "gree_arrow_icon.png"
                ,
                HeightRequest = 25
                ,
                WidthRequest = 25
                ,
                Aspect = Aspect.Fill
                ,
                HorizontalOptions = LayoutOptions.Center
                ,
                VerticalOptions = LayoutOptions.Center
            };



            var TextBoxstackLayout = new StackLayout
            {
                Padding = 10
                ,
                Spacing = 10
                ,
                Orientation = StackOrientation.Vertical
            };

            TextBoxstackLayout.Children.Add(Title);
            TextBoxstackLayout.Children.Add(Details);
            grid.Children.Add(leftArrowImage, 0, 0);
            grid.Children.Add(TextBoxstackLayout, 1, 0);
           


            Image imgRemove;
            if (withDeleteButton)
            {


                imgRemove = new Image
                    {
                        Source = "delete_green_icon"
                        ,
                    HeightRequest = 50,
                    WidthRequest = 50,
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Center
                ,
                    VerticalOptions = LayoutOptions.Center

                };
                imgRemove.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async (o) =>
                    {
                       
                        await imgRemove.ScaleTo(0.95, 50, Easing.CubicOut);
                        await imgRemove.ScaleTo(1, 50, Easing.CubicIn);
                        grid.BackgroundColor = Color.Red;
                        await this.TranslateTo(-450, 0, 350, Easing.CubicOut);
                        delete();
                    })
                });
                grid.Children.Add(imgRemove, 2, 0);    

            }
            _layout.Children.Add(grid);
            grid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async (o) =>
                {
                    await this.ScaleTo(0.95, 50, Easing.CubicOut);
                    await this.ScaleTo(1, 50, Easing.CubicIn);
                    new PostDataType(post.Id);
                })
            });

            this.Content = _layout;
        }
        private void eventdoitman(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public FullDetailStackLayout( int catId, bool WithIcon, bool even, int AreaID)
        {

            this.catgory = App.Categroies.CategoryList.Find(x => x.Id.Equals(catId));

            _layout = new StackLayout
            {
                HeightRequest = 100,
                Spacing = 0,
                Padding = 0
					
            };

            Grid grid = new Grid
            {
                Padding = new Thickness(0, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 5,
                RowDefinitions =
                {
                    new RowDefinition { Height = 70},
                },
                BackgroundColor = even ? Color.White : ZadSpecialDesigen.ZadWhite
            };
            if (WithIcon)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
            }
            else
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
            }
            this.Title = new Label
            {
                Text = cutString(catgory.Contents.ar.Title, 22),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Start,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = ZadSpecialDesigen.ZadGreen
                ,
                HorizontalOptions = LayoutOptions.End
            };
            this.Details = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.End,
                Text = cutString(catgory.Contents.ar.Description,45),
                TextColor = Color.FromHex("#555")
                         ,
                HorizontalOptions = LayoutOptions.End
            };


            this.RightImage = new Image
            {

                HeightRequest = 40,
                WidthRequest = 40,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center
                ,
                VerticalOptions = LayoutOptions.Center
            };
            switch (this.catgory.Type)
            {
                case "category":
                    RightImage.Source = "x_icon_clear_green.png";
                    break;
            }
            var leftArrowImage = new Image
            {
                Source = "gree_arrow_icon.png"
                ,
                HeightRequest = 25
                ,
                WidthRequest = 25
                ,
                Aspect = Aspect.Fill
                ,
                HorizontalOptions = LayoutOptions.Center
                ,
                VerticalOptions = LayoutOptions.Center
            };



            var TextBoxstackLayout = new StackLayout
            {
                Padding = 10
                ,
                Spacing = 10
                ,
                Orientation = StackOrientation.Vertical
            };

            TextBoxstackLayout.Children.Add(Title);
            TextBoxstackLayout.Children.Add(Details);
            grid.Children.Add(leftArrowImage, 0, 0);
            grid.Children.Add(TextBoxstackLayout, 1, 0);
            if (WithIcon)
                grid.Children.Add(RightImage, 2, 0);
            _layout.Children.Add(grid);


            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) => {
                await this.ScaleTo(0.95, 50, Easing.CubicOut);
                await this.ScaleTo(1, 50, Easing.CubicIn);
                new PostDataType(catgory,AreaID);
            };           

            grid.GestureRecognizers.Add(tap);

            this.Content = _layout;
        }



        private string cutString (string sb,int MaxLength)
        {
            string s =  Regex.Replace(sb, @"<[^>]+>|&nbsp;", "").Trim();
            string sc;
            if (s.Length > MaxLength)
            {
                sc = s.Substring(0, MaxLength);
                sc = sc + " ...";
                return sc;
            }
            return s;       
        }

    }
}
