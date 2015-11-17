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
        PurposeColor.interfaces.CustomImageButton emotionalPickerButton;
        PurposeColor.interfaces.CustomImageButton eventPickerButton;
        Label about;

        public FeelingNowPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb( 244, 244, 244 );
            deviceSpec = DependencyService.Get<IDeviceSpec>();


            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);

            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Awareness");
            subTitleBar.NextButtonTapRecognizer.Tapped += OnNextButtonTapRecognizerTapped;
            subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;


            slider = new CustomSlider
            {
                Minimum = -2,
                Maximum = 2,
                WidthRequest = deviceSpec.ScreenWidth * 90 / 100
            };


            Label howYouAreFeeling = new Label();
            howYouAreFeeling.Text = Constants.HOW_YOU_ARE_FEELING;
            howYouAreFeeling.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            howYouAreFeeling.TextColor = Color.FromRgb(40, 47, 50);
            howYouAreFeeling.FontSize = Device.OnPlatform( 20, 22, 30 );
            howYouAreFeeling.WidthRequest = deviceSpec.ScreenWidth * 70 / 100;
            howYouAreFeeling.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;


            Label howYouAreFeeling2 = new Label();
            howYouAreFeeling2.Text = "feeling now ?";
            howYouAreFeeling2.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            howYouAreFeeling2.TextColor = Color.FromRgb( 40, 47, 50 );
            howYouAreFeeling2.FontSize = Device.OnPlatform( 20, 22, 30 );
            howYouAreFeeling2.WidthRequest = deviceSpec.ScreenWidth * 70 / 100;
            howYouAreFeeling2.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;




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
            eventPickerButton.IsVisible = false;
            eventPickerButton.ImageName = "select_box_whitebg.png";
            eventPickerButton.Text = "Events, Situation & Thoughts";
            eventPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            eventPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            eventPickerButton.FontSize = 18;
            eventPickerButton.TextColor = Color.Gray;
            eventPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            eventPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            eventPickerButton.Clicked += OnEventPickerButtonClicked;



            about = new Label();
            about.IsVisible = false;
            about.Text = "About";
            about.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            about.TextColor = Color.Gray;
            about.FontSize = Device.OnPlatform(20, 16, 30);
            about.WidthRequest = deviceSpec.ScreenWidth * 50 / 100;
            about.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;
            about.HorizontalOptions = LayoutOptions.Center;

            Image sliderDivider1 = new Image();
            sliderDivider1.Source = "drag_sepeate.png";


            Image sliderDivider2 = new Image();
            sliderDivider2.Source = "drag_sepeate.png";


            Image sliderDivider3 = new Image();
            sliderDivider3.Source = "drag_sepeate.png";

            this.Appearing += FeelingNowPage_Appearing;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(howYouAreFeeling, 15, 22);
            masterLayout.AddChildToLayout(howYouAreFeeling2, 28, 27);
            masterLayout.AddChildToLayout(slider, 5, 35);

            masterLayout.AddChildToLayout(sliderDivider1, 29, 37);
            masterLayout.AddChildToLayout(sliderDivider2, 50, 37);
            masterLayout.AddChildToLayout(sliderDivider3, 70, 37);

            masterLayout.AddChildToLayout(emotionalPickerButton, 5, 50);
            masterLayout.AddChildToLayout(about, 5, 65);
            masterLayout.AddChildToLayout(eventPickerButton, 5, 70);

            Content = masterLayout;

        }


        protected override bool OnBackButtonPressed()
        {
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            if( pickView != null )
            {
                masterLayout.Children.Remove(pickView);
                pickView = null;
                return true;
            }

            return base.OnBackButtonPressed();
        }

        void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        void OnNextButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            if(  emotionalPickerButton.Text == "Select Emotion")
            {
                DisplayAlert("Purpose Color", "Emotion not selected.", "cancel");
            }
            else if (eventPickerButton.Text == "Events, Situation & Thoughts")
            {
                DisplayAlert("Purpose Color", "Event not selected.", "cancel");
            }
            else if( slider.Value == 0 )
            {
                DisplayAlert("Purpose Color", "Feelings slider is in Neutral", "Ok");
            }
            else
            {
                Navigation.PushAsync(new FeelingsSecondPage());
            }
            
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

            CustomPicker ePicker = new CustomPicker( masterLayout, GetEmotionsList(), 70 ,"", false);
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
           App.SelectedEmotion = item.Name;
           emotionalPickerButton.TextColor = Color.Black;
           View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
           masterLayout.Children.Remove( pickView );
           pickView = null;
           eventPickerButton.IsVisible = true;
           about.IsVisible = true;
     
        }

        void OnEventPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListViewItem item = e.SelectedItem as CustomListViewItem;
            eventPickerButton.Text = item.Name;
            eventPickerButton.TextColor = Color.Black;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
            pickView = null;
        }

        void emotionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        async  void FeelingNowPage_Appearing(object sender, System.EventArgs e)
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
