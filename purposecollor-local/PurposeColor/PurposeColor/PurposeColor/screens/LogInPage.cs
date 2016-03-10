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
			App.IsLoggedIn = false;
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
            subTitleBar = new PurposeColorBlueSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "               Sign In", true, true);
            subTitleBar.NextButtonTapRecognizer.Tapped += (s, e) =>
            {
                OnSignInButtonClicked(signInButton, null);
            };
            subTitleBar.BackButtonTapRecognizer.Tapped += (s, e) =>
            {
                App.masterPage.IsPresented = !App.masterPage.IsPresented;
            };

            userNameEntry = new CustomEntry
            {
                Placeholder = "Username",
                Keyboard = Keyboard.Email,
                HeightRequest = Device.OnPlatform(50, 50,75)
               // Text = "apptester" // for testing only // remove after testing
            };

            passwordEntry = new CustomEntry
            {
                Placeholder = "Password",
                IsPassword = true,
                HeightRequest = Device.OnPlatform(50, 50, 75)
            };

            signInButton = new Button
            {
                Text = "Sign in",
                TextColor = Color.White,
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BackgroundColor = Constants.BLUE_BG_COLOR
                //HeightRequest = 50
            };

            TapGestureRecognizer forgotPasswordTap = new TapGestureRecognizer();
            forgotPasswordLabel = new Label
            {
                Text = "Forgot password",
                TextColor = Constants.BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                FontSize = Device.OnPlatform(12,12,15),
                HeightRequest = Device.OnPlatform(15, 25, 25),
            };

            forgotPasswordLabel.GestureRecognizers.Add(forgotPasswordTap);
            forgotPasswordTap.Tapped += (s, e) =>
            {
                try
                {
                    Navigation.PushAsync(new ForgotPassword());
                }
                catch (Exception)
                {
                }
            };

            TapGestureRecognizer registerTap = new TapGestureRecognizer();
            registerLabel = new Label
            {
                Text = "Sign up with us",
                TextColor = Constants.BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                FontSize = Device.OnPlatform(12, 12, 15),
                HeightRequest = Device.OnPlatform(15, 25, 25),
            };
            registerLabel.GestureRecognizers.Add(registerTap);
            registerTap.Tapped += (s, e) =>
            {
                try
                {
                    Navigation.PushModalAsync(new RegistrationPageOne());
                }
                catch (Exception)
                {
                }
            };

            googleSignInButton = new Button
            {
                Text = "Sign in with Google",
                TextColor = Color.White,
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            faceBookSignInButton = new Button
            {
                Text = "Sign in with Facebook",
                TextColor = Color.White,
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            indicator = new ActivityIndicator();
            indicator.IsRunning = true;
            indicator.IsEnabled = true;
            indicator.IsVisible = false;

            userNameEntry.WidthRequest = screenWidth * 80 / 100;
            passwordEntry.WidthRequest = screenWidth * 80 / 100;
            signInButton.WidthRequest = screenWidth * 80 / 100;
            googleSignInButton.WidthRequest = screenWidth * 80 / 100;
            faceBookSignInButton.WidthRequest = screenWidth * 80 / 100;

            //imgsignInButton.WidthRequest = deviceSpec.ScreenWidth * 10 / 100;
            //imgsignInButton.HeightRequest = deviceSpec.ScreenHeight * 5 / 100;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(userNameEntry, 10, Device.OnPlatform(30, 30, 30));
            masterLayout.AddChildToLayout(passwordEntry, 10, Device.OnPlatform(40, 40, 38));
            masterLayout.AddChildToLayout(signInButton, 10, Device.OnPlatform(50, 50, 47));
            masterLayout.AddChildToLayout(forgotPasswordLabel, Device.OnPlatform(11, 11, 13), Device.OnPlatform(60,59,47));
            masterLayout.AddChildToLayout(registerLabel, Device.OnPlatform(62, 66, 65), Device.OnPlatform(60,59,47));
            
           // masterLayout.AddChildToLayout(googleSignInButton, 10, Device.OnPlatform(65,65,62));
           // masterLayout.AddChildToLayout(faceBookSignInButton, 10, Device.OnPlatform(75, 75, 70));

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
            try
            {
				App.IsLoggedIn = false;
                if (String.IsNullOrEmpty(userNameEntry.Text))
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Please provide username.", Constants.ALERT_OK);
                    return;
                }

                #region FOR TESTING

                if (userNameEntry.Text == "apptester")
                {
                    App.IsTesting = true;
					UpdateBurgerMenuList();
					App.IsLoggedIn = true;
					bool userSaved = await App.Settings.SaveUser(new User { UserId = "2", UserName = "App tester", AllowCommunitySharing = true, Email= "tester@test.com", StatusNote="Testing..", AllowFollowers = true }); // for testing only
                    if (!userSaved)
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Could not save user to local database.", Constants.ALERT_OK);
                    }
                    App.masterPage.IsPresented = false;
					UpdateBurgerMenuList();
                    App.masterPage.Detail = new NavigationPage(new FeelingNowPage());
                    return;
                }
                else
                {
                    App.IsTesting = false;
                }

                #endregion

                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(Constants.emailRegexString, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
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

                #region SERVIDE
                if (!String.IsNullOrEmpty(userNameEntry.Text) && !String.IsNullOrEmpty(passwordEntry.Text))
                {

                    IProgressBar progress = DependencyService.Get<IProgressBar>();
                    progress.ShowProgressbar("Signing in..");

                    try
                    {
                        bool isSaveSuccess = false;
                        var serviceResult = await PurposeColor.Service.ServiceHelper.Login(userNameEntry.Text, passwordEntry.Text);
                        if (serviceResult.code != null && serviceResult.code == "200")
                        {
							UpdateBurgerMenuList();
                            var loggedInUser = serviceResult.resultarray;
                            if (loggedInUser != null)
                            {
                                User newUser = null;
                                if (!string.IsNullOrEmpty(loggedInUser.email))
                                {
                                    newUser = await App.Settings.GetUserWithUserName(loggedInUser.email);
                                }

                                if (newUser == null)
                                {
                                    newUser = new User();
                                }

                                newUser.StatusNote = string.IsNullOrEmpty(loggedInUser.note) ? string.Empty : loggedInUser.note;
                                newUser.DisplayName = string.IsNullOrEmpty(loggedInUser.firstname) ? string.Empty : loggedInUser.firstname;
                                newUser.Email = string.IsNullOrEmpty(loggedInUser.email) ? string.Empty : loggedInUser.email;
                                newUser.ProfileImageUrl = string.IsNullOrEmpty(loggedInUser.profileurl) ? string.Empty : loggedInUser.profileurl;
                                newUser.UserId = loggedInUser.user_id;
								newUser.VerifiedStatus = loggedInUser.verified_status;
								newUser.AllowCommunitySharing = string.IsNullOrEmpty(loggedInUser.community_status) ? false: (loggedInUser.community_status == "1" ? true : false);
								newUser.AllowFollowers = string.IsNullOrEmpty(loggedInUser.follow_status) ? false: (loggedInUser.follow_status == "1" ? true : false); 
                                if (loggedInUser.usertype_id != null)
                                {
                                    newUser.UserType = int.Parse(loggedInUser.usertype_id);
                                }
                                if (loggedInUser.regdate != null)
                                {
                                    //newUser.RegistrationDate = DateTime.ParseExact(serviceResult.regdate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
                                    newUser.RegistrationDate = loggedInUser.regdate;
                                }

								App.Current.Properties["IsLoggedIn"] = true; // Persist Data - Save data in Xamarin.Forms

                                isSaveSuccess = await App.Settings.SaveUser(newUser);

                                PurposeColor.Model.GlobalSettings globalSettings = App.Settings.GetAppGlobalSettings();
								if(globalSettings != null)
								{
									globalSettings.ShowRegistrationScreen = false;
									globalSettings.IsLoggedIn = true;
									globalSettings.IsFirstLogin = true;
									await App.Settings.SaveAppGlobalSettings(globalSettings);
								}

                                progress.HideProgressbar();
                                App.masterPage.IsPresented = false;
                                App.masterPage.Detail = new NavigationPage(new FeelingNowPage());

                                //if (Device.OS != TargetPlatform.WinPhone)
                                //{
                                //    Navigation.RemovePage(this);
                                //}
                            }
                            else
                            {
                                progress.HideProgressbar();
                                await DisplayAlert(Constants.ALERT_TITLE, "Network error. Could not retrive user details.", Constants.ALERT_OK);
                                App.masterPage.IsPresented = false;
                                App.masterPage.Detail = new NavigationPage(new FeelingNowPage());
                                //if (Device.OS != TargetPlatform.WinPhone)
                                //{
                                //    Navigation.RemovePage(this);
                                //}
                            }
                        }
                        else
                        {
                            progress.HideProgressbar();
                            await DisplayAlert(Constants.ALERT_TITLE, "Could not login. Username password does not match, Please try again.", Constants.ALERT_OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        progress.HideProgressbar();
                        DisplayAlert(Constants.ALERT_TITLE, "Network error, Please try again.", Constants.ALERT_OK);
                        Debug.WriteLine("OnSignInButtonClicked: " + ex.Message);
                    }
                    progress.HideProgressbar();
                }

                #endregion

            }
            catch (Exception)
            {
            }

        }

        void OnGoogleSignInButtonClicked(object sender, EventArgs e)
        {
            try
            {
				App.IsLoggedIn = false;
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
                    Navigation.PushModalAsync(new ChangePassword());
                }
                // progress.ShowProgressbar(false, "Loading..");

            }
            catch (Exception)
            {
                
            }
        }

        void faceBookSignInButton_Clicked(object sender, EventArgs e)
        {
            try
            {
				App.IsLoggedIn = false;
                App.IsFacebookLogin = true;
                App.IsGoogleLogin = false;
                Navigation.PushModalAsync(new LoginWebViewHolder());
            }
            catch (Exception ex)
            {
            }
        }

		void UpdateBurgerMenuList()
		{
			try {

				if (App.burgerMenuItems == null) {
					App.burgerMenuItems = new System.Collections.ObjectModel.ObservableCollection<MenuItems> ();
				}

				if(App.burgerMenuItems.Count > 1)
				{
					return; // its alredy loged in menu, no need to change.
				}

				App.burgerMenuItems.Clear ();

				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.EMOTIONAL_AWARENESS,
					ImageName = Device.OnPlatform ("emotional_awrness_menu_icon.png", "emotional_awrness_menu_icon.png", "//Assets//emotional_awrness_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.GEM,
					ImageName = Device.OnPlatform ("gem_menu_icon.png", "gem_menu_icon.png", "//Assets//gem_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.GOALS_AND_DREAMS,
					ImageName = Device.OnPlatform ("goals_drms_menu_icon.png", "goals_drms_menu_icon.png", "//Assets//goals_drms_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.EMOTIONAL_INTELLIGENCE,
					ImageName = Device.OnPlatform ("emotion_intellegene_menu_icon.png", "emotion_intellegene_menu_icon.png", "//Assets//emotion_intellegene_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.COMMUNITY_GEMS,
					ImageName = Device.OnPlatform ("comunity_menu_icon.png", "comunity_menu_icon.png", "//Assets//comunity_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.APPLICATION_SETTTINGS,
					ImageName = Device.OnPlatform ("setings_menu_icon.png", "setings_menu_icon.png", "//Assets//setings_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.SIGN_OUT_TEXT,
					ImageName = Device.OnPlatform ("logout_icon.png", "logout_icon.png", "//Assets//logout_icon.png")
				});

			} catch (Exception ex) {
				var test = ex.Message;
			}
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
