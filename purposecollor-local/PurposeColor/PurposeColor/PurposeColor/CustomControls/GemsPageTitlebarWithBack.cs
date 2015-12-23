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
    public class GemsPageTitleBarWithBack : ContentView
    {
        CustomLayout masterLayout;
        public TapGestureRecognizer imageAreaTapGestureRecognizer;
        public Label title;

        public GemsPageTitleBarWithBack(Color backGroundColor, string titleValue, Color titleColor, string backButtonTitle, bool imageRequired = false)
        {
            Cross.IDeviceSpec spec = DependencyService.Get<Cross.IDeviceSpec>();
            int titlebarHeight = (int)spec.ScreenHeight * 10 / 100;
            int titlebarWidth = (int)spec.ScreenWidth;
            this.BackgroundColor = backGroundColor;

            masterLayout = new CustomLayout();
            masterLayout.HeightRequest = titlebarHeight;
            masterLayout.WidthRequest = titlebarWidth;
            masterLayout.BackgroundColor = backGroundColor;



            Image backButton = new Image();
            backButton.Source = Device.OnPlatform("bckarow.png", "bckarow.png", "//Assets//bckarow.png");
            backButton.HeightRequest = 30;
            backButton.WidthRequest = 25;


            imageAreaTapGestureRecognizer = new TapGestureRecognizer();
            backButton.GestureRecognizers.Add(imageAreaTapGestureRecognizer);


            title = new Label();
            title.Text = titleValue;
            title.FontSize = 20;
            title.TextColor = Color.White;

            Image logo = new Image();
            logo.Source = Device.OnPlatform("logo_icon.png", "logo_icon.png", "//Assets//logo_icon.png");
            logo.WidthRequest = spec.ScreenWidth;
            logo.HeightRequest = titlebarHeight;
            logo.WidthRequest = spec.ScreenWidth * 10 / 100;
            logo.HeightRequest = spec.ScreenHeight * 8 / 100;




            masterLayout.AddChildToLayout(logo, 10, 5, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(title, 22, 30, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(backButton, 2, 25, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);

            if (imageRequired)
            {
                // masterLayout.AddChildToLayout(userImg, 80, Device.OnPlatform( 17, 15, 17 ), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            }

            Content = masterLayout;

        }
    }
}
