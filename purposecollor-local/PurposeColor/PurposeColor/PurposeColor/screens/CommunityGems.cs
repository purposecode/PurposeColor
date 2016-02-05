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
        ScrollView masterScroll;
        Label title;
        Label description;
		List<string> mediaList = new List<string> ();
        string CurrentGemId = string.Empty;
        GemType CurrentGemType = GemType.Goal;
        PurposeColorTitleBar mainTitleBar;
		CommunityGemSubTitleBar subTitleBar;
        TapGestureRecognizer shareButtonTap;
        TapGestureRecognizer commentButtonTap;
        TapGestureRecognizer likeButtonTap;
        Label shareLabel;
        StackLayout gemMenuContainer;
        bool IsNavigationFrfomGEMS = false;
        DetailsPageModel modelObject;
        StackLayout masterStackLayout;
        CommunityGemsObject communityGems;
		User currentUser;
		int myGemsCount = 0;
		int MAX_ROWS_AT_A_TIME = 5;

        //public GemsDetailsPage(List<EventMedia> mediaArray, List<ActionMedia> actionMediaArray, string pageTitleVal, string titleVal, string desc, string Media, string NoMedia, string gemId, GemType gemType)
        public CommunityGems(DetailsPageModel model)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            masterScroll = new ScrollView();
            masterScroll.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();
			currentUser = App.Settings.GetUser ();
			if (currentUser == null)
				currentUser = new User (){ UserId = 2 };
            modelObject = model;
            CurrentGemId = model.gemId;
            CurrentGemType = model.gemType;


            mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
			subTitleBar = new CommunityGemSubTitleBar(Constants.SUB_TITLE_BG_COLOR, Constants.COMMUNITY_GEMS, true);
			subTitleBar.myGemsTapRecognizer.Tapped += async (object sender, EventArgs e) => 
			{
				IProgressBar progress = DependencyService.Get<IProgressBar>();
				progress.ShowProgressbar( "Loading Mygems.." );

				CommunityGemsObject myGems = await ServiceHelper.GetMyGemsDetails();
				if( myGems != null )
				{
					Navigation.PushAsync( new MyGemsPage( myGems ) );
					myGemsCount = myGems.resultarray.Count;
				}
			
				progress.HideProgressbar();

			/*	masterStack.Children.Clear();
				masterStackLayout.Children.Clear();
				masterScroll.Content = null;

				RenderGems( communityGems );*/


			};
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


            this.Appearing += OnAppearing;

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

            masterStackLayout = new StackLayout();
            masterStackLayout.Orientation = StackOrientation.Vertical;


            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(masterScroll, 5, 18);


            Content = masterLayout;
        }



       async  void OnAppearing(object sender, EventArgs e)
        {
			try
			{

				/*communityGems.resultarray = new List<CommunityGemsDetails>();
				communityGems.resultarray.Add( new CommunityGemsDetails{ firstname = "test", gem_datetime = "2014 feb 14", gem_id = "68", user_id = "2"  } );*/

				if (communityGems != null)
					return;

				User user = null;
				IsNavigationFrfomGEMS = modelObject.fromGEMSPage;
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
					


				communityGems = await ServiceHelper.GetCommunityGemsDetails();

				if( communityGems == null || communityGems.resultarray == null )
				{
					var success = await DisplayAlert(Constants.ALERT_TITLE, "Error in fetching gems", Constants.ALERT_OK, Constants.ALERT_RETRY);
					if (!success) 
					{
						OnAppearing (sender, EventArgs.Empty);
						return;
					} 
					else
					{
						if (Device.OS != TargetPlatform.WinPhone)
							communityGems = App.Settings.GetCommunityGemsObject();

					}
				}
				else
				{
					App.Settings.DeleteCommunityGems();
					App.Settings.SaveCommunityGemsDetails( communityGems );
				}

				await DownloadMedias();

				if( communityGems.resultarray.Count > MAX_ROWS_AT_A_TIME )
				{
					communityGems.resultarray.RemoveRange( MAX_ROWS_AT_A_TIME, communityGems.resultarray.Count - MAX_ROWS_AT_A_TIME );
				}


				RenderGems( communityGems );



			} 
			catch (Exception ex) 
			{
				string err = ex.Message;
			}
        }



		async Task<bool>  DownloadMedias()
		{
			IDownload downloader =  DependencyService.Get<IDownload> ();
			List<string> mediaListToDownload = new List<string> ();

			foreach (var item in communityGems.resultarray)
			{
			/*	string media = item.gem_media [0].gem_media;

				string fileExtenstion = Path.GetExtension(media);
				if (fileExtenstion == ".mp4") 
				{
					media = "video.png";
				} 
				else if (fileExtenstion == "3gpp" || fileExtenstion == "wav") 
				{
					media = "audio.png";
				} 
				else
				{
					string fileName = Path.GetFileName ( media );
					media = fileName;
				}

				mediaList.Add ( media );*/
				foreach (var mediaItem in item.gem_media) 
				{

					if( mediaItem.media_type == "png" || mediaItem.media_type == "jpg" || mediaItem.media_type == "jpeg"  )
						mediaListToDownload.Add( Constants.SERVICE_BASE_URL + mediaItem.gem_media );
				}
			}

			var val = await downloader.DownloadFiles ( mediaListToDownload );
			return val;
		}

		void RenderGems( CommunityGemsObject gemsObject )
		{
			try
			{
				Button loadPreviousGems = new Button();
				loadPreviousGems.BackgroundColor = Color.Transparent;
				loadPreviousGems.TextColor = Constants.BLUE_BG_COLOR;
				loadPreviousGems.Text = "Load previous gems";
				loadPreviousGems.FontSize = 12;
				loadPreviousGems.BorderWidth = 0;
				loadPreviousGems.BorderColor = Color.Transparent;
				loadPreviousGems.Clicked += OnLoadPreviousGemsClicked;
				masterStackLayout.Children.Add( loadPreviousGems );

				foreach (var item in gemsObject.resultarray )
				{
					CustomLayout masterStack = new CustomLayout();
					masterStack.ClassId = "masterstack" + item.gem_id;
					masterStack.BackgroundColor = Color.White;// Color.FromRgb(244, 244, 244);

					#region TOOLS LAYOUT

					StackLayout toolsLayout = new StackLayout();
					toolsLayout.BackgroundColor = Color.White;
					toolsLayout.Spacing = 10;
					toolsLayout.Orientation = StackOrientation.Horizontal;
					toolsLayout.WidthRequest = App.screenWidth * 95 / 100;
					toolsLayout.Padding = new Thickness(10, 10, 10, 10);

					likeButtonTap = new TapGestureRecognizer();
					Image likeButton = new Image();
					string likeSource = ( item.like_status == 1 ) ? "icn_liked.png" : "icn_like.png";
					likeButton.Source = likeSource;
					likeButton.WidthRequest = Device.OnPlatform(15, 15, 15);
					likeButton.HeightRequest = Device.OnPlatform(15, 15, 15);
					likeButton.VerticalOptions = LayoutOptions.Center;
					likeButton.ClassId = item.gem_id;
					likeButton.GestureRecognizers.Add(likeButtonTap);

					Label likeLabel = new Label
					{
						Text = "Like",
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						TextColor = Color.Gray,
						VerticalOptions = LayoutOptions.Center,
						FontSize = Device.OnPlatform(12, 12, 15),
						ClassId = item.gem_id
					};
					likeLabel.GestureRecognizers.Add(likeButtonTap);

					Label likeCount = new Label
					{
						Text = ( item.likecount > 0 ) ? item.likecount.ToString() : "",
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						TextColor = Color.Blue,
						VerticalOptions = LayoutOptions.Center,
						FontSize = Device.OnPlatform(12, 12, 15)
					};

					toolsLayout.Children.Add(likeCount);
					toolsLayout.Children.Add(likeButton);
					toolsLayout.Children.Add(likeLabel);

					likeButtonTap.Tapped += async (object tapSender, EventArgs eTap) => 
					{
						try
						{
							string gemID = "";
							Image likeImg = tapSender as Image;
							Label like = tapSender as Label;
							Label likeCountLabel = new Label();
							if (likeImg != null)
							{
								int likeImgIndex = toolsLayout.Children.IndexOf( likeImg );
								if( likeImgIndex > 0 )
								{
									likeCountLabel =(Label) toolsLayout.Children[ likeImgIndex - 1 ];
								}
								if (likeImg.ClassId != null)
									gemID = likeImg.ClassId;
							}

							if (like != null)
							{
								int labelIndex = toolsLayout.Children.IndexOf( like );
								if( labelIndex > 0 )
								{
								    likeImg =(Image) toolsLayout.Children[ labelIndex - 1 ];
									likeCountLabel =(Label) toolsLayout.Children[ labelIndex - 2 ];
								}
								if (like.ClassId != null)
									gemID = like.ClassId;
							}

							//likeButtonTap.Tapped -= OnLikeButtonTapped;
							progressBar.ShowProgressbar("Requesting...   ");
							/////////////// for testing /////////////

							LikeResponse likeRes = await ServiceHelper.LikeGem( gemID );
							if( likeRes != null )
							{
								string source = ( likeRes.like_status == 1 ) ? "icn_liked.png" : "icn_like.png";
								likeImg.Source = source;
								if( likeCountLabel != null )
									likeCountLabel.Text =  ( likeRes.likecount > 0 ) ? likeRes.likecount.ToString() : "";
							}
							progressBar.HideProgressbar();

						}
						catch (Exception ex)
						{
							progressBar.HideProgressbar();
							DisplayAlert(Constants.ALERT_TITLE, "Could not process the request now.", Constants.ALERT_OK);
						}

						progressBar.HideProgressbar();
					};


					Image shareButton = new Image();
					shareButton.Source = Device.OnPlatform("share.png", "share.png", "//Assets//share.png");
					shareButton.WidthRequest = Device.OnPlatform(15, 15, 15);
					shareButton.HeightRequest = Device.OnPlatform(15, 15, 15);
					shareButton.VerticalOptions = LayoutOptions.Center;
					shareButton.ClassId = item.gem_id;
					shareLabel = new Label
					{
						Text = "Share",
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						TextColor = Color.Gray,
						VerticalOptions = LayoutOptions.Center,
						FontSize = Device.OnPlatform(12, 12, 15),
						ClassId = item.gem_id
					};
					shareButtonTap = new TapGestureRecognizer();
					shareButtonTap.Tapped += OnShareButtonTapped;
					shareButton.GestureRecognizers.Add(shareButtonTap);
					shareLabel.GestureRecognizers.Add(shareButtonTap);
					toolsLayout.Children.Add(shareButton);
					toolsLayout.Children.Add(shareLabel);



					Image commentButton = new Image();
					commentButton.Source = Device.OnPlatform("icon_cmnt.png", "icon_cmnt.png", "//Assets//icon_cmnt.png");
					commentButton.WidthRequest = Device.OnPlatform(15, 15, 15);
					commentButton.HeightRequest = Device.OnPlatform(15, 15, 15);
					commentButton.VerticalOptions = LayoutOptions.Center;
					commentButton.ClassId = item.gem_id;
					Label commentsLabel = new Label
					{
						Text = "Comment",
						VerticalOptions = LayoutOptions.Center,
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						TextColor = Color.Gray,
						FontSize = Device.OnPlatform(12, 12, 15),
						ClassId = item.gem_id
					};

					commentButtonTap = new TapGestureRecognizer();
					commentButtonTap.Tapped += OnCommentButtonTapped;
					commentButton.GestureRecognizers.Add(commentButtonTap);
					commentsLabel.GestureRecognizers.Add(commentButtonTap);

					toolsLayout.Children.Add(commentButton);
					toolsLayout.Children.Add(commentsLabel);


					#endregion


					#region  title, description
					Image profileImage = new Image();
					profileImage.Source = (item.profileimg != null) ? Constants.SERVICE_BASE_URL + item.profileimg : "avatar.jpg";
					profileImage.WidthRequest = 60;
					profileImage.HeightRequest = 60;
					profileImage.Aspect = Aspect.Fill;

					Label userName = new Label();
					userName.Text = item.firstname;
					userName.TextColor = Color.Black;
					userName.WidthRequest = App.screenWidth * 90 / 100;
					userName.FontAttributes = FontAttributes.Bold;
					userName.FontSize = Device.OnPlatform(14, 15, 12);
					userName.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;

					title = new Label();
					title.Text = item.gem_datetime;
					title.TextColor = Color.Black;
					title.WidthRequest = App.screenWidth * 90 / 100;
					title.FontSize = Device.OnPlatform(12, 12, 12);
					title.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;

					description = new Label();
					description.WidthRequest = App.screenWidth * .80;
					description.Text = item.gem_details;
					description.TextColor = Color.Black;
					description.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
					description.FontSize = Device.OnPlatform(12, 15, 15);

					CustomImageButton menuButton = new CustomImageButton
					{
						ImageName = Device.OnPlatform("downarrow.png", "downarrow.png", "//Assets//downarrow.png"),
						Text = string.Empty,
						HorizontalOptions = LayoutOptions.End,
						BackgroundColor = Color.Transparent,
						WidthRequest = 30,
						HeightRequest = 20,
						ClassId = item.gem_id
					};
					menuButton.Clicked += GemMenuButton_Clicked;

					// masterStack.AddChildToLayout(pageTitle, 1, 1);
					//masterStack.AddChildToLayout(menuButton, 79, 1);
					masterStack.AddChildToLayout(profileImage, 2, 1);
					masterStack.AddChildToLayout(userName, 23, 3);
					masterStack.AddChildToLayout(title, 23, 7);

					TapGestureRecognizer moreTap = new TapGestureRecognizer();
					moreTap.Tapped += async (object senderr, EventArgs ee) =>
					{
						Image more = senderr as Image;
						if (more != null)
						{
							IProgressBar progress = DependencyService.Get<IProgressBar>();
							progress.ShowProgressbar("Loading more medias..");
							App.masterPage.IsPresented = false;
							CommunityGemsDetails gemInfo = communityGems.resultarray.FirstOrDefault(itm => itm.gem_id == more.ClassId);
							if (gemInfo != null)
							{
								List<PurposeColor.Constants.MediaDetails> mediaPlayerList = new List<PurposeColor.Constants.MediaDetails>();
								foreach (var mediaItem in  gemInfo.gem_media )
								{
									mediaPlayerList.Add(new PurposeColor.Constants.MediaDetails() { ImageName = mediaItem.gem_media, ID = item.gem_id, MediaType = mediaItem.media_type });
								}
								await Navigation.PushAsync(new CommunityMediaViewer(mediaPlayerList));
							}

							progress.HideProgressbar();
						}

					};
					Image moreImg = new Image();
					moreImg.Source = "more.png";
					moreImg.HorizontalOptions = LayoutOptions.End;
					moreImg.VerticalOptions = LayoutOptions.End;
					moreImg.GestureRecognizers.Add(moreTap);
					moreImg.ClassId = item.gem_id;

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

					if ( item.gem_media != null )
					{
						GemMedia gemMedia = (item.gem_media.Count > 0) ? item.gem_media[0] : null;
						if( gemMedia != null )
						{
							TapGestureRecognizer videoTap = new TapGestureRecognizer();
							videoTap.Tapped += OnGemTapped;

							string fileName = Path.GetFileName( Constants.SERVICE_BASE_URL + gemMedia.gem_media ); 

							Image img = new Image();
							bool isValidUrl = (gemMedia.gem_media != null && !string.IsNullOrEmpty(gemMedia.gem_media)) ? true : false;
							string source = (isValidUrl) ?  App.DownloadsPath + fileName : Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");
							string fileExtenstion = Path.GetExtension(source);
							bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;
							img.WidthRequest = App.screenWidth * 90 / 100;
							img.HeightRequest = App.screenWidth * 80 / 100;
							img.Aspect = Aspect.Fill;
							img.ClassId = null;
							if ( gemMedia.gem_media != null && gemMedia.media_type == "mp4")
							{
								img.ClassId = source;
								source = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
							}
							else if ( gemMedia.gem_media != null && gemMedia.media_type == "3gpp")
							{
								img.ClassId = source;
								source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
							}
							else if (gemMedia.gem_media != null && gemMedia.media_type == "wav")
							{
								img.ClassId = source;
								source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
							}


							img.Source = source;
							img.GestureRecognizers.Add(videoTap);
							var indicator = new ActivityIndicator { Color = new Color(.5), };
							indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
							masterStack.AddChildToLayout(indicator, 40, 30);

							CustomLayout imgContainer = new CustomLayout();
							imgContainer.WidthRequest = App.screenWidth * 90 / 100;
							imgContainer.HeightRequest = App.screenWidth * 90 / 100;
							img.ClassId = item.gem_id;
							imgContainer.Children.Add(img);
							if( item.gem_media != null && item.gem_media.Count > 1 )
								imgContainer.AddChildToLayout(moreImg, 75, 75, (int)imgContainer.WidthRequest, (int)imgContainer.HeightRequest);

							bottomAndLowerControllStack.Children.Add(imgContainer);
						}

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



				Button loadMoreGems = new Button();
				loadMoreGems.BackgroundColor = Color.Transparent;
				loadMoreGems.TextColor = Constants.BLUE_BG_COLOR;
				loadMoreGems.Text = "Load more gems";
				loadMoreGems.FontSize = 12;
				loadMoreGems.BorderWidth = 0;
				loadMoreGems.BorderColor = Color.Transparent;
				loadMoreGems.Clicked += OnLoadMoreGemsClicked;

				BoxView transBox = new BoxView();
				transBox.HeightRequest = 125;
				transBox.WidthRequest = App.screenWidth * 80 / 100;
				transBox.BackgroundColor = Color.Transparent;
				masterStackLayout.Children.Add(loadMoreGems);
				masterStackLayout.Children.Add(transBox);

				masterScroll.Content = masterStackLayout;
			}
			catch (Exception ex) 
			{

			}
		}



		void OnLoadPreviousGemsClicked (object sender, EventArgs e)
		{
			try
			{
				IProgressBar progress = DependencyService.Get< IProgressBar > ();
				progress.ShowProgressbar ( "loading...." );
				CommunityGemsDetails firstItem =   communityGems.resultarray.First ();

				CommunityGemsObject gemsObj =  App.Settings.GetCommunityGemsObject ();
				int firstRendererItemIndex = gemsObj.resultarray.FindIndex (itm => itm.gem_id == firstItem.gem_id);;//gemsObj.resultarray.IndexOf ( lastItem );
				if (firstRendererItemIndex > 0 && ( firstRendererItemIndex + 1 ) < gemsObj.resultarray.Count )
				{
					int itemCountToCopy = MAX_ROWS_AT_A_TIME;
					//	itemCountToCopy = (itemCountToCopy > MAX_ROWS_AT_A_TIME) ? MAX_ROWS_AT_A_TIME : itemCountToCopy;
					communityGems = null;
					communityGems = new CommunityGemsObject ();
					communityGems.resultarray = new List<CommunityGemsDetails> ();
					gemsObj.resultarray.RemoveRange( firstRendererItemIndex, gemsObj.resultarray.Count - firstRendererItemIndex );
					if( firstRendererItemIndex > MAX_ROWS_AT_A_TIME )
						gemsObj.resultarray.RemoveRange( 0, firstRendererItemIndex - MAX_ROWS_AT_A_TIME );

					communityGems.resultarray = gemsObj.resultarray;

					gemsObj = null;

					masterStackLayout.Children.Clear();

					masterScroll.Content = null;
					GC.Collect();

					RenderGems ( communityGems );
					masterScroll.ScrollToAsync (0, 0, false); 
				}

				progress.HideProgressbar ();
				progress = null;
			} 
			catch (Exception ex) 
			{
				DisplayAlert ( Constants.ALERT_TITLE, "Low memory error.", Constants.ALERT_OK );
			}
		}


        void OnMyGemsTapped(object sender, EventArgs e)
        {

        }


		async void OnLoadMoreGemsClicked (object sender, EventArgs e)
		{
			try 
			{

				IProgressBar progress = DependencyService.Get< IProgressBar > ();
				progress.ShowProgressbar ( "loading...." );
				CommunityGemsDetails lastItem =   communityGems.resultarray.Last ();

				CommunityGemsObject gemsObj =  App.Settings.GetCommunityGemsObject ();
				int lastRendererItemIndex = gemsObj.resultarray.FindIndex (itm => itm.gem_id == lastItem.gem_id);;//gemsObj.resultarray.IndexOf ( lastItem );
				if (lastRendererItemIndex > -1 && ( lastRendererItemIndex + 1 ) < gemsObj.resultarray.Count )
				{
					int itemCountToCopy = gemsObj.resultarray.Count - lastRendererItemIndex;
					itemCountToCopy = (itemCountToCopy > MAX_ROWS_AT_A_TIME) ? MAX_ROWS_AT_A_TIME : itemCountToCopy;
					communityGems = null;
					communityGems = new CommunityGemsObject ();
					communityGems.resultarray = new List<CommunityGemsDetails> ();
					communityGems.resultarray = gemsObj.resultarray.Skip (lastRendererItemIndex + 1).Take (itemCountToCopy).ToList();

					gemsObj = null;


					masterStackLayout.Children.Clear();

					masterScroll.Content = null;
					GC.Collect();

					RenderGems ( communityGems );
					masterScroll.ScrollToAsync (0, 0, false); 
				}

				progress.HideProgressbar ();
				progress = null;
			} 
			catch (Exception ex)
			{
				DisplayAlert ( Constants.ALERT_TITLE, "Low memory error.", Constants.ALERT_OK );
			}

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

        CarouselLayout CreatePagesCarousel(List<SelectedGoalMedia> media)
        {
            List<CarousalViewModel> Pages = new List<CarousalViewModel>();

            foreach (var item in media)
            {
                CarousalViewModel caros = new CarousalViewModel { Title = "1", Background = Color.White, ImageSource = Constants.SERVICE_BASE_URL + item.goal_media };
                Pages.Add(caros);
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
			CustomImageButton btn = sender as CustomImageButton;
			CustomLayout sellayout = null;
			if( btn != null )
			{
			     sellayout = (CustomLayout) masterStackLayout.Children.FirstOrDefault (itm => itm.ClassId == "masterstack" + btn.ClassId);
			}

			View menuView = sellayout.Children.FirstOrDefault(pick => pick.ClassId == Constants.CUSTOMLISTMENU_VIEW_CLASS_ID);
            if (menuView != null)
            {
				HideMenuPopUp ( sellayout,menuView );
                return;
            }

            List<CustomListViewItem> menuItems = new List<CustomListViewItem>();
			menuItems.Add(new CustomListViewItem { Name = "Remove", EmotionID = CurrentGemId.ToString(), EventID = btn.ClassId, SliderValue = 0 });

            PurposeColor.screens.CustomListMenu GemMenu = new screens.CustomListMenu(masterLayout, menuItems);
            //GemMenu.WidthRequest = App.screenWidth * .50;
            //GemMenu.HeightRequest = App.screenHeight * .40;
            GemMenu.ClassId = Constants.CUSTOMLISTMENU_VIEW_CLASS_ID;
            GemMenu.listView.ItemSelected += GemMenu_ItemSelected;

			if (sellayout != null)
			{
				sellayout.AddChildToLayout(GemMenu, 52, 4);
			}

			//masterStack.Children.Add (GemMenu, new Point (20, btn.Y));
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
				else if (item.Name == "Remove")
                {

                }

                HideCommentsPopup();

            }
            catch (Exception)
            {
            }
        }

		void HideMenuPopUp(  CustomLayout sellayout, View toRemove )
		{
			sellayout.Children.Remove ( toRemove );
		}

        void HideCommentsPopup()
        {
          /*  try
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
            }*/
        }

        async void OnCommentButtonTapped(object sender, EventArgs e)
        {

            //show comments popup
            try
            {
				string gemID = "";
				Image commentImg = sender as Image;
				Label commentLabel = sender as Label;
				if (commentImg != null)
				{

					if (commentImg.ClassId != null)
						gemID = commentImg.ClassId;
				}

				if (commentLabel != null)
				{

					if (commentLabel.ClassId != null)
						gemID = commentLabel.ClassId;
				}

				if( !string.IsNullOrEmpty( gemID ) )
				{
					progressBar.ShowProgressbar("Loading comments");
					List<Comment> comments = await PurposeColor.Service.ServiceHelper.GetComments(gemID, GemType.Goal, false);
					progressBar.HideProgressbar();


					PurposeColor.screens.CommentsView commentsView = new PurposeColor.screens.CommentsView(masterLayout, comments, gemID, CurrentGemType, false);
	                commentsView.ClassId = Constants.COMMENTS_VIEW_CLASS_ID;
	                commentsView.HeightRequest = App.screenHeight;
	                commentsView.WidthRequest = App.screenWidth;
	                masterLayout.AddChildToLayout(commentsView, 0, 0);
				}

            }
            catch (Exception ex)
            {
                progressBar.HideProgressbar();
                DisplayAlert(Constants.ALERT_TITLE, "Could not fetch comments, Please try again later.", Constants.ALERT_OK);
            }
            progressBar.HideProgressbar();
        }

        async void OnShareButtonTapped(object sender, EventArgs e)
        {

            string statusCode = "404";

            try
            {
                string gemID = "";
                Button button = sender as Button;
                Label label = sender as Label;
                if (button != null)
                {
                    if (button.ClassId != null)
                        gemID = button.ClassId;
                }

                if (label != null)
                {
                    if (label.ClassId != null)
                        gemID = label.ClassId;
                }

                CommunityGemsDetails gemInfo = communityGems.resultarray.FirstOrDefault(itm => itm.gem_id == gemID );

                if( gemInfo != null )
                {
					IProgressBar progress = DependencyService.Get< IProgressBar >();

					progress.ShowProgressbar( "Share view loading....." );

					try
					{

						IShareVia share = DependencyService.Get<IShareVia>();
						string mediaPath = (gemInfo.gem_media != null && gemInfo.gem_media.Count > 0) ?  Constants.SERVICE_BASE_URL +  gemInfo.gem_media[0].gem_media : null;
						string mediaType = (gemInfo.gem_media != null && gemInfo.gem_media.Count > 0) ?  gemInfo.gem_media[0].media_type : null;
						Constants.MediaType medaTypeToShare = Constants.MediaType.Image;
						if( mediaType == "mp4" )
						{
							medaTypeToShare = Constants.MediaType.Video;
						}
						else if( mediaType == "3gpp" || mediaType == "wav" )
						{
							medaTypeToShare = Constants.MediaType.Audio;
						}
						else if( mediaType == ".png" || mediaType == ".jpg" || mediaType == ".jpeg" )
						{
							medaTypeToShare = Constants.MediaType.Image;
						}

						share.ShareMedia(gemInfo.gem_details, mediaPath, medaTypeToShare);


					}
					catch (Exception ex)
					{

					}

                }

            }
            catch (Exception)
            {
                shareButtonTap.Tapped += OnShareButtonTapped;
            }


        }

	

        void OnGemTapped(object sender, EventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                string fileName = Path.GetFileName(img.ClassId);
                if (fileName != null)
                {
					string fileExtenstion = Path.GetExtension(img.ClassId);
					bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;

					if (!isImage) 
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
                    IVideoDownloader videoDownload = DependencyService.Get<IVideoDownloader>();
                    videoDownload.Download(img.ClassId, fileName);
                }

            }

        }



        public void Dispose()
        {
            masterLayout = null;
            progressBar = null;
            //masterStack = null;
            title = null;
            description = null;
            mediaList = null;
            shareLabel = null;
            if (shareButtonTap != null)
            {
                shareButtonTap.Tapped -= OnShareButtonTapped;
                shareButtonTap = null;
            }
            if (commentButtonTap != null)
            {
                commentButtonTap.Tapped -= OnCommentButtonTapped;
                commentButtonTap = null;
                likeButtonTap = null;
            }

            gemMenuContainer = null;
            modelObject = null;
            masterStackLayout = null;
            communityGems = null;
            GC.Collect();
        }
    }

}

