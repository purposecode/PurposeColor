using System;
using Xamarin.Forms;
using CustomControls;
using PurposeColor.interfaces;
using System.Collections;
using PurposeColor.Model;
using System.Collections.Generic;
using PurposeColor.CustomControls;
using System.Diagnostics;
using Xam.Plugin.DownloadManager.Abstractions;
using System.IO;
using System.Linq;

namespace PurposeColor
{
    public class GemsDetailsPage : ContentPage, IDisposable
    {
        CustomLayout masterLayout = null;
        IProgressBar progressBar;
        CustomLayout masterStack;
        ScrollView masterScroll;
        Label title;
        Label description;
        List<EventMedia> mediaList { get; set; }
        List<ActionMedia> actionMediaList { get; set; }
        string CurrentGemId = string.Empty;
        GemType CurrentGemType = GemType.Goal;
        PurposeColorTitleBar mainTitleBar;
        PurposeColorSubTitleBar subTitleBar;
		TapGestureRecognizer shareButtonTap;
		TapGestureRecognizer commentButtonTap;
		TapGestureRecognizer favoriteButtonTap;
		Label shareLabel;
        StackLayout gemMenuContainer;
		bool IsNavigationFrfomGEMS = false;

        //public GemsDetailsPage(List<EventMedia> mediaArray, List<ActionMedia> actionMediaArray, string pageTitleVal, string titleVal, string desc, string Media, string NoMedia, string gemId, GemType gemType)
        public GemsDetailsPage( DetailsPageModel model )
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            masterScroll = new ScrollView();
            masterScroll.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();
            masterStack = new CustomLayout();
            //masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.FromRgb(244, 244, 244);
            mediaList = model.mediaArray;
            actionMediaList = model.actionMediaArray;
            CurrentGemId = model.gemId;
            CurrentGemType = model.gemType;
            User user = null;
			IsNavigationFrfomGEMS = model.fromGEMSPage;
            try
            {
                user = App.Settings.GetUser();
                ////////////// for testing only // test //////////////
                user = new User { UserId = 2, AllowCommunitySharing = true }; // for testing only // test
                ////////////// for testing only // test //////////////
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Goal Enabling Materials", false);
            subTitleBar.BackButtonTapRecognizer.Tapped += async (object sender, EventArgs e) =>
            {
                try
				{
					await Navigation.PopAsync();
                }
				catch (Exception){
                }
            };
            

            Label pageTitle = new Label();
            pageTitle.Text = model.pageTitleVal;
            pageTitle.TextColor = Color.Black;
            pageTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            pageTitle.FontAttributes = FontAttributes.Bold;
            pageTitle.WidthRequest = App.screenWidth * 80 / 100;
            pageTitle.HeightRequest = 50;
            pageTitle.XAlign = TextAlignment.Start;
            pageTitle.YAlign = TextAlignment.Start;
            pageTitle.FontSize = Device.OnPlatform(15, 20, 15);

            StackLayout emptyLayout = new StackLayout();
            emptyLayout.BackgroundColor = Color.Transparent;
            emptyLayout.WidthRequest = App.screenWidth * 90 / 100;
            emptyLayout.HeightRequest = 30;

            #region TOOLS LAYOUT

            StackLayout toolsLayout = new StackLayout();
            toolsLayout.BackgroundColor = Color.White;
            toolsLayout.Spacing = 10;
            toolsLayout.Orientation = StackOrientation.Horizontal;
            toolsLayout.WidthRequest = App.screenWidth * 95 / 100;
            toolsLayout.Padding = new Thickness(10, 10, 10, 10);
            //toolsLayout.TranslationY = -55;
            //toolsLayout.HeightRequest = 50;

			Image favoriteButton = new Image();
			favoriteButton.Source = Device.OnPlatform("favoriteIcon.png", "favoriteIcon.png", "//Assets//favoriteIcon.png");
            favoriteButton.WidthRequest = Device.OnPlatform(15, 20, 15);
            favoriteButton.HeightRequest = Device.OnPlatform(15, 20, 15);
			favoriteButton.VerticalOptions = LayoutOptions.Center;
            favoriteButtonTap = new TapGestureRecognizer();
            favoriteButtonTap.Tapped += FavoriteButtonTapped;
            favoriteButton.GestureRecognizers.Add(favoriteButtonTap);

            Label favoriteLabel = new Label
            {
				Text = "Favorite",
                FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
                TextColor = Color.Gray,
				VerticalOptions = LayoutOptions.Center,
				FontSize = Device.OnPlatform(12,12,15)
            };
            favoriteLabel.GestureRecognizers.Add(favoriteButtonTap);

            toolsLayout.Children.Add(favoriteButton);
            toolsLayout.Children.Add(favoriteLabel);

            if (user.AllowCommunitySharing)
            {
                Image shareButton = new Image();
                shareButton.Source = Device.OnPlatform("share.png", "share.png", "//Assets//share.png");
                shareButton.WidthRequest = Device.OnPlatform(15, 15, 15);
                shareButton.HeightRequest = Device.OnPlatform(15, 15, 15);
                shareButton.VerticalOptions = LayoutOptions.Center;
                shareLabel = new Label
                {
                    Text = "Share",
                    FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
					FontSize = Device.OnPlatform(12,12,15)
                };
                shareButtonTap = new TapGestureRecognizer();
                shareButtonTap.Tapped += ShareButtonTapped;
                shareButton.GestureRecognizers.Add(shareButtonTap);
                shareLabel.GestureRecognizers.Add(shareButtonTap);
                toolsLayout.Children.Add(shareButton);
                toolsLayout.Children.Add(shareLabel);
            }
            
            

            Image commentButton = new Image();
            commentButton.Source = Device.OnPlatform("icon_cmnt.png", "icon_cmnt.png", "//Assets//icon_cmnt.png");
            commentButton.WidthRequest = Device.OnPlatform(15, 15, 15);
            commentButton.HeightRequest = Device.OnPlatform(15, 15, 15);
			commentButton.VerticalOptions = LayoutOptions.Center;
            Label commentsLabel = new Label
            {
				Text = "Comment",
				VerticalOptions = LayoutOptions.Center,
                FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
                TextColor = Color.Gray,
				FontSize = Device.OnPlatform(12,12,15)
            };

            commentButtonTap = new TapGestureRecognizer();
            commentButtonTap.Tapped += CommentButtonTapped;
            commentButton.GestureRecognizers.Add(commentButtonTap);
            commentsLabel.GestureRecognizers.Add(commentButtonTap);

            toolsLayout.Children.Add(commentButton);
            toolsLayout.Children.Add(commentsLabel);
            
            #endregion

            #region POST COMMENT
            //Label postLabel = new Label
            //{
            //    Text = "post",
            //    TextColor = Constants.BLUE_BG_COLOR,
            //    BackgroundColor = Color.Transparent,
            //    FontSize = Device.OnPlatform(12, 12, 15),
            //    HeightRequest = Device.OnPlatform(15, 25, 25),
            //    HorizontalOptions = LayoutOptions.End,
            //    VerticalOptions = LayoutOptions.Center
            //};
            //toolsLayout.Children.Add(postLabel);
            //TapGestureRecognizer postTap = new TapGestureRecognizer();
            //bool isPosting = false;

            
            //postLabel.GestureRecognizers.Add(postTap);

            #endregion

			#region  title, description
			title = new Label ();
			title.Text = model.titleVal;
			title.TextColor = Color.Black;
			title.WidthRequest = App.screenWidth * 90 / 100;
			title.FontSize = Device.OnPlatform (12, 12, 12);
            title.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;

			description = new Label ();
			description.WidthRequest = App.screenWidth * .80;
			description.Text = model.desc;
			description.TextColor = Color.Black;
            description.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            
            CustomImageButton menuButton = new CustomImageButton
            {
                ImageName = Device.OnPlatform("downarrow.png", "downarrow.png", "//Assets//downarrow.png"),
                Text = string.Empty,
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = Color.Transparent,
                WidthRequest = 30,
                HeightRequest = 20
            };
            menuButton.Clicked += GemMenuButton_Clicked;

            //gemMenuContainer = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    BackgroundColor = Color.Yellow,
            //    //WidthRequest = App.screenWidth * 55,
            //    Spacing = 1,
            //    Children = { menuButton}
            //};


            //StackLayout GemTitleAndMenuContainer = new StackLayout
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    Spacing = 0,
            //    Children = { pageTitle, gemMenuContainer }
            //};
			//masterStack.Children.Add (pageTitle);
//            masterStack.AddChildToLayout(GemTitleAndMenuContainer,5,20);

            masterStack.AddChildToLayout(pageTitle, 1, 1);
            masterStack.AddChildToLayout(menuButton, 79, 1);
            
            masterStack.AddChildToLayout(title,1,7);
            //masterStack.AddChildToLayout(description, 1, 10);
			#endregion

            StackLayout bottomAndLowerControllStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Transparent,
                Spacing = 1,
                Padding = new Thickness(0, 5, 0, 5),
                WidthRequest = App.screenWidth * .90
            };
            bottomAndLowerControllStack.Children.Add(new StackLayout { WidthRequest = App.screenWidth * .80, Children = { description } });
            
