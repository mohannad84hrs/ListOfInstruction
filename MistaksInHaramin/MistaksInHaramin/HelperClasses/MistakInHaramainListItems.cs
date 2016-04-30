using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MistaksInHaramin.HelperClasses
{
    public class MistakInHaramainListItems
    {
        public string Name
        {
            set;
            get;
        }
        public string Icon
        {
            set;
            get;
        }
        public Color Textcolor { get; } = Color.White;

        public MistakInHaramainListItems(string name, string icon)
        {
            this.Name = name;
            this.Icon = icon;
        }


    }
}
