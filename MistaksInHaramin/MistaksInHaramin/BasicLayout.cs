using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MistaksInHaramin.HelperClasses;

namespace MistaksInHaramin

{
    public class BasicLayout : ContentPage
    {
        protected Grid _layout;
        protected StackLayout MainForAdjDataView;
        public ScrollView MainBodyScrollView;
        private StackLayout _panel;
        SearchBar Search;
        HelperClasses.ILocationProvider location;
        private ToolbarItem PlacesNearByMe;
        private ToolbarItem noPlacesorDisablePlaces;
        private const double ClosePlacesDistanceInKM = 0.3;// set the circle of close area by km 
        public BasicLayout()
        {
            // create the layout
            _layout = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
                ,            
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(100,GridUnitType.Star) },
                }
            };
            Search = new SearchBar
            {
                Placeholder = "البحث .."
                ,TextColor=ZadSpecialDesigen.ZadGreen
                , SearchCommand = new Command(DoSearch)
                ,BackgroundColor=Color.White
               ,PlaceholderColor=ZadSpecialDesigen.ZadGreen
               ,HorizontalTextAlignment=TextAlignment.End
            };



            var searchstack = new StackLayout
            {
                Padding = new Thickness(3, 7, 3, 0)
            };
            MainForAdjDataView = new StackLayout
            {
                BackgroundColor = ZadSpecialDesigen.ZadWhite
                       ,
                Padding = 0
            };
            ////////////////////////////////////////////////////
            MainBodyScrollView = new ScrollView();
            searchstack.Children.Add(Search);
            MainForAdjDataView.Children.Add(searchstack);
            MainForAdjDataView.Children.Add(MainBodyScrollView);
		
			this.ToolbarItems.Add(new ToolbarItem("Z", "bookmark_icon.png", ViewFavoriteListInPage, ToolbarItemOrder.Primary, 1));    
            this.ToolbarItems.Add(new ToolbarItem("S", "humburger.png", AnimatePanel, ToolbarItemOrder.Primary,2));
            if (Device.OS == TargetPlatform.Android)
            {
                this.PlacesNearByMe = new ToolbarItem("P", "bell_blank_red", ViewTheDistancePage, 0);
            }
            else
            {
                this.PlacesNearByMe = new ToolbarItem("P", "bell_with_point", ViewTheDistancePage, 0);
            }          
             this.noPlacesorDisablePlaces = new ToolbarItem("Pl", "bell_icon", doNothing, 0);
            _layout.Children.Add(MainForAdjDataView, 0, 0);             
            this.Disappearing += (o, e) =>
             {
                 try {
                     _layout.Children.Remove(_panel);
                 }
                 catch(Exception)
                 {

                 }
             };
            CreatePanel();
            location = DependencyService.Get<HelperClasses.ILocationProvider>();
            this.Content = _layout;
       
        }
        protected override void OnAppearing()
        {
            NoPlaces = false;
            WithPlaces = false;
            if( (SettingPage.LocationEnabled)&&(location.Enabled))
            {
                timerCanceled = false;
                noPlacesorDisablePlaces.Icon = "bell_icon";
        
                ToolbarItems.Remove(noPlacesorDisablePlaces);
                ToolbarItems.Remove(PlacesNearByMe);
                ClosePlaces = new List<Models.Post>();
                onTick();
            }
            else
            {
                timerCanceled = true;
                ToolbarItems.Remove(noPlacesorDisablePlaces);
                ToolbarItems.Remove(PlacesNearByMe);
                noPlacesorDisablePlaces.Icon = "bell_off";
                ToolbarItems.Add(noPlacesorDisablePlaces);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            isViewed = false;
            //ClosePlaces.Clear(); ;
        }



        /// <summary>
        /// Creates the right side menu panel
        /// </summary>
        private void CreatePanel()
        {
            if (_panel == null)
            {
                Image home = new Image
                {
                    Source = "home_icon_new.png"
                                 
                };

                Image setting = new Image
                {
                    Source = "gear_icon.png"
                   
                };
                Image info = new Image
                {
                    Source = "info_icon.png"
            
             
                };
                setting.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async (o) =>
                    {
                        await setting.ScaleTo(0.95, 50, Easing.CubicOut);
                        await setting.ScaleTo(1, 50, Easing.CubicIn);
                        await base.Navigation.PushAsync(new SettingPage());
                    })
                });
                home.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async (o) =>
                    {
                        await home.ScaleTo(0.95, 50, Easing.CubicOut);
                        await home.ScaleTo(1, 50, Easing.CubicIn);
                        await base.Navigation.PopToRootAsync();
                    })
                });

				info.GestureRecognizers.Add(new TapGestureRecognizer
					{
						Command = new Command(async (o) =>
							{
								await info.ScaleTo(0.95, 50, Easing.CubicOut);
								await info.ScaleTo(1, 50, Easing.CubicIn);
								await base.Navigation.PushAsync(new Views.InfoPage());
							})
					});
				

                _panel = new StackLayout
                {                 
                    Padding=new Thickness(10,45,10,5),
                    Spacing = 30,    
                    WidthRequest=30,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    BackgroundColor=Color.FromHex("#222")                  
                };
                _panel.Children.Add(home);
                _panel.Children.Add(setting);
                _panel.Children.Add(info);
            }

        
        }
        private void DoSearch(object obj)
        {
            List<Models.Post> result = new List<Models.Post>();
           
            try {
                List<Models.Content> contentlist = new List<Models.Content>();
                foreach (var post in App.Data.Data)
                {
                    contentlist.Add(post.Content.ar);
                }
                 result = App.Data.Data.FindAll(x => x.Content.ar.Title.Contains(Search.Text));
                if (result.Count==0)

                {
                    result = App.Data.Data.FindAll(x => x.Content.ar.Description.Contains(Search.Text));
                }
            }
         catch (Exception ex)
            {
                var g = ex;
            }

            ContentPage SearchResultPage = new ContentPage { BackgroundColor=ZadSpecialDesigen.ZadWhite};
            ScrollView scollResult = new ScrollView();
            SearchResultPage.Title = "نتائج البحث";
            var btnreturn = new Button { Text = "العودة", BackgroundColor = ZadSpecialDesigen.ZadGreen, TextColor = Color.White, BorderRadius = 0 };
            btnreturn.Clicked += delegate { base.Navigation.PopAsync(); };

            var header = new StackLayout
            {
                Padding = new Thickness(0, 10, 0, 10)
          
            };
          
            if (result.Count > 0)
            {
                header.Children.Add(new Label { Text = string.Format("عدد نتائج البحث {0}", result.Count.ToString()), HorizontalOptions = LayoutOptions.Center, XAlign = TextAlignment.Center, FontSize = 22 ,TextColor=ZadSpecialDesigen.ZadGreenNavBar });
                var searchResultLayout = new StackLayout
                {
                    Spacing = 0
                };
                searchResultLayout.Children.Add(header);
                bool switcher = true;
                foreach(var res in result)
                {
           
                        searchResultLayout.Children.Add(new CustomControl.FullDetailStackLayout(res.Id, true, switcher));
                        switcher = !switcher;
                }
            
                scollResult.Content = searchResultLayout;

            }
            else
            {
                header.Children.Add(new Label { Text = string.Format("لايوجد نتائج ل  {0}", Search.Text), HorizontalOptions = LayoutOptions.Center, XAlign = TextAlignment.Center, FontSize = 22, TextColor = ZadSpecialDesigen.ZadGreenNavBar });
                header.Children.Add(btnreturn);
                scollResult.Content = header;
            }

            
            SearchResultPage.Content = scollResult;

            base.Navigation.PushAsync(SearchResultPage);
            SearchResultPage = null;
            scollResult = null;

        }

   

        /// <summary>
        /// Animates the panel in our out depending on the state
        /// </summary>
        /// 
        private bool isViewed = false;
        private async void AnimatePanel()
        {

            // swap the state
  
            // show or hide the panel
            if (!isViewed)
            {
 
                _layout.Children.Add(_panel, 0, 0);
                // hide all children
                foreach (var child in _panel.Children)
                {
                    child.Scale = 0;
                }
                // layout the panel to slide out
                var rect = new Rectangle(_layout.Width - _panel.Width, _panel.Y, _panel.Width, _panel.Height);
                await this._panel.LayoutTo(rect, 150, Easing.CubicIn);

                // scale in the children for the panel
                foreach (var child in _panel.Children)
                {
                    await child.ScaleTo(1.2, 50, Easing.CubicIn);
                    await child.ScaleTo(1, 50, Easing.CubicOut);
                }
                isViewed = true;
     
            }
            else
            {                    
                // layout the panel to slide in
                var rect = new Rectangle(_layout.Width, _panel.Y, _panel.Width, _panel.Height);
                await this._panel.LayoutTo(rect, 100, Easing.CubicOut);
                isViewed = false;
                // hide all children
                _layout.Children.Remove(_panel);

            }
 
        }
   
       
        ContentPage FavoritePageList;
        async private void ViewFavoriteListInPage()
        {
            FavoritePageList = new ContentPage { BackgroundColor = ZadSpecialDesigen.ZadWhite, Title = "قائمة المفضلة" };
            FavoritePageList.BackgroundColor = ZadSpecialDesigen.ZadWhite;
            FillFovrite();
            await base.Navigation.PushAsync(FavoritePageList);
        }
       async private void FillFovrite()
        {
            var header = new StackLayout
            {
                Padding = 20
            };
            StackLayout favoritebody = new StackLayout { Spacing = 0 };
            var headerLable = new Label { HorizontalOptions = LayoutOptions.End, XAlign = TextAlignment.End, FontSize = 22, TextColor = ZadSpecialDesigen.ZadGreenNavBar };
            var FavList = await HelperClasses.Favorite.GetFavoritList();
            if (FavList.Count == 0)
            {
                headerLable.Text = "لا يوجد قائمة المفضلة";
                header.Children.Add(headerLable);
                favoritebody.Children.Add(header);
                FavoritePageList.Content= favoritebody;
            }
            else
            {
                headerLable.Text = "استعراض قائمة المفضلة";
            }
            header.Children.Add(headerLable);
            favoritebody.Children.Add(header);

           
            favoritebody.Children.Add(await fullFavoriteInList());
            favoritebody.Children.Add(new StackLayout { HeightRequest = 40, BackgroundColor = ZadSpecialDesigen.ZadGreen });
            ScrollView s = new ScrollView();
            s.Content = favoritebody;
            ////////////////////////////////
            //// Add the scoll view in favorite page
            ///////////////////////////////
            FavoritePageList.Content = s;// here to put the scorll view        
        }
  
        async private Task<StackLayout> fullFavoriteInList()
        {
            bool switcher = true;
            StackLayout current = new StackLayout();
            if (App.Data.Data != null)
            {
                foreach (var post in App.Data.Data)
                {
                    if (await HelperClasses.Favorite.inFavorite(post.Id))
                    {
                        var c = new CustomControl.FullDetailStackLayout(post.Id, true, switcher, true, async()=> {
                            await HelperClasses.Favorite.RemoveFromFavorite(post.Id);
                            FillFovrite();

                             });
                        current.Children.Add(c);
                        switcher = !switcher;
                    }
                }
            }
            return current;

        }

     

        /// <summary>
        /// GBS notification mangemnet system
        /// </summary>
        /// <returns></returns>
        List<Models.Post> ClosePlaces;
        private bool onTick()
        {
            if (timerCanceled) return false;

            var time = TimeSpan.FromSeconds(30);
            Device.StartTimer(time, onTick);
            if (location.GetCurrentLocation().Longitude != 0)
            {
                foreach (var p in App.Data.Data)
                {
                    double lon, lat;
                    if (p.Type == "place")
                    {
                        if (double.TryParse(p.Longitude, out lon) && (double.TryParse(p.Latitude, out lat)))
                            if (distance(location.GetCurrentLocation().Latitude, location.GetCurrentLocation().Longitude, lat, lon, 'k') < ClosePlacesDistanceInKM)
                            {
                                if(!ClosePlaces.Exists(x=>x.Id.Equals(p.Id)))
                                    ClosePlaces.Add(p);
                            }
                    }
                }

            }
            if (ClosePlaces.Count > 0)
            {
                if (!WithPlaces)
                {
                    this.ToolbarItems.Remove(noPlacesorDisablePlaces);
                    this.ToolbarItems.Remove(PlacesNearByMe);
                    this.ToolbarItems.Add(PlacesNearByMe);
                    WithPlaces = true;
                    NoPlaces = false;
                }

            }
            else
            {
                if (!NoPlaces)
                {
                    this.ToolbarItems.Remove(PlacesNearByMe);
                    this.ToolbarItems.Remove(noPlacesorDisablePlaces);
                    this.ToolbarItems.Add(noPlacesorDisablePlaces);
                    WithPlaces = false;
                    NoPlaces = true;
                }

            }
            

            return false;
        }
        private bool NoPlaces=false;
        private bool WithPlaces = false;

        private bool timerCanceled = false;
        async private void doNothing()
        { }


        async private void ViewTheDistancePage()
        {

            Random rnd = new Random();
            timerCanceled = true;
            var tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
        

            /////////////////////////////////////////
            var content = new StackLayout();
            foreach (var p in ClosePlaces)
            {
                var pful = new CustomControl.LayoutWithCircleImages(p.Id);

                content.Children.Add(pful);
                

            }
            var scrolbar = new ScrollView();
            scrolbar.Content = content;
            var CloseButon = new StackLayout { Padding = new Thickness(35, 0) };
            CloseButon.Children.Add(

                        new Button
                        {
                            Text = "لا شكرا"
                               ,
                            BackgroundColor = Color.White,
                            TextColor = Color.Red,
                            Command = new Command(async o =>
                            {
                                var result = rnd.Next(2) == 1;
                                await base.Navigation.PopAsync();

                                tcs.SetResult(result);

                            }
                               )
                        });
            var total = new Grid
            {
                BackgroundColor = ZadSpecialDesigen.ZadWhite,
                RowDefinitions =
                {
                  
                    new RowDefinition {Height=new GridLength(80,GridUnitType.Star) },
                    new RowDefinition {Height=new GridLength(20,GridUnitType.Star) }
                }
            };

            total.Children.Add(scrolbar, 0, 0);
            total.Children.Add(CloseButon, 0, 1);
            ContentPage c = new ContentPage
            {
                Title="قائمة بالاماكن القريبة",
                BackgroundColor = ZadSpecialDesigen.ZadWhite,
      
                Content = total
            };
            c.Disappearing += (o, e) => { timerCanceled = false; ClosePlaces.Clear(); };

          await  base.Navigation.PushAsync(c);

        }




        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }




    }



    }



