using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Database;
using PurposeColor.Model;
using PurposeColor.screens;
using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using PurposeColor.Service;
using PushNotifictionListener;

namespace PurposeColor
{
	public class AppTest : Application
	{
		//
		// This page is displayed from within 'native' pages on iOS and Android
		//

		public static Page GetSecondPage ()
		{
			var formsPage = new NavigationPage (new FeelingNowPage ()); 

			return formsPage;
		}

		public AppTest ( string msg, string fromID )
		{
			ObservableCollection<ChatDetails> chatInfos = new ObservableCollection<ChatDetails> ();

			if (App.chatList != null && App.chatList.resultarray != null)
			{
				ChatUsersInfo userInfo = App.chatList.resultarray.Find (itm => itm.user_id == fromID);

				if (userInfo != null) 
				{
					chatInfos.Add ( new ChatDetails{ CurrentUserid = App.Settings.GetUser().UserId.ToString(), FromUserID = fromID, Message = msg } );
					MainPage = new NavigationPage (new ChatDetailsPage ( chatInfos, fromID, userInfo.profileImgUrl, userInfo.firstname ));
				}

			}
		}
	}

	public class App : Application
	{
		public static string CurrentChatUserID { get; set; }
		public static string NotificationToken { get; set; }
		public static string NotificationReqID { get; set; }
		public static string WindowsDownloadedMedia { get; set; }
		public static long DownloadID{ get; set;}
		public static long ShareDownloadID { get; set; }
		public static string DownloadsPath { get; set; }
		public static bool IsLoggedIn { get; set; }
		public static INavigation Navigator { get; set; }
		public static bool IsGoogleLogin { get; set; }
		public static bool IsFacebookLogin { get; set; }
		public static string SelectedEmotion { get; set; }
		public static PurposeMasterDetailPage masterPage;
		static string token;
		public static double Lattitude;
		public static double Longitude;
		public static string SelectedActionStartDate { get; set; }
		public static string SelectedActionEndDate { get; set; }
		public static int SelectedActionReminderValue { get; set; }
		static ApplicationSettings applicationSettings;
		public static List<CustomListViewItem> goalsListSource;
		public static List<CustomListViewItem> actionsListSource;
		public static List<CustomListViewItem> eventsListSource;
		public static List<CustomListViewItem> emotionsListSource;
		public static List<CustomListViewItem> nearByLocationsSource;
		public static List<MediaItem> MediaArray { get; set; }
		public static List<ExtentionItem> ExtentionArray { get; set; }
		public static List<string> ContactsArray { get; set; }
		public static ObservableCollection<PreviewItem> PreviewListSource = new ObservableCollection<PreviewItem>();
		public static ObservableCollection<MenuItems> burgerMenuItems = null;
		public static string newEmotionId;
		public static bool IsTesting = false;
		public static bool isEmotionsListing = false;
		public static ChatObject chatList{ get; set; }
		public static bool GemDeleted = false;
		public static ApplicationSettings Settings
		{
			get
			{
				return applicationSettings;
			}
		}
		public static string Token
		{
			get { return token; }
		}
		IDeviceSpec deviceSpec;
		public static double screenHeight;
		public static double screenWidth;
		public static double screenDensity;
		public static CreateEventPage CalPage{ get; set; }

		public App()
		{


			deviceSpec = DependencyService.Get<IDeviceSpec>();
			screenHeight = deviceSpec.ScreenHeight;
			screenWidth = deviceSpec.ScreenWidth;
			screenDensity = deviceSpec.ScreenDensity;
			MediaArray = new List<MediaItem>();
			ContactsArray = new List<string>();
			ExtentionArray = new List<ExtentionItem>();
			NavigationPage.SetHasNavigationBar(this, false);
			nearByLocationsSource = new List<CustomListViewItem>();
			if (applicationSettings == null)
			{
				applicationSettings = new ApplicationSettings();
			}
			MenuPage menuPage = new MenuPage();
			burgerMenuItems = new ObservableCollection<MenuItems> ();
			masterPage = new PurposeMasterDetailPage();
			MainPage = masterPage;

		}

