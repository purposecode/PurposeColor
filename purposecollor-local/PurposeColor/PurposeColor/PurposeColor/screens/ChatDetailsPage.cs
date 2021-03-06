﻿using System;
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
using PushNotifictionListener;
using XLabs.Forms.Controls;
using System.Threading.Tasks;
using System.Linq;
using AdvancedTimer.Forms.Plugin.Abstractions;

namespace PurposeColor
{
	public class MessageViewCell : ViewCell
	{
	}

	public class ChatDetailsPage : ContentPage, IDisposable
	{
		PurposeColorTitleBar mainTitleBar;
		CommunityGemChatTitleBar subTitleBar;
		CustomLayout masterLayout;
		IProgressBar progressBar;
		ListView chatHistoryListView;
		ObservableCollection<ChatDetails> chatList = null;
		User currentuser;
		string touserID;
		IAdvancedTimer timer;

		public ChatDetailsPage ( ObservableCollection<ChatDetails> chats,string tosusrID, string userImageUrl, string toUserName )
		{


			App.CurrentChatUserID = tosusrID;
			NavigationPage.SetHasNavigationBar(this, false);

			chatList = chats;
			touserID = tosusrID;
			currentuser = App.Settings.GetUser ();

		    timer = DependencyService.Get<IAdvancedTimer>();
			timer.initTimer (30000, SyncChatHistoryFromServer, true);
			timer.startTimer ();
			//Xamarin.Forms.Device.StartTimer ( TimeSpan.FromSeconds( 1 ), SyncChatHistoryFromServer );


			string chatTouser = toUserName;

			if (chatTouser.Length > 30)
			{
				chatTouser = chatTouser.Substring(0, 30);
				chatTouser += "...";
			}


			progressBar = DependencyService.Get< IProgressBar > ();
			mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), chatTouser, Color.Black, userImageUrl, true);
			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += (object sender, EventArgs e) => 
			{
				App.masterPage.IsPresented = true;
			};
			subTitleBar = new CommunityGemChatTitleBar(Constants.SUB_TITLE_BG_COLOR, chatTouser, userImageUrl, false);
			subTitleBar.BackButtonTapRecognizer.Tapped += async (object sender, EventArgs e) => 
			{
				timer.stopTimer ();
				App.CurrentChatUserID = null;
				await Navigation.PopAsync();
			};

			masterLayout = new CustomLayout ();
			masterLayout.WidthRequest = App.screenWidth;
			masterLayout.HeightRequest = App.screenHeight - 50;
			masterLayout.BackgroundColor = Color.FromRgb(45, 62, 80);

			chatHistoryListView = new ListView();
			chatHistoryListView.ItemTemplate = new DataTemplate(typeof(ChatHistoryListCell));
			chatHistoryListView.SeparatorVisibility = SeparatorVisibility.None;
			chatHistoryListView.HeightRequest = App.screenHeight * 70 / 100;
			chatHistoryListView.HasUnevenRows = true;
			chatHistoryListView.BackgroundColor =  Color.FromRgb(54, 79, 120);
			chatHistoryListView.ItemsSource = chatList;
			if( chatList != null && chatList.Count > 1 )
				chatHistoryListView.ScrollTo( chatList[ chatList.Count -1 ], ScrollToPosition.End, true );
			
			chatHistoryListView.ItemTapped += (object sender, ItemTappedEventArgs e) => 
			{
				chatHistoryListView.SelectedItem = null;
			};
			chatHistoryListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => 
			{
				chatHistoryListView.SelectedItem = null;
			};


		

			ExtendedEntry chatEntry = new ExtendedEntry
			{
				Placeholder = "Enter your chat...",
				BackgroundColor = Color.White,//Color.White,
				WidthRequest = App.screenWidth * .80,
				HorizontalOptions = LayoutOptions.Start,
				TextColor = Color.Black
			};
			chatEntry.TextChanged += ChatEntry_TextChanged;

			Image postChatButton = new Image();
			postChatButton.Source = Device.OnPlatform("icon_send.png", "icon_send.png", "//Assets//icon_send.png");

			postChatButton.VerticalOptions = LayoutOptions.Center;
			postChatButton.HorizontalOptions = LayoutOptions.Center;
			TapGestureRecognizer postChatButtonTap = new TapGestureRecognizer();

