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

namespace PurposeColor.screens
{
    public class ProfileSettingsPage : ContentPage, IDisposable
    {
        CustomEntry statusEntry = null;
		int userIdForProfileInfo = -1;
		User userInfo = null;

		public ProfileSettingsPage(int userId = 0)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
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


			if (userId != 0) {
				userIdForProfileInfo = userId;
				// get user info from service.

			} else {
				User user = App.Settings.GetUser ();


				// for testing only. //test
				user = new User {
					DisplayName = "Sam Fernando Disusa",
					Age = 25,
					Email = "sam@testmail.com",
					StatusNote = "Feeling hungry.. :-P",
					ProfileImageUrl = Constants.SERVICE_BASE_URL + "admin/uploads/default/noprofile.png"
				};
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
			}

			Image profilePic = new Image {
				Source = userInfo.ProfileImageUrl,
				HeightRequest = 100,
				WidthRequest = 100,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.End
			};
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
				Spacing = 5,
				Children = {userDisplayName, emailLabel}
			};
			masterLayout.AddChildToLayout(nameEmailStack,10, 40);
			masterLayout.AddChildToLayout(statusEntry, 10, 50);

			statusEntry.Completed += StatusEntry_Completed;

            Content = masterLayout;
        }

        async void StatusEntry_Completed (object sender, EventArgs e)
        {
			try {
				string newStatus = (sender as CustomEntry).Text;

				if (!string.IsNullOrEmpty(newStatus)) {
					if(newStatus.Trim() != userInfo.StatusNote.Trim())
					{
						string responceCode = await PurposeColor.Service.ServiceHelper.SendProfilePicAndStatusNote(newStatus,string.Empty,string.Empty);
						if (responceCode == null|| responceCode != "200") {
							await DisplayAlert(Constants.ALERT_TITLE,"Network error, Please try again later. " + responceCode, Constants.ALERT_OK); // responceCode fro testing // test
						}else
						{
							PurposeColor.interfaces.IProgressBar progressBar = DependencyService.Get<PurposeColor.interfaces.IProgressBar>();
							progressBar.ShowToast("Status updated.");
						}
					}
				}
			} catch (Exception ex) {
				
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
}
