using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.IO;

namespace PurposeColor
{
	public partial class ImageTileViewer : ContentPage
	{
		public ImageTileViewer ( List<PurposeColor.Constants.MediaDetails> mediaList )
		{
			InitializeComponent ();

			this.BackgroundColor = Color.Black;
			previewImage.Aspect = Aspect.AspectFill;
			thumbArea.Orientation = ScrollOrientation.Horizontal;
			thumbArea.BackgroundColor = Color.Black;
			thumbArea.HorizontalOptions = LayoutOptions.Center;
			thumbArea.VerticalOptions = LayoutOptions.Center;


			StackLayout container = new StackLayout ();
			container.Orientation = StackOrientation.Horizontal;
			container.HorizontalOptions = LayoutOptions.Center;
			container.VerticalOptions = LayoutOptions.Center;

			if (mediaList != null && mediaList.Count > 0)
			{
				TapGestureRecognizer previewImgeTap = new TapGestureRecognizer ();
				previewImgeTap.Tapped += OnPreviewImgeTapped;
				string source = "";
				if (mediaList [0] != null && mediaList [0].MediaType == "mp4") {
					source = Device.OnPlatform ("video.png", "video.png", "//Assets//video.png");
				} else if (mediaList [0] != null && mediaList [0].MediaType == "aac") {
					source = Device.OnPlatform ("audio.png", "audio.png", "//Assets//audio.png");
				} else if (mediaList [0] != null && mediaList [0].MediaType == "3gpp") {
					source = Device.OnPlatform ("video.png", "video.png", "//Assets//video.png");
				} else if (mediaList [0] != null && mediaList [0].MediaType == "wav") {
					source = Device.OnPlatform ("audio.png", "audio.png", "//Assets//audio.png");
				} 
				else 
				{
					source = mediaList [0].Url;
				}
				previewImage.Source = source;
				previewImage.ClassId = source;
				previewImage.GestureRecognizers.Add ( previewImgeTap );
			}

			foreach (var item in mediaList) 
			{
				TapGestureRecognizer thumpTap = new TapGestureRecognizer ();
				thumpTap.Tapped += (object sender, EventArgs e) => 
				{
					Image prevImg = sender as Image;
					if (prevImg != null) 
					{
						string fileName = Path.GetFileName (prevImg.ClassId);
						if (fileName != null)
						{
							string fileExt = Path.GetExtension (fileName);
							bool isImg = (fileExt == ".png" || fileExt == ".jpg" || fileExt == ".jpeg") ? true : false;

							if (isImg) 
							{
								previewImage.ClassId = "";
								previewImage.ClassId = App.DownloadsPath + fileName;
								previewImage.Source = App.DownloadsPath + fileName;

							} 
							else
							{
								IVideoDownloader videoDownload = DependencyService.Get<IVideoDownloader> ();
								videoDownload.Download (prevImg.ClassId, fileName);
							}

						}

					}
				};

				StackLayout imgContainer = new StackLayout ();
				imgContainer.WidthRequest = 100;
				imgContainer.HeightRequest = 100;
				imgContainer.HorizontalOptions = LayoutOptions.Center;
				imgContainer.VerticalOptions = LayoutOptions.Center;

				bool isValidUrl = ( item.Url != null && !string.IsNullOrEmpty ( item.Url )) ? true : false;
				string source = (isValidUrl) ? item.Url : Device.OnPlatform ("noimage.png", "noimage.png", "//Assets//noimage.png");
				string fileExtenstion = Path.GetExtension (source);
				bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;



				Image img = new Image ();


				img.ClassId = source;
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
				img.Aspect = Aspect.Fill;
				img.WidthRequest = 100;
				img.HeightRequest = 100;
				img.HorizontalOptions = LayoutOptions.Center;
				img.VerticalOptions = LayoutOptions.Center;
				img.GestureRecognizers.Add ( thumpTap );

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
					play.WidthRequest = 25;
					play.HeightRequest = 25;
					play.HorizontalOptions = LayoutOptions.Center;
					play.VerticalOptions = LayoutOptions.Center;
					play.ClassId =  img.ClassId;
					play.GestureRecognizers.Add(thumpTap);

					grid.Children.Add( img, 0, 0 );
					Grid.SetColumnSpan(img, 3);
					Grid.SetRowSpan( img, 3 );
					grid.Children.Add(play, 1, 1);
					imgContainer.Children.Add ( grid );
				}
				else
				{
					imgContainer.Children.Add ( img );
				}


				container.Children.Add ( imgContainer );
			}

			thumbArea.Content = container;


		}

		void OnPreviewImgeTapped (object sender, EventArgs e)
		{
			Image img = sender as Image;
			if (img != null) 
			{
				string fileName = Path.GetFileName (img.ClassId);
				if (fileName != null)
				{
					string fileExtenstion = Path.GetExtension (fileName);
					bool isImage = (fileExtenstion == ".png" || fileExtenstion == ".jpg" || fileExtenstion == ".jpeg") ? true : false;

					if (isImage) 
					{
						IMediaVIew mediaView = DependencyService.Get<IMediaVIew> ();
						mediaView.ShowImage ( App.DownloadsPath + fileName );
					} 
					else
					{
						IVideoDownloader videoDownload = DependencyService.Get<IVideoDownloader> ();
						videoDownload.Download (img.ClassId, fileName);
					}


				}

			}
		}
	}
}

