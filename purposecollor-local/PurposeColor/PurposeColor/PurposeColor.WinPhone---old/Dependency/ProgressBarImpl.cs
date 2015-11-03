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
    class ProgressBarImpl : IProgressBar
    {

        public void ShowProgressbar(bool show, string text)
        {
            if (SystemTray.ProgressIndicator != null && SystemTray.ProgressIndicator.IsVisible)
            {
                SystemTray.ProgressIndicator.IsIndeterminate = false;
                SystemTray.ProgressIndicator.IsVisible = false;
                return;
            }
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = text;
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;
        }
       

 

       /* public void ShowToast(string messege)
        {
            ShellToast toast = new ShellToast();
            toast.Title = "";
            toast.Content = messege;
            toast.Show();
        }*/
    }
}
