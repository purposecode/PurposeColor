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
using CustomLayouts.ViewModels;
using CustomLayouts.Controls;
using CustomLayouts;
using PurposeColor.Service;
using Cross;
using System.Threading.Tasks;

namespace PurposeColor
{
    public class CommunityGems : ContentPage, IDisposable
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
        TapGestureRecognizer likeButtonTap;
        Label shareLabel;
        StackLayout gemMenuContainer;
        bool IsNavigationFrfomGEMS = false;

        //public GemsDetailsPage(List<EventMedia> mediaArray, List<ActionMedia> actionMediaArray, string pageTitleVal, string titleVal, string desc, string Media, string NoMedia, string gemId, GemType gemType)
        public CommunityGems(DetailsPageModel model)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            masterScroll = new ScrollView();
            masterScroll.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();

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
                catch (Exception)
                {
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

			BoxView emptyLayout = new BoxView();
            emptyLayout.BackgroundColor = Color.Transparent;
            emptyLayout.WidthRequest = App.screenWidth * 90 / 100;
            emptyLayout.HeightRequest = 30;

            StackLayout masterStackLayout = new StackLayout();
            masterStackLayout.Orientation = StackOrientation.Vertical;


            for (int mainIndex = 0; mainIndex < 10; mainIndex++)
            {

                masterStack = new CustomLayout();
                masterStack.BackgroundColor = Color.White;// Color.FromRgb(244, 244, 244);

                #region TOOLS LAYOUT

                StackLayout toolsLayout = new StackLayout();
                toolsLayout.BackgroundColor = Color.White;
                toolsLayout.Spacing = 10;
                toolsLayout.Orientation = StackOrientation.Horizontal;
                toolsLayout.WidthRequest = App.screenWidth * 95 / 100;
                toolsLayout.Padding = new Thickness(10, 10, 10, 10);
                //toolsLayout.TranslationY = -55;
                //toolsLayout.HeightRequest = 50;

                Image likeButton = new Image();
                likeButton.Source = Device.OnPlatform("icn_like.png", "icn_like.png", "//Assets//icn_like.png");
                likeButton.WidthRequest = Device.OnPlatform(15, 15, 15);
                likeButton.HeightRequest = Device.OnPlatform(15, 15, 15);
                likeButton.VerticalOptions = LayoutOptions.Center;
                likeButtonTap = new TapGestureRecognizer();
                likeButtonTap.Tapped += OnLikeButtonTapped;
                likeButton.GestureRecognizers.Add(likeButtonTap);

                Label likeLabel = new Label
                {
                    Text = "Like",
                    FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = Device.OnPlatform(12, 12, 15)
                };
                likeLabel.GestureRecognizers.Add(likeButtonTap);

                toolsLayout.Children.Add(likeButton);
                toolsLayout.Children.Add(likeLabel);

                Image shareButton = new Image();
                if (user.AllowCommunitySharing)
                {
                   
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
                        FontSize = Device.OnPlatform(12, 12, 15)
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
                    FontSize = Device.OnPlatform(12, 12, 15)
                };

                commentButtonTap = new TapGestureRecognizer();
                commentButtonTap.Tapped += CommentButtonTapped;
                commentButton.GestureRecognizers.Add(commentButtonTap);
                commentsLabel.GestureRecognizers.Add(commentButtonTap);

                toolsLayout.Children.Add(commentButton);
                toolsLayout.Children.Add(commentsLabel);



                Label myGemsLabel = new Label
                {
                    Text = "My Gems",
                    VerticalOptions = LayoutOptions.Center,
                    FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
                    TextColor = Color.Gray,
                    FontSize = Device.OnPlatform(12, 12, 15)
                };

                TapGestureRecognizer myGemsTap = new TapGestureRecognizer();
                myGemsTap.Tapped += OnMyGemsTapped;
                myGemsLabel.GestureRecognizers.Add(myGemsTap);
                toolsLayout.Children.Add(myGemsLabel);
                #endregion


                #region  title, description
                Image profileImage = new Image();
                profileImage.Source = "avatar.jpg";
                profileImage.WidthRequest = 60;
                profileImage.HeightRequest = 60;
                profileImage.Aspect = Aspect.Fill;

                Label userName = new Label();
                userName.Text = "Big CEO";
                userName.TextColor = Color.Black;
                userName.WidthRequest = App.screenWidth * 90 / 100;
                userName.FontAttributes = FontAttributes.Bold;
                userName.FontSize = Device.OnPlatform(14, 15, 12);
                userName.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;

                title = new Label();
                title.Text = model.titleVal;
                title.TextColor = Color.Black;
                title.WidthRequest = App.screenWidth * 90 / 100;
                title.FontSize = Device.OnPlatform(12, 12, 12);
                title.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;

                description = new Label();
                description.WidthRequest = App.screenWidth * .80;
                description.Text = model.desc;
                description.TextColor = Color.Black;
				description.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
				description.FontSize = Device.OnPlatform( 12, 15, 15 );

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

               // masterStack.AddChildToLayout(pageTitle, 1, 1);
                masterStack.AddChildToLayout(menuButton, 79, 1);
                masterStack.AddChildToLayout( profileImage, 2, 1 );
                masterStack.AddChildToLayout( userName, 23, 3 );
                masterStack.AddChildToLayout(title, 23, 7);

				TapGestureRecognizer moreTap = new TapGestureRecognizer();
				moreTap.Tapped += async (object sender, EventArgs e) => 
				{

					IProgressBar progress = DependencyService.Get<IProgressBar>();
					progress.ShowProgressbar( "Loading community gems" );
					App.masterPage.IsPresented = false;
					SelectedGoal goalInfo = await ServiceHelper.GetSelectedGoalDetails("37");
					if (goalInfo != null)
					{
						List<PurposeColor.Constants.MediaDetails> mediaPlayerList = new List<PurposeColor.Constants.MediaDetails>();
						foreach (var item in goalInfo.resultarray.goal_media) 
						{
							mediaPlayerList.Add( new PurposeColor.Constants.MediaDetails(){ImageName = item.goal_media, ID = item.goal_id, MediaType = item.media_type } );
						}
						await Navigation.PushAsync( new CommunityMediaViewer( mediaPlayerList ) );
						//  App.masterPage.Detail = new NavigationPage(new CommunityGemsPage());
					}

					progress.HideProgressbar();

				};
				Image  moreImg = new Image();
				moreImg.Source = "more.png";
				moreImg.HorizontalOptions = LayoutOptions.End;
				moreImg.VerticalOptions = LayoutOptions.End;
				moreImg.GestureRecognizers.Add( moreTap );

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

                if (model.goal_media != null)
                {
                   /* ScrollView imgScrollView = new ScrollView();
                    imgScrollView.Orientation = ScrollOrientation.Horizontal;
                    
                    StackLayout horizmgConatiner = new StackLayout();
                    horizmgConatiner.Orientation = StackOrientation.Horizontal;*/

               /*   for (int index = 0; index < model.goal_media.Count; index++)
                    {*/
					int index = 0;
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
                        shareButton.ClassId = source;
                        shareLabel.ClassId = source;
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

                        CustomLayout imgContainer = new CustomLayout();
                        imgContainer.WidthRequest = App.screenWidth * 90 / 100;
                        imgContainer.HeightRequest = App.screenWidth * 90 / 100;
                        imgContainer.Children.Add(img);
                        imgContainer.AddChildToLayout(moreImg, 75, 75, (int)imgContainer.WidthRequest, (int)imgContainer.HeightRequest);

                        bottomAndLowerControllStack.Children.Add(imgContainer);
                }

                #endregion


                bottomAndLowerControllStack.Children.Add(toolsLayout);
                masterStack.AddChildToLayout(bottomAndLowerControllStack, 1, 12);
			  //  masterStack.AddChildToLayout( moreImg, 65, Device.OnPlatform( 30, 20, 20 ) );

                //masterStack.AddChildToLayout(spaceOffsetlayout, 1, 85);
               /// bottomAndLowerControllStack.Children.Add(spaceOffsetlayout);

                masterScroll.HeightRequest = App.screenHeight - 20;
                masterScroll.WidthRequest = App.screenWidth * 90 / 100;


                masterStackLayout.Children.Add(masterStack);
            }


            masterScroll.Content = masterStackLayout;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(masterScroll, 5, 18);
          

            #region CUSTOM LIST MENU

            //new CustomListViewItem { EmotionID = item.EmotionId.ToString(), Name = item.EmpotionName, SliderValue = item.EmotionValue  }

            #endregion
            Content = masterLayout;
        }

