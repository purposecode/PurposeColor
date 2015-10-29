using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor
{
    public class FeelingNowPage : ContentPage
    {
        public FeelingNowPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Gray;

            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            CustomSlider slider = new CustomSlider
            {
                Minimum = 0,
                Maximum = 10,
                WidthRequest = deviceSpec.ScreenWidth * 90 / 100
            };

            masterLayout.AddChildToLayout(slider, 5, 30);

            Content = masterLayout;

        }
    }
}
