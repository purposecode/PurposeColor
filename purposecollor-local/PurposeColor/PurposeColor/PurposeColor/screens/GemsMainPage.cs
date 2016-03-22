using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using PurposeColor.Model;
using PurposeColor.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace PurposeColor.screens
{
	public class GemsMainPage : ContentPage, IDisposable
	{
		CustomLayout masterLayout;
		IProgressBar progressBar;
		GemsPageTitleBar mainTitleBar;
		ScrollView masterScroll;
		StackLayout masterStack;
		List<EventWithImage> eventsWithImage;
		List<ActionWithImage> actionsWithImage;

		StackLayout emotionsButtion = null;
		StackLayout goalsButton = null;
		Label goalsAndDreamsLabel = null;
		Label emotionLabel = null;

		TapGestureRecognizer emotionListingBtnTapgesture = null;
		TapGestureRecognizer goalsListingBtnTapgesture = null;

		//bool isEmotionsListing = false;
		StackLayout listViewContainer;
		string previousTitle = string.Empty;

		bool reachedFront = true;
		bool displayedLastGem = false;
		int lastGemIndexOnDisplay = 0;
		int firstGemIndexOnDisplay = 0;
		bool isLoadingFromDetailsPage = false;


		public GemsMainPage()
		{

			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
			progressBar = DependencyService.Get<IProgressBar>();
			progressBar.ShowProgressbar ("Loading gems..");
			App.isEmotionsListing = false;
			isLoadingFromDetailsPage = false;

			this.Appearing += OnAppearing;
			this.Disappearing += GemsMainPage_Disappearing;
			mainTitleBar = new GemsPageTitleBar(Color.FromRgb(8, 135, 224), "Goal Enabling Materials", Color.White, "", true);
			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += OnImageAreaTapGestureRecognizerTapped;
			masterScroll = new ScrollView();
			masterScroll.WidthRequest = App.screenWidth;
			masterScroll.HeightRequest = App.screenHeight * 85 / 100;
			masterScroll.BackgroundColor = Color.White;
			masterScroll.Scrolled += OnScroll;
			masterScroll.IsClippedToBounds = true;

			masterStack = new StackLayout();
			masterStack.Orientation = StackOrientation.Vertical;
			masterStack.BackgroundColor = Color.White;

			emotionLabel = new Label {
				Text = "EMOTIONS  ", 
				FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
				FontSize = Device.OnPlatform (14, 18, 14),
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				WidthRequest = App.screenWidth * .5,
				XAlign = TextAlignment.Center
			};

			emotionsButtion = new StackLayout{
				Children = {
					emotionLabel
				},
				BackgroundColor = Color.FromRgb(8, 159, 245),
				Orientation = StackOrientation.Horizontal,
				WidthRequest = App.screenWidth * .5,
				//HorizontalOptions = LayoutOptions.Center
			};

			emotionListingBtnTapgesture = new TapGestureRecognizer ();
			emotionListingBtnTapgesture.Tapped += ShowEmotionsTapGesture_Tapped;
			emotionsButtion.GestureRecognizers.Add (emotionListingBtnTapgesture);

			goalsAndDreamsLabel = new Label {
				Text = "  GOALS & DREAMS", 
				FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
				FontSize = Device.OnPlatform (14, 18, 14),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.Gray
			};

			goalsButton = new StackLayout{
				Children = {
					goalsAndDreamsLabel
				},
				BackgroundColor = Constants.INPUT_GRAY_LINE_COLOR,
				Orientation = StackOrientation.Horizontal,
				WidthRequest = App.screenWidth * .5
			};
			goalsListingBtnTapgesture = new TapGestureRecognizer ();
			goalsListingBtnTapgesture.Tapped += GoalsListingBtnTapgesture_Tapped;
			goalsButton.GestureRecognizers.Add (goalsListingBtnTapgesture);

			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(new StackLayout{HeightRequest =  App.screenHeight * 0.08, Orientation = StackOrientation.Horizontal, Spacing = 0, Children = {emotionsButtion, goalsButton}}, 0,Device.OnPlatform(9,10,10));

			progressBar.HideProgressbar ();
		}

		void GemsMainPage_Disappearing (object sender, EventArgs e)
		{
			//Dispose (); // if so on navigating back from gem details page will cause null exceptions
		}

		async void GoalsListingBtnTapgesture_Tapped (object sender, EventArgs e)
		{
			try {
				if (!App.isEmotionsListing) {
					return;
				}
				if (progressBar == null) {
					progressBar = DependencyService.Get<IProgressBar>();
				}

				progressBar.ShowProgressbar ("Loading gems..");

				if (actionsWithImage == null) 
				{
					actionsWithImage = await ServiceHelper.GetAllActionsWithImage ();

					if (actionsWithImage == null) 
					{
						var success = await DisplayAlert (Constants.ALERT_TITLE, "Error in fetching GEMS", Constants.ALERT_OK, Constants.ALERT_RETRY);
						if (!success) {
							GoalsListingBtnTapgesture_Tapped(goalsButton, null);
							return;
						}
						else 
						{
							if (Device.OS != TargetPlatform.WinPhone)
							{
								actionsWithImage = await App.Settings.GetAllActionWithImage();
							}
						}
					}
					else
					{
						if (Device.OS != TargetPlatform.WinPhone)
						{
							await App.Settings.SaveActionsWithImage(actionsWithImage);
						}
					}
				}
				progressBar.HideProgressbar ();
				// do list the actions.....//
				bool isSuccess = await AddActionsToView(0, true);
				if (isSuccess) 
				{
					App.isEmotionsListing = false;
					await masterScroll.ScrollToAsync(0,0, true);
					#region button color
					// hide emotions & show goals , change selection buttons color
					Color goalBtnClr = goalsButton.BackgroundColor;
					goalsButton.BackgroundColor = emotionsButtion.BackgroundColor;
					emotionsButtion.BackgroundColor = goalBtnClr;
					Color golsTxtClr = goalsAndDreamsLabel.TextColor;
					goalsAndDreamsLabel.TextColor = emotionLabel.TextColor;
					emotionLabel.TextColor = golsTxtClr;
					#endregion
				}

			} catch (Exception ex) {
				var test = ex.Message;
				progressBar.HideProgressbar ();
			}

		}

		async void ShowEmotionsTapGesture_Tapped (object sender, EventArgs e)
		{
			try {
				if (App.isEmotionsListing) {
					return;
				}

				if(progressBar == null)
				{
					progressBar = DependencyService.Get<IProgressBar>();
				}
				// display the emotions list and change color of Goals selection uttion
				//progressBar.ShowProgressbar ("Loading gems.. 3");

				bool isSuccess = await AddEventsToView (0);
				if (isSuccess) 
				{
					masterScroll.ScrollToAsync(0,0, true);
					App.isEmotionsListing = true;
					#region MyRegionButton color
					Color eBtnClr = emotionsButtion.BackgroundColor;
					emotionsButtion.BackgroundColor = goalsButton.BackgroundColor;
					goalsButton.BackgroundColor = eBtnClr;
					Color ETxtClr = emotionLabel.TextColor;
					emotionLabel.TextColor = goalsAndDreamsLabel.TextColor;
					goalsAndDreamsLabel.TextColor = ETxtClr;

					#endregion
				}
				progressBar.HideProgressbar();
			} catch (Exception ) {
				
			}
			//progressBar.HideProgressbar ();
		}

		void OnBackButtonTapRecognizerTapped(object sender, EventArgs e)
		{
			try {
				App.Navigator.PopAsync();
			} catch (Exception ) {

			}
		}

		#region OnAppearing

		async void OnAppearing (object sender, EventArgs e)
		{
			if (!isLoadingFromDetailsPage) 
			{
				if(progressBar == null)
				progressBar = DependencyService.Get<IProgressBar> ();

				if(Device.OS != TargetPlatform.iOS)
				progressBar.ShowProgressbar ("Loading gems..");

				try {
					try {
						if (eventsWithImage == null) {
							eventsWithImage = await ServiceHelper.GetAllEventsWithImage ();

							if (eventsWithImage != null) {
								App.Settings.SaveEventsWithImage (eventsWithImage);
							} else {
								var success = await DisplayAlert (Constants.ALERT_TITLE, "Error in fetching gems", Constants.ALERT_OK, Constants.ALERT_RETRY);
								if (!success) {
									OnAppearing (sender, EventArgs.Empty);
									return;
								} else {
									eventsWithImage = await App.Settings.GetAllEventWithImage (); // get from local db.
									//progress.HideProgressbar ();
								}
							}
						}
					} catch (Exception ex) {
						var test = ex.Message;
					}

					listViewContainer = new StackLayout {
						Orientation = StackOrientation.Vertical,
						Padding = new Thickness (0, 0, 0, 5),
						//BackgroundColor = Color.White, //
						WidthRequest = App.screenWidth,
						Spacing = 0 // App.screenHeight * .02
					};
					masterStack.Children.Add (listViewContainer);

					StackLayout empty = new StackLayout ();
					empty.HeightRequest = Device.OnPlatform (30, 30, 50);
					empty.WidthRequest = App.screenWidth;
					empty.BackgroundColor = Color.Transparent;
					masterStack.Children.Add (empty);

					masterScroll.Content = masterStack;

					masterLayout.AddChildToLayout (masterScroll, 0, Device.OnPlatform (17, 18, 18));

					Content = masterLayout;

					// call - ShowEmotionsTapGesture_Tapped //
					progressBar.HideProgressbar ();
					ShowEmotionsTapGesture_Tapped (emotionsButtion, null);	
				}
				catch (Exception ex) {
					var test = ex.Message;
					progressBar.HideProgressbar ();
				}
				//progressBar.HideProgressbar ();
			}
		}

		#endregion

		async Task<bool> AddEventsToView(int index, bool showNextGems = true)
		{
			progressBar.ShowProgressbar ("Loading gems..");
			try {
				int listCapacity = 5;
				int max;

				if (showNextGems)
				{
					max = (index + listCapacity) <= eventsWithImage.Count ? (index + listCapacity) : eventsWithImage.Count;
					if (index >= max || eventsWithImage == null || eventsWithImage.Count == 0)
					{
						displayedLastGem = true;
						progressBar.HideProgressbar();
						return false;
					}
				}
				else
				{
					// show previous gems.
					max = index;
					index = (index - listCapacity) > 0 ? (index - listCapacity): 0;
				}

				if (max >= eventsWithImage.Count) {
					displayedLastGem = true;
				}else {
					displayedLastGem = false;
				}

				if (index == 0) {
					reachedFront = true;
				}
				else {
					reachedFront = false;
				}

				firstGemIndexOnDisplay = index;
				lastGemIndexOnDisplay = max;

				IDownload downloader = DependencyService.Get<IDownload> ();
				List<string> filesTodownliad = new List<string> ();


				// handle single gem view..
				if((max - index) < 3)
				{
					if((index - 1) >= 0)
					{
						index = index - 1;
					}
					else
					{
//						if(max + 1 < eventsWithImage.Count)
//						{
//							max = max + 1;
//						}
					}
				}

				for (int i = index; i < max; i++) 
				{
					string fileExtenstion = Path.GetExtension(eventsWithImage [i].event_media);


					if (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") 
					{
						filesTodownliad.Add (Constants.SERVICE_BASE_URL + eventsWithImage [i].event_media);
					}
				}

				await downloader.DownloadFiles (filesTodownliad);

				downloader = null;
				filesTodownliad.Clear();
				filesTodownliad = null;

				try {
					int cou = listViewContainer.Children.Count; 
					if (cou > 0) {
						listViewContainer.Children.Clear ();
					}
				} catch (Exception ) {

				}

				for (int i = index; i < max; i++) {
					try {
						await AddToScrollView (new CustomGemItemModel {
							Description = eventsWithImage [i].event_details,
							Source = App.DownloadsPath + Path.GetFileName (eventsWithImage [i].event_media),
							ID = eventsWithImage [i].event_id.ToString(),
							GroupTitle = eventsWithImage [i].emotion_title,
							CellIndex = i
						});
					} catch (Exception ex) {
						var test = ex.Message;
					}
				}


				//await masterScroll.ScrollToAsync( 0, 10, false );
				progressBar.HideProgressbar();
				return true;

			} catch (Exception ex) {
				var test = ex.Message;
			}
			progressBar.HideProgressbar();
			return false;
		}

		async Task<bool> AddActionsToView(int index, bool showNextGems = true)
		{
			progressBar.ShowProgressbar ("Loading gems..");

			int listCapacity = 5;
			int max  = 0;

			try 
			{
				if (showNextGems) 
				{
					//display next gems
					max = (index + listCapacity) <= actionsWithImage.Count ? (index + listCapacity) : actionsWithImage.Count;
					if (index >= max || actionsWithImage == null || actionsWithImage.Count == 0)
					{
						displayedLastGem = true;
						progressBar.HideProgressbar();
						return false;
					}
				}
				else
				{
					// show previous gems.
					max = index + 1;
					index = (index - listCapacity) > 0 ? (index - listCapacity): 0;
				}

				if (max >= actionsWithImage.Count) {
					displayedLastGem = true;
				}else {
					displayedLastGem = false;
				}

				if (index == 0) {
					reachedFront = true;
				}
				else {
					reachedFront = false;
				}
				if( (max - index) < 3)
				{
					if((index - 2) >= 0)
					{
						index = index - 1;
					}
					else
					{
//						if(max + 1 < actionsWithImage.Count)
//						{
//							max = max + 1;
//						}
					}
				}

				firstGemIndexOnDisplay = index;
				lastGemIndexOnDisplay = max;

				IDownload downloader = DependencyService.Get<IDownload> ();
				List<string> filesTodownliad = new List<string> ();
				for (int i = index; i < max; i++)
				{
					if(!string.IsNullOrEmpty(actionsWithImage[i].action_media)&& 
						!string.IsNullOrEmpty(actionsWithImage[i].action_details)&& 
						!string.IsNullOrEmpty(actionsWithImage[i].goalaction_id) && 
						!string.IsNullOrEmpty(actionsWithImage[i].goal_title))
					{
						filesTodownliad.Add (Constants.SERVICE_BASE_URL + actionsWithImage[i].action_media);
					}
				}
				await downloader.DownloadFiles (filesTodownliad);

				downloader = null;
				filesTodownliad.Clear();
				filesTodownliad = null;

				try {
					int cou = listViewContainer.Children.Count; 
					if (cou > 0) {
						listViewContainer.Children.Clear ();
					}
				} catch (Exception ) {

				}

				for (int i = index; i < max; i++) {
					try {
						await AddToScrollView (new CustomGemItemModel {
							Description = actionsWithImage [i].action_details,
							Source = App.DownloadsPath + Path.GetFileName (actionsWithImage [i].action_media),
							ID = actionsWithImage [i].goalaction_id.ToString(),
							GroupTitle = actionsWithImage [i].goal_title,
							CellIndex = i
						});
					} catch (Exception ex) {
						var test = ex.Message;
					}
				}


				//await masterScroll.ScrollToAsync( 0, 0, false );
				progressBar.HideProgressbar();
				return true;
			} catch (Exception ex) {
				var test = ex.Message;
			}
			progressBar.HideProgressbar();
			return false;
		}

		async Task<bool> AddToScrollView(CustomGemItemModel gemModel)
		{
			try {
				CustomLayout gemLayout = null;
				Image image = null;
				StackLayout bgStack = null;
				Label detailsLabel = null;
				Label groupTitleLabel = null;

				gemLayout = new CustomLayout ();
				gemLayout.BackgroundColor = Color.FromRgb(219,221,221);
				gemLayout.WidthRequest = App.screenWidth;
				gemLayout.Padding = new Thickness(0,0,0,App.screenHeight * .04);
				detailsLabel = new Label ();
				detailsLabel.Text = gemModel.Description;
				detailsLabel.TextColor = Color.White; // Color.FromRgb(8, 135, 224);//Color.FromRgb(8, 159, 245);
				detailsLabel.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
				detailsLabel.FontSize = 18;
				detailsLabel.WidthRequest = App.screenWidth - (App.screenWidth *.01);
				detailsLabel.HorizontalOptions = LayoutOptions.Center;
				detailsLabel.YAlign = TextAlignment.Center;
				detailsLabel.XAlign = TextAlignment.Center;

				bgStack = new StackLayout ();
				bgStack.WidthRequest = App.screenWidth;
				bgStack.HeightRequest = 85;
				bgStack.BackgroundColor = Color.Black;//Color.FromRgb(220, 220, 220);
				bgStack.Opacity = .2;
				bgStack.ClassId = gemModel.ID;

				if(!string.IsNullOrEmpty(detailsLabel.Text))
				{
					int textleng = detailsLabel.Text.Length;
				}

				image = new Image {
					Aspect = Aspect.AspectFill, //Aspect.Fill,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					WidthRequest = App.screenWidth,
					HeightRequest = (App.screenHeight * .35),
					Rotation= 0,
					Source = gemModel.Source
				};

				Image nextBtn = new Image
				{
					Source = "roundNextBtn.png",
					WidthRequest = App.screenWidth * .1,
					HeightRequest = App.screenWidth * .1,
					ClassId = gemModel.ID.ToString()
				};

				TapGestureRecognizer DetailsTapgesture = null;
				DetailsTapgesture = new TapGestureRecognizer();
				DetailsTapgesture.Tapped += DetailsTapgesture_Tapped;
				nextBtn.GestureRecognizers.Add(DetailsTapgesture);

				if (previousTitle == null || !string.IsNullOrEmpty(gemModel.GroupTitle) && gemModel.GroupTitle != previousTitle)
				{
					groupTitleLabel = new Label
					{
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						FontSize = 19,
						YAlign = TextAlignment.Start,
						VerticalOptions = LayoutOptions.Center,
						TextColor = Color.White,
						Text = "  " + gemModel.GroupTitle.Trim()
					};
					StackLayout titleHolder = new StackLayout{
						Children={groupTitleLabel},
						BackgroundColor = Color.Navy, //Color.FromRgb(59,164,222),//Color.FromRgb(88,168,6), // Color.FromRgb(111, 199, 251),
						Padding = 0,
						Orientation = StackOrientation.Horizontal,
						WidthRequest= App.screenWidth
					};
					gemLayout.AddChildToLayout (titleHolder, 0, 0);
					gemLayout.AddChildToLayout (image, 0, 4);
					gemLayout.AddChildToLayout (bgStack, 0, 16);//16 - appear @ center of img. 26 - text appear at bottom corner of img.
					gemLayout.AddChildToLayout (detailsLabel, 1, 16);
					gemLayout.AddChildToLayout (nextBtn, 47, 23); // btn width = 1% of width.

				}else
				{
					gemLayout.AddChildToLayout (image, 0, 0);
					gemLayout.AddChildToLayout (bgStack, 0, 12);// 12 - aliended center to img. // 22 - align to bottom of img.
					gemLayout.AddChildToLayout (detailsLabel, 1, 12);
					gemLayout.AddChildToLayout (nextBtn, 47, 19);
				}

				gemLayout.ClassId = gemModel.ID;
				gemLayout.GestureRecognizers.Add(DetailsTapgesture);
				bgStack.GestureRecognizers.Add(DetailsTapgesture);

				if (!string.IsNullOrEmpty(gemModel.GroupTitle)) {
					previousTitle = gemModel.GroupTitle.Trim();
				}

				listViewContainer.Children.Add(gemLayout);

				gemModel = null; // to clear mem.

				return true;
			} catch (Exception ex) {
				var test = ex.Message;
			}

			return false;
		}

		async void DetailsTapgesture_Tapped (object sender, EventArgs e)
		{
			try {
				if(progressBar == null)
				progressBar = DependencyService.Get<IProgressBar>();

				progressBar.ShowProgressbar("Retriving details");

				string btnId = "0";

				try {
					var senderType = sender.GetType();
					if (senderType == typeof(CustomLayout))
					{
						btnId = (sender as CustomLayout).ClassId;
					}
					else if(senderType == typeof(Image))
					{
						btnId = (sender as Image).ClassId;
					}
					else if(senderType == typeof(StackLayout))
					{
						btnId = (sender as StackLayout).ClassId;
					}
				} catch (Exception ) {
					
				}

				if(btnId == "0")
				{
					progressBar.HideProgressbar();
					return;
				}

				if (App.isEmotionsListing) {
					try {
						SelectedEventDetails eventDetails = await ServiceHelper.GetSelectedEventDetails(btnId);
						if (eventDetails != null)
						{

							List<string> listToDownload = new List<string>();

							foreach (var eventi in eventDetails.event_media) 
							{
								if(string.IsNullOrEmpty(eventi.event_media))
								{
									continue;
								}

								if (eventi.media_type == "png" || eventi.media_type == "jpg" || eventi.media_type == "jpeg") 
								{

									listToDownload.Add(Constants.SERVICE_BASE_URL+eventi.event_media);
									string fileName = System.IO.Path.GetFileName(eventi.event_media);
									eventi.event_media = App.DownloadsPath + fileName;
								}
								else
								{
									eventi.event_media = Constants.SERVICE_BASE_URL + eventi.event_media ;
								}
							}

							if (listToDownload != null && listToDownload.Count > 0) {
								IDownload downloader = DependencyService.Get<IDownload>();
								//progressBar.ShowProgressbar("loading details..");
								await downloader.DownloadFiles(listToDownload);

							}

							DetailsPageModel model = new DetailsPageModel();
							model.actionMediaArray = null;
							model.eventMediaArray = eventDetails.event_media;
							model.goal_media = null;
							model.Media = null;
							model.NoMedia = null;
							model.pageTitleVal = "Event Details";
							model.titleVal = eventDetails.event_title;
							model.description = eventDetails.event_details;
							model.gemType = GemType.Event;
							model.gemId = btnId;
							if (progressBar != null) {
								progressBar.HideProgressbar();
							}
							isLoadingFromDetailsPage = true;

							await Navigation.PushAsync(new GemsDetailsPage(model));
							eventDetails = null;
						}
					} catch (Exception ) {
						
					}
				}
				else
				{
					//-- call service for Action details
					try {

						SelectedActionDetails actionDetails = await ServiceHelper.GetSelectedActionDetails(btnId);

						List<string> listToDownload = new List<string>();

						foreach (var action in actionDetails.action_media) 
						{
							if( string.IsNullOrEmpty(action.action_media))
							{
								continue;
							}

							if (action.media_type == "png" || action.media_type == "jpg" || action.media_type == "jpeg")
							{

								listToDownload.Add(Constants.SERVICE_BASE_URL+action.action_media);
								string fileName = System.IO.Path.GetFileName(action.action_media);
								action.action_media = App.DownloadsPath + fileName;
							}
							else
							{
								action.action_media = Constants.SERVICE_BASE_URL + action.action_media;
							}
						}

						if (listToDownload != null && listToDownload.Count > 0) {
							IDownload downloader = DependencyService.Get<IDownload>();
							//progressBar.ShowProgressbar("loading details..");
							await downloader.DownloadFiles(listToDownload);
							//progressBar.HideProgressbar();
						}

						if (actionDetails != null) {
							DetailsPageModel model = new DetailsPageModel();
							model.actionMediaArray = actionDetails.action_media;
							model.eventMediaArray = null;
							model.goal_media = null;
							model.Media = null;
							model.NoMedia = null;
							model.pageTitleVal = "Action Details";
							model.titleVal = actionDetails.action_title;
							model.description = actionDetails.action_details;
							model.gemType = GemType.Action;
							model.gemId = btnId;
							if (progressBar != null) {
								progressBar.HideProgressbar();
							}
							isLoadingFromDetailsPage = true;

							await Navigation.PushAsync(new GemsDetailsPage(model));
							actionDetails = null;
						}
					} catch (Exception ) {
						
					}
				}
				progressBar.HideProgressbar();
			}
			catch (Exception ex)
			{
				if (progressBar != null) {
					progressBar.HideProgressbar();
				}
				var test = ex.Message;
			}
		}

		async void OnScroll(object sender, ScrolledEventArgs e)
		{
			try {
				if(progressBar == null)
				{
					progressBar = DependencyService.Get<IProgressBar>();
				}
				if (masterScroll.Height+ masterScroll.ScrollY > (masterStack.Height - masterStack.Y-20)) {//Device.OnPlatform (512, 550, 0)
					masterScroll.Scrolled -= OnScroll;
					if (!displayedLastGem) {
						//progressBar.ShowProgressbar ("loading gems..");
						await LoadMoreGemsClicked ();
						//progressBar.HideProgressbar ();
					} else {
						progressBar.ShowToast ("Reached end of the list..");
					}
					await Task.Delay (TimeSpan.FromSeconds (2));
					masterScroll.Scrolled += OnScroll;
				} else if (masterScroll.ScrollY < Device.OnPlatform (-15, 10, 0)) {
					masterScroll.Scrolled -= OnScroll;
					if (!reachedFront) {
						//progressBar.ShowProgressbar ("Lading gems..");
						await LoadPreviousGems ();
						//progressBar.HideProgressbar ();
					} else {
						progressBar.ShowToast ("Reached starting of the list..");
					}
					await Task.Delay (TimeSpan.FromSeconds (2));
					masterScroll.Scrolled += OnScroll;
				}
			} catch (Exception ex) {
				progressBar.HideProgressbar ();                                                                         
			}
		}

		async Task<bool>LoadMoreGemsClicked ()
		{
			try 
			{
				if (App.isEmotionsListing) { // the gems displayed will be Events for each emotion.
					await AddEventsToView (lastGemIndexOnDisplay);
				}
				else
				{
					bool isSuccess = await AddActionsToView(lastGemIndexOnDisplay, true);
				}
				await masterScroll.ScrollToAsync( 0, 0, false );
			}
			catch (Exception)
			{
				DisplayAlert ( Constants.ALERT_TITLE, "Low memory error.", Constants.ALERT_OK );
				return false;
			}
			return true;
		}

		async Task<bool> LoadPreviousGems()
		{
			try {
				if (App.isEmotionsListing) { // the gems displayed will be Events for each emotion.
					await AddEventsToView (firstGemIndexOnDisplay, false); // false = load previous gems
				}
				else {
					await AddActionsToView(firstGemIndexOnDisplay, false); // false = load previous gems
				}
				await masterScroll.ScrollToAsync( 0, masterStack.Height - Device.OnPlatform( 512, 550, 0 ), false );
				//await masterScroll.ScrollToAsync( 0, masterStack.Height - masterStack.Y-20, false );
			}
			catch (Exception) {
				return false;
			}
			return true;
		}

		void OnImageAreaTapGestureRecognizerTapped(object sender, EventArgs e)
		{
			App.masterPage.IsPresented = !App.masterPage.IsPresented;
		}

		public void Dispose()
		{
			masterLayout = null;
			progressBar = null;
			mainTitleBar = null;
			masterScroll = null;
			masterStack = null;
			eventsWithImage = null;
			actionsWithImage = null;
			emotionsButtion = null;
			goalsButton = null;
			goalsAndDreamsLabel = null;
			emotionLabel = null;
			emotionListingBtnTapgesture = null;
			goalsListingBtnTapgesture = null;
			listViewContainer = null;


			//GC.SuppressFinalize(this);
		}

		public void ScrollVisibleItems( int visbleIndex )
		{
		}

		//		~GemsMainPage()
		//		{
		//			Dispose ();
		//		}
		//
		//		protected virtual void Dispose(bool isDisposing)
		//		{
		//			if (isDisposing) {
		//
		//			}
		//
		//		}
		//
	}

	public class CustomGemItemModel
	{
		string description;

		public string Description
		{
			get { return description; }
			set
			{
				string trimmedText = string.Empty;
				if (value.Length > 70)
				{
					trimmedText = value.Substring(0, 70); // should adjst the height of bg img as well.
					trimmedText += "...";
				}
				else
				{
					trimmedText = value;
				}

				description = trimmedText;
			}
		}

		public string Source { get; set; }
		public string ID { get; set; }
		public string GroupTitle{ get; set;}
		public int CellIndex {
			get;
			set;
		}

		public CustomGemItemModel()
		{
		}
		public void Dispose()
		{
			GC.Collect();
		}
	}
}
