using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using System.IO;
using PurposeColor.interfaces;
using Media.Plugin;

namespace PurposeColor.screens
{
	public class ProfileSettingsPage : ContentPage, IDisposable
	{
		CustomEntry statusEntry = null;
		int userIdForProfileInfo = -1;
		User userInfo = null;
		CustomLayout masterLayout = null;
		TapGestureRecognizer galleryInputStackTapRecognizer = null;
		StackLayout galleryInputStack = null;
		Image galleryInput = null;
		int ICON_SIZE = 8;
		public Image profilePic = null;
		public PurposeColor.interfaces.IProgressBar progressBar = null;

		public ProfileSettingsPage(int userId = 0)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;

			PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "       Profile Info", false, true);

			subTitleBar.BackButtonTapRecognizer.Tapped += (s, e) =>
			{
				try
				{
					Navigation.PopAsync();
				}
				catch (Exception ex)
				{
				}
			};
			subTitleBar.NextButtonTapRecognizer.Tapped += SubTitleBar_NextButtonTapRecognizer_Tapped;

			PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);

			galleryInput = new Image()
			{
				Source = Device.OnPlatform("icn_gallery.png", "icn_gallery.png", "//Assets//icn_gallery.png"),
				Aspect = Aspect.AspectFit
			};

			if (Device.OS == TargetPlatform.WinPhone)
			{
				galleryInput.WidthRequest = App.screenWidth * ICON_SIZE / 100;
				galleryInput.HeightRequest = App.screenWidth * ICON_SIZE / 100;
			}
			galleryInputStack = new StackLayout
			{
				Padding = new Thickness(5, 10, 5, 10),
				Spacing = 0,
				Children = { galleryInput
				}
			};
			galleryInputStack.ClassId = "gallery";

			galleryInputStackTapRecognizer = new TapGestureRecognizer();
			galleryInputStack.GestureRecognizers.Add(galleryInputStackTapRecognizer);
			galleryInputStackTapRecognizer.Tapped += async (se, e) =>
			{
				try {
					StackLayout send = se as StackLayout;
					SourceChooser chooser = new SourceChooser(masterLayout, send.ClassId, this);
					chooser.ClassId = "mediachooser";
					masterLayout.AddChildToLayout(chooser, 0, 0);

				} catch (Exception ex) {
					var test = ex.Message;
				}

			};


			if (userId != 0) {
				userIdForProfileInfo = userId;
				// get user info from service.

			} else {
				User user = App.Settings.GetUser ();

				// for testing only. //test
				if (user == null)
				{
					user = new User {
						DisplayName = "Sam Fernando Disusa",
						Age = 25,
						Email = "sam@testmail.com",
						StatusNote = "Feeling hungry.. :-P",
						ProfileImageUrl = Constants.SERVICE_BASE_URL + "admin/uploads/default/noprofile.png"
					};
				}
				// for testing only// test

				userInfo = user;
			}
			//////// for testing only //

			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(subTitleBar, 0, 10);

			statusEntry = new CustomEntry
			{
				Placeholder = "Status note",
				HeightRequest = Device.OnPlatform(50, 50, 75),
				BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY,
				TextColor = Color.Black,
				WidthRequest = App.screenWidth * 80 / 100,
			};
			if (userInfo.StatusNote != null) {
				statusEntry.Text = userInfo.StatusNote.Trim ();
			} else {
				statusEntry.Text = "Please provide your status message..";
			}

			profilePic = new Image {
				Source = Constants.SERVICE_BASE_URL + userInfo.ProfileImageUrl,
				HeightRequest = 110,
				WidthRequest = 100,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.End
			};

			TapGestureRecognizer imageTapRecognizer = new TapGestureRecognizer ();
			imageTapRecognizer.Tapped += ImageTapRecognizer_Tapped;
			profilePic.GestureRecognizers.Add (imageTapRecognizer);
			profilePic.ClassId = "camera";
			masterLayout.AddChildToLayout(profilePic, 35, 20);

			Label userDisplayName = new Label {
				Text = !string.IsNullOrEmpty (userInfo.DisplayName) ? userInfo.DisplayName : (!string.IsNullOrEmpty (userInfo.UserName) ? userInfo.UserName : ""),
				WidthRequest = App.screenWidth * 0.80,
				HorizontalOptions = LayoutOptions.Center,
				XAlign = TextAlignment.Center,
				VerticalOptions = LayoutOptions.Center,
				FontSize = App.screenDensity >= 2 ? 20 : 15,
				TextColor = Color.Black,
				FontAttributes = FontAttributes.Bold
			};
			//masterLayout.AddChildToLayout(userDisplayName,40, 28);

			Label emailLabel = new Label {
				Text = !string.IsNullOrEmpty (userInfo.Email) ? userInfo.Email : "",
				WidthRequest = App.screenWidth * 0.50,
				HorizontalOptions = LayoutOptions.Center,
				XAlign = TextAlignment.Center,
				VerticalOptions = LayoutOptions.Center,
				FontSize = App.screenDensity >= 2 ? 12 : 10,
				TextColor = Color.Gray,
				FontAttributes = FontAttributes.None

			};
			//masterLayout.AddChildToLayout(emailLabel, 40, 40);

			StackLayout nameEmailStack = new StackLayout {
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 3,
				Children = {userDisplayName, emailLabel}
			};


			statusEntry.Completed += StatusEntry_Completed;

			progressBar = DependencyService.Get<PurposeColor.interfaces.IProgressBar>();

			if (userInfo.VerifiedStatus != 0) {
				// display verified icon over name.
				Image verifiedBadge = new Image {
					Source = "verified_icon.png",
					HeightRequest = App.screenHeight * .05, //.04, = if near name   //.06, = if near profile image 
					WidthRequest = App.screenWidth * .05,//.04, = if near name // //.06, = if near profile image 
					HorizontalOptions = LayoutOptions.Center,
					BackgroundColor = Color.Transparent
				};

				//  aligned to name.
//				masterLayout.AddChildToLayout(verifiedBadge, 51, 39); //  aligned to name.
//				if (emailLabel.Text != null) 
//				{
//					if (userDisplayName.Text.Length < 33)
//						verifiedBadge.TranslationX = userDisplayName.Text.Length * 4;
//					else
//						verifiedBadge.TranslationX = 100;
//				}

				//  aligned to profile pic.
				int xpos= 62;
				if (App.screenDensity > 2) {
					xpos = 64;
					masterLayout.AddChildToLayout (nameEmailStack, 10, 40);
					masterLayout.AddChildToLayout (statusEntry, 10, 50);
				} else {
					masterLayout.AddChildToLayout(nameEmailStack,10, 42);
					masterLayout.AddChildToLayout(statusEntry, 10, 52);
				}
				masterLayout.AddChildToLayout(verifiedBadge, xpos, 18); //  aligned to profile pic.

			}


			Content = masterLayout;
		}

		async void ImageTapRecognizer_Tapped (object sender, EventArgs e)
		{
			try {
				// show image selection options.

				string id = (sender as Image).ClassId;
				SourceChooser chooser = new SourceChooser(masterLayout, id, this);
				chooser.ClassId = "mediachooser";
				masterLayout.AddChildToLayout(chooser, 0, 0);

				//				User user = App.Settings.GetUser();
				//				if(user != null && profilePic != null)
				//				{
				//
				//					if(System.IO.Path.GetFileName(user.ProfileImageUrl)!= null)
				//					{
				//						profilePic.Source = user.ProfileImageUrl;
				//					}
				//				}

			} catch (Exception ex) {
				DisplayAlert("Camera", ex.Message + " Please try again later", "ok");
			}
		}

		async void StatusEntry_Completed (object sender, EventArgs e)
		{
			try {

				string newStatus = (sender as CustomEntry).Text;

				if (!string.IsNullOrEmpty(newStatus)) {
					if(newStatus.Trim() != userInfo.StatusNote.Trim())
					{
						progressBar.ShowProgressbar("Saving your settings");
						string responceCode = await PurposeColor.Service.ServiceHelper.SendProfilePicAndStatusNote(newStatus,null,string.Empty);
						if (responceCode == null|| responceCode != "200") {
							await DisplayAlert(Constants.ALERT_TITLE,"Network error, Please try again later. " + responceCode, Constants.ALERT_OK); // responceCode fro testing // test
						}else
						{

							User user = App.Settings.GetUser();
							user.StatusNote = newStatus;
							App.Settings.SaveUser(user);
							progressBar.HideProgressbar();
							progressBar.HideProgressbar();
							progressBar.ShowToast("Status updated.");
						}
					}
				}
			} catch (Exception ex) {
				progressBar.HideProgressbar();
				progressBar.ShowToast("Network error, Please try again later");
			}
		}

		void SubTitleBar_NextButtonTapRecognizer_Tapped (object sender, EventArgs e)
		{
			// chk if any values changes if so save changes.
		}

		void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
		{
			App.masterPage.IsPresented = !App.masterPage.IsPresented;
		}

		public void Dispose()
		{
			this.statusEntry = null;
			GC.Collect();
		}

	}



	public class SourceChooser : ContentView
	{
		CustomLayout PageContainer;
		ProfileSettingsPage callerObject = null;

		public SourceChooser(CustomLayout pageContainer, string type, ProfileSettingsPage parentObject)
		{
			CustomLayout masterLayout = new CustomLayout();
			masterLayout.BackgroundColor = Color.Transparent;
			//IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
			double screenWidth = App.screenWidth;
			double screenHeight = App.screenHeight;
			PageContainer = pageContainer;

			callerObject = parentObject;

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
			imageButton.ImageName = Device.OnPlatform("photoCamera_icon.png", "photoCamera_icon.png", @"/Assets/photoCamera_icon.png");
			imageButton.WidthRequest = screenWidth * 20 / 100;
			imageButton.HeightRequest = screenHeight * 10 / 100;
			imageButton.ClassId = type;
			imageButton.Clicked += OnImageButtonClicked;

			CustomImageButton selectFromGalleryButton = new CustomImageButton();
			selectFromGalleryButton.ImageName = Device.OnPlatform("image.png", "image.png", @"/Assets/image.png");
			selectFromGalleryButton.WidthRequest = screenWidth * 20 / 100;
			selectFromGalleryButton.HeightRequest = screenHeight * 10 / 100;
			selectFromGalleryButton.ClassId = type;
			selectFromGalleryButton.Clicked += SelectFromGalleryButtonClicked;

			masterLayout.AddChildToLayout(layout, 0, 0);
			masterLayout.AddChildToLayout(imageButton, 40, 40);
			masterLayout.AddChildToLayout(selectFromGalleryButton, 40, 60);

			this.BackgroundColor = Color.Transparent;



			Content = masterLayout;
		}

		async void OnImageButtonClicked(object sender, EventArgs e)
		{
			try
			{


				if (Device.OS != TargetPlatform.iOS)
					callerObject.progressBar.ShowProgressbar("Saving your settings..");

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
							callerObject.progressBar.HideProgressbar();
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

						AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Image);
					}
				}
				catch (Exception ex)
				{
					var test = ex.Message;
					callerObject.progressBar.HideProgressbar();
				}


				View mediaChooserView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "mediachooser");
				PageContainer.Children.Remove(mediaChooserView);
				mediaChooserView = null;

				callerObject.progressBar.HideProgressbar();

			}
			catch (Exception ex)
			{
				var test = ex.Message;
			}

		}

		async void SelectFromGalleryButtonClicked(object sender, EventArgs e)
		{
			try
			{


				if (Device.OS != TargetPlatform.iOS)
					callerObject.progressBar.ShowProgressbar("Saving your settings..");

				try
				{

					if (!CrossMedia.Current.IsPickPhotoSupported)
					{
						callerObject.progressBar.HideProgressbar();
						return;
					}

					var file = await CrossMedia.Current.PickPhotoAsync();

					if (file == null)
					{
						callerObject.progressBar.HideProgressbar();
						return;
					}


					MemoryStream ms = new MemoryStream();
					file.GetStream().CopyTo(ms);
					ms.Position = 0;

					AddFileToMediaArray(ms, file.Path, PurposeColor.Constants.MediaType.Image);

				}
				catch (Exception ex)
				{
					var test = ex.Message;
					callerObject.progressBar.HideProgressbar();
				}


				View pickView = PageContainer.Children.FirstOrDefault(pick => pick.ClassId == "mediachooser");
				PageContainer.Children.Remove(pickView);
				pickView = null;
				callerObject.progressBar.HideProgressbar();

				GC.Collect();

			}
			catch (Exception ex)
			{
				var test = ex.Message;
			}
		}

		async public void AddFileToMediaArray(MemoryStream ms, string path, PurposeColor.Constants.MediaType mediaType)
		{
			try
			{
				MediaPost mediaWeb = new MediaPost();

				string imgType = System.IO.Path.GetExtension(path);
				string fileName = System.IO.Path.GetFileName(path);
				MediaItem mediaItem = new MediaItem();

				#region MEDIA COMPRESSION AND SERIALISING

				if (ms != null)
				{
					imgType = imgType.Replace(".", "");
					if (mediaType == Constants.MediaType.Image)
					{
						MemoryStream compressedStream = new MemoryStream();
						IResize resize = DependencyService.Get<IResize>();
						Byte[] resizedOutput = resize.Resize(ms.ToArray(), (float)(App.screenWidth * App.screenDensity), (float)(App.screenHeight * App.screenDensity), path);
						if(resizedOutput== null)
							return;

						MemoryStream resizedStream = new MemoryStream(resizedOutput);
						compressedStream = resize.CompessImage(25, resizedStream);


						var path2 = System.IO.Path.Combine(App.DownloadsPath, fileName);

						Byte[] inArray = compressedStream.ToArray();
						Char[] outArray = new Char[(int)(compressedStream.ToArray().Length * 1.34)];
						Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
						string test2 = new string(outArray);
						App.ExtentionArray.Add(imgType);

						mediaItem.MediaString = test2;
						mediaItem.Name = fileName;
						App.MediaArray.Add(mediaItem);

						inArray = null;
						outArray = null;
						test2 = null;

						resizedOutput = null;
						GC.Collect();
					}
					//
					//					imgType = string.Empty;
					//					fileName = string.Empty;

				}

				#endregion

				if(mediaItem != null)
				{
					string serviceResult = await PurposeColor.Service.ServiceHelper.SendProfilePicAndStatusNote(string.Empty, mediaItem, imgType);
					if (serviceResult != null  ) 
					{
						if(callerObject != null)
						{
							callerObject.profilePic.Source = Constants.SERVICE_BASE_URL + serviceResult;
						}

						User user = App.Settings.GetUser();
						if(user != null)
						{
							user.ProfileImageUrl = serviceResult;
							App.Settings.SaveUser(user);
						}
					}
				}

				if (callerObject != null) {
					callerObject.progressBar.HideProgressbar();
				}

				mediaItem = null;
			}
			catch (Exception ex)
			{
				var test = ex.Message;
			}
		}
	}
}
