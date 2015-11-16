﻿using CustomControls;
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
        public CustomImageButton NextButton;
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
            bgImage.Source = Device.OnPlatform("top_bg.png", "light_blue_bg.png", "//Assets//top_bg.png");
          //  bgImage.WidthRequest = spec.ScreenWidth;
           // bgImage.HeightRequest = titlebarHeight;
            bgImage.Aspect = Aspect.Fill;

            Image backArrow = new Image();
            backArrow.Source = Device.OnPlatform("back_arrow.png", "bckarow.png", "//Assets//back_arrow.png");
           // backArrow.HeightRequest = spec.ScreenHeight * 4 / 100;
           // backArrow.WidthRequest = spec.ScreenWidth * 5 / 100;
            BackButtonTapRecognizer = new TapGestureRecognizer();
            backArrow.GestureRecognizers.Add(BackButtonTapRecognizer);

            Image imgDivider = new Image();
            imgDivider.Source = Device.OnPlatform("top_seperate.png", "icn_seperate.png", "//Assets//top_seperate.png");
           // imgDivider.HeightRequest = spec.ScreenHeight * 4 / 100;


            NextButton = new CustomImageButton();
            NextButton.Text = "Next";
            NextButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            NextButton.FontSize = Device.OnPlatform( 12,12,18 );
            NextButton.TextColor = Color.White;
            NextButton.BackgroundColor = Color.Transparent;
            NextButton.BorderColor = Color.Transparent;
            NextButton.ImageName = "icon_tick.png";
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
           // logo.WidthRequest = spec.ScreenWidth * 70 / 100;
           // logo.HeightRequest = spec.ScreenHeight * 8 / 100;


            Image nextImage = new Image();
            nextImage.Source = Device.OnPlatform("logo.png", "icon_tick.png", "//Assets//logo.png");
           // nextImage.Aspect = Aspect.Fill;
           // nextImage.WidthRequest = spec.ScreenWidth * 8 / 100;
           // nextImage.HeightRequest = spec.ScreenHeight * 4 / 100;

           // masterLayout.AddChildToLayout(bgImage, 0, 0, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(title, 20, 22, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(imgDivider, 80, 26, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);


            if( nextButtonVisible )
            {
                masterLayout.AddChildToLayout(nextImage, Device.OnPlatform(83, 89, 76), Device.OnPlatform(10, 40, -5), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
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
