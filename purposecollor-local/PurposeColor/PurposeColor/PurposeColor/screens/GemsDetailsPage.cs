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
        List<string> CommentsList = null;

        public GemsDetailsPage(List<EventMedia> mediaArray, List<ActionMedia> actionMediaArray, string pageTitleVal, string titleVal, string desc, string Media, string NoMedia, string GoalEventId)
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

            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Goal Enabling Materials", false);
			subTitleBar.BackButtonTapRecognizer.Tapped += (object sender, EventArgs e) => 
			{
				Navigation.PopAsync();
			};

            CommentsList = new List<string>();

            Label pageTitle = new Label();
            pageTitle.Text = pageTitleVal;
            pageTitle.TextColor = Color.Black;
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

            StackLayout toolsLayout = new StackLayout();
            toolsLayout.BackgroundColor = Color.White;
            toolsLayout.Spacing = 5;
            toolsLayout.Orientation = StackOrientation.Horizontal;
            toolsLayout.WidthRequest = App.screenWidth * 95 / 100;
            toolsLayout.Padding = new Thickness(10, 0, 10, 0);
            toolsLayout.TranslationY = -55;
            //toolsLayout.HeightRequest = 100;

            Image likeButton = new Image();
            likeButton.Source = Device.OnPlatform("like.png", "like.png", "//Assets//like.png");
            likeButton.WidthRequest = Device.OnPlatform(15, 15, 15);
            likeButton.HeightRequest = Device.OnPlatform(15, 15, 15);

            Image shareButton = new Image();
            shareButton.Source = Device.OnPlatform("share.png", "share.png", "//Assets//share.png");
            shareButton.WidthRequest = Device.OnPlatform(15, 15, 15);
            shareButton.HeightRequest = Device.OnPlatform(15, 15, 15);

            CustomEntry comment = new CustomEntry();
            comment.BackGroundImageName = Device.OnPlatform("comnt_box.png", "comnt_box.png", "//Assets//comnt_box.png");
            comment.BackgroundColor = Color.White;
            comment.Placeholder = "Add comments";
            comment.WidthRequest = App.screenWidth * 70 / 100;
            comment.HeightRequest = Device.OnPlatform(50, 50, 73);

            toolsLayout.Children.Add(likeButton);
            toolsLayout.Children.Add(shareButton);
            toolsLayout.Children.Add(comment);
            
            #region POST COMMENT
            Label postLabel = new Label
            {
                Text = "post",
                TextColor = Constants.BLUE_BG_COLOR,
                BackgroundColor = Color.Transparent,
                FontSize = Device.OnPlatform(12, 12, 15),
                HeightRequest = Device.OnPlatform(15, 25, 25),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };
            toolsLayout.Children.Add(postLabel);
            TapGestureRecognizer postTap = new TapGestureRecognizer();
            bool isPosting = false;

            postTap.Tapped += async (s, e) =>
            {
                try
                {
                    if (isPosting == true)
                    {
                        return;
                    }

                    isPosting = true;

                    if (!string.IsNullOrWhiteSpace(comment.Text))
                    {
                        User user = App.Settings.GetUser();

                        ////////////// for testing  //test //////////////
                        user = new User
                        {
                            UserId = 2,
                            ShareToCommunity = 1,
                        };
                        ////////////// for testing  //test //////////////

                        if (user == null)
                        {
                            isPosting = false;
                            return;
                        }

                        string statusCode = await PurposeColor.Service.ServiceHelper.AddComment(user.UserId.ToString(), GoalEventId, comment.Text.Trim(), user.ShareToCommunity.ToString());
                        if (statusCode == "200")
                        {
                            await DisplayAlert(Constants.ALERT_TITLE, "Comment saved", Constants.ALERT_OK);
                            isPosting = false;
                            if (CommentsList == null)
                            {
                                CommentsList = new List<string>();
                            }
                            CommentsList.Add(comment.Text.Trim());
                            comment.Text = string.Empty;

                            //// add the comment as a label below the image ///


                        }
                        else
                        {
                            await DisplayAlert(Constants.ALERT_TITLE, "Could not save the comment now, please try again later", Constants.ALERT_OK);
                        }
                    }
                    else
                    {
                        isPosting = false;
                    }

                }
                catch (Exception)
                {
                    isPosting = false;
                }
            };
            postLabel.GestureRecognizers.Add(postTap);
            //StackLayout commentContainer = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    VerticalOptions = LayoutOptions.Center,
            //    HorizontalOptions = LayoutOptions.Center,
            //    BackgroundColor = Color.Transparent,
            //    Spacing = 1,
            //    HeightRequest = 70,
            //    Children =
            //    {
            //        comment,
            //        postLabel
            //    }
            //};
            //toolsLayout.Children.Add(commentContainer);

            
            #endregion

            title = new Label();
            title.Text = titleVal;
            title.TextColor = Color.Black;
            title.WidthRequest = App.screenWidth * 90 / 100;
            title.FontSize = Device.OnPlatform(12, 12, 12);

            description = new Label();
            description.WidthRequest = App.screenWidth * 90 / 100;
            description.Text = desc;
            description.TextColor = Color.Black;


            masterStack.Children.Add(pageTitle);
            masterStack.Children.Add(title);
            masterStack.Children.Add(description);

            #region MEDIA LIST

            if (mediaList != null)
            {
                for (int index = 0; index < mediaList.Count; index++)
                {
                    TapGestureRecognizer videoTap = new TapGestureRecognizer();
                    videoTap.Tapped += OnEventVideoTapped;
                    bool isValidUrl = (mediaList[index].event_media != null && !string.IsNullOrEmpty(mediaList[index].event_media)) ? true : false;
                    string source = (isValidUrl) ? Constants.SERVICE_BASE_URL + Media + mediaList[index].event_media : comment.BackGroundImageName = Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");
                    string fileExtenstion = Path.GetExtension(source);
                    bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;
                    Image img = new Image();
                    img.WidthRequest = App.screenWidth * 90 / 100;
                    img.HeightRequest = App.screenWidth * 90 / 100;
                    img.Aspect = Aspect.AspectFill;
                    img.Source = (isImage) ? source : "video.png";
                    img.GestureRecognizers.Add(videoTap);
                    img.ClassId = (!isImage) ? source : null;
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
                    bool isValidUrl = (actionMediaList[index].event_media != null && !string.IsNullOrEmpty(actionMediaList[index].event_media)) ? true : false;
                    string source = (isValidUrl) ? Constants.SERVICE_BASE_URL + Media + actionMediaList[index].event_media : comment.BackGroundImageName = Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");
                    string fileExtenstion = Path.GetExtension(source);
                    bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;
                    img.WidthRequest = App.screenWidth * 90 / 100;
                    img.HeightRequest = App.screenWidth * 90 / 100;
                    img.Aspect = Aspect.AspectFill;
                    img.Source = (isImage) ? source : "video.png";
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
            spaceOffsetlayout.HeightRequest = Device.OnPlatform(0, 100, 200);
            spaceOffsetlayout.BackgroundColor = Color.Transparent;
            masterStack.Children.Add(spaceOffsetlayout);


            masterScroll.HeightRequest = App.screenHeight - 10;
            masterScroll.WidthRequest = App.screenWidth * 90 / 100;

            masterScroll.Content = masterStack;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(masterScroll, 5, 18);
            Content = masterLayout;

            /*	Content = new StackLayout
                {
                    BackgroundColor = Color.Red,
                    WidthRequest = 500,
                    HeightRequest = 900
                };*/
        }


        void OnActionVideoTapped(object sender, EventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                string fileName = Path.GetFileName(img.ClassId);
                if (fileName != null)
                {
                    if (Device.OS != TargetPlatform.WinPhone)
                    {
                        IVideoDownloader videoDownload = DependencyService.Get<IVideoDownloader>();
                        videoDownload.Download(img.ClassId, fileName);
                    }

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
					if (Device.OS  != TargetPlatform.WinPhone ) 
					{
                        IVideoDownloader videoDownload = DependencyService.Get<IVideoDownloader>();
                        videoDownload.Download(img.ClassId, fileName);
					}
                }

            }

        }

        protected override bool OnBackButtonPressed ()
		{
			Dispose ();
			return base.OnBackButtonPressed ();
		}

        public void Dispose()
        {
            masterScroll = null;
            Content = null;
            masterLayout = null;
			progressBar = null;
			masterStack = null;
			masterScroll = null;
			title = null;
			description = null;
			mediaList = null;
			actionMediaList = null;
			this.Content = null;
			GC.Collect ();
        }
    }

}

