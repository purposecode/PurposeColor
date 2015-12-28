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


            // PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar = new GemsPageTitleBarWithBack(Color.FromRgb(8, 135, 224), "Add Supporting Emotions", Color.White, "", false);
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
				App.Navigator.PopModalAsync();
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


            // this.Animate("", (s) => Layout(new Rectangle(X, (1 - s) * Height, Width, Height)), 0, 1000, Easing.SpringIn, null, null); // slide up
            this.Animate("", (s) => Layout(new Rectangle(X, (s - 1) * Height, Width, Height)), 0, 1000, Easing.SpringIn, null, null);

			if (emotionsMasterList != null)
			{
				for (int index = 0; index < emotionsMasterList.event_details.Count; index++) {
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
					int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform (17, 16, 13) : 15;
					firstDetailsInfo.FontSize = Device.OnPlatform (firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


					Label firstDateInfo = new Label ();
					firstDateInfo.Text = (emotionsMasterList.event_datetime != null && emotionsMasterList.event_datetime.Count > 0) ? emotionsMasterList.event_datetime [index].event_datetime : "empty";
					//firstDateInfo.Text = "2015 Januvary 30";
					firstDateInfo.TextColor = Color.Black;
					firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
					int dateFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform (15, 15, 13) : 12;
					firstDateInfo.FontSize = Device.OnPlatform (dateFontSize, dateFontSize, dateFontSize);


					Image firstEmotionsImage = new Image ();
					firstEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform (25, 30, 20) / 100;
					firstEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform (25, 30, 20) / 100;
					bool firstImageValidity = ( emotionsMasterList.event_media != null &&  emotionsMasterList.event_media.Count > 0 && !string.IsNullOrEmpty ( emotionsMasterList.event_media[index].event_media)) ? true : false;
					string firstImageSource = (firstImageValidity) ? Constants.SERVICE_BASE_URL + eventsMediaThumbPath + emotionsMasterList.event_media [index] : Constants.SERVICE_BASE_URL + eventsNoMediaPath;
					firstEmotionsImage.Source = Device.OnPlatform (firstImageSource, firstImageSource, firstImageSource);
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
					customLayout.AddChildToLayout (firstDateInfo, 5, Device.OnPlatform (4, 9, 5));
					customLayout.AddChildToLayout (firstEmotionsImage, 65, Device.OnPlatform (-5, 0, 0));

					masterStack.Children.Add (customLayout);
				}
			} 
			else if (goalsMasterList != null) 
			{
				for (int index = 0; index < goalsMasterList.action_details.Count; index++)
				{
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
					int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(17, 16, 13) : 15;
					firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


					Label firstDateInfo = new Label();
					firstDateInfo.Text = (goalsMasterList.action_datetime != null && goalsMasterList.action_datetime.Count > 0) ? goalsMasterList.action_datetime[index].action_datetime : "empty";
					//firstDateInfo.Text = "2015 Januvary 30";
					firstDateInfo.TextColor = Color.Black;
					firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
					int dateFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(15, 15, 13) : 12;
					firstDateInfo.FontSize = Device.OnPlatform(dateFontSize, dateFontSize, dateFontSize);


					Image firstEmotionsImage = new Image();
					firstEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform(25, 30, 20) / 100;
					firstEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform(25, 30, 20) / 100;

					bool firstImageValidity = ( goalsMasterList.action_media != null && goalsMasterList.action_media.Count > 0 && !string.IsNullOrEmpty (goalsMasterList.action_media[index].event_media)) ? true : false;
					string firstImageSource = ( firstImageValidity ) ? Constants.SERVICE_BASE_URL +  goalsMediaThumbPath + goalsMasterList.action_media[index] : Constants.SERVICE_BASE_URL + goalsNoMediaPath;
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
					customLayout.AddChildToLayout(firstDateInfo, 5, Device.OnPlatform(4, 9, 5));
					customLayout.AddChildToLayout(firstEmotionsImage, 65, Device.OnPlatform(-5, 0, 0));

					masterStack.Children.Add(customLayout);
				}
			}
           
        }

       /* void OnScroll(object sender, ScrolledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Scroll pos : " + masterScroll.ScrollY.ToString());

            if (masterScroll.ScrollY > 1 && masterScroll.ScrollY < 307)
            {
                if (mainTitleBar.title.Text != "0 th Menu")
                    mainTitleBar.title.Text = "My Supporting Emotions";
            }
            else if (masterScroll.ScrollY > 307 && masterScroll.ScrollY < 610)
            {
                if (mainTitleBar.title.Text != "1 th Menu")
                    mainTitleBar.title.Text = "1 th Menu";
            }
            else if (masterScroll.ScrollY > 610 && masterScroll.ScrollY < 900)
            {
                if (mainTitleBar.title.Text != "2 th Menu")
                    mainTitleBar.title.Text = "2 th Menu";
            }

        }*/

        void NextButtonTapRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        public void Dispose()
        {

        }

 
    }
}
