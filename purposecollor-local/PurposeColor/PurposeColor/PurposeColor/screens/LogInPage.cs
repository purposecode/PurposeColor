using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Database;
using PurposeColor.interfaces;
using PurposeColor.Model;
using SolTech.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class LogInPage : ContentPage
    {
        public Color TextColors { get; set; }
        ActivityIndicator indicator;
        CustomEntry userNameEntry;
        CustomEntry passwordEntry;

        public LogInPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            
            masterLayout.BackgroundColor = Color.FromRgb(230, 255, 254);

            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;

            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Color.FromRgb(12, 113, 210), "Emotional Awareness" );


            /* Label userNameLabel = new Label
             {
                 Text = "User name",
                 TextColor = TextColors
             };

             Label passwordLabel = new Label
             {
                 Text = "Password",
                 TextColor = TextColors
             };*/

            userNameEntry = new CustomEntry
            {
                Placeholder = "User name"
            };

            passwordEntry = new CustomEntry
            {
                Placeholder = "Password"
            };

            Button signInButton = new Button
            {
                Text = "Sign In",
                TextColor = TextColors,
                BorderColor = Color.Black,
                BorderWidth = 2
            };


            ImageButton imgsignInButton = new ImageButton
            {
                Source = "//Assets//circle.png"
            };

            Button googleSignInButton = new Button
            {
                Text = "Sign In With Google",
                TextColor = Color.Red,
                BorderColor = Color.Black,
                BorderWidth = 2
            };

            Button faceBookSignInButton = new Button
            {
                Text = "Sign In With Facebook",
                TextColor = Color.Red,
                BorderColor = Color.Black,
                BorderWidth = 2
            };

            indicator = new ActivityIndicator();
            indicator.IsRunning = true;
            indicator.IsEnabled = true;
            indicator.IsVisible = false;

            userNameEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            passwordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            signInButton.WidthRequest = deviceSpec.ScreenWidth * 40 / 100;
            googleSignInButton.WidthRequest = deviceSpec.ScreenWidth * 40 / 100;
            faceBookSignInButton.WidthRequest = deviceSpec.ScreenWidth * 40 / 100;

            imgsignInButton.WidthRequest = deviceSpec.ScreenWidth * 10 / 100;
            imgsignInButton.HeightRequest = deviceSpec.ScreenHeight * 5 / 100;

            
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform( 9, 10, 10 ));
            masterLayout.AddChildToLayout(userNameEntry, 10, 25);
            masterLayout.AddChildToLayout(passwordEntry, 10, 35);
            masterLayout.AddChildToLayout(signInButton, 30, 50);
            masterLayout.AddChildToLayout(googleSignInButton, 30, 60);
            masterLayout.AddChildToLayout(faceBookSignInButton, 30, 75);

            googleSignInButton.Clicked += OnGoogleSignInButtonClicked;
            faceBookSignInButton.Clicked += faceBookSignInButton_Clicked;
            signInButton.Clicked += OnSignInButtonClicked;

            Content = masterLayout;
        }


        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }


        void OnSignInButtonClicked(object sender, EventArgs e)
		{
			//Navigation.PushAsync( new GraphPage() );
			// Navigation.PushAsync(new CustomListView());
			//Navigation.PushAsync( new TestScreen() );


			#region FOR DB testing
			if (!String.IsNullOrEmpty (userNameEntry.Text)) 
			{
				ApplicationSettings AppSettings = App.Settings;
				User user = new User ();
				user.UserName = userNameEntry.Text;
				user.Password = passwordEntry.Text;
			         	
				try {
					User newUser = AppSettings.GetUserWithUserName (user.UserName);
					bool isSaveSuccess = false;
					if (newUser == null) {
						isSaveSuccess = AppSettings.SaveUser (user);
					}
			         	
					if (newUser != null) { // for testing only 
						if (newUser.Password == passwordEntry.Text) {
							DisplayAlert ("Login", "User login successfull", "OK");
						} else if (newUser.Password != passwordEntry.Text) {
							DisplayAlert ("Login", "Username password do not match, please verify", "OK");
						}
					} else if (isSaveSuccess) {
						DisplayAlert ("Registeration", "New user registered successfully", "OK");
					}
				} catch (Exception ex) {
					var msg = ex.Message;
					if (true) {
					
					}
				}
			}
			#endregion

			Navigation.PushAsync (new FeelingNowPage ());
		}

        void OnGoogleSignInButtonClicked(object sender, EventArgs e)
        {
            IProgressBar progress = DependencyService.Get<IProgressBar>();
            // progress.ShowProgressbar(true, "Loading..");
            App.IsGoogleLogin = true;

            if (Device.OS != TargetPlatform.WinPhone)
            {
                //Navigation.PushModalAsync(new LoginGooglePage());
                Navigation.PushModalAsync(new LoginWebViewHolder());
            }
            else
            {
                IAuthenticate winGoogle = DependencyService.Get<IAuthenticate>();
                winGoogle.AutheticateGoogle();
                Navigation.PushAsync(new ChangePassword(new User()));
            }
            // progress.ShowProgressbar(false, "Loading..");
        }

        void faceBookSignInButton_Clicked(object sender, EventArgs e)
        {
            App.IsFacebookLogin = true;
            App.IsGoogleLogin = false;
            Navigation.PushModalAsync(new LoginWebViewHolder());
        }
    }
}
