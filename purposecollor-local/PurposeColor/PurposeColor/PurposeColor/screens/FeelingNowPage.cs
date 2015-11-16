using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using System;
using System.Threading;
using System.Threading.Tasks;
using PurposeColor.screens;

namespace PurposeColor
{

    public class FeelingNowPage : ContentPage, IDisposable
    {
        CustomSlider slider;
        CustomPicker ePicker;
        CustomLayout masterLayout;
        IDeviceSpec deviceSpec;
        Button emotionalPickerButton;
        Button eventPickerButton;

        public FeelingNowPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.White;
            deviceSpec = DependencyService.Get<IDeviceSpec>();


            PurposeColorTitleBar titleBar = new PurposeColorTitleBar(Color.FromRgb(8, 137, 216), "Purpose Color", Color.Black, "back");
            titleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;

            slider = new CustomSlider
            {
                Minimum = 0,
                Maximum = 100,
                WidthRequest = deviceSpec.ScreenWidth * 90 / 100
            };


            Label howYouAreFeeling = new Label();
            howYouAreFeeling.Text = Constants.HOW_YOU_ARE_FEELING;
            howYouAreFeeling.TextColor = Color.Black;
            howYouAreFeeling.FontSize = Device.OnPlatform( 20, 22, 30 );
            howYouAreFeeling.WidthRequest = deviceSpec.ScreenWidth * 70 / 100;
            howYouAreFeeling.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;

            emotionalPickerButton = new Button();
            emotionalPickerButton.Text = "Please select an emotion";
            emotionalPickerButton.FontSize = 16;
            emotionalPickerButton.TextColor = Color.Black;
            emotionalPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            emotionalPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            emotionalPickerButton.Clicked += OnEmotionalPickerButtonClicked;

            eventPickerButton = new Button();
            eventPickerButton.Text = "Events, Situation & Thoughts";
            eventPickerButton.FontSize = 16;
            eventPickerButton.TextColor = Color.Black;
            eventPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            eventPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            eventPickerButton.Clicked += OnEventPickerButtonClicked;



            Label about = new Label();
            about.Text = "about";
            about.TextColor = Color.Black;
            about.FontSize = Device.OnPlatform(20, 22, 30);
            about.WidthRequest = deviceSpec.ScreenWidth * 50 / 100;
            about.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;
            about.HorizontalOptions = LayoutOptions.Center;



            this.Appearing += FeelingNowPage_Appearing;

            masterLayout.AddChildToLayout(titleBar, 0, 0);
            masterLayout.AddChildToLayout(howYouAreFeeling, 15, 15);
            masterLayout.AddChildToLayout(slider, 5, 35);
            masterLayout.AddChildToLayout(emotionalPickerButton, 5, 50);
            masterLayout.AddChildToLayout(about, 40, 60);
            masterLayout.AddChildToLayout(eventPickerButton, 5, 70);

            Content = masterLayout;

        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        void backButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync( new GraphPage() );
        }


        void OnEmotionalPickerButtonClicked(object sender, System.EventArgs e)
        {

            CustomPicker ePicker = new CustomPicker( masterLayout, GetEmotionsList(), 30 );
            ePicker.WidthRequest = deviceSpec.ScreenWidth;
            ePicker.HeightRequest = deviceSpec.ScreenHeight;
            ePicker.ClassId = "ePicker";
            ePicker.listView.ItemSelected += OnEmotionalPickerItemSelected;
            masterLayout.AddChildToLayout(ePicker, 0, 0);

            //double yPos = 60 * deviceSpec.ScreenHeight / 100;
           // ePicker.TranslateTo(0, -yPos, 250, Easing.BounceIn);
           // ePicker.FadeTo(1, 750, Easing.Linear); 
        }

