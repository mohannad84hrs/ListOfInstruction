using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MistaksInHaramin
{
   
    public class OurCrousalPage : ContentPage
    {
    
       public Button btnEntry;
		Label lblTexti;
       
        RelativeLayout relativeLayout;
        public OurCrousalPage(string BbackgrounImageSource )
        {
            relativeLayout = new RelativeLayout();
			lblTexti = new Label{

				TextColor = Color.Gray
					,
				HorizontalOptions = LayoutOptions.Center
					,
				VerticalOptions = LayoutOptions.Center
					,
				XAlign = TextAlignment.Center
					, FontSize= Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};
			lblTexti.Text = "يتم الآن تحميل البيانات ...";
            Grid grid = new Grid {
				
			};
			Image img = new Image {
				Source = BbackgrounImageSource
					,
				Aspect = Aspect.AspectFill
			};
				
           // this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
            btnEntry = new Button
            {
                Text = "الدخول الى التطبيق",               
               FontSize=12,
                BackgroundColor = Color.FromRgb(24,113,107),
                TextColor = Color.White
                ,HeightRequest=45
    
            };
        
            relativeLayout.Children.Add(btnEntry,
                Constraint.RelativeToParent((parent) => {
                    btnEntry.WidthRequest = parent.Width / 1.2;
                    return parent.Width /12;
          
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height / 1.12;
                }));
			grid.Children.Add (img);
   
            grid.Children.Add (relativeLayout);
			this.Content = grid;
   
			relativeLayout.Children.Add(lblTexti, Constraint.RelativeToParent((parent) => {
				lblTexti.WidthRequest = parent.Width / 2;
				return parent.Width / 4;

			}), Constraint.RelativeToParent((parent) => {
				return parent.Height / 1.6;
			}));





        }
        public void AddText(string text)
        {
           
            lblTexti.Text = text;

     

        }
    }
}
