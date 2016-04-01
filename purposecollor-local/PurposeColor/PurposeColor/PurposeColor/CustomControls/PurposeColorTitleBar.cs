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

				TapGestureRecognizer profileImgTap = new TapGestureRecognizer ();
				profileImgTap.Tapped += ProfileImgTap_Tapped;
				userImg.GestureRecognizers.Add (profileImgTap);

			}
   

            masterLayout.AddChildToLayout(logo, 5, 5, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest); 
            masterLayout.AddChildToLayout(menuButton, 2, 25, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);



            Content = masterLayout;

        }

        void ProfileImgTap_Tapped (object sender, EventArgs e)
        {
			// nav to profile with user id.
			try {
				
				User user = App.Settings.GetUser();
				string userId = user.UserId;
				if (!string.IsNullOrEmpty (userId)) {
					int id = Convert.ToInt32 (userId);
					Navigation.PushAsync (new PurposeColor.screens.ProfileSettingsPage (id));
				}
			} catch (Exception ex) {
			}
        }
    }
}
