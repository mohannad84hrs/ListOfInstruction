
using MistaksInHaramin.HelperClasses;
using Xamarin.Forms;

namespace MistaksInHaramin.Views
{
    class InfoPage : ContentPage
    {
        public Grid MainGrid;

        Image imgMedia;

        public InfoPage()
        {
            imgMedia = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = Helper.AboutInfoImageUri,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            MainGrid = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0,
               BackgroundColor = Color.White,
            };

            MainGrid.Children.Add(imgMedia);

            this.Content = MainGrid;
        }
    }
}