            #region MEDIA LIST

            if (mediaList != null)
            {
                for (int index = 0; index < mediaList.Count; index++)
                {
                    TapGestureRecognizer videoTap = new TapGestureRecognizer();
                    videoTap.Tapped += OnEventVideoTapped;
                    bool isValidUrl = (mediaList[index].event_media != null && !string.IsNullOrEmpty(mediaList[index].event_media)) ? true : false;
                    string source = (isValidUrl) ? Constants.SERVICE_BASE_URL + model.Media + mediaList[index].event_media : Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");

                    Image img = new Image();
                    img.WidthRequest = App.screenWidth * 90 / 100;
                    img.HeightRequest = App.screenWidth * 90 / 100;
                    img.Aspect = Aspect.AspectFill;
                    img.ClassId = null;
                    if (mediaList[index] != null && mediaList[index].media_type == "mp4")
                    {
                        img.ClassId = source;
                        source = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                    }
                    else if (mediaList[index] != null && mediaList[index].media_type == "3gpp")
                    {
                        img.ClassId = source;
                        source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
                    else if (mediaList[index] != null && mediaList[index].media_type == "wav")
                    {
                        img.ClassId = source;
                        source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
                    img.Source = source;
                    img.GestureRecognizers.Add(videoTap);

                    var indicator = new ActivityIndicator { Color = new Color(.5), };
                    indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
                    indicator.BindingContext = img;
                    masterStack.AddChildToLayout(indicator, 40, 30);
                    //masterStack.AddChildToLayout(img, 1, 15);
                    bottomAndLowerControllStack.Children.Add(img);
                }
            }

            if (actionMediaList != null)
            {
                for (int index = 0; index < actionMediaList.Count; index++)
                {
                    TapGestureRecognizer videoTap = new TapGestureRecognizer();
                    videoTap.Tapped += OnActionVideoTapped;

                    Image img = new Image();
                    bool isValidUrl = (actionMediaList[index].action_media != null && !string.IsNullOrEmpty(actionMediaList[index].action_media)) ? true : false;
                    string source = (isValidUrl) ? Constants.SERVICE_BASE_URL + model.Media + actionMediaList[index].action_media : Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");
                    string fileExtenstion = Path.GetExtension(source);
                    bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;
                    img.WidthRequest = App.screenWidth * 90 / 100;
                    img.HeightRequest = App.screenWidth * 90 / 100;
                    img.Aspect = Aspect.AspectFill;
                    img.ClassId = null;
                    if (actionMediaList[index] != null && actionMediaList[index].media_type == "mp4")
                    {
                        img.ClassId = source;
                        source = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                    }
                    else if (actionMediaList[index] != null && actionMediaList[index].media_type == "3gpp")
                    {
                        img.ClassId = source;
                        source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
                    else if (actionMediaList[index] != null && actionMediaList[index].media_type == "wav")
                    {
                        img.ClassId = source;
                        source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
                    img.Source = source;
                    img.GestureRecognizers.Add(videoTap);
                    img.ClassId = (!isImage) ? source : null;
                    var indicator = new ActivityIndicator { Color = new Color(.5), };
                    indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
                    indicator.BindingContext = img;
                    masterStack.AddChildToLayout(indicator, 40, 30);
                    //masterStack.AddChildToLayout(img, 1, 15);
                    bottomAndLowerControllStack.Children.Add(img);
                }
            }


            if (model.goal_media != null)
            {
                for (int index = 0; index < model.goal_media.Count; index++)
                {
                    TapGestureRecognizer videoTap = new TapGestureRecognizer();
                    videoTap.Tapped += OnActionVideoTapped;

                    Image img = new Image();
                    bool isValidUrl = (model.goal_media[index].goal_media != null && !string.IsNullOrEmpty(model.goal_media[index].goal_media)) ? true : false;
                    string source = (isValidUrl) ? Constants.SERVICE_BASE_URL + model.goal_media[index].goal_media : Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");
                    string fileExtenstion = Path.GetExtension(source);
                    bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;
                    img.WidthRequest = App.screenWidth * 90 / 100;
                    img.HeightRequest = App.screenWidth * 90 / 100;
                    img.Aspect = Aspect.AspectFill;
                    img.ClassId = null;
                    if (model.goal_media[index] != null && model.goal_media[index].media_type == "mp4")
                    {
                        img.ClassId = source;
                        source = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                    }
                    else if (model.goal_media[index] != null && model.goal_media[index].media_type == "3gpp")
                    {
                        img.ClassId = source;
                        source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
                    else if (model.goal_media[index] != null && model.goal_media[index].media_type == "wav")
                    {
                        img.ClassId = source;
                        source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
                    img.Source = source;
                    img.GestureRecognizers.Add(videoTap);
                    var indicator = new ActivityIndicator { Color = new Color(.5), };
                    indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
                    indicator.BindingContext = img;
                    masterStack.AddChildToLayout(indicator, 40, 30);
                    bottomAndLowerControllStack.Children.Add(img);
                }
            }
            
            #endregion

            //masterStack.AddChildToLayout(toolsLayout,1,65);
            //masterStack.AddChildToLayout(emptyLayout,1,75);
            bottomAndLowerControllStack.Children.Add(toolsLayout);
            bottomAndLowerControllStack.Children.Add(emptyLayout);
            masterStack.AddChildToLayout(bottomAndLowerControllStack, 1, 12);

            StackLayout spaceOffsetlayout = new StackLayout();
            spaceOffsetlayout.WidthRequest = App.screenWidth * 50 / 100;
            spaceOffsetlayout.HeightRequest = Device.OnPlatform(100, 100, 250);
            spaceOffsetlayout.BackgroundColor = Color.Transparent;
            //masterStack.AddChildToLayout(spaceOffsetlayout, 1, 85);
            bottomAndLowerControllStack.Children.Add(spaceOffsetlayout);

            masterScroll.HeightRequest = App.screenHeight - 20;
            masterScroll.WidthRequest = App.screenWidth * 90 / 100;

            masterScroll.Content = masterStack;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(masterScroll, 5, 18);

            #region CUSTOM LIST MENU

            //new CustomListViewItem { EmotionID = item.EmotionId.ToString(), Name = item.EmpotionName, SliderValue = item.EmotionValue  }
            
            #endregion
            Content = masterLayout;
        }

        void GemMenuButton_Clicked(object sender, EventArgs e)
        {
            View menuView = masterStack.Children.FirstOrDefault(pick => pick.ClassId == Constants.CUSTOMLISTMENU_VIEW_CLASS_ID);
            if (menuView != null)
            {
                HideCommentsPopup();
                return;
            }
            
            List<CustomListViewItem> menuItems = new List<CustomListViewItem>();
            menuItems.Add(new CustomListViewItem { Name = "Edit", EmotionID = CurrentGemId.ToString(),EventID = string.Empty, SliderValue = 0 });
            menuItems.Add(new CustomListViewItem { Name = "Hide", EmotionID = CurrentGemId.ToString(), EventID = string.Empty, SliderValue = 0 });
            menuItems.Add(new CustomListViewItem { Name = "Delete", EmotionID = CurrentGemId.ToString(), EventID = string.Empty, SliderValue = 0 });

            PurposeColor.screens.CustomListMenu GemMenu = new screens.CustomListMenu(masterLayout, gemMenuContainer, menuItems, 0, 10, 50);
            //GemMenu.WidthRequest = App.screenWidth * .50;
            //GemMenu.HeightRequest = App.screenHeight * .40;
            GemMenu.ClassId = Constants.CUSTOMLISTMENU_VIEW_CLASS_ID;
            GemMenu.listView.ItemSelected += GemMenu_ItemSelected;
            masterStack.AddChildToLayout(GemMenu, 52, 4);
        }

        async void GemMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                CustomListViewItem item = e.SelectedItem as CustomListViewItem;
                if (item.Name == "Delete")
                {
                    // do call the delete gem api
                    // pass gem type and gem id to service helper .
					var alert = await DisplayAlert(Constants.ALERT_TITLE, "Are you sure you want to delete this GEM?", Constants.ALERT_OK, "Cancel");
					if (alert)
					{
						string responceCode = await PurposeColor.Service.ServiceHelper.DeleteGem(item.EmotionID, CurrentGemType);
						if (responceCode == "200")
						{
							await DisplayAlert(Constants.ALERT_TITLE, "GEM deleted.", Constants.ALERT_OK);
							try
							{
								if (IsNavigationFrfomGEMS) {
									//nav to gems main page
									//await Navigation.PopAsync();
									App.masterPage.IsPresented = false;
									App.masterPage.Detail = new NavigationPage(new PurposeColor.screens.GemsMainPage());
								}else
								{
									// nav to goals n dreams
									App.masterPage.IsPresented = false;
									App.masterPage.Detail = new NavigationPage(new PurposeColor.screens.GoalsMainPage());
								}
							}
							catch (Exception)
							{
							}
						}
						else if (responceCode == "404")
						{
							await DisplayAlert(Constants.ALERT_TITLE, "GEM alredy deleted.", Constants.ALERT_OK);
						}
						else
						{
							await DisplayAlert(Constants.ALERT_TITLE, "Please try again later.", Constants.ALERT_OK);
						}
					}
                }
                else if (item.Name == "Hide")
                {
                    // remove the community sharing of current gem
                }
                else if (item.Name == "Edit")
                {

                }

                HideCommentsPopup();

            }
            catch (Exception)
            {
            }
        }

        void HideCommentsPopup()
        {
            try
            {
                View menuView = masterStack.Children.FirstOrDefault(pick => pick.ClassId == Constants.CUSTOMLISTMENU_VIEW_CLASS_ID);
                if (menuView != null)
                {
                    masterStack.Children.Remove(menuView);
                    menuView = null;
                }
            }
            catch (Exception)
            {
            }
        }

        async void CommentButtonTapped(object sender, EventArgs e)
        {

            //show comments popup
			try {
                progressBar.ShowProgressbar("Loading comments");

                List<Comment> comments = await PurposeColor.Service.ServiceHelper.GetComments(CurrentGemId, CurrentGemType, false);
                progressBar.HideProgressbar();

                PurposeColor.screens.CommentsView commentsView = new PurposeColor.screens.CommentsView(masterLayout, comments, CurrentGemId, CurrentGemType, false);
                commentsView.ClassId = Constants.COMMENTS_VIEW_CLASS_ID;
                commentsView.HeightRequest = App.screenHeight;
                commentsView.WidthRequest = App.screenWidth;
                masterLayout.AddChildToLayout(commentsView, 0, 0);
				
			} 
            catch (Exception ex) 
            {
                progressBar.HideProgressbar();
                DisplayAlert(Constants.ALERT_TITLE, "Could not fetch comments, Please try again later.", Constants.ALERT_OK);
			}
            progressBar.HideProgressbar();
        }

        async void ShareButtonTapped(object sender, EventArgs e)
        {
			string statusCode = "404";

            try {
				
				progressBar.ShowProgressbar ("Sharing to community");
				shareButtonTap.Tapped -= ShareButtonTapped;

				string actionId = "0";
				string eventId = "0";
				string goalId = "0";
				//goal_id,event_id or goalaction_id 
				switch (CurrentGemType) {
				case GemType.Goal:
					goalId = CurrentGemId;
					break;
				case GemType.Event:
					eventId = CurrentGemId;
					break;
				case GemType.Action:
					actionId = CurrentGemId;
					break;
				case GemType.Emotion:
					break;
				default:
					break;
				}
            	
				statusCode = await PurposeColor.Service.ServiceHelper.ShareToCommunity (goalId, eventId, actionId);
			}
			catch (Exception) 
			{
				shareButtonTap.Tapped += ShareButtonTapped;
			}

			progressBar.HideProgressbar();
			if (statusCode == "200") {
				progressBar.ShowToast ("GEM shard to community.");
			} else if (statusCode == "401") {
				progressBar.ShowToast ("Could not process your request");
			} else {
				progressBar.ShowToast ("Network error, Please try again later.");
                shareButtonTap.Tapped += ShareButtonTapped;
			}
		}

        async void FavoriteButtonTapped(object sender, EventArgs e)
        {
            // mark it as fav
			try {
				favoriteButtonTap.Tapped -= FavoriteButtonTapped;
                progressBar.ShowProgressbar("Requesting...   ");
                User user = App.Settings.GetUser();

                /////////////// for testing /////////////
                user = new User { UserId = 2, DisplayName = "TestUser"};
                /////////////// for testing /////////////

                if (user == null)
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Could not process the request now.", Constants.ALERT_OK);
                }
                else
                {
                    string responceCode = await PurposeColor.Service.ServiceHelper.AddToFavorite(user.UserId.ToString(), CurrentGemId, CurrentGemType);

					progressBar.HideProgressbar();

                    if (responceCode == "200")
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "GEM added to favourites.", Constants.ALERT_OK);
                    }
                    else if (responceCode == "401")
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Current GEM is alredy added to favourites.", Constants.ALERT_OK);
                    }
                    else
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Network error, Could not process the request.", Constants.ALERT_OK);
                    }
                }

			} catch (Exception ex) {
				favoriteButtonTap.Tapped += FavoriteButtonTapped;
                progressBar.HideProgressbar();
                DisplayAlert(Constants.ALERT_TITLE, "Could not process the request now.", Constants.ALERT_OK);
			}

