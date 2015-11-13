
using Cross;
using CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PurposeColor;
using PurposeColor.CustomControls;

namespace PurposeColor.screens
{

    public class MenuItems
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public MenuItems()
        {
        }
    }


    public class CustomMenuItemCell : ViewCell
    {
        public CustomMenuItemCell()
        {

            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            Label name = new Label();
            name.SetBinding(Label.TextProperty, "Name");
            name.TextColor = Color.White;
            name.FontSize = Device.OnPlatform(12, 13, 18);

            StackLayout divider = new StackLayout();
            divider.WidthRequest = deviceSpec.ScreenWidth;
            divider.HeightRequest = .75;
            divider.BackgroundColor = Color.FromRgb(255, 255, 255);

            Image sideImage = new Image();
            sideImage.WidthRequest = 20;
            sideImage.HeightRequest = 20;
            sideImage.SetBinding(Image.SourceProperty, "ImageName");
            sideImage.Aspect = Aspect.Fill;

            masterLayout.WidthRequest = deviceSpec.ScreenWidth;
            masterLayout.HeightRequest = deviceSpec.ScreenHeight * Device.OnPlatform(30, 30, 10) / 100;

            masterLayout.AddChildToLayout(sideImage, (float)5, (float)Device.OnPlatform(5, 0, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
			masterLayout.AddChildToLayout(name, (float)Device.OnPlatform( 15, 15 , 15 ), (float)Device.OnPlatform(5, 0, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(divider, (float)1, (float)20, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            this.View = masterLayout;
        }

    }

    public class MenuPage : ContentPage
    {
        ListView listView;
        public MenuPage()
        {
            this.BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            PurposeColorTitleBar titleBar = new PurposeColorTitleBar(Color.FromRgb(8, 137, 216), "Purpose Color", Color.Black, "back");

            List<MenuItems> menuItems = new List<MenuItems>();
            menuItems.Add(new MenuItems { Name = Constants.EMOTIONAL_AWARENESS, ImageName = Device.OnPlatform("emotion.png", "emotion.png", "//Assets//emotion.png") });
            menuItems.Add(new MenuItems { Name = Constants.GEM, ImageName = Device.OnPlatform("settings.png", "settings.png", "//Assets//settings.png") });
            menuItems.Add(new MenuItems { Name = Constants.GOALS_AND_DREAMS, ImageName = Device.OnPlatform("goals.png", "goals.png", "//Assets//goals.png") });
            menuItems.Add(new MenuItems { Name = Constants.EMOTIONAL_INTELLIGENCE, ImageName = Device.OnPlatform("emotion.png", "emotion.png", "//Assets//emotion.png") });
            menuItems.Add(new MenuItems { Name = Constants.COMMUNITY_GEMS, ImageName = Device.OnPlatform("Intel.png", "Intel.png", "//Assets//Intel.png") });
            menuItems.Add(new MenuItems { Name = Constants.APPLICATION_SETTTINGS, ImageName = Device.OnPlatform("goals.png", "goals.png", "//Assets//goals.png") });
            menuItems.Add(new MenuItems { Name = Constants.AUDIO_RECORDING, ImageName = Device.OnPlatform("settings.png", "settings.png", "//Assets//settings.png") });

            listView = new ListView();
            listView.ItemsSource = menuItems;
            listView.ItemTemplate = new DataTemplate(typeof(CustomMenuItemCell));
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += OnListViewItemSelected;
            listView.BackgroundColor = Constants.MENU_BG_COLOR;

            Icon = "icon.png";
            Title = "Menu";



            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;


            masterLayout.AddChildToLayout(titleBar, 0, 0);
            masterLayout.AddChildToLayout(listView, 0, Device.OnPlatform( 30,30, 10 ));
            Content = masterLayout;
        }


        public void Dispose()
        {
            listView.ItemSelected -= OnListViewItemSelected;
            listView = null;
            GC.Collect();
        }

        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            MenuItems selItem = e.SelectedItem as MenuItems;

            if ("EMOTIONAL AWARENESS" == selItem.Name)
            {

                App.masterPage.IsPresented = false;
                App.masterPage.Detail = new NavigationPage(new FeelingNowPage());
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
            else if (Constants.AUDIO_RECORDING == selItem.Name)
            {
                App.masterPage.IsPresented = false;
                App.masterPage.Detail = new NavigationPage(new AudioRecorderPage());
            }
        }
    }
}

