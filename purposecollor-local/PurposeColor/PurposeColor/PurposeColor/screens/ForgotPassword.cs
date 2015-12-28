using System;
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
        PurposeColor.CustomControls.PurposeColorSubTitleBar subTitleBar = null;
        double screenHeight;
        double screenWidth;
        CustomEntry email = null;



        public ForgotPassword()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            PurposeColor.interfaces.IProgressBar progress = DependencyService.Get<PurposeColor.interfaces.IProgressBar>();

            masterLayout.BackgroundColor = Color.FromRgb(230, 255, 254);
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            Cross.IDeviceSpec deviceSpec = DependencyService.Get<Cross.IDeviceSpec>();
            PurposeColor.CustomControls.PurposeColorTitleBar mainTitleBar = new PurposeColor.CustomControls.PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += (s, e) =>
            {
                App.masterPage.IsPresented = !App.masterPage.IsPresented;
            };

            subTitleBar = new PurposeColor.CustomControls.PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Forgot password");
            subTitleBar.BackButtonTapRecognizer.Tapped += (s, e) =>
            {
                Navigation.PopAsync();
            };
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            subTitleBar.NextButtonTapRecognizer.Tapped += async (s, e) =>
            {
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
                        await DisplayAlert(Constants.ALERT_TITLE, "Please reset your password using the link send to "+email.Text+".", Constants.ALERT_OK);
                        await Navigation.PushAsync(new LogInPage());
                    }
                    else if(statusCode == "404")
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
                    progress.HideProgressbar();
                    var test = ex.Message;
                }

                progress.HideProgressbar();
            };

            description = new Label
            {
                Text = "Please provide your registered email adddress, we will be sending a password reset link to the same. Please add our address in your contacts to prevent routing our mails to spam folder.",
                TextColor = Constants.BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                FontSize = 16,
                WidthRequest = screenWidth * 80 / 100,
                HeightRequest = 70
            };

            masterLayout.AddChildToLayout(description, 10, 20);
            email = new CustomEntry
            {
                Placeholder = "Email",
                Keyboard = Keyboard.Email,
                BackgroundColor = Color.Black,
                TextColor = Color.White,
                Text = "Email...",
                HeightRequest = 50
            };
            email.WidthRequest = screenWidth * 80 / 100;
            masterLayout.AddChildToLayout(email, 10, 40);
            

            Content = masterLayout;
        }

    }
}
