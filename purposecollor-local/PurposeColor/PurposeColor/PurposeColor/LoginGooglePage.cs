using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor
{
    public class LoginGooglePage : ContentPage
    {
        public LoginGooglePage()
        {
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Gray;

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
                TextColor = Color.Black,
                BorderColor = Color.Black,
                BorderWidth = 2
            };


            paswordEntry.WidthRequest = App.screenWidth * 80 / 100;
            confirmPaswordEntry.WidthRequest = App.screenWidth * 80 / 100;
            submitButton.WidthRequest = App.screenWidth * 40 / 100;


            masterLayout.AddChildToLayout(paswordEntry, 10, 30);
            masterLayout.AddChildToLayout(confirmPaswordEntry, 10, 45);
            masterLayout.AddChildToLayout(submitButton, 30, 60);

            submitButton.Clicked += OnSubmitButtonClicked;

            Content = masterLayout;
        }

        void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync( new FeelingNowPage() );
        }
    }
}
