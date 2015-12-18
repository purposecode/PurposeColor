
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
        #endregion

        public AddEventsSituationsOrThoughts(string title)
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
                trimmedPageTitle += "...";
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
                WidthRequest = (int)(devWidth * .90) // 90% of screen,
            };

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

            eventDescription.WidthRequest = textInputWidth;

            #endregion

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

            Image audioRecodeOffButton = new Image
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

            audioRecodeOffHolder.TranslateTo(0, Device.OnPlatform(audioRecodeOffButton.Y + 60,audioRecodeOffButton.Y + 60,audioRecodeOffButton.Y + 50), 5, null);
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
			locationInfo.HeightRequest = Device.OnPlatform( 15, 25, 25 );
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
			locAndContactsEntry.WidthRequest = App.screenWidth  * 80 / 100;

            editLocationDoneButton = new CustomImageButton();
            editLocationDoneButton.VerticalOptions = LayoutOptions.Center;
			editLocationDoneButton.ImageName = "icn_done.png";
            editLocationDoneButton.HeightRequest = 25;
			editLocationDoneButton.WidthRequest = 25;
            editLocationDoneButton.Clicked += OnLocationEditCompleted;


			editLocationAndContactsStack.Children.Add ( locAndContactsEntry );
			editLocationAndContactsStack.Children.Add ( editLocationDoneButton );

			if (Device.OS == TargetPlatform.iOS) 
			{
				editLocationAndContactsStack.TranslationY = -30;
			}

            locLayout = new StackLayout();
            locLayout.Orientation = StackOrientation.Vertical;
            locLayout.BackgroundColor = Color.Transparent;

     

            locLayout.Children.Add( locationInfo );

			TapGestureRecognizer contactsLabelTap = new TapGestureRecognizer();
		    contactInfo = new Label();
			contactInfo.TextColor = Constants.BLUE_BG_COLOR;
			contactInfo.BackgroundColor = Color.Transparent;
			contactInfo.FontSize = 12;
			contactInfo.HeightRequest = Device.OnPlatform( 15, 25, 25 );
			contactInfo.GestureRecognizers.Add (contactsLabelTap);
			contactsLabelTap.Tapped += async (object sender, EventArgs e) => 
			{
				editLocationAndContactsStack.ClassId = "contactedit";
				locAndContactsEntry.Text = contactInfo.Text;
				editLocationAndContactsStack.IsVisible = true;
				contactInfo.IsVisible = false;
				iconContainerGrid.IsVisible = false;
                locationInfo.IsVisible = true;

                await editLocationAndContactsStack.TranslateTo(100, 0, 300, Easing.SinInOut);
                await editLocationAndContactsStack.TranslateTo(0, 0, 300, Easing.SinIn);

			};

			locLayout.IsVisible = false;
			contactInfo.IsVisible = false;

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

            #region AUDIO TAP RECOGNIZER

            //TapGestureRecognizer audioTapGestureRecognizer = new TapGestureRecognizer();

            //audioTapGestureRecognizer.Tapped += (s, e) =>
            //{
            //    try
            //    {
            //        if (!isAudioRecording)
            //        {
            //            isAudioRecording = true;
            //            if (!audioRecorder.RecordAudio())
            //            {
            //                DisplayAlert("Audio recording", "Audio cannot be recorded, please try again later.", "ok");
            //            }
            //            else
            //            {
            //                DisplayAlert("Audio recording", "Audio recording started, Tap the audio icon again to end.", "ok");
            //            }
            //        }
            //        else
            //        {
            //            isAudioRecording = false;
            //            MemoryStream stream = audioRecorder.StopRecording();


            //            AddFileToMediaArray(stream, audioRecorder.AudioPath, Constants.MediaType.Audio);

            //        }
            //    }
            //    catch (System.Exception)
            //    {
            //        DisplayAlert("Audio recording", "Audio cannot be recorded, please try again later.", "ok");
            //    }
            //};

            //audioInputStack.GestureRecognizers.Add(audioTapGestureRecognizer);

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
                IProgressBar progress = DependencyService.Get<IProgressBar>();
                try
                {
                    progress.ShowProgressbar("Fetching contacts..");
                    List<string> conatctList = new List<string>();
                    List<Contact> plugInContacts = new List<Contact>();

                    if( Device.OS == TargetPlatform.Android )
                    {
                        PurposeColor.interfaces.IDeviceContacts contacts = DependencyService.Get<PurposeColor.interfaces.IDeviceContacts>();
                        conatctList = await contacts.GetContacts();
                    }
                    else
                    {
						if(await CrossContacts.Current.RequestPermission())
						{
							await Task.Run(() =>
								{
									CrossContacts.Current.PreferContactAggregation = false;

									if (CrossContacts.Current.Contacts == null)
										return;


									plugInContacts = CrossContacts.Current.Contacts
										.Where(c => !string.IsNullOrWhiteSpace(c.FirstName) && c.Phones.Count > 0)
										.ToList();

									conatctList = plugInContacts.Select(item => item.FirstName).ToList();
								});
						}
                      
                    }


                    ////////////////////////// for testing only// test //////////////////////////
                    if (conatctList == null) // for testing only// test
                    {
                        conatctList = new List<string>();
                        conatctList.Add("Sam");
                        conatctList.Add("Tom");
                    }
                    ////////////////////////// for testing only// test //////////////////////////

                    if (conatctList != null && conatctList.Count > 0)
                    {
                        List<CustomListViewItem> contactsListViewSource = new List<CustomListViewItem>();
                        foreach (var item in conatctList)
                        {
                            if (item != null)
                                contactsListViewSource.Add(new CustomListViewItem { Name = item });
                        }

                        System.Collections.Generic.List<CustomListViewItem> pickerSource = contactsListViewSource;
                        CustomPicker ePicker = new CustomPicker(masterLayout, pickerSource, 65, "Select Contact", true, false);
                        ePicker.WidthRequest = screenWidth;
                        ePicker.HeightRequest = screenHeight;
                        ePicker.ClassId = "ePicker";
                        ePicker.listView.ItemSelected += OnContactsPickerItemSelected;
                        masterLayout.AddChildToLayout(ePicker, 0, 0);
                    }
                    else
                    {
                        DisplayAlert("Purpose Color", "Could not read contacts.", "Ok");
                    }


                    progress.HideProgressbar();

                    #region CONTACTS BK UP

                    /*	if (await CrossContacts.Current.RequestPermission())
                        {
                            try 
                            {	        
                                CrossContacts.Current.PreferContactAggregation = false;

                                if (CrossContacts.Current.Contacts == null)
                                {
                                    return;
                                }

                                List<Contact> contactSource = new List<Contact>();

                                contactSource = CrossContacts.Current.Contacts.Where( name => name.DisplayName != null ).ToList();
                                contacts = new List<CustomListViewItem>();
                                foreach (var item in contactSource)
                                {
								
                                    try {
                                        if( item != null && item.DisplayName != null)
                                            contacts.Add(new CustomListViewItem { Name = item.DisplayName});
                                    } catch (Exception ex) {
									
                                    }
								
                                }

                                contacts = contacts.OrderBy(c => c.Name).ToList();

                                System.Collections.Generic.List<CustomListViewItem> pickerSource = contacts;
                                CustomPicker ePicker = new CustomPicker(masterLayout, pickerSource, 65, "Select Contact", true, false);
                                ePicker.WidthRequest = deviceSpec.ScreenWidth;
                                ePicker.HeightRequest = deviceSpec.ScreenHeight;
                                ePicker.ClassId = "ePicker";
                                ePicker.listView.ItemSelected += OnContactsPickerItemSelected;
                                masterLayout.AddChildToLayout(ePicker, 0, 0);
                            }
                            catch (Exception ex)
                            {
                                DisplayAlert( "",ex.Message, "ok" );
                            }
                        }
                        else
                        {
                            DisplayAlert("contacts access permission ", "Please add permission to access contacts", "ok");
                        }*/

                    #endregion
                }
                catch (Exception ex)
                {
                    DisplayAlert("contactsInputTapRecognizer: ", ex.Message, "ok");
                }
                progress.HideProgressbar();
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
            if (pageTitle == Constants.ADD_ACTIONS || pageTitle == Constants.ADD_GOALS)
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
				previewListView.SelectedItem = null;
			};
            listContainer.Children.Add(previewListView);
            masterLayout.AddChildToLayout(listContainer, 5, Device.OnPlatform( 63, 63, 50 ));
            #endregion

			//masterLayout.AddChildToLayout(locationEditStack, 5, 30 );
            #endregion

            Content = masterLayout;
        }


		private async Task<bool> ApplyAnimation( StackLayout layout )
		{
			await layout.TranslateTo(0, -30, 250, Easing.CubicOut);
			await layout.TranslateTo(0, 0, 250, Easing.CubicOut);
			return true;
		}

        void OnLocationEditCompleted(object sender, EventArgs e)
        {
			if (editLocationAndContactsStack.ClassId == "locationedit")
			{
				locationInfo.Text = locAndContactsEntry.Text;
				locationInfo.IsVisible = true;
			}
			else
			{
				contactInfo.Text = locAndContactsEntry.Text;
				contactInfo.IsVisible = true;
			}
			editLocationAndContactsStack.IsVisible = false;
			iconContainerGrid.IsVisible = true;
        }



        async void OnEditLocationInfo(object sender, EventArgs e)
		{

			editLocationAndContactsStack.ClassId = "locationedit";
			locAndContactsEntry.Text = locationInfo.Text;
			editLocationAndContactsStack.IsVisible = true;
			locationInfo.IsVisible = false;
			iconContainerGrid.IsVisible = false;
            contactInfo.IsVisible = true;

            await editLocationAndContactsStack.TranslateTo(100, 0, 300, Easing.SinInOut);
            await editLocationAndContactsStack.TranslateTo(0, 0, 300, Easing.SinIn);
        }

        void RecodeOnTapRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                IProgressBar progress = DependencyService.Get<IProgressBar>();
                if (!isAudioRecording)
                {
                    audioRecodeOffHolder.IsVisible = true;
                    audioRecodeOnHolder.IsVisible = false;
                    isAudioRecording = true;
                    Device.StartTimer(TimeSpan.FromSeconds(60), () =>
                    {
                        RecodeOffTapRecognizer_Tapped(audioRecodeOnHolder, null);
                        progress.ShowToast("Maximum duration has reached.");
                        return false;
                    });

                    if (!audioRecorder.RecordAudio())
                    {
                        progress.ShowToast("Audio cannot be recorded, please try again later.");
                    }
                    else
                    {
                        progress.ShowToast("Audio recording started.");
                    }
                }
            }
            catch (System.Exception)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Audio cannot be recorded, please try again later.", Constants.ALERT_OK);
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
            Navigation.PushAsync(new CreateEventPage());
        }

        void OnEndDateCalanderClicked(object sender, EventArgs e)
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

        void OnEndCalendarViewDateSelected(object sender, DateTime e)
        {
            endDateLabel.Text = "Ends : " + e.Day.ToString() + "-" + e.Month.ToString() + "-" + e.Year.ToString();
            View view = masterLayout.Children.First(child => child.ClassId == "endcalander");
            if (view != null)
            {
                masterLayout.Children.Remove(view);
                view = null;
            }
        }

        void OnStartDateCalanderClicked(object sender, EventArgs e)
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

        void OnStartDateCalendarViewDateSelected(object sender, DateTime e)
        {
            startDateLabel.Text = "Starts : " + e.Day.ToString() + "-" + e.Month.ToString() + "-" + e.Year.ToString();
            View view = masterLayout.Children.First(child => child.ClassId == "startcalander");
            if (view != null)
            {
                masterLayout.Children.Remove(view);
                view = null;
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
            CustomListViewItem item =(CustomListViewItem) e.SelectedItem;

          /*  if (item != null && eventDescription.Text != null && eventDescription.Text.Contains(currentAddress))
             {
                 eventDescription.Text = eventDescription.Text.Replace(currentAddress, "");
             }

            string formattedString = eventDescription.Text;
            if( formattedString != null )
            {
                eventDescription.Text.Replace("@", "");
            }
            eventDescription.Text = formattedString + item.Name;

             currentAddress =  item.Name;*/

            locationInfo.Text = "";
            locationInfo.Text = "  @" + item.Name;
			locLayout.IsVisible = true;
             View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
             masterLayout.Children.Remove(pickView);
             pickView = null;
        }

        private void OnContactsPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var obj = e.SelectedItem as CustomListViewItem;
            string name = (e.SelectedItem as CustomListViewItem).Name;
            if (!string.IsNullOrEmpty(name))
            {
                int nIndex = 0;
                string preText = "  With ";
                selectedContact = name;
				if (string.IsNullOrEmpty (contactInfo.Text))
                {
					contactInfo.Text = preText;
                }

				if (contactInfo.Text != "  With ") {
					contactInfo.Text = contactInfo.Text + "," + selectedContact;
				} 
				else 
                {
					contactInfo.Text = contactInfo.Text +  selectedContact;
                }

				if (contactInfo.Text.Length > 40) 
				{
					contactInfo.Text = contactInfo.Text.Substring(0, 40);
					contactInfo.Text += "...";
				}
					
				contactInfo.IsVisible = true;
                App.ContactsArray.Add(name);

            }

            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
            pickView = null;
        }


        public void DisplayAlert( string messege )
        {
            DisplayAlert(Constants.ALERT_TITLE, messege, Constants.ALERT_OK);
        }

        async void NextButtonTapRecognizer_Tapped(object sender, System.EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(eventDescription.Text) || string.IsNullOrWhiteSpace(eventTitle.Text))
            {
                DisplayAlert(pageTitle, "value cannot be empty", "ok");
            }
            else
            {
                string input = pageTitle;
                CustomListViewItem item = new CustomListViewItem { Name = eventDescription.Text };


                if (input == Constants.ADD_ACTIONS)
                {
                    IProgressBar progress = DependencyService.Get<IProgressBar>();
                    progress.ShowProgressbar("Creating new action..");

                    try
                    {


                        ActionModel details = new ActionModel();
                        details.action_title = eventTitle.Text;
                        details.action_details = eventDescription.Text;
                        details.user_id = "2";
                        details.location_latitude = lattitude;
                        details.location_longitude = longitude;

                        details.start_date = DateTime.Now.ToString("yyyy/MM/dd"); // for testing only
                        details.end_date = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd"); // for testing only
                        details.start_time = DateTime.Now.AddHours(1).ToString("HH:mm"); //for testing only
                        details.end_time = DateTime.Now.AddHours(2).ToString("HH:mm"); //for testing only
                        details.action_repeat = "0";
                        details.action_alert = "0";

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
                            }
                            catch (System.Exception)
                            {
                                DisplayAlert(Constants.ALERT_TITLE, "Error in retrieving goals list, Please try again", Constants.ALERT_OK);
                            }
                        }

                        ILocalNotification notfiy = DependencyService.Get<ILocalNotification>();
                        notfiy.ShowNotification("Purpose Color - Action Created", eventTitle.Text);

                    }
                    catch (Exception ex)
                    {
                        var test = ex.Message;
                        progress.HideProgressbar();
                    }

                    progress.HideProgressbar();
                }
                else if (input == Constants.ADD_EVENTS)
                {
                    try
                    {

                        EventDetails details = new EventDetails();
                        details.event_title = eventTitle.Text;
                        details.event_details = eventDescription.Text;
                        details.user_id = "2";
                        details.location_latitude = lattitude;
                        details.location_longitude = longitude;
                        if (!string.IsNullOrEmpty(currentAddress))
                        {
                            details.location_address = currentAddress;
                        }

                        IProgressBar progress = DependencyService.Get<IProgressBar>();
                        progress.ShowProgressbar("Creating new event..");
                        if (!await ServiceHelper.AddEvent(details))
                        {
                            await DisplayAlert(Constants.ALERT_TITLE, Constants.NETWORK_ERROR_MSG, Constants.ALERT_OK);
                        }
                        await FeelingNowPage.DownloadAllEvents();

                        progress.HideProgressbar();

                    }
                    catch (Exception ex)
                    {
                        var test = ex.Message;
                    }
                }
                else if (input == Constants.ADD_GOALS)
                {
                    try
                    {
                        GoalDetails newGoal = new GoalDetails();
                        //EventDetails newGoal = new EventDetails();
                        newGoal.goal_title = eventTitle.Text;
                        newGoal.goal_details = eventDescription.Text;
                        newGoal.user_id = "2"; // for testing only // test
                        newGoal.location_latitude = lattitude;
                        newGoal.location_longitude = longitude;
                        newGoal.category_id = "1";
                        newGoal.start_date = DateTime.Now.ToString("yyyy/MM/dd"); // for testing only
                        newGoal.end_date = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd"); // for testing onl

                        if (!string.IsNullOrEmpty(currentAddress))
                        {
                            newGoal.location_address = currentAddress;
                        }
                        IProgressBar progress = DependencyService.Get<IProgressBar>();
                        progress.ShowProgressbar("Creating new goal..");
                        if (!await ServiceHelper.AddGoal(newGoal))
                        {
                            await DisplayAlert(Constants.ALERT_TITLE, Constants.NETWORK_ERROR_MSG, Constants.ALERT_OK);
                        }
                        else
                        {
                            try
                            {
                                var goals = await ServiceHelper.GetAllGoals(2); //for testing only
                                if (goals != null)
                                {
                                    App.goalsListSource = null;
                                    App.goalsListSource = new List<CustomListViewItem>();
                                    foreach (var goal in goals)
                                    {
                                        App.goalsListSource.Add(goal);
                                    }
                                }
                            }
                            catch (System.Exception)
                            {
                                DisplayAlert(Constants.ALERT_TITLE, "Error in retrieving goals list, Please try again", Constants.ALERT_OK);
                            }
                        }

                        progress.HideProgressbar();
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert(Constants.ALERT_TITLE, ex.Message, Constants.ALERT_OK);
                    }
                }

                Navigation.PopAsync();

            }
        }

        void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        public void AddFileToMediaArray(MemoryStream ms, string path, PurposeColor.Constants.MediaType mediaType)
        {
            try
            {
                MediaPost mediaWeb = new MediaPost();
                mediaWeb.event_details = string.IsNullOrWhiteSpace(eventDescription.Text) ? string.Empty : eventDescription.Text;
                mediaWeb.event_title = string.IsNullOrWhiteSpace(eventTitle.Text) ? string.Empty : eventTitle.Text;
                mediaWeb.user_id = 2;

                string imgType = System.IO.Path.GetExtension(path);
                string fileName = System.IO.Path.GetFileName(path);

                if (mediaType == Constants.MediaType.Image)
                {
                    App.PreviewListSource.Add(new PreviewItem { Name = fileName, Image = Device.OnPlatform("image.png", "image.png", "//Assets//image.png") });//Device.OnPlatform("delete_button.png", "delete_button.png", "//Assets//delete_button.png");
                }
                else if (mediaType == Constants.MediaType.Video)
                {
                    App.PreviewListSource.Add(new PreviewItem { Name = fileName, Image = Device.OnPlatform("video.png", "video.png", "//Assets//video.png") });
                }
                else
                {
                    App.PreviewListSource.Add(new PreviewItem { Name = fileName, Image = Device.OnPlatform("mic.png", "mic.png", "//Assets//mic.png") });
                }

                imgType = imgType.Replace(".", "");
                if (mediaType == Constants.MediaType.Image)
                {
                    MemoryStream compressedStream = new MemoryStream();
                    IResize resize = DependencyService.Get<IResize>();
                    compressedStream = resize.CompessImage(50, ms);

                    Byte[] inArray = compressedStream.ToArray();
                    Char[] outArray = new Char[(int)(compressedStream.ToArray().Length * 1.34)];
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
                    App.MediaArray.Add(item);

                    inArray = null;
                    outArray = null;
                    test2 = null;
                    item = null;
                    GC.Collect();
                }
                imgType = string.Empty;
                fileName = string.Empty;

                StackLayout preview = (StackLayout)masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "preview");
                masterLayout.Children.Remove(preview);
                preview = null;
                previewListView.ItemsSource = null;
                previewListView.ItemsSource = App.PreviewListSource;
                masterLayout.AddChildToLayout(listContainer, 5, 60);
            }
            catch (Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Unable to add the media", Constants.ALERT_OK);
                var test = ex.Message;
            }
        }


        public  async  void ShowAlert( string messege, PreviewItem toDelete )
        {
            var alert = await DisplayAlert(Constants.ALERT_TITLE, messege, Constants.ALERT_OK, "Cancel");
            if( alert  )
            {
                App.PreviewListSource.Remove(toDelete);
                MediaItem media = App.MediaArray.FirstOrDefault(med => med.Name == toDelete.Name);
                if (media != null)
                    App.MediaArray.Remove(media);
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
                mediaWeb.user_id = 2;

                string imgType = System.IO.Path.GetExtension(path);
                string fileName = System.IO.Path.GetFileName(path);

                App.PreviewListSource.Add(new PreviewItem { Name = fileName, Image = Device.OnPlatform("video.png", "video.png", "//Assets//video.png") });

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
                GC.Collect();
                
                imgType = string.Empty;
                fileName = string.Empty;

                StackLayout preview = (StackLayout)masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "preview");
                masterLayout.Children.Remove(preview);
                preview = null;
                previewListView.ItemsSource = null;
                previewListView.ItemsSource = App.PreviewListSource;
                masterLayout.AddChildToLayout(listContainer, 5, 60);
            }
            catch (Exception ex)
            {
                //DisplayAlert(Constants.ALERT_TITLE, "Unable to add the media", Constants.ALERT_OK);
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

            IProgressBar progres = DependencyService.Get<IProgressBar>();

			if(  Device.OS != TargetPlatform.iOS )
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


                        MemoryStream ms = new MemoryStream();
                        file.GetStream().CopyTo(ms);
                        ms.Position = 0;

                        MasterObject.AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Image);
                    }
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
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
                }
                progres.HideProgressbar();
            }


            View mediaChooserView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "mediachooser");
            PageContainer.Children.Remove(mediaChooserView);
            mediaChooserView = null;

            progres.HideProgressbar();

        }

        async void OnVideoButtonClicked(object sender, EventArgs e)
        {
            IProgressBar progres = DependencyService.Get<IProgressBar>();
			if(  Device.OS != TargetPlatform.iOS )
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
                        var file = await CrossMedia.Current.TakeVideoAsync(new Media.Plugin.Abstractions.StoreVideoOptions
                        {
                            Name = fileName,
                            Directory = "DefaultVideos",
                        });

                        if (file == null)
                        {
                            progres.HideProgressbar();
                            return;
                        }


                        MemoryStream ms = new MemoryStream();
                        file.GetStream().CopyTo(ms);
                        ms.Position = 0;

                        if (ms.Length > 15728640)
                        {
                            MasterObject.DisplayAlert("Can not add video, Maximum file size limied to 15 MB");
                            progres.HideProgressbar();
                            ms = null;
                            file = null;
                            return;
                        }

                        MasterObject.AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Video);
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
                    if (!CrossMedia.Current.IsPickVideoSupported)
                    {
                        progres.HideProgressbar();
                        return;
                    }
                    var file = await CrossMedia.Current.PickVideoAsync();

                    if (file == null)
                    {
                        progres.HideProgressbar();
                        return;
                    }

                    MemoryStream ms = new MemoryStream();
                    file.GetStream().CopyTo(ms);
                    ms.Position = 0;

                    if (ms.Length > 15728640)
                    {
                        MasterObject.DisplayAlert("Can not add video, Maximum file size limied to 15 MB");
                        progres.HideProgressbar();
                        ms = null;
                        file = null;
                        return;
                    }

                    MasterObject.AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Video);

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
