using CustomControls;
using ImageCircle.Forms.Plugin.Abstractions;
using PurposeColor.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.CustomControls
{
    public class PurposeColorBlueSubTitleBar : ContentView, IDisposable
    {
        CustomLayout masterLayout;
        public TapGestureRecognizer BackButtonTapRecognizer;
        public TapGestureRecognizer NextButtonTapRecognizer;
        public CustomImageButton NextButton;
        Label title;

        public PurposeColorBlueSubTitleBar(Color backGroundColor, string titleValue, bool nextButtonVisible = true, bool backButtonVisible = true)
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
            bgImage.Source = Device.OnPlatform("top_bg.png", "light_blue_bg.png", "//Assets//light_blue_bg.png");
            //  bgImage.WidthRequest = spec.ScreenWidth;
            // bgImage.HeightRequest = titlebarHeight;
            bgImage.Aspect = Aspect.Fill;

            Image backArrow = new Image();
            backArrow.Source = Device.OnPlatform("arrow_blue.png", "arrow_blue.png", "//Assets//arrow_blue.png");
            // backArrow.HeightRequest = spec.ScreenHeight * 4 / 100;
            // backArrow.WidthRequest = spec.ScreenWidth * 5 / 100;
            BackButtonTapRecognizer = new TapGestureRecognizer();
            backArrow.GestureRecognizers.Add(BackButtonTapRecognizer);

            title = new Label();
            title.Text = titleValue;
            title.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            title.FontSize = Device.OnPlatform(17, 20, 22);
            title.TextColor = Color.FromRgb( 30, 127, 210 );

            Image nextImage = new Image();
            nextImage.Source = Device.OnPlatform("tick_blue.png", "tick_blue.png", "//Assets//tick_blue.png");
            NextButtonTapRecognizer = new TapGestureRecognizer();
            nextImage.GestureRecognizers.Add(NextButtonTapRecognizer);

            masterLayout.AddChildToLayout(title, 20, 18, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);

            if (nextButtonVisible)
            {
                StackLayout touchArea = new StackLayout();
                touchArea.WidthRequest = 100;
                touchArea.HeightRequest = spec.ScreenHeight * 8 / 100;
                touchArea.BackgroundColor = Color.Transparent;
                touchArea.GestureRecognizers.Add(NextButtonTapRecognizer);
                masterLayout.AddChildToLayout(nextImage, Device.OnPlatform(83, 89, 85), Device.OnPlatform(10, 10, 10), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
                masterLayout.AddChildToLayout(touchArea, Device.OnPlatform(83, 80, 85), Device.OnPlatform(10, 2, 10), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            }

            if (backButtonVisible)
            {
                masterLayout.AddChildToLayout(backArrow, 5, 25, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
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
