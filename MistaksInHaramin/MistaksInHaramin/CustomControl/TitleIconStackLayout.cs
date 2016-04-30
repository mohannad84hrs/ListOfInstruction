using MistaksInHaramin.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MistaksInHaramin
{
    class TitleIconStackLayout : StackLayout
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Color BackColor { get; set; }
        public Color TextColor { get; set; }

        public TitleIconStackLayout(string title, string iconSource)
        {
            this.Title = title;
            this.IconSource = iconSource;
            this.BackColor = Color.FromHex("#0B6A62");
            this.TextColor = Color.White;
            InitializeControls();
        }
        public TitleIconStackLayout(string title, string iconSource, Color backColor)
        {
            this.Title = title;
            this.IconSource = iconSource;
            this.BackColor = backColor;
            this.TextColor = Color.White;
            InitializeControls();
        }
        public TitleIconStackLayout(string title, string iconSource, Color backColor, Color textColor)
        {
            this.Title = title;
            this.IconSource = iconSource;
            this.BackColor = backColor;
            this.TextColor = textColor;
            InitializeControls();
        }

        private void InitializeControls()
        {
            var imgIcon = new Image
            {
                Source = this.IconSource,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = this.BackColor,
                
            };

            var lblTitle = new Label()
            {
                Text = this.Title,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Start,
                FontSize = ZadSpecialDesigen.GetFontSizeOfTitle(),
                TextColor = this.TextColor,
                BackgroundColor = this.BackColor,
            };

            var grid = new Grid()
            {
                Padding = 0,
                ColumnSpacing = 0,
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(0.9, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) }
                },             
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) },
                },
                BackgroundColor = this.BackColor,
                HeightRequest = 80,
            };

            grid.Children.Add(lblTitle, 1, 1);
            grid.Children.Add(imgIcon, 2, 1);

            base.Children.Add(grid);
        }
    }
}
