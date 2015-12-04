using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PurposeColor.interfaces;
using PurposeColor.WinPhone.Dependency;
using Microsoft.Phone.Shell;

[assembly: Dependency(typeof(ProgressBarImpl))]
namespace PurposeColor.WinPhone.Dependency
{
    public class ProgressBarImpl : IProgressBar
    {

        public ProgressBarImpl()
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
        }

        public void ShowProgressbar(string text)
        {
            SystemTray.ProgressIndicator.Text = text;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;
        }


        public void HideProgressbar()
        {
            if (SystemTray.ProgressIndicator != null && SystemTray.ProgressIndicator.IsVisible)
            {
                SystemTray.ProgressIndicator.IsIndeterminate = false;
                SystemTray.ProgressIndicator.IsVisible = false;
            }
        }
        public void ShowToast(string messege)
        {

        }

    }
}
