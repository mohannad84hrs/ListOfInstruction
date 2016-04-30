using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MistaksInHaramin.CustomControl
{
    /// <summary>
    /// Custom Image Control that has an click event handler,
    /// as well as shows to end user that clicked event has took place.
    /// </summary>
    class ClickableImage : Image
    {
        TapGestureRecognizer tapGestureRecognizer = null;
        public ClickableImage() : base() {
             tapGestureRecognizer = new TapGestureRecognizer();

            // create delay for 200 ms just to shwo that image is clicked.
            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
                this.Opacity = .5;
                await Task.Delay(200);
                this.Opacity = 1;
            };

            // Add an event click when tapGestureRecognizer is Tapped
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;

            this.GestureRecognizers.Add(tapGestureRecognizer);           
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (Clicked != null)
            {
                Clicked(this, e);
            }
        }


        /// <summary>
        /// Disable click event and reduce the opacity of image.
        /// </summary>
        public bool Disabled {
            set
            {
                if (value && this.GestureRecognizers.Count > 0)
                { 
                    this.GestureRecognizers.Remove(tapGestureRecognizer);
                    this.Opacity = .7;
                }
                else if (GestureRecognizers.Count == 0) // where (value == ture) && there is no any tapGestureRecognizer
                { 
                    this.GestureRecognizers.Add(tapGestureRecognizer);
                    this.Opacity = 1;
                }
            }
        }

        //
        // Summary:
        //     Occurs when the Image is clicked.
        //
        // Remarks:
        //     The developer may be able to raise the clicked event using accessibility or keyboard
        //     controls when the Button has focus.
        public event EventHandler Clicked;

    }
}
