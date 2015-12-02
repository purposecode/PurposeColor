using Contacts.Plugin;
using Contacts.Plugin.Abstractions;
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

namespace PurposeColor.screens
{
	public class AddEventsSituationsOrThoughts : ContentPage, System.IDisposable
	{
		#region MEMBERS

		CustomLayout masterLayout;
		StackLayout TopTitleBar;
		PurposeColorBlueSubTitleBar subTitleBar;
		CustomEditor eventDescription;
        CustomEntry eventTitle;
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
		StackLayout iconContainer;
		StackLayout textinputAndIconsHolder;
		TapGestureRecognizer CameraTapRecognizer;
		string pageTitle;
		bool isAudioRecording = false;
		PurposeColor.interfaces.IAudioRecorder audioRecorder;
		string folder;
		string path;
		List<CustomListViewItem> contacts;
        IDeviceSpec deviceSpec;
        string lattitude;
        string longitude;
        string currentAddress;
        string selectedContact;
        StackLayout listContainer;
        ListView previewListView;
		#endregion

		public AddEventsSituationsOrThoughts(string title)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			audioRecorder = DependencyService.Get<PurposeColor.interfaces.IAudioRecorder>();
		    deviceSpec = DependencyService.Get<IDeviceSpec>();
			masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
			pageTitle = title;
            lattitude = string.Empty;
            longitude = string.Empty;
            currentAddress = string.Empty;
			int devWidth = (int)deviceSpec.ScreenWidth;
            App.MediaArray = new List<MediaItem>();
            App.ContactsArray = new List<string>();
            

			#region TITLE BARS
			TopTitleBar = new StackLayout
			{
				BackgroundColor = Constants.BLUE_BG_COLOR,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Padding = 0,
				Spacing = 0,
				Children = { new BoxView { WidthRequest = deviceSpec.ScreenWidth } }
			};
			masterLayout.AddChildToLayout(TopTitleBar, 0, 0);

			string trimmedPageTitle = string.Empty;
			if (title.Length > 20)
			{
				trimmedPageTitle = title.Substring(0, 20);
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

            eventTitle = new CustomEntry
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				BackgroundColor = Color.White,
				Placeholder = "Title",
				TextColor = Color.FromHex("#424646"),
				WidthRequest = (int)(devWidth * .92) // 92% of screen
					//  FontSize = Device.OnPlatform( 20, 22, 30 ),
					//  FontFamily = Constants.HELVERTICA_NEUE_LT_STD
			};

			masterLayout.AddChildToLayout(eventTitle, 3, 11);

			#region TEXT INPUT CONTROL

			eventDescription = new CustomEditor
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				HeightRequest = 150,
				Placeholder = pageTitle
			};

			//string input = pageTitle;
			//if (input == Constants.ADD_ACTIONS)
			//{
			//    textInput.Text = "Add supporting actions";
			//}
			//else if (input == Constants.ADD_EVENTS)
			//{
			//    textInput.Text = "Add Events";
			//}
			//else if (input == Constants.ADD_GOALS)
			//{
			//    textInput.Text = "Add Goals";
			//}

			textInputContainer = new StackLayout
			{
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				Padding = 1,
				Spacing = 0,
				Children = { eventDescription }
			};

			//int devWidth = (int)deviceSpec.ScreenWidth;
			int textInputWidth = (int)(devWidth * .92); // 92% of screen
			eventDescription.WidthRequest = textInputWidth;

			#endregion

			#region ICONS

