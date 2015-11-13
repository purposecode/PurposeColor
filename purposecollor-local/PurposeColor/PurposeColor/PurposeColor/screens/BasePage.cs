using Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class BasePage : ContentPage
    {
        public IDeviceSpec deviceSpec;

        public BasePage()
        {
             deviceSpec = DependencyService.Get<IDeviceSpec>();
        }
    }
}
