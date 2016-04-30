using MistaksInHaramin.CustomControl;
using MistaksInHaramin.HelperClasses;
using MistaksInHaramin.Models;
using MistaksInHaramin.ViewData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Resources;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using MistaksInHaramin;
using MistaksInHaramin.Resx;

namespace MistaksInHaramin.Views
{
    enum CurrentMedia { Image, Map }
    public class PlaceDetailsPage : BasicLayout
    {
        private const string AZKAR_TITLE = "من الأدعية والأذكار";
        private const string MISTAKES_TITLE = "من الأخطاء التي تقع";
        private const string AHKAM_TITLE = "من الآداب والأحكام";
        private const string VIRTUE_TITLE = "من الخصائص والفضائل";

        private const string DEFAULT_LOADING_IMAGE = "loading.png";

        private const string PLACE_MAP_ICON_URI = "map_icon_green_bg.png";
        private const string PLACE_IMAGE_ICON_URI = "image_icon_green_bg";
        private const string MAP_VIEW_ICON_URI = "enlarge_map_gree_icon.png";

        private const string AZKAR_ICON_URI = "hands_icon_clear.png";
        private const string MISTAKES_ICON_URI = "x_icon_clear.png";
        private const string AHKAM_ICON_URI = "book_icon_clear.png";
        private const string VIRTUE_ICON_URI = "vertues_icon_round_small.png";// to do///////////////

        private const string AZKAR_FOOTER_ICON_URI = "hands_icon_clear.png";
        private const string AHKAM_FOOTER_ICON_URI = "book_icon_clear.png";
        private const string MISTAKES_FOOTER_ICON_URI = "x_icon_clear.png";
        private const string VIRTUE_FOOTER_ICON_URI = "vertues_icon_round_small.png";  //To do:////////////

        PostDataType postData;
        public PostDataType PostDataType { get { return postData; } }

        PostType relatedFilter;
        public PostType RelatedPostFilter { get { return relatedFilter; } }

        CurrentMedia currentMedia;

        RelatedPosts azkar;
        RelatedPosts mistakes;
        RelatedPosts ahkam;
        RelatedPosts virtue;

        public Grid MainGrid;
        StackLayout MainStackLayout;

        StackLayout azkarStackLayout;
        StackLayout mistakesStackLayout;
        StackLayout ahkamStackLayout;
        StackLayout virtueStackLayout;

        Grid gridToolBar;
        Grid gridMedia;
        Image imgMedia;
        ClickableImage imgMapPlaceIcon;
        ClickableImage imgMapViewIcon;
        ClickableImage imgFavorite;
        ClickableImage imgAzkar;
        ClickableImage imgAhkam;
        ClickableImage imgMistakes;
        ClickableImage imgVirtue;
        Label lblPlaceTitle;
        Label lblPlaceDescription;
        ScrollView RelatedScrollView;

        public PlaceDetailsPage() { }
        public PlaceDetailsPage(PostDataType postDataType)
        {
            postData = postDataType;
            relatedFilter = PostType.Place;// default filter is place whear everyting is showing
            this.Title = postData.Title;
            Init();
        }

        /// <summary>
        /// Additional default constructer to define a place with specific type of related posts..
        /// for ex: place with mistakes only, or place with supplications only. etc..
        /// </summary>
        /// <param name="postDataType"> that has post and anlylize its content</param>
        /// <param name="relatedPostFilter"> the type of related posts which we need the filter with it</param>
        public PlaceDetailsPage(PostDataType postDataType, PostType relatedPostFilter, string title)
        {
            postData = postDataType;
            relatedFilter = relatedPostFilter;
            this.Title = title;
            Init();
        }