			audioInput = new Image()
			{
				Source = Device.OnPlatform("ic_music.png", "ic_music.png", "//Assets//ic_music.png"),
				Aspect = Aspect.AspectFit
			};
			audioInputStack = new StackLayout
			{
				Padding = 10,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Children = { audioInput, new Label { Text = "Audio", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
			};

			cameraInput = new Image()
			{
				Source = Device.OnPlatform("icn_camera.png", "icn_camera.png", "//Assets//icn_camera.png"),
				Aspect = Aspect.AspectFit
			};
			cameraInputStack = new StackLayout
			{
				Padding = 10,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Children = { cameraInput, new Label { Text = "Camera", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
			};
            cameraInputStack.ClassId = "camera";

			#region CAMERA TAP RECOGNIZER
			CameraTapRecognizer = new TapGestureRecognizer();
			cameraInputStack.GestureRecognizers.Add(CameraTapRecognizer);
			CameraTapRecognizer.Tapped += async (s, e) =>
			{
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
			};

			#endregion

			#region AUDIO TAP RECOGNIZER

			TapGestureRecognizer audioTapGestureRecognizer = new TapGestureRecognizer();

			audioTapGestureRecognizer.Tapped += (s, e) =>
			{
				try
				{
					if (!isAudioRecording)
					{
						isAudioRecording = true;
						if (!audioRecorder.RecordAudio())
						{
							DisplayAlert("Audio recording", "Audio cannot be recorded, please try again later.", "ok");
						}
						else
						{
							DisplayAlert("Audio recording", "Audio recording started, Tap the audio icon again to end.", "ok");
						}
					}
					else
					{
						isAudioRecording = false;
						MemoryStream stream = audioRecorder.StopRecording();


                        AddFileToMediaArray(stream, audioRecorder.AudioPath, Constants.MediaType.Audio);

					}
				}
				catch (System.Exception)
				{
					DisplayAlert("Audio recording", "Audio cannot be recorded, please try again later.", "ok");
				}
			};
			audioInputStack.GestureRecognizers.Add(audioTapGestureRecognizer);

			#endregion

			galleryInput = new Image()
			{
				Source = Device.OnPlatform("icn_gallery.png", "icn_gallery.png", "//Assets//icn_gallery.png"),
				Aspect = Aspect.AspectFit
			};
			galleryInputStack = new StackLayout
			{
				Padding = 10,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Children = { galleryInput, new Label { Text = "Gallery", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
			};
            galleryInputStack.ClassId = "gallery";

			#region GALLERY  TAP RECOGNIZER

			TapGestureRecognizer galleryInputStackTapRecognizer = new TapGestureRecognizer();
			galleryInputStack.GestureRecognizers.Add(galleryInputStackTapRecognizer);
			galleryInputStackTapRecognizer.Tapped += async (s, e) =>
			{
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
			locationInputStack = new StackLayout
			{
				Padding = new Thickness(Device.OnPlatform(10,10,14),10,Device.OnPlatform(10,10,14),10),
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Children = { locationInput, new Label { Text = "Location", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
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

			contactInputStack = new StackLayout
			{
				Padding = 10,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				Spacing = 0,
				HorizontalOptions = LayoutOptions.Center,
				Children = { contactInput, new Label { Text = "Contact", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
			};

			#region CONTACTS TAP RECOGNIZER

			TapGestureRecognizer contactsInputTapRecognizer = new TapGestureRecognizer();
			contactInputStack.GestureRecognizers.Add(contactsInputTapRecognizer);
			contactsInputTapRecognizer.Tapped += async (s, e) =>
			{
                IProgressBar progress = DependencyService.Get<IProgressBar>();
				try
				{

                    
                    progress.ShowProgressbar("Fetching contacts..");
                    List<string> conatctList = new List<string>();
                    PurposeColor.interfaces.IDeviceContacts contacts = DependencyService.Get<PurposeColor.interfaces.IDeviceContacts>();
                    conatctList = await contacts.GetContacts();
                    if (conatctList == null)
                    {
                        conatctList = new List<string>();
                    }
                    
                    
                    conatctList.Add("Sam");
                    conatctList.Add("Tom");
                    

                    if( conatctList != null && conatctList.Count > 0 )
                    {
                        List<CustomListViewItem> contactsListViewSource = new List<CustomListViewItem>();
                        foreach (var item in conatctList)
                        {
                            if (item != null)
                                contactsListViewSource.Add(new CustomListViewItem { Name = item });
                        }

                        System.Collections.Generic.List<CustomListViewItem> pickerSource = contactsListViewSource;
                        CustomPicker ePicker = new CustomPicker(masterLayout, pickerSource, 65, "Select Contact", true, false);
                        ePicker.WidthRequest = deviceSpec.ScreenWidth;
                        ePicker.HeightRequest = deviceSpec.ScreenHeight;
                        ePicker.ClassId = "ePicker";
                        ePicker.listView.ItemSelected += OnContactsPickerItemSelected;
                        masterLayout.AddChildToLayout(ePicker, 0, 0);
                    }
                    else
                    {
                        DisplayAlert("Purpose Color", "No contacts to display.", "Ok");
                    }


                    progress.HideProgressbar();
                
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
				}
				catch (Exception ex)
				{
					DisplayAlert("contactsInputTapRecognizer: ", ex.Message,"ok");
				}
                progress.HideProgressbar();
			};

			#endregion

			#endregion

			#region CONTAINERS

			iconContainer = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = (int)(devWidth * .92) + 2, /// 2 pxl padding added to text input.
				Spacing = Device.OnPlatform((deviceSpec.ScreenWidth * 4.5 / 100),(deviceSpec.ScreenWidth * 4.5 / 100),(deviceSpec.ScreenWidth * 3.8 / 100)),
				Padding = 0,
				Children = { galleryInputStack, cameraInputStack, audioInputStack, locationInputStack, contactInputStack }
			};

			textinputAndIconsHolder = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Padding = 0,
				Children = { textInputContainer, iconContainer }
			};

			int iconY = (int)eventDescription.Y + (int)eventDescription.Height + 5;
			masterLayout.AddChildToLayout(textinputAndIconsHolder, 3, 21);


            #region PREVIEW LIST
            listContainer = new StackLayout();
            listContainer.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            listContainer.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            listContainer.HeightRequest = deviceSpec.ScreenHeight * 30 / 100;
            listContainer.ClassId = "preview";

            previewListView = new ListView();
            previewListView.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            previewListView.ItemTemplate = new DataTemplate(typeof(PreviewListViewCellItem));
            previewListView.SeparatorVisibility = SeparatorVisibility.None;
            previewListView.Opacity = 1;
            previewListView.ItemsSource = App.PreviewListSource;
            listContainer.Children.Add( previewListView );
            masterLayout.AddChildToLayout(listContainer, 5, 60);
            #endregion

			#endregion

			Content = masterLayout;
		}

		async void LocationInputTapRecognizer_Tapped (object sender, EventArgs e)
		{
            IProgressBar progress = DependencyService.Get<IProgressBar>();
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                if (!locator.IsGeolocationEnabled)
                {
                    DisplayAlert("Purpose Color", "Please turn ON location services", "Ok");
                    return;
                }


                progress.ShowProgressbar("Getting Location..");

                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                try
                {
                    locator.StartListening(1, 100);
                
                if (position == null)
                {
                    return;
                }
                App.Lattitude = position.Latitude;
                App.Longitude = position.Longitude;
                lattitude = position.Latitude.ToString() != null ? position.Latitude.ToString(): string.Empty;
                longitude = position.Longitude.ToString() != null ? position.Longitude.ToString(): string.Empty;
                locator.StopListening();

                }
                catch (Exception)
                {
                    DisplayAlert("Location error", "Location could not be retrived", "OK");
                }
                ILocation loc = DependencyService.Get<ILocation>();
                try
                {
                    var address = await loc.GetLocation(position.Latitude, position.Longitude);
                    currentAddress = address;
                    if (currentAddress != null && eventDescription.Text != null && eventDescription.Text.Contains(currentAddress))
                    {
                        eventDescription.Text = eventDescription.Text.Replace(currentAddress, "");
                    }
                    eventDescription.Text = eventDescription.Text + address;
                }
                catch (Exception)
                {
                    DisplayAlert("Location error", "Address could not be retrived", "OK");
                }
                
                progress.HideProgressbar();
            }
            catch (Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Location service failed.", Constants.ALERT_OK);
                progress.HideProgressbar();
            }
			
		}

      
        void imageButton_Clicked(object sender, EventArgs e)
        {
            
        }

		private void OnContactsPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var obj = e.SelectedItem as CustomListViewItem;
			string name = (e.SelectedItem as CustomListViewItem).Name;
			if (!string.IsNullOrEmpty(name))
			{
				int nIndex = 0;
				string preText = " with ";
                selectedContact = name;
				if (eventDescription.Text != null)
				{
					nIndex = eventDescription.Text.IndexOf(name);
					preText = eventDescription.Text.IndexOf("with") <= 0 ? " with " : ", ";
				}

				if (nIndex <= 0)
				{
					eventDescription.Text = eventDescription.Text + preText + name;
				}
                //App.ContactsArray = new List<string>();
                App.ContactsArray.Add(name);
                //App.ContactsArray.Add("Tom");

			}

			View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
			masterLayout.Children.Remove(pickView);
			pickView = null;
		}

		async void NextButtonTapRecognizer_Tapped(object sender, System.EventArgs e)
		{
          
			if (string.IsNullOrWhiteSpace(eventDescription.Text)|| string.IsNullOrWhiteSpace(eventTitle.Text))
			{
				DisplayAlert(pageTitle, "value cannot be empty", "ok");
			}
			else
			{
				string input = pageTitle;
				CustomListViewItem item = new CustomListViewItem { Name = eventDescription.Text };
				if (input == Constants.ADD_ACTIONS)
				{
                    IReminderService reminder = DependencyService.Get<IReminderService>();
                    reminder.Remind( DateTime.UtcNow.AddMinutes( 1 ), DateTime.UtcNow.AddMinutes( 2 ), "Purpose Color Event", eventTitle.Text );

                    ILocalNotification notfiy = DependencyService.Get<ILocalNotification>();
                    notfiy.ShowNotification("Purpose Color - Action Created", eventTitle.Text);
					//App.actionsListSource.Add(item);
				}
				else if (input == Constants.ADD_EVENTS)
				{
                    EventDetails details = new EventDetails();
                    details.event_title = eventTitle.Text;
                    details.event_details = eventDescription.Text;
                    details.user_id = "2";
                    details.location_latitude = lattitude;
                    details.location_longitude = longitude;

                    IProgressBar progress = DependencyService.Get<IProgressBar>();
                    progress.ShowProgressbar( "Creating Event.." );
                    if( !await ServiceHelper.AddEvent(details))
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, Constants.NETWORK_ERROR_MSG, Constants.ALERT_OK);
                    }

                    await FeelingNowPage.DownloadAllEvents();
                    progress.HideProgressbar();
					
				}
				else if (input == Constants.ADD_GOALS)
				{
                    try
                    {

                        EventDetails newGoal = new EventDetails();
                        newGoal.event_title = eventTitle.Text;
                        newGoal.event_details = eventDescription.Text;
                        newGoal.user_id = "2"; // for testing only // test
                        newGoal.location_latitude = lattitude;
                        newGoal.location_longitude = longitude;

                        IProgressBar progress = DependencyService.Get<IProgressBar>();
                        progress.ShowProgressbar("Creating new goal..");
                        if (!await ServiceHelper.AddGoal(newGoal))
                        {
                            await DisplayAlert(Constants.ALERT_TITLE, Constants.NETWORK_ERROR_MSG, Constants.ALERT_OK);
                        }

                        progress.HideProgressbar();


                        // for testing 
                        if (App.goalsListSource == null)
                        {
                            App.goalsListSource = new List<CustomListViewItem>();
                        }
                        App.goalsListSource.Add(item);
                        

                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Alert", ex.Message, Constants.ALERT_OK);
                    }
				}

				Navigation.PopAsync();

			}
		}

		void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
		{
			Navigation.PopAsync();
		}


        public void AddFileToMediaArray( MemoryStream ms, string path, PurposeColor.Constants.MediaType mediaType )
        {
            MediaPost mediaWeb = new MediaPost();
            mediaWeb.event_details = eventDescription.Text;
            mediaWeb.event_title = eventTitle.Text;
            mediaWeb.user_id = 2;

            string imgType = System.IO.Path.GetExtension(path);
            string fileName = System.IO.Path.GetFileName(path);

            if( mediaType == Constants.MediaType.Image )
            {
                App.PreviewListSource.Add(new PreviewItem { Name = fileName, Image = "image.png" });
            }
            else if (mediaType == Constants.MediaType.Video)
            {
                App.PreviewListSource.Add(new PreviewItem { Name = fileName, Image = "video.png" });
            }
            else
            {
                App.PreviewListSource.Add(new PreviewItem { Name = fileName, Image = "ic_music.png" });
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
                App.MediaArray.Add( item );
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
            }


         /*   StackLayout preview = (StackLayout)masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "preview");
            masterLayout.Children.Remove(preview);
            preview = null;
            previewListView.ItemsSource = null;
            previewListView.ItemsSource = App.PreviewListSource;
            masterLayout.AddChildToLayout(listContainer, 5, 60);*/
            
        }


		public void Dispose()
		{
			subTitleBar.BackButtonTapRecognizer.Tapped -= OnBackButtonTapRecognizerTapped;
			subTitleBar.NextButtonTapRecognizer.Tapped -= NextButtonTapRecognizer_Tapped;
			this.masterLayout = null;
			this.TopTitleBar = null;
			this.subTitleBar = null;
			this.eventDescription = null;
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
			this.iconContainer = null;
			this.textinputAndIconsHolder = null;
			this.audioRecorder = null;
			this.eventTitle = null;
			this.contacts = null;
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
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            PageContainer = pageContainer;

            StackLayout layout = new StackLayout();
            layout.BackgroundColor = Color.Black;
            layout.Opacity = .6;
            layout.WidthRequest = deviceSpec.ScreenWidth;
            layout.HeightRequest = deviceSpec.ScreenHeight;

            TapGestureRecognizer emptyAreaTapGestureRecognizer = new TapGestureRecognizer();
            emptyAreaTapGestureRecognizer.Tapped += (s, e) =>
            {

                View pickView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "mediachooser");
                PageContainer.Children.Remove(pickView);
                pickView = null;

            };
            layout.GestureRecognizers.Add(emptyAreaTapGestureRecognizer);



            CustomImageButton imageButton = new CustomImageButton();
            imageButton.ImageName = "image.png";
            imageButton.WidthRequest = deviceSpec.ScreenWidth * 20 / 100;
            imageButton.HeightRequest = deviceSpec.ScreenHeight * 10 / 100;
            imageButton.ClassId = type;
            imageButton.Clicked += OnImageButtonClicked;


            CustomImageButton videoButton = new CustomImageButton();
            videoButton.ImageName = "video.png";
            videoButton.WidthRequest = deviceSpec.ScreenWidth * 20 / 100;
            videoButton.HeightRequest = deviceSpec.ScreenHeight * 10 / 100;
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
            progres.ShowProgressbar("Preparing media..");
            if ((sender as CustomImageButton).ClassId == "camera")
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
            else if ((sender as CustomImageButton).ClassId == "gallery")
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


            View mediaChooserView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "mediachooser");
            PageContainer.Children.Remove(mediaChooserView);
            mediaChooserView = null;

            progres.HideProgressbar();

        }

        async void OnVideoButtonClicked(object sender, EventArgs e)
        {
            IProgressBar progres = DependencyService.Get<IProgressBar>();
            progres.ShowProgressbar("Preparing media..");
            if ((sender as CustomImageButton).ClassId == "camera")
            {
                if (Media.Plugin.CrossMedia.Current.IsCameraAvailable)
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

                    MasterObject.AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Video);
                }
            }
            else if ((sender as CustomImageButton).ClassId == "gallery")
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

                MasterObject.AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Video);
            }


