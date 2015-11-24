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
           /* Device.BeginInvokeOnMainThread(() =>
            {
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                try
                {
                    AndHUD.Shared.Dismiss();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });*/

            AndHUD.Shared.Dismiss();

        }

        public void ShowToast(string messege)
        {
            AndHUD.Shared.ShowToast(Forms.Context, messege, MaskType.Black,  TimeSpan.FromSeconds( .5 ));
            
        }

    }
}
