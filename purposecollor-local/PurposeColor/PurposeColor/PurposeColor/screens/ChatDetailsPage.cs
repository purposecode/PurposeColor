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
	public class ChatDetailsPage : ContentPage, IDisposable
	{
		PurposeColorTitleBar mainTitleBar;
		CommunityGemSubTitleBar subTitleBar;
		CustomLayout masterLayout;
		IProgressBar progressBar;
		ListView chatHistoryListView;
		ObservableCollection<string> chatList = null;

		public ChatDetailsPage ( ObservableCollection<string> chats )
		{

			chatList = chats;

			progressBar = DependencyService.Get< IProgressBar > ();
			mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
			subTitleBar = new CommunityGemSubTitleBar(Constants.SUB_TITLE_BG_COLOR, Constants.COMMUNITY_GEMS, true);

			masterLayout = new CustomLayout ();
			masterLayout.WidthRequest = App.screenWidth;
			masterLayout.HeightRequest = App.screenHeight - 100;
			masterLayout.BackgroundColor = Color.FromRgb(45, 62, 80);

			chatHistoryListView = new ListView();
			chatHistoryListView.ItemTemplate = new DataTemplate(typeof(ChatHistoryListCell));
			chatHistoryListView.SeparatorVisibility = SeparatorVisibility.None;
			chatHistoryListView.HeightRequest = App.screenHeight * 70 / 100;
			chatHistoryListView.HasUnevenRows = true;
			chatHistoryListView.BackgroundColor = Color.FromRgb(54, 79, 120);
			chatHistoryListView.ItemsSource = chatList;


			CustomEditor chatEntry = new CustomEditor
			{
				Placeholder = "Enter your chat...",
				BackgroundColor = Color.White,//Color.White,
				WidthRequest = App.screenWidth * .80,
				HorizontalOptions = LayoutOptions.Start,
				Text = Device.OnPlatform(string.Empty, string.Empty, "Enter your chat...")

			};

			Image addCommentButton = new Image();
			addCommentButton.Source = Device.OnPlatform("icon_send.png", "icon_send.png", "//Assets//icon_send.png");

			addCommentButton.VerticalOptions = LayoutOptions.Center;
			addCommentButton.HorizontalOptions = LayoutOptions.Center;
			TapGestureRecognizer addCommentButtonTap = new TapGestureRecognizer();
			addCommentButtonTap.Tapped += async (object sender, EventArgs e) => 
			{
				
			};
			addCommentButton.GestureRecognizers.Add(addCommentButtonTap);

			StackLayout inputCountainer = new StackLayout
			{
				Spacing = Device.OnPlatform(5, 5, 1),
				Padding = Device.OnPlatform(5, 5, 5),
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.FromRgb( 45, 62, 80 ),
				Children = { chatEntry, addCommentButton },
				WidthRequest = App.screenWidth
			};


			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
			masterLayout.AddChildToLayout ( chatHistoryListView, 0, 10 );
			masterLayout.AddChildToLayout ( inputCountainer, 0, 85 );

			ScrollView masterScroll = new ScrollView ();
			masterScroll.Orientation = ScrollOrientation.Vertical;
			masterScroll.Content = masterLayout;

			Content = masterScroll;

		}

		public void Dispose ()
		{
			
		}


	}


	public class ChatHistoryListCell : ViewCell
	{
		public ChatHistoryListCell()
		{
			StackLayout mainLayout = new StackLayout ();
			mainLayout.Orientation = StackOrientation.Horizontal;
			mainLayout.WidthRequest = App.screenWidth;
			//mainLayout.HeightRequest = App.screenHeight * 50 / 100;
			mainLayout.BackgroundColor =  Color.FromRgb(54, 79, 120);
			mainLayout.Padding = new Thickness (10, 10, 10, 10);
			mainLayout.Spacing = 50;

			StackLayout labelContainer = new StackLayout ();
			labelContainer.BackgroundColor = Color.White;
			//labelContainer.Spacing = 10;

			labelContainer.Padding = new Thickness ( 5, 15, 5, 15 );

			StackLayout labelMasterContainer = new StackLayout ();
			labelMasterContainer.BackgroundColor = Color.White;
			//labelMasterContainer.Padding = new Thickness ( 5, 0, 5, 0 );



			BoxView trasperentArea = new BoxView ();
			trasperentArea.BackgroundColor = Color.Transparent;
			trasperentArea.WidthRequest = App.screenWidth;
			trasperentArea.HeightRequest = 100;

			Label chat = new Label ();
			chat.BackgroundColor = Color.White;
			chat.TextColor = Color.Black;
			chat.XAlign = TextAlignment.Start;
			chat.YAlign = TextAlignment.Center;
			chat.VerticalOptions = LayoutOptions.Start;
			chat.FontSize = 15;
			chat.HorizontalOptions = LayoutOptions.CenterAndExpand;
			chat.SetBinding ( Label.TextProperty, "." );
			labelContainer.Children.Add ( chat );
		//	labelContainer.Children.Add ( trasperentArea );



			//labelMasterContainer.Children.Add ( labelContainer );

			/*CircleImage userImage = new CircleImage 
			{
				Aspect = Aspect.AspectFit,
				WidthRequest = 100,
				HeightRequest = 100,
				HorizontalOptions = LayoutOptions.Start
			};
			userImage.SetBinding ( CircleImage.SourceProperty, "profileImgUrl" );

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
			mainLayout.Children.Add ( userName );*/
			mainLayout.Children.Add ( labelContainer );
		//	mainLayout.Children.Add ( trasperentArea );


			View = mainLayout;
			this.View.BackgroundColor =  Color.FromRgb(54, 79, 120);
		}


	}
}


