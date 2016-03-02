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
using PurposeColor.Service;
using Media.Plugin.Abstractions;

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
		ProfileDetails userProfile = null;
		PurposeColorTitleBar mainTitleBar = null;
		PurposeColorSubTitleBar subTitleBar = null;

		Label CommunitySharingLabel= null;
		Image communityShareIcon = null;
		StackLayout communityStatusBtn = null;
		User currentUser = null;
		TapGestureRecognizer communityShareTap = null;
		TapGestureRecognizer followIconTap = null;

		Image allowFollowIcon = null;
		StackLayout followStatusBtn = null;

		public ProfileSettingsPage(int userId = 0)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
			subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "       Profile Info", false, true);
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

			mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);

			try {
				currentUser = App.Settings.GetUser ();
			} catch (Exception ex) {
				
			}

			if (userId != 0) {
				userIdForProfileInfo = userId;
				// get user info from service.
			} 
			else 
			{
				if (currentUser == null) {
					return;
				}
				else
				{
					userInfo = currentUser;
				}
			}

			this.Appearing += ProfileSettingsPage_Appearing;

		}

		async void  ProfileSettingsPage_Appearing (object sender, EventArgs e)
		{
			if(userIdForProfileInfo > 0)
			{
				await GetProfileInfo ();
			}

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
			galleryInputStackTapRecognizer.Tapped += async (se, ee) =>
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

			profilePic = new Image {
				Source = Constants.SERVICE_BASE_URL + (!string.IsNullOrEmpty(userInfo.ProfileImageUrl)? userInfo.ProfileImageUrl : "admin/uploads/default/noprofile.png"),
				HeightRequest = 110,
				WidthRequest = 100,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.End
			};

			if (userIdForProfileInfo <= 0 || userIdForProfileInfo.ToString () == currentUser.UserId)  // display info of current logged in user.
			{
				#region Profile image tap
				TapGestureRecognizer profileImageTapRecognizer = new TapGestureRecognizer ();
				profileImageTapRecognizer.Tapped += ProfileImage_Tapped;
				profilePic.GestureRecognizers.Add (profileImageTapRecognizer);
				#endregion

				#region Ststus entry
				if (userInfo.StatusNote != null) {
					statusEntry.Text = userInfo.StatusNote.Trim ();
				} else {
					statusEntry.Text = "Please provide your status message..";
				}
				statusEntry.Completed += StatusEntry_Completed;
				#endregion

				#region Community Sharing
				CommunitySharingLabel = new Label {
					Text = "Share to community",
					FontSize = App.screenDensity >= 2 ? 15 : 12,
					TextColor = Color.Gray,
				};

				communityShareIcon = new Image ();

				communityStatusBtn = new StackLayout ();
				communityStatusBtn.WidthRequest = 50;
				communityStatusBtn.HeightRequest = 50;
				communityStatusBtn.BackgroundColor = Color.Transparent;
				communityStatusBtn.VerticalOptions = LayoutOptions.Center;

				//communityShareIcon.IsEnabled = false;
				if (currentUser.AllowCommunitySharing) {
					communityShareIcon.Source = Device.OnPlatform ("tic_active.png", "tic_active.png", "//Assets//tic_active.png");
				} else {
					communityShareIcon.Source = Device.OnPlatform ("tick_box.png", "tick_box.png", "//Assets//tick_box.png");
				}
				communityShareIcon.Aspect = Aspect.AspectFill;
				communityShareIcon.WidthRequest = 20;
				communityShareIcon.HeightRequest = 20;
				communityShareIcon.ClassId = "Cshare";
				communityShareIcon.HorizontalOptions = LayoutOptions.Center;
				communityShareIcon.VerticalOptions = LayoutOptions.End;
				//communityShareIcon.TranslationY = 15;
				communityStatusBtn.Children.Add (communityShareIcon);

				communityShareTap = new TapGestureRecognizer ();
				communityShareTap.Tapped += UpdateShareToCommunityStatus;
				communityStatusBtn.GestureRecognizers.Add (communityShareTap);

				masterLayout.AddChildToLayout (CommunitySharingLabel, 10, 65);
				masterLayout.AddChildToLayout (communityStatusBtn, Device.OnPlatform(55,50,50), 65);
				#endregion

				#region Allow Follow

				Label followLabel = new Label {
					Text = "Allow others to follow",
					FontSize = App.screenDensity >= 2 ? 15 : 12,
					TextColor = Color.Gray,
				};

				allowFollowIcon = new Image ();
				if (currentUser.AllowFollowers) {
					allowFollowIcon.Source = Device.OnPlatform ("tic_active.png", "tic_active.png", "//Assets//tic_active.png");
				} else {
					allowFollowIcon.Source = Device.OnPlatform ("tick_box.png", "tick_box.png", "//Assets//tick_box.png");
				}
				allowFollowIcon.Aspect = Aspect.AspectFill;
				allowFollowIcon.WidthRequest = 20;
				allowFollowIcon.HeightRequest = 20;
				allowFollowIcon.HorizontalOptions = LayoutOptions.Center;
				allowFollowIcon.VerticalOptions = LayoutOptions.End;

				followStatusBtn = new StackLayout ();
				followStatusBtn.WidthRequest = 50;
				followStatusBtn.HeightRequest = 50;
				followStatusBtn.BackgroundColor = Color.Transparent;
				followStatusBtn.VerticalOptions = LayoutOptions.Center;
				followStatusBtn.Children.Add (allowFollowIcon);
				followIconTap = new TapGestureRecognizer ();
				followIconTap.Tapped += UpdateFollowStatus;
				followStatusBtn.GestureRecognizers.Add (followIconTap);

				masterLayout.AddChildToLayout (followLabel, 10, 70);
				masterLayout.AddChildToLayout (followStatusBtn, Device.OnPlatform(55,50,50), 70);
				#endregion

			}
			else 
			{
				statusEntry.IsEnabled = false;
				if (userInfo.StatusNote != null) {
					statusEntry.Text = userInfo.StatusNote.Trim ();
				} else {
					statusEntry.IsVisible = false;
				}
			}

			profilePic.ClassId = "camera";
			masterLayout.AddChildToLayout(profilePic, 35, 20);

			Label userDisplayName = new Label {
				Text = !string.IsNullOrEmpty (userInfo.DisplayName) ? userInfo.DisplayName : (!string.IsNullOrEmpty (userInfo.UserName) ? userInfo.UserName : ""),
				//WidthRequest = App.screenWidth * 0.80,
				HorizontalOptions = LayoutOptions.Center,
				XAlign = TextAlignment.Center,
				VerticalOptions = LayoutOptions.Center,
				FontSize = App.screenDensity >= 2 ? 20 : 15,
				TextColor = Color.Black,
				FontAttributes = FontAttributes.Bold
			};

			Label emailLabel = new Label {
				Text = !string.IsNullOrEmpty (userInfo.Email) ? userInfo.Email : "",
				//WidthRequest = App.screenWidth * 0.50,
				HorizontalOptions = LayoutOptions.Center,
				XAlign = TextAlignment.Center,
				VerticalOptions = LayoutOptions.Center,
				FontSize = App.screenDensity >= 2 ? 12 : 10,
				TextColor = Color.Gray,
				FontAttributes = FontAttributes.None

			};

			Image verifiedBadge = new Image {
				Source = "verified_icon.png",
				HeightRequest = App.screenHeight * .05,
				WidthRequest = App.screenWidth * .05,
				HorizontalOptions = LayoutOptions.Start,
				BackgroundColor = Color.Transparent,
				IsVisible = userInfo.VerifiedStatus != 0
			};

			StackLayout nameEmailStack = new StackLayout {
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = App.screenWidth * 0.80,
				Spacing = 3,
				Children = {new StackLayout{ Spacing = 2,Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center, 
						Children = {userDisplayName, verifiedBadge}}, 
						emailLabel}
			};

			progressBar = DependencyService.Get<PurposeColor.interfaces.IProgressBar>();
			int xpos = 62;
			if (App.screenDensity > 2) {
				xpos = 64;
				masterLayout.AddChildToLayout (nameEmailStack, 10, 40);
				masterLayout.AddChildToLayout (statusEntry, 10, 50);
			} else {
				masterLayout.AddChildToLayout(nameEmailStack,10, 42);
				masterLayout.AddChildToLayout(statusEntry, 10, 52);
			}

			Content = masterLayout;
		}

		async void UpdateFollowStatus (object sender, EventArgs e)
		{
			try {
				if(currentUser == null)
				{
					return;
				}
				//
				progressBar.ShowProgressbar("updating status");

				string newAllowFollowersStatus = "1";

				if (currentUser.AllowFollowers) 
				{
					newAllowFollowersStatus = "0";
				}

				string responce = await ServiceHelper.UpdateShreAndFollowStatus(currentUser.UserId, string.Empty, newAllowFollowersStatus);

				if(responce == null || responce != "200")
				{
					progressBar.HideProgressbar();
					progressBar.ShowToast("Network error, please try again later");
				}
				else
				{
					currentUser.AllowFollowers= !currentUser.AllowFollowers;

					// update icon 
					if (currentUser.AllowFollowers) {
						allowFollowIcon.Source = Device.OnPlatform ("tic_active.png", "tic_active.png", "//Assets//tic_active.png");
					} else {
						allowFollowIcon.Source = Device.OnPlatform("tick_box.png", "tick_box.png", "//Assets//tick_box.png");
					}
					await App.Settings.SaveUser(currentUser);

					progressBar.HideProgressbar();
					progressBar.ShowToast("Status updated");
				}

			} catch (Exception ex) {
				progressBar.HideProgressbar();
			}
		}

		async void UpdateShareToCommunityStatus(object sender, EventArgs e)
		{
			try {
				if(currentUser == null)
				{
					return;
				}
				communityShareTap.Tapped -= UpdateShareToCommunityStatus;
				progressBar.ShowProgressbar("updating status");

				string newCommunitySharingStatus = "1";

				if (currentUser.AllowCommunitySharing) 
				{
					newCommunitySharingStatus = "0";
				}

				string responce = await ServiceHelper.UpdateShreAndFollowStatus(currentUser.UserId, newCommunitySharingStatus,string.Empty);

				if(responce == null || responce != "200")
				{
					progressBar.HideProgressbar();
					progressBar.ShowToast("Network error, please try again later");
				}
				else
				{
					currentUser.AllowCommunitySharing = !currentUser.AllowCommunitySharing;


					// update icon 
					if (currentUser.AllowCommunitySharing) {
						communityShareIcon.Source = Device.OnPlatform ("tic_active.png", "tic_active.png", "//Assets//tic_active.png");
					} else {
						communityShareIcon.Source = Device.OnPlatform("tick_box.png", "tick_box.png", "//Assets//tick_box.png");
					}
					await App.Settings.SaveUser(currentUser);

					progressBar.HideProgressbar();
					progressBar.ShowToast("Status updated");
				}

			} catch (Exception ex) {
				progressBar.HideProgressbar();
			}
			communityShareTap.Tapped += UpdateShareToCommunityStatus;
		}

		async System.Threading.Tasks.Task<bool> GetProfileInfo()
		{

			try {
				userProfile = await ServiceHelper.GetProfileInfoByUserId (userIdForProfileInfo);
				if (userProfile != null) {
					userInfo = new User ();
					userInfo.UserName = userProfile.firstname;
					userInfo.StatusNote = userProfile.note;
					userInfo.VerifiedStatus = userProfile.verified_status;
					userInfo.Email = userProfile.email;
					userInfo.ProfileImageUrl = userProfile.profileurl;
				}
				return true;
			} catch (Exception ex) {

			}
			return false;
		}

		async void ProfileImage_Tapped (object sender, EventArgs e)
		{
			try {
				// show image selection options.

				string id = (sender as Image).ClassId;
				SourceChooser chooser = new SourceChooser(masterLayout, id, this);
				chooser.ClassId = "mediachooser";
				masterLayout.AddChildToLayout(chooser, 0, 0);
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
						}
						else
						{
							currentUser.StatusNote = newStatus;
							App.Settings.SaveUser(currentUser);
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

		void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
		{
			App.masterPage.IsPresented = !App.masterPage.IsPresented;
		}

		public void Dispose()
		{
			this.statusEntry = null;
			userInfo = null;
			masterLayout = null;
			galleryInputStackTapRecognizer = null;
			galleryInputStack = null;
			galleryInput = null;
			profilePic = null;
			progressBar = null;
			userProfile = null;
			mainTitleBar = null;
			subTitleBar = null;
			CommunitySharingLabel= null;
			communityShareIcon = null;
			communityStatusBtn = null;
			User currentUser = null;

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
								Name = fileName,
								DefaultCamera = CameraDevice.Front
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
				//callerObject.progressBar.HideProgressbar();

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
							await App.Settings.SaveUser(user);
						}
					}
				}

				mediaItem = null;
				if (callerObject != null) {
					callerObject.progressBar.HideProgressbar();
				}
			}
			catch (Exception ex)
			{
				var test = ex.Message;
			}
		}
	}
}
