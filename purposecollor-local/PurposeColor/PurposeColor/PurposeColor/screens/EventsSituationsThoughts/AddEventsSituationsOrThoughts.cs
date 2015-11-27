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
		#endregion

		public AddEventsSituationsOrThoughts(string title)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			audioRecorder = DependencyService.Get<PurposeColor.interfaces.IAudioRecorder>();
		    deviceSpec = DependencyService.Get<IDeviceSpec>();
			masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
			pageTitle = title;
			int devWidth = (int)deviceSpec.ScreenWidth;

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

			#region CAMERA TAP RECOGNIZER
			CameraTapRecognizer = new TapGestureRecognizer();
			cameraInputStack.GestureRecognizers.Add(CameraTapRecognizer);
			CameraTapRecognizer.Tapped += async (s, e) =>
			{
				try
				{
					if (Media.Plugin.CrossMedia.Current.IsCameraAvailable)
					{

						string fileName = string.Format("Image{0}.png", System.DateTime.Now.ToString("yyyyMMddHHmmss"));

						var file = await Media.Plugin.CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
							{

								Directory = "Purposecolor",
								Name = fileName
							});

						//if (file == null)
						//{
						//    DisplayAlert("Alert", "Image could not be saved, please try again later", "ok");
						//}
					}
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
						audioRecorder.StopRecording();
						DisplayAlert("Audio recording", "Audio saved to gallery.", "ok");
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

			#region GALLERY  TAP RECOGNIZER

			TapGestureRecognizer galleryInputStackTapRecognizer = new TapGestureRecognizer();
			galleryInputStack.GestureRecognizers.Add(galleryInputStackTapRecognizer);
			galleryInputStackTapRecognizer.Tapped += async (s, e) =>
			{
				if (!CrossMedia.Current.IsPickPhotoSupported)
				{
					DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
					return;
				}

				var file = await CrossMedia.Current.PickPhotoAsync();

				MemoryStream ms = new MemoryStream();
				file.GetStream().CopyTo(ms);
                ms.Position = 0;

				MediaPost mediaWeb = new MediaPost();
				mediaWeb.event_details = eventDescription.Text;
				mediaWeb.event_title = eventTitle.Text;
				mediaWeb.user_id = 2;

                string imgType = System.IO.Path.GetExtension(file.Path);

                imgType = imgType.Replace(".", "");

                 MemoryStream compressedStream = new MemoryStream();
                 IResize resize = DependencyService.Get<IResize>();
                 compressedStream = resize.CompessImage(50, ms);

                Byte[] inArray = compressedStream.ToArray();
                Char[] outArray = new Char[(int)(compressedStream.ToArray().Length * 1.34)];
                Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
                string test2 = new string(outArray);

                App.ExtentionArray.Add(imgType);
                App.gallleryArray.Add(test2);

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
				try
				{

                    IProgressBar progress = DependencyService.Get<IProgressBar>();
                    progress.ShowProgressbar("Fetching contacts..");
                    List<string> conatctList = new List<string>();
                    PurposeColor.interfaces.IDeviceContacts contacts = DependencyService.Get<PurposeColor.interfaces.IDeviceContacts>();
                    conatctList = await contacts.GetContacts();

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

			#endregion

			Content = masterLayout;
		}

		async void LocationInputTapRecognizer_Tapped (object sender, EventArgs e)
		{
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			IProgressBar progress = DependencyService.Get<IProgressBar> ();

			if (!locator.IsGeolocationEnabled) 
			{
				DisplayAlert ("Purpose Color", "Please turn ON location services", "Ok");
				return;
			}

		
			progress.ShowProgressbar ( "Getting Location.." );

            locator.StartListening(1, 100);
			var position = await locator.GetPositionAsync (timeoutMilliseconds: 10000);
			App.Lattitude = position.Latitude;
			App.Longitude = position.Longitude;
            locator.StopListening();

			ILocation loc = DependencyService.Get<ILocation> ();
			var address = await loc.GetLocation ( position.Latitude, position.Longitude );
             if( App.CurrentAddress!= null && eventDescription.Text.Contains(App.CurrentAddress))
             {
                eventDescription.Text = eventDescription.Text.Replace(App.CurrentAddress, "");
             }
			eventDescription.Text = eventDescription.Text + address;
            App.CurrentAddress = address;
			progress.HideProgressbar ();
		}

		private void OnContactsPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var obj = e.SelectedItem as CustomListViewItem;
			string name = (e.SelectedItem as CustomListViewItem).Name;
			if (!string.IsNullOrEmpty(name))
			{
				int nIndex = 0;
				string preText = " with ";
				if (eventDescription.Text != null)
				{
					nIndex = eventDescription.Text.IndexOf(name);
					preText = eventDescription.Text.IndexOf("with") <= 0 ? " with " : ", ";
				}

				if (nIndex <= 0)
				{
					eventDescription.Text = eventDescription.Text + preText + name;
				}

			}

			View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
			masterLayout.Children.Remove(pickView);
			pickView = null;
		}

		async void NextButtonTapRecognizer_Tapped(object sender, System.EventArgs e)
		{

            IProgressBar progress = DependencyService.Get<IProgressBar>();
            try
            {

                progress.ShowProgressbar("Media uploading...");
                var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 20, 0);
                client.BaseAddress = new Uri(Constants.SERVICE_BASE_URL);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

                var url = "api.php?action=eventinsertarray";

                MultipartFormDataContent content = new MultipartFormDataContent();

                for (int index = 0; index < App.gallleryArray.Count; index++)
                {
                    int imgIndex = index + 1;
                    content.Add(new StringContent(App.gallleryArray[index], Encoding.UTF8), "event_media" + imgIndex.ToString());
                    content.Add(new StringContent(App.ExtentionArray[index], Encoding.UTF8), "file_type" + imgIndex.ToString());
                }


                /*  content.Add(new StringContent(App.gallleryArray[0] , Encoding.UTF8), "event_image1");
                  content.Add(new StringContent("jpeg", Encoding.UTF8), "file_type");

                  content.Add(new StringContent(App.gallleryArray[1], Encoding.UTF8), "event_image2");
                  content.Add(new StringContent("jpeg", Encoding.UTF8), "file_type");*/


                content.Add(new StringContent(App.gallleryArray.Count.ToString(), Encoding.UTF8), "event_image_count");
                content.Add(new StringContent("event_titleeeee", Encoding.UTF8), "event_title");
                content.Add(new StringContent("2", Encoding.UTF8), "user_id");

                content.Add(new StringContent("event_detailseeee", Encoding.UTF8), "event_details");
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var eventsJson = response.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<TestJSon>(eventsJson);

                    string test = "test";

                }

                progress.HideProgressbar();

            }
            catch (Exception ex)
            {
                progress.HideProgressbar();
                DisplayAlert(ex.Message, ex.Message, "ok");
            }





            return;
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
					UserEvent newEvent = new UserEvent();
					newEvent.EventName = eventDescription.Text;

					// save event to local db and send to API.
					//App.eventsListSource.Add(item);
				}
				else if (input == Constants.ADD_GOALS)
				{
					//App.goalsListSource.Add(item);
				}

				Navigation.PopAsync();
			}
		}

		void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
		{
			Navigation.PopAsync();
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
}
