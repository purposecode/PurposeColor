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
        CustomPicker ePicker;
        CustomLayout masterLayout;
        IDeviceSpec deviceSpec;
        PurposeColor.interfaces.CustomImageButton actionPickerButton;
        PurposeColor.interfaces.CustomImageButton goalsAndDreamsPickerButton;

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
            subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;


            Label firstLine = new Label();
            firstLine.Text = "Does being";
            firstLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            firstLine.TextColor = Color.FromRgb(40, 47, 50);
            firstLine.FontSize = Device.OnPlatform(20, 22, 30);
            firstLine.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;
            firstLine.HorizontalOptions = LayoutOptions.Center;
            firstLine.WidthRequest = deviceSpec.ScreenWidth;
            firstLine.XAlign = TextAlignment.Center;


            Label secondLine = new Label();
            secondLine.Text = App.SelectedEmotion;
            secondLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            secondLine.TextColor = Color.FromRgb(40, 47, 50);
            secondLine.FontSize = Device.OnPlatform(20, 22, 30);
            secondLine.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;
            secondLine.HorizontalOptions = LayoutOptions.Center;
            secondLine.WidthRequest = deviceSpec.ScreenWidth;
            secondLine.XAlign = TextAlignment.Center;
            


            Label thirdLine = new Label();
            thirdLine.Text = "support your goals and dreams?";
            thirdLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            thirdLine.TextColor = Color.FromRgb(40, 47, 50);
            thirdLine.FontSize = Device.OnPlatform(20, 22, 30);
            thirdLine.HeightRequest = deviceSpec.ScreenHeight * 10 / 100;
            thirdLine.HorizontalOptions = LayoutOptions.Center;
            thirdLine.WidthRequest = deviceSpec.ScreenWidth;
            thirdLine.XAlign = TextAlignment.Center;


            goalsAndDreamsPickerButton = new PurposeColor.interfaces.CustomImageButton();
            goalsAndDreamsPickerButton.ImageName = "select_box_whitebg.png";
            goalsAndDreamsPickerButton.Text = "Goals & Dreams";
            goalsAndDreamsPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            goalsAndDreamsPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            goalsAndDreamsPickerButton.FontSize = 18;
            goalsAndDreamsPickerButton.TextColor = Color.Gray;
            goalsAndDreamsPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            goalsAndDreamsPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            goalsAndDreamsPickerButton.Clicked += OnGoalsPickerButtonClicked;


            actionPickerButton = new PurposeColor.interfaces.CustomImageButton();
            actionPickerButton.IsVisible = false;
            actionPickerButton.ImageName = "select_box_whitebg.png";
            actionPickerButton.Text = "Supporting Action";
            actionPickerButton.FontSize = 18;
            actionPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            actionPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            actionPickerButton.TextColor = Color.Gray;
            actionPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            actionPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            actionPickerButton.Clicked += OnActionPickerButtonClicked;


            CustomSlider slider = new CustomSlider
            {
                Minimum = -2,
                Maximum = 2,
                WidthRequest = deviceSpec.ScreenWidth * 90 / 100
            };


            Image sliderDivider1 = new Image();
            sliderDivider1.Source = "drag_sepeate.png";


            Image sliderDivider2 = new Image();
            sliderDivider2.Source = "drag_sepeate.png";


            Image sliderDivider3 = new Image();
            sliderDivider3.Source = "drag_sepeate.png";

            Image sliderBG = new Image();
            sliderBG.Source = "drag_bg.png";

            this.Appearing += FeelingNowPage_Appearing;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(firstLine, 0, 22);
            masterLayout.AddChildToLayout(secondLine, 0, 27);
            masterLayout.AddChildToLayout(thirdLine, 0, 32);
            masterLayout.AddChildToLayout(sliderBG, 7, 45);
            masterLayout.AddChildToLayout(slider, 5, 39);
            masterLayout.AddChildToLayout(sliderDivider1, 30, 45);
            masterLayout.AddChildToLayout(sliderDivider2, 50, 45);
            masterLayout.AddChildToLayout(sliderDivider3, 70, 45);
            masterLayout.AddChildToLayout(goalsAndDreamsPickerButton, 5, 55);
            masterLayout.AddChildToLayout(actionPickerButton, 5, 70);

            Content = masterLayout;

        }

        void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
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


        void OnActionPickerButtonClicked(object sender, System.EventArgs e)
        {

            CustomPicker ePicker = new CustomPicker(masterLayout, App.GetActionsList(), 50, Constants.ADD_ACTIONS, true, true);
            ePicker.WidthRequest = deviceSpec.ScreenWidth;
            ePicker.HeightRequest = deviceSpec.ScreenHeight;
            ePicker.ClassId = "ePicker";
            ePicker.listView.ItemSelected += OnActionPickerItemSelected;
            masterLayout.AddChildToLayout(ePicker, 0, 0);

            //double yPos = 60 * deviceSpec.ScreenHeight / 100;
            // ePicker.TranslateTo(0, -yPos, 250, Easing.BounceIn);
            // ePicker.FadeTo(1, 750, Easing.Linear); 
        }

        void OnGoalsPickerButtonClicked(object sender, System.EventArgs e)
        {

            CustomPicker ePicker = new CustomPicker(masterLayout, App.GetGoalsList(), 50, Constants.ADD_GOALS, true, true);
            ePicker.WidthRequest = deviceSpec.ScreenWidth;
            ePicker.HeightRequest = deviceSpec.ScreenHeight;
            ePicker.ClassId = "ePicker";
            ePicker.listView.ItemSelected += OnGoalsPickerItemSelected;
            masterLayout.AddChildToLayout(ePicker, 0, 0);
            //double yPos = 60 * deviceSpec.ScreenHeight / 100;
            //ePicker.TranslateTo(0, yPos, 250, Easing.BounceIn);
            // ePicker.FadeTo(1, 750, Easing.Linear); 
        }



        protected override bool OnBackButtonPressed()
        {
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            if (pickView != null)
            {
                masterLayout.Children.Remove(pickView);
                pickView = null;
                return true;
            }

            return base.OnBackButtonPressed();
        }

        void OnActionPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListViewItem item = e.SelectedItem as CustomListViewItem;
            actionPickerButton.Text = item.Name;
            actionPickerButton.TextColor = Color.Black;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
            pickView = null;
        }

        void OnGoalsPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListViewItem item = e.SelectedItem as CustomListViewItem;
            goalsAndDreamsPickerButton.Text = item.Name;
            goalsAndDreamsPickerButton.TextColor = Color.Black;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
            pickView = null;
            actionPickerButton.IsVisible = true;
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
            ePicker = null;
            masterLayout = null;
            deviceSpec = null;
            actionPickerButton = null;
            goalsAndDreamsPickerButton = null;
        }
    }
}
