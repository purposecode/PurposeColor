using System;
using Xamarin.Forms;
using System.Collections.Generic;
using CustomControls;
using Cross;
using PurposeColor.CustomControls;
using ImageCircle.Forms.Plugin.Abstractions;
using PurposeColor.Service;
using PurposeColor.Model;
using PurposeColor.interfaces;
using System.Collections.ObjectModel;
using System.IO;

namespace PurposeColor
{
	public class ContactDetails
	{
		public string Name { get; set; }
		public string Image{ get; set; }
		public bool IsOnline{ get; set; }
	}

	public class ChatPage : ContentPage, IDisposable
	{
		PurposeColorTitleBar mainTitleBar;
		CommunityGemSubTitleBar subTitleBar;
		CustomLayout masterLayout;
		ListView chatContactsListView;
		ChatObject userObject;
		IProgressBar progressBar;

		public ChatPage ()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			progressBar = DependencyService.Get< IProgressBar > ();
			mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += (object sender, EventArgs e) => 
			{
				App.masterPage.IsPresented = true;
			};
			subTitleBar = new CommunityGemSubTitleBar(Constants.SUB_TITLE_BG_COLOR, Constants.COMMUNITY_GEMS, true);
			subTitleBar.BackButtonTapRecognizer.Tapped += async (object sender, EventArgs e) => 
			{
				await Navigation.PopAsync();
			};

			this.Appearing += OnChatPageAppearing;
			masterLayout = new CustomLayout ();
			masterLayout.WidthRequest = App.screenWidth;
			masterLayout.HeightRequest = App.screenHeight;


			chatContactsListView = new ListView();
			chatContactsListView.ItemTemplate = new DataTemplate(typeof(ChatContactListCell));
			chatContactsListView.SeparatorVisibility = SeparatorVisibility.Default;
			chatContactsListView.BackgroundColor = Color.White;
			chatContactsListView.HeightRequest = App.screenHeight * 90 / 100;
			chatContactsListView.HasUnevenRows = true;
			//chatContactsListView.RowHeight = (int) App.screenHeight * 10 / 100;
			chatContactsListView.SeparatorColor = Color.FromRgb (8, 135, 224);
			chatContactsListView.ItemSelected +=  async (object sender, SelectedItemChangedEventArgs e) => 
			{
				string curentUserId = App.Settings.GetUser().UserId.ToString();
				ObservableCollection<ChatDetails> chats = new ObservableCollection<ChatDetails>();
				ChatUsersInfo selItem = chatContactsListView.SelectedItem as ChatUsersInfo;
				if( selItem != null )
				{
					ChatHistoryObject history = await ServiceHelper.GetChatHistory ( selItem.user_id, curentUserId );

					if (history != null)
					{
						foreach (var item in history.resultarray) 
						{
							ChatDetails detail = new ChatDetails ();
							detail.CurrentUserid = curentUserId;
							detail.FromUserID = item.from_id;
							detail.Message = item.msg;
							chats.Add ( detail );
						}
					}

					await Navigation.PushAsync( new ChatDetailsPage( chats, selItem.user_id, selItem.profileImgUrl,  selItem.firstname ) );
				}
				chatContactsListView.SelectedItem= null;
			};


			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
			masterLayout.AddChildToLayout ( chatContactsListView, 0, 10 );

			Content = masterLayout;
		}


		async void OnChatPageAppearing (object sender, EventArgs e)
		{
			if (userObject != null)
				return;

			progressBar.ShowProgressbar ( "Loading users list...." );
			userObject = await ServiceHelper.GetAllChatUsers ();
			if (userObject != null)
			{
				App.chatList = userObject;
				List<string> profileImageUrlList = new List<string> ();

				foreach ( var item in userObject.resultarray ) 
				{
					string profileUrl = Constants.SERVICE_BASE_URL + item.profileimage;
					profileImageUrlList.Add ( profileUrl );		
				}

				IDownload downloader = DependencyService.Get<IDownload> ();
				downloader.DownloadFiles ( profileImageUrlList );
				chatContactsListView.ItemsSource = userObject.resultarray;
			}

			progressBar.HideProgressbar ();
		}

		protected override bool OnBackButtonPressed ()
		{
			Dispose ();
			return base.OnBackButtonPressed ();
		}

		public void Dispose ()
		{
			mainTitleBar = null;
			subTitleBar = null;
			masterLayout = null;
			chatContactsListView = null;
			userObject = null;
			this.Content = null;
		}
	}



	public class ChatContactListCell : ViewCell
	{
		public ChatContactListCell()
		{
			StackLayout mainLayout = new StackLayout ();
			mainLayout.Orientation = StackOrientation.Horizontal;
			mainLayout.WidthRequest = App.screenWidth;
			mainLayout.HeightRequest = App.screenHeight * 8 / 100;
			mainLayout.BackgroundColor = Color.White;
			mainLayout.Padding = new Thickness (0, 10, 10, 10);
			mainLayout.Spacing = 10;

			Label userName = new Label ();
			userName.TextColor = Color.Black;
			userName.XAlign = TextAlignment.Start;
			userName.YAlign = TextAlignment.Center;
			userName.VerticalOptions = LayoutOptions.Center;
			userName.FontSize = 15;
			userName.HorizontalOptions = LayoutOptions.CenterAndExpand;
			userName.SetBinding ( Label.TextProperty, "firstname" );


			Image userImage = new Image 
			{
				Aspect = Aspect.Fill,
				WidthRequest = 50,
				HeightRequest = 50,
				HorizontalOptions = LayoutOptions.Center
			};
			if (Device.OS == TargetPlatform.iOS) {
				userImage.SetBinding (Image.SourceProperty, "profilImageWebUrl");
			}
			else 
			{
				userImage.SetBinding ( Image.SourceProperty, "profileImgUrl" );				
				//userImage.SetBinding (Image.SourceProperty, "profilImageWebUrl");
			}
			userImage.TranslationX = 20;
			/*Image userImage = new Image ();
			userImage.WidthRequest = App.screenWidth * 10 / 100;
			userImage.HeightRequest = App.screenWidth * 10 / 100;
			userImage.Source = "avatar.jpg";
			userImage.Aspect = Aspect.Fill;
			userImage.VerticalOptions = LayoutOptions.Center;
			userImage.HorizontalOptions = LayoutOptions.Start;*/

			Image statusImg = new Image ();
			statusImg.WidthRequest = App.screenWidth * 5 / 100;
			statusImg.HeightRequest = App.screenWidth * 5 / 100;
			statusImg.Source = "online.png";
			statusImg.Aspect = Aspect.Fill;
			statusImg.VerticalOptions = LayoutOptions.Center;
			statusImg.HorizontalOptions = LayoutOptions.EndAndExpand;

			BoxView availabelStatus = new BoxView ();
			availabelStatus.BackgroundColor = Color.Green;
			availabelStatus.WidthRequest = 10;
			availabelStatus.HeightRequest = 5;
			availabelStatus.VerticalOptions = LayoutOptions.Center;
			availabelStatus.HorizontalOptions = LayoutOptions.EndAndExpand;


			mainLayout.Children.Add ( userImage );
			mainLayout.Children.Add ( userName );
			mainLayout.Children.Add ( availabelStatus );

			View = mainLayout;

		}


	}
}


