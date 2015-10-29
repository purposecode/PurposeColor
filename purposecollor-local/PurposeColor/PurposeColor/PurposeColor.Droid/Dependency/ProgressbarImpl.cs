using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PurposeColor.interfaces;
using PurposeColor.WinPhone.Dependency;
using AndroidHUD;
using PurposeColor.Droid;


[assembly: Dependency(typeof(ProgressBarImpl))]
namespace PurposeColor.WinPhone.Dependency
{
    public class ProgressBarImpl : IProgressBar
    {

        public void ShowProgressbar(string text)
        {
            AndHUD.Shared.Show(MainActivity.GetMainActivity(), text, -1, MaskType.Clear);
        }


        public void HideProgressbar()
        {
            AndHUD.Shared.Dismiss();
        }

    }
}
