﻿
using Cross;
using CustomControls;
using Media.Plugin;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using PurposeColor.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using System.Linq;
using Geolocator.Plugin;
using PurposeColor.interfaces;
using System.Net.Http;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using Media.Plugin.Abstractions;

namespace PurposeColor.screens
{
    public class AddEventsSituationsOrThoughts : ContentPage, System.IDisposable
    {
        #region MEMBERS

        public static CustomLayout masterLayout;
        StackLayout TopTitleBar;
        PurposeColorBlueSubTitleBar subTitleBar;
        public static CustomEditor eventDescription;
        public static CustomEntry eventTitle;
        StackLayout textInputContainer;
        StackLayout audioInputStack;
        Image cameraInput;
        Image audioInput;
        StackLayout cameraInputStack;
        Image galleryInput;
        StackLayout galleryInputStack;
        Image locationInput;
        StackLayout locationInputStack;
        Image contactInput;
        StackLayout contactInputStack;
        StackLayout textinputAndIconsHolder;
        TapGestureRecognizer CameraTapRecognizer;
        string pageTitle;
        bool isAudioRecording = false;
        PurposeColor.interfaces.IAudioRecorder audioRecorder;
        bool isUpdatePage = false;

        //IDeviceSpec deviceSpec;
        string lattitude;
        string longitude;
        string currentAddress;
        string selectedContact;
        public static StackLayout listContainer;
        public static ListView previewListView;
        Label startDateLabel;
        Label endDateLabel;
        double screenHeight;
        double screenWidth;
        Grid iconContainerGrid = null;
        StackLayout audioRecodeOnHolder = null;
        StackLayout audioRecodeOffHolder = null;
        Label locationInfo;
        Entry locAndContactsEntry;
        StackLayout editLocationAndContactsStack;
        CustomImageButton editLocationDoneButton;
		Label contactInfo;
		StackLayout locLayout;
        CustomPicker ePicker;
        Image audioRecodeOffButton = null;
        int Seconds = 0;
        string currentGemId;
        GemType currentGemType;
		public static Action<string> contactSelectAction; 
		public static FeelingNowPage feelingsPage{ get; set; }
		public static FeelingsSecondPage feelingSecondPage{ get; set; }
        #endregion

        public AddEventsSituationsOrThoughts(string title, DetailsPageModel detailsPageModel = null)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            audioRecorder = DependencyService.Get<PurposeColor.interfaces.IAudioRecorder>();
            //deviceSpec = DependencyService.Get<IDeviceSpec>();
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;

            masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            pageTitle = title;
            lattitude = string.Empty;
            longitude = string.Empty;
            currentAddress = string.Empty;
            int devWidth = (int)screenWidth;
            App.MediaArray = new List<MediaItem>();
            App.ContactsArray = new List<string>();
            App.PreviewListSource.Clear();
            int textInputWidth = (int)(devWidth * .80);
			contactSelectAction = OnContactSelected;

            #region TITLE BARS
            TopTitleBar = new StackLayout
            {
                BackgroundColor = Constants.BLUE_BG_COLOR,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = 0,
                Spacing = 0,
                Children = { new BoxView { WidthRequest = screenWidth } }
            };
            masterLayout.AddChildToLayout(TopTitleBar, 0, 0);
            
            string trimmedPageTitle = string.Empty;

            int titleMaxLength = 24;
            if (App.screenDensity > 1.5)
            {
                titleMaxLength = 24;
            }
            else
            {
                titleMaxLength = 22;
            }

            if (title.Length > titleMaxLength)
            {
                trimmedPageTitle = title.Substring(0, titleMaxLength);
                trimmedPageTitle += "..";
            }
            else
            {
                trimmedPageTitle = pageTitle;
            }

            subTitleBar = new PurposeColorBlueSubTitleBar(Constants.SUB_TITLE_BG_COLOR, trimmedPageTitle, true, true);
            masterLayout.AddChildToLayout(subTitleBar, 0, 1);
            subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;
            subTitleBar.NextButtonTapRecognizer.Tapped += NextButtonTapRecognizer_Tapped;
            #endregion

            #region EVENT TITLE - CUSTOM ENTRY

            eventTitle = new CustomEntry
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.White,
                Placeholder = "Title",
                TextColor = Color.FromHex("#424646"),
                HeightRequest = Device.OnPlatform(50,50,73),
                WidthRequest = (int)(devWidth * .90) // 90% of screen,
            };
			eventTitle.TextChanged += EventTitle_TextChanged;

            
            //if (App.screenDensity > 1.5)
            //{
            //    eventTitle.HeightRequest = screenHeight * 6 / 100;
            //}
            //else
            //{
            //    eventTitle.HeightRequest = screenHeight * 9 / 100;
            //}
            masterLayout.AddChildToLayout(eventTitle, 5, 11);

            #endregion

            #region EVENT DESCRIPTION

