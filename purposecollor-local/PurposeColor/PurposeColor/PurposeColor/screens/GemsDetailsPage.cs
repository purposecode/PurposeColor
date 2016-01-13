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

namespace PurposeColor
{
    public class GemsDetailsPage : ContentPage, IDisposable
    {
        CustomLayout masterLayout;
        IProgressBar progressBar;
        StackLayout masterStack;
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

        public GemsDetailsPage(List<EventMedia> mediaArray, List<ActionMedia> actionMediaArray, string pageTitleVal, string titleVal, string desc, string Media, string NoMedia, string gemId, GemType gemType)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            masterScroll = new ScrollView();
            masterScroll.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();
            masterStack = new StackLayout();
            masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.FromRgb(244, 244, 244);
            mediaList = mediaArray;
            actionMediaList = actionMediaArray;
            CurrentGemId = gemId;
            CurrentGemType = gemType;
            User user = null;

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
            
			try {
				user = App.Settings.GetUser ();

                ////////////// for testing only // test //////////////
                user = new User { UserId = 2, AllowCommunitySharing = true }; // for testing only // test
                ////////////// for testing only // test //////////////


			} catch (Exception ex) {
                var test = ex.Message;
			}

            Label pageTitle = new Label();
            pageTitle.Text = pageTitleVal;
            pageTitle.TextColor = Color.Black;
            pageTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            pageTitle.FontAttributes = FontAttributes.Bold;
            pageTitle.WidthRequest = App.screenWidth * 90 / 100;
            pageTitle.HeightRequest = 50;
            pageTitle.XAlign = TextAlignment.Start;
            pageTitle.YAlign = TextAlignment.Center;
            pageTitle.FontSize = Device.OnPlatform(15, 20, 15);

            StackLayout emptyLayout = new StackLayout();
            emptyLayout.BackgroundColor = Color.White;
            emptyLayout.WidthRequest = App.screenWidth * 90 / 100;
            emptyLayout.HeightRequest = 60;

            #region TOOLS LAYOUT

            StackLayout toolsLayout = new StackLayout();
            toolsLayout.BackgroundColor = Color.White;
            toolsLayout.Spacing = 10;
            toolsLayout.Orientation = StackOrientation.Horizontal;
            toolsLayout.WidthRequest = App.screenWidth * 95 / 100;
            toolsLayout.Padding = new Thickness(10, 0, 10, 0);
            toolsLayout.TranslationY = -55;
            toolsLayout.HeightRequest = 50;

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
				VerticalOptions = LayoutOptions.Center
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
                    VerticalOptions = LayoutOptions.Center
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
                TextColor = Color.Gray
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
			title.Text = titleVal;
			title.TextColor = Color.Black;
			title.WidthRequest = App.screenWidth * 90 / 100;
			title.FontSize = Device.OnPlatform (12, 12, 12);
            title.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;

			description = new Label ();
			description.WidthRequest = App.screenWidth * 90 / 100;
			description.Text = desc;
			description.TextColor = Color.Black;
            description.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;

			masterStack.Children.Add (pageTitle);
			masterStack.Children.Add (title);
			masterStack.Children.Add (description);
			#endregion

            #region MEDIA LIST

            if (mediaList != null)
            {
                for (int index = 0; index < mediaList.Count; index++)
                {
                    TapGestureRecognizer videoTap = new TapGestureRecognizer();
                    videoTap.Tapped += OnEventVideoTapped;
                    bool isValidUrl = (mediaList[index].event_media != null && !string.IsNullOrEmpty(mediaList[index].event_media)) ? true : false;
                    string source = (isValidUrl) ? Constants.SERVICE_BASE_URL + Media + mediaList[index].event_media : Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");

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
                    masterStack.Children.Add(indicator);
                    masterStack.Children.Add(img);
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
                    string source = (isValidUrl) ? Constants.SERVICE_BASE_URL + Media + actionMediaList[index].action_media : Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");
                    string fileExtenstion = Path.GetExtension(source);
                    bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;
                    img.WidthRequest = App.screenWidth * 90 / 100;
                    img.HeightRequest = App.screenWidth * 90 / 100;
                    img.Aspect = Aspect.AspectFill;
                    img.ClassId = null;
                    if (actionMediaList[index] != null && actionMediaList[index].media_type == "mp4")
                    {
                        source = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                        img.ClassId = source;
                    }
                    else if (actionMediaList[index] != null && actionMediaList[index].media_type == "3gpp")
                    {
                        source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                        img.ClassId = source;
                    }
                    else if (actionMediaList[index] != null && actionMediaList[index].media_type == "wav")
                    {
                        source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                        img.ClassId = source;
                    }
                    img.Source = source;
                    img.GestureRecognizers.Add(videoTap);
                    img.ClassId = (!isImage) ? source : null;
                    var indicator = new ActivityIndicator { Color = new Color(.5), };
                    indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
                    indicator.BindingContext = img;
                    masterStack.Children.Add(indicator);
                    masterStack.Children.Add(img);
                    //masterStack.Children.Add(img);
                }
            }
            
            #endregion

            masterStack.Children.Add(emptyLayout);
            masterStack.Children.Add(toolsLayout);

            StackLayout spaceOffsetlayout = new StackLayout();
            spaceOffsetlayout.WidthRequest = App.screenWidth * 50 / 100;
            spaceOffsetlayout.HeightRequest = Device.OnPlatform(0, 100, 250);
            spaceOffsetlayout.BackgroundColor = Color.Transparent;
            masterStack.Children.Add(spaceOffsetlayout);

            masterScroll.HeightRequest = App.screenHeight - 20;
            masterScroll.WidthRequest = App.screenWidth * 90 / 100;

            masterScroll.Content = masterStack;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(masterScroll, 5, 18);
            Content = masterLayout;
        }

        async void CommentButtonTapped(object sender, EventArgs e)
        {

            //show comments popup
			try {
                progressBar.ShowProgressbar("Loading..");

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
				
				progressBar.ShowProgressbar ("Sharing..");
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
                progressBar.ShowProgressbar("Requesting..");
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

