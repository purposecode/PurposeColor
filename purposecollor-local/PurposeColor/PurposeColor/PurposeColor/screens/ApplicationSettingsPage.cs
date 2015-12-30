using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.interfaces;
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
        IProgressBar progressBar;

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
            progressBar = DependencyService.Get<IProgressBar>();
            signOutButton = new Button
            {
                Text = "Sign out",
                TextColor = Color.White,
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            changePassword = new Button
            {
                Text = "Change password",
                TextColor = Color.White,
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            signOutButton.WidthRequest = screenWidth * 80 / 100;
            changePassword.WidthRequest = screenWidth * 80 / 100;
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(signOutButton, 10, 20);
            masterLayout.AddChildToLayout(changePassword, 10, Device.OnPlatform(30, 30,28));
            signOutButton.Clicked += OnSignOutButtonClicked;
            changePassword.Clicked += ChangePassword_Clicked;

            Content = masterLayout;
        }

        async void ChangePassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (App.Settings.GetUser() != null)
                {
                    await Navigation.PushAsync(new ChangePassword());
                }
                else
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Could not process your request now, please try again later.", Constants.ALERT_OK);
                }
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                DisplayAlert(Constants.ALERT_TITLE, "Could not process your request now, please try again later.", Constants.ALERT_OK);
            }
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        async void OnSignOutButtonClicked(object sender, EventArgs e)
        {
            try
            {
                progressBar.ShowToast("Signing out..");
                #region SAVING SIGN OUT SETTINGS
                string statusCode = await PurposeColor.Service.ServiceHelper.LogOut(App.Settings.GetUser().UserId.ToString());

                if (statusCode != "200")
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Network error, please try again later.", Constants.ALERT_OK);
                }
                #endregion
                App.Settings.DeleteAllUsers();
                await App.Settings.SaveAppGlobalSettings(new PurposeColor.Model.GlobalSettings());
                progressBar.HideProgressbar();
                await Navigation.PushAsync(new LogInPage());
                if (Device.OS != TargetPlatform.WinPhone)
                {
                    Navigation.RemovePage(this);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Network error, please try again later.", Constants.ALERT_OK);
            }
            progressBar.HideProgressbar();
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
