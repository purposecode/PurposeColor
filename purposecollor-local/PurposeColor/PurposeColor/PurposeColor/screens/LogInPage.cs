using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using PurposeColor.Model;
using System;
using System.Collections.Generic;
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
        public LogInPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Gray;

            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

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

            CustomEntry userNameEntry = new CustomEntry
            {
                Placeholder = "User name"
            };

            CustomEntry passwordEntry = new CustomEntry
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

            Button googleSignInButton = new Button
            {
                Text = "Sign In With Google",
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


            masterLayout.AddChildToLayout(userNameEntry, 10, 25);
            masterLayout.AddChildToLayout(passwordEntry, 10, 35);
            masterLayout.AddChildToLayout(signInButton, 30, 50);
            masterLayout.AddChildToLayout(googleSignInButton, 30, 60);



            googleSignInButton.Clicked += OnGoogleSignInButtonClicked;
            signInButton.Clicked += OnSignInButtonClicked;

            Content = masterLayout;
        }

        void OnSignInButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync( new GraphPage() );
          //Navigation.PushAsync( new TestScreen() );
        }

        void OnGoogleSignInButtonClicked(object sender, EventArgs e)
        {
            IProgressBar progress = DependencyService.Get<IProgressBar>();
            // progress.ShowProgressbar(true, "Loading..");

            if (Device.OS != TargetPlatform.WinPhone)
            {
                Navigation.PushModalAsync(new LoginGooglePage());
            }
            else
            {
                IAuthenticate winGoogle = DependencyService.Get<IAuthenticate>();
                winGoogle.AutheticateGoogle();
                Navigation.PushAsync(new ChangePassword(new User()));
            }

            // progress.ShowProgressbar(false, "Loading..");



        }
    }
}
