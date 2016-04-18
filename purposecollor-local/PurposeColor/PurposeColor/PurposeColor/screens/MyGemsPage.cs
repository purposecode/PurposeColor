﻿
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
	public class MyGemsPage : ContentPage, IDisposable
	{
		CustomLayout masterLayout = null;
		IProgressBar progressBar;
		CustomLayout masterStack;
		ScrollView masterScroll;
		Label title;
		Label description;
		List<EventMedia> mediaList { get; set; }
		List<ActionMedia> actionMediaList { get; set; }
		GemType CurrentGemType = GemType.Goal;
		PurposeColorTitleBar mainTitleBar;
		CommunityGemSubTitleBar subTitleBar;
		TapGestureRecognizer shareButtonTap;
		TapGestureRecognizer commentButtonTap;
		TapGestureRecognizer likeButtonTap;
		Label shareLabel;
		StackLayout gemMenuContainer;
		StackLayout masterStackLayout;
		CommunityGemsObject communityGems;
		int MAX_ROWS_AT_A_TIME = 10;
		bool reachedEnd;
		bool reachedFront;

		//public GemsDetailsPage(List<EventMedia> mediaArray, List<ActionMedia> actionMediaArray, string pageTitleVal, string titleVal, string desc, string Media, string NoMedia, string gemId, GemType gemType)
		public MyGemsPage( CommunityGemsObject gemsObject )
		{
			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
			masterScroll = new ScrollView();
			masterScroll.IsClippedToBounds = true;
			masterScroll.BackgroundColor = Color.FromRgb(244, 244, 244);
			progressBar = DependencyService.Get<IProgressBar>();
			communityGems = gemsObject;
			App.Settings.DeleteCommunityGems ();
			App.Settings.SaveCommunityGemsDetails (gemsObject);

			mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += OnImageAreaTapGestureRecognizerTapped;
			subTitleBar = new CommunityGemSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "My Gems", false);

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

			BoxView emptyLayout = new BoxView();
			emptyLayout.BackgroundColor = Color.Transparent;
			emptyLayout.WidthRequest = App.screenWidth * 90 / 100;
			emptyLayout.HeightRequest = 30;

			masterStackLayout = new StackLayout();
			masterStackLayout.Orientation = StackOrientation.Vertical;


			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
			masterLayout.AddChildToLayout(masterScroll, -1, 18);

			if( communityGems.resultarray.Count > MAX_ROWS_AT_A_TIME )
			{
				communityGems.resultarray.RemoveRange( MAX_ROWS_AT_A_TIME, communityGems.resultarray.Count - MAX_ROWS_AT_A_TIME );
			}

			RenderGems( gemsObject );


			masterScroll.Scrolled += OnMasterScrollScrolled;
			Content = masterLayout;
		}

		void OnImageAreaTapGestureRecognizerTapped(object sender, EventArgs e)
		{
			App.masterPage.IsPresented = !App.masterPage.IsPresented;
		}

		async void OnMasterScrollScrolled (object sender, ScrolledEventArgs e)
		{
			if(  masterScroll.ScrollY > ( masterStackLayout.Height - Device.OnPlatform( 512, 650, 0 ) ) && !reachedEnd )
			{
				masterScroll.Scrolled -= OnMasterScrollScrolled;
				progressBar.ShowProgressbar( "loading gems..." );

				OnLoadMoreGemsClicked( masterScroll, EventArgs.Empty );

				//await Task.Delay( TimeSpan.FromSeconds( 1 ) );
				await masterScroll.ScrollToAsync( 0, 10, false );

				progressBar.HideProgressbar();


				await Task.Delay( TimeSpan.FromSeconds( 2 ) );
				masterScroll.Scrolled += OnMasterScrollScrolled;


			}
			else if( masterScroll.ScrollY < Device.OnPlatform( -50, 1, 0 ) && !reachedFront  )
			{
				masterScroll.Scrolled -= OnMasterScrollScrolled;
				progressBar.ShowProgressbar( "loading gems..." );


				OnLoadPreviousGemsClicked( masterScroll, EventArgs.Empty );

				//await Task.Delay( TimeSpan.FromSeconds( 1 ) );
				await masterScroll.ScrollToAsync( 0,  masterStackLayout.Height - 750, false );


				progressBar.HideProgressbar();

				await Task.Delay( TimeSpan.FromSeconds( 2 ) );
				masterScroll.Scrolled += OnMasterScrollScrolled;

			}
		}


		void OnLoadPreviousGemsClicked (object sender, EventArgs e)
		{
			try
			{
				CommunityGemsDetails firstItem =   communityGems.resultarray.First ();

				CommunityGemsObject gemsObj =  App.Settings.GetCommunityGemsObject ();


				int firstRendererItemIndex = gemsObj.resultarray.FindIndex (itm => itm.gem_id == firstItem.gem_id);;//gemsObj.resultarray.IndexOf ( lastItem );
				if (firstRendererItemIndex > 0 && ( firstRendererItemIndex + 1 ) < gemsObj.resultarray.Count )
				{
					reachedEnd = false;
					reachedFront = false;
					int itemCountToCopy = MAX_ROWS_AT_A_TIME;
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
				}
				else
				{
					reachedFront = true;
				}

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

				CommunityGemsDetails lastItem =   communityGems.resultarray.Last ();

				CommunityGemsObject gemsObj =  App.Settings.GetCommunityGemsObject ();
				int lastRendererItemIndex = gemsObj.resultarray.FindIndex (itm => itm.gem_id == lastItem.gem_id);;//gemsObj.resultarray.IndexOf ( lastItem );
				if (lastRendererItemIndex > 0 && ( lastRendererItemIndex + 1 ) < gemsObj.resultarray.Count )
				{
					reachedEnd = false;
					reachedFront = false;
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
				}
				else
				{
					reachedEnd = true;
				}

			} 
			catch (Exception ex)
			{
				DisplayAlert ( Constants.ALERT_TITLE, "Low memory error.", Constants.ALERT_OK );
			}

		}


		async  void OnAppearing(object sender, EventArgs e)
		{
			try
			{








			} 
			catch (Exception ex) 
			{
				string err = ex.Message;
			}
		}


		void RenderGems( CommunityGemsObject gemsObject )
		{
			try
			{
				foreach (var item in gemsObject.resultarray )
				{
					masterStack = new CustomLayout();
					masterStack.WidthRequest = App.screenWidth * 102 / 100;
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
					likeButton.ClassId = item.gem_id + "&&"  + item.gem_type;
					likeButton.GestureRecognizers.Add(likeButtonTap);

					Label likeLabel = new Label
					{
						Text = "Like",
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						TextColor = Color.Gray,
						VerticalOptions = LayoutOptions.Center,
						FontSize = Device.OnPlatform(12, 12, 15),
						ClassId = item.gem_id + "&&"  + item.gem_type
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

							string[] delimiters = { "&&" };
							string[] clasIDArray = gemID.Split(delimiters, StringSplitOptions.None);
							string selectedGemID = clasIDArray [0];
							string selectedGemType = clasIDArray [1];

							if( !string.IsNullOrEmpty( gemID ) )
							{
								LikeResponse likeRes = await ServiceHelper.LikeGem( selectedGemID, selectedGemType );
								if( likeRes != null )
								{
									string source = ( likeRes.like_status == 1 ) ? "icn_liked.png" : "icn_like.png";
									likeImg.Source = source;
									if( likeCountLabel != null )
										likeCountLabel.Text =  ( likeRes.likecount > 0 ) ? likeRes.likecount.ToString() : "";
								}
							}
							else
							{
								progressBar.ShowToast( "Invalid Gem ID." );
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
					commentButton.ClassId = item.gem_id + "&&"  + item.gem_type;
					Label commentsLabel = new Label
					{
						Text = "Comment",
						VerticalOptions = LayoutOptions.Center,
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						TextColor = Color.Gray,
						FontSize = Device.OnPlatform(12, 12, 15),
						ClassId = item.gem_id + "&&"  + item.gem_type
					};
					if( item.comment_count > 0 )
					{
						commentsLabel.Text = "Comments (" + item.comment_count.ToString() + ")";
					}
					else
					{
						commentsLabel.Text = "Comments";
					}


					commentButtonTap = new TapGestureRecognizer();
					commentButtonTap.Tapped += CommentButtonTapped;
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

					masterStack.AddChildToLayout(menuButton, 89, 1);
					masterStack.AddChildToLayout(profileImage, 4, 1);
					masterStack.AddChildToLayout(userName, 25, 3);
					masterStack.AddChildToLayout(title, 25, 7);

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

								List<string> listToDownload = new List<string>();

								foreach (var mediaItem in  gemInfo.gem_media )
								{
									if(string.IsNullOrEmpty(mediaItem.gem_media))
									{
										continue;
									}

									if (mediaItem.media_type == "png" || mediaItem.media_type == "jpg" || mediaItem.media_type == "jpeg") 
									{

										listToDownload.Add(Constants.SERVICE_BASE_URL+ mediaItem.gem_media);
										string fileName = System.IO.Path.GetFileName(mediaItem.gem_media);
										mediaItem.gem_media = App.DownloadsPath + fileName;
										mediaPlayerList.Add(new PurposeColor.Constants.MediaDetails() { ImageName = mediaItem.gem_media, ID = item.gem_id, MediaType = mediaItem.media_type, Url = mediaItem.gem_media });
									}
									else if( mediaItem.media_type == "mp4" || mediaItem.media_type == "3gpp" )
									{
										mediaItem.gem_media = Constants.SERVICE_BASE_URL + mediaItem.gem_media ;
										mediaPlayerList.Add(new PurposeColor.Constants.MediaDetails() { ImageName = mediaItem.video_thumb, ID = item.gem_id, MediaType = mediaItem.media_type, Url = mediaItem.gem_media });
									}
									else
									{
										mediaItem.gem_media = Constants.SERVICE_BASE_URL + mediaItem.gem_media ;
										mediaPlayerList.Add(new PurposeColor.Constants.MediaDetails() { ImageName = mediaItem.gem_media, ID = item.gem_id, MediaType = mediaItem.media_type, Url = mediaItem.gem_media });
									}

									//mediaPlayerList.Add(new PurposeColor.Constants.MediaDetails() { ImageName = mediaItem.gem_media, ID = item.gem_id, MediaType = mediaItem.media_type });
								}

								if (listToDownload != null && listToDownload.Count > 0) {
									IDownload downloader = DependencyService.Get<IDownload>();
									await downloader.DownloadFiles(listToDownload);
								}

								if( Device.OS == TargetPlatform.Android )
								{
									await Navigation.PushModalAsync( new ImageTileViewer( mediaPlayerList ) );
								}
								else
								{
									await Navigation.PushAsync( new ImageTileViewer( mediaPlayerList ) );
								}
								//await Navigation.PushAsync(new CommunityMediaViewer(mediaPlayerList));
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
					moreImg.TranslationX = Device.OnPlatform( 0, -20, 0 );

					#endregion

					StackLayout bottomAndLowerControllStack = new StackLayout
					{
						Orientation = StackOrientation.Vertical,
						BackgroundColor = Color.Transparent,
						Spacing = 1,
						Padding = new Thickness(0, 5, 0, 5),
						WidthRequest = App.screenWidth * 102 / 100
					};

					BoxView paddingBoxView = new BoxView();
					paddingBoxView.WidthRequest = App.screenWidth * 3 / 100;
					paddingBoxView.BackgroundColor = Color.Transparent;

					bottomAndLowerControllStack.Children.Add(new StackLayout { Orientation = StackOrientation.Horizontal , WidthRequest = App.screenWidth * 102 / 100, Children = { paddingBoxView, description } });

					#region MEDIA LIST

					if ( item.gem_media != null )
					{
						GemMedia gemMedia = (item.gem_media.Count > 0) ? item.gem_media[0] : null;
						if( gemMedia != null )
						{
							TapGestureRecognizer videoTap = new TapGestureRecognizer();
							videoTap.Tapped += OnActionVideoTapped;

							string fileName = Path.GetFileName( Constants.SERVICE_BASE_URL + gemMedia.gem_media ); 

							Image img = new Image();
							bool isValidUrl = (gemMedia.gem_media != null && !string.IsNullOrEmpty(gemMedia.gem_media)) ? true : false;
							string source = (isValidUrl) ?  App.DownloadsPath + fileName : null;
							string fileExtenstion = Path.GetExtension(source);

							bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;
							img.WidthRequest = App.screenWidth * 102 / 100;
							img.HeightRequest = App.screenWidth * 80 / 100;
							img.Aspect = Aspect.Fill;
							img.ClassId = null;
							if ( gemMedia.gem_media != null && gemMedia.media_type == "mp4")
							{
								img.ClassId = source;
								source = Constants.SERVICE_BASE_URL + gemMedia.video_thumb;
							}
							else if ( gemMedia.gem_media != null && (gemMedia.media_type == "3gpp" || gemMedia.media_type == "aac"))
							{
								img.ClassId = source;
								source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
							}
							else if (gemMedia.gem_media != null && gemMedia.media_type == "wav")
							{
								img.ClassId = source;
								source = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
							}

							if( source != null )
							{
								img.Source = source;
								img.GestureRecognizers.Add(videoTap);
								var indicator = new ActivityIndicator { Color = new Color(.5), };
								indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
								masterStack.AddChildToLayout(indicator, 40, 30);

								/*	CustomLayout imgContainer = new CustomLayout();
								imgContainer.WidthRequest = App.screenWidth * 90 / 100;
								imgContainer.HeightRequest = App.screenWidth * 90 / 100;
								imgContainer.Children.Add(img);
								if( item.gem_media != null && item.gem_media.Count > 1 )
									imgContainer.AddChildToLayout(moreImg, 75, 75, (int)imgContainer.WidthRequest, (int)imgContainer.HeightRequest);*/

								Grid grid = new Grid
								{
									WidthRequest = App.screenWidth * 102 / 100,
									VerticalOptions = LayoutOptions.FillAndExpand,
									HorizontalOptions = LayoutOptions.FillAndExpand,
									RowDefinitions = 
									{
										new RowDefinition { Height = new GridLength( img.HeightRequest / 3 ) },
										new RowDefinition { Height = new GridLength( img.HeightRequest / 3 ) },
										new RowDefinition { Height = new GridLength( img.HeightRequest / 3 ) },

									},
									ColumnDefinitions = 
									{
										new ColumnDefinition { Width = new GridLength( img.WidthRequest / 3 ) },
										new ColumnDefinition { Width = new GridLength( img.WidthRequest / 3 ) },
										new ColumnDefinition { Width = new GridLength( img.WidthRequest / 3 ) },

									}
									};

								if ( gemMedia != null && gemMedia.media_type == "mp4")
								{

									Image play = new Image();
									play.Source = "video_play.png";
									play.Aspect = Aspect.AspectFit;
									play.WidthRequest = 75;
									play.HeightRequest = 75;
									play.HorizontalOptions = LayoutOptions.Center;
									play.VerticalOptions = LayoutOptions.Center;
									play.ClassId =  img.ClassId;
									play.GestureRecognizers.Add(videoTap);

									BoxView box = new BoxView();
									box.BackgroundColor = Color.Red;
									box.WidthRequest = 100;
									box.HeightRequest = 100;

									grid.Children.Add( img, 0, 0 );
									Grid.SetColumnSpan(img, 3);
									Grid.SetRowSpan( img, 3 );
									grid.Children.Add(play, 1, 1);
									if( item.gem_media.Count > 1 )
										grid.Children.Add(moreImg, 2, 2);
									bottomAndLowerControllStack.Children.Add(grid);
								}
								else
								{	
									grid.Children.Add( img, 0, 0 );
									Grid.SetColumnSpan(img, 3);
									Grid.SetRowSpan( img, 3 );
									if( item.gem_media.Count > 1 )
										grid.Children.Add(moreImg, 2, 2);
									bottomAndLowerControllStack.Children.Add(grid);
								}
							}

						}

					}

					#endregion


					bottomAndLowerControllStack.Children.Add(toolsLayout);
					masterStack.AddChildToLayout(bottomAndLowerControllStack, 1, 12);
					//  masterStack.AddChildToLayout( moreImg, 65, Device.OnPlatform( 30, 20, 20 ) );

					//masterStack.AddChildToLayout(spaceOffsetlayout, 1, 85);
					/// bottomAndLowerControllStack.Children.Add(spaceOffsetlayout);

					masterScroll.HeightRequest = App.screenHeight - 20;
					masterScroll.WidthRequest = App.screenWidth * 102 / 100;// * 90 / 100;



					masterStackLayout.Children.Add(masterStack);
				}

				masterScroll.Content = masterStackLayout;
			}
			catch (Exception ex) 
			{

			}
		}




		protected override bool OnBackButtonPressed()
		{
			Dispose ();
			return base.OnBackButtonPressed ();
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


			CommunityGemsDetails seletedGem =  communityGems.resultarray.Find (item => item.gem_id == btn.ClassId);

			List<CustomListViewItem> menuItems = new List<CustomListViewItem>();
			menuItems.Add(new CustomListViewItem { Name = "Remove", EmotionID = "", EventID = btn.ClassId, SliderValue = 0 , gemType = GetGemType( seletedGem.gem_type )});

			PurposeColor.screens.CustomListMenu GemMenu = new screens.CustomListMenu(masterLayout, menuItems);
			//GemMenu.WidthRequest = App.screenWidth * .50;
			//GemMenu.HeightRequest = App.screenHeight * .40;
			GemMenu.ClassId = Constants.CUSTOMLISTMENU_VIEW_CLASS_ID;
			GemMenu.listView.ItemSelected += GemMenu_ItemSelected;

			if (sellayout != null)
			{
				sellayout.AddChildToLayout(GemMenu, 65, 4);
			}

			//masterStack.Children.Add (GemMenu, new Point (20, btn.Y));
		}


		private GemType GetGemType( string gemType )
		{
			if (gemType == "goal")
				return GemType.Goal;
			else if (gemType == "action")
				return GemType.Action;
			else if (gemType == "event")
				return GemType.Event;
			else
				return GemType.Goal;
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
					IProgressBar progress = DependencyService.Get<IProgressBar>();
					progress.ShowProgressbar( "Removing Gem..." );
					await ServiceHelper.RemoveGemFromCommunity( item.EventID, item.gemType );

					masterScroll.Scrolled -= OnMasterScrollScrolled;

					communityGems = null;
					masterStack.Children.Clear();
					masterStackLayout.Children.Clear();
					masterScroll.Content = null;
					GC.Collect();

				/*	communityGems =   await ServiceHelper.GetMyGemsDetails();

					App.Settings.DeleteCommunityGems ();
					App.Settings.SaveCommunityGemsDetails (communityGems);

					RenderGems( communityGems );
					progress.HideProgressbar();*/

					masterScroll.Scrolled += OnMasterScrollScrolled;
					App.masterPage.IsPresented = false;
					DetailsPageModel model = new DetailsPageModel();
					App.masterPage.Detail = new NavigationPage(new CommunityGems( model ));
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
				string gemID = "";
				Image shareImg = sender as Image;
				Label commentLabel = sender as Label;
				if (shareImg != null)
				{
					if (shareImg.ClassId != null)
						gemID = shareImg.ClassId;
				}

				if (commentLabel != null)
				{
					if (commentLabel.ClassId != null)
						gemID = commentLabel.ClassId;
				}

				if( !string.IsNullOrEmpty( gemID ) )
				{
					string[] delimiters = { "&&" };
					string[] clasIDArray = gemID.Split(delimiters, StringSplitOptions.None);
					string selectedGemID = clasIDArray [0];
					string selectedGemType = clasIDArray [1];
					GemType currentGemType = GetGemType( selectedGemType );

					progressBar.ShowProgressbar("Loading comments");
					List<Comment> comments = await PurposeColor.Service.ServiceHelper.GetComments(selectedGemID, currentGemType, true);
					progressBar.HideProgressbar();

					PurposeColor.screens.CommentsView commentsView = new PurposeColor.screens.CommentsView(masterLayout, comments, selectedGemID, currentGemType, true, commentLabel);
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
					IShareVia share = DependencyService.Get<IShareVia>();
					string mediaPath = (gemInfo.gem_media != null && gemInfo.gem_media.Count > 0) ?  Constants.SERVICE_BASE_URL +  gemInfo.gem_media[0].gem_media : null;
					share.ShareMedia(gemInfo.gem_details, mediaPath, Constants.MediaType.Image);
				}

			}
			catch (Exception)
			{
				shareButtonTap.Tapped += OnShareButtonTapped;
			}


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
				shareButtonTap.Tapped -= OnShareButtonTapped;
				shareButtonTap = null;
			}
			if (commentButtonTap != null)
			{
				commentButtonTap.Tapped -= CommentButtonTapped;
				commentButtonTap = null;
				likeButtonTap = null;
			}

			gemMenuContainer = null;
			masterStackLayout = null;
			communityGems = null;
			GC.Collect();
		}
	}

}






