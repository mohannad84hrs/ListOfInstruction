using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ImageCircle;
using ImageCircle.Forms.Plugin.Abstractions;
using System.Text.RegularExpressions;

namespace MistaksInHaramin.CustomControl
{
    class LayoutWithCircleImages : StackLayout
    {
        
        public LayoutWithCircleImages( int PostId)
        {
            var post = App.Data.Data.Find(x => x.Id.Equals(PostId));
                 
            this.HeightRequest = 85;
            Grid grid = new Grid {
                Padding = new Thickness(0, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 5,
                RowDefinitions =
                {
                    new RowDefinition { Height = 70},
                },
            };
            this.BackgroundColor = Color.White;
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(25, GridUnitType.Star) });
            var Title = new Label
            {
				Text = cutString(post.Content.ar.Title,16),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Start,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = ZadSpecialDesigen.ZadGreen
             ,
                HorizontalOptions = LayoutOptions.End
            };
            var Details = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.End,
                Text = cutString(post.Content.ar.Description,19),
                TextColor = Color.FromHex("#555")
                         ,
                HorizontalOptions = LayoutOptions.End
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
            var c = new CircleImage
            {
                BorderColor = Color.White,
                BorderThickness = 3,
         
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Source = ZadSpecialDesigen.ImageUrl + post.Content.ar.Attachments[0].Thumbnail,
            };
			if (Device.OS == TargetPlatform.iOS) {
				c.WidthRequest = 70;
				c.HeightRequest = 70;

			} else {
				c.WidthRequest = 115;
				c.HeightRequest = 115;
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
            grid.Children.Add(leftArrowImage, 0, 0);
            grid.Children.Add(TextBoxstackLayout, 1, 0);
     
            grid.Children.Add(c, 2, 0);
            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) => {
                await this.ScaleTo(0.95, 50, Easing.CubicOut);
                await this.ScaleTo(1, 50, Easing.CubicIn);
      
                new ViewData.PostDataType(post.Id);
            };

            grid.GestureRecognizers.Add(tap);
            this.Children.Add(grid);
        }

		private string cutString(string sb, int MaxLength)
        {
            string s = Regex.Replace(sb, @"<[^>]+>|&nbsp;", "").Trim();
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

