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
    public class LogInPage : ContentPage, IDisposable
    {
        ActivityIndicator indicator;
        CustomEntry userNameEntry;
        CustomEntry passwordEntry;
        Label forgotPasswordLabel = null;
        Label registerLabel = null;
        Button googleSignInButton = null;
        PurposeColorBlueSubTitleBar subTitleBar = null;
        Button faceBookSignInButton = null;
        Button signInButton = null;
        CustomLayout masterLayout = null;
        double screenHeight;
        double screenWidth;

        public LogInPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
            subTitleBar = new PurposeColorBlueSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "               Sign In", true, false);
            subTitleBar.NextButtonTapRecognizer.Tapped += (s, e) =>
            {
                OnSignInButtonClicked(signInButton, null);
            };

            userNameEntry = new CustomEntry
            {
                Placeholder = "Username",
                Keyboard = Keyboard.Email
            };

            passwordEntry = new CustomEntry
            {
                Placeholder = "Password",
                IsPassword = true
            };

            signInButton = new Button
            {
                Text = "Sign in",
                TextColor = Color.White,
                BorderColor = Constants.BLUE_BG_COLOR,
                BorderWidth = 2,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            TapGestureRecognizer forgotPasswordTap = new TapGestureRecognizer();
            forgotPasswordLabel = new Label
            {
                Text = "Forgot password",
                TextColor = Constants.BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                FontSize = 12,
                HeightRequest = Device.OnPlatform(15, 25, 25),
            };

            forgotPasswordLabel.GestureRecognizers.Add(forgotPasswordTap);
            forgotPasswordTap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new ForgotPassword());
            };

            TapGestureRecognizer registerTap = new TapGestureRecognizer();
            registerLabel = new Label
            {
                Text = "Sign up",
                TextColor = Constants.BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                FontSize = 12,
                HeightRequest = Device.OnPlatform(15, 25, 25),
            };
            registerLabel.GestureRecognizers.Add(registerTap);
            registerTap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new RegistrationPageOne());
            };

            googleSignInButton = new Button
            {
                Text = "Sign in with Google",
                TextColor = Color.White,
                BorderColor = Constants.BLUE_BG_COLOR,
                BorderWidth = 2,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            faceBookSignInButton = new Button
            {
                Text = "Sign in with Facebook",
                TextColor = Color.White,
                BorderColor = Constants.BLUE_BG_COLOR,
                BorderWidth = 2,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            indicator = new ActivityIndicator();
            indicator.IsRunning = true;
            indicator.IsEnabled = true;
            indicator.IsVisible = false;

            userNameEntry.WidthRequest = screenWidth * 80 / 100;
            passwordEntry.WidthRequest = screenWidth * 80 / 100;
            signInButton.WidthRequest = screenWidth * 60 / 100;
            googleSignInButton.WidthRequest = screenWidth * 60 / 100;
            faceBookSignInButton.WidthRequest = screenWidth * 60 / 100;

            //imgsignInButton.WidthRequest = deviceSpec.ScreenWidth * 10 / 100;
            //imgsignInButton.HeightRequest = deviceSpec.ScreenHeight * 5 / 100;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(userNameEntry, 10, 25);
            masterLayout.AddChildToLayout(passwordEntry, 10, 33);
            masterLayout.AddChildToLayout(signInButton, 20, 41);
            masterLayout.AddChildToLayout(forgotPasswordLabel, 21, 50);
            masterLayout.AddChildToLayout(registerLabel, 65, 50);
            
            
            masterLayout.AddChildToLayout(googleSignInButton, 20, 65);
            masterLayout.AddChildToLayout(faceBookSignInButton, 20, 75);

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

            #region FOR TESTING

            if (userNameEntry.Text == "apptester")
            {
                App.IsTesting = true;
                await Navigation.PushAsync(new FeelingNowPage());
                Navigation.RemovePage(this);
                return;
            }
            else
            {
                App.IsTesting = false;
            }
            
            #endregion
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(Constants.emailRegexString,System.Text.RegularExpressions.RegexOptions.IgnoreCase);
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

            

            #region SERVIDE
            if (!String.IsNullOrEmpty(userNameEntry.Text) && !String.IsNullOrEmpty(passwordEntry.Text))
            {

                IProgressBar progress = DependencyService.Get<IProgressBar>();
                 progress.ShowProgressbar("Signing in..");

                ApplicationSettings AppSettings = App.Settings;

                try
                {
                    bool isSaveSuccess = false;
                    var serviceResult = await PurposeColor.Service.ServiceHelper.Login(userNameEntry.Text, passwordEntry.Text);
                    if (serviceResult.code != null && serviceResult.code == "200")
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
                            
                            PurposeColor.Model.GlobalSettings globalSettings = AppSettings.GetAppGlobalSettings();
                            globalSettings.ShowRegistrationScreen = false;
                            globalSettings.IsLoggedIn = true;
                            globalSettings.IsFirstLogin = true;
                            await AppSettings.SaveAppGlobalSettings(globalSettings);

                            progress.HideProgressbar();
                            await Navigation.PushAsync(new FeelingNowPage());
                            Navigation.RemovePage(this);
                        }
                        else
                        {
                            progress.HideProgressbar();
                            await DisplayAlert(Constants.ALERT_TITLE, "Network error. Could not retrive user details.", Constants.ALERT_OK);
                            await Navigation.PushAsync(new FeelingNowPage());
                            Navigation.RemovePage(this);
                        }
                    }
                    else
                    {
                        progress.HideProgressbar();
                        await DisplayAlert(Constants.ALERT_TITLE, "Could not login. Username password does not match, Please try again", Constants.ALERT_OK);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    progress.HideProgressbar();
                    DisplayAlert(Constants.ALERT_TITLE, "Network error, Please try again", Constants.ALERT_OK);
                    Debug.WriteLine("OnSignInButtonClicked: " + ex.Message);
                }
                progress.HideProgressbar();
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

        public void Dispose()
        {
            this.forgotPasswordLabel = null;
            this.userNameEntry = null;
            this.passwordEntry = null;
            this.indicator = null;
            this.googleSignInButton = null;
            this.subTitleBar = null;
            this.faceBookSignInButton = null;
            this.signInButton = null;
            this.subTitleBar = null;
            this.masterLayout = null;

            GC.Collect();
        }
    }
}
