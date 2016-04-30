using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using MistaksInHaramin.HelperClasses;

namespace MistaksInHaramin.Views
{
    public class MapPage : ContentPage
    {
        Map map;
        Position geoPosition;
        double meterDistance;

        public MapPage(double latitude, double longitude, double _meterDistance)
        {
            meterDistance = _meterDistance;
            geoPosition = new Position(latitude, longitude);
            Init();
        }

        public MapPage(Position longLatPosition, double _meterDistance)
        {
            meterDistance = _meterDistance;
            geoPosition = longLatPosition;
            Init();
        }

        public MapPage(Position longLatPosition, double _meterDistance, Pin pin)
        {
            meterDistance = _meterDistance;
            geoPosition = longLatPosition;
            Init();
            map.Pins.Add(pin);
        }
        public MapPage(Position longLatPosition, double _meterDistance, Pin pin, string title)
        {
            meterDistance = _meterDistance;
            geoPosition = longLatPosition;
            Init();
            map.Pins.Add(pin);
            this.Title = title;
        }

        public void Init()
        {
            map = new Map
            {
                IsShowingUser = false, // to show GPS button
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            // You can use MapSpan.FromCenterAndRadius 
            map.MoveToRegion(MapSpan.FromCenterAndRadius(geoPosition, Distance.FromMeters(meterDistance)));

            // or create a new MapSpan object directly			
            //map.MoveToRegion (new MapSpan (geoPosition, 360, 360) );

            // add the slider
            var slider = new Slider(1, 18, 16);
            slider.ValueChanged += (sender, e) => {
                var zoomLevel = e.NewValue; // between 1 and 18
                var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
                Debug.WriteLine(zoomLevel + " -> " + latlongdegrees);
                if (map.VisibleRegion != null)
                    map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongdegrees, latlongdegrees));
            };


            // create map style buttons
            var street = new Button { Text = "Street" };
            var hybrid = new Button { Text = "Hybrid" };
            var satellite = new Button { Text = "Satellite" };
            street.Clicked += HandleClicked;
            hybrid.Clicked += HandleClicked;
            satellite.Clicked += HandleClicked;
            var segments = new StackLayout
            {
                Spacing = 30,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = { street, hybrid, satellite }
            };


            // put the page together
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            stack.Children.Add(slider);
            stack.Children.Add(segments);
            Content = stack;

        }
        void HandleClicked(object sender, EventArgs e)
        {
            var b = sender as Button;
            switch (b.Text)
            {
                case "Street":
                    map.MapType = MapType.Street;
                    break;
                case "Hybrid":
                    map.MapType = MapType.Hybrid;
                    break;
                case "Satellite":
                    map.MapType = MapType.Satellite;
                    break;
            }
        }

        public void AddPin(Pin pin)
        {
            map.Pins.Add(pin);
        }

        public void AddPin(Position longLatPosition, string lable)
        {
            var pin = new Pin()
            {
                Position = longLatPosition,
                Label = lable
            };
            map.Pins.Add(pin);
        }

        public void AddPin(double longitude, double latitude, string lable)
        {
            var pin = new Pin()
            {
                Position = new Position(longitude, longitude),
                Label = lable
            };
            map.Pins.Add(pin);
        }

        public void AddPin(string address, string lable)
        {
            var pin = new Pin()
            {
                Address = address,
                Label = lable
            };
            map.Pins.Add(pin);
        }

    }
}
