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

namespace PurposeColor
{
	public class CommunityMediaViewer : ContentPage, IDisposable
	{
		CustomLayout masterLayout = null;
		IProgressBar progressBar = null;
		CustomLayout masterStack = null;
		ScrollView masterScroll = null;
		PurposeColorTitleBar mainTitleBar = null;
		PurposeColorSubTitleBar subTitleBar = null;

		public CommunityMediaViewer ( List<PurposeColor.Constants.MediaDetails> mediaList )
		{
			NavigationPage.SetHasNavigationBar (this, false);
			masterLayout = new CustomLayout ();
			masterLayout.BackgroundColor = Color.Black;// Color.FromRgb(244, 244, 244);
			masterScroll = new ScrollView ();
			masterScroll.BackgroundColor = Color.Black;//Color.FromRgb(244, 244, 244);
			progressBar = DependencyService.Get<IProgressBar> ();

			mainTitleBar = new PurposeColorTitleBar (Color.FromRgb (8, 135, 224), "Purpose Color", Color.Black, "back", true);
			subTitleBar = new PurposeColorSubTitleBar (Constants.SUB_TITLE_BG_COLOR, "Gem Media Viewer", false);
			subTitleBar.BackButtonTapRecognizer.Tapped += async (object sender, EventArgs e) => {
				try {
					await Navigation.PopAsync ();
				} catch (Exception) {
				}
			};

			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += (object sender, EventArgs e) => 
			{
				App.masterPage.IsPresented = !App.masterPage.IsPresented;
			};
				

			masterStack = new CustomLayout ();
			masterStack.BackgroundColor = Color.Transparent;
			masterStack.HorizontalOptions = LayoutOptions.Center;

			StackLayout bottomAndLowerControllStack = new StackLayout {
				Orientation = StackOrientation.Vertical,
				BackgroundColor = Color.Transparent,
				Spacing = 1,
				Padding = new Thickness (0, 5, 0, 5),
				WidthRequest = App.screenWidth,
			};


			ScrollView imgScrollView = new ScrollView ();
			imgScrollView.Orientation = ScrollOrientation.Horizontal;

			StackLayout horizmgConatiner = new StackLayout ();
			horizmgConatiner.Orientation = StackOrientation.Horizontal;


			CustomImageButton nextImg = new CustomImageButton ();
			nextImg.ImageName = "next.png";
			nextImg.WidthRequest = 40;
			nextImg.HeightRequest = 65;
				
			nextImg.Clicked += async (object sender, EventArgs e) => 
			{

				double curX = imgScrollView.ScrollX;
				double imgWidth = App.screenWidth * 100 / 100;

				if( Device.OS == TargetPlatform.iOS )
				{
					if( curX + imgWidth + 15 < imgScrollView.ContentSize.Width )
						await imgScrollView.ScrollToAsync( curX + imgWidth , 0, true );
				}
				else
				{
					await imgScrollView.ScrollToAsync( curX + imgWidth , 0, true );
				}
					
			};


			CustomImageButton prevImg = new CustomImageButton ();
			prevImg.ImageName = "prev.png";
			prevImg.WidthRequest = 40;
			prevImg.HeightRequest = 65;
			prevImg.Clicked += async (object sender, EventArgs e) => 
			{
				double curX = imgScrollView.ScrollX;
				double imgWidth = App.screenWidth * 90 / 100;
				if( curX > 0 )
				await imgScrollView.ScrollToAsync( curX - App.screenWidth , 0, true );
			};


			#region MEDIA LIST

			if (mediaList != null) 
			{
				foreach (var item in mediaList) 
				{
					TapGestureRecognizer videoTap = new TapGestureRecognizer ();
					videoTap.Tapped += OnActionVideoTapped;

					Image img = new Image ();
					bool isValidUrl = ( item.Url != null && !string.IsNullOrEmpty ( item.Url )) ? true : false;
					string source = (isValidUrl) ? item.Url : Device.OnPlatform ("noimage.png", "noimage.png", "//Assets//noimage.png");
					string fileExtenstion = Path.GetExtension (source);
					bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;
					img.WidthRequest = App.screenWidth;
					img.HeightRequest = App.screenWidth;
					img.Aspect = Aspect.AspectFill;
					img.ClassId = null;
					if (item != null && item.MediaType == "mp4") 
					{
						img.ClassId = source;
						source = Constants.SERVICE_BASE_URL + item.ImageName;
					}
					else if ( item != null &&  item.MediaType == "aac")
					{
						img.ClassId = source;
						source = Device.OnPlatform ("audio.png", "audio.png", "//Assets//audio.png");
					}
					else if ( item != null &&  item.MediaType == "3gpp")
					{
						img.ClassId = source;
						source = Constants.SERVICE_BASE_URL + item.ImageName;
					}
					else if ( item != null && item.MediaType == "wav")
					{
						img.ClassId = source;
						source = Device.OnPlatform ("audio.png", "audio.png", "//Assets//audio.png");
					}
					img.Source = source;
					img.GestureRecognizers.Add (videoTap);
					var indicator = new ActivityIndicator { Color = new Color (.5), };
					indicator.SetBinding (ActivityIndicator.IsRunningProperty, "IsLoading");
					indicator.BindingContext = img;
					masterStack.AddChildToLayout (indicator, 40, 30);

					if (item != null && ( item.MediaType == "mp4" || item.MediaType == "3gpp" ) )
					{
						Grid grid = new Grid
						{
							VerticalOptions = LayoutOptions.FillAndExpand,
							HorizontalOptions = LayoutOptions.FillAndExpand,
							RowDefinitions = 
							{
								new RowDefinition { Height = new GridLength( img.WidthRequest / 3 ) },
								new RowDefinition { Height = new GridLength( img.WidthRequest / 3 ) },
								new RowDefinition { Height = new GridLength( img.WidthRequest / 3 ) },

							},
							ColumnDefinitions = 
							{
								new ColumnDefinition { Width = new GridLength( img.WidthRequest / 3 ) },
								new ColumnDefinition { Width = new GridLength( img.WidthRequest / 3 ) },
								new ColumnDefinition { Width = new GridLength( img.WidthRequest / 3 ) },

							}
						};

						Image play = new Image();
						play.Source = "video_play.png";
						play.Aspect = Aspect.AspectFit;
						play.WidthRequest = 75;
						play.HeightRequest = 75;
						play.HorizontalOptions = LayoutOptions.Center;
						play.VerticalOptions = LayoutOptions.Center;
						play.ClassId =  img.ClassId;
						play.GestureRecognizers.Add(videoTap);

						grid.Children.Add( img, 0, 0 );
						Grid.SetColumnSpan(img, 3);
						Grid.SetRowSpan( img, 3 );
						grid.Children.Add(play, 1, 1);
						horizmgConatiner.Children.Add(grid);
					}
					else
					{
						horizmgConatiner.Children.Add(img);
					}
				}
				imgScrollView.Content = horizmgConatiner;
				bottomAndLowerControllStack.Children.Add (imgScrollView);

			}

			#endregion

			masterStack.AddChildToLayout (bottomAndLowerControllStack, 0, Device.OnPlatform (5, 9, 12));//12
			masterScroll.HeightRequest = App.screenHeight - 20;
			masterScroll.WidthRequest = App.screenWidth;// * 90 / 100;

			StackLayout masterStackLayout = new StackLayout ();
			masterStackLayout.HorizontalOptions = LayoutOptions.Center;
			masterStackLayout.BackgroundColor = Color.Black;
			masterStackLayout.Orientation = StackOrientation.Vertical;
			masterStackLayout.Children.Add (masterStack);
			masterScroll.Content = masterStackLayout;
			masterLayout.AddChildToLayout (mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout (subTitleBar, 0, Device.OnPlatform (9, 10, 10));
			masterLayout.AddChildToLayout (masterScroll, 0, 18);
			masterLayout.AddChildToLayout (prevImg, Device.OnPlatform( -2, -2, 0 ), 50);
			masterLayout.AddChildToLayout (nextImg, 90, 50);
			Content = masterLayout;
		}


		void OnActionVideoTapped (object sender, EventArgs e)
		{
			Image img = sender as Image;
			if (img != null) {
				string fileName = Path.GetFileName (img.ClassId);
				if (fileName != null)
				{
					IVideoDownloader videoDownload = DependencyService.Get<IVideoDownloader> ();
					videoDownload.Download (img.ClassId, fileName);

				}

			}
		}

		void OnEventVideoTapped (object sender, EventArgs e)
		{
			Image img = sender as Image;
			if (img != null) {
				string fileName = Path.GetFileName (img.ClassId);
				if (fileName != null) {
					IVideoDownloader videoDownload = DependencyService.Get<IVideoDownloader> ();
					videoDownload.Download (img.ClassId, fileName);
				}

			}

		}

		protected override bool OnBackButtonPressed ()
		{
            Dispose();
			return base.OnBackButtonPressed ();
		}

		public void Dispose ()
		{
			masterScroll = null;
			Content = null;
			masterLayout = null;
			progressBar = null;
			masterStack = null;
			this.Content = null;
			GC.Collect ();
		}
	}

}

