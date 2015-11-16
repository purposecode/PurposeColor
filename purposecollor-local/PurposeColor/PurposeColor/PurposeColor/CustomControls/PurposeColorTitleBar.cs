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
    public class PurposeColorTitleBar : ContentView
    {
        CustomLayout masterLayout;
        public TapGestureRecognizer imageAreaTapGestureRecognizer;
        Label title;

        public PurposeColorTitleBar(Color backGroundColor, string titleValue, Color titleColor, string backButtonTitle, bool imageRequired = false)
        {
            Cross.IDeviceSpec spec = DependencyService.Get<Cross.IDeviceSpec>();
            int titlebarHeight = (int)spec.ScreenHeight * 10 / 100;
            int titlebarWidth = (int)spec.ScreenWidth;
            this.BackgroundColor = backGroundColor;

            masterLayout = new CustomLayout();
            masterLayout.HeightRequest = titlebarHeight;
            masterLayout.WidthRequest = titlebarWidth;
            masterLayout.BackgroundColor = backGroundColor;



            Image menuButton = new Image();
            menuButton.Source = Device.OnPlatform("menu.png", "menu.png", "//Assets//menu.png");
            menuButton.HeightRequest = 30;
            menuButton.WidthRequest = 25;


            imageAreaTapGestureRecognizer = new TapGestureRecognizer();
            menuButton.GestureRecognizers.Add(imageAreaTapGestureRecognizer);


            title = new Label();
            title.Text = titleValue;
            title.FontSize = 20;
            title.TextColor = Color.Black;

            Image logo = new Image();
            logo.Source = Device.OnPlatform("logo.png", "logo.png", "//Assets//logo.png");
            logo.WidthRequest = spec.ScreenWidth;
            logo.HeightRequest = titlebarHeight;
            logo.WidthRequest = spec.ScreenWidth * 70 / 100;
            logo.HeightRequest = spec.ScreenHeight * 8 / 100;

          /*  CircleImage userImg = new CircleImage
            {
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                Source = Device.OnPlatform("avatar.jpg", "avatar.jpg", "//Assets//avatar.jpg")
            };

            userImg.WidthRequest = spec.ScreenWidth * 10 / 100;
			userImg.HeightRequest = spec.ScreenHeight * Device.OnPlatform( 6,8,8 ) / 100;*/


            masterLayout.AddChildToLayout(logo, 5, 5, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest); 
            masterLayout.AddChildToLayout(menuButton, 2, 25, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);

            if (imageRequired)
            {
               // masterLayout.AddChildToLayout(userImg, 80, Device.OnPlatform( 17, 15, 17 ), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            }

            Content = masterLayout;

        }
    }
}
