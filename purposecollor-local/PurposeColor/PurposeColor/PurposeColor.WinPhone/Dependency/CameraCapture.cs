using Microsoft.Phone.Controls;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(PurposeColor.WinPhone.Dependency.CameraCapture))]
namespace PurposeColor.WinPhone.Dependency
{
    public class CameraCapture : PurposeColor.interfaces.ICameraCapture
    {
        private readonly PhoneApplicationFrame frame;

        public CameraCapture()
        {
            frame = (PhoneApplicationFrame)(System.Windows.Application.Current.RootVisual);

        }

        public void RecodeVideo()
        {
            try
            {
                //MainPage.Navigate();
                //NavigationService.NavigateTo("VideoCamera");


                Device.BeginInvokeOnMainThread(() =>
                {
                    TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                    try
                    {
                        frame.Navigate(new Uri("/VideoCamera.xaml", UriKind.Relative));
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });


                

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }
    }
}
