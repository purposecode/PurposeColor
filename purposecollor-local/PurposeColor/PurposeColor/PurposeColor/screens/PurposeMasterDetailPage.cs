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
            App.Navigator = Navigation;
            NavigationPage.SetHasNavigationBar(this, false);
            Master = new MenuPage();
            Detail = new NavigationPage(new CommunityGemsPage());
        }
    }
}
