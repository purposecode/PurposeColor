

using Cross;
using CustomControls;
using PurposeColor.interfaces;
using PurposeColor.screens;
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
            masterLayout.BackgroundColor = Constants.LIST_BG_COLOR;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            Label name = new Label();
            name.SetBinding(Label.TextProperty, "Name");
            name.TextColor = Color.Black;
            name.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            name.FontSize = 18;

            StackLayout divider = new StackLayout();
            divider.WidthRequest = deviceSpec.ScreenWidth * 60 * 100;
            divider.HeightRequest = 1;
            divider.BackgroundColor = Color.FromRgb(220, 220, 220);

            masterLayout.WidthRequest = deviceSpec.ScreenWidth * 60 / 100;
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
        public CustomPicker(CustomLayout containerLayout, List<CustomListViewItem> itemSource, int topY, string title, bool titelBarRequired, bool addButtonRequired)
        {
            pageContainedLayout = containerLayout;
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Transparent;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            StackLayout layout = new StackLayout();
            layout.BackgroundColor = Color.Black;
            layout.Opacity = .4;
            layout.WidthRequest = deviceSpec.ScreenWidth;
            layout.HeightRequest = deviceSpec.ScreenHeight;

            TapGestureRecognizer emptyAreaTapGestureRecognizer = new TapGestureRecognizer();
            emptyAreaTapGestureRecognizer.Tapped += (s, e) =>
            {
                View pickView = pageContainedLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                pageContainedLayout.Children.Remove(pickView);

            };
            layout.GestureRecognizers.Add(emptyAreaTapGestureRecognizer);


            StackLayout listContainer = new StackLayout();
            listContainer.WidthRequest = deviceSpec.ScreenWidth * 96 / 100;
            listContainer.HeightRequest = deviceSpec.ScreenHeight * topY / 100;


            StackLayout listHeader = new StackLayout();
            listHeader.WidthRequest = deviceSpec.ScreenWidth * 96 / 100;
            listHeader.HeightRequest = deviceSpec.ScreenHeight * 10 / 100;
            listHeader.BackgroundColor = Color.FromRgb( 30, 126, 210 );

            Label listTitle = new Label();
            listTitle.Text = title;
            listTitle.TextColor = Color.White;
            listTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            listTitle.FontSize = 17;


            CustomImageButton addButton = new CustomImageButton();
            addButton.ImageName = "icn_plus.png";
            addButton.WidthRequest = 15;
            addButton.HeightRequest = 15;


            StackLayout addButtonLayout = new StackLayout();
            addButtonLayout.HeightRequest = 50;
            addButtonLayout.WidthRequest = 50;
            addButtonLayout.BackgroundColor = Color.Transparent;

            TapGestureRecognizer addButtonLayoutTapGestureRecognizer = new TapGestureRecognizer();
            addButtonLayoutTapGestureRecognizer.Tapped += OnAddButtonClicked;
            addButtonLayout.GestureRecognizers.Add(addButtonLayoutTapGestureRecognizer);

            listView = new ListView();
            listView.ItemsSource = itemSource;
            listView.ItemTemplate = new DataTemplate(typeof(CustomListViewCellItem));
            listView.HeightRequest = deviceSpec.ScreenHeight * 42 / 100;
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.Opacity = 1;
            listView.BackgroundColor = Constants.LIST_BG_COLOR;
            listView.WidthRequest = deviceSpec.ScreenWidth * 60;
            listContainer.Children.Add( listView );




            this.WidthRequest = deviceSpec.ScreenWidth;
            this.HeightRequest = deviceSpec.ScreenHeight;

            masterLayout.AddChildToLayout(layout, 0, 0);
            if (titelBarRequired)
            {
                masterLayout.AddChildToLayout(listHeader, 2, (100 - topY - 1) - 10);
                masterLayout.AddChildToLayout(listTitle, 5, (100 - topY - 1) - 7 );
                masterLayout.AddChildToLayout(addButton, 85, (100 - topY - 1) - 6);
                masterLayout.AddChildToLayout(addButtonLayout, 80, (100 - topY - 1) - 9); 
            }
            masterLayout.AddChildToLayout(listContainer, 2, 100 - topY - 1);
            this.BackgroundColor = Color.Transparent;


            Content = masterLayout;
        }

        void OnAddButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync( new AddEventsSituationsOrThoughts("Emotinal Awareness") );
        }


        public void Dispose()
        {
            listView = null;
            pageContainedLayout = null;
            GC.Collect();
        }
    }
}

