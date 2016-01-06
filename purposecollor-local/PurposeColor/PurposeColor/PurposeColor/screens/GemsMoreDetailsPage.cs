using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using PurposeColor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;


namespace PurposeColor.screens
{
    public class GemsMoreDetailsPage : ContentPage, IDisposable
    {
        CustomLayout masterLayout;
        GemsPageTitleBarWithBack mainTitleBar;
        ScrollView masterScroll;
        StackLayout masterStack;
        GemsEmotionsDetails emotionsMasterList;
		GemsGoalsDetails goalsMasterList;
        public string eventsMediaPath;
        public string eventsMediaThumbPath;
		public string goalsMediaPath;
		public string goalsMediaThumbPath;
		public string goalsNoMediaPath;
		public string eventsNoMediaPath;
		static bool isFirstTime;
		public GemsMoreDetailsPage(GemsEmotionsDetails emotionsList, GemsGoalsDetails goalsList, string eventMedia, string eventMediaThumb, string eventNoMedia, string goalsMedia, string goalsMediaThumb, string goalsNoMedia)
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
			IProgressBar progressBar = DependencyService.Get<IProgressBar>();
            eventsMediaPath = eventMedia;
            eventsMediaThumbPath = eventMediaThumb;
			goalsMediaPath = goalsMedia;
			goalsMediaThumbPath = goalsMediaThumb;
			goalsNoMediaPath = goalsNoMedia;
			eventsNoMediaPath = eventNoMedia;
            emotionsMasterList = emotionsList;
			goalsMasterList = goalsList;
			isFirstTime = true;


            // PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar = new GemsPageTitleBarWithBack(Color.FromRgb(8, 135, 224), "Add Supporting Emotions", Color.White, "", false);
			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += (object sender, EventArgs e) => 
			{
                try
                {
                    Navigation.PopAsync();
                }
                catch (Exception)
                {
                    
                }
			};

            if (emotionsMasterList != null)
            {
                mainTitleBar.title.Text = emotionsMasterList.emotion_title;
            }
            else
            {
                mainTitleBar.title.Text = goalsMasterList.goal_title;
            }
			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += (object sender, EventArgs e) => 
			{
                try
                {
                    App.Navigator.PopModalAsync();
                }
                catch (Exception )
                {
                    
                }
			};


            masterScroll = new ScrollView();
            masterScroll.WidthRequest = App.screenWidth;
            masterScroll.HeightRequest = App.screenHeight * 85 / 100;

            masterStack = new StackLayout();
            masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.Transparent;


			this.Appearing += OnAppearing;


            masterScroll.Content = masterStack;
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(masterScroll, 0, 10);
            Content = masterLayout;
        }


