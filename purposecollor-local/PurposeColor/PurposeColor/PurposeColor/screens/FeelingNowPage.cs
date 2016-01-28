using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
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
        CustomSlider slider = null;
        CustomSlider popupSlider;
        CustomLayout masterLayout = null;
        public PurposeColor.interfaces.CustomImageButton emotionalPickerButton = null;

        public PurposeColor.interfaces.CustomImageButton eventPickerButton = null;
        CustomListViewItem selectedEmotionItem = null;
        public CustomListViewItem selectedEventItem = null;
        Label about = null;
        public static int sliderValue;
        double screenHeight;
        double screenWidth;
        IProgressBar progressBar = null;
        PurposeColorSubTitleBar subTitleBar = null;
        PurposeColorTitleBar mainTitleBar = null;
        Label sliderValLabel;
        Label emotionTextLabel;
        Label eventTextLabel;
        Image circleImg3;
        Image circleImg2;
        Image circleImg1;
        StackLayout imagesContainer;
        bool emotionsDisplaying = false;
        bool eventsDisplaying = false;

        public FeelingNowPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            progressBar = DependencyService.Get<IProgressBar>();

            mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
            subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Awareness");
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
            #region Top stack - dynamic label

            BoxView hLine = new BoxView { BackgroundColor = Constants.TEXT_COLOR_GRAY, WidthRequest = App.screenWidth, HeightRequest = 1 };

            //imagesContainer = new StackLayout
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    BackgroundColor = Color.Transparent, //
            //    HorizontalOptions = LayoutOptions.Center,
            //    VerticalOptions = LayoutOptions.Center,
            //    Padding = new Thickness(30, 0, 0, 0),
            //    Spacing = (App.screenWidth / 3),
            //    WidthRequest = App.screenWidth,

            //};

            circleImg1 = new Image
            {
                Source = "finger_gray",//"icn_list",
                HeightRequest = 20,
                WidthRequest = 20
            };
            circleImg2 = new Image
            {
                Source = "finger_gray",//"icn_list",
                HeightRequest = 20,
                WidthRequest = 20,
                Rotation = 180
            };
            circleImg3 = new Image
            {
                Source = "finger_gray",//"icn_list",
                HeightRequest = 20,
                WidthRequest = 20
            };

            //imagesContainer.Children.Add(circleImg1);
            //imagesContainer.Children.Add(circleImg2);
            //imagesContainer.Children.Add(circleImg3);

            masterLayout.AddChildToLayout(circleImg1, 8, (Device.OnPlatform(9, 23, 10)));
            masterLayout.AddChildToLayout(circleImg2, 47, (Device.OnPlatform(9, 26, 10)));
            masterLayout.AddChildToLayout(circleImg3, 85, (Device.OnPlatform(9, 23, 10)));
            circleImg1.TranslationY = -1;
            circleImg2.TranslationY = -1;
            circleImg3.TranslationY = -1;


            StackLayout inputAckLabelStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = 5,
                HeightRequest = 1,
                WidthRequest = App.screenWidth,
                Children = { hLine }
            };

            #endregion

            Label howYouAreFeeling = new Label();
            howYouAreFeeling.Text = Constants.HOW_YOU_ARE_FEELING;
            howYouAreFeeling.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            howYouAreFeeling.TextColor = Color.FromRgb(40, 47, 50);
            howYouAreFeeling.HorizontalOptions = LayoutOptions.Center;

            Label howYouAreFeeling2 = new Label();
            howYouAreFeeling2.Text = "feeling now ?";
            howYouAreFeeling2.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            howYouAreFeeling2.TextColor = Color.FromRgb(40, 47, 50);
            howYouAreFeeling2.HorizontalOptions = LayoutOptions.Center;

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

            if (!eventsDisplaying)
            {
                eventPickerButton.Clicked += OnEventPickerButtonClicked;
            }

            about = new Label();
            about.IsVisible = false;
            about.Text = "About";
            about.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            about.WidthRequest = screenWidth;
            about.HorizontalOptions = LayoutOptions.Center;
            about.XAlign = TextAlignment.Center;

            about.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            about.TextColor = Color.FromRgb(40, 47, 50);

            int fontSize = 15;
            if (App.screenDensity > 1.5)
            {
                howYouAreFeeling.FontSize = Device.OnPlatform(20, 22, 30);
                howYouAreFeeling2.FontSize = Device.OnPlatform(20, 22, 30);
                about.FontSize = Device.OnPlatform(15, 18, 30);

                emotionalPickerButton.HeightRequest = screenHeight * 6 / 100;
                fontSize = 17;
                eventPickerButton.HeightRequest = screenHeight * 6 / 100;
            }
            else
            {
                howYouAreFeeling.FontSize = Device.OnPlatform(16, 18, 26);
                howYouAreFeeling2.FontSize = Device.OnPlatform(16, 18, 26);
                about.FontSize = Device.OnPlatform(16, 18, 26);

                emotionalPickerButton.HeightRequest = screenHeight * 9 / 100;
                fontSize = 15;
                eventPickerButton.HeightRequest = screenHeight * 9 / 100;
            }

            emotionalPickerButton.FontSize = Device.OnPlatform(fontSize, fontSize, 22);
            eventPickerButton.FontSize = Device.OnPlatform(fontSize, fontSize, 22);

            this.Appearing += OnFeelingNowPageAppearing;

            sliderValue = slider.CurrentValue;
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(inputAckLabelStack, 0, Device.OnPlatform(9, 25, 10));
            //masterLayout.AddChildToLayout(imagesContainer,0, (Device.OnPlatform(9, 25, 10)));//25: center f line.

            sliderValLabel = new Label
            {
                TextColor = Constants.TEXT_COLOR_GRAY,//Constants.BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                XAlign = TextAlignment.Center,
                WidthRequest = 80//,
                //Text = "werewr qrawe"
            };

            #region SLIDER LABEL TAP

            TapGestureRecognizer sliderLabelTapRecognizer = new TapGestureRecognizer();
            sliderValLabel.GestureRecognizers.Add(sliderLabelTapRecognizer);
            sliderLabelTapRecognizer.Tapped += (s, e) =>
            {
                /// show a slider as a popup and get its value,

                RemoveSliderPopup();

                popupSlider = new CustomSlider
                {
                    Minimum = -2,
                    Maximum = 2,
                    WidthRequest = screenWidth * 90 / 100,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
                
                StackLayout sliderBg = new StackLayout
                {
                    BackgroundColor = Color.Black,
                    Opacity = .95,
                    HeightRequest = App.screenHeight,
                    WidthRequest = App.screenWidth,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                                    new StackLayout{HeightRequest = 250},
                                    new StackLayout{
                                        Children = { popupSlider },
                                        Padding = 10,
                                        BackgroundColor = Color.FromRgb(244, 244, 244),
                                        HorizontalOptions = LayoutOptions.Center,
                                        VerticalOptions = LayoutOptions.Center,
                                        Opacity = 1
                                    }
                                },
                    ClassId = "sliderBg"
                };

                TapGestureRecognizer sliderBgTapRecognizer = new TapGestureRecognizer();
                sliderBg.GestureRecognizers.Add(sliderBgTapRecognizer);
                sliderBgTapRecognizer.Tapped += (snd, eve) =>
                {
                    RemoveSliderPopup();
                };

                popupSlider.CurrentValue = slider.CurrentValue;
                popupSlider.StopGesture = GetstopGetsture;

                masterLayout.AddChildToLayout(sliderBg, 0, 0);
            };
            
            #endregion

            emotionTextLabel = new Label
            {
                TextColor = Constants.TEXT_COLOR_GRAY,//BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                XAlign = TextAlignment.Center,
                WidthRequest = 150//,
                // Text = "erewrewr werewrww"
            };
            TapGestureRecognizer emotionTextTap = new TapGestureRecognizer();
            emotionTextLabel.GestureRecognizers.Add(emotionTextTap);
            emotionTextTap.Tapped += async (s, e) =>
            {
                if (!emotionsDisplaying)
                {
                    await Task.Delay(500);
                    OnEmotionalPickerButtonClicked(emotionalPickerButton, EventArgs.Empty);
                }
            };

            eventTextLabel = new Label
            {
                TextColor = Constants.TEXT_COLOR_GRAY,//BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                XAlign = TextAlignment.End,
                WidthRequest = 200//,
                // Text ="werewr wer ewr wer w"
            };

            TapGestureRecognizer eventTextTap = new TapGestureRecognizer();
            eventTextLabel.GestureRecognizers.Add(eventTextTap);
            eventTextTap.Tapped += async (s, e) =>
            {
                if (!eventsDisplaying)
                {
                    await Task.Delay(500);
                    OnEventPickerButtonClicked(eventPickerButton, EventArgs.Empty);
                }
            };

            masterLayout.AddChildToLayout(sliderValLabel, 1, (Device.OnPlatform(9, 19, 10)));
            masterLayout.AddChildToLayout(emotionTextLabel, 30, (Device.OnPlatform(9, 29, 10)));//27
            masterLayout.AddChildToLayout(eventTextLabel, 38, (Device.OnPlatform(9, 19, 10)));



            masterLayout.AddChildToLayout(howYouAreFeeling, 16, Device.OnPlatform(22, 33, 30));//30
            masterLayout.AddChildToLayout(howYouAreFeeling2, 29, Device.OnPlatform(27, 38, 27));//35
            //  masterLayout.AddChildToLayout(sliderBG, 7, 40);
            masterLayout.AddChildToLayout(slider, 5, 43);//40

            masterLayout.AddChildToLayout(emotionalPickerButton, 5, Device.OnPlatform(50, 57, 47));//55
            masterLayout.AddChildToLayout(about, 0, Device.OnPlatform(62, 66, 59));//65
            masterLayout.AddChildToLayout(eventPickerButton, 5, Device.OnPlatform(70, 73, 67));
            SetFeedBackLablText();
            Content = masterLayout;

        }

        private void RemoveSliderPopup()
        {
            try
            {

                View sliderContainer = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "sliderBg");
                if (sliderContainer != null)
                {
                    masterLayout.Children.Remove(sliderContainer);
                    sliderContainer = null;
                    GC.Collect();
                }
                
            }
            catch (Exception)
            {
            }
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
            

            if (popupSlider != null)
            {
                sliderValue = popupSlider.CurrentValue;
                slider.Value = popupSlider.CurrentValue;
                popupSlider = null;
            }
            else
            {
                sliderValue = slider.CurrentValue;
            }

            if (sliderValue == 0)
            {
                sliderValLabel.Text = "";
                circleImg1.Source = "finger_gray";
                progressBar.ShowToast("slider is in neutral");
            }
            else
            {
                if (sliderValue == 2)
                {
                    sliderValLabel.Text = "like the most";
                }
                else if (sliderValue == 1)
                {
                    sliderValLabel.Text = "do like";
                }
                else if (sliderValue == -1)
                {
                    sliderValLabel.Text = "don't like";
                }
                else if (sliderValue == -2)
                {
                    sliderValLabel.Text = "worst";
                }

                sliderValLabel.XAlign = TextAlignment.Center;
                circleImg1.Source = "finger";//"icn_selected";
            }

            if (!emotionsDisplaying && sliderValue != 0)
            {
                await Task.Delay(500);
                RemoveSliderPopup();
                OnEmotionalPickerButtonClicked(emotionalPickerButton, EventArgs.Empty);
            }
        }

        void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                if (popupSlider != null)
                {
                    sliderValue = popupSlider.CurrentValue;
                }
                else
                {
                    sliderValue = slider.CurrentValue;
                }

                if (sliderValue != 0)
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
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        async void OnNextButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            try
            {
                this.subTitleBar.NextButtonTapRecognizer.Tapped -= OnNextButtonTapRecognizerTapped; // to prevent duplicate submission when double tapped. // readded the event after saving data.

                if (emotionalPickerButton.Text == "Select Emotion")
                {
                    await DisplayAlert("Purpose Color", "Emotion not selected.", "Ok");
                    this.subTitleBar.NextButtonTapRecognizer.Tapped += OnNextButtonTapRecognizerTapped;
                }
                else if (eventPickerButton.Text == "Events, Situation & Thoughts")
                {
                    await DisplayAlert("Purpose Color", "Event not selected.", "Ok");
                    this.subTitleBar.NextButtonTapRecognizer.Tapped += OnNextButtonTapRecognizerTapped;
                }
                else if (slider.Value == 0)
                {
                    await DisplayAlert("Purpose Color", "Feelings slider is in Neutral", "Ok");
                    this.subTitleBar.NextButtonTapRecognizer.Tapped += OnNextButtonTapRecognizerTapped;
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
                        return;
                    }
                }
                else
                {
                    this.subTitleBar.NextButtonTapRecognizer.Tapped += OnNextButtonTapRecognizerTapped;
                    await Navigation.PushAsync(new FeelingsSecondPage());
                    return;
                }
            }
            catch (System.Exception ex)
            {
                progressBar.HideProgressbar();
                DisplayAlert(Constants.ALERT_TITLE, "Network error, unable to save the detais", "OK");
            }
            this.subTitleBar.NextButtonTapRecognizer.Tapped += OnNextButtonTapRecognizerTapped; // added to perform function if user navigates back to this page and changes selection.
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        async void OnEmotionalPickerButtonClicked(object sender, System.EventArgs e)
        {
            try
            {
                this.emotionalPickerButton.Clicked -= OnEmotionalPickerButtonClicked;
                emotionsDisplaying = true;
                await emotionalPickerButton.ScaleTo(1.5, 100, Easing.Linear);
                await emotionalPickerButton.ScaleTo(1, 100, Easing.Linear);


                List<CustomListViewItem> pickerSource = App.emotionsListSource.Where(toAdd => toAdd.SliderValue == slider.CurrentValue).ToList();
                CustomPicker ePicker = new CustomPicker(masterLayout, pickerSource, Device.OnPlatform(65, 65, 55), Constants.SELECT_EMOTIONS, true, true);// 65
                ePicker.WidthRequest = screenWidth;
                ePicker.HeightRequest = screenHeight;
                ePicker.ClassId = "ePicker";
                ePicker.FeelingsPage = this;
                ePicker.listView.ItemSelected += OnEmotionalPickerItemSelected;
                masterLayout.AddLayout(ePicker, 0, Device.OnPlatform(0, 0, 0));
                ePicker.ChildRemoved += async (s, ee) =>
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
            this.emotionalPickerButton.Clicked += OnEmotionalPickerButtonClicked;
            emotionsDisplaying = false;
        }

        public async void OnEventPickerButtonClicked(object sender, System.EventArgs e)
        {
            try
            {
                eventsDisplaying = true;
                this.eventPickerButton.Clicked -= OnEventPickerButtonClicked;
                
                await eventPickerButton.ScaleTo(1.5, 100, Easing.Linear);
                await eventPickerButton.ScaleTo(1, 100, Easing.Linear);

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
            this.eventPickerButton.Clicked += OnEventPickerButtonClicked;
            eventsDisplaying = false;
        }

        public async void OnEmotionalPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
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
                if (! eventsDisplaying)
                {
                    await Task.Delay(500);
                    OnEventPickerButtonClicked(eventPickerButton, EventArgs.Empty);

                    SetFeedBackLablText();
                }

                string trimmedText = item.Name;
                if (trimmedText.Length > 15)
                {
                    trimmedText = trimmedText.Substring(0, 15);
                    trimmedText += "..";
                }
                emotionTextLabel.Text = trimmedText;
                emotionTextLabel.XAlign = TextAlignment.Center;

                circleImg2.Source = "finger";// "icn_selected";
            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Please try again", Constants.ALERT_OK);
            }
        }

        private void SetFeedBackLablText()
        {

            var spanString = new FormattedString();
            string trimmedText;
            spanString.Spans.Add(new Span { Text = "Feeling ", ForegroundColor = Color.Black, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 16 });
            if (selectedEmotionItem != null)
            {
                if (selectedEmotionItem.Name != null && selectedEmotionItem.Name.Length > 10)
                {
                    trimmedText = selectedEmotionItem.Name.Substring(0, 10);
                    trimmedText += "..";
                }
                else
                {
                    trimmedText = selectedEmotionItem.Name;
                }
                spanString.Spans.Add(new Span { Text = trimmedText, ForegroundColor = Constants.BLUE_BG_COLOR, FontAttributes = FontAttributes.Italic, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 20 });
            }
            else
            {
                spanString.Spans.Add(new Span { Text = "............", ForegroundColor = Color.Black, FontAttributes = FontAttributes.Italic, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 16 });
            }
            spanString.Spans.Add(new Span { Text = " about ", ForegroundColor = Color.Black, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 16 });

            if (selectedEventItem != null)
            {
                if (selectedEventItem.Name != null && selectedEventItem.Name.Length > 10)
                {
                    trimmedText = selectedEventItem.Name.Substring(0, 10);
                    trimmedText += "..";
                }
                else
                {
                    trimmedText = selectedEventItem.Name;
                }

                spanString.Spans.Add(new Span { Text = trimmedText + ".", ForegroundColor = Constants.BLUE_BG_COLOR, FontAttributes = FontAttributes.Italic, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 20 });
            }
            else
            {
                spanString.Spans.Add(new Span { Text = "............", ForegroundColor = Color.Black, FontAttributes = FontAttributes.Italic, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 16 });
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

                SetFeedBackLablText();
                string trimmedText = item.Name;
                if (trimmedText.Length > 15)
                {
                    trimmedText = trimmedText.Substring(0, 15);
                    trimmedText += "..";
                }
                eventTextLabel.Text = trimmedText;
                circleImg3.Source = "finger";// "icn_selected";
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
                    var downloadEventsStatus = await DownloadAllEvents();
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
            this.eventPickerButton = null;
            this.eventPickerButton.Clicked -= OnEventPickerButtonClicked;
            this.emotionalPickerButton = null;
            this.emotionalPickerButton.Clicked -= OnEmotionalPickerButtonClicked;
            this.slider = null;
            this.masterLayout = null;
            this.progressBar = null;
            this.selectedEmotionItem = null;
            this.selectedEventItem = null;
            this.about = null;
            this.subTitleBar.NextButtonTapRecognizer.Tapped -= OnNextButtonTapRecognizerTapped;
            this.subTitleBar.BackButtonTapRecognizer.Tapped -= OnBackButtonTapRecognizerTapped;
            this.subTitleBar = null;
            this.Appearing -= OnFeelingNowPageAppearing;
            this.mainTitleBar = null;

            GC.Collect();
        }
    }
}
