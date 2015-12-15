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
using PurposeColor.Service;
using System.Diagnostics;
using PurposeColor.interfaces;

namespace PurposeColor
{

    public class FeelingNowPage : ContentPage, IDisposable
    {
        CustomSlider slider;
        CustomPicker ePicker;
        CustomLayout masterLayout;
        public PurposeColor.interfaces.CustomImageButton emotionalPickerButton;
        public PurposeColor.interfaces.CustomImageButton eventPickerButton;
        CustomListViewItem selectedEmotionItem;
        public CustomListViewItem selectedEventItem;
        Label about;
        public static int sliderValue;
        double screenHeight;
        double screenWidth;
        IProgressBar progressBar;

        public FeelingNowPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            progressBar = DependencyService.Get<IProgressBar>();

            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Awareness");
            subTitleBar.NextButtonTapRecognizer.Tapped += OnNextButtonTapRecognizerTapped;
            subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;

            slider = new CustomSlider
            {
                Minimum = -2,
                Maximum = 2,
                WidthRequest = screenWidth * 90 / 100
            };
            slider.StopGesture = GetstopGetsture;

            //slider.ValueChanged += slider_ValueChanged;

            Label howYouAreFeeling = new Label();
            howYouAreFeeling.Text = Constants.HOW_YOU_ARE_FEELING;
            howYouAreFeeling.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            howYouAreFeeling.TextColor = Color.FromRgb(40, 47, 50);

            //howYouAreFeeling.WidthRequest = screenWidth * 70 / 100;
            //howYouAreFeeling.HeightRequest = screenHeight * 15 / 100;
            howYouAreFeeling.HorizontalOptions = LayoutOptions.Center;

            Label howYouAreFeeling2 = new Label();
            howYouAreFeeling2.Text = "feeling now ?";
            howYouAreFeeling2.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            howYouAreFeeling2.TextColor = Color.FromRgb(40, 47, 50);

            howYouAreFeeling2.HorizontalOptions = LayoutOptions.Center;
            //howYouAreFeeling2.WidthRequest = screenWidth * 70 / 100;
            //howYouAreFeeling2.HeightRequest = screenHeight * 15 / 100;

            emotionalPickerButton = new PurposeColor.interfaces.CustomImageButton();
            emotionalPickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", @"/Assets/select_box_whitebg.png");
            emotionalPickerButton.Text = "Select Emotion";

            emotionalPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            emotionalPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            emotionalPickerButton.TextColor = Color.Gray;
            emotionalPickerButton.WidthRequest = screenWidth * 90 / 100;
            emotionalPickerButton.Clicked += OnEmotionalPickerButtonClicked;

            eventPickerButton = new PurposeColor.interfaces.CustomImageButton();
            eventPickerButton.IsVisible = false;
            eventPickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "/Assets/select_box_whitebg.png");
            eventPickerButton.Text = "Events, Situation & Thoughts";
            eventPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            eventPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            eventPickerButton.TextColor = Color.Gray;
            eventPickerButton.WidthRequest = screenWidth * 90 / 100;

            eventPickerButton.Clicked += OnEventPickerButtonClicked;

            about = new Label();
            about.IsVisible = false;
            about.Text = "About";
            about.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            //about.TextColor = Color.Gray;
            //about.FontSize = Device.OnPlatform(16, 16, 30);
			about.WidthRequest = screenWidth;
            //about.HeightRequest = screenHeight * 15 / 100;
            about.HorizontalOptions = LayoutOptions.Center;
			about.XAlign = TextAlignment.Center;

            about.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            about.TextColor = Color.FromRgb(40, 47, 50);


