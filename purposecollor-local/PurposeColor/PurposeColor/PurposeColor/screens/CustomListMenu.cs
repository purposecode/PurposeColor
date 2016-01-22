using CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class CustomListMenu : ContentView
    {
        CustomLayout masterLayout;
        CustomLayout pageContainedLayout;

        public ListView listView;
        public CustomListMenu(CustomLayout containerLayout, StackLayout parentStack, List<PurposeColor.CustomControls.CustomListViewItem> itemSource, int menuXpercent, int menuYpercent, int menuWidthPercent)
        {
            pageContainedLayout = containerLayout;
            this.BackgroundColor = Color.Transparent;
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Transparent;

            #region LIST VIEW
            listView = new ListView();
            listView.ItemsSource = itemSource;
            listView.ItemTemplate = new DataTemplate(typeof(PurposeColor.CustomControls.CustomListViewCellItem));
			listView.WidthRequest = Device.OnPlatform(App.screenWidth * .28, App.screenWidth * .30, App.screenWidth * .30);
			listView.HeightRequest = Device.OnPlatform(itemSource.Count * 44,itemSource.Count * 45, itemSource.Count * 45);
			listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.HorizontalOptions = LayoutOptions.Center;
            listView.VerticalOptions = LayoutOptions.Center;
            listView.BackgroundColor = Color.Transparent;
            listView.Opacity = 1;

            #endregion

            Image bg = new Image
            {
				WidthRequest = Device.OnPlatform((App.screenWidth * .30) + 2, (App.screenWidth * .31) + 2, (App.screenWidth * .31) + 2),
				HeightRequest = Device.OnPlatform(itemSource.Count * 48,itemSource.Count * 50, itemSource.Count * 50),
                Source = Device.OnPlatform("arrow_box.png", "arrow_box.png", "//Assets//arrow_box.png"),
                VerticalOptions = LayoutOptions.Start,
                Aspect = Aspect.Fill
            };

            masterLayout.AddChildToLayout(bg, 0, 0);
            masterLayout.AddChildToLayout(listView, 1, 2);
            masterLayout.HeightRequest = itemSource.Count * 65;
            masterLayout.WidthRequest = App.screenWidth * .34;

            Content = new StackLayout { Padding = 1, BackgroundColor = Color.Transparent, Children = { masterLayout }, HeightRequest = itemSource.Count * 70 };//App.screenHeight * .21
        }

        void HideCommentsPopup()
        {
            try
            {
                View menuView = pageContainedLayout.Children.FirstOrDefault(pick => pick.ClassId == Constants.CUSTOMLISTMENU_VIEW_CLASS_ID);
                pageContainedLayout.Children.Remove(menuView);
                menuView = null;
            }
            catch (Exception)
            {
            }
        }

    }
}
