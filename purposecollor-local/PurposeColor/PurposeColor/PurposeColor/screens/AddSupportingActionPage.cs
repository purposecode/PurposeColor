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
    public class AddSupportingActionPage : ContentView
    {
        string pageTitle;
        CustomLayout masterLayout;
        Label listTitle;
        IDeviceSpec deviceSpec;

        public AddSupportingActionPage()
        {
            Content = new Label { Text = "Hello ContentView" };
        }
    }
}
