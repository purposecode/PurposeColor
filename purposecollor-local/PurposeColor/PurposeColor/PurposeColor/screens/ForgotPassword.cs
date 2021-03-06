﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using PurposeColor.CustomControls;

using Xamarin.Forms;
using CustomControls;

namespace PurposeColor.screens
{
    public class ForgotPassword : ContentPage
    {
        Label description = null;
        CustomLayout masterLayout = null;
        PurposeColor.CustomControls.PurposeColorBlueSubTitleBar subTitleBar = null;
        double screenHeight;
        double screenWidth;
        CustomEntry email = null;
        Button resetPasswordButton = null;

        public ForgotPassword()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            

            masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            Cross.IDeviceSpec deviceSpec = DependencyService.Get<Cross.IDeviceSpec>();
            PurposeColor.CustomControls.PurposeColorTitleBar mainTitleBar = new PurposeColor.CustomControls.PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += (s, e) =>
            {
                App.masterPage.IsPresented = !App.masterPage.IsPresented;
            };

            subTitleBar = new PurposeColor.CustomControls.PurposeColorBlueSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "       Forgot password");
            subTitleBar.BackButtonTapRecognizer.Tapped += (s, e) =>
            {
                try
                {
                    Navigation.PopAsync();
                }
                catch (Exception)
                {
                }
            };
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            subTitleBar.NextButtonTapRecognizer.Tapped += (s, e) =>
            {
                resetPasswordButton_Clicked(resetPasswordButton, null);
            };

            description = new Label
            {
                Text = "Please provide your registered email adddress, we will send a password reset link to the same. Please add our address in your contacts to prevent routing our mails to spam folder.",
                TextColor = Color.FromHex("#424646"),//Constants.BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                FontSize = 16,
                WidthRequest = screenWidth * 80 / 100,
                HeightRequest = 100
            };

            masterLayout.AddChildToLayout(description, 10, 20);
            email = new CustomEntry
            {
                Placeholder = "Email",
                Keyboard = Keyboard.Email,
                BackgroundColor = Color.Transparent,
                TextColor = Color.Black,
                HeightRequest = Device.OnPlatform(50, 50, 75)
                
            };
            email.WidthRequest = screenWidth * 80 / 100;
            masterLayout.AddChildToLayout(email, 10, 38);

            resetPasswordButton = new Button
            {
                Text = "Submit",
                TextColor = Color.White,
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            resetPasswordButton.Clicked += resetPasswordButton_Clicked;

            resetPasswordButton.WidthRequest = screenWidth * 80 / 100;
            masterLayout.AddChildToLayout(resetPasswordButton, 10, Device.OnPlatform(48, 48,46));

            Content = masterLayout;
        }

        async void resetPasswordButton_Clicked(object sender, EventArgs e)
        {
            PurposeColor.interfaces.IProgressBar progress = DependencyService.Get<PurposeColor.interfaces.IProgressBar>();
            try
            {
                if (String.IsNullOrEmpty(email.Text))
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Please provide your registered email address.", Constants.ALERT_OK);
                    return;
                }

                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(Constants.emailRegexString);
                System.Text.RegularExpressions.Match match = regex.Match(email.Text);
                if (!match.Success)
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Please provide your registered email address.", Constants.ALERT_OK);
                    return;
                }


                progress.ShowProgressbar("Requesting Password reset.");
                string statusCode = await PurposeColor.Service.ServiceHelper.ResetPassword(email.Text);
                progress.HideProgressbar();
                if (statusCode == "200")
                {
                    progress.HideProgressbar();
                    await DisplayAlert(Constants.ALERT_TITLE, "Please reset your password using the link send to " + email.Text + ".", Constants.ALERT_OK);
                    //await Navigation.PushModalAsync(new LogInPage());
                    App.masterPage.IsPresented = false;
                    App.masterPage.Detail = new NavigationPage(new LogInPage());
                }
                else if (statusCode == "404")
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Please verify the email, " + email.Text + " is not registered with us.", Constants.ALERT_OK);
                }
                else
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Network error, could not complete your request, Please try again.", Constants.ALERT_OK);
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            progress.HideProgressbar();
        }

    }
}