        async void Init()
        {
            currentMedia = CurrentMedia.Image;

            ///////////// Initilize Related Post ////////////
            azkar = new RelatedPosts(AppResources.AzkarAndSupplicationsTitle, AZKAR_ICON_URI, RelatedPostType.Supplications);
            mistakes = new RelatedPosts(AppResources.MistakesOccurredTitle, MISTAKES_ICON_URI, RelatedPostType.Mistakes);
            ahkam = new RelatedPosts(AppResources.AhkamAndRulesTitle, AHKAM_ICON_URI, RelatedPostType.Rules);
            virtue = new RelatedPosts(AppResources.FeaturesAndVirtues, VIRTUE_ICON_URI, RelatedPostType.Virtue);

            List<Post> posts = App.Data.Data;

            posts = posts.Where(x => RelatedPosts.IsRelated(x, postData.PostID)).ToList();
            if (posts != null && posts.Count > 0)
            {
                foreach (Post item in posts)
                {
                    if (item == null || item.Content == null) // ignore the post that does not have a content
                        continue;

                    switch (item.Type)
                    {
                        case "place":
                            continue; // ignore
                        case "azkar":
                            if (this.RelatedPostFilter == PostType.Place || this.RelatedPostFilter == PostType.Azkar)
                                azkar.Add(item);
                            break;
                        case "mistake":
                            if (this.RelatedPostFilter == PostType.Place || this.RelatedPostFilter == PostType.Mistake)
                                mistakes.Add(item);
                            break;
                        case "ahkam":
                            if (this.RelatedPostFilter == PostType.Place || this.RelatedPostFilter == PostType.Ahkam)
                                ahkam.Add(item);
                            break;
                        case "virtue":
                            if (this.RelatedPostFilter == PostType.Place || this.RelatedPostFilter == PostType.Virtue)
                                virtue.Add(item);
                            break;
                    }
                }
            }

            InitializeControls();
            imgFavorite.Source = await Helper.GetAutoFavoriteIcon(postData.PostID);

            MainGrid = new Grid()
            {
                Padding = new Thickness(0, -6, 0, 0),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(.92, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(.08, GridUnitType.Star) },
                },
            };
            RelatedScrollView = new ScrollView() { Content = MainStackLayout };
            MainGrid.Children.Add(RelatedScrollView);
            MainGrid.Children.Add(gridToolBar, 0, 1);

            this.MainBodyScrollView.Content = null;
            base.MainForAdjDataView.Children.Add(MainGrid);
        }