            int fontSize = 15;
            if (App.screenDensity > 1.5)
            {
                howYouAreFeeling.FontSize = Device.OnPlatform(20, 22, 30);
                howYouAreFeeling2.FontSize = Device.OnPlatform(20, 22, 30);
                about.FontSize = Device.OnPlatform(15, 22, 30);

                emotionalPickerButton.HeightRequest = screenHeight * 6 / 100;
                fontSize = 17;
                eventPickerButton.HeightRequest = screenHeight * 6 / 100;
            }
            else
            {
                howYouAreFeeling.FontSize = Device.OnPlatform(16, 20, 26);
                howYouAreFeeling2.FontSize = Device.OnPlatform(16, 20, 26);
                about.FontSize = Device.OnPlatform(16, 20, 26);

                emotionalPickerButton.HeightRequest = screenHeight * 9 / 100;
                fontSize = 15;
                eventPickerButton.HeightRequest = screenHeight * 9 / 100;
            }

            emotionalPickerButton.FontSize = Device.OnPlatform( fontSize, fontSize, 22 );
            eventPickerButton.FontSize = Device.OnPlatform(fontSize, fontSize, 22);

            Image sliderDivider1 = new Image();
            sliderDivider1.Source = Device.OnPlatform("drag_sepeate.png", "drag_sepeate.png", "//Assets//drag_sepeate.png");
            //bgImage.Source = Device.OnPlatform("top_bg.png", "light_blue_bg.png", "//Assets//light_blue_bg.png");

            //Image sliderDivider2 = new Image();
            //sliderDivider2.Source = "drag_sepeate.png";

            //Image sliderDivider3 = new Image();
            //sliderDivider3.Source = "drag_sepeate.png";

            //Image sliderBG = new Image();
            //sliderBG.Source = "drag_bg.png";

            this.Appearing += OnFeelingNowPageAppearing;

            sliderValue = slider.CurrentValue;
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(howYouAreFeeling, 16, Device.OnPlatform( 22,22,22 ));
            masterLayout.AddChildToLayout(howYouAreFeeling2, 29, Device.OnPlatform(27,27,27));
            //  masterLayout.AddChildToLayout(sliderBG, 7, 40);
            masterLayout.AddChildToLayout(slider, 5, 34);

            /* masterLayout.AddChildToLayout(sliderDivider1, 30, 40.5f);
             masterLayout.AddChildToLayout(sliderDivider2, 50, 40.5f);
             masterLayout.AddChildToLayout(sliderDivider3, 70, 40.5f);*/

            masterLayout.AddChildToLayout(emotionalPickerButton, 5, Device.OnPlatform( 50,50,47 ));
            masterLayout.AddChildToLayout(about, 0, Device.OnPlatform(62, 62, 59));
            masterLayout.AddChildToLayout(eventPickerButton, 5, Device.OnPlatform(70, 70,67));