		protected override void OnStart()
		{
			if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
			{
				CrossPushNotification.Current.Register();
			}

			if (applicationSettings == null)
			{
				applicationSettings = new ApplicationSettings();
			}
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		public static async Task<bool> SaveUserData(User userInfo, bool isGoogleuser)
		{
			UserDetailsOnLogin userdetails = null;
			User currentUser = null;
			try 
			{
				
				if (isGoogleuser) {
					// call api  to save google user
					userdetails = await ServiceHelper.GoogleUserLogin(userInfo.Email, userInfo.UserName, userInfo.UserId,userInfo.ProfileImageUrl,userInfo.Gender);

					//get useer//

				} else {
					// call api for saving facebook user
					userdetails = await ServiceHelper.FacebookLogin(userInfo.Email, userInfo.UserName, userInfo.UserId);
				}

				if(userdetails!= null && userdetails.resultarray!= null && userdetails.code == "200")
				{
					currentUser = new User();
					var loggedInUser = userdetails.resultarray;
					currentUser.StatusNote = string.IsNullOrEmpty(loggedInUser.note) ? string.Empty : loggedInUser.note;
					currentUser.DisplayName = string.IsNullOrEmpty(loggedInUser.firstname) ? string.Empty : loggedInUser.firstname;
					currentUser.Email = string.IsNullOrEmpty(loggedInUser.email) ? string.Empty : loggedInUser.email;
					currentUser.ProfileImageUrl = string.IsNullOrEmpty(loggedInUser.profileurl) ? string.Empty : loggedInUser.profileurl;
					currentUser.UserId = loggedInUser.user_id;
					currentUser.VerifiedStatus = loggedInUser.verified_status;
					currentUser.UserName = string.IsNullOrEmpty(loggedInUser.firstname) ? string.Empty : loggedInUser.firstname;

					if (loggedInUser.usertype_id != null)
					{
						currentUser.UserType = int.Parse(loggedInUser.usertype_id);
					}
					if (loggedInUser.regdate != null)
					{
						currentUser.RegistrationDate = loggedInUser.regdate;
					}
				}

				if (currentUser != null) 
				{
					App.Settings.SaveUser (currentUser);
				}

				return true;
			} catch (Exception ex) 
			{
				var test = ex.Message;
			}
			return false;
		}

		public static void NavigateToChatDetailsPage(User userInfo, string tosusrID, string userImageUrl, string toUserName )
		{
			Device.BeginInvokeOnMainThread(  async () => 
				{
					ObservableCollection<ChatDetails> chats = new ObservableCollection<ChatDetails>();

					ChatDetails test = new ChatDetails();
					test.AuthorName = "prvn";
					test.CurrentUserid = App.Settings.GetUser().UserId;
					test.Message = "test chat";

					chats.Add( test );

					await Navigator.PushModalAsync( new ChatDetailsPage( chats,tosusrID, userImageUrl, toUserName) );


				});
		}

		public static void SaveToken(string tkn)
		{
			token = tkn;
		}

		public static async Task<bool> UpdateNotificationStatus( string reqID, string status )
		{
			return await ServiceHelper.UpdateNotificationRequest ( status, reqID );

		}

		public static List<CustomListViewItem> GetEmotionsList()
		{
			if (emotionsListSource != null && emotionsListSource.Count > 0)
				return emotionsListSource;
			/*  eventsListSource.Add(new CustomListViewItem { Name = "married" });
            eventsListSource.Add(new CustomListViewItem { Name = "divorsed" });
            eventsListSource.Add(new CustomListViewItem { Name = "got promotion" });
            eventsListSource.Add(new CustomListViewItem { Name = "got a trip" });
            eventsListSource.Add(new CustomListViewItem { Name = "bought a car" });*/
			return emotionsListSource;
		}

		public static List<CustomListViewItem> GetEventsList()
		{
			if (eventsListSource != null && eventsListSource.Count > 0)
				return eventsListSource;

			//eventsListSource = new List<CustomListViewItem>();
			//eventsListSource.Add(new CustomListViewItem { Name = "Lost Job" });
			//eventsListSource.Add(new CustomListViewItem { Name = "married" });
			//eventsListSource.Add(new CustomListViewItem { Name = "divorsed" });
			//eventsListSource.Add(new CustomListViewItem { Name = "got promotion" });
			//eventsListSource.Add(new CustomListViewItem { Name = "got a trip" });
			//eventsListSource.Add(new CustomListViewItem { Name = "bought a car" });
			return eventsListSource;
		}

		public static  List<CustomListViewItem> GetActionsList()
		{
			if (actionsListSource != null && actionsListSource.Count > 0)
				return actionsListSource;

			return actionsListSource;
		}

		public static List<CustomListViewItem> GetGoalsList()
		{
			if (goalsListSource != null && goalsListSource.Count > 0)
				return goalsListSource;

			/*   goalsListSource = new List<CustomListViewItem>();
            goalsListSource.Add(new CustomListViewItem { Name = "Loose Weight" });
            goalsListSource.Add(new CustomListViewItem { Name = "Learn Yoga" });
            goalsListSource.Add(new CustomListViewItem { Name = "To be Rich" });
            goalsListSource.Add(new CustomListViewItem { Name = "Peace Of Mind" });
            goalsListSource.Add(new CustomListViewItem { Name = "Buy a New Car" });
            goalsListSource.Add(new CustomListViewItem { Name = "Visit Everest" });
            goalsListSource.Add(new CustomListViewItem { Name = "Meet Prime Minister" });*/
			return goalsListSource;
		}

		public static Action SuccessfulLoginAction
		{
			get
			{
				NavigationPage navPage = new NavigationPage(new FeelingNowPage());
				return new Action(() =>
					{
						try
						{
							//Navigator.PushModalAsync(navPage);
							UpdateBurgerMenuList();

							App.masterPage.Detail = new NavigationPage(new FeelingNowPage());
							Navigator.PopModalAsync();
							//Navigator.PushModalAsync(navPage);
						}
						catch (Exception ex)
						{
							Debug.WriteLine("SuccessfulLoginAction : " + ex.Message);
						}
					});
			}
		}

		public static void UpdateBurgerMenuList()
		{
			try {

				if (burgerMenuItems == null) {
					burgerMenuItems = new System.Collections.ObjectModel.ObservableCollection<MenuItems> ();
				}

				if(burgerMenuItems.Count > 1)
				{
					return; // its alredy loged in menu, no need to change.
				}

				burgerMenuItems.Clear ();

				burgerMenuItems.Add (new MenuItems {
					Name = Constants.EMOTIONAL_AWARENESS,
					ImageName = Device.OnPlatform ("emotional_awrness_menu_icon.png", "emotional_awrness_menu_icon.png", "//Assets//emotional_awrness_menu_icon.png")
				});
				burgerMenuItems.Add (new MenuItems {
					Name = Constants.GEM,
					ImageName = Device.OnPlatform ("gem_menu_icon.png", "gem_menu_icon.png", "//Assets//gem_menu_icon.png")
				});
				burgerMenuItems.Add (new MenuItems {
					Name = Constants.GOALS_AND_DREAMS,
					ImageName = Device.OnPlatform ("goals_drms_menu_icon.png", "goals_drms_menu_icon.png", "//Assets//goals_drms_menu_icon.png")
				});
				burgerMenuItems.Add (new MenuItems {
					Name = Constants.EMOTIONAL_INTELLIGENCE,
					ImageName = Device.OnPlatform ("emotion_intellegene_menu_icon.png", "emotion_intellegene_menu_icon.png", "//Assets//emotion_intellegene_menu_icon.png")
				});
				burgerMenuItems.Add (new MenuItems {
					Name = Constants.COMMUNITY_GEMS,
					ImageName = Device.OnPlatform ("comunity_menu_icon.png", "comunity_menu_icon.png", "//Assets//comunity_menu_icon.png")
				});
				burgerMenuItems.Add (new MenuItems {
					Name = Constants.APPLICATION_SETTTINGS,
					ImageName = Device.OnPlatform ("setings_menu_icon.png", "setings_menu_icon.png", "//Assets//setings_menu_icon.png")
				});
				burgerMenuItems.Add (new MenuItems {
					Name = Constants.SIGN_OUT_TEXT,
					ImageName = Device.OnPlatform ("logout_icon.png", "logout_icon.png", "//Assets//logout_icon.png")
				});

			} catch (Exception ex) {
				var test = ex.Message;
			}
		}
	}


