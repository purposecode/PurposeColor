using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class ChangePassword : ContentPage, IDisposable
    {
        CustomEntry newPaswordEntry = null;
        CustomEntry oldPaswordEntry = null;
        CustomEntry confirmPaswordEntry = null;
        Button submitButton = null;

        public ChangePassword()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            PurposeColorBlueSubTitleBar subTitleBar = null;
            subTitleBar = new PurposeColorBlueSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "       Change Password", true, true);
            subTitleBar.BackButtonTapRecognizer.Tapped += (s, e) =>
            {
                try
                {
                    Navigation.PopModalAsync();
                }
                catch (Exception ex)
                {
                }
            };
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;

            oldPaswordEntry = new CustomEntry
            {
                Placeholder = "Old password",
                HeightRequest = Device.OnPlatform(50, 50, 75),
                IsPassword = true
            };

            newPaswordEntry = new CustomEntry
            {
                Placeholder = "New password",
                HeightRequest = Device.OnPlatform(50, 50, 75),
                IsPassword = true
            };

            confirmPaswordEntry = new CustomEntry
            {
                Placeholder = "Confirm new password",
                HeightRequest = Device.OnPlatform(50, 50, 75),
                IsPassword = true
            };

            submitButton = new Button
            {
                Text = "Submit",
                TextColor = Color.White,
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BackgroundColor = Constants.BLUE_BG_COLOR,
                HeightRequest = Device.OnPlatform(50, 50, 75),
            };

            oldPaswordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            newPaswordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            confirmPaswordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            submitButton.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, 10);
            masterLayout.AddChildToLayout(oldPaswordEntry, 10, 25);
            masterLayout.AddChildToLayout(newPaswordEntry, 10, 35);
            masterLayout.AddChildToLayout(confirmPaswordEntry, 10, 45);
            masterLayout.AddChildToLayout(submitButton, 10, 55);

            submitButton.Clicked += OnSubmitButtonClicked;

            subTitleBar.NextButtonTapRecognizer.Tapped += (s, e) =>
            {
                OnSubmitButtonClicked(submitButton, null);
            };

            Content = masterLayout;
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        async void OnSubmitButtonClicked(object sender, EventArgs e)  
        {
            PurposeColor.interfaces.IProgressBar progress = DependencyService.Get<PurposeColor.interfaces.IProgressBar>();
            bool isSuccess = false;
            try
            {
                submitButton.Clicked -= OnSubmitButtonClicked;
                User user = App.Settings.GetUser();
                if (user == null)
                {
                    user = new User { UserId = 2 }; // for testing only
                }

                if (user == null)
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Could not change password, please try after relogin.", Constants.ALERT_OK);
                    return;
                }

                if (String.IsNullOrEmpty(oldPaswordEntry.Text) || String.IsNullOrEmpty(newPaswordEntry.Text) || String.IsNullOrEmpty(confirmPaswordEntry.Text))
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Please fill all fields.", Constants.ALERT_OK);
                    return;
                }

                if (oldPaswordEntry.Text.Length < 6 || newPaswordEntry.Text.Length < 6 || confirmPaswordEntry.Text.Length < 6)
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "password must be of minimum 6 characters length.", Constants.ALERT_OK);
                    return;
                }

                if (newPaswordEntry.Text != confirmPaswordEntry.Text)
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "New password and Confirm password fields should match.", Constants.ALERT_OK);
                    return;
                }

                progress.ShowProgressbar("Requesting new password.");
                string statusCode = await PurposeColor.Service.ServiceHelper.UpdatePassword(user.UserId.ToString(), oldPaswordEntry.Text, newPaswordEntry.Text, confirmPaswordEntry.Text);
                progress.HideProgressbar();
                if (statusCode == "200")
                {
                    await App.Settings.SaveAppGlobalSettings(new GlobalSettings { IsLoggedIn = false, ShowRegistrationScreen = false, IsFirstLogin = false });
                    App.Settings.DeleteAllUsers();
                    progress.HideProgressbar();
                    await DisplayAlert(Constants.ALERT_TITLE, "Password updated successfully, please relogin.", Constants.ALERT_OK);
                    isSuccess = true;
                    //await Navigation.PushAsync(new LogInPage());
                    App.masterPage.IsPresented = false;
                    App.masterPage.Detail = new NavigationPage(new LogInPage());
                    //if (Device.OS != TargetPlatform.WinPhone)
                    //{
                    //    Navigation.RemovePage(this);
                    //}

                }
                else if (statusCode == "400")
                {
                    progress.HideProgressbar();
                    await DisplayAlert(Constants.ALERT_TITLE, "Password could not be updated, Please verify your old password and try again.", Constants.ALERT_OK);
                }
                else
                {
                    progress.HideProgressbar();
                    await DisplayAlert(Constants.ALERT_TITLE, "Network error, could not complete your request, Please try again later.", Constants.ALERT_OK);
                }
            }
            catch (Exception ex)
            {
                var test = ex.Message;
                progress.HideProgressbar();
                DisplayAlert(Constants.ALERT_TITLE, "Network error, could not complete your request, Please try again.", Constants.ALERT_OK);
            }
            progress.HideProgressbar();
            if (!isSuccess)
            {
                submitButton.Clicked += OnSubmitButtonClicked;
            }
        }

        public void Dispose()
        {
            this.newPaswordEntry = null;
            this.oldPaswordEntry = null;
            this.confirmPaswordEntry = null;
            this.submitButton.Clicked -= OnSubmitButtonClicked;
            this.submitButton = null;

            GC.Collect();
        }
    }
}
