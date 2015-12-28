using System;

using Xamarin.Forms;
using CustomControls;
using PurposeColor.interfaces;
using System.Collections;
using PurposeColor.Model;
using System.Collections.Generic;
using PurposeColor.CustomControls;

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
		GemsPageTitleBarWithBack mainTitleBar;
		public GemsDetailsPage (  List<EventMedia> mediaArray, string titleVal, string desc, string eventMedia, string eventNoMedia, string goalsMedia, string goalsNoMedia )
		{
			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
			masterScroll = new ScrollView ();
			masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
			progressBar = DependencyService.Get<IProgressBar>();
			masterStack = new StackLayout ();
			masterStack.Orientation = StackOrientation.Vertical;
			mediaList = mediaArray;

			// PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
			mainTitleBar = new GemsPageTitleBarWithBack(Color.FromRgb(8, 135, 224), "Add Supporting Emotions", Color.White, "", false);

			title = new Label ();
			title.Text = titleVal;
			title.TextColor = Color.Red;

			description = new Label ();
			description.Text = desc;
			description.TextColor = Color.Red;



			masterStack.Children.Add ( title );
			masterStack.Children.Add ( description );

			for (int index = 0; index < mediaList.Count; index++)
			{
				Image img = new Image ();
				bool isValidUrl = (mediaList [index].event_media != null && !string.IsNullOrEmpty (mediaList [index].event_media)) ? true : false;
				img.Source = (isValidUrl) ? Constants.SERVICE_BASE_URL + eventMedia + mediaList [index].event_media : Constants.SERVICE_BASE_URL + eventNoMedia;
				img.Aspect = Aspect.AspectFill;
				//img.HeightRequest = 200;
				//img.WidthRequest = 150;
				masterStack.Children.Add ( img );
			}

			masterScroll.HeightRequest = App.screenHeight - 10;
			masterScroll.WidthRequest = App.screenWidth;

			masterScroll.Content = masterStack;


			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(masterScroll, 0, 10);
			Content = masterLayout;

		/*	Content = new StackLayout
			{
				BackgroundColor = Color.Red,
				WidthRequest = 500,
				HeightRequest = 900
			};*/
		}

		public void Dispose ()
		{
			masterScroll = null;
			Content = null;
			masterLayout = null;
		}
	}
}


