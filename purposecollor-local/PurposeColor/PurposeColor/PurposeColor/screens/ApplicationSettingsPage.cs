using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class ApplicationSettingsPage : ContentPage, IDisposable
    {
        PurposeColorBlueSubTitleBar subTitleBar = null;
        Button signOutButton = null;
        Button changePassword = null;
        CustomLayout masterLayout = null;
        PurposeColorTitleBar mainTitleBar = null;
        double screenHeight;
        double screenWidth;

        public ApplicationSettingsPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
            subTitleBar = new PurposeColorBlueSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Application Settings",false, true);

            subTitleBar.BackButtonTapRecognizer.Tapped += (s, e) =>
            {
                App.masterPage.IsPresented = !App.masterPage.IsPresented;
            };

            signOutButton = new Button
            {
                Text = "Sign out",
                TextColor = Color.White,
                BorderColor = Constants.BLUE_BG_COLOR,
                BorderWidth = 2,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            changePassword = new Button
            {
                Text = "Change password",
                TextColor = Color.White,
                BorderColor = Constants.BLUE_BG_COLOR,
                BorderWidth = 2,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            signOutButton.WidthRequest = screenWidth * 80 / 100;
            changePassword.WidthRequest = screenWidth * 80 / 100;
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(signOutButton, 10, 20);
            masterLayout.AddChildToLayout(changePassword, 10, 30);
            signOutButton.Clicked += OnSignOutButtonClicked;
            changePassword.Clicked += ChangePassword_Clicked;

            Content = masterLayout;
        }

        async void ChangePassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                PurposeColor.Database.ApplicationSettings AppSettings = App.Settings;
                if (AppSettings != null)
                {
                    await Navigation.PushAsync(new ChangePassword(AppSettings.GetUser()));
                    return;
                }
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            DisplayAlert(Constants.ALERT_TITLE, "Page not available now, please try again later.", Constants.ALERT_OK);
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        async void OnSignOutButtonClicked(object sender, EventArgs e)
        {
            try
            {

                #region SAVING SIGN OUT SETTINGS

                PurposeColor.Database.ApplicationSettings AppSettings = App.Settings;
                if (AppSettings == null)
                {
                    return;
                }
                AppSettings.DeleteAllUsers();
                //PurposeColor.Model.GlobalSettings globalSettings = AppSettings.GetAppGlobalSettings();
                //globalSettings.ShowRegistrationScreen = false;
                //globalSettings.IsLoggedIn = false;
                //globalSettings.IsFirstLogin = true;
                await AppSettings.SaveAppGlobalSettings(new PurposeColor.Model.GlobalSettings());
                #endregion

                // to do service to sign out user so that further notifications are not send from server. //


                await Navigation.PushAsync(new LogInPage());
                Navigation.RemovePage(this);

            }
            catch (Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Network error, please try again later.", Constants.ALERT_OK);
            }
        }

        public void Dispose()
        {
            this.subTitleBar = null;
            this.signOutButton = null;
            this.changePassword = null;
            this.masterLayout = null;
            this.mainTitleBar = null;
            
            GC.Collect();
        }
    }
}