            Content = masterLayout;

        }

        public async void GetstopGetsture(bool pressed)
        {
            /* if (slider.Value != 0)
             {
                 View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                 if (pickView != null)
                     return;

                 CustomPicker ePicker = new CustomPicker(masterLayout, App.GetEmotionsList(), 65, "Select Emotions", true, false);
                 ePicker.WidthRequest = screenWidth;
                 ePicker.HeightRequest = screenHeight;
                 ePicker.ClassId = "ePicker";
                 ePicker.listView.ItemSelected += OnEmotionalPickerItemSelected;
                 masterLayout.AddChildToLayout(ePicker, 0, 0);
             }*/

            sliderValue = slider.CurrentValue;
            if (slider.CurrentValue == 0)
            {

                progressBar.ShowToast("slider is in neutral");
            }
            else
            {
                OnEmotionalPickerButtonClicked(emotionalPickerButton, EventArgs.Empty);
            }
        }

        void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {

                if (slider.Value != 0)
                {
                    View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                    if (pickView != null)
                        return;

                    CustomPicker ePicker = new CustomPicker(masterLayout, App.GetEmotionsList(), 65, "Select Emotions", true, false);
                    ePicker.WidthRequest = screenWidth;
                    ePicker.HeightRequest = screenHeight;
                    ePicker.ClassId = "ePicker";
                    ePicker.listView.ItemSelected += OnEmotionalPickerItemSelected;
                    masterLayout.AddChildToLayout(ePicker, 0, 0);
                }

            }
            catch (System.Exception ex)
            {
                var test = ex.Message;
            }
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

        void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        async void OnNextButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {

            try
            {

                if (emotionalPickerButton.Text == "Select Emotion")
                {
                    await DisplayAlert("Purpose Color", "Emotion not selected.", "Ok");
                }
                else if (eventPickerButton.Text == "Events, Situation & Thoughts")
                {
                    await DisplayAlert("Purpose Color", "Event not selected.", "Ok");
                }
                else if (slider.Value == 0)
                {
                    await DisplayAlert("Purpose Color", "Feelings slider is in Neutral", "Ok");
                }
                else
                {
                    SaveData();
                }
            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Network error, unable to save the detais, please try again", Constants.ALERT_OK);
            }
        }

        private async void SaveData()
        {
            try
            {
                progressBar.ShowProgressbar("Saving details..");
                bool isDataSaved = await ServiceHelper.SaveEmotionAndEvent(selectedEmotionItem.EmotionID, selectedEventItem.EventID, "2");
                progressBar.HideProgressbar();
                if (!isDataSaved)
                {
                    bool doRetry = await DisplayAlert(Constants.ALERT_TITLE, "Network error, unable to save the detais", "Retry", "Cancel");
                    if (doRetry)
                    {
                        SaveData();
                    }
				}
				else
				{
					await Navigation.PushAsync(new FeelingsSecondPage());
				}
            }
            catch (System.Exception ex)
            {
                progressBar.HideProgressbar();
                DisplayAlert(Constants.ALERT_TITLE, "Network error, unable to save the detais", "OK");
            }
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        void backButton_Clicked(object sender, System.EventArgs e)
        {

            try
            {
                Navigation.PushAsync(new GraphPage());
            }
            catch (System.Exception)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Please try again", Constants.ALERT_OK);
            }
        }

       async  void OnEmotionalPickerButtonClicked(object sender, System.EventArgs e)
        {
            try
            {

                List<CustomListViewItem> pickerSource = App.emotionsListSource.Where(toAdd => toAdd.SliderValue == slider.CurrentValue).ToList();
                CustomPicker ePicker = new CustomPicker(masterLayout, pickerSource, Device.OnPlatform( 65, 65, 55 ), Constants.SELECT_EMOTIONS, true, true);// 65
                ePicker.WidthRequest = screenWidth;
                ePicker.HeightRequest = screenHeight;
                ePicker.ClassId = "ePicker";
                ePicker.FeelingsPage = this;
                ePicker.listView.ItemSelected += OnEmotionalPickerItemSelected;
				masterLayout.AddLayout(ePicker, 0, Device.OnPlatform( 0, 0, 0 ));
                ePicker.ChildRemoved += async (s, ee)=>
                {
                    var test = "test";
                };

                //ePicker.FadeTo(1, 1000, Easing.CubicOut);
                //double yPos = 60 * screenHeight / 100;
                //ePicker.TranslateTo(0, 65, 3000, Easing.Linear);
                // ePicker.FadeTo(1, 750, Easing.Linear); 

            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Please try again", Constants.ALERT_OK);
            }
        }

        public void OnEventPickerButtonClicked(object sender, System.EventArgs e)
        {
            try
            {
                CustomPicker ePicker = new CustomPicker(masterLayout, App.GetEventsList(), 45, Constants.ADD_EVENTS, true, true);
                ePicker.WidthRequest = screenWidth;
                ePicker.HeightRequest = screenHeight;
                ePicker.ClassId = "ePicker";
                ePicker.listView.ItemSelected += OnEventPickerItemSelected;
				masterLayout.AddLayout(ePicker, 0, 0);
                //double yPos = 60 * screenHeight / 100;
                //ePicker.TranslateTo(0, yPos, 250, Easing.BounceIn);
                // ePicker.FadeTo(1, 750, Easing.Linear); 

            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Please try again", Constants.ALERT_OK);
            }
        }

        public void OnEmotionalPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                CustomListViewItem item = e.SelectedItem as CustomListViewItem;
                emotionalPickerButton.Text = item.Name;
                selectedEmotionItem = item;
                emotionalPickerButton.TextColor = Color.Black;
                App.SelectedEmotion = item.Name;
                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                masterLayout.Children.Remove(pickView);
                pickView = null;
                eventPickerButton.IsVisible = true;
                about.IsVisible = true;
                OnEventPickerButtonClicked(eventPickerButton, EventArgs.Empty);

            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Please try again", Constants.ALERT_OK);
            }
        }

        void OnEventPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                CustomListViewItem item = e.SelectedItem as CustomListViewItem;
                eventPickerButton.Text = item.Name;
                eventPickerButton.TextColor = Color.Black;
                selectedEventItem = item;
                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                masterLayout.Children.Remove(pickView);
                pickView = null;

            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Please try again", Constants.ALERT_OK);
            }
        }

        void emotionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        async void OnFeelingNowPageAppearing(object sender, System.EventArgs e)
        {
            try
            {
                base.OnAppearing();
                if (App.emotionsListSource == null || App.emotionsListSource.Count < 1)
                {
                    progressBar.ShowProgressbar("Loading emotions...");
                    var downloadEmotionStatus = await DownloadAllEmotions();
                    if (!downloadEmotionStatus)
                    {
                        progressBar.HideProgressbar();
                        DisplayAlert("Purpose Color", "Netwrok error unable to update  the emotions.", "Ok");
                        
                        var oldList = App.emotionsListSource;
                        App.emotionsListSource = App.Settings.GetAllEmotions();
                    }
                    else
                    {
                        if (App.emotionsListSource != null)
                        {
                            App.Settings.DeleteAllEmotions();
                            App.Settings.SaveEmotions(App.emotionsListSource);
                        }
                    }

                    progressBar.HideProgressbar();
                }

                if (App.eventsListSource == null || App.eventsListSource.Count < 1)
                {
                    var downloadEventsStatus  = await DownloadAllEvents();
                    if (!downloadEventsStatus)
                    {
                        //DisplayAlert("Purpose Color", "Netwrok error unable to update  the emotions.", "Ok");
                        App.eventsListSource = App.Settings.GetAllEvents();
                    }
                    else
                    {
                        if (App.eventsListSource != null)
                        {
                            App.Settings.DeleteAllEvents();
                            App.Settings.SaveEvents(App.eventsListSource);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Please try again", Constants.ALERT_OK);
            }
        }

        public async Task<bool> DownloadAllEmotions()
        {
            try
            {

                var emotionsReult = await ServiceHelper.GetAllEmotions(2);
                if (emotionsReult != null)
                {
                    App.emotionsListSource = null;
                    App.emotionsListSource = emotionsReult;
                    emotionsReult = null;
                    return true;
                }
                else
                {
                    //DisplayAlert(Constants.ALERT_TITLE, "Could not refresh the emotions.", Constants.ALERT_OK);
                    //var oldList = App.emotionsListSource;
                    //App.emotionsListSource = App.Settings.GetAllEmotions();
                }

            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Could not refresh the emotions.", Constants.ALERT_OK);
                

            }
            return false;

        }

        public static async Task<bool> DownloadAllEvents()
        {
            try
            {
                var eventList = await ServiceHelper.GetAllEvents();
                if (eventList != null)
                {
                    App.eventsListSource = null;
                    App.eventsListSource = new List<CustomListViewItem>();
                    foreach (var item in eventList)
                    {
                        App.eventsListSource.Add(item);
                    }
                    eventList = null;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                //DisplayAlert(Constants.ALERT_TITLE, "Could not refresh the events", Constants.ALERT_OK);
                return false;
            }

            return false;
        }

        public void Dispose()
        {
            eventPickerButton = null;
            eventPickerButton.Clicked -= OnEventPickerButtonClicked;
            emotionalPickerButton = null;
            emotionalPickerButton.Clicked -= OnEmotionalPickerButtonClicked;
            slider = null;
            ePicker = null;
            masterLayout = null;
            progressBar = null;
            selectedEmotionItem = null;
            selectedEventItem = null;
            about = null;

            this.Appearing -= OnFeelingNowPageAppearing;

            GC.Collect();
        }
    }
}
