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
using PurposeColor.Model;

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
		public static int sliderValue = 0;
        double screenHeight;
        double screenWidth;
        IProgressBar progressBar = null;
        PurposeColorSubTitleBar subTitleBar = null;
        PurposeColorTitleBar mainTitleBar = null;

        Label sliderValLabel;
		Label emotionTextLabel = null;
		Label eventTextLabel = null;

        StackLayout imagesContainer = null;
        bool emotionsDisplaying = false;
        bool eventsDisplaying = false;
		StackLayout feedbackLabelStack = null;
		StackLayout sliderFeedbackStack = null;
		StackLayout feelingFeedbackStack = null;
		StackLayout eventFeedbackStack = null;
		BoxView hLine = null;
		TapGestureRecognizer emotionTextTap = null;
		TapGestureRecognizer eventTextTap = null;
		Image sliderValueImage = null;


		Image topCloseBtn = null;
		StackLayout topBgandCloseBtn = null;
		StackLayout topLabelBg = null;
		CustomLayout topLabelsContainer = null;

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

			#region  Emotion pic button
			emotionalPickerButton = new PurposeColor.interfaces.CustomImageButton ();
			emotionalPickerButton.ImageName = Device.OnPlatform ("select_box_whitebg.png", "select_box_whitebg.png", @"/Assets/select_box_whitebg.png");
			emotionalPickerButton.Text = "Select Emotion";

			emotionalPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
			emotionalPickerButton.TextOrientation = interfaces.TextOrientation.Left;
			emotionalPickerButton.TextColor = Color.Gray;
			emotionalPickerButton.WidthRequest = screenWidth * 90 / 100;
			emotionalPickerButton.Clicked += OnEmotionalPickerButtonClicked;

			eventPickerButton = new PurposeColor.interfaces.CustomImageButton ();
			eventPickerButton.IsVisible = false;
			eventPickerButton.ImageName = Device.OnPlatform ("select_box_whitebg.png", "select_box_whitebg.png", "/Assets/select_box_whitebg.png");
			eventPickerButton.Text = "Events, Situation & Thoughts";
			eventPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
			eventPickerButton.TextOrientation = interfaces.TextOrientation.Left;
			eventPickerButton.TextColor = Color.Gray;
			eventPickerButton.WidthRequest = screenWidth * 90 / 100;


            if (!eventsDisplaying)
            {
                eventPickerButton.Clicked += OnEventPickerButtonClicked;
            }
			#endregion

			#region About text
			about = new Label ();
			about.IsVisible = false;
			about.Text = "About";
			about.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
			about.WidthRequest = screenWidth;
			about.HorizontalOptions = LayoutOptions.Center;
			about.XAlign = TextAlignment.Center;

			about.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
			about.TextColor = Color.FromRgb (40, 47, 50);

			int fontSize = 15;
			if (App.screenDensity > 1.5) {
				howYouAreFeeling.FontSize = Device.OnPlatform (20, 22, 30);
				howYouAreFeeling2.FontSize = Device.OnPlatform (20, 22, 30);
				about.FontSize = Device.OnPlatform (15, 18, 30);

				emotionalPickerButton.HeightRequest = screenHeight * 6 / 100;
				fontSize = 17;
				eventPickerButton.HeightRequest = screenHeight * 6 / 100;
			} else {
				howYouAreFeeling.FontSize = Device.OnPlatform (16, 18, 26);
				howYouAreFeeling2.FontSize = Device.OnPlatform (16, 18, 26);
				about.FontSize = Device.OnPlatform (16, 18, 26);

				emotionalPickerButton.HeightRequest = screenHeight * 9 / 100;
				fontSize = 15;
				eventPickerButton.HeightRequest = screenHeight * 9 / 100;
			}

			emotionalPickerButton.FontSize = Device.OnPlatform (fontSize, fontSize, 22);
			eventPickerButton.FontSize = Device.OnPlatform (fontSize, fontSize, 22);
			#endregion

            this.Appearing += OnFeelingNowPageAppearing;

            sliderValue = slider.CurrentValue;
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            
            sliderValLabel = new Label
            {
				TextColor = Constants.BLUE_BG_COLOR,
				BackgroundColor = Color.Transparent,
				XAlign = TextAlignment.Start
            };

			sliderValueImage = new Image {
				Source = "Sliderfeedback0.png",
				HeightRequest = 30,
				Aspect = Aspect.Fill
			};

            #region SLIDER LABEL TAP

            TapGestureRecognizer sliderLabelTapRecognizer = new TapGestureRecognizer();
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
					CurrentValue = sliderValue
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
				TextColor = Constants.BLUE_BG_COLOR,
				BackgroundColor = Color.Transparent,
				XAlign = TextAlignment.Start,
				FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
				FontSize = Device.OnPlatform(12, 14, 26)
            };
            emotionTextTap = new TapGestureRecognizer();
			emotionTextTap.Tapped += EmotionTextTap_Tapped;

            eventTextLabel = new Label
            {
				TextColor = Constants.BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
				XAlign = TextAlignment.Start,
				FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
				FontSize = Device.OnPlatform(12, 14, 26)
            };

            eventTextTap = new TapGestureRecognizer();
            eventTextTap.Tapped += async (s, e) =>
            {
                if (!eventsDisplaying)
                {
                    await Task.Delay(100);
                    OnEventPickerButtonClicked(eventPickerButton, EventArgs.Empty);
                }
            };
			//eventTextLabel.GestureRecognizers.Add(eventTextTap);

			sliderFeedbackStack = new StackLayout {
				IsVisible = false,
				BackgroundColor = Color.Transparent,
				VerticalOptions = LayoutOptions.Center,
				Orientation = StackOrientation.Horizontal, 
				Spacing = 0, Padding = new Thickness (App.screenWidth * .10, 0, 10, 0), 
				Children = {
					new Label {
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						FontSize = Device.OnPlatform(12, 14, 26),
						Text = "Happiness : ",
						TextColor = Color.Black,
						VerticalOptions = LayoutOptions.End,
					},
					sliderValueImage
				}
			};
			sliderFeedbackStack.GestureRecognizers.Add(sliderLabelTapRecognizer);


			feelingFeedbackStack = new StackLayout {
				IsVisible = false,
				Orientation = StackOrientation.Horizontal,
				Spacing = 0,
				Padding = new Thickness (App.screenWidth * .10, 0, 10, 0),
				Children = {
					new Label {
						Text = "Feeling : " ,
						TextColor = Color.Black,
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						FontSize = Device.OnPlatform(12, 14, 26),
					},
					emotionTextLabel
				}
			};
			feelingFeedbackStack.GestureRecognizers.Add(emotionTextTap);

			eventFeedbackStack = new StackLayout {
				IsVisible = false,
				//BackgroundColor = Color.Red,
				Orientation = StackOrientation.Horizontal,
				Spacing = 0,
				Padding = new Thickness (App.screenWidth * .10, 0, 10, 0),
				Children = {
					new Label {
						Text = "Event : " ,
						TextColor = Color.Black,
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						FontSize = Device.OnPlatform(12, 14, 26),
					},
					eventTextLabel
				}
			};
			eventFeedbackStack.GestureRecognizers.Add(eventTextTap);

			// add this to a customLayout n a stack with bg transparent.
			topLabelsContainer = new CustomLayout {
				WidthRequest = screenWidth,
			};

			topLabelBg = new StackLayout {
				WidthRequest = screenWidth,
				BackgroundColor = Color.Gray,
				Opacity = .2
			};

			topBgandCloseBtn = new StackLayout {
				//BackgroundColor  = Color.Yellow, // for testing only
				Orientation = StackOrientation.Vertical,
				Spacing = 0,
				Children = {topLabelBg}
			};

			topCloseBtn = new Image {
				Source = "downarrow.png",
				IsVisible = false,
				HeightRequest = 25,
				WidthRequest = 35,
				HorizontalOptions = LayoutOptions.End
			};
			topBgandCloseBtn.Children.Add (topCloseBtn);

			TapGestureRecognizer topCloseBtnTapRecognizer = new TapGestureRecognizer();
			topCloseBtnTapRecognizer.Tapped += (s, e) =>
			{
				AnimateToplabels(0);
			};

			topCloseBtn.GestureRecognizers.Add (topCloseBtnTapRecognizer);

			feedbackLabelStack = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				//BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Spacing = 2,
				WidthRequest = App.screenWidth,
				Children = {sliderFeedbackStack, feelingFeedbackStack, eventFeedbackStack}
			};
			topLabelsContainer.AddChildToLayout (topBgandCloseBtn, 0, 0);
			topLabelsContainer.AddChildToLayout (feedbackLabelStack, 0, 0);


			//masterLayout.AddChildToLayout(feedbackLabelStack, 0, Device.OnPlatform(16, 18, 10));
			masterLayout.AddChildToLayout(topLabelsContainer, 0, Device.OnPlatform(16, 18, 10));

            masterLayout.AddChildToLayout(howYouAreFeeling, 16, Device.OnPlatform(33, 33, 30));
            masterLayout.AddChildToLayout(howYouAreFeeling2, 29, Device.OnPlatform(38, 38, 27));
            masterLayout.AddChildToLayout(slider, 5, 43);

            masterLayout.AddChildToLayout(emotionalPickerButton, 5, Device.OnPlatform(50, 57, 47));
            masterLayout.AddChildToLayout(about, 0, Device.OnPlatform(62, 66, 59));
            masterLayout.AddChildToLayout(eventPickerButton, 5, Device.OnPlatform(70, 73, 67));
            //SetFeedBackLablText();
            Content = masterLayout;

        }

		async void EmotionTextTap_Tapped (object sender, EventArgs e)
        {
			await Task.Delay(100);
			OnEmotionalPickerButtonClicked(emotionalPickerButton, EventArgs.Empty);
        }

        private void RemoveSliderPopup()
        {
            try
            {
				if (popupSlider!= null) {
					sliderValue = popupSlider.CurrentValue;
					popupSlider = null;
				}
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
			if (popupSlider != null )
            {
                sliderValue = popupSlider.CurrentValue;
				slider.Value = sliderValue;
				slider.CurrentValue = sliderValue;

				if (sliderValue != 0) {
					popupSlider = null;
					RemoveSliderPopup();
				}
            }
			else
            {
				sliderValue = slider.CurrentValue;
            }

			switch (sliderValue) 
			{
			case 2:
				sliderValueImage.Source = "Sliderfeedback2.png";
				break;
			case 1:
				sliderValueImage.Source = "Sliderfeedback1.png";
				break;
			case -1:
				sliderValueImage.Source = "SliderfeedbackMinues1.png";
				break;
			case -2:
				sliderValueImage.Source = "SliderfeedbackMinues2.png";
				break;
			case 0:
				sliderValueImage.Source = "Sliderfeedback0.png";
				break;
			default:
				break;
			};

			sliderFeedbackStack.IsVisible = true;
			sliderFeedbackStack.HeightRequest = 0;

			if (sliderValue == 0) {
				progressBar.ShowToast ("slider is in neutral");
			}
			else if (!emotionsDisplaying && sliderValue != 0)
            {
				AnimateToplabels(1);

                await Task.Delay(100);
                OnEmotionalPickerButtonClicked(emotionalPickerButton, EventArgs.Empty);

            }
        }

        protected override bool OnBackButtonPressed()
        {
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            if (pickView != null)
            {
                masterLayout.Children.Remove(pickView);
                pickView = null;
				GC.Collect();
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

				User user = App.Settings.GetUser();
				if( user == null )
					user = new User(){ UserId = 2 };
				
                progressBar.ShowProgressbar("Saving details..");
				bool isDataSaved = await ServiceHelper.SaveEmotionAndEvent(selectedEmotionItem.EmotionID, selectedEventItem.EventID,  user.UserId.ToString());
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
			if (slider.CurrentValue == 0) {
				progressBar.ShowToast("Slider is in neutral.");

				return;
			}

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
				if (item == null) {
					return;
				}
				if (string.IsNullOrEmpty(item.Name)) {
					return;
				}
                emotionalPickerButton.Text = item.Name;
                selectedEmotionItem = item;
                emotionalPickerButton.TextColor = Color.Black;
                App.SelectedEmotion = item.Name;
                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                masterLayout.Children.Remove(pickView);
                pickView = null;
				GC.Collect();
                eventPickerButton.IsVisible = true;
                about.IsVisible = true;
                if (! eventsDisplaying)
                {
                    await Task.Delay(100);
                    OnEventPickerButtonClicked(eventPickerButton, EventArgs.Empty);
                    //SetFeedBackLablText();
                }

                string trimmedText = item.Name;
                if (trimmedText.Length > 15)
                {
                    trimmedText = trimmedText.Substring(0, 15);
                    trimmedText += "..";
                }

				emotionTextLabel.Text = item.Name;
				feelingFeedbackStack.IsVisible = true;
				feelingFeedbackStack.HeightRequest = 0;
				AnimateToplabels(2);

            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Please try again", Constants.ALERT_OK);
            }
        }

        private void SetFeedBackLablText()
        {
//
//            var spanString = new FormattedString();
//            string trimmedText;
//            spanString.Spans.Add(new Span { Text = "Feeling ", ForegroundColor = Color.Black, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 16 });
//            if (selectedEmotionItem != null)
//            {
//                if (selectedEmotionItem.Name != null && selectedEmotionItem.Name.Length > 10)
//                {
//                    trimmedText = selectedEmotionItem.Name.Substring(0, 10);
//                    trimmedText += "..";
//                }
//                else
//                {
//                    trimmedText = selectedEmotionItem.Name;
//                }
//                spanString.Spans.Add(new Span { Text = trimmedText, ForegroundColor = Constants.BLUE_BG_COLOR, FontAttributes = FontAttributes.Italic, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 20 });
//            }
//            else
//            {
//                spanString.Spans.Add(new Span { Text = "............", ForegroundColor = Color.Black, FontAttributes = FontAttributes.Italic, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 16 });
//            }
//            spanString.Spans.Add(new Span { Text = " about ", ForegroundColor = Color.Black, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 16 });
//
//            if (selectedEventItem != null)
//            {
//                if (selectedEventItem.Name != null && selectedEventItem.Name.Length > 10)
//                {
//                    trimmedText = selectedEventItem.Name.Substring(0, 10);
//                    trimmedText += "..";
//                }
//                else
//                {
//                    trimmedText = selectedEventItem.Name;
//                }
//
//                spanString.Spans.Add(new Span { Text = trimmedText + ".", ForegroundColor = Constants.BLUE_BG_COLOR, FontAttributes = FontAttributes.Italic, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 20 });
//            }
//            else
//            {
//                spanString.Spans.Add(new Span { Text = "............", ForegroundColor = Color.Black, FontAttributes = FontAttributes.Italic, FontFamily = Constants.HELVERTICA_NEUE_LT_STD, FontSize = 16 });
//            }
        }

        void OnEventPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                CustomListViewItem item = e.SelectedItem as CustomListViewItem;
				if (item == null) {
					return;
				}
				if (string.IsNullOrEmpty(item.Name)) {
					return;
				}

                eventPickerButton.Text = item.Name;
                eventPickerButton.TextColor = Color.Black;
                selectedEventItem = item;
                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                masterLayout.Children.Remove(pickView);
                pickView = null;
				GC.Collect();
                SetFeedBackLablText();
                string trimmedText = item.Name;
                if (trimmedText.Length > 15)
                {
                    trimmedText = trimmedText.Substring(0, 15);
                    trimmedText += "..";
                }
				eventFeedbackStack.IsVisible = true;
				eventFeedbackStack.HeightRequest = 0;
				eventTextLabel.Text = item.Name;

				AnimateToplabels(3);
            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Please try again", Constants.ALERT_OK);
            }
        }

		void AnimateToplabels (int LabelIndex)
		{
			if (LabelIndex == 0) {
				if (topCloseBtn.Rotation != 0) 
				{
					sliderFeedbackStack.IsVisible = false;
					feelingFeedbackStack.IsVisible = false;
					eventFeedbackStack.IsVisible = false;

					var animate = new Animation (d => topLabelBg.HeightRequest = d, topLabelBg.Height, 0, Easing.SpringIn);
					animate.Commit (topLabelBg, "BarGraph", 16, 200);
					animate = null;

					topCloseBtn.Rotation = 0;

					return;
				} else {
				
					AnimateToplabels (1);
					AnimateToplabels (2);
					AnimateToplabels (3);
				return;
				}
			}

			int heightReq = 0;
			heightReq += string.IsNullOrEmpty (emotionTextLabel.Text) ? 0 : (int)emotionTextLabel.Height;
			heightReq += string.IsNullOrEmpty (eventTextLabel.Text) ? 0 : 25;
			heightReq += slider.CurrentValue == 0 ? 0 : (int)sliderValueImage.Height;

			topLabelsContainer.HeightRequest = heightReq + topCloseBtn.Height;

			if (heightReq > 20) {
				topCloseBtn.Rotation = 180;
				topCloseBtn.IsVisible = true;
			} else {
				topCloseBtn.Rotation = 0;
				topCloseBtn.IsVisible = false;
			}
			//topLabelBg.HeightRequest = heightReq;

			{
				var animate = new Animation(d => topLabelBg.HeightRequest = d,topLabelBg.Height, heightReq + 3, Easing.SpringIn);
				animate.Commit(topLabelBg, "BarGraph", 16, 500);
				animate = null;
			}

			var sliderFeedbackStackH = sliderFeedbackStack.Height;


			if (slider.CurrentValue != 0 && LabelIndex == 1) {
				sliderFeedbackStack.IsVisible = true;
				var animate = new Animation(d => sliderFeedbackStack.HeightRequest = d, 0, sliderValueImage.Height, Easing.SpringIn);
				animate.Commit(sliderFeedbackStack, "BarGraph", 16, 500);
				animate = null;
			}

			if (!string.IsNullOrEmpty (emotionTextLabel.Text)  && LabelIndex == 2) {
				feelingFeedbackStack.IsVisible = true;
				var animate = new Animation(d => feelingFeedbackStack .HeightRequest = d, 0, emotionTextLabel.Height  , Easing.SpringIn);
				animate.Commit(feelingFeedbackStack , "BarGraph", 16, 500);
				animate = null;
			}

			if (!string.IsNullOrEmpty (eventTextLabel.Text)  && LabelIndex == 3) {
				eventFeedbackStack.IsVisible = true;
				var animate = new Animation(d => eventFeedbackStack.HeightRequest = d, 0,23, Easing.SpringIn);
				animate.Commit(eventFeedbackStack, "BarGraph", 16, 500);
				animate = null;
			}

			GC.Collect ();
		}

        void emotionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        async void OnFeelingNowPageAppearing(object sender, System.EventArgs e)
        {
            try
            {
                base.OnAppearing();

				var tokenResp =  await ServiceHelper.SendNotificationToken (App.NotificationToken);
				if(!tokenResp  )
				{
					DisplayAlert( Constants.ALERT_TITLE, "Token updation failed.", Constants.ALERT_OK );
				}

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
			sliderValLabel = null;
			emotionTextLabel = null;
			eventTextLabel = null;
			imagesContainer = null;
			feedbackLabelStack = null;
			sliderFeedbackStack = null;
			feelingFeedbackStack = null;
			eventFeedbackStack = null;
			hLine = null;
			emotionTextTap = null;

            GC.Collect();
        }
    }
}
