using Facebook.Client;
using Microsoft.Phone.Controls;
using PurposeColor;
using PurposeColor.screens;
using PurposeColor.WinPhone.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Windows.System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(LoginWebViewHolder), typeof(LoginPageRenderer))]
namespace PurposeColor.WinPhone.Renderers
{
    public class LoginPageRenderer : PageRenderer
    {
        protected async override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            try
            {
                await GetFBLoginPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine("OnElementChanged : " + ex.Message);
            }
        }

        private async Task GetFBLoginPage()
        {
            try
            {
                FacebookSessionClient facebookSessionClient = new  FacebookSessionClient("1218463084847397");
                FacebookSession session;
                session = await facebookSessionClient.LoginAsync("user_about_me");
                PurposeColor.App.SaveToken(session.AccessToken);
                PurposeColor.App.SuccessfulLoginAction();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetFBLoginPage : " + ex.Message);
            }
        }
    }
}