        public void InitializeControls()
        {
            MainStackLayout = new StackLayout
            {
                Padding = 0,
                Spacing = 1,
            };

            azkarStackLayout = new StackLayout
            {
                Padding = 0,
                Spacing = 1,
            };

            mistakesStackLayout = new StackLayout
            {
                Padding = 0,
                Spacing = 1,
            };

            ahkamStackLayout = new StackLayout
            {
                Padding = 0,
                Spacing = 1,
            };

            virtueStackLayout = new StackLayout
            {
                Padding = 0,
                Spacing = 1,
            };
            /////////////////////////////////////////////////////
            // Image of place and description
            /////////////////////////////////////////////////////
            imgMedia = new Image
            {
                Source = postData.GetImageUri(),
                Aspect = Aspect.Fill,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,

            };

            imgMapPlaceIcon = new ClickableImage
            {
                Source = PLACE_MAP_ICON_URI,
                VerticalOptions = LayoutOptions.Start,
                TranslationY = -30,
                Scale = .9,
            };

            imgMapViewIcon = new ClickableImage
            {
                Source = MAP_VIEW_ICON_URI,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                TranslationX = -10,
                TranslationY = 10,
                IsVisible = false,
                Scale = .8,
            };

            if (Device.OS == TargetPlatform.iOS)
            {
                imgMapPlaceIcon.Scale = .45;
                imgMapPlaceIcon.TranslationY = -72;
                imgMapViewIcon.Scale = .45;

                imgMapViewIcon.TranslationX = +5;
                imgMapViewIcon.TranslationY = -5;
            }

            imgMapPlaceIcon.Clicked += ImgMapPlaceIcon_Clicked;
            imgMapViewIcon.Clicked += ImgMapViewIcon_Clicked;


            lblPlaceTitle = new Label()
            {
                Text = postData.Title,
                VerticalOptions = LayoutOptions.FillAndExpand,
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Start,
                FontSize = ZadSpecialDesigen.GetFontSizeOfTitle(),
                TextColor = ZadSpecialDesigen.ZadGreen,
                //BackgroundColor = ZadSpecialDesigen.ZadWhite,
            };
            lblPlaceDescription = new Label()
            {
                Text = postData.Description,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = ZadSpecialDesigen.GetFontSizeOfTitle(),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Start,
                TextColor = ZadSpecialDesigen.GetTextColorOfDescription(),
                //BackgroundColor = Color.FromHex("#F3F7F8"),////
            };

            gridMedia = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(250,GridUnitType.Absolute) },
                    new RowDefinition { Height = GridLength.Auto },
                },
                BackgroundColor = Color.FromHex("#F3F7F8"),
            };

            var stackDescription = new StackLayout()
            {
                Padding = ZadSpecialDesigen.GetPaddingOfDescription(),
                BackgroundColor = Color.FromHex("#F3F7F8"),////
            };

            stackDescription.VerticalOptions = LayoutOptions.Fill;
            //stackDescription.HorizontalOptions = LayoutOptions.FillAndExpand;
            stackDescription.Children.Add(lblPlaceTitle);
            stackDescription.Children.Add(lblPlaceDescription);

            //// image for loading status which apper under the media image and it will be disappear when the image media cove over it.
            gridMedia.Children.Add(new StackLayout
            {
                Padding = 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = { new Image { Source = DEFAULT_LOADING_IMAGE, } },
            }, 0, 0);

            gridMedia.Children.Add(imgMedia, 0, 0);
            gridMedia.Children.Add(imgMapViewIcon, 0, 0);
            gridMedia.Children.Add(stackDescription, 0, 1);
            gridMedia.Children.Add(imgMapPlaceIcon, 0, 1);

            MainStackLayout.Children.Add(gridMedia);
            MainStackLayout.Children.Add(azkarStackLayout);
            MainStackLayout.Children.Add(mistakesStackLayout);
            MainStackLayout.Children.Add(ahkamStackLayout);
            MainStackLayout.Children.Add(virtueStackLayout);


            /////////////////////////////////////////////////////
            ///////     Related Post Layout
            /////////////////////////////////////////////////////
            FillRelatedPostsLayout(PostType.Place);

            /////////////////////////////////////////////////////
            ///////     Footer toolBar Area
            /////////////////////////////////////////////////////
            imgAzkar = new ClickableImage { Source = AZKAR_FOOTER_ICON_URI, };
            imgAhkam = new ClickableImage { Source = AHKAM_FOOTER_ICON_URI, };
            imgMistakes = new ClickableImage { Source = MISTAKES_FOOTER_ICON_URI, };
            imgVirtue = new ClickableImage { Source = VIRTUE_FOOTER_ICON_URI };
            imgFavorite = new ClickableImage { Source = Helper.GetUnFavoriteIconUri() };

            imgAzkar.Clicked += ImgAzkar_Clicked;
            imgAhkam.Clicked += ImgAhkam_Clicked;
            imgMistakes.Clicked += ImgMistakes_Clicked;
            imgVirtue.Clicked += ImgVirtue_Clicked;
            imgFavorite.Clicked += ImgFavorite_Clicked;

            if (!azkar.HasData())
                imgAzkar.Disabled = true;

            if (!mistakes.HasData())
                imgMistakes.Disabled = true;

            if (!ahkam.HasData())
                imgAhkam.Disabled = true;

            if (!virtue.HasData())
                imgVirtue.Disabled = true;

            gridToolBar = new Grid()
            {
                ColumnSpacing = 20,
                Padding = 5,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(.20, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(.20, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(.20, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(.20, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(.20, GridUnitType.Star) },
                },
                BackgroundColor = Color.FromHex("#189597"),
            };

            gridToolBar.Children.Add(imgAzkar, 0, 0);
            gridToolBar.Children.Add(imgAhkam, 1, 0);
            gridToolBar.Children.Add(imgMistakes, 2, 0);
            gridToolBar.Children.Add(imgVirtue, 3, 0);
            gridToolBar.Children.Add(imgFavorite, 4, 0);
        }

        private void FillRelatedPostsLayout(PostType filterType)
        {
            //RelatedPostStackLayout.Children.Clear();

            if ((filterType == PostType.Place || filterType == PostType.Azkar) && azkar.HasData())
            {
                TitleIconStackLayout titleLayout = new TitleIconStackLayout(azkar.Title, azkar.TitleIconUri);
                azkarStackLayout.Children.Add(titleLayout);
                foreach (var item in azkar.RelatedPost)
                {
                    azkarStackLayout.Children.Add(new FullDetailStackLayout(item.Id, false, true));
                }
            }

            if ((filterType == PostType.Place || filterType == PostType.Mistake) && mistakes.HasData())
            {
                TitleIconStackLayout titleLayout = new TitleIconStackLayout(mistakes.Title, mistakes.TitleIconUri);
                mistakesStackLayout.Children.Add(titleLayout);
                foreach (var item in mistakes.RelatedPost)
                {
                    mistakesStackLayout.Children.Add(new FullDetailStackLayout(item.Id, false, true));
                }
            }

            if ((filterType == PostType.Place || filterType == PostType.Ahkam) && ahkam.HasData())
            {
                TitleIconStackLayout titleLayout = new TitleIconStackLayout(ahkam.Title, ahkam.TitleIconUri);
                ahkamStackLayout.Children.Add(titleLayout);
                foreach (var item in ahkam.RelatedPost)
                {
                    ahkamStackLayout.Children.Add(new FullDetailStackLayout(item.Id, false, true));
                }
            }

            if ((filterType == PostType.Place || filterType == PostType.Virtue) && virtue.HasData())
            {
                TitleIconStackLayout titleLayout = new TitleIconStackLayout(virtue.Title, virtue.TitleIconUri);
                virtueStackLayout.Children.Add(titleLayout);
                foreach (var item in virtue.RelatedPost)
                {
                    virtueStackLayout.Children.Add(new FullDetailStackLayout(item.Id, false, true));
                }
            }
        }
        // */

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (imgFavorite != null && postData != null)
                imgFavorite.Source = await Helper.GetAutoFavoriteIcon(postData.PostID);

            //var location = DependencyService.Get<ILocationProvider>();
            //await DisplayAlert("GPS notify", location.GetCurrentLocation().Latitude + " : " + location.GetCurrentLocation().Longitude, "OK");
        }

        private void ImgMapViewIcon_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new Views.MapPage(postData.Coordinate, ZadSpecialDesigen.DefaultCoordinateDistance
                 , new Pin() { Position = postData.Coordinate, Label = postData.Title }, postData.Title));

            ///////////////////////////////////////////
            var mapAddress = postData.MapAddress;

            /* var address = "Mecca+Saudi+Arabia/@21.422285,39.8253242,509m/data=!3m1!1e3!4m2!3m1!1s0x15c21b4ced818775:0x98ab2469cf70c9ce!6m1!1e1?hl=en";
            switch (Device.OS)
            {
                case TargetPlatform.iOS:
                    Device.OpenUri(
                      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(address))));
                    break;
                case TargetPlatform.Android:
                    Device.OpenUri(
                      new Uri(string.Format("geo:0,0?q={0}", WebUtility.UrlEncode(address))));
                    break;
                case TargetPlatform.Windows:
                case TargetPlatform.WinPhone:
                    Device.OpenUri(
                      new Uri(string.Format("bingmaps:?where={0}", Uri.EscapeDataString(address))));
                    break;
            }
            // */
        }

        private void ImgMapPlaceIcon_Clicked(object sender, EventArgs e)
        {
            // /*
            switch (currentMedia)
            {
                case CurrentMedia.Image:
                    if (postData.HasMapImage())
                    {
                        imgMapPlaceIcon.Source = PLACE_IMAGE_ICON_URI; // Switch the clickable icon back to place
                        currentMedia = CurrentMedia.Map;
                        if (postData.HasCoordinate())
                            imgMapViewIcon.IsVisible = true;
                        imgMedia.Source = postData.GetMapImageSource();
                    }
                    break;
                case CurrentMedia.Map:
                    if (postData.HasPlaceImage())
                    {
                        imgMapPlaceIcon.Source = PLACE_MAP_ICON_URI; // Switch the clickable icon back to map
                        currentMedia = CurrentMedia.Image;
                        imgMapViewIcon.IsVisible = false;
                        imgMedia.Source = postData.GetPlaceImageSource();
                    }
                    break;
            }
        }

        private void ImgAzkar_Clicked(object sender, EventArgs e)
        {
            //base.Navigation.PushAsync(new Views.PlaceDetailsPage(postData, PostType.Azkar, azkar.Title + " - " + postData.Title));
            RelatedScrollView.ScrollToAsync(azkarStackLayout, ScrollToPosition.Start, true);

        }

        private void ImgAhkam_Clicked(object sender, EventArgs e)
        {
            //base.Navigation.PushAsync(new Views.PlaceDetailsPage(postData, PostType.Ahkam, ahkam.Title + " - " + postData.Title));
            RelatedScrollView.ScrollToAsync(ahkamStackLayout, ScrollToPosition.Start, true);
        }

        private void ImgMistakes_Clicked(object sender, EventArgs e)
        {
            //base.Navigation.PushAsync(new Views.PlaceDetailsPage(postData, PostType.Mistake, mistakes.Title + " - " + postData.Title));
            RelatedScrollView.ScrollToAsync(mistakesStackLayout, ScrollToPosition.Start, true);
        }
        private void ImgVirtue_Clicked(object sender, EventArgs e)
        {
            RelatedScrollView.ScrollToAsync(virtueStackLayout, ScrollToPosition.Start, true);
        }

        private async void ImgFavorite_Clicked(object sender, EventArgs e)
        {
            if (postData == null)
                return;

            imgFavorite.Source = Helper.GetIsFavoriteIconUri();

            bool isFav = await Favorite.inFavorite(postData.PostID);

            if (!isFav)
            {
                Favorite.AddToFavofite(postData.PostID);
            }
        }

    }
}
