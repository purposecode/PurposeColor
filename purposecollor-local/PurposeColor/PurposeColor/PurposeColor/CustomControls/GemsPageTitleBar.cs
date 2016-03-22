using CustomControls;
using ImageCircle.Forms.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using PurposeColor.Model;

namespace PurposeColor.CustomControls
{
    public class GemsPageTitleBar : ContentView
    {
        CustomLayout masterLayout;
        public TapGestureRecognizer imageAreaTapGestureRecognizer;
        public Label title;

        public GemsPageTitleBar(Color backGroundColor, string titleValue, Color titleColor, string backButtonTitle, bool imageRequired = false)
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
            title.TextColor = Color.White;

            Image logo = new Image();
            logo.Source = Device.OnPlatform("logo_icon.png", "logo_icon.png", "//Assets//logo_icon.png");
            logo.WidthRequest = spec.ScreenWidth;
            logo.HeightRequest = titlebarHeight;
            logo.WidthRequest = spec.ScreenWidth * 10 / 100;
            logo.HeightRequest = spec.ScreenHeight * 8 / 100;

        
			User curUser = App.Settings.GetUser ();

			if (curUser != null) 
			{
				CircleImage userImg = new CircleImage
				{
					Aspect = Aspect.AspectFill,
					HorizontalOptions = LayoutOptions.Center,
					Source =  Constants.SERVICE_BASE_URL + curUser.ProfileImageUrl
				};

				userImg.WidthRequest = 30;
				userImg.HeightRequest = 30;
				if (imageRequired)
				{
					masterLayout.AddChildToLayout(userImg, 88, Device.OnPlatform( 17, 25, 17 ), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
				}
			}


            masterLayout.AddChildToLayout(logo, 10, 5, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(title, 22, 30, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(menuButton, 2, 25, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);


            Content = masterLayout;

        }
    }
}
