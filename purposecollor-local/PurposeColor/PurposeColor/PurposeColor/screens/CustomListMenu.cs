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
            masterLayout = new CustomLayout();

            #region LIST VIEW
            listView = new ListView();
            listView.ItemsSource = itemSource;
            listView.ItemTemplate = new DataTemplate(typeof(PurposeColor.CustomControls.CustomListViewCellItem));
            listView.WidthRequest = App.screenWidth * .45;
            listView.HeightRequest = App.screenHeight * .20;
            listView.HorizontalOptions = LayoutOptions.Start;
            listView.BackgroundColor = Color.White;
            listView.Opacity = 1;
            #endregion

            Content = new StackLayout { BackgroundColor = Color.White, Children = { listView }, HeightRequest = App.screenHeight * .20 };
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