            progressBar.HideProgressbar();
        }

        void OnActionVideoTapped(object sender, EventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                string fileName = Path.GetFileName(img.ClassId);
                if (fileName != null)
                {
                    IVideoDownloader videoDownload = DependencyService.Get<IVideoDownloader>();
                    videoDownload.Download(img.ClassId, fileName);

                }

            }
        }

        void OnEventVideoTapped(object sender, EventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                string fileName = Path.GetFileName(img.ClassId);
                if (fileName != null)
                {
                    IVideoDownloader videoDownload = DependencyService.Get<IVideoDownloader>();
                    videoDownload.Download(img.ClassId, fileName);
                }

            }

        }

        protected override bool OnBackButtonPressed ()
		{
			return base.OnBackButtonPressed ();
		}

        public void Dispose()
        {
            masterScroll = null;
            Content = null;
            masterLayout = null;
			progressBar = null;
			masterStack = null;
			title = null;
			description = null;
			mediaList = null;
			actionMediaList = null;
			this.Content = null;
			shareLabel = null;
            if (shareButtonTap != null)
            {
                shareButtonTap.Tapped -= ShareButtonTapped;
                shareButtonTap = null;
            }
            if (commentButtonTap!= null)
            {
                commentButtonTap.Tapped -= CommentButtonTapped;
                favoriteButtonTap.Tapped -= FavoriteButtonTapped;
                commentButtonTap = null;
                favoriteButtonTap = null;
            }
			

			GC.Collect ();
        }
    }

}

