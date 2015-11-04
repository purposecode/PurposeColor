using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class TestPage : ContentPage
    {
        public TestPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Transparent;

            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();


            StackLayout layout = new StackLayout();
            layout.WidthRequest = deviceSpec.ScreenWidth;
            layout.HeightRequest = deviceSpec.ScreenHeight * 50 / 100;
            layout.BackgroundColor = Color.Transparent;


            StackLayout layout2 = new StackLayout();
            layout2.WidthRequest = deviceSpec.ScreenWidth;
            layout2.HeightRequest = deviceSpec.ScreenHeight * 50 / 100;
            layout2.BackgroundColor = Color.Red;


            masterLayout.AddChildToLayout( layout, 0, 0 );
            masterLayout.AddChildToLayout(layout2, 0, 50);

            Content = masterLayout;
        }
    }
}