			postChatButton.GestureRecognizers.Add(postChatButtonTap);

			StackLayout inputCountainer = new StackLayout
			{
				Spacing = Device.OnPlatform(5, 5, 1),
				Padding = Device.OnPlatform(5, 5, 5),
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.FromRgb( 45, 62, 80 ),
				Children = { chatEntry, postChatButton },
				WidthRequest = App.screenWidth
			};


			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
			masterLayout.AddChildToLayout ( chatHistoryListView, 0, 17 );
			masterLayout.AddChildToLayout ( inputCountainer, 0, 85 );

			ScrollView masterScroll = new ScrollView ();
			masterScroll.Orientation = ScrollOrientation.Vertical;
			masterScroll.Content = masterLayout;
			masterScroll.IsClippedToBounds = true;

			postChatButtonTap.Tapped += async (object sender, EventArgs e) => 
			{
				ChatDetails detail = new ChatDetails();
				detail.AuthorName = "prvn";
				detail.Message = chatEntry.Text;
				detail.FromUserID = currentuser.UserId.ToString();
				detail.CurrentUserid = currentuser.UserId.ToString();
				chatList.Add( detail );
				chatEntry.Text = "";
				chatHistoryListView.ScrollTo( chatList[ chatList.Count -1 ], ScrollToPosition.End, true );

				if(!string.IsNullOrEmpty( detail.Message ))
					await ServiceHelper.SendChatMessage( currentuser.UserId.ToString(), touserID, detail.Message );
			};



			/*	this.Appearing += async (object sender, EventArgs e) => 
			{

				progressBar.ShowProgressbar( "Preparing chat window..." );
				masterScroll.IsVisible = true;

				chatUsersList = await ServiceHelper.GetAllChatUsers ();



				progressBar.HideProgressbar();

			};*/


			MessagingCenter.Subscribe<CrossPushNotificationListener, string>(this, "boom", (page, message) =>
				{
					string pushResult = message;
					string[] delimiters = { "&&" };
					string[] clasIDArray = pushResult.Split(delimiters, StringSplitOptions.None);
					string chatMessage = clasIDArray [0];
					string fromUser = clasIDArray [1];


					if( touserID == fromUser )
					{
						ChatDetails detail = new ChatDetails();
						detail.AuthorName = fromUser;
						detail.Message = chatMessage;
						detail.FromUserID = fromUser;
						detail.CurrentUserid = currentuser.UserId.ToString();
						chatList.Add( detail );

						if( chatList != null && chatList.Count > 1 )
						chatHistoryListView.ScrollTo( chatList[ chatList.Count -1 ], ScrollToPosition.End, true );
					}


				});


