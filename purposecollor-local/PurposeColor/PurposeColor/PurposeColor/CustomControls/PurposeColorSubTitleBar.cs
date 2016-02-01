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
    public class PurposeColorSubTitleBar : ContentView, IDisposable
    {
        CustomLayout masterLayout;
        public TapGestureRecognizer BackButtonTapRecognizer;
        public TapGestureRecognizer NextButtonTapRecognizer;
        public CustomImageButton NextButton;
        public Label title;
        double screenHeight;
        double screenWidth;

        public PurposeColorSubTitleBar(Color backGroundColor, string titleValue, bool nextButtonVisible = true, bool backButtonVisible = true )
        {
            int titlebarHeight = (int)App.screenHeight * 7 / 100;
            int titlebarWidth = (int)App.screenWidth;
            this.BackgroundColor = backGroundColor;
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;

            masterLayout = new CustomLayout();
            masterLayout.HeightRequest = titlebarHeight;
            masterLayout.WidthRequest = titlebarWidth;
            masterLayout.BackgroundColor = backGroundColor;

            Image bgImage = new Image();
            bgImage.Source = Device.OnPlatform("top_bg.png", "light_blue_bg.png", "//Assets//top_bg.png");
          //  bgImage.WidthRequest = spec.ScreenWidth;
           // bgImage.HeightRequest = titlebarHeight;
            bgImage.Aspect = Aspect.Fill;

            Image backArrow = new Image();
            backArrow.Source = Device.OnPlatform("bckarow.png", "bckarow.png", "//Assets//bckarow.png");
            BackButtonTapRecognizer = new TapGestureRecognizer();
            backArrow.GestureRecognizers.Add(BackButtonTapRecognizer);

            

            Image imgDivider = new Image();
			imgDivider.Source = Device.OnPlatform("icn_seperate.png", "icn_seperate.png", "//Assets//top_seperate.png");
           // imgDivider.HeightRequest = spec.ScreenHeight * 4 / 100;

            title = new Label();
            title.Text = titleValue;
            title.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            title.FontSize = Device.OnPlatform( 17, 20, 22 );
            title.TextColor = Color.Black;

            Image logo = new Image();
            logo.Source = Device.OnPlatform("logo.png", "logo.png", "//Assets//logo.png");
            logo.WidthRequest = App.screenWidth;
            logo.HeightRequest = titlebarHeight;
           // logo.WidthRequest = spec.ScreenWidth * 70 / 100;
           // logo.HeightRequest = spec.ScreenHeight * 8 / 100;

            Image nextImage = new Image();
            nextImage.Source = Device.OnPlatform("icon_tick.png", "icon_tick.png", "//Assets//icon_tick.png");
            NextButtonTapRecognizer = new TapGestureRecognizer();
            nextImage.GestureRecognizers.Add(NextButtonTapRecognizer);
           // nextImage.Aspect = Aspect.Fill;

            if( Device.OS == TargetPlatform.WinPhone )
            {
                backArrow.HeightRequest = screenHeight * 4 / 100;
                backArrow.WidthRequest = screenWidth * 8 / 100;

                nextImage.HeightRequest = screenHeight * 2 / 100;
                nextImage.WidthRequest = screenWidth * 10 / 100;

                imgDivider.HeightRequest = screenHeight * 4 / 100;
            }
            

           // masterLayout.AddChildToLayout(bgImage, 0, 0, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            //masterLayout.AddChildToLayout(title, 20, 18, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(title, Device.OnPlatform(20, 20, 28), Device.OnPlatform(18, 18, 32), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
           


            if( nextButtonVisible )
            {
                StackLayout touchArea = new StackLayout();
                touchArea.WidthRequest = 100;
                touchArea.HeightRequest = App.screenHeight * 8 / 100;
                touchArea.BackgroundColor = Color.Transparent;
                touchArea.GestureRecognizers.Add(NextButtonTapRecognizer);
                if (Device.OS != TargetPlatform.iOS)
                {
                    masterLayout.AddChildToLayout(imgDivider, 83, 26, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
                }
                masterLayout.AddChildToLayout(nextImage, Device.OnPlatform(87, 89, 85), Device.OnPlatform(10, 40, 38), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
                masterLayout.AddChildToLayout(touchArea, Device.OnPlatform(87, 80, 75), Device.OnPlatform(10, 2, 15), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            }

            if (backButtonVisible)
            {
                masterLayout.AddChildToLayout(backArrow, 5, Device.OnPlatform(25, 25,20), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
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
