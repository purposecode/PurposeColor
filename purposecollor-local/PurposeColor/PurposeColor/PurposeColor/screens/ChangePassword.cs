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
    public class ChangePassword : ContentPage
    {
        public Color TextColors { get; set; }

        public ChangePassword( User userInfo )
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Gray;

            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            PurposeColorTitleBar titleBar = new PurposeColorTitleBar(Color.FromRgb(8, 137, 216), "Purpose Color", Color.Black, "back");

            CustomEntry paswordEntry = new CustomEntry
            {
                Placeholder = "Password"
            };

            CustomEntry confirmPaswordEntry = new CustomEntry
            {
                Placeholder = "Confirm Password"
            };


            Button submitButton = new Button
            {
                Text = "Submit",
                TextColor = TextColors,
                BorderColor = Color.Black,
                BorderWidth = 2
            };


            paswordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            confirmPaswordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            submitButton.WidthRequest = deviceSpec.ScreenWidth * 40 / 100;

            masterLayout.AddChildToLayout(titleBar, 0, 0);
            masterLayout.AddChildToLayout(paswordEntry, 10, 30);
            masterLayout.AddChildToLayout(confirmPaswordEntry, 10, 45);
            masterLayout.AddChildToLayout(submitButton, 30, 60);

            submitButton.Clicked += OnSubmitButtonClicked;

            Content = masterLayout;
        }

        void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            
        }
    }
}
