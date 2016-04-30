using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace MistaksInHaramin.CustomControl
{
    public class PlacesViewer : Grid 
    {

     
        public void AddPlacetoStackLayout(int postID, int left, int top)
        {
            MyLable title, detials;
            AllObject all = new AllObject(postID);
            title = new MyLable();
            detials = new MyLable(true);
            var post = App.Data.Data.Find(a => a.Id.Equals(postID));
            title.Text = cutString(post.Content.ar.Title, 14);
            detials.Text = cutString(post.Content.ar.Description, 16);
            all.middlerange.Children.Add(title, 1, 0);
            all.middlerange.Children.Add(all.arrowIcon, 0, 0);

            all.currentGrid.Children.Add(all.middlerange, 0, 1);

            all.currentGrid.Children.Add(detials, 0, 2);
            if (post.Content.ar.Attachments.Count > 1)
            {
                if ((post.Content.ar.Attachments[0].Thumbnail.IndexOf("map") < 0))
                    all.image.Source = post.Content.ar.Attachments.Count == 0 ? "default-piece-image-0.png" : ZadSpecialDesigen.ImageUrl + post.Content.ar.Attachments[0].Thumbnail;
                else
                    all.image.Source = post.Content.ar.Attachments.Count == 0 ? "default-piece-image-0.png" : ZadSpecialDesigen.ImageUrl + post.Content.ar.Attachments[1].Thumbnail;
            }
            else
            {
                all.image.Source = post.Content.ar.Attachments.Count == 0 ? "default-piece-image-0.png" : ZadSpecialDesigen.ImageUrl + post.Content.ar.Attachments[0].Thumbnail;
            }

            this.Children.Add(all.image, left, top);
            this.Children.Add(all.shadow, left, top);
            this.Children.Add(all.currentGrid, left, top);
            all.Dispose();
            post = null;
        }
     
        public PlacesViewer()
        {

        
            this.BackgroundColor = ZadSpecialDesigen.ZadWhite;

        }
        private string cutString(string s, int wordcount)
        {
            string sc;
            if (s.Length > wordcount)
            {
                sc = s.Substring(0, wordcount);
                sc = sc + " ...";
                return sc;
            }
            return s;
        }


     
    }
    class MyLable :Label
    {
        public MyLable() {
            TextColor = Color.White;
            HorizontalOptions = LayoutOptions.End;
            FontAttributes = FontAttributes.Bold;
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            XAlign = TextAlignment.End;
       }
        public MyLable(bool details)
        {
            TextColor = Color.White;
            HorizontalOptions = LayoutOptions.End;
            FontAttributes = FontAttributes.Bold;
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            XAlign = TextAlignment.End;
        }
    }



    public class AllObject :IDisposable
    {

      public  Grid currentGrid;
        public Grid middlerange;
        public Image image, shadow, arrowIcon;
       public AllObject( int id )
        {
            currentGrid = new Grid { Padding = 5 };
            middlerange = new Grid { Padding = 0 };
            var middleline = new RowDefinition();
            arrowIcon = new Image { Source = "arrow_icon.png" };
            shadow = new Image { Source = "gradient.png", Aspect = Aspect.AspectFill };
            currentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Star) });
            currentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(22, GridUnitType.Star) });
            currentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(22, GridUnitType.Star) });

            middlerange.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(9, GridUnitType.Star) });
            middlerange.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(85, GridUnitType.Star) });
           currentGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async (o) =>
                {
                    await currentGrid.ScaleTo(0.95, 50, Easing.CubicOut);
                    new ViewData.PostDataType(id);
                    await currentGrid.ScaleTo(1, 50, Easing.CubicIn);
     
                })
            });
            image = new Image
            {
                Aspect = Aspect.Fill,
            };
        }

        public void Dispose()
        {
            this.image = null;
            this.middlerange = null;
            this.arrowIcon = null;
            this.shadow = null;

        }
        ~AllObject()
        {
            Dispose();
        }
    }
   


}