			Content = masterScroll;

		}


		public  void timerElapsed(object o, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine ( "-----------------i am CCCC timer------------" );
		}


		public void SyncChatHistoryFromServer( object o, EventArgs e )
		{

			ChatHistoryObject history = new ChatHistoryObject ();
			Task.Run( async () =>
			{ 
					 history = await ServiceHelper.GetChatHistory ( touserID, currentuser.UserId );
				} ).Wait();


			if (history != null && history.resultarray.Count > 0) 
			{
				
				List<string> serverChat = history.resultarray.Select (itm => itm.msg).ToList ();

				List<string> localChat = chatList.Select ( itm => itm.Message ).ToList();


				bool updateChatList = (serverChat != null && chatList != null && serverChat.Count != chatList.Count) ? true : false;

				if ( updateChatList ) 
				{
					chatList.Clear ();
					foreach (var item in history.resultarray) 
					{
						chatList.Add ( new ChatDetails{ CurrentUserid = currentuser.UserId, FromUserID = item.from_id, Message = item.msg } );

					}
					if (chatList != null && chatList.Count > 1)
					{
						ILocalNotification notify = DependencyService.Get<ILocalNotification> ();
						notify.ShowNotification ( "notify", "chat", chatList[ chatList.Count -1 ].Message, false );

						Device.BeginInvokeOnMainThread(  async () => 
						{
								chatHistoryListView.ScrollTo( chatList[ chatList.Count -1 ], ScrollToPosition.End, true );
						});
						
					}

				}

			}
		}

		void ChatEntry_TextChanged (object sender, TextChangedEventArgs e)
		{
			ExtendedEntry cEntry = sender as ExtendedEntry;
			if (e.OldTextValue != null && e.NewTextValue.Length > Constants.CHAT_MESSAGE_MAX_LENGTH) {
				cEntry.Text = e.OldTextValue;
			}
		}

		protected override bool OnBackButtonPressed ()
		{
			timer.stopTimer ();
			App.CurrentChatUserID = null;
			Dispose ();
			return base.OnBackButtonPressed ();
		}

		public void Dispose ()
		{
			timer.stopTimer ();
			timer = null;
			App.CurrentChatUserID = null;
		}

		private Cell CreateMessageCell()
		{
			var timestampLabel = new Label();
			timestampLabel.SetBinding(Label.TextProperty, new Binding("Timestamp", stringFormat: "[{0:HH:mm}]"));
			timestampLabel.TextColor = Color.Silver;
			timestampLabel.Font = Font.SystemFontOfSize(14);

			var authorLabel = new Label();
			authorLabel.SetBinding(Label.TextProperty, new Binding("AuthorName", stringFormat: "{0}: "));
			authorLabel.TextColor = Device.OnPlatform(Color.Blue, Color.Yellow, Color.Yellow);
			authorLabel.Font = Font.SystemFontOfSize(14);

			var messageLabel = new Label();
			messageLabel.SetBinding(Label.TextProperty, new Binding("Message"));
			messageLabel.Font = Font.SystemFontOfSize(14);

			var stack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {authorLabel, messageLabel}
			};


			var view = new MessageViewCell
			{
				View = stack
			};
			return view;
		}


	}


	public class CustomImageLabel : Label
	{
		public string BackGroundImage{ get; set;}
	}


	public class ChatHistoryListCell : ViewCell
	{
		public ChatHistoryListCell()
		{
			StackLayout mainLayout = new StackLayout ();
			//mainLayout.Orientation = StackOrientation.Horizontal;
			mainLayout.WidthRequest = App.screenWidth;

			mainLayout.BackgroundColor = Color.FromRgb(54, 79, 120);// Color.FromRgb(54, 79, 120);
			mainLayout.Padding = new Thickness (10, 10, 10, 10);
			mainLayout.Spacing = 0;


			StackLayout tipContainer = new StackLayout ();
			//tipContainer.BackgroundColor =  Color.tr  Color.FromRgb(54, 79, 120);
			tipContainer.HorizontalOptions = LayoutOptions.Center;
			tipContainer.VerticalOptions = LayoutOptions.Center;
			tipContainer.SetBinding ( StackLayout.HorizontalOptionsProperty, "BubblePos" );
			//tipContainer.SetBinding ( StackLayout.BackgroundColorProperty, "BubbleColor" );



			StackLayout labelContainer = new StackLayout ();
			labelContainer.Orientation = StackOrientation.Horizontal;
			labelContainer.Padding = new Thickness ( 5, 15, 5, 15 );
			labelContainer.SetBinding ( StackLayout.HorizontalOptionsProperty, "BubblePos" );
			labelContainer.SetBinding ( StackLayout.BackgroundColorProperty, "BubbleColor" );


			Label chat = new Label ();
			chat.TextColor = Color.Black;
			chat.XAlign = TextAlignment.End;
			chat.YAlign = TextAlignment.Center;
			chat.VerticalOptions = LayoutOptions.Start;
			chat.FontSize = 15;
			chat.HorizontalOptions = LayoutOptions.End;
			chat.SetBinding ( Label.TextProperty, "Message" );

			Image imgTip = new Image ();
			imgTip.Aspect = Aspect.Fill;
			imgTip.WidthRequest = 25;
			imgTip.HeightRequest = 15;
			imgTip.HorizontalOptions = LayoutOptions.Start;
			imgTip.VerticalOptions = LayoutOptions.End;
			imgTip.SetBinding ( Image.SourceProperty, "ImageTip" );

			tipContainer.Children.Add ( imgTip );
			labelContainer.Children.Add ( chat );



			mainLayout.Children.Add ( labelContainer );
			mainLayout.Children.Add ( tipContainer );



			View = mainLayout;
			//this.View.BackgroundColor =  Color.FromRgb(54, 79, 120);
		}


	}
}


