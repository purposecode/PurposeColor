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

            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Color.FromRgb(12, 113, 210), "Emotional Awareness");

            userNameEntry = new CustomEntry
            {
                Placeholder = "User name",
                Keyboard = Keyboard.Email
            };

            passwordEntry = new CustomEntry
            {
                Placeholder = "Password",
                IsPassword = true
            };

            Button signInButton = new Button
            {
                Text = "Sign in",
                TextColor = Color.Gray,
                BorderColor = Color.Black,
                BorderWidth = 2
            };

            ImageButton imgsignInButton = new ImageButton
            {
                Source = "//Assets//circle.png"
            };

            Button googleSignInButton = new Button
            {
                Text = "Sign in with Google",
                TextColor = Color.Gray,
                BorderColor = Color.Black,
                BorderWidth = 2
            };

            Button faceBookSignInButton = new Button
            {
                Text = "Sign in with Facebook",
                TextColor = Color.Gray,
                BorderColor = Color.Black,
                BorderWidth = 2
            };

            indicator = new ActivityIndicator();
            indicator.IsRunning = true;
            indicator.IsEnabled = true;
            indicator.IsVisible = false;

            userNameEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            passwordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            signInButton.WidthRequest = deviceSpec.ScreenWidth * 60 / 100;
            googleSignInButton.WidthRequest = deviceSpec.ScreenWidth * 60 / 100;
            faceBookSignInButton.WidthRequest = deviceSpec.ScreenWidth * 60 / 100;

            imgsignInButton.WidthRequest = deviceSpec.ScreenWidth * 10 / 100;
            imgsignInButton.HeightRequest = deviceSpec.ScreenHeight * 5 / 100;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(userNameEntry, 10, 25);
            masterLayout.AddChildToLayout(passwordEntry, 10, 35);
            masterLayout.AddChildToLayout(signInButton, 20, 50);
            masterLayout.AddChildToLayout(googleSignInButton, 20, 60);
            masterLayout.AddChildToLayout(faceBookSignInButton, 20, 70);

            googleSignInButton.Clicked += OnGoogleSignInButtonClicked;
            faceBookSignInButton.Clicked += faceBookSignInButton_Clicked;
            signInButton.Clicked += OnSignInButtonClicked;

            Content = masterLayout;
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        async void OnSignInButtonClicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(userNameEntry.Text))
            {
                await DisplayAlert(Constants.ALERT_TITLE, "Please provide username", Constants.ALERT_OK);
                return;
            }

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            System.Text.RegularExpressions.Match match = regex.Match(userNameEntry.Text);
            if (!match.Success)
            {
                await DisplayAlert(Constants.ALERT_TITLE, "Username will be same as your valid email address.", Constants.ALERT_OK);
                return;
            }

            if (String.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert(Constants.ALERT_TITLE, "Please provide password.", Constants.ALERT_OK);
                return;
            }
            else if (!String.IsNullOrEmpty(passwordEntry.Text) && passwordEntry.Text.Length < 6)
            {
                await DisplayAlert(Constants.ALERT_TITLE, "Password must be six characters long.", Constants.ALERT_OK);
                return;
            }

            #region FOR DB
            if (!String.IsNullOrEmpty(userNameEntry.Text) && !String.IsNullOrEmpty(passwordEntry.Text))
            {
                ApplicationSettings AppSettings = App.Settings;

                try
                {
                    bool isSaveSuccess = false;
                    var serviceResult = await PurposeColor.Service.ServiceHelper.Login(userNameEntry.Text, passwordEntry.Text);
                    if (serviceResult.code != null && serviceResult.code == "200" )
                    {
                        var loggedInUser = serviceResult.resultarray;
                        if (loggedInUser != null)
                        {
                            User newUser = null;
                            if (!string.IsNullOrEmpty(loggedInUser.email))
                            {
                                newUser = await AppSettings.GetUserWithUserName(loggedInUser.email);
                            }

                            if (newUser == null)
                            {
                                newUser = new User();
                            }

                            newUser.StatusNote = string.IsNullOrEmpty(loggedInUser.note) ? string.Empty : loggedInUser.note;
                            newUser.DisplayName = string.IsNullOrEmpty(loggedInUser.firstname) ? string.Empty : loggedInUser.firstname;
                            newUser.Email = string.IsNullOrEmpty(loggedInUser.email) ? string.Empty : loggedInUser.email;
                            newUser.ProfileImageUrl = string.IsNullOrEmpty(loggedInUser.profileurl) ? string.Empty : loggedInUser.profileurl;
                            if (loggedInUser.user_id != null)
                            {
                                newUser.UserId = Int32.Parse(loggedInUser.user_id);
                            }
                            if (loggedInUser.usertype_id != null)
                            {
                                newUser.UserType = Int32.Parse(loggedInUser.usertype_id);
                            }
                            if (loggedInUser.regdate != null)
                            {
                                //newUser.RegistrationDate = DateTime.ParseExact(serviceResult.regdate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                                newUser.RegistrationDate = loggedInUser.regdate;
                            }

                            isSaveSuccess = await AppSettings.SaveUser(newUser);

                            await Navigation.PushAsync(new FeelingNowPage());
                        }
                        else
                        {
                            await DisplayAlert(Constants.ALERT_TITLE, "Network error. Could not retrive user details.", Constants.ALERT_OK);
                            await Navigation.PushAsync(new FeelingNowPage());
                        }
                    }
                    else
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Could not login. Username password does not match, Please try again", Constants.ALERT_OK);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert(Constants.ALERT_TITLE, "Network error, Please try again", Constants.ALERT_OK);
                    Debug.WriteLine("OnSignInButtonClicked: " + ex.Message);
                }
            }

            #endregion


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
