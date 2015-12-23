using CustomControls;
using PurposeColor.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class ForgotPassword : ContentPage
    {
        CustomLayout masterLayout = null;
        double screenHeight;
        double screenWidth;
        IProgressBar progressBar;

        public ForgotPassword()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            progressBar = DependencyService.Get<IProgressBar>();
        }
    }
}
