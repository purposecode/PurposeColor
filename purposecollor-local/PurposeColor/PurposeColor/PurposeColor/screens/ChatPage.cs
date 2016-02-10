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
		ChatUsersObject userObject;
		IProgressBar progressBar;

		public ChatPage ()
		{
			progressBar = DependencyService.Get< IProgressBar > ();
		    mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
		    subTitleBar = new CommunityGemSubTitleBar(Constants.SUB_TITLE_BG_COLOR, Constants.COMMUNITY_GEMS, true);

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
			chatContactsListView.ItemSelected +=  (object sender, SelectedItemChangedEventArgs e) => 
			{
				ObservableCollection<string> chathistory = new ObservableCollection<string>();
				chathistory.Add( "Hi" );
				chathistory.Add( "Hello" );
				chathistory.Add( "how are you" );
				chathistory.Add( "from where. am from delhi, india . am from delhi, india djg  am from delhi, india am from delhi, india ? from where. am from delhi, india . am from delhi, india djg  am from delhi, india am from delhi, india ? from where. am from delhi, india . am from delhi, india djg  am from delhi, india am from delhi, india ?" );
				chathistory.Add( "wht abt u" );chathistory.Add( "you are" );
				chathistory.Add( "from where. am from delhi, india . am from delhi, india djg  am from delhi, india am from delhi, india ?" );
				chathistory.Add( "am from delhi, india" );chathistory.Add( "so how s " );
				chathistory.Add( "life there ?" );
				chathistory.Add( "not going good" );
				chathistory.Add( "ok..." );
				 Navigation.PushAsync( new ChatDetailsPage( chathistory ) );
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
			chatContactsListView.ItemsSource = userObject.resultarray;
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

			CircleImage userImage = new CircleImage 
			{
				Aspect = Aspect.AspectFit,
				WidthRequest = 75,
				HeightRequest = 75,
				HorizontalOptions = LayoutOptions.Start
			};
			userImage.SetBinding ( CircleImage.SourceProperty, "profileImgUrl" );
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


