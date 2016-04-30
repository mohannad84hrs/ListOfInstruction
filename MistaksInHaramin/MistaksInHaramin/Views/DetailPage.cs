using MistaksInHaramin.CustomControl;
using MistaksInHaramin.HelperClasses;
using MistaksInHaramin.Models;
using MistaksInHaramin.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MistaksInHaramin.Views
{
    public class DetailPage : BasicLayout
    {     
        private readonly string DEFAULT_ICON_SHARE = "share_icon.png";

        PostDataType postData;
        public string TitleIconUri { get; set; }

        //PlaceDetailsPage placePage;
        ClickableImage imgFavorite;
        static WebView webViewPlayer;

        public DetailPage(PostDataType postDataType)
        {
            postData = postDataType;
            this.Title = postData.Title;

            if (postData.Post == null)
                return;

            this.TitleIconUri = postData.GetTitleIconUri();
            InitializeControls();
        }

        protected async void InitializeControls()
        {
            Image imgMedia;
            Label lblDescription;
            Grid gridTextLayout;
            Grid gridToolBar;
            Grid gridMedia;

            /////////////////////////////////////////////////////
            // Media Area show media wheather it's video or image
            /////////////////////////////////////////////////////
            string imageUri = postData.GetImageUri();

            imgMedia = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = imageUri,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ZadSpecialDesigen.DefaultNoneImageColor,
            };

            // /*////////////////////////////////////////////////////////////////////////////////////////
            

            gridMedia = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ColumnSpacing = 0,
                RowSpacing = 0,
                BackgroundColor = Color.Black,
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition {BindingContext  = new GridLength(1, GridUnitType.Star) }
                },
            };

            /// if media type is audio or video initialize webview with source
            if (postData.DataType == DataType.Video || postData.DataType == DataType.Audio)
            {
                if (webViewPlayer == null) //for first time
                {
                    webViewPlayer = new WebView
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Source = await GetHTMLWebViewSource(),
                    };
                }
                else
                    webViewPlayer.Source = await GetHTMLWebViewSource();

                var webStackLayout = new StackLayout
                {
                    BackgroundColor = Color.Black,
                    Children = { webViewPlayer },
                };
                gridMedia.Children.Add(webStackLayout);
            }
            else
                gridMedia.Children.Add(imgMedia);

            /////////////////////////////////////////////////////
            // Text Area for title and description (Paragraph)
            /////////////////////////////////////////////////////

            lblDescription = new Label()
            {
                Text = postData.Description,
                FontSize = ZadSpecialDesigen.GetFontSizeOfDescription(),
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Start,
                TextColor = Color.FromHex("#666666"),
                BackgroundColor = Color.White
            };
            StackLayout descriptionLayout = new StackLayout
            {
                Padding = ZadSpecialDesigen.GetPaddingOfDescription(),
            };
            descriptionLayout.Children.Add(lblDescription);

            gridTextLayout = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                BackgroundColor = Color.White
            };

            gridTextLayout.Children.Add(
                new TitleIconStackLayout(postData.Title, this.TitleIconUri, Color.FromHex("#F0F4F5"), Color.FromHex("#19716B"))
                , 0, 0);
            gridTextLayout.Children.Add(descriptionLayout, 0, 1);

            /////////////////////////////////////////////////////
            // Footer ToolBar Area for shared and favorite
            /////////////////////////////////////////////////////
            var imgShare = new ClickableImage { Source = DEFAULT_ICON_SHARE, };
            imgFavorite = new ClickableImage { Source = Helper.GetUnFavoriteIconUri(), };

            imgShare.Clicked += ImgShare_Clicked;
            imgFavorite.Clicked += ImgFavorite_Clicked;

            gridToolBar = new Grid()
            {
                RowSpacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(.1, GridUnitType.Star) },
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition {Height = new GridLength(.1, GridUnitType.Star) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(30, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(30, GridUnitType.Star) },
                },
                BackgroundColor = Color.FromHex("#199595"),
            };

            gridToolBar.Children.Add(imgFavorite, 1, 1);
            gridToolBar.Children.Add(imgShare, 2, 1);

            ////////////////////////////////////////////////////////////
            ///////////////////// Main grid //////////////////////////
            //////////////////////////////////////////////////////////
            var scrollview = new ScrollView
            {
                Content = gridTextLayout,
            };

            Grid MainGrid = new Grid()
            {
                Padding = new Thickness(0,-6,0,0),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                ColumnSpacing = 0,
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(0.46, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(0.46, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(0.08, GridUnitType.Star) },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            MainGrid.Children.Add(gridMedia, 0, 0);
            MainGrid.Children.Add(scrollview, 0, 1);
            MainGrid.Children.Add(gridToolBar, 0, 2);

            base.MainBodyScrollView.IsVisible = false;
            base.MainForAdjDataView.Children.Add(MainGrid);
            base.MainForAdjDataView.Padding = 0;
            base.MainForAdjDataView.Spacing = 0;


            if (postData.Description == null || postData.Description == "") // no content
                gridTextLayout.Children.Add(Helper.NoContentLable);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(imgFavorite != null && postData != null)
                imgFavorite.Source = await Helper.GetAutoFavoriteIcon(postData.PostID);
        }

        protected async override void OnDisappearing()
        {
            if(Device.OS == TargetPlatform.iOS && postData.DataType == DataType.Video) // for IOS do nothing in case of Video
            { }
			else // stop audio streaming for Video and Audio when leaving the page 
            if (postData.DataType == DataType.Video || postData.DataType == DataType.Audio)  
			{
                // change the webView Source is enough for Android version less than Lollipop.
                //if (webViewPlayer != null && Device.OS == TargetPlatform.Android)
                //{
                //    webViewPlayer.Source = new HtmlWebViewSource().Html = Helper.GetNoInternetConnectinHttpStyle();
                //}

                // In Case of IOS (Audio) and Android Lollipop and above(Video and Audio)
                // we need to initilize new webPlayer with new Source in order for audio background to be stopped.
                if (webViewPlayer == null)
				{
					webViewPlayer = new WebView
					{
						VerticalOptions = LayoutOptions.FillAndExpand,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Source = await Helper.GetAsyncNoInternetHTMLWebViewSource(),
					};
				}
				else
					webViewPlayer.Source = await Helper.GetAsyncNoInternetHTMLWebViewSource();
				

				var webStackLayout = new StackLayout
				{
					BackgroundColor = Color.Black,
					Children = { webViewPlayer },
				};
				this.Content = webStackLayout;
			}

            base.OnDisappearing();
        }
			

        private async Task<HtmlWebViewSource> GetHTMLWebViewSource()
        {
            var htmlSource = new HtmlWebViewSource();
            var networkStatus = DependencyService.Get<HelperClasses.INetworkConnection>();
            if (!networkStatus.IsConnected)
            {
                htmlSource.Html = Helper.GetNoInternetConnectinHttpStyle();
                return htmlSource;
            }
            htmlSource.Html =
                    @"<html><body>              
                <iframe width='100%' height='94%' src='" + postData.DataSource + @"' frameborder='0'></iframe>
            </body></html>";

            if (Device.OS == TargetPlatform.iOS)
            {
                htmlSource.Html =
                    @"<html><body>              
                <iframe  width='100%' height='50%' src='" + postData.DataSource + @"' frameborder='0'></iframe>
              </body></html>";
            }

            return htmlSource;
        }

        private void ImgShare_Clicked(object sender, EventArgs e)
        {
            if (postData.Post == null || postData.Post.Content == null)
                return;

            var shareable = DependencyService.Get<IShareable>();
            string msg = "";

            if (postData.DataType == DataType.Video || postData.DataType == DataType.Audio) // video or audio
                msg = postData.Description +"\n"+ postData.DataSource;
            else
                msg = postData.Description;

            shareable.ShareText(msg, Helper.ShareTitleOfChooseApp);
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
