using CustomControls;
using ImageCircle.Forms.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.CustomControls
{
    public class PurposeColorSubTitleBar : ContentView, IDisposable
    {
        CustomLayout masterLayout;
        public TapGestureRecognizer BackButtonTapRecognizer;
        public Button NextButton;
        Label title;

        public PurposeColorSubTitleBar(Color backGroundColor, string titleValue, bool nextButtonVisible = true, bool backButtonVisible = true )
        {
            Cross.IDeviceSpec spec = DependencyService.Get<Cross.IDeviceSpec>();
            int titlebarHeight = (int)spec.ScreenHeight * 7 / 100;
            int titlebarWidth = (int)spec.ScreenWidth;
            this.BackgroundColor = backGroundColor;

            masterLayout = new CustomLayout();
            masterLayout.HeightRequest = titlebarHeight;
            masterLayout.WidthRequest = titlebarWidth;
            masterLayout.BackgroundColor = backGroundColor;

            Image bgImage = new Image();
            bgImage.Source = Device.OnPlatform("top_bg.png", "top_bg.png", "//Assets//top_bg.png");
            bgImage.WidthRequest = spec.ScreenWidth;
            bgImage.HeightRequest = titlebarHeight;
            bgImage.Aspect = Aspect.Fill;

            Image backArrow = new Image();
            backArrow.Source = Device.OnPlatform("back_arrow.png", "back_arrow.png", "//Assets//back_arrow.png");
            backArrow.HeightRequest = spec.ScreenHeight * 4 / 100;
            backArrow.WidthRequest = spec.ScreenWidth * 5 / 100;
            BackButtonTapRecognizer = new TapGestureRecognizer();
            backArrow.GestureRecognizers.Add(BackButtonTapRecognizer);

            Image imgDivider = new Image();
            imgDivider.Source = Device.OnPlatform("top_seperate.png", "top_seperate.png", "//Assets//top_seperate.png");
            imgDivider.HeightRequest = spec.ScreenHeight * 4 / 100;


            NextButton = new Button();
            NextButton.Text = "Next";
            NextButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            NextButton.FontSize = Device.OnPlatform( 12,12,18 );
            NextButton.TextColor = Color.White;
            NextButton.BackgroundColor = Color.Transparent;
            NextButton.BorderColor = Color.Transparent;
            NextButton.BorderWidth = 0;
            NextButton.WidthRequest = spec.ScreenWidth * Device.OnPlatform( 15,15,25 ) / 100;
            NextButton.HeightRequest = spec.ScreenHeight * Device.OnPlatform( 5,5,8 ) / 100;


            title = new Label();
            title.Text = titleValue;
            title.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            title.FontSize = Device.OnPlatform( 17, 20, 22 );
            title.TextColor = Color.Black;

            Image logo = new Image();
            logo.Source = Device.OnPlatform("logo.png", "logo.png", "//Assets//logo.png");
            logo.WidthRequest = spec.ScreenWidth;
            logo.HeightRequest = titlebarHeight;
            logo.WidthRequest = spec.ScreenWidth * 70 / 100;
            logo.HeightRequest = spec.ScreenHeight * 8 / 100;


           // masterLayout.AddChildToLayout(bgImage, 0, 0, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(title, 20, 22, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(imgDivider, 75, 25, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);


            if( nextButtonVisible )
            {
                masterLayout.AddChildToLayout(NextButton, Device.OnPlatform( 83, 83, 76 ), Device.OnPlatform( 10, 10, -5 ), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            }

            if (backButtonVisible)
            {
                masterLayout.AddChildToLayout(backArrow, 3, 25, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            }

            Content = masterLayout;

        }

        public void Dispose()
        {
            masterLayout = null;
            BackButtonTapRecognizer = null;
            NextButton = null;
            title = null;
            GC.Collect();
        }
        
    }
}
