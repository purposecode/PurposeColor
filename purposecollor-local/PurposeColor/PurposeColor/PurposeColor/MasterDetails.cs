using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace PurposeColor
{
    public class MasterDetails : ContentPage
    {
        public MasterDetails()
        {
            Content = new StackLayout
            {
                Children = {
					new Label { Text = "Hello ContentPage" }
				}
            };
        }
    }
}