            View pickView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "mediachooser");
            PageContainer.Children.Remove(pickView);
            pickView = null;
            progres.HideProgressbar();
        }
    }


    public class PreviewListViewCellItem : ViewCell
    {
        public PreviewListViewCellItem()
        {
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            Label name = new Label();
            name.SetBinding(Label.TextProperty, "Name");
            name.TextColor = Color.Black;
            name.FontSize = Device.OnPlatform(12, 13, 18);
            name.WidthRequest = deviceSpec.ScreenWidth * 50 / 100;

            StackLayout divider = new StackLayout();
            divider.WidthRequest = deviceSpec.ScreenWidth;
            divider.HeightRequest = .75;
            divider.BackgroundColor = Color.FromRgb(255, 255, 255);

            Image sideImage = new Image();
            sideImage.WidthRequest = 15;
            sideImage.HeightRequest = 15;
            sideImage.SetBinding(Image.SourceProperty, "Image");
            sideImage.Aspect = Aspect.Fill;

            CustomImageButton deleteButton = new CustomImageButton();
            deleteButton.ImageName = "delete_button.png";
            deleteButton.WidthRequest = 20;
            deleteButton.HeightRequest = 20;
            deleteButton.SetBinding( CustomImageButton.ClassIdProperty, "Name" );

            deleteButton.Clicked += ( sender,  e) =>
            {
                CustomImageButton button = sender as CustomImageButton;
                PreviewItem itemToDel = App.PreviewListSource.FirstOrDefault(item => item.Name == button.ClassId);
                if( itemToDel != null )
                {
                    App.PreviewListSource.Remove(itemToDel);
                    MediaItem media = App.MediaArray.FirstOrDefault( med => med.Name == itemToDel.Name );
                    if (media != null)
                        App.MediaArray.Remove( media );
                }

            };
          
            masterLayout.WidthRequest = deviceSpec.ScreenWidth;
            masterLayout.HeightRequest = deviceSpec.ScreenHeight * Device.OnPlatform(30, 50, 10) / 100;

            masterLayout.AddChildToLayout(sideImage, (float)5, (float)Device.OnPlatform(5, 5, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(name, (float)Device.OnPlatform(15, 15, 15), (float)Device.OnPlatform(5, 5, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(deleteButton, (float)80, (float)Device.OnPlatform(5, 3.5, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            // masterLayout.AddChildToLayout(divider, (float)1, (float)20, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            this.View = masterLayout;

        }

    

    }
}
