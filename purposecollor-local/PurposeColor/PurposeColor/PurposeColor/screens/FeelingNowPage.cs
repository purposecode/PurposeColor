using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PurposeColor
{
    public class FeelingNowPage : ContentPage
    {
        CustomSlider slider;

        public FeelingNowPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Gray;

            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            slider = new CustomSlider
            {
                Minimum = 0,
                Maximum = 100,
                WidthRequest = deviceSpec.ScreenWidth * 90 / 100
            };

            this.Appearing += FeelingNowPage_Appearing;
            masterLayout.AddChildToLayout(slider, 5, 30);

            Content = masterLayout;

        }

      async  void FeelingNowPage_Appearing(object sender, System.EventArgs e)
        {
            int val = 2;
            for( int index = 0; index < 200; index++ )
            {
                await Task.Delay(2);

                if( slider.Value > 90 )
                {
                    val = -2;
                }
                slider.Value += val;
            }
        }
    }
}
