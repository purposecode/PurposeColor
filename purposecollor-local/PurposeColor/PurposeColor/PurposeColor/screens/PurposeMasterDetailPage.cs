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

				if (AppSettings != null )
				{
					globalSettings = AppSettings.GetAppGlobalSettings();
				}
				bool isUserLoggedIn = false;
				if(App.Current.Properties.ContainsKey("IsLoggedIn"))
				{
					isUserLoggedIn = (bool)App.Current.Properties["IsLoggedIn"];
				}

				if(isUserLoggedIn)
				{
					App.IsLoggedIn = true;
					UpdateBurgerMenuList();
					Detail = new NavigationPage(new FeelingNowPage());
				}
				else if ( AppSettings.GetUser() != null && AppSettings.GetUser().UserId != null)
				{
					App.IsLoggedIn = true;
					UpdateBurgerMenuList();
					Detail = new NavigationPage(new FeelingNowPage());
				}
				else if ( (globalSettings == null || globalSettings.IsLoggedIn)  && AppSettings.GetUser() == null)
				{
					App.IsLoggedIn = false;
					UpdateBurgerMenuList();
					Detail = new NavigationPage(new LogInPage());
				}
				else if (globalSettings.ShowRegistrationScreen)
				{
					App.IsLoggedIn = false;
					UpdateBurgerMenuList();
					Detail = new NavigationPage(new RegistrationPageOne());
				}
				else
				{
					App.IsLoggedIn = false;
					UpdateBurgerMenuList();
					Detail = new NavigationPage(new LogInPage());
				}
            }
            catch (Exception ex)
            {
                var test = ex.Message;

                Detail = new NavigationPage(new LogInPage());
            }
        }

		void UpdateBurgerMenuList()
		{
			try {

				if (App.burgerMenuItems == null) {
					App.burgerMenuItems = new System.Collections.ObjectModel.ObservableCollection<MenuItems> ();
				}


				if(App.IsLoggedIn)
				{
					if (App.burgerMenuItems.Count > 1) {
						return;
					}

					App.burgerMenuItems.Clear ();

					App.burgerMenuItems.Add (new MenuItems {
						Name = Constants.EMOTIONAL_AWARENESS,
						ImageName = Device.OnPlatform ("emotional_awrness_menu_icon.png", "emotional_awrness_menu_icon.png", "//Assets//emotional_awrness_menu_icon.png")
					});
					App.burgerMenuItems.Add (new MenuItems {
						Name = Constants.GEM,
						ImageName = Device.OnPlatform ("gem_menu_icon.png", "gem_menu_icon.png", "//Assets//gem_menu_icon.png")
					});
					App.burgerMenuItems.Add (new MenuItems {
						Name = Constants.GOALS_AND_DREAMS,
						ImageName = Device.OnPlatform ("goals_drms_menu_icon.png", "goals_drms_menu_icon.png", "//Assets//goals_drms_menu_icon.png")
					});
					App.burgerMenuItems.Add (new MenuItems {
						Name = Constants.EMOTIONAL_INTELLIGENCE,
						ImageName = Device.OnPlatform ("emotion_intellegene_menu_icon.png", "emotion_intellegene_menu_icon.png", "//Assets//emotion_intellegene_menu_icon.png")
					});
					App.burgerMenuItems.Add (new MenuItems {
						Name = Constants.COMMUNITY_GEMS,
						ImageName = Device.OnPlatform ("comunity_menu_icon.png", "comunity_menu_icon.png", "//Assets//comunity_menu_icon.png")
					});
					App.burgerMenuItems.Add (new MenuItems {
						Name = Constants.APPLICATION_SETTTINGS,
						ImageName = Device.OnPlatform ("setings_menu_icon.png", "setings_menu_icon.png", "//Assets//setings_menu_icon.png")
					});
					App.burgerMenuItems.Add (new MenuItems {
						Name = Constants.SIGN_OUT_TEXT,
						ImageName = Device.OnPlatform ("logout_icon.png", "logout_icon.png", "//Assets//logout_icon.png")
					});
				}
				else {
					if (App.burgerMenuItems.Count == 1) {
						return;
					}

					App.burgerMenuItems.Clear();
					App.burgerMenuItems.Add (new MenuItems {
						Name = Constants.SIGN_OUT_IN,
						ImageName = Device.OnPlatform ("logout_icon.png", "logout_icon.png", "//Assets//logout_icon.png")
					});
				}

			} catch (Exception ex) {
				var test = ex.Message;
			}
		}


    }
}