	public class MyMasterDetailPage : MasterDetailPage
	{
		public MyMasterDetailPage()
		{
			App.Navigator = Navigation;
			NavigationPage.SetHasNavigationBar(this, false);
			MasterPage page = new MasterPage();
			Master = page;
			Detail = new NavigationPage(new LogInPage());
			this.Title = "test";
		}
	}

	public class MasterPage : ContentPage
	{
		public MasterPage()
		{
			Icon = "icon.png";
			Title = "Menu";
			//  BackgroundColor = Color.FromHex("444444");
			BackgroundColor = Color.Red;
			Content = new Label
			{
				Text = "master"
			};
		}
	}

	public class DetailsPage : ContentPage
	{
		MyMasterDetailPage masterPage;
		public DetailsPage(MyMasterDetailPage master)
		{
			masterPage = master;

			//  Title = "details";
			int titlebarHeight = (int)App.screenHeight * 10 / 100;
			int titlebarWidth = (int)App.screenWidth;

			CustomLayout masteLlayout = new CustomLayout();
			masteLlayout.WidthRequest = (int)App.screenWidth;
			masteLlayout.HeightRequest = (int)App.screenHeight;

			//        CustomTitleBar titleBar = new CustomTitleBar();

			StackLayout layout = new StackLayout();
			layout.WidthRequest = (int)App.screenWidth;
			layout.HeightRequest = (int)App.screenHeight * 90 / 100;

			masteLlayout.AddChildToLayout(layout, 0, 10);

			NavigationPage.SetHasNavigationBar(this, false);
			masteLlayout.BackgroundColor = Color.White;
			Content = masteLlayout;
		}

		void backButton_Clicked(object sender, EventArgs e)
		{
			masterPage.IsPresented = true;
		}
	}
}
