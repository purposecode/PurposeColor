using Cross;
using CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PurposeColor;

namespace PurposeColor.screens
{

    public class MenuItems
    {
        public string Name { get; set; }
        public MenuItems()
        {
        }
    }


    public class CustomMenuItemCell : ViewCell
    {
        public CustomMenuItemCell()
        {
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Gray;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            masterLayout.BackgroundColor = Color.White;
            Label name = new Label();
            name.SetBinding( Label.TextProperty, "Name" );
            name.TextColor = Color.Black;
			name.FontSize = Device.OnPlatform( 15, 18, 18 );

			StackLayout divider = new StackLayout();
			divider.WidthRequest = deviceSpec.ScreenWidth;
			divider.HeightRequest = 1;
			divider.BackgroundColor = Color.Navy;

            masterLayout.WidthRequest = deviceSpec.ScreenWidth;
            masterLayout.HeightRequest = deviceSpec.ScreenHeight * Device.OnPlatform( 30, 30, 10 ) / 100;

            masterLayout.AddChildToLayout(name, (float)5, (float)Device.OnPlatform( 5,5,50 ), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
			masterLayout.AddChildToLayout(divider, (float)1, (float)20, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            this.View = masterLayout;
        }

    }

    public class MenuPage : ContentPage
    {
        public MenuPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Gray;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            List<MenuItems> menuItems = new List<MenuItems>();
            menuItems.Add( new MenuItems{  Name = Constants.EMOTIONAL_AWARENESS});
            menuItems.Add(new MenuItems { Name = Constants.GEM });
            menuItems.Add(new MenuItems { Name = Constants.GOALS_AND_DREAMS });
            menuItems.Add(new MenuItems { Name = Constants.EMOTIONAL_INTELLIGENCE });
            menuItems.Add(new MenuItems { Name = Constants.COMMUNITY_GEMS });
            menuItems.Add(new MenuItems { Name = Constants.APPLICATION_SETTTINGS });

            ListView listView = new ListView();
            listView.ItemsSource = menuItems;
            listView.ItemTemplate = new DataTemplate(typeof(CustomMenuItemCell));
			listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += OnListViewItemSelected;
          
            Icon = "icon.png";
            Title = "Menu";

            masterLayout.BackgroundColor = Color.White;
            masterLayout.AddChildToLayout( listView,0, 15);
            Content = masterLayout;
        }


       void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            MenuItems selItem = e.SelectedItem as MenuItems;

            if ("EMOTIONAL AWARENESS" == selItem.Name)
            {

                App.masterPage.IsPresented = false;
                App.masterPage.Detail =new NavigationPage( new FeelingNowPage());
            }
            else if ("GEMs (Goal Enabling Materials)" == selItem.Name)
            {

            }
            else if ("GOALS & DREAMS" == selItem.Name)
            {

            }
            else if ("EMOTIONAL INTELLIGENCE" == selItem.Name)
            {

                App.masterPage.IsPresented = false;
                App.masterPage.Detail = new NavigationPage(new GraphPage());
            }
            else if ("COMMUNITY GEMs" == selItem.Name)
            {

            }
            else
            {

            }
        }
    }
}
