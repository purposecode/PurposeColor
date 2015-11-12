

using Cross;
using CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.CustomControls
{

    public class CustomListViewItem
    {
        public string Name { get; set; }
        public CustomListViewItem()
        {
        }
    }


    public class CustomListViewCellItem : ViewCell
    {
        public CustomListViewCellItem()
        {
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            Label name = new Label();
            name.SetBinding(Label.TextProperty, "Name");
            name.TextColor = Color.White;
            name.FontSize = 18;

            StackLayout divider = new StackLayout();
            divider.WidthRequest = deviceSpec.ScreenWidth;
            divider.HeightRequest = 1;
            divider.BackgroundColor = Color.White;

            masterLayout.WidthRequest = deviceSpec.ScreenWidth;
            masterLayout.HeightRequest = deviceSpec.ScreenHeight * Device.OnPlatform(30, 30, 10) / 100;

            masterLayout.AddChildToLayout(name, (float)5, (float)Device.OnPlatform(5, 5, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(divider, (float)2, (float)Device.OnPlatform(20, 20, 5), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            this.View = masterLayout;

        }

    }

    public class CustomPicker : ContentView, IDisposable
    {
        public ListView listView;
        CustomLayout pageContainedLayout;
        public CustomPicker(CustomLayout containerLayout, List<CustomListViewItem> itemSource)
        {
            pageContainedLayout = containerLayout;
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Transparent;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            StackLayout layout = new StackLayout();
            layout.BackgroundColor = Color.Transparent;
            layout.Opacity = .1;
            layout.WidthRequest = deviceSpec.ScreenWidth;
            layout.HeightRequest = deviceSpec.ScreenHeight * 60 / 100;

            TapGestureRecognizer emptyAreaTapGestureRecognizer = new TapGestureRecognizer();
            emptyAreaTapGestureRecognizer.Tapped += (s, e) =>
            {
                View pickView = pageContainedLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                pageContainedLayout.Children.Remove(pickView);

            };
            layout.GestureRecognizers.Add(emptyAreaTapGestureRecognizer);


            StackLayout listContainer = new StackLayout();
            listContainer.WidthRequest = deviceSpec.ScreenWidth;
            listContainer.HeightRequest = deviceSpec.ScreenHeight * 45 / 100;

            listView = new ListView();
            listView.ItemsSource = itemSource;
            listView.ItemTemplate = new DataTemplate(typeof(CustomListViewCellItem));
            listView.HeightRequest = deviceSpec.ScreenHeight * 42 / 100;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.Opacity = 1;
            listView.BackgroundColor = Constants.MENU_BG_COLOR;
            //listContainer.Children.Add( listView );

            this.WidthRequest = deviceSpec.ScreenWidth;
            this.HeightRequest = deviceSpec.ScreenHeight;

            masterLayout.AddChildToLayout(layout, 0, 0);
            masterLayout.AddChildToLayout(listView, 0, 60);
            this.BackgroundColor = Color.Transparent;


            Content = masterLayout;
        }


        public void Dispose()
        {
            listView = null;
            pageContainedLayout = null;
            GC.Collect();
        }
    }
}

