﻿using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class RegistrationPageOne : ContentPage, IDisposable
    {
        CustomLayout masterLayout = null;
        double screenHeight;
        double screenWidth;
        IProgressBar progressBar = null;
        Image pageBackGround = null;
        CustomEntry nameEntry = null;
        CustomEntry emailEntry = null;
        CustomEntry passwordEntry = null;
        CustomEntry confirmPasswordEntry = null;
        Label termsOfUseLabel = null;
        Image registrationButton = null;
        Switch termsSwitch = null;
        bool doesAgreeTerms = false;
        Image appIcon = null;

        public RegistrationPageOne()
        {
            masterLayout = new CustomLayout();

            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            progressBar = DependencyService.Get<IProgressBar>();

            #region BACKGROUND IMAGE

            pageBackGround = new Image();
            pageBackGround.WidthRequest = screenWidth;
            pageBackGround.HeightRequest = screenHeight;
            pageBackGround.Source = Device.OnPlatform("bg.png", "bg.png", "//Assets//bg.png");
            masterLayout.AddChildToLayout(pageBackGround, 0, 0);


            #endregion

            appIcon = new Image
            {
                BackgroundColor = Color.Transparent,
                Source = Device.OnPlatform("app_icon.png", "app_icon.png", "//Assets//app_icon.png")//app_icon //logo_icon
            };
			masterLayout.AddChildToLayout(appIcon, Device.OnPlatform(40, 40, 40), Device.OnPlatform(7, 10, 0));

            #region ENTRYS

            nameEntry = new CustomEntry
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                Placeholder = "First name",
                WidthRequest = screenWidth * .80, // 80% of screen,
                Keyboard = Keyboard.Default,
                HeightRequest = 50,

            };
            masterLayout.AddChildToLayout(nameEntry, 10, Device.OnPlatform(30, 30, 18));

            emailEntry = new CustomEntry
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                Placeholder = "Email",
                WidthRequest = screenWidth * .80, // 80% of screen,
                Keyboard = Keyboard.Email,
                HeightRequest = 50
            };
            masterLayout.AddChildToLayout(emailEntry, 10, Device.OnPlatform(40, 40, 28));

            passwordEntry = new CustomEntry
            {
                IsPassword = true,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                Placeholder = "Password (min 6 char)",
                //TextColor = Color.FromHex("#424646"),
                WidthRequest = screenWidth * .80, // 80% of screen,
                Keyboard = Keyboard.Default,
                HeightRequest = 50
            };
            masterLayout.AddChildToLayout(passwordEntry, 10, Device.OnPlatform(50, 50, 38));

            confirmPasswordEntry = new CustomEntry
            {
                IsPassword = true,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Transparent,
                Placeholder = "Confirm password",
                //TextColor = Color.FromHex("#424646"),
                WidthRequest = screenWidth * .80,// 80% of screen,
                Keyboard = Keyboard.Default,
                HeightRequest = 50
            };
            masterLayout.AddChildToLayout(confirmPasswordEntry, 10, Device.OnPlatform(60, 60, 48));

            #endregion

            #region TERMS OF USE
            var fs = new FormattedString();
            fs.Spans.Add(new Span { Text = "I have read and agggree to the ", ForegroundColor = Color.FromHex("#424646"), FontSize = Device.OnPlatform(12, 12, 14), FontAttributes = FontAttributes.None });
            fs.Spans.Add(new Span { Text = "terms of use", ForegroundColor = Color.Blue, FontSize = Device.OnPlatform(12, 12, 14), FontAttributes = FontAttributes.Italic });
            fs.Spans.Add(new Span { Text = " and ", ForegroundColor = Color.FromHex("#424646"), FontSize = Device.OnPlatform(12, 12, 14), FontAttributes = FontAttributes.None });
            fs.Spans.Add(new Span { Text = "Privacy policy", ForegroundColor = Color.Blue, FontSize = Device.OnPlatform(12, 12, 14), FontAttributes = FontAttributes.Italic });

            termsSwitch = new Switch();
            termsSwitch.BackgroundColor = Color.Transparent;
            termsSwitch.VerticalOptions = LayoutOptions.Center;
            termsSwitch.Toggled += (s, e) =>
            {
                doesAgreeTerms = !doesAgreeTerms;
            };

            masterLayout.AddChildToLayout(termsSwitch, 10, Device.OnPlatform(70, 70, 56));

            termsOfUseLabel = new Label
            {
                FormattedText = fs,
				WidthRequest = Device.OnPlatform(screenWidth * .60,screenWidth * .50,screenWidth * .50),
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center
            };
            masterLayout.AddChildToLayout(termsOfUseLabel, Device.OnPlatform(33, 40, 35), Device.OnPlatform(70, 70, 59));

            #endregion

            #region REGISTRATION

            registrationButton = new Image
            {
                BackgroundColor = Color.Transparent,
                WidthRequest = screenWidth * .80,
                Source = Device.OnPlatform("reg_btn.png", "reg_btn.png", "//Assets//reg_btn.png")
            };
			masterLayout.AddChildToLayout(registrationButton, Device.OnPlatform(10, 10, 13), Device.OnPlatform(80, 80, 65));
            TapGestureRecognizer registrationButtonTapRecognizer = new TapGestureRecognizer();
            registrationButton.GestureRecognizers.Add(registrationButtonTapRecognizer);

			if (Device.OS == TargetPlatform.WinPhone) 
			{
				appIcon.WidthRequest = (int)(screenWidth * .20);
				appIcon.HeightRequest = (int)(screenHeight * .20);

				nameEntry.HeightRequest = 73;
				emailEntry.HeightRequest = 73;
				passwordEntry.HeightRequest = 73;
				confirmPasswordEntry.HeightRequest = 73;

				registrationButton.HeightRequest = 80;
				registrationButton.WidthRequest = screenWidth * .75;
			}
			else if (Device.OS == TargetPlatform.iOS) 
			{
				appIcon.WidthRequest = (int)(screenWidth * .20);
				appIcon.HeightRequest = (int)(screenHeight * .20);

				nameEntry.BackgroundColor = Color.White;
				passwordEntry.BackgroundColor = Color.White;
				confirmPasswordEntry.BackgroundColor = Color.White;
				emailEntry.BackgroundColor = Color.White;
				registrationButton.WidthRequest = screenWidth * .80;
			}

            registrationButtonTapRecognizer.Tapped += async (s, e) =>
            {
                try
                {
                    #region FOR TESTING

                    if (nameEntry.Text != null && nameEntry.Text == "apptester")
                    {
                        PurposeColor.Database.ApplicationSettings AppSettings = App.Settings;
                        PurposeColor.Model.GlobalSettings globalSettings = AppSettings.GetAppGlobalSettings();
                        if (globalSettings == null)
                        {
                            globalSettings = new Model.GlobalSettings();
                        }
                        globalSettings.IsFirstLogin = true;
                        globalSettings.IsLoggedIn = false;
                        globalSettings.ShowRegistrationScreen = false;

                        await AppSettings.SaveAppGlobalSettings(globalSettings);

                        //await Navigation.PushModalAsync(new LogInPage());
                        App.masterPage.IsPresented = false;
                        App.masterPage.Detail = new NavigationPage(new LogInPage());
                        //if (Device.OS != TargetPlatform.WinPhone)
                        //{
                        //    Navigation.RemovePage(this);
                        //}
                        return;
                    }
                    
                    #endregion

                    bool allErntryAreValid = false;

                    if (nameEntry.Text != null && !string.IsNullOrWhiteSpace(nameEntry.Text) &&
                        emailEntry.Text != null && !string.IsNullOrWhiteSpace(emailEntry.Text) &&
                        passwordEntry.Text != null && !string.IsNullOrWhiteSpace(passwordEntry.Text) &&
                        confirmPasswordEntry.Text != null && !string.IsNullOrWhiteSpace(confirmPasswordEntry.Text)
                        )
                    {
                        allErntryAreValid = true;
                    }
                    else
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Please enter all fields", Constants.ALERT_OK);
                        allErntryAreValid = false;
                        return;
                    }

                    Regex regex = new Regex(Constants.emailRegexString);
                    Match match = regex.Match(emailEntry.Text);
                    if (match.Success)
                    {
                        allErntryAreValid = true;
                    }
                    else
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Please enter valid email address.", Constants.ALERT_OK);
                        allErntryAreValid = false;
                        return;
                    }

                    if (passwordEntry.Text.Length < 6) 
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "password must be of minimum 6 characters length.", Constants.ALERT_OK);
                        allErntryAreValid = false;
                        return;
                    }

                    if (passwordEntry.Text != confirmPasswordEntry.Text) 
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Password and Confirm password fields should match.", Constants.ALERT_OK);
                        allErntryAreValid = false;
                        return;
                    }

                    if (!termsSwitch.IsToggled)
                    {
                        allErntryAreValid = false;
                        await DisplayAlert(Constants.ALERT_TITLE, "To register please agree with the App Terms and Conditions.", Constants.ALERT_OK); ;
                        return;
                    }

                    bool doRegister = false;
                    if (allErntryAreValid)
                    {
                        doRegister = await DisplayAlert(Constants.ALERT_TITLE, "Confirm registration", "Register", "Cancel");
                    }

                    if (doRegister)
                    {
                        try
                        {
                            progressBar = DependencyService.Get<IProgressBar>();
                            progressBar.ShowProgressbar("Registering new user");

                            if (await RegisterUser())
                            {
                                progressBar.HideProgressbar();
                                await DisplayAlert(Constants.ALERT_TITLE, "User registration completed, Please verify the email. Please add our address in your contacts to prevent routing our mails to spam folder.", Constants.ALERT_OK);

                                #region GLOBAL SETTINGS DB

                                PurposeColor.Database.ApplicationSettings AppSettings = App.Settings;
                                PurposeColor.Model.GlobalSettings globalSettings = AppSettings.GetAppGlobalSettings();
                                if (globalSettings == null)
                                {
                                    globalSettings = new Model.GlobalSettings();
                                }
                                globalSettings.IsFirstLogin = true;
                                globalSettings.IsLoggedIn = false;
                                globalSettings.ShowRegistrationScreen = false;

                                await AppSettings.SaveAppGlobalSettings(globalSettings);
                                
                                #endregion

                                //await Navigation.PushModalAsync(new LogInPage());
                                App.masterPage.IsPresented = false;
                                App.masterPage.Detail = new NavigationPage(new LogInPage());
                            }
                            else
                            {
                                await DisplayAlert(Constants.ALERT_TITLE, "Network error, registration incomplete.", Constants.ALERT_OK);
                            }

                            progressBar.HideProgressbar();
                        }
                        catch (Exception ex)
                        {
                            progressBar.HideProgressbar();
                            DisplayAlert(Constants.ALERT_TITLE, "Network error, registration incomplete.", Constants.ALERT_OK);
                        }
                        progressBar.HideProgressbar();
                    }
                }
                catch (Exception ex)
                {
                    // DisplayAlert(Constants.ALERT_TITLE, ex.Message, Constants.ALERT_OK);
                }
                progressBar.HideProgressbar();
            };

            #endregion



            Content = masterLayout;
        }

        private async System.Threading.Tasks.Task<bool> RegisterUser()
        {
            try
            {
                string statusCode = await PurposeColor.Service.ServiceHelper.RegisterUser(nameEntry.Text, emailEntry.Text, passwordEntry.Text);
                if (statusCode == "201")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (progressBar != null)
                {
                    progressBar.HideProgressbar();
                }
            }

            return false;
        }

        public void Dispose()
        {
            this.masterLayout = null;
            this.progressBar = null;
            this.pageBackGround = null;
            this.nameEntry = null;
            this.emailEntry = null;
            this.passwordEntry = null;
            this.confirmPasswordEntry = null;
            this.termsOfUseLabel = null;
            this.registrationButton = null;

            GC.Collect();
        }
    }
}
