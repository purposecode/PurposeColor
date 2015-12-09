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
            Device.BeginInvokeOnMainThread(() =>
            {
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                try
                {
                    SystemTray.ProgressIndicator = new ProgressIndicator();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

        }

        public void ShowProgressbar(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                try
                {
                    SystemTray.ProgressIndicator.Text = text;
                    SystemTray.ProgressIndicator.IsIndeterminate = true;
                    SystemTray.ProgressIndicator.IsVisible = true;
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
        }


        public void HideProgressbar()
        {
            if (SystemTray.ProgressIndicator != null && SystemTray.ProgressIndicator.IsVisible)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                    try
                    {
                        SystemTray.ProgressIndicator.IsIndeterminate = false;
                        SystemTray.ProgressIndicator.IsVisible = false;
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });
            }
        }
        public void ShowToast(string messege)
        {

        }

    }
}