        void OnEventPickerButtonClicked(object sender, System.EventArgs e)
        {

            CustomPicker ePicker = new CustomPicker( masterLayout, GetEventsList(), 50 );
            ePicker.WidthRequest = deviceSpec.ScreenWidth;
            ePicker.HeightRequest = deviceSpec.ScreenHeight;
            ePicker.ClassId = "ePicker";
            ePicker.listView.ItemSelected += OnEventPickerItemSelected;
            masterLayout.AddChildToLayout(ePicker, 0, 0);
            //double yPos = 60 * deviceSpec.ScreenHeight / 100;
            //ePicker.TranslateTo(0, yPos, 250, Easing.BounceIn);
           // ePicker.FadeTo(1, 750, Easing.Linear); 
        }


        private List<CustomListViewItem> GetEmotionsList()
        {
            List<CustomListViewItem> listSource = new List<CustomListViewItem>();
            listSource.Add(new CustomListViewItem { Name = "Happy" });
            listSource.Add(new CustomListViewItem { Name = "Sad" });
            listSource.Add(new CustomListViewItem { Name = "Frustated" });
            listSource.Add(new CustomListViewItem { Name = "extreme happy" });
            listSource.Add(new CustomListViewItem { Name = "polite" });
            listSource.Add(new CustomListViewItem { Name = "grtitude" });
            listSource.Add(new CustomListViewItem { Name = "Relaxed" });
            listSource.Add(new CustomListViewItem { Name = "hungry" });
            listSource.Add(new CustomListViewItem { Name = "Happy" });
            listSource.Add(new CustomListViewItem { Name = "Sad" });
            listSource.Add(new CustomListViewItem { Name = "Frustated" });
            listSource.Add(new CustomListViewItem { Name = "extreme happy" });
            listSource.Add(new CustomListViewItem { Name = "polite" });
            listSource.Add(new CustomListViewItem { Name = "grtitude" });
            listSource.Add(new CustomListViewItem { Name = "Relaxed" });
            listSource.Add(new CustomListViewItem { Name = "hungry" });
            listSource.Add(new CustomListViewItem { Name = "Happy" });
            listSource.Add(new CustomListViewItem { Name = "Sad" });
            listSource.Add(new CustomListViewItem { Name = "Frustated" });
            listSource.Add(new CustomListViewItem { Name = "extreme happy" });
            listSource.Add(new CustomListViewItem { Name = "polite" });
            listSource.Add(new CustomListViewItem { Name = "grtitude" });
            listSource.Add(new CustomListViewItem { Name = "Relaxed" });
            listSource.Add(new CustomListViewItem { Name = "hungry" });
            return listSource;
        }


        private List<CustomListViewItem> GetEventsList()
        {
            List<CustomListViewItem> listSource = new List<CustomListViewItem>();
            listSource.Add(new CustomListViewItem { Name = "Lost Job" });
            listSource.Add(new CustomListViewItem { Name = "married" });
            listSource.Add(new CustomListViewItem { Name = "divorsed" });
            listSource.Add(new CustomListViewItem { Name = "got promotion" });
            listSource.Add(new CustomListViewItem { Name = "got a trip" });
            listSource.Add(new CustomListViewItem { Name = "bought a car" });
            return listSource;
        }

        void OnEmotionalPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListViewItem item = e.SelectedItem as CustomListViewItem;
            emotionalPickerButton.Text = item.Name;
           View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
           masterLayout.Children.Remove( pickView );
        }

        void OnEventPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListViewItem item = e.SelectedItem as CustomListViewItem;
            eventPickerButton.Text = item.Name;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
        }

        void emotionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        async  void FeelingNowPage_Appearing(object sender, System.EventArgs e)
        {
            int val = 2;
            for( int index = 0; index < 200; index++ )

            {
                await Task.Delay(2);

                if( slider.Value > 90 )
                {
                    val = -2;
                }
                slider.Value += val;
            }
        }

        public void Dispose()
        {
            slider = null;
            ePicker = null;
            masterLayout = null;
            deviceSpec = null;
            emotionalPickerButton = null;
            eventPickerButton = null;
        }
    }
}