        async void OnAppearing(object sender, EventArgs e)
        {

			if (!isFirstTime)
				return;

            // this.Animate("", (s) => Layout(new Rectangle(X, (1 - s) * Height, Width, Height)), 0, 1000, Easing.SpringIn, null, null); // slide up
        

			if (emotionsMasterList != null)
			{
				for (int index = 0; index < emotionsMasterList.event_details.Count; index++)
				{
					TapGestureRecognizer tap = new TapGestureRecognizer ();
					tap.Tapped += OnEmotionTapped;
					StackLayout cellMasterLayout = new StackLayout ();
					cellMasterLayout.Orientation = StackOrientation.Vertical;
					cellMasterLayout.BackgroundColor = Color.White;

					StackLayout headerLayout = new StackLayout ();
					headerLayout.Orientation = StackOrientation.Vertical;
					headerLayout.BackgroundColor = Color.FromRgb (244, 244, 244);

					CustomLayout customLayout = new CustomLayout ();
					customLayout.BackgroundColor = Color.FromRgb (244, 244, 244);
					double screenWidth = App.screenWidth;
					double screenHeight = App.screenHeight;
					customLayout.ClassId = emotionsMasterList.event_details [index].event_id;
					customLayout.GestureRecognizers.Add (  tap);

					CustomImageButton mainTitle = new CustomImageButton ();
					//  mainTitle.IsEnabled = false;
					mainTitle.BackgroundColor = Color.FromRgb (30, 126, 210);
					mainTitle.ImageName = Device.OnPlatform ("blue_bg.png", "blue_bg.png", @"/Assets/blue_bg.png");
					mainTitle.Text = "My Supporting Emotions";
					mainTitle.TextColor = Color.White;
					mainTitle.FontSize = Device.OnPlatform (12, 12, 18);
					mainTitle.WidthRequest = App.screenWidth;
					mainTitle.TextOrientation = TextOrientation.Middle;
					headerLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
					mainTitle.HeightRequest = 80;


					Label subTitle = new Label ();
					subTitle.Text = (emotionsMasterList.event_title != null && emotionsMasterList.event_title.Count > 0) ? emotionsMasterList.event_title [index].event_title : "empty";
					subTitle.TextColor = Color.Gray;
					subTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
					subTitle.XAlign = TextAlignment.Center;
					int subTitleFontSize = (App.screenDensity > 1.5) ? 18 : 16;
					subTitle.VerticalOptions = LayoutOptions.Center;
					subTitle.FontSize = Device.OnPlatform (subTitleFontSize, subTitleFontSize, 22);
					subTitle.WidthRequest = App.screenWidth * 90 / 100;
					headerLayout.HorizontalOptions = LayoutOptions.Center;
					subTitle.HeightRequest = Device.OnPlatform (40, 40, 30);
					subTitle.ClassId = emotionsMasterList.event_details [index].event_id;
					subTitle.GestureRecognizers.Add (  tap);


					Label firstDetailsInfo = new Label ();
					string trimmedFirstDetails = (emotionsMasterList.event_details != null && emotionsMasterList.event_details.Count > 0) ? emotionsMasterList.event_details [index].event_details : "empty";
					if (trimmedFirstDetails != null && trimmedFirstDetails.Length > 50) {
						trimmedFirstDetails = trimmedFirstDetails.Substring (0, 50);
						trimmedFirstDetails = trimmedFirstDetails + "....";
						trimmedFirstDetails = trimmedFirstDetails.Replace ("\\n", string.Empty);
						trimmedFirstDetails = trimmedFirstDetails.Replace ("\\r", string.Empty);
					}

  
					firstDetailsInfo.Text = trimmedFirstDetails;
					firstDetailsInfo.TextColor = Color.Gray;
					firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
					firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
					firstDetailsInfo.HeightRequest = 40;
					firstDetailsInfo.ClassId = emotionsMasterList.event_details [index].event_id;
					firstDetailsInfo.GestureRecognizers.Add (  tap);
					int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform (17, 16, 13) : 15;
					firstDetailsInfo.FontSize = Device.OnPlatform (firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


					Label firstDateInfo = new Label ();
					firstDateInfo.Text = (emotionsMasterList.event_datetime != null && emotionsMasterList.event_datetime.Count > 0) ? emotionsMasterList.event_datetime [index].event_datetime : "empty";
					//firstDateInfo.Text = "2015 Januvary 30";
					firstDateInfo.TextColor = Color.Black;
					firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
					int dateFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform (13, 15, 13) : 12;
					firstDateInfo.FontSize = Device.OnPlatform (dateFontSize, dateFontSize, dateFontSize);
					firstDateInfo.ClassId = emotionsMasterList.event_details [index].event_id;
					firstDateInfo.GestureRecognizers.Add (  tap);


					Image firstEmotionsImage = new Image ();
					firstEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform (30, 28, 25) / 100;
					firstEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform (25, 22, 18) / 100;

                    string eventID = emotionsMasterList.event_details[index].event_id;
                    List<EventMedia> firstThumbMedia = emotionsMasterList.event_media.FindAll(itm => itm.event_id == eventID).ToList();
                    bool firstImageValidity = (firstThumbMedia != null && firstThumbMedia.Count > 0 && !string.IsNullOrEmpty(firstThumbMedia[0].event_media)) ? true : false;
                    string firstImageSource = (firstImageValidity) ? Constants.SERVICE_BASE_URL + eventsMediaThumbPath + firstThumbMedia[0].event_media : Constants.SERVICE_BASE_URL + eventsNoMediaPath;
                    if (firstThumbMedia != null && firstThumbMedia[0].media_type == "mp4" )
                    {
                        firstImageSource = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                    }
                    else if (firstThumbMedia[0] != null && firstThumbMedia[0].media_type == "3gpp" )
                    {
                        firstImageSource = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
                    else if (firstThumbMedia[0] != null && firstThumbMedia[0].media_type == "wav")
                    {
                        firstImageSource = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
                    firstEmotionsImage.Source = firstImageSource;
					firstEmotionsImage.ClassId = emotionsMasterList.event_details [index].event_id;
					firstEmotionsImage.GestureRecognizers.Add (  tap);
					firstEmotionsImage.Aspect = Aspect.Fill;
					//firstEmotionsImage.SetBinding(Image.SourceProperty, "FirstImage");


					customLayout.WidthRequest = screenWidth;
					customLayout.HeightRequest = 125;//screenHeight * Device.OnPlatform(30, 31, 7) / 100;


					StackLayout viewContainer = new StackLayout ();
					viewContainer.WidthRequest = App.screenWidth;
					viewContainer.HeightRequest = 100;//screenHeight * Device.OnPlatform(30, 27, 7) / 100;
					viewContainer.BackgroundColor = Color.White;

					Image divider = new Image ();
					divider.Source = "line_seperate.png";
					divider.BackgroundColor = Color.Transparent;
					divider.WidthRequest = App.screenWidth * 85 / 100;

					customLayout.AddChildToLayout (viewContainer, 0, Device.OnPlatform (-5, 0, 0));
					customLayout.AddChildToLayout (firstDetailsInfo, 5, Device.OnPlatform (-3, 2, 2));
					customLayout.AddChildToLayout (firstDateInfo, 5, Device.OnPlatform (4, 9, 6));
					customLayout.AddChildToLayout (firstEmotionsImage, Device.OnPlatform( 67, 65, 67 ), Device.OnPlatform (-3, 2, 1));

					masterStack.Children.Add (customLayout);
				}
				isFirstTime = false;
			} 
			else if (goalsMasterList != null) 
			{
				for (int index = 0; index < goalsMasterList.action_details.Count; index++)
				{
					TapGestureRecognizer tap = new TapGestureRecognizer ();
					tap.Tapped += OnGoalsTapped;;
					StackLayout cellMasterLayout = new StackLayout();
					cellMasterLayout.Orientation = StackOrientation.Vertical;
					cellMasterLayout.BackgroundColor = Color.White;

					StackLayout headerLayout = new StackLayout();
					headerLayout.Orientation = StackOrientation.Vertical;
					headerLayout.BackgroundColor = Color.FromRgb(244, 244, 244);

					CustomLayout customLayout = new CustomLayout();
					customLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
					double screenWidth = App.screenWidth;
					double screenHeight = App.screenHeight;
					customLayout.ClassId = goalsMasterList.action_details [index].goalaction_id;
					customLayout.GestureRecognizers.Add ( tap );


					CustomImageButton mainTitle = new CustomImageButton();
					//  mainTitle.IsEnabled = false;
					mainTitle.BackgroundColor = Color.FromRgb(30, 126, 210);
					mainTitle.ImageName = Device.OnPlatform("blue_bg.png", "blue_bg.png", @"/Assets/blue_bg.png");
					mainTitle.Text = "My Goals and Dreams";
					mainTitle.TextColor = Color.White;
					mainTitle.FontSize = Device.OnPlatform(12, 12, 18);
					mainTitle.WidthRequest = App.screenWidth;
					mainTitle.TextOrientation = TextOrientation.Middle;
					headerLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
					mainTitle.HeightRequest = 80;


					Label subTitle = new Label();
					subTitle.Text = (goalsMasterList.action_title != null && goalsMasterList.action_title.Count > 0) ? goalsMasterList.action_title[index].action_title : "empty";
					subTitle.TextColor = Color.Gray;
					subTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
					subTitle.XAlign = TextAlignment.Center;
					int subTitleFontSize = (App.screenDensity > 1.5) ? 18 : 16;
					subTitle.VerticalOptions = LayoutOptions.Center;
					subTitle.FontSize = Device.OnPlatform(subTitleFontSize, subTitleFontSize, 22);
					subTitle.WidthRequest = App.screenWidth * 90 / 100;
					headerLayout.HorizontalOptions = LayoutOptions.Center;
					subTitle.HeightRequest = Device.OnPlatform(40, 40, 30);

					Label firstDetailsInfo = new Label();
					string trimmedFirstDetails = (goalsMasterList.action_details != null &&goalsMasterList.action_details.Count > 0) ? goalsMasterList.action_details[index].action_details : "empty";
					if (trimmedFirstDetails != null && trimmedFirstDetails.Length > 50)
					{
						trimmedFirstDetails = trimmedFirstDetails.Substring(0, 50);
						trimmedFirstDetails = trimmedFirstDetails + "....";
						trimmedFirstDetails = trimmedFirstDetails.Replace("\\n", string.Empty);
						trimmedFirstDetails = trimmedFirstDetails.Replace("\\r", string.Empty);
					}
					firstDetailsInfo.Text = trimmedFirstDetails;
					firstDetailsInfo.TextColor = Color.Gray;
					firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
					firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
					firstDetailsInfo.HeightRequest = 40;
					firstDetailsInfo.ClassId = goalsMasterList.action_details [index].goalaction_id;
					firstDetailsInfo.GestureRecognizers.Add ( tap );
					int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(17, 16, 13) : 15;
					firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


					Label firstDateInfo = new Label();
					firstDateInfo.Text = (goalsMasterList.action_datetime != null && goalsMasterList.action_datetime.Count > 0) ? goalsMasterList.action_datetime[index].action_datetime : "empty";
					//firstDateInfo.Text = "2015 Januvary 30";
					firstDateInfo.TextColor = Color.Black;
					firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
					firstDateInfo.ClassId = goalsMasterList.action_details [index].goalaction_id;
					firstDateInfo.GestureRecognizers.Add ( tap );
					int dateFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(13, 15, 13) : 12;
					firstDateInfo.FontSize = Device.OnPlatform(dateFontSize, dateFontSize, dateFontSize);


					Image firstEmotionsImage = new Image();
					firstEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform (30, 28, 25) / 100;
					firstEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform (25, 22, 18) / 100;
					firstEmotionsImage.ClassId = goalsMasterList.action_details [index].goalaction_id;
                    string actionID = goalsMasterList.action_details[index].goalaction_id;
                    List<ActionMedia> FirstThumbMedia = goalsMasterList.action_media.FindAll(itm => itm.goalaction_id == actionID).ToList();
					firstEmotionsImage.GestureRecognizers.Add ( tap );
                    bool firstImageValidity = (FirstThumbMedia != null && FirstThumbMedia.Count > 0 && !string.IsNullOrEmpty(FirstThumbMedia[0].action_media)) ? true : false;
                    string firstImageSource = (firstImageValidity) ? Constants.SERVICE_BASE_URL + goalsMediaThumbPath + FirstThumbMedia[0].action_media : Constants.SERVICE_BASE_URL + goalsNoMediaPath;
                    if ( FirstThumbMedia!= null && FirstThumbMedia[0].media_type == "mp4")
                    {
                        firstImageSource = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                    }
                    else if (FirstThumbMedia[0].media_type == "3gpp")
                    {
                        firstImageSource = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
                    else if (FirstThumbMedia[0].media_type == "wav")
                    {
                        firstImageSource = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                    }
					firstEmotionsImage.Source = Device.OnPlatform(firstImageSource, firstImageSource, firstImageSource);
					//firstEmotionsImage.SetBinding(Image.SourceProperty, "FirstImage");


					customLayout.WidthRequest = screenWidth;
					customLayout.HeightRequest = 125;//screenHeight * Device.OnPlatform(30, 31, 7) / 100;


					StackLayout viewContainer = new StackLayout();
					viewContainer.WidthRequest = App.screenWidth;
					viewContainer.HeightRequest = 100;//screenHeight * Device.OnPlatform(30, 27, 7) / 100;
					viewContainer.BackgroundColor = Color.White;

					Image divider = new Image();
					divider.Source = "line_seperate.png";
					divider.BackgroundColor = Color.Transparent;
					divider.WidthRequest = App.screenWidth * 85 / 100;

					customLayout.AddChildToLayout(viewContainer, 0, Device.OnPlatform(-5, 0, 0));
					customLayout.AddChildToLayout(firstDetailsInfo, 5, Device.OnPlatform(-3, 2, 2));
					customLayout.AddChildToLayout(firstDateInfo, 5, Device.OnPlatform(4, 9, 6));
                    customLayout.AddChildToLayout(firstEmotionsImage, Device.OnPlatform(67, 65, 67), Device.OnPlatform(-3, 2, 1));

					masterStack.Children.Add(customLayout);
				}

				isFirstTime = false;
			}
           
        }

		async  void OnGoalsTapped (object sender, EventArgs e)
        {
			View tap = sender as View;
			if (tap != null && tap.ClassId != null)
			{
				List<ActionMedia> media = new List<ActionMedia>();
				ActionTitle eventTitle = new ActionTitle ();
				ActionDetail eventDetail = new ActionDetail ();

				eventDetail = goalsMasterList.action_details.FirstOrDefault (itm => itm.goalaction_id == tap.ClassId);
				eventTitle = goalsMasterList.action_title.FirstOrDefault ( itm => itm.goalaction_id == tap.ClassId );
				media = goalsMasterList.action_media.FindAll (itm => itm.goalaction_id == tap.ClassId).ToList();


				//	title = emotionList.eve
				if (media != null) 
				{
					await Navigation.PushAsync (new GemsDetailsPage(null, media, goalsMasterList.goal_title, eventTitle.action_title, eventDetail.action_details, goalsMediaPath, goalsNoMediaPath, eventDetail.goalaction_id, GemType.Action));

					eventDetail = null;
					eventTitle = null;
					media = null;
				}
			} 
			else
			{
				await DisplayAlert( Constants.ALERT_TITLE, "Not a valid Action", Constants.ALERT_OK );
			}
        }


		async void OnEmotionTapped (object sender, EventArgs e)
		{
			View tap = sender as View;
			if (tap != null && tap.ClassId != null)
			{
				List<EventMedia> media = new List<EventMedia>();
				EventTitle eventTitle = new EventTitle ();
				EventDetail eventDetail = new EventDetail ();


				eventDetail = emotionsMasterList.event_details.FirstOrDefault (itm => itm.event_id == tap.ClassId);
				eventTitle = emotionsMasterList.event_title.FirstOrDefault ( itm => itm.event_id == tap.ClassId );
				media = emotionsMasterList.event_media.FindAll (itm => itm.event_id == tap.ClassId).ToList();


				//	title = emotionList.eve
				if (media != null) 
				{
                    await Navigation.PushAsync(new GemsDetailsPage(media, null, emotionsMasterList.emotion_title, eventTitle.event_title, eventDetail.event_details, eventsMediaPath, eventsNoMediaPath, eventDetail.event_id, GemType.Event));


					eventDetail = null;
					eventTitle = null;
					media = null;
					GC.Collect ();
				}
			} 
			else
			{
				await DisplayAlert( Constants.ALERT_TITLE, "Not a valid Event", Constants.ALERT_OK );
			}
		}

        void NextButtonTapRecognizer_Tapped(object sender, EventArgs e)
        {

        }

		protected override bool OnBackButtonPressed ()
		{
			Dispose ();
			return base.OnBackButtonPressed ();
		}

        public void Dispose()
        {
			masterLayout = null;
			mainTitleBar = null;
			masterScroll = null;
			masterStack = null;
			emotionsMasterList = null;
			goalsMasterList = null;
			eventsMediaPath = null;
			eventsMediaThumbPath = null;
			goalsMediaPath = null;
			goalsMediaThumbPath = null;
			goalsNoMediaPath = null;
			eventsNoMediaPath = null;
			GC.Collect ();
        }

 
    }
}