        void OnMyGemsTapped(object sender, EventArgs e)
        {
            
        }


        protected override bool OnBackButtonPressed()
        {
            //var success =  DisplayAlert(Constants.ALERT_TITLE, "Do you want to exit from App ?", Constants.ALERT_OK, "Cancel").Result;

            Task<bool> action = DisplayAlert(Constants.ALERT_TITLE, "Do you want to exit from App ?", Constants.ALERT_OK, "Cancel");
            action.ContinueWith(task =>
            {
                bool val = task.Result;
                if (task.Result)
                {
                    CloseAllPages();

                }

            });

            return true;

        }

        private void CloseAllPages()
        {
            if (Device.OS != TargetPlatform.iOS)
            {
                Dispose();
                GC.Collect();
                IDeviceSpec device = DependencyService.Get<IDeviceSpec>();
                device.ExitApp();
            }

        }

        CarouselLayout CreatePagesCarousel( List<SelectedGoalMedia> media )
        {
            List<CarousalViewModel> Pages = new List<CarousalViewModel>();

            foreach (var item in media)
            {
                CarousalViewModel caros = new CarousalViewModel { Title = "1", Background = Color.White, ImageSource = Constants.SERVICE_BASE_URL + item.goal_media };
                Pages.Add( caros );
            }
           /* List<CarousalViewModel> Pages = new List<CarousalViewModel>() {
				new CarousalViewModel { Title = "1", Background = Color.White, ImageSource = "icon.png" },
				new CarousalViewModel { Title = "2", Background = Color.Red, ImageSource = "icon.png" },
				new CarousalViewModel { Title = "3", Background = Color.Blue, ImageSource = "one.jpeg" },
				new CarousalViewModel { Title = "4", Background = Color.Yellow, ImageSource = "icon.png" },
			};*/

            CarouselLayout carousel = new CarouselLayout
            {
                IndicatorStyle = CustomLayouts.Controls.CarouselLayout.IndicatorStyleEnum.Dots,
                ItemTemplate = new DataTemplate(typeof(CarousalTemplate))
            };
            carousel.ItemsSource = Pages;
            //carousel.SetBinding(CarouselLayout.ItemsSourceProperty, "Pages");
            //carousel.SetBinding(CarouselLayout.SelectedItemProperty, "CurrentPage", BindingMode.TwoWay);

            return carousel;
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
            menuItems.Add(new CustomListViewItem { Name = "Edit", EmotionID = CurrentGemId.ToString(), EventID = string.Empty, SliderValue = 0 });
            menuItems.Add(new CustomListViewItem { Name = "Hide", EmotionID = CurrentGemId.ToString(), EventID = string.Empty, SliderValue = 0 });
            menuItems.Add(new CustomListViewItem { Name = "Delete", EmotionID = CurrentGemId.ToString(), EventID = string.Empty, SliderValue = 0 });

            PurposeColor.screens.CustomListMenu GemMenu = new screens.CustomListMenu(masterLayout, menuItems );
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
                                if (IsNavigationFrfomGEMS)
                                {
                                    //nav to gems main page
                                    //await Navigation.PopAsync();
                                    App.masterPage.IsPresented = false;
                                    App.masterPage.Detail = new NavigationPage(new PurposeColor.screens.GemsMainPage());
                                }
                                else
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
            try
            {
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

            try
            {
                string path = "";
                Button button = sender as Button;
                Label label = sender as Label;
                if( button != null )
                {
                    if (button.ClassId != null)
                        path = button.ClassId;
                }

                if( label != null)
                {
                    if (label.ClassId != null)
                        path = label.ClassId;
                }

                IShareVia share = DependencyService.Get<IShareVia>();
                share.ShareMedia("Hello", path, Constants.MediaType.Image);
             
            }
            catch (Exception)
            {
                shareButtonTap.Tapped += ShareButtonTapped;
            }

          
        }

        async void OnLikeButtonTapped(object sender, EventArgs e)
        {
            // mark it as fav
            try
            {
                likeButtonTap.Tapped -= OnLikeButtonTapped;
                progressBar.ShowProgressbar("Requesting...   ");
                User user = App.Settings.GetUser();

                /////////////// for testing /////////////
                user = new User { UserId = 2, DisplayName = "TestUser" };
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

            }
            catch (Exception ex)
            {
                likeButtonTap.Tapped += OnLikeButtonTapped;
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

  

        public void Dispose()
        {
            masterLayout = null;
            progressBar = null;
            masterStack = null;
            title = null;
            description = null;
            mediaList = null;
            actionMediaList = null;
            shareLabel = null;
            if (shareButtonTap != null)
            {
                shareButtonTap.Tapped -= ShareButtonTapped;
                shareButtonTap = null;
            }
            if (commentButtonTap != null)
            {
                commentButtonTap.Tapped -= CommentButtonTapped;
                likeButtonTap.Tapped -= OnLikeButtonTapped;
                commentButtonTap = null;
                likeButtonTap = null;
            }

            masterScroll = null;
            GC.Collect();
        }
    }

}

