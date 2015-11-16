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

    public class FeelingsSecondPage : ContentPage, IDisposable
    {
        CustomSlider slider;
        CustomPicker ePicker;
        CustomLayout masterLayout;
        IDeviceSpec deviceSpec;
        PurposeColor.interfaces.CustomImageButton emotionalPickerButton;
        PurposeColor.interfaces.CustomImageButton eventPickerButton;

        public FeelingsSecondPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            deviceSpec = DependencyService.Get<IDeviceSpec>();
            this.Appearing += FeelingsSecondPage_Appearing;


            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;

            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Awareness");



            slider = new CustomSlider
            {
                Minimum = -2,
                Maximum = 2,
                WidthRequest = deviceSpec.ScreenWidth * 90 / 100
            };


            Label firstLine = new Label();
            firstLine.Text = "Does being";
            firstLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            firstLine.TextColor = Color.FromRgb(40, 47, 50);
            firstLine.FontSize = Device.OnPlatform(20, 22, 30);
            firstLine.WidthRequest = deviceSpec.ScreenWidth * 70 / 100;
            firstLine.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;


            Label secondLine = new Label();
            secondLine.Text = "Happy";
            secondLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            secondLine.TextColor = Color.FromRgb(40, 47, 50);
            secondLine.FontSize = Device.OnPlatform(20, 22, 30);
            secondLine.WidthRequest = deviceSpec.ScreenWidth * 70 / 100;
            secondLine.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;


           /* Label secondLine = new Label();
            secondLine.Text = "Happy";
            secondLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            secondLine.TextColor = Color.FromRgb(40, 47, 50);
            secondLine.FontSize = Device.OnPlatform(20, 22, 30);
            secondLine.WidthRequest = deviceSpec.ScreenWidth * 70 / 100;
            secondLine.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;*/



            emotionalPickerButton = new PurposeColor.interfaces.CustomImageButton();
            emotionalPickerButton.ImageName = "select_box_whitebg.png";
            emotionalPickerButton.Text = "Select Emotion";
            emotionalPickerButton.FontSize = 18;
            emotionalPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            emotionalPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            emotionalPickerButton.TextColor = Color.Gray;
            emotionalPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            emotionalPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            emotionalPickerButton.Clicked += OnEmotionalPickerButtonClicked;

            eventPickerButton = new PurposeColor.interfaces.CustomImageButton();
            eventPickerButton.ImageName = "select_box_whitebg.png";
            eventPickerButton.Text = "Events, Situation & Thoughts";
            eventPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            eventPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            eventPickerButton.FontSize = 18;
            eventPickerButton.TextColor = Color.Gray;
            eventPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            eventPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            eventPickerButton.Clicked += OnEventPickerButtonClicked;



            Label about = new Label();
            about.Text = "About";
            about.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            about.TextColor = Color.Gray;
            about.FontSize = Device.OnPlatform(20, 16, 30);
            about.WidthRequest = deviceSpec.ScreenWidth * 50 / 100;
            about.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;
            about.HorizontalOptions = LayoutOptions.Center;



            this.Appearing += FeelingNowPage_Appearing;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(firstLine, 15, 22);
            masterLayout.AddChildToLayout(secondLine, 28, 27);
            masterLayout.AddChildToLayout(slider, 5, 35);
            masterLayout.AddChildToLayout(emotionalPickerButton, 5, 50);
            masterLayout.AddChildToLayout(about, 5, 65);
            masterLayout.AddChildToLayout(eventPickerButton, 5, 70);

            Content = masterLayout;

        }

        void FeelingsSecondPage_Appearing(object sender, System.EventArgs e)
        {
            base.OnAppearing();
           // this.Animate("", (s) => Layout(new Rectangle(((1 - s) * Width), Y, Width, Height)), 0, 600, Easing.SpringIn, null, null);
          //  this.Animate("", (s) => Layout(new Rectangle(X, (s - 1) * Height, Width, Height)), 0, 600, Easing.SpringIn, null, null); //slide down

           // this.Animate("", (s) => Layout(new Rectangle(X, (1 - s) * Height, Width, Height)), 0, 600, Easing.SpringIn, null, null); // slide up
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        void backButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new GraphPage());
        }


        void OnEmotionalPickerButtonClicked(object sender, System.EventArgs e)
        {

            CustomPicker ePicker = new CustomPicker(masterLayout, GetEmotionsList(), 70, "", false);
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

            CustomPicker ePicker = new CustomPicker(masterLayout, GetEventsList(), 50, "Add Events Situation or Thoughts", true);
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
            listSource.Add(new CustomListViewItem { Name = "Pissed off" });
            listSource.Add(new CustomListViewItem { Name = "Frustated" });
            listSource.Add(new CustomListViewItem { Name = "Determined" });
            listSource.Add(new CustomListViewItem { Name = "Bored" });
            listSource.Add(new CustomListViewItem { Name = "Frustated" });
            listSource.Add(new CustomListViewItem { Name = "Tired" });
            listSource.Add(new CustomListViewItem { Name = "Pissed off" });
            listSource.Add(new CustomListViewItem { Name = "Determined" });
            listSource.Add(new CustomListViewItem { Name = "Bored" });
            listSource.Add(new CustomListViewItem { Name = "Frustated" });
            listSource.Add(new CustomListViewItem { Name = "Tired" });
            listSource.Add(new CustomListViewItem { Name = "Pissed off" });
            listSource.Add(new CustomListViewItem { Name = "Determined" });
            listSource.Add(new CustomListViewItem { Name = "Tired" });
            listSource.Add(new CustomListViewItem { Name = "Pissed off" });
            listSource.Add(new CustomListViewItem { Name = "Determined" });
            listSource.Add(new CustomListViewItem { Name = "Bored" });
            listSource.Add(new CustomListViewItem { Name = "Frustated" });

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
            emotionalPickerButton.TextColor = Color.Black;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
        }

        void OnEventPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListViewItem item = e.SelectedItem as CustomListViewItem;
            eventPickerButton.Text = item.Name;
            eventPickerButton.TextColor = Color.Black;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
        }

        void emotionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        async void FeelingNowPage_Appearing(object sender, System.EventArgs e)
        {
            /* int val = 2;
             for( int index = 0; index < 200; index++ )

             {
                 await Task.Delay(2);

                 if( slider.Value > 90 )
                 {
                     val = -2;
                 }
                 slider.Value += val;
             }*/
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
