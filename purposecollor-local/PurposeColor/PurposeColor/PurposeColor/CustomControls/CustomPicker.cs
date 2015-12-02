

using Cross;
using CustomControls;
using PurposeColor.interfaces;
using PurposeColor.screens;
using PurposeColor.Service;
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
        string name;

        public string Name
        {
            get { return name; }
            set {
                string trimmedName = string.Empty;
                if (value.Length > 20)
                {
                    trimmedName = value.Substring(0, 28);
                    trimmedName += "...";
                }
                else
                {
                    trimmedName = value;
                }

                name = trimmedName;
            }
        }

        public string EmotionID { get; set; }
        public string EventID { get; set; }
        public int SliderValue { get; set; }
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
        string pageTitle;
        CustomLayout masterLayout;
        Label listTitle;
        IDeviceSpec deviceSpec;
        CustomImageButton addButton;
        CustomImageButton addEmotionButton;
        public FeelingNowPage FeelingsPage
        {
            get;
            set;
        }
        int topYPos;
        public CustomPicker(CustomLayout containerLayout, List<CustomListViewItem> itemSource, int topY, string title, bool titelBarRequired, bool addButtonRequired)
        {
            pageContainedLayout = containerLayout;
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Transparent;
            deviceSpec = DependencyService.Get<IDeviceSpec>();

            string trimmedPageTitle = string.Empty;
            if (title.Length > 25)
            {
                trimmedPageTitle = title.Substring(0, 25);
                trimmedPageTitle += "...";
            }
            else
            {
                trimmedPageTitle = title;
            }

            pageTitle = title;
            topYPos = topY;

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
                pickView = null;

            };
            layout.GestureRecognizers.Add(emptyAreaTapGestureRecognizer);


            StackLayout listContainer = new StackLayout();
            listContainer.WidthRequest = deviceSpec.ScreenWidth * 96 / 100;
            listContainer.HeightRequest = (deviceSpec.ScreenHeight * topY / 100) - 20;


            StackLayout listHeader = new StackLayout();
            listHeader.WidthRequest = deviceSpec.ScreenWidth * 96 / 100;
            listHeader.HeightRequest = deviceSpec.ScreenHeight * 10 / 100;
            listHeader.BackgroundColor = Color.FromRgb( 30, 126, 210 );

            listTitle = new Label();
            listTitle.Text = trimmedPageTitle;
            listTitle.TextColor = Color.White;
            listTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            listTitle.FontSize = 17;


            addButton = new CustomImageButton();
            if (Device.OS == TargetPlatform.WinPhone)
            {
                addButton.Image = (FileImageSource)ImageSource.FromFile(Device.OnPlatform("icn_plus.png", "icn_plus.png", "//Assets//icn_plus.png"));
            }
            else
            {
                addButton.ImageName = "icn_plus.png";
            }
            
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
            StackLayout listViewSpacer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(5,0,5,5),
                BackgroundColor = Constants.LIST_BG_COLOR,//Color.White,
                Children = { listView }
            };
            listViewSpacer.WidthRequest = deviceSpec.ScreenWidth * 95 / 100;
            listViewSpacer.HeightRequest = (deviceSpec.ScreenHeight * topY / 100)-20;


            listContainer.Children.Add( listViewSpacer );




            this.WidthRequest = deviceSpec.ScreenWidth;
            this.HeightRequest = deviceSpec.ScreenHeight;

            masterLayout.AddChildToLayout(layout, 0, 0);
            if (titelBarRequired)
            {
                masterLayout.AddChildToLayout(listHeader, 2, (100 - topY - 1) - 10);
                masterLayout.AddChildToLayout(listTitle, 7, (100 - topY - 1) - 7 );
                if (addButtonRequired)
                {

                    masterLayout.AddChildToLayout(addButton, 85, (100 - topY - 1) - 6);
                    masterLayout.AddChildToLayout(addButtonLayout, Device.OnPlatform(80, 80, 83), Device.OnPlatform((100 - topY - 1) - 9, (100 - topY - 1) - 9, (100 - topY - 1) - 8)); 
                }

            }
            masterLayout.AddChildToLayout(listContainer, 2, 100 - topY - 1);
            this.BackgroundColor = Color.Transparent;


            Content = masterLayout;
        }

        void OnAddButtonClicked(object sender, EventArgs e)
        {
            if (pageTitle == Constants.SELECT_EMOTIONS)
            {
                CustomEntry emotionsEntry = new CustomEntry();
                emotionsEntry.BackgroundColor = Color.White;
                emotionsEntry.Placeholder = "Enter emotion";
                emotionsEntry.WidthRequest =  deviceSpec.ScreenWidth * 75 / 100;
                emotionsEntry.TextColor = Color.Black;
                listTitle.IsVisible = false;
                addButton.IsVisible = false;

                addEmotionButton = new CustomImageButton();
                if (Device.OS == TargetPlatform.WinPhone)
                {
					addEmotionButton.Image = (FileImageSource)ImageSource.FromFile(Device.OnPlatform("tick_with_bg.png", "tick_with_bg.png", "//Assets//tick_with_bg.png"));
                }
                else
                {
					addEmotionButton.ImageName = "tick_with_bg.png";
                }

                addEmotionButton.WidthRequest = 25;
                addEmotionButton.HeightRequest = 25;

				StackLayout addEmotionButtonLayout = new StackLayout();
				addEmotionButtonLayout.HeightRequest = 50;
				addEmotionButtonLayout.WidthRequest = 50;
				addEmotionButtonLayout.BackgroundColor = Color.Transparent;

				TapGestureRecognizer addEmotionButtonLayoutTapGestureRecognizer = new TapGestureRecognizer();
				addEmotionButtonLayoutTapGestureRecognizer.Tapped += async (
					object addsender, EventArgs adde) => 
				{

                    IProgressBar progressBar = DependencyService.Get<IProgressBar>();

                    progressBar.ShowProgressbar("sending new emotion");


					listTitle.IsVisible = true;
					addButton.IsVisible = true;
					addEmotionButton.IsVisible  = false;
					emotionsEntry.IsVisible  = false;
					addEmotionButtonLayout.IsVisible  = false;

                    string emotionValue = emotionsEntry.Text.Trim();
                    if( emotionValue != null && emotionValue.Length == 0 )
                    {
                        progressBar.ShowToast("emotion is empty");
                        progressBar.HideProgressbar();
                        return;
                    }
                    var addService = await ServiceHelper.AddEmotion(FeelingNowPage.sliderValue.ToString(), emotionsEntry.Text, "2");

                    await FeelingsPage.DownloadAllEmotions();

                    View pickView = pageContainedLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                    pageContainedLayout.Children.Remove(pickView);
                    pickView = null;

                    progressBar.HideProgressbar();

				};
				addEmotionButtonLayout.GestureRecognizers.Add(addEmotionButtonLayoutTapGestureRecognizer);

				masterLayout.AddChildToLayout(addEmotionButton, 85, (100 - topYPos - 2) - 6);
                masterLayout.AddChildToLayout(emotionsEntry, 7, (100 - topYPos  - 2 ) - 7);
				masterLayout.AddChildToLayout(addEmotionButtonLayout, Device.OnPlatform(80, 80, 83), Device.OnPlatform((100 - topYPos - 1) - 9, (100 - topYPos - 1) - 9, (100 - topYPos - 1) - 8)); 
            }
            else
            {
                Navigation.PushAsync(new AddEventsSituationsOrThoughts(pageTitle));
                View pickView = pageContainedLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                pageContainedLayout.Children.Remove(pickView);
                pickView = null;
            }

        }


        public void Dispose()
        {
            listView = null;
            pageContainedLayout = null;
            pageTitle = null;
            masterLayout = null;
            listTitle = null;
            deviceSpec = null;
            addButton = null;
            addEmotionButton = null;
            GC.Collect();
        }
    }
}

