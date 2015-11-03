using Cross;
using CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class CameraPage : ContentView
    {
        public CameraPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Gray;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();


        }
    }
}
