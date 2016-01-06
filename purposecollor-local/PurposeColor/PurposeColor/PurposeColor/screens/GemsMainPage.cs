using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using PurposeColor.Model;
using PurposeColor.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class GemsMainPage : ContentPage, IDisposable
    {
        CustomLayout masterLayout;
        IProgressBar progressBar;
        StackLayout listContainer;
        List<GemsPageInfo> gemsList;
        int listViewVislbleIndex;
        GemsPageTitleBar mainTitleBar;
        ScrollView masterScroll;
        StackLayout masterStack;
        GemsEmotionsObject gemsEmotionsObject;
        GemsGoalsObject gemsGoalsObject;
		List<GemsEmotionsDetails> emotionList;
		List<GemsGoalsDetails> goalsList;
        public GemsMainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();


            this.Appearing += OnAppearing;
           // PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar = new GemsPageTitleBar(Color.FromRgb(8, 135, 224), "Add Supporting Emotions", Color.White, "", false);
           


            masterScroll = new ScrollView();
            masterScroll.WidthRequest = App.screenWidth;
            masterScroll.HeightRequest = App.screenHeight * 85 / 100;

            masterStack = new StackLayout();
            masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.Transparent;


    

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(masterScroll, 0, 10);

            
        }

        void OnBackButtonTapRecognizerTapped(object sender, EventArgs e)
        {
            App.Navigator.PopAsync();
        }

        async void OnAppearing(object sender, EventArgs e)
        {
            IProgressBar progress = DependencyService.Get<IProgressBar>();

            if (gemsEmotionsObject != null || gemsGoalsObject != null)
                return;

            progress.ShowProgressbar( "Loading gems.." );
            gemsEmotionsObject = await ServiceHelper.GetAllSupportingEmotions();
            gemsGoalsObject = await ServiceHelper.GetAllSupportingGoals();
            if (gemsEmotionsObject == null)
            {
                var success = await DisplayAlert( Constants.ALERT_TITLE, "Error in fetching gems", Constants.ALERT_OK, Constants.ALERT_RETRY );
                if (!success)
                {
                    OnAppearing(sender, EventArgs.Empty);
                    return;
                }
                else
                {
                    progress.HideProgressbar();
                    return;
                }
                   
            }

            if (gemsGoalsObject == null)
            {
                var success = await DisplayAlert(Constants.ALERT_TITLE, "Error in fetching gems", Constants.ALERT_OK, Constants.ALERT_RETRY);
                if (!success)
                    OnAppearing(sender, EventArgs.Empty);
                else
                    return;
            }

            emotionList = new List<GemsEmotionsDetails>();
            if (gemsEmotionsObject.resultarray != null && gemsEmotionsObject.resultarray.Count > 1)
            {
                emotionList.Add(gemsEmotionsObject.resultarray[0]);
                emotionList.Add(gemsEmotionsObject.resultarray[1]);
            }

            goalsList = new List<GemsGoalsDetails>();
            if( gemsGoalsObject.resultarray != null && gemsGoalsObject.resultarray.Count > 1 )
            {
                goalsList.Add( gemsGoalsObject.resultarray[0] );
                goalsList.Add( gemsGoalsObject.resultarray[1] );
            }

            int emotionIndex = 0;
            foreach (var item in emotionList )
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
                mainTitle.Text = "My Supporting Emotions";
                mainTitle.TextColor = Color.White;
                mainTitle.FontSize = Device.OnPlatform(12, 18, 18);
                mainTitle.WidthRequest = App.screenWidth;
                mainTitle.TextOrientation = TextOrientation.Middle;
                headerLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
                mainTitle.HeightRequest = 80;

				TapGestureRecognizer titleTap = new TapGestureRecognizer ();
				titleTap.ClassId = item.event_title [0].event_id;
				titleTap.Tapped += OnEmotionTapped;
                Label subTitle = new Label();
                subTitle.Text = item.emotion_title;
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
				string trimmedFirstDetails = (item.event_details != null && item.event_details.Count > 0) ? item.event_details[0].event_details : "empty";
                if( trimmedFirstDetails != null && trimmedFirstDetails.Length > 50 )
                {
                    trimmedFirstDetails = trimmedFirstDetails.Substring(0, 50);
                    trimmedFirstDetails = trimmedFirstDetails + "....";
					trimmedFirstDetails = trimmedFirstDetails.Replace("\\n", string.Empty);
					trimmedFirstDetails = trimmedFirstDetails.Replace("\\r", string.Empty);
                }

				firstDetailsInfo.ClassId = item.event_title[0].event_id + "&&" + item.emotion_title;
                firstDetailsInfo.Text = trimmedFirstDetails;
                firstDetailsInfo.TextColor = Color.Gray;
                firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                firstDetailsInfo.HeightRequest = 45;
                int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(12, 16, 13) : 15;
                firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);
				firstDetailsInfo.GestureRecognizers.Add ( titleTap );


                Label firstDateInfo = new Label();
				firstDateInfo.ClassId = item.event_title [0].event_id + "&&" + item.emotion_title;
				firstDateInfo.Text = (item.event_datetime != null && item.event_datetime.Count > 0) ? item.event_datetime[0].event_datetime : "empty";
                //firstDateInfo.Text = "2015 Januvary 30";
                firstDateInfo.TextColor = Color.Black;
                firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                int dateFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(10, 13, 13) : 12;
                firstDateInfo.FontSize = Device.OnPlatform(dateFontSize, dateFontSize, dateFontSize);
				firstDateInfo.GestureRecognizers.Add ( titleTap );


                Image firstEmotionsImage = new Image();
				firstEmotionsImage.ClassId = item.event_title [0].event_id + "&&" + item.emotion_title;;
                firstEmotionsImage.Aspect = Aspect.Fill;
                firstEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform(23, 25, 22) / 100;
                firstEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform(17, 17, 14) / 100;
				string firstImageSource = (item.event_media != null && item.event_media.Count > 0) ? Constants.SERVICE_BASE_URL + gemsEmotionsObject.mediathumbpath + item.event_media[0].event_media : "no_image_found.jpg";
                if ( item.event_media[0] != null && item.event_media[0].media_type == "mp4")
                {
                    firstImageSource = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                }
                else if (item.event_media[0] != null && (item.event_media[0].media_type == "3gpp" || item.event_media[0].media_type == "wav"))
                {
                    firstImageSource = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                }
                firstEmotionsImage.Source = firstImageSource;
				firstEmotionsImage.GestureRecognizers.Add ( titleTap );
				//firstEmotionsImage.Source = "manali.jpg";
                //firstEmotionsImage.SetBinding(Image.SourceProperty, "FirstImage");



                Label secondDetailsInfo = new Label();
                string trimmedSecondDetails = (item.event_details != null && item.event_details.Count > 1) ? item.event_details[1].event_details : "empty";
                if (trimmedSecondDetails != null && trimmedSecondDetails.Length > 50)
                {
                    trimmedSecondDetails = trimmedSecondDetails.Substring(0, 50);
                    trimmedSecondDetails = trimmedSecondDetails + "....";
					trimmedSecondDetails = trimmedSecondDetails.Replace("\\n", string.Empty);
					trimmedSecondDetails = trimmedSecondDetails.Replace("\\r", string.Empty);
                }
                secondDetailsInfo.Text = trimmedSecondDetails;
				secondDetailsInfo.ClassId = item.event_details[1].event_id + "&&" + item.emotion_title;;
				secondDetailsInfo.GestureRecognizers.Add ( titleTap );
                // secondDetailsInfo.Text = "Referece site about lorem lpsum. Referece site about lorem lpsum. Referece site about lorem lpsum";
                secondDetailsInfo.TextColor = Color.Gray;
                secondDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                secondDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                secondDetailsInfo.HeightRequest = 45;
                secondDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


                Label secondDateInfo = new Label();
				secondDateInfo.ClassId = item.event_details[1].event_id + "&&" + item.emotion_title;;
				secondDateInfo.GestureRecognizers.Add ( titleTap );
				secondDateInfo.Text = (item.event_datetime != null && item.event_datetime.Count > 1) ? item.event_datetime[1].event_datetime: "empty";
                secondDateInfo.TextColor = Color.Black;
                secondDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                secondDateInfo.FontSize = Device.OnPlatform(dateFontSize, dateFontSize, dateFontSize);


                Image secondEmotionsImage = new Image();
				secondEmotionsImage.GestureRecognizers.Add ( titleTap );
				secondEmotionsImage.ClassId = item.event_details[1].event_id + "&&" + item.emotion_title;;
                secondEmotionsImage.Aspect = Aspect.Fill;
                secondEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform(23, 25, 22) / 100;
                secondEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform(17, 17, 14) / 100;
				string secondImageSource = (item.event_media != null && item.event_media.Count > 1) ? Constants.SERVICE_BASE_URL + gemsEmotionsObject.mediathumbpath + item.event_media[1].event_media : "no_image_found.jpg";
                if ( item.event_media[1] != null && item.event_media[1].media_type == "mp4")
                {
                    secondImageSource = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                }
                else if (item.event_media[1] != null && (item.event_media[1].media_type == "3gpp" || item.event_media[1].media_type == "wav"))
                {
                    secondImageSource = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                }
                secondEmotionsImage.Source = secondImageSource;
                //secondEmotionsImage.Source = "manali.jpg"; 


                CustomImageButton moreButton = new CustomImageButton();
                moreButton.BackgroundColor = Color.Transparent;
                moreButton.BorderColor = Color.Transparent;
                moreButton.BorderWidth = 0;
                moreButton.Text = "More";
				moreButton.FontSize = Device.OnPlatform (12, 15, 15);
				moreButton.MinimumHeightRequest = 20;
                moreButton.TextColor = Color.Silver;
                moreButton.ClassId = item.emotion_id.ToString();
                moreButton.Clicked += OnEmotionsMoreButtonClicked;

                customLayout.WidthRequest = screenWidth;
                customLayout.HeightRequest = 200;//screenHeight * Device.OnPlatform(30, 31, 7) / 100;


                StackLayout viewContainer = new StackLayout();
                viewContainer.WidthRequest = App.screenWidth * 90 / 100;
                viewContainer.HeightRequest = 175;//screenHeight * Device.OnPlatform(30, 27, 7) / 100;
                viewContainer.BackgroundColor = Color.White;

                Image divider = new Image();
                divider.Source = "line_seperate.png";
                divider.BackgroundColor = Color.Transparent;
                divider.WidthRequest = App.screenWidth * 85 / 100;

                /*  StackLayout whiteBorder = new StackLayout();
                  whiteBorder.BackgroundColor = Color.White;
                  whiteBorder.HeightRequest = 5;
                  whiteBorder.WidthRequest = App.screenWidth;*/

                if( emotionIndex == 0)
                headerLayout.Children.Add(mainTitle);
                headerLayout.Children.Add(subTitle);


                customLayout.AddChildToLayout(viewContainer, 0, Device.OnPlatform(-5, 0, 0));
                customLayout.AddChildToLayout(firstDetailsInfo, 5, Device.OnPlatform(-3, 2, 2));
                customLayout.AddChildToLayout(firstDateInfo, 5, Device.OnPlatform(4, 9, 7));
				customLayout.AddChildToLayout(firstEmotionsImage, Device.OnPlatform( 63, 60, 60 ), Device.OnPlatform(-2, 4, 1));
                customLayout.AddChildToLayout(divider, 5, 14);

                customLayout.AddChildToLayout(secondDetailsInfo, 5, Device.OnPlatform(14, 15, 11));
                customLayout.AddChildToLayout(secondDateInfo, 5, Device.OnPlatform(20, 22, 16));
				customLayout.AddChildToLayout(secondEmotionsImage, Device.OnPlatform( 63, 60, 60 ), Device.OnPlatform(14, 16, 12));
				customLayout.AddChildToLayout(moreButton, Device.OnPlatform( 77, 75, 75 ), Device.OnPlatform(24, 27, 19));

                double paddingLeft = App.screenWidth * 5 / 100;
                customLayout.Padding = new Thickness(paddingLeft, 0, paddingLeft, 0);

                masterStack.Children.Add(headerLayout);
                masterStack.Children.Add(customLayout);
                emotionIndex++;
                // masterStack.Children.Add( cellMasterLayout );
            }
            emotionIndex = 0;

			int goalsIndex = 0;
			foreach( var item in goalsList )
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
                mainTitle.FontSize = Device.OnPlatform(12, 18, 18);
                mainTitle.WidthRequest = App.screenWidth;
                mainTitle.TextOrientation = TextOrientation.Middle;
                headerLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
                mainTitle.HeightRequest = 80;


                Label subTitle = new Label();
                subTitle.Text = item.goal_title;
                subTitle.TextColor = Color.Gray;
                subTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                subTitle.XAlign = TextAlignment.Center;
                int subTitleFontSize = (App.screenDensity > 1.5) ? 18 : 16;
                subTitle.VerticalOptions = LayoutOptions.Center;
                subTitle.FontSize = Device.OnPlatform(subTitleFontSize, subTitleFontSize, 22);
                subTitle.WidthRequest = App.screenWidth * 90 / 100;
                headerLayout.HorizontalOptions = LayoutOptions.Center;
                subTitle.HeightRequest = Device.OnPlatform(40, 40, 30);

				TapGestureRecognizer goalsTap = new TapGestureRecognizer ();
				goalsTap.Tapped += GoalsTapped;
                Label firstDetailsInfo = new Label();
				string trimmedFirstDetails = (item.action_details != null && item.action_details.Count > 0 ) ? item.action_details[0].action_details : "empty";
                if (trimmedFirstDetails != null && trimmedFirstDetails.Length > 50)
                {
                    trimmedFirstDetails = trimmedFirstDetails.Substring(0, 50);
                    trimmedFirstDetails = trimmedFirstDetails + "....";
                    trimmedFirstDetails = trimmedFirstDetails.Replace("\\n", string.Empty);
                    trimmedFirstDetails = trimmedFirstDetails.Replace("\\r", string.Empty);
                }

  
				firstDetailsInfo.GestureRecognizers.Add ( goalsTap );
				firstDetailsInfo.ClassId = item.action_media [0].goalaction_id;
                firstDetailsInfo.Text = trimmedFirstDetails;
                firstDetailsInfo.TextColor = Color.Gray;
                firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                firstDetailsInfo.HeightRequest = 40;
                int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(12, 16, 13) : 15;
                firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


                Label firstDateInfo = new Label();
				firstDateInfo.ClassId = item.action_media [0].goalaction_id;
				firstDateInfo.GestureRecognizers.Add ( goalsTap );
				firstDateInfo.Text = (item.action_datetime != null && item.action_datetime.Count > 0 ) ? item.action_datetime[0].action_datetime : "empty";
                //firstDateInfo.Text = "2015 Januvary 30";
                firstDateInfo.TextColor = Color.Black;
                firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                int dateFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(10, 13, 13) : 12;
                firstDateInfo.FontSize = Device.OnPlatform(dateFontSize, dateFontSize, dateFontSize);


                Image firstEmotionsImage = new Image();
				firstEmotionsImage.ClassId = item.action_media [0].goalaction_id;
				firstEmotionsImage.GestureRecognizers.Add ( goalsTap );
                firstEmotionsImage.Aspect = Aspect.Fill;
                firstEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform(23, 25, 22) / 100;
                firstEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform(17, 17, 14) / 100;
                bool firstImageValidity = (item.action_media != null && item.action_media.Count > 0 && !string.IsNullOrEmpty(item.action_media[0].action_media)) ? true : false;
				//string firstImageSource = (item.action_media != null && item.action_media.Count > 0) ? Constants.SERVICE_BASE_URL +  gemsGoalsObject.mediathumbpath + item.action_media[0] : "no_image_found.jpg";
                string firstImageSource = (firstImageValidity) ? Constants.SERVICE_BASE_URL + gemsGoalsObject.mediathumbpath + item.action_media[0].action_media : Constants.SERVICE_BASE_URL + gemsGoalsObject.noimageurl;
                if (item.action_media[0] != null && item.action_media[0].media_type == "mp4")
                {
                    firstImageSource = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                }
                else if (item.action_media[0] != null && (item.action_media[0].media_type == "3gpp" || item.action_media[0].media_type == "wav"))
                {
                    firstImageSource = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                }

                firstEmotionsImage.Source = firstImageSource;
                //firstEmotionsImage.Source = "manali.jpg";



                Label secondDetailsInfo = new Label();
				secondDetailsInfo.GestureRecognizers.Add ( goalsTap );
				string trimmedSecondDetails = (item.action_details != null && item.action_details.Count > 1) ? item.action_details[1].action_details : "empty";
                if (trimmedSecondDetails != null && trimmedSecondDetails.Length > 50)
                {
                    trimmedSecondDetails = trimmedSecondDetails.Substring(0, 50);
                    trimmedSecondDetails = trimmedSecondDetails + "....";
                    trimmedSecondDetails = trimmedSecondDetails.Replace("\\n", string.Empty);
                    trimmedSecondDetails = trimmedSecondDetails.Replace("\\r", string.Empty);
                }
                secondDetailsInfo.Text = trimmedSecondDetails;
				secondDetailsInfo.ClassId = ( item.action_media.Count > 1 ) ? item.action_media [1].goalaction_id : null;
                // secondDetailsInfo.Text = "Referece site about lorem lpsum. Referece site about lorem lpsum. Referece site about lorem lpsum";
                secondDetailsInfo.TextColor = Color.Gray;
                secondDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                secondDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                secondDetailsInfo.HeightRequest = 40;
                secondDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


                Label secondDateInfo = new Label();
				secondDateInfo.ClassId =  ( item.action_media.Count > 1 ) ? item.action_media [1].goalaction_id : null;
				secondDateInfo.GestureRecognizers.Add ( goalsTap );
				secondDateInfo.Text = (item.action_datetime != null && item.action_datetime.Count > 1) ? item.action_datetime[1].action_datetime : "empty";
                secondDateInfo.TextColor = Color.Black;
                secondDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                secondDateInfo.FontSize = Device.OnPlatform(dateFontSize, dateFontSize, dateFontSize);


                Image secondEmotionsImage = new Image();
				secondEmotionsImage.ClassId =  ( item.action_media.Count > 1 ) ? item.action_media [1].goalaction_id : null;
				secondEmotionsImage.GestureRecognizers.Add ( goalsTap );
                secondEmotionsImage.Aspect = Aspect.Fill;
                secondEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform(23, 25, 22) / 100;
                secondEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform(17, 17, 14) / 100;
                bool secondImageValidity = (item.action_media != null && item.action_media.Count > 1 && !string.IsNullOrEmpty(item.action_media[1].action_media)) ? true : false;
                string secondImageSource = (secondImageValidity) ? Constants.SERVICE_BASE_URL + gemsGoalsObject.mediathumbpath + item.action_media[1].action_media : Constants.SERVICE_BASE_URL + gemsGoalsObject.noimageurl;
                //secondEmotionsImage.Source = "manali.jpg"; 
                if ( item.action_media[1] != null && item.action_media[1].media_type == "mp4")
                {
                    secondImageSource = Device.OnPlatform("video.png", "video.png", "//Assets//video.png");
                }
                else if (item.action_media[1] != null && (item.action_media[1].media_type == "3gpp" || item.action_media[1].media_type == "3gpp"))
                {
                    secondImageSource = Device.OnPlatform("audio.png", "audio.png", "//Assets//audio.png");
                }
                secondEmotionsImage.Source = secondImageSource;


				CustomImageButton moreButton = new CustomImageButton();
                moreButton.BackgroundColor = Color.Transparent;
                moreButton.BorderColor = Color.Transparent;
                moreButton.BorderWidth = 0;
                moreButton.Text = "More";
				moreButton.FontSize = Device.OnPlatform (12, 15, 15);
                moreButton.MinimumHeightRequest = 20;
                moreButton.TextColor = Color.Silver;
				moreButton.ClassId = item.goal_id.ToString();
                moreButton.Clicked += OnGoalsMore;

                customLayout.WidthRequest = screenWidth;
                customLayout.HeightRequest = 200;//screenHeight * Device.OnPlatform(30, 31, 7) / 100;


                StackLayout viewContainer = new StackLayout();
                viewContainer.WidthRequest = App.screenWidth * 90 / 100;
                viewContainer.HeightRequest = 175;//screenHeight * Device.OnPlatform(30, 27, 7) / 100;
                viewContainer.BackgroundColor = Color.White;

                Image divider = new Image();
                divider.Source = "line_seperate.png";
                divider.BackgroundColor = Color.Transparent;
                divider.WidthRequest = App.screenWidth * 85 / 100;


				if (goalsIndex == 0)
                    headerLayout.Children.Add(mainTitle);
                headerLayout.Children.Add(subTitle);


                customLayout.AddChildToLayout(viewContainer, 0, Device.OnPlatform(-5, 0, 0));
                customLayout.AddChildToLayout(firstDetailsInfo, 5, Device.OnPlatform(-3, 2, 2));
                customLayout.AddChildToLayout(firstDateInfo, 5, Device.OnPlatform(4, 9, 7));
				customLayout.AddChildToLayout(firstEmotionsImage, Device.OnPlatform( 63, 60, 60 ), Device.OnPlatform(-2, 4, 1));
                customLayout.AddChildToLayout(divider, 5, 14);

                customLayout.AddChildToLayout(secondDetailsInfo, 5, Device.OnPlatform(14, 15, 11));
                customLayout.AddChildToLayout(secondDateInfo, 5, Device.OnPlatform(20, 22, 16));
				customLayout.AddChildToLayout(secondEmotionsImage, Device.OnPlatform( 63, 60, 60 ), Device.OnPlatform(14, 16, 12));
				customLayout.AddChildToLayout(moreButton, Device.OnPlatform( 77, 75, 75 ), Device.OnPlatform(24, 27, 19));

                double paddingLeft = App.screenWidth * 5 / 100;
                customLayout.Padding = new Thickness(paddingLeft, 0, paddingLeft, 0);

                masterStack.Children.Add(headerLayout);
                masterStack.Children.Add(customLayout);
                goalsIndex++;
                // masterStack.Children.Add( cellMasterLayout );
            }
            goalsIndex = 0;

            masterScroll.Scrolled += OnScroll;

			StackLayout empty = new StackLayout ();
			empty.HeightRequest = Device.OnPlatform( 50, 50, 100 );
			empty.WidthRequest = App.screenWidth * 90 / 100;
			empty.BackgroundColor = Color.Transparent;
			masterStack.Children.Add ( empty );

            masterScroll.Content = masterStack;
            Content = masterLayout;


            progress.HideProgressbar();
        }


		async void OnEmotionTapped (object sender, EventArgs e)
		{
			
			View tap = sender as View;
			if (tap != null && tap.ClassId != null)
			{
				string[] delimiters = { "&&" };
				string[] clasIDArray = tap.ClassId.Split(delimiters, StringSplitOptions.None);
				string selectedEmotionName = clasIDArray [1];
				string selectedEventID = clasIDArray [0];

				List<EventMedia> media = new List<EventMedia>();
				EventTitle eventTitle = new EventTitle ();
				EventDetail eventDetail = new EventDetail ();
			
				for (int index = 0;  index < emotionList.Count; index++)
				{
					media = emotionList[index].event_media.FindAll (med => med.event_id == selectedEventID).ToList();
					if (media != null && media.Count > 0 )
					{
						break;
					}
				}

				for (int index = 0;  index < emotionList.Count; index++)
				{
					eventTitle = emotionList [index].event_title.FirstOrDefault (itm => itm.event_id == selectedEventID);
					if (eventTitle != null)
					{
						break;
					}
				}

				for (int index = 0;  index < emotionList.Count; index++)
				{
					eventDetail = emotionList [index].event_details.FirstOrDefault (itm => itm.event_id == selectedEventID);
					if (eventDetail != null)
					{
						break;
					}
				}
					
		
			//	title = emotionList.eve
				if (media != null) 
				{
					await Navigation.PushAsync (new GemsDetailsPage (media, null, selectedEmotionName, eventTitle.event_title, eventDetail.event_details, gemsEmotionsObject.mediapath, gemsEmotionsObject.noimageurl, eventDetail.event_id));
				}
			} 
			else
			{
				await DisplayAlert( Constants.ALERT_TITLE, "Not a valid Event", Constants.ALERT_OK );
			}
		

		}


		async void GoalsTapped (object sender, EventArgs e)
		{
			View goalsTap = sender as View;

			if (goalsTap != null && goalsTap.ClassId != null)
			{
				List<ActionMedia> media = new List<ActionMedia>();
				ActionTitle eventTitle = new ActionTitle ();
				ActionDetail eventDetail = new ActionDetail ();

				for (int index = 0;  index < goalsList.Count; index++)
				{
					media = goalsList[index].action_media.FindAll (med => med.goalaction_id == goalsTap.ClassId).ToList();
					if (media != null && media.Count > 0 )
					{
						break;
					}
				}

				for (int index = 0;  index < goalsList.Count; index++)
				{
					eventTitle = goalsList [index].action_title.FirstOrDefault (itm => itm.goalaction_id == goalsTap.ClassId);
					if (eventTitle != null)
					{
						break;
					}
				}

				for (int index = 0;  index < goalsList.Count; index++)
				{
					eventDetail = goalsList [index].action_details.FirstOrDefault (itm => itm.goalaction_id == goalsTap.ClassId);
					if (eventDetail != null)
					{
						break;
					}
				}

				if (media != null) 
				{
					await Navigation.PushAsync (new GemsDetailsPage(null, media, "", eventTitle.action_title, eventDetail.action_details, gemsGoalsObject.mediapath, gemsGoalsObject.noimageurl, eventDetail.goalaction_id));
				}
			}
			else
			{
				await DisplayAlert( Constants.ALERT_TITLE, "Not a valid goal", Constants.ALERT_OK );
			}
		}

        async void OnEmotionsMoreButtonClicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            GemsEmotionsDetails emotionDetailsList = gemsEmotionsObject.resultarray.FirstOrDefault(item => item.emotion_id == btn.ClassId);

			await Navigation.PushAsync(new GemsMoreDetailsPage(emotionDetailsList, null, gemsEmotionsObject.mediapath, gemsEmotionsObject.mediathumbpath, gemsEmotionsObject.noimageurl, null, null, null));
        }

		async  void OnGoalsMore (object sender, EventArgs e)
        {
			Button btn = sender as Button;
			GemsGoalsDetails goalsDetailsList = gemsGoalsObject.resultarray.FirstOrDefault(item => item.goal_id == btn.ClassId);

			await Navigation.PushAsync(new GemsMoreDetailsPage(null, goalsDetailsList, null, null, null, gemsGoalsObject.mediapath, gemsGoalsObject.mediathumbpath, gemsGoalsObject.noimageurl));
        }

        void OnScroll(object sender, ScrolledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Scroll pos : " + masterScroll.ScrollY.ToString());

            if( masterScroll.ScrollY > 1 && masterScroll.ScrollY < 600 )
            {
				if( mainTitleBar.title.Text != "My Supporting Emotions" )
                mainTitleBar.title.Text = "My Supporting Emotions";
            }
            else if (masterScroll.ScrollY > 600 && masterScroll.ScrollY < 900)
            {
                if( mainTitleBar.title.Text != "My Goals and Dreams")
					mainTitleBar.title.Text = "My Goals and Dreams";
            }

        }

        void NextButtonTapRecognizer_Tapped(object sender, EventArgs e)
        {
           
        }

        public void Dispose()
        {
            masterLayout = null;
            progressBar = null;
            listContainer = null;
            gemsList = null;
            mainTitleBar = null;
            masterScroll = null; ;
            masterStack = null;
            GemsEmotionsObject gemsEmotionsObject;
        }

        public void ScrollVisibleItems( int visbleIndex )
        {
     
            if (visbleIndex >= 0 && visbleIndex < gemsList.Count && listViewVislbleIndex != visbleIndex)
            {
                GemsPageInfo gems = gemsList[visbleIndex];

                /*if( gems.IsMainTitleVisible )
                {
                    
                }*/
                if( visbleIndex < 2 )
                {
                    mainTitleBar.title.Text = "My Supporting Emotions";
                }
                else 
                {
                    mainTitleBar.title.Text = "My Goals and Dreams";
                }
               
            }

            listViewVislbleIndex = visbleIndex;
        }
    }


    public class GemsListCellTemplate : ViewCell
    {
        public GemsListCellTemplate()
        {

            StackLayout cellMasterLayout = new StackLayout();
            cellMasterLayout.Orientation = StackOrientation.Vertical;
            cellMasterLayout.BackgroundColor = Color.White;


            StackLayout headerLayout = new StackLayout();
            headerLayout.Orientation = StackOrientation.Vertical;
            headerLayout.BackgroundColor = Color.FromRgb(244, 244, 244);

            CustomLayout customLayout = new CustomLayout();
            customLayout.BackgroundColor =  Color.FromRgb(244, 244, 244);
            double screenWidth = App.screenWidth;
            double screenHeight = App.screenHeight;

            CustomImageButton mainTitle = new CustomImageButton();
          //  mainTitle.IsEnabled = false;
            mainTitle.BackgroundColor = Color.FromRgb(30, 126, 210);
            mainTitle.ImageName = "blue_bg.png";
            mainTitle.SetBinding(CustomImageButton.TextProperty, "MainTitle");
            mainTitle.TextColor = Color.White;
            mainTitle.FontSize = 12;
            mainTitle.WidthRequest = App.screenWidth;
           // mainTitle.HeightRequest = App.screenHeight * 5 / 100;
            mainTitle.TextOrientation = TextOrientation.Middle;
            mainTitle.SetBinding(CustomImageButton.IsVisibleProperty, "IsMainTitleVisible");
            headerLayout.VerticalOptions = LayoutOptions.Center;


            Label subTitle = new Label();
            subTitle.SetBinding(Label.TextProperty, "SubTitle");
            subTitle.TextColor = Color.Gray;
            subTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            subTitle.XAlign = TextAlignment.Center;
            int subTitleFontSize = (App.screenDensity > 1.5) ? 18 : 16;
            subTitle.VerticalOptions = LayoutOptions.Center;
            subTitle.FontSize = Device.OnPlatform(subTitleFontSize, subTitleFontSize, 22);
            subTitle.WidthRequest = App.screenWidth * 90 / 100;
            subTitle.HeightRequest = 40;

            Label firstDetailsInfo = new Label();
            firstDetailsInfo.SetBinding(Label.TextProperty, "TrimmedFirstDetailsInfo");
            //firstDetailsInfo.Text = "Referece site about lorem lpsum. Referece site about lorem lpsum. Referece site about lorem lpsum";
            firstDetailsInfo.TextColor = Color.Gray;
            firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
            firstDetailsInfo.HeightRequest = 45;
            int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? 17 : 15;
            firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, 22);


            Label firstDateInfo = new Label();
            firstDateInfo.SetBinding(Label.TextProperty, "FirstDateInfo");
            //firstDateInfo.Text = "2015 Januvary 30";
            firstDateInfo.TextColor = Color.Black;
            firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            int detailsFontSize = (App.screenDensity > 1.5) ? 15 : 12;
            firstDateInfo.FontSize = Device.OnPlatform(detailsFontSize, detailsFontSize, 22);


            Image firstEmotionsImage = new Image();
            firstEmotionsImage.WidthRequest = App.screenWidth * 25 / 100;
            firstEmotionsImage.HeightRequest = App.screenWidth * 25 / 100;
            //firstEmotionsImage.Source = "manali.jpg";
            firstEmotionsImage.SetBinding(Image.SourceProperty, "FirstImage");



            Label secondDetailsInfo = new Label();
            secondDetailsInfo.SetBinding(Label.TextProperty, "SecondDetailsInfo");
           // secondDetailsInfo.Text = "Referece site about lorem lpsum. Referece site about lorem lpsum. Referece site about lorem lpsum";
            secondDetailsInfo.TextColor = Color.Gray;
            secondDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            secondDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
            secondDetailsInfo.HeightRequest = 45;
            secondDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, 22);


            Label secondDateInfo = new Label();
            secondDateInfo.SetBinding(Label.TextProperty, "SecondDateInfo");
            secondDateInfo.TextColor = Color.Black;
            secondDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            firstDateInfo.FontSize = Device.OnPlatform(detailsFontSize, detailsFontSize, 22);


            Image secondEmotionsImage = new Image();
            secondEmotionsImage.WidthRequest = App.screenWidth * 25 / 100;
            secondEmotionsImage.HeightRequest = App.screenWidth * 25 / 100;
            secondEmotionsImage.SetBinding(Image.SourceProperty, "SecondImage");


            Button moreButton = new Button();
            moreButton.BackgroundColor = Color.Transparent;
            moreButton.BorderColor = Color.Transparent;
            moreButton.BorderWidth = 0;
            moreButton.Text = "more";
            moreButton.FontSize = 15;
            moreButton.MinimumHeightRequest = 20;
            moreButton.TextColor = Color.Silver;

            customLayout.WidthRequest = screenWidth;
            customLayout.HeightRequest = screenHeight * Device.OnPlatform(30, 31, 7) / 100;

            
            StackLayout viewContainer = new StackLayout();
            viewContainer.WidthRequest = App.screenWidth;
            viewContainer.HeightRequest = screenHeight * Device.OnPlatform(30, 27, 7) / 100;
            viewContainer.BackgroundColor = Color.White;

            Image divider = new Image();
            divider.Source = "line_seperate.png";
            divider.BackgroundColor = Color.Transparent;
            divider.WidthRequest = App.screenWidth * 85 / 100;

          /*  StackLayout whiteBorder = new StackLayout();
            whiteBorder.BackgroundColor = Color.White;
            whiteBorder.HeightRequest = 5;
            whiteBorder.WidthRequest = App.screenWidth;*/

            headerLayout.Children.Add( mainTitle );
            headerLayout.Children.Add( subTitle );


            customLayout.AddChildToLayout(viewContainer, 0, 0 );
            customLayout.AddChildToLayout(firstDetailsInfo, 5, 2);
            customLayout.AddChildToLayout(firstDateInfo, 5, 9);
            customLayout.AddChildToLayout(firstEmotionsImage, 65, 0);
            customLayout.AddChildToLayout(divider, 5, 14);

            customLayout.AddChildToLayout(secondDetailsInfo, 5, 15);
            customLayout.AddChildToLayout(secondDateInfo, 5, 22 );
            customLayout.AddChildToLayout(secondEmotionsImage, 65, 13);
            customLayout.AddChildToLayout(moreButton, 75, 25);

            cellMasterLayout.Children.Add( headerLayout );
            cellMasterLayout.Children.Add( customLayout );
           // masterLayout.AddChildToLayout(whiteBorder, 0, 45);

          /*  masterLayout.AddChildToLayout(firstDetailsInfo, (float)8, (float)Device.OnPlatform(5, 15, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(firstDateInfo, (float)8, (float)Device.OnPlatform(5, 30, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(firstEmotionsImage, (float)70, (float)Device.OnPlatform(5, 15, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);

            masterLayout.AddChildToLayout(divider, (float)5, (float)Device.OnPlatform(5, 50, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);


            masterLayout.AddChildToLayout(secondDateInfo, (float)8, (float)Device.OnPlatform(5, 65, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(secondDateInfo, (float)8, (float)Device.OnPlatform(5, 80, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(secondEmotionsImage, (float)70, (float)Device.OnPlatform(5, 65, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            */

            this.View = cellMasterLayout;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }


    public class GemsPageInfo
    {
        public string MainTitle { get; set; }
        public string SubTitle { get; set; }
        public string FirstDetailsInfo { get; set; }
        public string FirstDateInfo { get; set; }
        public string SecondDetailsInfo { get; set; }
        public string SecondDateInfo { get; set; }
        public string FirstImage { get; set; }
        public string SecondImage { get; set; }
        public bool IsMainTitleVisible { get; set; }
        public string TrimmedFirstDetailsInfo
        {
            get
            {
                if (FirstDetailsInfo != null && FirstDetailsInfo.Length > 50)
                {
                    string trimmedInfo = FirstDetailsInfo.Substring(0, 50);
                    return trimmedInfo + "....";
                }
                return FirstDetailsInfo;
            }
        }
    }
}