            eventDescription = new CustomEditor
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 100,
                Placeholder = pageTitle,
                BackgroundColor = Color.White
            };
			eventDescription.TextChanged += EventDescription_TextChanged;
				

            eventDescription.WidthRequest = textInputWidth;

            if (detailsPageModel != null)
            {
				if (detailsPageModel.IsCopyingGem) {
					isUpdatePage = false;
				}
				else
				{
					isUpdatePage = true;
				}

                currentGemId = detailsPageModel.gemId;
                if (detailsPageModel.gemType != null)
                {
                    currentGemType = detailsPageModel.gemType;
                    switch (currentGemType)
                    {
                        case GemType.Goal:
                            pageTitle = Constants.EDIT_GOALS;
                            break;
                        case GemType.Event:
                            pageTitle = Constants.EDIT_EVENTS;
                            break;
                        case GemType.Action:
                            pageTitle = Constants.EDIT_ACTIONS;
                            break;
                        default:
                            break;
                    }
                }
                
                if (detailsPageModel.titleVal != null)
                {
                    eventTitle.Text = detailsPageModel.titleVal;
                }
                if ( detailsPageModel.description != null)
	            {
		             eventDescription.Text = detailsPageModel.description;
	            }
            }

            #endregion

            #region MEDIA INPUTS

            Image pinButton = new Image
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Source = Device.OnPlatform("icn_attach.png", "icn_attach.png", "//Assets//icn_attach.png"),

            };
            StackLayout pinButtonHolder = new StackLayout
            {
                Padding = 10,
                VerticalOptions = LayoutOptions.Start,
                Children = { pinButton }
            };
            TapGestureRecognizer pinButtonTapRecognizer = new TapGestureRecognizer();
            pinButtonHolder.GestureRecognizers.Add(pinButtonTapRecognizer);
            pinButtonTapRecognizer.Tapped += (s, e) =>
            {
                iconContainerGrid.IsVisible = !iconContainerGrid.IsVisible;
            };

            Image audioRecodeOnButton = new Image
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center,
                Source = Device.OnPlatform("mic.png", "mic.png", "//Assets//mic.png"),
            };
            audioRecodeOnHolder = new StackLayout
            {
                Padding = 10,
                VerticalOptions = LayoutOptions.End,
                Children = { audioRecodeOnButton }
            };
            TapGestureRecognizer RecodeOnTapRecognizer = new TapGestureRecognizer();
            audioRecodeOnHolder.GestureRecognizers.Add(RecodeOnTapRecognizer);

            audioRecodeOffButton = new Image
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Center,
                Source = Device.OnPlatform("turn_off_mic.png", "turn_off_mic.png", "//Assets//turn_off_mic.png"),
            };
            audioRecodeOffHolder = new StackLayout
            {
                BackgroundColor = Color.Transparent,
                Padding = 10,
                VerticalOptions = LayoutOptions.End,
                IsVisible = false,
                Children = { audioRecodeOffButton }
            };
            TapGestureRecognizer RecodeOffTapRecognizer = new TapGestureRecognizer();
            audioRecodeOffHolder.GestureRecognizers.Add(RecodeOffTapRecognizer);

            audioRecodeOffHolder.TranslateTo(0, Device.OnPlatform(audioRecodeOffButton.Y + 60, audioRecodeOffButton.Y + 60, audioRecodeOffButton.Y + 50), 5, null);
            audioRecodeOnHolder.TranslateTo(0, Device.OnPlatform(audioRecodeOffButton.Y + 60, audioRecodeOffButton.Y + 60, audioRecodeOffButton.Y + 50), 5, null);

            RecodeOnTapRecognizer.Tapped += RecodeOnTapRecognizer_Tapped;
            RecodeOffTapRecognizer.Tapped += RecodeOffTapRecognizer_Tapped;

            StackLayout menuPinContainer = new StackLayout
            {
                BackgroundColor = Color.White,
                Orientation = StackOrientation.Vertical,
                HeightRequest = 140,
                WidthRequest = (int)(devWidth * .10),
                Children = {
                    pinButtonHolder, 
                    audioRecodeOnHolder, 
                    audioRecodeOffHolder
                }
            };


            TapGestureRecognizer locationlabelTap = new TapGestureRecognizer();

            locationInfo = new Label();
            locationInfo.TextColor = Constants.BLUE_BG_COLOR;
            locationInfo.BackgroundColor = Color.Transparent;
            locationInfo.FontSize = 12;
            locationInfo.HeightRequest = Device.OnPlatform(15, 25, 25);
            locationInfo.GestureRecognizers.Add(locationlabelTap);
            locationlabelTap.Tapped += OnEditLocationInfo;

            editLocationAndContactsStack = new StackLayout();
            editLocationAndContactsStack.Padding = new Thickness(1, 1, 1, 1);
            editLocationAndContactsStack.BackgroundColor = Color.FromRgb(30, 126, 210);
            editLocationAndContactsStack.WidthRequest = App.screenWidth * 90 / 100;
            editLocationAndContactsStack.IsVisible = false;
            editLocationAndContactsStack.Orientation = StackOrientation.Horizontal;


            locAndContactsEntry = new Entry();
            locAndContactsEntry.TextColor = Color.Black;
            locAndContactsEntry.BackgroundColor = Color.White;
            locAndContactsEntry.VerticalOptions = LayoutOptions.Center;
            locAndContactsEntry.WidthRequest = App.screenWidth * 80 / 100;
            locAndContactsEntry.HeightRequest = 50;

            editLocationDoneButton = new CustomImageButton();
            editLocationDoneButton.VerticalOptions = LayoutOptions.Center;
            editLocationDoneButton.ImageName = "icn_done.png";
            editLocationDoneButton.HeightRequest = 25;
            editLocationDoneButton.WidthRequest = 25;
            editLocationDoneButton.Clicked += OnLocationEditCompleted;


            editLocationAndContactsStack.Children.Add(locAndContactsEntry);
            editLocationAndContactsStack.Children.Add(editLocationDoneButton);

            if (Device.OS == TargetPlatform.iOS)
            {
                editLocationAndContactsStack.TranslationY = -30;
            }

            locLayout = new StackLayout();
            locLayout.Orientation = StackOrientation.Vertical;
            locLayout.BackgroundColor = Color.Transparent;



            locLayout.Children.Add(locationInfo);

            TapGestureRecognizer contactsLabelTap = new TapGestureRecognizer();
            contactInfo = new Label();
            contactInfo.TextColor = Constants.BLUE_BG_COLOR;
            contactInfo.BackgroundColor = Color.Transparent;
            contactInfo.FontSize = 12;
            contactInfo.HeightRequest = Device.OnPlatform(15, 25, 25);
            contactInfo.GestureRecognizers.Add(contactsLabelTap);
            contactsLabelTap.Tapped += async (object sender, EventArgs e) =>
            {
                editLocationAndContactsStack.ClassId = "contactedit";
                string spanContacts = "";
                if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
                    spanContacts = contactInfo.FormattedText.Spans[1].Text;
                locAndContactsEntry.Text = spanContacts;
                editLocationAndContactsStack.IsVisible = true;
                contactInfo.IsVisible = false;
                iconContainerGrid.IsVisible = false;
                locationInfo.IsVisible = true;

                await editLocationAndContactsStack.TranslateTo(100, 0, 300, Easing.SinInOut);
                await editLocationAndContactsStack.TranslateTo(0, 0, 300, Easing.SinIn);

            };

            locLayout.IsVisible = false;
            contactInfo.IsVisible = false;
            
            #endregion

            if(detailsPageModel != null)
            {
                if (detailsPageModel.eventMediaArray!= null && detailsPageModel.eventMediaArray.Count > 0)
                {
                    foreach (EventMedia eventObj in detailsPageModel.eventMediaArray)
                    {
						if (eventObj.event_media != null && !eventObj.event_media.Contains("default")) {
							AddFilenameToMediaList(eventObj.event_media);
						}
                    }
                }

                if (detailsPageModel.goal_media != null && detailsPageModel.goal_media.Count > 0)
                {
                    foreach (SelectedGoalMedia goalObj in detailsPageModel.goal_media)
                    {
						if (goalObj.goal_media != null && !goalObj.goal_media.Contains("default")) {
							AddFilenameToMediaList(goalObj.goal_media);
						}
                    }
                }

                if (detailsPageModel.actionMediaArray != null && detailsPageModel.actionMediaArray.Count > 0)
                {
                    foreach (ActionMedia actionObj in detailsPageModel.actionMediaArray)
                    {
						if (actionObj.action_media != null && !actionObj.action_media.Contains("default")) {
							AddFilenameToMediaList(actionObj.action_media);
						}
                    }
                }

            }



            StackLayout entryAndLocContainer = new StackLayout();
            entryAndLocContainer.Orientation = StackOrientation.Vertical;
            entryAndLocContainer.BackgroundColor = Color.White;
            entryAndLocContainer.Children.Add( eventDescription );
			entryAndLocContainer.Children.Add(contactInfo);
            entryAndLocContainer.Children.Add(locLayout);

            textInputContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                Padding = 0,
                Children = { entryAndLocContainer, menuPinContainer }
            };

            #region ICONS

            audioInput = new Image()
            {
                Source = Device.OnPlatform("ic_music.png", "ic_music.png", "//Assets//ic_music.png"),
                Aspect = Aspect.AspectFit
            };

            int ICON_SIZE = 8;

            if( Device.OS == TargetPlatform.WinPhone )
            {
                audioInput.WidthRequest = screenWidth * ICON_SIZE / 100;
                audioInput.HeightRequest = screenWidth * ICON_SIZE / 100;
            }
            audioInputStack = new StackLayout
            {
                Padding = new Thickness(5, 10, 5, 10),
                //BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                //HorizontalOptions = LayoutOptions.Center,
                Spacing = 0,
                Children = { audioInput 
                                /*, new Label { Text = "Audio", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) }*/ 
                           }
            };

            cameraInput = new Image()
            {
                Source = Device.OnPlatform("icn_camera.png", "icn_camera.png", "//Assets//icn_camera.png"),
                Aspect = Aspect.AspectFit
            };



            if (Device.OS == TargetPlatform.WinPhone)
            {
                cameraInput.WidthRequest = screenWidth * ICON_SIZE / 100;
                cameraInput.HeightRequest = screenWidth * ICON_SIZE / 100;
            }


            cameraInputStack = new StackLayout
            {
                Padding = new Thickness(5, 10, 5, 10),
                //BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                //HorizontalOptions = LayoutOptions.Center,
                Spacing = 0,
                Children = { cameraInput
                                /*, new Label { Text = "Camera", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } 
                                 */
                            }
            };
            cameraInputStack.ClassId = "camera";

            #region CAMERA TAP RECOGNIZER
            CameraTapRecognizer = new TapGestureRecognizer();
            cameraInputStack.GestureRecognizers.Add(CameraTapRecognizer);
            CameraTapRecognizer.Tapped += async (s, e) =>
            {
                try
                {
					await ApplyAnimation( cameraInputStack );
                    StackLayout send = s as StackLayout;
                    MediaSourceChooser chooser = new MediaSourceChooser(this, masterLayout, send.ClassId);
                    chooser.ClassId = "mediachooser";
                    masterLayout.AddChildToLayout(chooser, 0, 0);
                   
                }
                catch (System.Exception ex)
                {
                    DisplayAlert("Camera", ex.Message + " Please try again later", "ok");
                }

                /*
                try
                {
                    StackLayout send = s as StackLayout;
                    MediaSourceChooser chooser = new MediaSourceChooser(this, masterLayout, send.ClassId);
                    chooser.ClassId = "mediachooser";
                    masterLayout.AddChildToLayout(chooser, 0, 0);
                }
                catch (System.Exception ex)
                {
                    DisplayAlert("Camera", ex.Message + " Please try again later", "ok");
                }
                */
            };

            #endregion

            galleryInput = new Image()
            {
                Source = Device.OnPlatform("icn_gallery.png", "icn_gallery.png", "//Assets//icn_gallery.png"),
                Aspect = Aspect.AspectFit
            };

            if (Device.OS == TargetPlatform.WinPhone)
            {
                galleryInput.WidthRequest = screenWidth * ICON_SIZE / 100;
                galleryInput.HeightRequest = screenWidth * ICON_SIZE / 100;
            }

            galleryInputStack = new StackLayout
            {
                Padding = new Thickness(5, 10, 5, 10),
                Spacing = 0,
                Children = { galleryInput 
                                //new Label { Text = "Gallery", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } 
                            }
            };
            galleryInputStack.ClassId = "gallery";

            #region GALLERY  TAP RECOGNIZER

            TapGestureRecognizer galleryInputStackTapRecognizer = new TapGestureRecognizer();
            galleryInputStack.GestureRecognizers.Add(galleryInputStackTapRecognizer);
            galleryInputStackTapRecognizer.Tapped += async (s, e) =>
            {
				await ApplyAnimation( galleryInputStack );
                StackLayout send = s as StackLayout;
                MediaSourceChooser chooser = new MediaSourceChooser(this, masterLayout, send.ClassId);
                chooser.ClassId = "mediachooser";
                masterLayout.AddChildToLayout(chooser, 0, 0);

            };

            #endregion

            locationInput = new Image()
            {
                Source = Device.OnPlatform("icn_location.png", "icn_location.png", "//Assets//icn_location.png"),
                Aspect = Aspect.AspectFit
            };

            if (Device.OS == TargetPlatform.WinPhone)
            {
                locationInput.WidthRequest = screenWidth * ICON_SIZE / 100;
                locationInput.HeightRequest = screenWidth * ICON_SIZE / 100;
            }


            locationInputStack = new StackLayout
            {
                Padding = new Thickness(5, 10, 5, 10),
                //BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                //HorizontalOptions = LayoutOptions.Center,
                Spacing = 0,
                Children = { locationInput
                                //, new Label { Text = "Location", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } 
                            }
            };

            #region LOCATION TAP RECOGNIZER

            TapGestureRecognizer locationInputTapRecognizer = new TapGestureRecognizer();
            locationInputStack.GestureRecognizers.Add(locationInputTapRecognizer);
            locationInputTapRecognizer.Tapped += LocationInputTapRecognizer_Tapped;

            #endregion

            contactInput = new Image()
            {
                Source = Device.OnPlatform("icn_contact.png", "icn_contact.png", "//Assets//icn_contact.png"),
                Aspect = Aspect.AspectFit
            };

            if (Device.OS == TargetPlatform.WinPhone)
            {
                contactInput.WidthRequest = screenWidth * ICON_SIZE / 100;
                contactInput.HeightRequest = screenWidth * ICON_SIZE / 100;
            }

            contactInputStack = new StackLayout
            {
                Padding = new Thickness(5, 10, 0, 10),
                Spacing = 0,
                Children = { contactInput
                    //new Label { Text = "Contact", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) }
                     }
            };

            #region CONTACTS TAP RECOGNIZER

            TapGestureRecognizer contactsInputTapRecognizer = new TapGestureRecognizer();
            contactInputStack.GestureRecognizers.Add(contactsInputTapRecognizer);

            contactsInputTapRecognizer.Tapped += async (s, e) =>
            {
				await ApplyAnimation( contactInputStack );

				try
				{
					if( Device.OS == TargetPlatform.Android  || Device.OS == TargetPlatform.iOS )
					{
						IContactPicker testicker = DependencyService.Get< IContactPicker >();
						testicker.ShowContactPicker();
					}	
				}
				catch (Exception ex)
                {
                    DisplayAlert("contactsInputTapRecognizer: ", ex.Message, "ok");
                }
                 
            };

            #endregion

            #endregion

            #region ICON CONTAINER GRID

            iconContainerGrid = new Grid
            {
                IsVisible = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = 
            {
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions = 
            {
                    new ColumnDefinition { Width = new GridLength(((screenWidth * .80) )/4, GridUnitType.Absolute) }, // icon container x = 3 //new ColumnDefinition { Width = GridLength.Auto },
                  //  new ColumnDefinition { Width = new GridLength(((screenWidth * .80)) /5, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(((screenWidth * .80))/4, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(((screenWidth * .80))/4, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(((screenWidth * .80))/4, GridUnitType.Absolute) },
            }
            };

            iconContainerGrid.Children.Add(galleryInputStack, 0, 0);
            iconContainerGrid.Children.Add(cameraInputStack, 1, 0);
          //  iconContainerGrid.Children.Add(audioInputStack, 2, 0);
            iconContainerGrid.Children.Add(locationInputStack, 2, 0);
            iconContainerGrid.Children.Add(contactInputStack, 3, 0);

            textinputAndIconsHolder = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0,
                Padding = 0,
				Children = { textInputContainer, iconContainerGrid, editLocationAndContactsStack }
            };

            Button createEvent = new Button();
            if (pageTitle == Constants.ADD_ACTIONS || pageTitle == Constants.ADD_GOALS || pageTitle == Constants.EDIT_ACTIONS || pageTitle == Constants.EDIT_GOALS)
            {
                createEvent.BackgroundColor = Color.Transparent;
                createEvent.TextColor = Constants.BLUE_BG_COLOR;
                createEvent.Text = "Create Reminder";
                createEvent.FontSize = 12;
                createEvent.BorderWidth = 0;
                createEvent.BorderColor = Color.Transparent;
                createEvent.Clicked += createEvent_Clicked;
				if( Device.OS == TargetPlatform.iOS )
				createEvent.TranslationY = -8;
                textinputAndIconsHolder.Children.Add(createEvent);

            }
            masterLayout.AddChildToLayout(textinputAndIconsHolder, 5, 21);


            #region PREVIEW LIST
            listContainer = new StackLayout();
            listContainer.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            listContainer.WidthRequest = screenWidth * 90 / 100;
            listContainer.HeightRequest = screenHeight * 25 / 100;
            listContainer.ClassId = "preview";

            previewListView = new ListView();
            previewListView.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            PreviewListViewCellItem.addEvntObject = this;
            previewListView.ItemTemplate = new DataTemplate(typeof(PreviewListViewCellItem));
            previewListView.SeparatorVisibility = SeparatorVisibility.None;
            previewListView.Opacity = 1;
            previewListView.ItemsSource = App.PreviewListSource;
			previewListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => 
			{
              /*  PreviewItem items = previewListView.SelectedItem as PreviewItem;
                if( items != null )
                App.Navigator.PushModalAsync( new VideoPlayerView( items.Path ) );*/
				previewListView.SelectedItem = null;
			};
            listContainer.Children.Add(previewListView);
            masterLayout.AddChildToLayout(listContainer, 5, Device.OnPlatform( 63, 63, 50 ));
            #endregion

			//masterLayout.AddChildToLayout(locationEditStack, 5, 30 );
            #endregion

            Content = masterLayout;
        }

        void EventDescription_TextChanged (object sender, TextChangedEventArgs e)
        {
			CustomEditor editor = sender as CustomEditor;
			if (e.NewTextValue != null && e.NewTextValue.Length > 1000) {
				editor.Text = e.OldTextValue;
			}
        }

        void EventTitle_TextChanged (object sender, TextChangedEventArgs e)
        {
			CustomEntry entry = sender as CustomEntry;
			if (e.NewTextValue != null && e.NewTextValue.Length > 35) {
				entry.Text = e.OldTextValue;
			}
        }

        private void AddFilenameToMediaList(string fileName)
        {
            string imgType = System.IO.Path.GetExtension(fileName);
            
			if (imgType== ".png" || imgType== ".jpeg" || imgType== ".jpg" || imgType== ".bmp")
	        {
                AddFileToMediaArray(null, fileName, PurposeColor.Constants.MediaType.Image);
	        }
			else if (imgType== ".3gpp" || imgType== ".wma" || imgType== ".mp3" || imgType== ".ogg"|| imgType== ".wav" || imgType== ".amr" || imgType== ".3gp")
	        {
                AddFileToMediaArray(null, fileName, PurposeColor.Constants.MediaType.Audio);
	        }
			else if (imgType== ".mp4" || imgType== ".avi" || imgType== ".flv"|| imgType== ".wmv"|| imgType== ".ogg")
	        {
                AddFileToMediaArray(null, fileName, PurposeColor.Constants.MediaType.Video);
	        }
        }


		private async Task<bool> ApplyAnimation( StackLayout layout )
		{
			await layout.TranslateTo(0, -30, 250, Easing.CubicOut);
			await layout.TranslateTo(0, 0, 250, Easing.CubicOut);
			return true;
		}

        void OnLocationEditCompleted(object sender, EventArgs e)
        {
            try
            {

                if (editLocationAndContactsStack.ClassId == "locationedit")
                {
                    if (locationInfo.FormattedText != null && locationInfo.FormattedText.Spans.Count > 1)
                        locationInfo.FormattedText.Spans[1].Text = locAndContactsEntry.Text;
                    locationInfo.IsVisible = true;
                }
                else
                {
                    //contactInfo.Text = locAndContactsEntry.Text;
                    if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
                        contactInfo.FormattedText.Spans[1].Text = locAndContactsEntry.Text;
                    contactInfo.IsVisible = true;
                }
                editLocationAndContactsStack.IsVisible = false;
                iconContainerGrid.IsVisible = true;

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        private void AnimateMic()
        {
            if (Seconds % 2 == 0)
            {
                audioRecodeOffButton.FadeTo(0, 10, Easing.Linear);
            }
            else
            {
                audioRecodeOffButton.FadeTo(1, 10, Easing.Linear);
            }
        }

        async void OnEditLocationInfo(object sender, EventArgs e)
		{
            try
            {

                editLocationAndContactsStack.ClassId = "locationedit";
                if (locationInfo.FormattedText != null && locationInfo.FormattedText.Spans.Count > 1)
                    locAndContactsEntry.Text = locationInfo.FormattedText.Spans[1].Text;
                editLocationAndContactsStack.IsVisible = true;
                locationInfo.IsVisible = false;
                iconContainerGrid.IsVisible = false;
                contactInfo.IsVisible = true;

                await editLocationAndContactsStack.TranslateTo(100, 0, 300, Easing.SinInOut);
                await editLocationAndContactsStack.TranslateTo(0, 0, 300, Easing.SinIn);

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        void RecodeOnTapRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                IProgressBar progress = DependencyService.Get<IProgressBar>();
                if (!isAudioRecording)
                {
                    
                    if (!audioRecorder.RecordAudio())
                    {
                        progress.ShowToast("Audio cannot be recorded, please try again later.");
                        isAudioRecording = false;
                        audioRecodeOffHolder.IsVisible = false;
                        audioRecodeOnHolder.IsVisible = true;
                    }
                    else
                    {
                        audioRecodeOffHolder.IsVisible = true;
                        audioRecodeOnHolder.IsVisible = false;
                        isAudioRecording = true;
                        Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                        {
                            //RecodeOffTapRecognizer_Tapped(audioRecodeOnHolder, null);
                            //progress.ShowToast("Maximum duration has reached.");
                            //return false;
                            if (isAudioRecording)
                            {
                                if (Seconds >= 120) // for one minute// 120 * 500 = 60 Sec. //
                                {
                                    RecodeOffTapRecognizer_Tapped(audioRecodeOnHolder, null);
                                    progress.ShowToast("Maximum duration has reached.");
                                    Seconds = 0;
                                    return false;
                                }
                                else
                                {
                                    Seconds++;
                                    AnimateMic();
                                    return true;
                                }
                            }
                            Seconds = 0;
                            return false;

                        });

                        progress.ShowToast("Audio recording started.");
                    }
                }
            }
            catch (System.Exception)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Audio cannot be recorded, please try again later.", Constants.ALERT_OK);
                audioRecodeOffHolder.IsVisible = false;
                audioRecodeOnHolder.IsVisible = true;
                isAudioRecording = false;
                Seconds = 0;
            }
        }

        void RecodeOffTapRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                IProgressBar progress = DependencyService.Get<IProgressBar>();
                if (isAudioRecording)
                {
                    audioRecodeOnHolder.IsVisible = true;
                    audioRecodeOffHolder.IsVisible = false;
                    isAudioRecording = false;
                    MemoryStream stream = audioRecorder.StopRecording();
                    if (stream == null)
                    {
                        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                        {
                            stream = audioRecorder.StopRecording();
                            AddFileToMediaArray(stream, audioRecorder.AudioPath, Constants.MediaType.Audio);
                            if (stream == null)
                            {
                                progress.ShowToast("Could not save audio, please try again later.");
                            }
                            return false;
                        });

                        
                    }
                    else
                    {
                        AddFileToMediaArray(stream, audioRecorder.AudioPath, Constants.MediaType.Audio);
                    }
                }
            }
            catch (System.Exception)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Audio cannot be recorded, please try again later.", Constants.ALERT_OK);
            }
        }

        void createEvent_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushModalAsync(new CreateEventPage());
            }
            catch (Exception)
            {
            }
        }

        void OnEndDateCalanderClicked(object sender, EventArgs e)
        {
            try
            {

                CalendarView endCalendarView = new CalendarView()
                {
                    BackgroundColor = Color.FromRgb(30, 126, 210),// Color.FromRgb(200, 219, 238),
					MinDate = CalendarView.FirstDayOfMonth(DateTime.Now),
					MaxDate = CalendarView.LastDayOfMonth(DateTime.Now.AddMonths(3)),
                    HighlightedDateBackgroundColor = Color.FromRgb(227, 227, 227),
                    ShouldHighlightDaysOfWeekLabels = false,
                    SelectionBackgroundStyle = CalendarView.BackgroundStyle.CircleFill,
                    TodayBackgroundStyle = CalendarView.BackgroundStyle.CircleOutline,
                    HighlightedDaysOfWeek = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday },
                    ShowNavigationArrows = true,
                    MonthTitleFont = Font.OfSize("Open 24 Display St", NamedSize.Medium),
                    MonthTitleForegroundColor = Color.White,
                    DayOfWeekLabelForegroundColor = Color.White,

                };
                endCalendarView.ClassId = "endcalander";
                endCalendarView.DateSelected += OnEndCalendarViewDateSelected;
                masterLayout.AddChildToLayout(endCalendarView, 0, 35);

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        void OnEndCalendarViewDateSelected(object sender, DateTime e)
        {
            try
            {

                endDateLabel.Text = "Ends : " + e.Day.ToString() + "-" + e.Month.ToString() + "-" + e.Year.ToString();
                View view = masterLayout.Children.First(child => child.ClassId == "endcalander");
                if (view != null)
                {
                    masterLayout.Children.Remove(view);
                    view = null;
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        void OnStartDateCalanderClicked(object sender, EventArgs e)
        {
            try
            {

                CalendarView calendarView = new CalendarView()
                {
                    BackgroundColor = Color.FromRgb(30, 126, 210),// Color.FromRgb(200, 219, 238),
					MinDate = CalendarView.FirstDayOfMonth(DateTime.Now),
					MaxDate = CalendarView.LastDayOfMonth(DateTime.Now.AddMonths(3)),
                    HighlightedDateBackgroundColor = Color.FromRgb(227, 227, 227),
                    ShouldHighlightDaysOfWeekLabels = false,
                    SelectionBackgroundStyle = CalendarView.BackgroundStyle.CircleFill,
                    TodayBackgroundStyle = CalendarView.BackgroundStyle.CircleOutline,
                    HighlightedDaysOfWeek = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday },
                    ShowNavigationArrows = true,
                    MonthTitleFont = Font.OfSize("Open 24 Display St", NamedSize.Medium),
                    MonthTitleForegroundColor = Color.White,
                    DayOfWeekLabelForegroundColor = Color.White,

                };
                calendarView.ClassId = "startcalander";
                calendarView.DateSelected += OnStartDateCalendarViewDateSelected;
                masterLayout.AddChildToLayout(calendarView, 0, 35);

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        void OnStartDateCalendarViewDateSelected(object sender, DateTime e)
        {
            try
            {

                startDateLabel.Text = "Starts : " + e.Day.ToString() + "-" + e.Month.ToString() + "-" + e.Year.ToString();
                View view = masterLayout.Children.First(child => child.ClassId == "startcalander");
                if (view != null)
                {
                    masterLayout.Children.Remove(view);
                    view = null;
                }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        async void LocationInputTapRecognizer_Tapped(object sender, EventArgs e)
        {
			await ApplyAnimation ( locationInputStack );
            App.nearByLocationsSource.Clear();
            IProgressBar progress = DependencyService.Get<IProgressBar>();
            try
            {
                var locator = CrossGeolocator.Current;
				//locator.AllowsBackgroundUpdates = true;
                locator.DesiredAccuracy = 50;

                if (!locator.IsGeolocationEnabled)
                {
                    DisplayAlert("Purpose Color", "Please turn ON location services", "Ok");
                    return;
                }


                progress.ShowProgressbar("Getting Location..");

                locator.StartListening(1, 100);


                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                if (position == null)
                {
                    return;
                }
                App.Lattitude = position.Latitude;
                App.Longitude = position.Longitude;
                lattitude = position.Latitude.ToString() != null ? position.Latitude.ToString() : string.Empty;
                longitude = position.Longitude.ToString() != null ? position.Longitude.ToString() : string.Empty;
                locator.StopListening();

                ILocation loc = DependencyService.Get<ILocation>();
                try
                {

                    await ServiceHelper.GetCurrentAddressToList(App.Lattitude, App.Longitude);
                    await ServiceHelper.GetNearByLocations( App.Lattitude, App.Longitude );


                    progress.HideProgressbar();
                    List<CustomListViewItem> pickerSource = App.nearByLocationsSource;
                    CustomPicker ePicker = new CustomPicker(masterLayout, pickerSource, Device.OnPlatform(65, 65, 55),"Nearby Places", true, false);// 65
                    ePicker.WidthRequest = screenWidth;
                    ePicker.HeightRequest = screenHeight;
                    ePicker.ClassId = "ePicker";
                    ePicker.listView.ItemSelected += OnLocationListViewItemSelected;
                    masterLayout.AddChildToLayout(ePicker, 0, 0);
                }
                catch (Exception)
                {
                    DisplayAlert("Location error", "Address could not be retrived", "OK");
                }


            }
            catch (Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Location service failed.", Constants.ALERT_OK);
                progress.HideProgressbar();
            }

        }

        void OnLocationListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                CustomListViewItem item = (CustomListViewItem)e.SelectedItem;

                locationInfo.Text = "";
                var s = new FormattedString();
                s.Spans.Add(new Span { Text = "   - at ", ForegroundColor = Color.Black });
                s.Spans.Add(new Span { Text = item.Name });
				currentAddress = item.Name;
                locationInfo.FormattedText = s;
                locLayout.IsVisible = true;
                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                masterLayout.Children.Remove(pickView);
                pickView = null;

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }



		private void OnContactSelected( string contactName )
		{
			try
			{
				string name = contactName;
				if (!string.IsNullOrEmpty(name))
				{
					string preText = "   - with ";
					selectedContact = name;

					var s = new FormattedString();

					if (contactInfo.FormattedText == null)
					{
						contactInfo.Text = preText;
						s.Spans.Add(new Span { Text = preText, ForegroundColor = Color.Black });
					}

					if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
					{
						string spanContact = "";
						if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
						{
							spanContact = contactInfo.FormattedText.Spans[1].Text + " , " + selectedContact; ;
						}
						s.Spans.Add(new Span { Text = preText, ForegroundColor = Color.Black });
						s.Spans.Add(new Span { Text = spanContact });
					}
					else
					{

						string spanContact = "";
						if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
						{
							spanContact = contactInfo.FormattedText.Spans[1].Text;
						}
						else
						{
							spanContact = selectedContact;
							s.Spans.Add(new Span { Text = selectedContact });
						}

					}


					contactInfo.FormattedText = s;

					if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
					{
						if (contactInfo.FormattedText.Spans[1].Text.Length > 40)
						{
							string trimmedContacts = contactInfo.FormattedText.Spans[1].Text;
							trimmedContacts = trimmedContacts.Substring(0, 40);
							trimmedContacts += "...";

							contactInfo.FormattedText.Spans[1].Text = trimmedContacts;
						}

					}



					contactInfo.IsVisible = true;
					App.ContactsArray.Add(name);

				}

				View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
				masterLayout.Children.Remove(pickView);
				pickView = null;

			}
			catch (Exception ex)
			{
				var test = ex.Message;
			}
		}


        private void OnContactsPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                var obj = e.SelectedItem as CustomListViewItem;
                string name = (e.SelectedItem as CustomListViewItem).Name;
                if (!string.IsNullOrEmpty(name))
                {
                    string preText = "   - with ";
                    selectedContact = name;

                    var s = new FormattedString();

                    if (contactInfo.FormattedText == null)
                    {
                        contactInfo.Text = preText;
                        s.Spans.Add(new Span { Text = preText, ForegroundColor = Color.Black });
                    }

                    if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
                    {
                        string spanContact = "";
                        if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
                        {
                            spanContact = contactInfo.FormattedText.Spans[1].Text + " , " + selectedContact; ;
                        }
                        s.Spans.Add(new Span { Text = preText, ForegroundColor = Color.Black });
                        s.Spans.Add(new Span { Text = spanContact });
                    }
                    else
                    {

                        string spanContact = "";
                        if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
                        {
                            spanContact = contactInfo.FormattedText.Spans[1].Text;
                        }
                        else
                        {
                            spanContact = selectedContact;
                            s.Spans.Add(new Span { Text = selectedContact });
                        }

                    }


                    contactInfo.FormattedText = s;

                    if (contactInfo.FormattedText != null && contactInfo.FormattedText.Spans.Count > 1)
                    {
                        if (contactInfo.FormattedText.Spans[1].Text.Length > 40)
                        {
                            string trimmedContacts = contactInfo.FormattedText.Spans[1].Text;
                            trimmedContacts = trimmedContacts.Substring(0, 40);
                            trimmedContacts += "...";

                            contactInfo.FormattedText.Spans[1].Text = trimmedContacts;
                        }

                    }



                    contactInfo.IsVisible = true;
                    App.ContactsArray.Add(name);

                }

                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                masterLayout.Children.Remove(pickView);
                pickView = null;

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        public void DisplayAlert( string messege )
        {
            DisplayAlert(Constants.ALERT_TITLE, messege, Constants.ALERT_OK);
        }

        async void NextButtonTapRecognizer_Tapped(object sender, System.EventArgs e)
        {
			User user = null;
			try {
				user = App.Settings.GetUser();
			} catch (Exception ex) {

			}
            IProgressBar progress = DependencyService.Get<IProgressBar>();
            try
            {
                if (string.IsNullOrWhiteSpace(eventDescription.Text) || string.IsNullOrWhiteSpace(eventTitle.Text))
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Value cannot be empty", Constants.ALERT_OK);
                }
                else
                {
                    string input = pageTitle;
                    CustomListViewItem item = new CustomListViewItem { Name = eventDescription.Text };
                    bool serviceResultOK = false;
                    if (input == Constants.ADD_ACTIONS || input == Constants.EDIT_ACTIONS)
                    {
                        #region ADD || EDIT ACTIONS

                        try
                        {
							GoalDetails newGoal = new GoalDetails();



                            ActionModel details = new ActionModel();
                            if (!isUpdatePage)
                            {
                                progress.ShowProgressbar("Creating new action..");
                            }
                            else
                            {
                                progress.ShowProgressbar("Updating the action..");
                                details.action_id = currentGemId;
                            }
                            details.action_title = eventTitle.Text;
                            details.action_details = eventDescription.Text;
							details.user_id = user.UserId;
                            details.location_latitude = lattitude;
                            details.location_longitude = longitude;
							if(!string.IsNullOrEmpty(currentAddress))
							{
								details.location_address = currentAddress;
							}
                            //details.start_date = DateTime.Now.ToString("yyyy/MM/dd"); // for testing only
                            //details.end_date = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd"); // for testing only
                            //details.start_time = DateTime.Now.AddHours(1).ToString("HH:mm"); //for testing only
                            //details.end_time = DateTime.Now.AddHours(2).ToString("HH:mm"); //for testing only

                            if (!string.IsNullOrEmpty(App.SelectedActionStartDate))
                            {
								DateTime myDate = DateTime.Now;//DateTime.ParseExact("2009-05-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
                                myDate = DateTime.Parse(App.SelectedActionStartDate);
                                details.start_date = App.SelectedActionStartDate;
                                details.end_date = App.SelectedActionEndDate;
                                details.start_time = myDate.ToString("HH:mm");
                                myDate = DateTime.Parse(App.SelectedActionEndDate);
                                details.end_time = myDate.ToString("HH:mm");
                                details.action_repeat = "0";
                                details.action_alert = App.SelectedActionReminderValue.ToString();
                            }
                            

                            if (!string.IsNullOrEmpty(currentAddress))
                            {
                                details.location_address = currentAddress;
                            }

                            if (!await ServiceHelper.AddAction(details))
                            {
                                await DisplayAlert(Constants.ALERT_TITLE, Constants.NETWORK_ERROR_MSG, Constants.ALERT_OK);
                            }
                            else
                            {
                                try
                                {
                                    var suportingActions = await ServiceHelper.GetAllSpportingActions(); //for testing only
                                    if (suportingActions != null)
                                    {
                                        App.actionsListSource = null;
                                        App.actionsListSource = new List<CustomListViewItem>();
                                        foreach (var action in suportingActions)
                                        {
                                            App.actionsListSource.Add(action);
                                        }
                                    }

									// No need to update action list view since we are not showing action listview anywhere in gems
									if( input != Constants.EDIT_ACTIONS )
									{
										CustomListViewItem newEmotionItem = new CustomListViewItem();
										newEmotionItem.EventID = App.actionsListSource.First().EventID;
										newEmotionItem.Name = App.actionsListSource.First().Name;
										newEmotionItem.SliderValue = App.actionsListSource.First().SliderValue;
										newEmotionItem.Source = Device.OnPlatform("tick_box.png", "tick_box.png", "//Assets//tick_box.png");
										SelectedItemChangedEventArgs newEmotionEvent = new SelectedItemChangedEventArgs( newEmotionItem );
										feelingSecondPage.OnActionPickerItemSelected( this, newEmotionEvent );
									}
										
                                    serviceResultOK = true;
                                }
                                catch (System.Exception)
                                {
                                    DisplayAlert(Constants.ALERT_TITLE, "Error in retrieving goals list, Please try again", Constants.ALERT_OK);
                                }
                            }

//                            ILocalNotification notfiy = DependencyService.Get<ILocalNotification>();
//                            if (!isUpdatePage)
//                            {
//								notfiy.ShowNotification("Purpose Color - Action Created", "", eventTitle.Text, false);
//                            }
//                            else
//                            {
//								notfiy.ShowNotification("Purpose Color - Action Updated","", eventTitle.Text, false);
//                            }
                        }
                        catch (Exception ex)
                        {
                            var test = ex.Message;
                        }

                        progress.HideProgressbar();

                        
                        #endregion
                    }
                    else if (input == Constants.ADD_EVENTS || input == Constants.EDIT_EVENTS)
                    {
                        #region ADD || EDIT EVENTS

                        try
                        {
                            EventDetails details = new EventDetails();
                            details.event_title = eventTitle.Text;
                            details.event_details = eventDescription.Text;
							details.user_id = user.UserId;
                            details.location_latitude = lattitude;
                            details.location_longitude = longitude;
							if (!string.IsNullOrEmpty(currentAddress))
							{
								details.location_address = currentAddress;
							}
                            
                            if (!isUpdatePage)
                            {
                                progress.ShowProgressbar("Creating new event..");
                            }
                            else
                            {
                                progress.ShowProgressbar("Updating the event..");
                                details.event_id = currentGemId;
                            }

                            if (!await ServiceHelper.AddEvent(details))
                            {
                                await DisplayAlert(Constants.ALERT_TITLE, Constants.NETWORK_ERROR_MSG, Constants.ALERT_OK);
                            }
                            else
                            {
                                await FeelingNowPage.DownloadAllEvents();
                                serviceResultOK = true;


								CustomListViewItem newEmotionItem = new CustomListViewItem();
								newEmotionItem.EventID = App.eventsListSource.First().EventID;
								newEmotionItem.Name = App.eventsListSource.First().Name;
								newEmotionItem.SliderValue = App.eventsListSource.First().SliderValue;

								SelectedItemChangedEventArgs newEmotionEvent = new SelectedItemChangedEventArgs( newEmotionItem );
								feelingsPage.OnEventPickerItemSelected( this, newEmotionEvent );

//								if(!isUpdatePage)
//								{
//									await Navigation.PopModalAsync();
//								}
//								else {
//									await Navigation.PopModalAsync();
//								}
                            }
                        }
                        catch (Exception ex)
                        {
                            var test = ex.Message;
                        }
                        progress.HideProgressbar();
                        
                        #endregion
                    }
                    else if (input == Constants.ADD_GOALS || input == Constants.EDIT_GOALS)
                    {
                        #region ADD || EDIT GOALS
		
                        try
                        {
                            GoalDetails newGoal = new GoalDetails();

                            //EventDetails newGoal = new EventDetails();
                            newGoal.goal_title = eventTitle.Text;
                            newGoal.goal_details = eventDescription.Text;
							newGoal.user_id = user.UserId;
                            newGoal.location_latitude = lattitude;
                            newGoal.location_longitude = longitude;
                            newGoal.category_id = "1";

                            if (!string.IsNullOrEmpty(App.SelectedActionStartDate))
                            {
                                newGoal.start_date = App.SelectedActionStartDate; //DateTime.Now.ToString("yyyy/MM/dd"); // for testing only
                                newGoal.end_date = App.SelectedActionEndDate; //DateTime.Now.AddDays(1).ToString("yyyy/MM/dd"); // for testing onl
                            }
                            
                            if (!string.IsNullOrEmpty(currentAddress))
                            {
                                newGoal.location_address = currentAddress;
                            }

                            if (!isUpdatePage)
                            {
                                progress.ShowProgressbar("Creating new goal..");
                            }
                            else
                            {
                                progress.ShowProgressbar("Updating the goal..");
                                newGoal.goal_id = currentGemId;
                            }

                            if (!await ServiceHelper.AddGoal(newGoal))
                            {
                                await DisplayAlert(Constants.ALERT_TITLE, Constants.NETWORK_ERROR_MSG, Constants.ALERT_OK);
                            }
                            else
                            {
                                try
                                {
									var goals = await ServiceHelper.GetAllGoals( user.UserId );
                                    if (goals != null)
                                    {
                                        App.goalsListSource = new List<CustomListViewItem>();
                                        foreach (var goal in goals)
                                        {
                                            App.goalsListSource.Add(goal);
                                        }
                                    }

									// When editing goals no need to update goals list view since we are now in gems module not in emotional awareness
									// we are not showing goals list view any where in gems module
									if( input != Constants.EDIT_GOALS )
									{
										CustomListViewItem newEmotionItem = new CustomListViewItem();
										newEmotionItem.EventID = App.goalsListSource.First().EventID;
										newEmotionItem.Name = App.goalsListSource.First().Name;
										newEmotionItem.SliderValue = App.goalsListSource.First().SliderValue;

										SelectedItemChangedEventArgs newGoalsEvent = new SelectedItemChangedEventArgs( newEmotionItem );
										feelingSecondPage.OnGoalsPickerItemSelected( this, newGoalsEvent );
									}
                                    serviceResultOK = true;
                                }
                                catch (System.Exception)
                                {
                                    DisplayAlert(Constants.ALERT_TITLE, "Error in retrieving goals list, Please try again", Constants.ALERT_OK);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DisplayAlert(Constants.ALERT_TITLE, ex.Message, Constants.ALERT_OK);
                        }
                        progress.HideProgressbar();
 
	                    #endregion
                    }

                    if (serviceResultOK)
                    {
                        if (!isUpdatePage)
                        {
							await Navigation.PopAsync();
                        }
                        else
                        {
							if(Device.OS != TargetPlatform.iOS)
							{
								await Navigation.PopToRootAsync();
							}
							else
							{
								await Navigation.PopToRootAsync();
							}
							#region MyRegionNavigate back to GEM details page
							//progress.ShowProgressbar("Loading");
//
//							if (App.isEmotionsListing) {
//								try {
//									SelectedEventDetails eventDetails = await ServiceHelper.GetSelectedEventDetails(currentGemId);
//									if (eventDetails != null)
//									{
//										List<string> listToDownload = new List<string>();
//
//										foreach (var eventi in eventDetails.event_media) 
//										{
//											if(string.IsNullOrEmpty(eventi.event_media))
//											{
//												continue;
//											}
//
//											if (eventi.media_type == "png" || eventi.media_type == "jpg" || eventi.media_type == "jpeg") 
//											{
//
//												listToDownload.Add(Constants.SERVICE_BASE_URL+eventi.event_media);
//												string fileName = System.IO.Path.GetFileName(eventi.event_media);
//												eventi.event_media = App.DownloadsPath + fileName;
//											}
//											else
//											{
//												eventi.event_media = Constants.SERVICE_BASE_URL + eventi.event_media ;
//											}
//										}
//
//										if (listToDownload != null && listToDownload.Count > 0) {
//											IDownload downloader = DependencyService.Get<IDownload>();
//											//progressBar.ShowProgressbar("loading details..");
//											await downloader.DownloadFiles(listToDownload);
//
//										}
//
//										DetailsPageModel model = new DetailsPageModel();
//										model.actionMediaArray = null;
//										model.eventMediaArray = eventDetails.event_media;
//										model.goal_media = null;
//										model.Media = null;
//										model.NoMedia = null;
//										model.pageTitleVal = "Event Details";
//										model.titleVal = eventDetails.event_title;
//										model.description = eventDetails.event_details;
//										model.gemType = GemType.Event;
//										model.gemId = currentGemId;
//										progress.HideProgressbar();
//
//										await Navigation.PushAsync(new GemsDetailsPage(model));
//										eventDetails = null;
//									}
//								} catch (Exception ) {
//									progress.HideProgressbar();
//									Navigation.PopAsync();
//								}
//							}
//							else
//							{
//								//-- call service for Action details
//								try {
//
//									SelectedActionDetails actionDetails = await ServiceHelper.GetSelectedActionDetails(currentGemId);
//
//									List<string> listToDownload = new List<string>();
//
//									foreach (var action in actionDetails.action_media) 
//									{
//										if( string.IsNullOrEmpty(action.action_media))
//										{
//											continue;
//										}
//
//										if (action.media_type == "png" || action.media_type == "jpg" || action.media_type == "jpeg")
//										{
//
//											listToDownload.Add(Constants.SERVICE_BASE_URL+action.action_media);
//											string fileName = System.IO.Path.GetFileName(action.action_media);
//											action.action_media = App.DownloadsPath + fileName;
//										}
//										else
//										{
//											action.action_media = Constants.SERVICE_BASE_URL + action.action_media;
//										}
//									}
//
//									if (listToDownload != null && listToDownload.Count > 0) {
//										IDownload downloader = DependencyService.Get<IDownload>();
//										//progressBar.ShowProgressbar("loading details..");
//										await downloader.DownloadFiles(listToDownload);
//										//progressBar.HideProgressbar();
//									}
//
//									if (actionDetails != null) {
//										DetailsPageModel model = new DetailsPageModel();
//										model.actionMediaArray = actionDetails.action_media;
//										model.eventMediaArray = null;
//										model.goal_media = null;
//										model.Media = null;
//										model.NoMedia = null;
//										model.pageTitleVal = "Action Details";
//										model.titleVal = actionDetails.action_title;
//										model.description = actionDetails.action_details;
//										model.gemType = GemType.Action;
//										model.gemId = currentGemId;
//
//										progress.HideProgressbar();
//										await Navigation.PushAsync(new GemsDetailsPage(model));
//
//
//
//
//										actionDetails = null;
//									}
//								} catch (Exception ) {
//									progress.HideProgressbar();
//									Navigation.PopAsync();
//								}
//        
//							
							#endregion

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        async void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            try
            {
				if(!isUpdatePage) {
					await Navigation.PopModalAsync();
				}
				else {
					// Navigation.PopToRootAsync(); // this will take to GEMS page.
					await Navigation.PopAsync();
				}
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        public void AddFileToMediaArray(MemoryStream ms, string path, PurposeColor.Constants.MediaType mediaType)
        {
            try
            {
                MediaPost mediaWeb = new MediaPost();
                mediaWeb.event_details = string.IsNullOrWhiteSpace(eventDescription.Text) ? string.Empty : eventDescription.Text;
                mediaWeb.event_title = string.IsNullOrWhiteSpace(eventTitle.Text) ? string.Empty : eventTitle.Text;
				User user = App.Settings.GetUser();
                
				mediaWeb.user_id = Convert.ToInt32(user.UserId);

                string imgType = System.IO.Path.GetExtension(path);
                string fileName = System.IO.Path.GetFileName(path);

                if (mediaType == Constants.MediaType.Image)
                {
                    App.PreviewListSource.Add(new PreviewItem { Path = path, Name = fileName, Image = Device.OnPlatform("image.png", "image.png", "//Assets//image.png") });//Device.OnPlatform("delete_button.png", "delete_button.png", "//Assets//delete_button.png");
                }
                else if (mediaType == Constants.MediaType.Video)
                {
					/*string videoThumpName = Path.GetFileNameWithoutExtension( path ) + ".jpg";
					string downloadFilePath = Path.Combine(App.DownloadsPath, videoThumpName);

					App.PreviewListSource.Add(new PreviewItem { Path = path, Name = fileName, Image = Device.OnPlatform( downloadFilePath, downloadFilePath, "//Assets//video.png") });*/

                    App.PreviewListSource.Add(new PreviewItem { Path = path, Name = fileName, Image = Device.OnPlatform("video.png", "video.png", "//Assets//video.png") });
                }
                else
                {
                    App.PreviewListSource.Add(new PreviewItem { Path = path, Name = fileName, Image = Device.OnPlatform("mic.png", "mic.png", "//Assets//mic.png") });
                }

                #region MEDIA COMPRESSION AND SERIALISING

                if (ms != null)
                {
                    imgType = imgType.Replace(".", "");
                    if (mediaType == Constants.MediaType.Image)
                    {
						IResize resize = DependencyService.Get<IResize>();
						Size imgSize =  resize.GetImageSize( path );
						Byte[] resizedOutput = null;
						if( imgSize.Height > imgSize.Width )
						{
							float screenWidthPixels =  ( float )( App.screenWidth * App.screenDensity );
							double widthRatio =   (double) (imgSize.Width / screenWidthPixels);
							resizedOutput = resize.Resize(ms.ToArray(), (float) screenWidthPixels , (float)(imgSize.Height / widthRatio), path);
						}
						else
						{

							float screenWidthPixels =  ( float )( App.screenWidth * App.screenDensity );
							double widthRatio =   (double) (imgSize.Width / screenWidthPixels);
							resizedOutput = resize.Resize(ms.ToArray(), (float) screenWidthPixels , (float)(imgSize.Height / widthRatio), path);
						}


						MemoryStream resizedStream = new MemoryStream(resizedOutput);
						int streamLength = (int)resizedStream.Length;

						int compressionRate  = 100;



						//compressedStream = resize.CompessImage(compressionRate, resizedStream);

						Byte[] inArray = resizedStream.ToArray();
						Char[] outArray = new Char[(int)(resizedStream.ToArray().Length * 1.34)];
						Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
						string test2 = new string(outArray);
						App.ExtentionArray.Add(imgType);
						MediaItem item = new MediaItem();
						item.MediaString = test2;
						item.Name = fileName;
						App.MediaArray.Add(item);

						inArray = null;
						outArray = null;
						test2 = null;
						item = null;
						resizedOutput = null;
						GC.Collect();
                    }
                    else
                    {
                        Byte[] inArray = ms.ToArray();
                        Char[] outArray = new Char[(int)(ms.ToArray().Length * 1.34)];
                        Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
                        string test2 = new string(outArray);
                        App.ExtentionArray.Add(imgType);
                        MediaItem item = new MediaItem();
                        item.MediaString = test2;
                        item.Name = fileName;
						if( mediaType == Constants.MediaType.Video )
						{
							IVideoCompressor compressor = DependencyService.Get<IVideoCompressor>();
							string imgThumbPath =  App.DownloadsPath +  Path.GetFileNameWithoutExtension( path ) + ".jpg";
							MemoryStream thumbStream = compressor.CreateVideoThumbnail( path, imgThumbPath );

							Byte[] thumbInArray = thumbStream.ToArray();
							Char[] thumbOutArray = new Char[(int)(thumbStream.ToArray().Length * 1.34)];
							Convert.ToBase64CharArray(thumbInArray, 0, thumbInArray.Length, thumbOutArray, 0);
							string thumbString = new string(thumbOutArray);

							item.MediaType = Constants.MediaType.Video;
							item.MediaThumbString = thumbString;
							ms.Dispose();
							thumbStream.Dispose();

							thumbInArray = null;
							thumbOutArray = null;
						}
                        App.MediaArray.Add(item);

                        inArray = null;
                        outArray = null;
                        test2 = null;
                        item = null;
                        GC.Collect();
                    }
                    imgType = string.Empty;
                    fileName = string.Empty;

                }
                
                #endregion

                StackLayout preview = (StackLayout)masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "preview");
                if (preview != null)
                {
                    masterLayout.Children.Remove(preview);
                    preview = null;
                    previewListView.ItemsSource = null;

                    previewListView.ItemsSource = App.PreviewListSource;
                    masterLayout.AddChildToLayout(listContainer, 5, 60);
                }
                
            }
            catch (Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Unable to add the media", Constants.ALERT_OK);
                var test = ex.Message;
            }
        }

        public  async  void ShowAlert( string messege, PreviewItem toDelete )
        {
            try
            {

				
                var alert = await DisplayAlert(Constants.ALERT_TITLE, messege, Constants.ALERT_OK, "Cancel");

                if (alert)
                {
                    MediaItem media = App.MediaArray.FirstOrDefault(med => med.Name == toDelete.Name);

                    if (media == null || toDelete.Name == toDelete.Path)
                    {
                        #region DELETE FROM SERVER.
                        // delete from view and delete from db by api call.
                        IProgressBar progress = DependencyService.Get<IProgressBar>();
                        progress.ShowProgressbar("deleting media");
                        string responseCode = await ServiceHelper.DeleteMediaFromGem(currentGemId, currentGemType, toDelete.Name);
                        if (responseCode == "200")
                        {
                            App.PreviewListSource.Remove(toDelete);
                            progress.HideProgressbar();
                        }
                        else
                        {
                            progress.HideProgressbar();
                            progress.ShowToast("Could not delete the media now.");
                        }

                        #endregion
                    }
                    else
                    {
                        // delete from view and local memory
                        App.PreviewListSource.Remove(toDelete);
                        if (media != null)
                        {
                            App.MediaArray.Remove(media);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        public static void ReceiveVideoFromWindows(MemoryStream ms, string path)
        {
            //AddFileToMediaArray(ms, fileName, PurposeColor.Constants.MediaType.Video);

            try
            {
                MediaPost mediaWeb = new MediaPost();
                mediaWeb.event_details = string.IsNullOrWhiteSpace(eventDescription.Text) ? string.Empty : eventDescription.Text;
                mediaWeb.event_title = string.IsNullOrWhiteSpace(eventTitle.Text) ? string.Empty : eventTitle.Text;
				User user = App.Settings.GetUser();
				mediaWeb.user_id = Convert.ToInt32(user.UserId);

                string imgType = System.IO.Path.GetExtension(path);
                string fileName = System.IO.Path.GetFileName(path);

                App.PreviewListSource.Add(new PreviewItem { Name = fileName, Path = path, Image = Device.OnPlatform("video.png", "video.png", "//Assets//video.png") });

                imgType = imgType.Replace(".", "");
                
                
                Byte[] inArray = ms.ToArray();
                Char[] outArray = new Char[(int)(ms.ToArray().Length * 1.34)];
                Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
                string test2 = new string(outArray);
                App.ExtentionArray.Add(imgType);
                MediaItem item = new MediaItem();
                item.MediaString = test2;
                item.Name = fileName;
                App.MediaArray.Add(item);

                inArray = null;
                outArray = null;
                test2 = null;
                item = null;
                imgType = string.Empty;
                fileName = string.Empty;

                StackLayout preview = (StackLayout)masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "preview");
                masterLayout.Children.Remove(preview);
                preview = null;
                previewListView.ItemsSource = null;
                previewListView.ItemsSource = App.PreviewListSource;
                masterLayout.AddChildToLayout(listContainer, 5, 60);

                GC.Collect();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        public void Dispose()
        {
            subTitleBar.BackButtonTapRecognizer.Tapped -= OnBackButtonTapRecognizerTapped;
            subTitleBar.NextButtonTapRecognizer.Tapped -= NextButtonTapRecognizer_Tapped;
            masterLayout = null;
            this.TopTitleBar = null;
            this.subTitleBar = null;
            eventDescription = null;
            this.textInputContainer = null;
            this.audioInputStack = null;
            this.cameraInput = null;
            this.audioInput = null;
            this.cameraInputStack = null;
            this.galleryInput = null;
            this.galleryInputStack = null;
            this.locationInput = null;
            this.locationInputStack = null;
            this.contactInput = null;
            this.contactInputStack = null;
            this.textinputAndIconsHolder = null;
            this.audioRecorder = null;
            eventTitle = null;
            this.iconContainerGrid = null;
			this.locAndContactsEntry = null;
			this.editLocationAndContactsStack = null;
			this.editLocationDoneButton = null;
            GC.Collect();
        }
    }

    public class MediaSourceChooser : ContentView
    {
        CustomLayout PageContainer;
        AddEventsSituationsOrThoughts MasterObject;
        public MediaSourceChooser(AddEventsSituationsOrThoughts masterObject, CustomLayout pageContainer, string type)
        {

            MasterObject = masterObject;
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Transparent;
            //IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            double screenWidth = App.screenWidth;
            double screenHeight = App.screenHeight;
            PageContainer = pageContainer;

            StackLayout layout = new StackLayout();
            layout.BackgroundColor = Color.Black;
            layout.Opacity = .6;
            layout.WidthRequest = screenWidth;
            layout.HeightRequest = screenHeight;

            TapGestureRecognizer emptyAreaTapGestureRecognizer = new TapGestureRecognizer();
            emptyAreaTapGestureRecognizer.Tapped += (s, e) =>
            {

                View pickView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "mediachooser");
                PageContainer.Children.Remove(pickView);
                pickView = null;

            };
            layout.GestureRecognizers.Add(emptyAreaTapGestureRecognizer);

            CustomImageButton imageButton = new CustomImageButton();
            imageButton.ImageName = Device.OnPlatform("image.png", "image.png", @"/Assets/image.png");
            imageButton.WidthRequest = screenWidth * 20 / 100;
            imageButton.HeightRequest = screenHeight * 10 / 100;
            imageButton.ClassId = type;
            imageButton.Clicked += OnImageButtonClicked;

            CustomImageButton videoButton = new CustomImageButton();
            videoButton.ImageName = Device.OnPlatform("video.png", "video.png", @"/Assets/video.png");
            videoButton.WidthRequest = screenWidth * 20 / 100;
            videoButton.HeightRequest = screenHeight * 10 / 100;
            videoButton.ClassId = type;
            videoButton.Clicked += OnVideoButtonClicked;

            masterLayout.AddChildToLayout(layout, 0, 0);
            masterLayout.AddChildToLayout(imageButton, 40, 40);
            masterLayout.AddChildToLayout(videoButton, 40, 60);

            this.BackgroundColor = Color.Transparent;



            Content = masterLayout;
        }

        async void OnImageButtonClicked(object sender, EventArgs e)
        {
            try
            {

                IProgressBar progres = DependencyService.Get<IProgressBar>();

                if (Device.OS != TargetPlatform.iOS)
                    progres.ShowProgressbar("Preparing media..");
                if ((sender as CustomImageButton).ClassId == "camera")
                {
                    try
                    {
                        if (Media.Plugin.CrossMedia.Current.IsCameraAvailable)
                        {
                            string fileName = string.Format("Img{0}.png", System.DateTime.Now.ToString("yyyyMMddHHmmss"));
                            var file = await Media.Plugin.CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
                            {
								Directory = "Purposecolor",
                                Name = fileName
                            });


                            if (file == null)
                            {
                                progres.HideProgressbar();
                                return;
                            }


                            IMediaVIew mediaView = DependencyService.Get<IMediaVIew>();
                            if( mediaView != null )
                            {
                                await mediaView.FixOrientationAsync(file);
                                mediaView = null;
                            }



                            MemoryStream ms = new MemoryStream();
                            file.GetStream().CopyTo(ms);
                            ms.Position = 0;

                            MasterObject.AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Image);
                        }
                    }
                    catch (Exception ex)
                    {
                        var test = ex.Message;
                        progres.HideProgressbar();
                    }

                }
                else if ((sender as CustomImageButton).ClassId == "gallery")
                {
                    try
                    {

                        if (!CrossMedia.Current.IsPickPhotoSupported)
                        {
                            progres.HideProgressbar();
                            return;
                        }

                        var file = await CrossMedia.Current.PickPhotoAsync();

                        if (file == null)
                        {
                            progres.HideProgressbar();
                            return;
                        }


                        MemoryStream ms = new MemoryStream();
                        file.GetStream().CopyTo(ms);
                        ms.Position = 0;

                        MasterObject.AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Image);

                    }
                    catch (Exception ex)
                    {
                        var test = ex.Message;
                        progres.HideProgressbar();
                    }

                }


                View mediaChooserView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "mediachooser");
                PageContainer.Children.Remove(mediaChooserView);
                mediaChooserView = null;

                progres.HideProgressbar();

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

        }

        async void OnVideoButtonClicked(object sender, EventArgs e)
        {
            try
            {


                IProgressBar progres = DependencyService.Get<IProgressBar>();
                if (Device.OS != TargetPlatform.iOS)
                    progres.ShowProgressbar("Preparing media..");
                if ((sender as CustomImageButton).ClassId == "camera")
                {
                    try
                    {
                        if (Device.OS == TargetPlatform.WinPhone)
                        {
                            PurposeColor.interfaces.ICameraCapture camera = DependencyService.Get<PurposeColor.interfaces.ICameraCapture>();
                            camera.RecodeVideo();
                        }
                        else if (Media.Plugin.CrossMedia.Current.IsCameraAvailable)
                        {
                            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
                            {
                                progres.HideProgressbar();
                                return;
                            }



							string fileName = string.Format("Video{0}.mp4", System.DateTime.Now.ToString("yyyyMMddHHmmss"));
							StoreVideoOptions videoOptions = new StoreVideoOptions();
							videoOptions.Name = fileName;
							videoOptions.Directory = "DefaultVideos";
							videoOptions.DesiredLength = TimeSpan.FromMinutes(5);
							videoOptions.Quality = VideoQuality.Low;

							if( Device.OS == TargetPlatform.Android )
							{
								IVideoCompressor compress = DependencyService.Get<IVideoCompressor>();
								Size camSize =  compress.GetCameraSize();
								if( camSize.Width > 5000 )
									videoOptions.Quality = VideoQuality.Low;
								else
									videoOptions.Quality = VideoQuality.Medium;
							}

							var file = await CrossMedia.Current.TakeVideoAsync( videoOptions );

						//	MasterObject.ShowAlert( "test", null );

							await Task.Delay( TimeSpan.FromSeconds( 2 ) );


							//progres.ShowProgressbar( "video is compressing..." );

                            if (file == null)
                            {
                                progres.HideProgressbar();
                                return;
                            }

							string videoFilename = Path.GetFileName( file.Path );
							MemoryStream ms = new MemoryStream();
							/*file.GetStream().CopyTo(ms);*/

							if( Device.OS == TargetPlatform.Android )
							{
								IVideoCompressor compressor = DependencyService.Get<IVideoCompressor>();
								ms = compressor.CompressVideo( file.Path, App.DownloadsPath + videoFilename, false );
								ms.Position = 0;
							}
							else if( Device.OS == TargetPlatform.iOS )
							{
								file.GetStream().CopyTo(ms);
							}
	

			
							if( ms == null )
							{
								MasterObject.DisplayAlert("Error in adding video.");
								return;
							}
                            if (ms.Length > 15728640)
                            {
                                MasterObject.DisplayAlert("Can not add video, Maximum file size limied to 15 MB");
                                progres.HideProgressbar();
                                ms = null;
                                file = null;
                                GC.Collect();
                                return;
                            }



							if( Device.OS == TargetPlatform.Android )
							{
								MasterObject.AddFileToMediaArray(ms, App.DownloadsPath + videoFilename, PurposeColor.Constants.MediaType.Video);
							}
							else if( Device.OS == TargetPlatform.iOS )
							{
								MasterObject.AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Video);
							}

                        }

                    }
                    catch (Exception ex)
                    {
                        var test = ex.Message;
                    }
                    progres.HideProgressbar();
                }
                else if ((sender as CustomImageButton).ClassId == "gallery")
                {
                    try
                    {
                        if (Device.OS == TargetPlatform.WinPhone)
                        {
                            //https://social.msdn.microsoft.com/Forums/windowsapps/en-US/7e4492dc-d8f3-4dc5-8055-625352aaa8b6/fileopenpicker-class-on-wp8
                            // We do not currently support choosing files other than photos or choosing files from other Store apps

                            // list the video folder content in a list view.
                            // http://www.c-sharpcorner.com/UploadFile/2b876a/how-to-use-folders-and-files-in-windows-phone-8/

                            PurposeColor.interfaces.IFileBrowser fileBrowser = DependencyService.Get<PurposeColor.interfaces.IFileBrowser>();
                            List<String> files = await fileBrowser.GetVideoFileList();
                            //fileBrowser = null; // so the the memory can be released.

                            if (files == null || files.Count < 1)
                            {
                                //progres.HideProgressbar();
                                // should hide the DataTemplateSelector so don't return from here.'
                                MasterObject.DisplayAlert("Video files not accessible");
                            }
                            else
                            {
                                // display the file names in custom picker, and get the file once the user taps on any of the file name.
                                View fileView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "filePicker");
                                if (fileView != null)
                                {
                                    PageContainer.Children.Remove(fileView);
                                    fileView = null;
                                }

                                List<CustomListViewItem> customList = new List<CustomListViewItem>();
                                foreach (var item in files)
                                {
                                    customList.Add(new CustomListViewItem { Name = item });
                                }

                                CustomPicker filePicker = new CustomPicker(PageContainer, customList, 65, "Select file", true, false);
                                //customList = null;
                                filePicker.WidthRequest = App.screenWidth;
                                filePicker.HeightRequest = App.screenHeight;
                                filePicker.ClassId = "filePicker";
                                PageContainer.AddChildToLayout(filePicker, 0, 0);

                                filePicker.listView.ItemSelected += async (s, eve) =>
                                {
                                    CustomListViewItem item = eve.SelectedItem as CustomListViewItem;
                                    MemoryStream videoFileMS = await fileBrowser.GetVideostream(item.Name);

                                    if (videoFileMS != null)
                                    {
                                        MasterObject.AddFileToMediaArray(videoFileMS, item.Name, Constants.MediaType.Video);
                                    }
                                    else
                                    {
                                        MasterObject.DisplayAlert("File read error");
                                    }

                                    View filepickView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "filePicker");
                                    if (filepickView != null)
                                    {
                                        PageContainer.Children.Remove(filepickView);
                                        fileView = null;
                                    }
                                    //videoFileMS = null;
                                };

                                //fileBrowser = null;
                            }
                        }
                        else if (CrossMedia.Current.IsPickVideoSupported)
                        {
							var file = await CrossMedia.Current.PickVideoAsync();

                            if (file == null)
                            {
                                progres.HideProgressbar();
                                return;
                            }

                            MemoryStream ms = new MemoryStream();
                           /* file.GetStream().CopyTo(ms);
                            ms.Position = 0;*/

							if( Device.OS == TargetPlatform.Android )
							{
								string videoFilename = Path.GetFileName( file.Path );
								IVideoCompressor compressor = DependencyService.Get<IVideoCompressor>();
								ms = compressor.CompressVideo( file.Path, App.DownloadsPath + videoFilename, false );
								ms.Position = 0;
							}
							else if( Device.OS == TargetPlatform.iOS )
							{
								string videoFilename = Path.GetFileName( file.Path );
								IVideoCompressor compressor = DependencyService.Get<IVideoCompressor>();
								ms = compressor.CompressVideo( file.Path, App.DownloadsPath + videoFilename, false );
							}


                            if (ms.Length > 15728640)
                            {
                                MasterObject.DisplayAlert("Can not add video, Maximum file size limied to 15 MB");
                                progres.HideProgressbar();
                                ms = null;
                                file = null;
                                GC.Collect();
                                return;
                            }

							if( Device.OS == TargetPlatform.Android )
							{
								MasterObject.AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Video);	
							}
							else if( Device.OS == TargetPlatform.iOS )
							{
								string fileName = Path.GetFileNameWithoutExtension( file.Path ) + ".mp4";
								string downloadFilePath = Path.Combine(App.DownloadsPath, fileName );
								MasterObject.AddFileToMediaArray(ms, downloadFilePath, PurposeColor.Constants.MediaType.Video);
							}

                            
                        }
                        else
                        {
                            progres.HideProgressbar();
                            MasterObject.DisplayAlert("Video library not available");
                            return;
                        }


                    }
                    catch (Exception ex)
                    {
                        var test = ex.Message;
                    }
                    progres.HideProgressbar();
                }

                View pickView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "mediachooser");
                PageContainer.Children.Remove(pickView);
                pickView = null;
                progres.HideProgressbar();

                GC.Collect();

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }
    }

    public class PreviewListViewCellItem : ViewCell
    {
        public static AddEventsSituationsOrThoughts addEvntObject;
        public PreviewListViewCellItem()
        {
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;
            //IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            double screenWidth = App.screenWidth;
            double screenHeight = App.screenHeight;
            Label name = new Label();
            name.SetBinding(Label.TextProperty, "Name");
            name.TextColor = Color.Black;
            name.FontSize = Device.OnPlatform(12, 10, 18);
            name.WidthRequest = screenWidth * 50 / 100;

            StackLayout divider = new StackLayout();
            divider.WidthRequest = screenWidth;
            divider.HeightRequest = .75;
            divider.BackgroundColor = Color.FromRgb(255, 255, 255);

            Image sideImage = new Image();
            sideImage.WidthRequest = Device.OnPlatform(15, 15, 20);
            sideImage.HeightRequest = Device.OnPlatform(15, 15, 20);
            sideImage.SetBinding(Image.SourceProperty, "Image");
            sideImage.Aspect = Aspect.AspectFit;

            CustomImageButton deleteButton = new CustomImageButton();
            deleteButton.ImageName = Device.OnPlatform("delete_button.png", "delete_button.png", @"/Assets/delete_button.png");
            deleteButton.WidthRequest = Device.OnPlatform( 20, 20, 55 );
            deleteButton.HeightRequest = Device.OnPlatform(20, 20, 55);
            deleteButton.SetBinding(CustomImageButton.ClassIdProperty, "Name");

            deleteButton.Clicked += (sender, e) =>
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        
                        CustomImageButton button = sender as CustomImageButton;
                        PreviewItem itemToDel = App.PreviewListSource.FirstOrDefault(item => item.Name == button.ClassId);
                        if (itemToDel != null)
                        {
                            addEvntObject.ShowAlert("Are you sure you want to delete this item ?", itemToDel);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                });


            };

            masterLayout.WidthRequest = screenWidth;
            masterLayout.HeightRequest = screenHeight * Device.OnPlatform(30, 50, 6) / 100;

            masterLayout.AddChildToLayout(sideImage, (float)5, (float)Device.OnPlatform(5, 5, 25), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(name, (float)Device.OnPlatform(15, 15, 15), (float)Device.OnPlatform(5, 5, 25), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(deleteButton, (float)Device.OnPlatform( 80, 80, 75 ), (float)Device.OnPlatform(5, 3.5, 2), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            // masterLayout.AddChildToLayout(divider, (float)1, (float)20, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            this.View = masterLayout;
        }
    }
}
