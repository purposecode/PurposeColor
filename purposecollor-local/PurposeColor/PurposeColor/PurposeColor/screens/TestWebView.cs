using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class TestWebView : ContentPage
    {
        public TestWebView()
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
