using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class PurposeMasterDetailPage : MasterDetailPage
    {
        public PurposeMasterDetailPage()
        {
            try
            {

                App.Navigator = Navigation;
                NavigationPage.SetHasNavigationBar(this, false);
                Master = new MenuPage();

                PurposeColor.Database.ApplicationSettings AppSettings = App.Settings;
                PurposeColor.Model.GlobalSettings globalSettings = null;

                if (AppSettings != null)
                {
                    globalSettings = AppSettings.GetAppGlobalSettings();
                }
                if (globalSettings == null)
                {
                    Detail = new NavigationPage(new RegistrationPageOne());
                }
                else if (globalSettings.IsLoggedIn)
                {
                    Detail = new NavigationPage(new FeelingNowPage());
                }
                else if (globalSettings.ShowRegistrationScreen)
                {
                    Detail = new NavigationPage(new RegistrationPageOne());
                }
                else
                {
                    Detail = new NavigationPage(new LogInPage());
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;

                Detail = new NavigationPage(new LogInPage());
            }
            
        }
    }
}
