using MistaksInHaramin.CustomControl;
using MistaksInHaramin.HelperClasses;
using MistaksInHaramin.Models;
using MistaksInHaramin.ViewData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MistaksInHaramin.Views
{
    class MistakePlacesPage : BasicLayout
    {
        private readonly string MISTAKE_ICON_URI = "x_icon_green_bg.png";
        private readonly string DEFAULT_MISTAKE_IMAGE_URI = "default_blank_image.jpg";

        Cateogry misCategory;
        int areaID;
        public int AreaId { get { return areaID; } }

        RelatedPosts places;

        public Grid MainGrid;
        StackLayout MainStackLayout;

        Image imgMedia;
        Image imgIcon;

        public MistakePlacesPage(Cateogry mistakeCategory, int areaId)
        {          
            misCategory = mistakeCategory;
            areaID = areaId;
            this.Title = Helper.GetTitle(misCategory);
            Init();
        }

        void Init()
        {
            FillData();

            MainStackLayout = new StackLayout
            {
                Padding = 0,
                Spacing = 1,
            };

            foreach (var item in places.RelatedPost)
            {
                MainStackLayout.Children.Add(new LayoutWithCircleImages(item.Id));
            }

            imgMedia = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = DEFAULT_MISTAKE_IMAGE_URI,
                BackgroundColor = ZadSpecialDesigen.DefaultNoneImageColor,
				HeightRequest = 230,
            };

            imgIcon = new Image
            {
                Source = MISTAKE_ICON_URI,
                VerticalOptions = LayoutOptions.Start,
                TranslationY = -70,
                Scale = .4,
            };

            if (Device.OS == TargetPlatform.iOS)
            {
                imgIcon.TranslationY = -93;
                imgMedia.HeightRequest = 270;
            }

            MainGrid = new Grid()
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                BackgroundColor = ZadSpecialDesigen.DefaultNoneImageColor,
            };

			MainGrid.Children.Add(new StackLayout { BackgroundColor = Color.White, Children = { imgMedia }, Spacing = 0, Padding = new Thickness(0, 0, 0, 20) });
            MainGrid.Children.Add(MainStackLayout, 0, 1);
            MainGrid.Children.Add(imgIcon, 0, 1);

            this.MainBodyScrollView.Content = MainGrid;
        }

        private void FillData()
        {
            places = new RelatedPosts();

            List<Post> posts = App.Data.Data;
            if (posts != null && posts.Count > 0)
            {
                List<int> placesIds = new List<int>();

                foreach (Post item in posts)
                {
                    if (item == null || item.Content == null) // ignore the post that does not have a content
                        continue;

                    if (!Helper.IsExistsInArray(item.Id, misCategory.Posts)) // where this mistake is not related to this category
                        continue;

                    switch (item.Type)
                    {
                        case "place":
                            continue; // ignore places
                        case "azkar": // ignore azkar
                            continue;
                        case "ahkam": // ignore ahkam
                            continue;
                        case "mistake":
                            if (item.Relations != null && item.Relations.Place != null)
                                placesIds.AddRange(item.Relations.Place);
                            break;
                    }
                }

                if (placesIds.Count > 0) // if there is places related to this category
                {
                    IEnumerable<int> uniquePlaces = placesIds.Distinct<int>(); // distinct the repeated IDs

                    foreach (var id in uniquePlaces)
                    {
                        Post _post = Helper.GetPost(id);
                        if (Helper.IsRelatedToArea(_post, this.AreaId)) // where is place is related to this area
                        {
                            places.Add(_post);
                        }
                    }
                }
            }          
        }
    }
}
