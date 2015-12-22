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

          
        }

        async void OnAppearing(object sender, EventArgs e)
        {
            IProgressBar progress = DependencyService.Get<IProgressBar>();
            progress.ShowProgressbar( "Loading gems.." );
            gemsEmotionsObject = await ServiceHelper.GetAllSupportingEmotions();
            if (gemsEmotionsObject == null)
            {
                var success = await DisplayAlert( Constants.ALERT_TITLE, "Error in fetching gems", Constants.ALERT_OK, Constants.ALERT_RETRY );
                if (!success)
                    OnAppearing(sender, EventArgs.Empty);
                else
                    return;
            }

            List<GemsEmotionsDetails> emotionList = new List<GemsEmotionsDetails>();
            if (gemsEmotionsObject.resultarray != null && gemsEmotionsObject.resultarray.Count > 1)
            {
                emotionList.Add(gemsEmotionsObject.resultarray[0]);
                emotionList.Add(gemsEmotionsObject.resultarray[3]);
            }

            int index = 0;
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
                mainTitle.FontSize = Device.OnPlatform(12, 12, 18);
                mainTitle.WidthRequest = App.screenWidth;
                mainTitle.TextOrientation = TextOrientation.Middle;
                headerLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
                mainTitle.HeightRequest = 80;


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
                string trimmedFirstDetails = (item.event_details != null && item.event_details.Count > 0) ? item.event_details[0] : "empty";
                if( trimmedFirstDetails != null && trimmedFirstDetails.Length > 50 )
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
                firstDetailsInfo.HeightRequest = 45;
                int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(17, 16, 13) : 15;
                firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


                Label firstDateInfo = new Label();
                firstDateInfo.Text = (item.event_datetime != null && item.event_datetime.Count > 0) ? item.event_datetime[0] : "empty";
                //firstDateInfo.Text = "2015 Januvary 30";
                firstDateInfo.TextColor = Color.Black;
                firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                int dateFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(15, 15, 13) : 12;
                firstDateInfo.FontSize = Device.OnPlatform(dateFontSize, dateFontSize, dateFontSize);


                Image firstEmotionsImage = new Image();
                firstEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform(25, 25, 20) / 100;
                firstEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform(25, 25, 20) / 100;
                string firstImageSource = (item.event_media != null && item.event_media.Count > 0) ? Constants.SERVICE_BASE_URL + gemsEmotionsObject.mediathumbpath + item.event_media[0] : "no_image_found.jpg";
                firstEmotionsImage.Source = Device.OnPlatform("manali.jpg", firstImageSource, "//Assets//manali.jpg");
                //firstEmotionsImage.SetBinding(Image.SourceProperty, "FirstImage");



                Label secondDetailsInfo = new Label();
                string trimmedSecondDetails = (item.event_details != null && item.event_details.Count > 1) ? item.event_details[1] : "empty";
                if (trimmedSecondDetails != null && trimmedSecondDetails.Length > 50)
                {
                    trimmedSecondDetails = trimmedSecondDetails.Substring(0, 50);
                    trimmedSecondDetails = trimmedSecondDetails + "....";
					trimmedSecondDetails = trimmedSecondDetails.Replace("\\n", string.Empty);
					trimmedSecondDetails = trimmedSecondDetails.Replace("\\r", string.Empty);
                }
                secondDetailsInfo.Text = trimmedSecondDetails;
                // secondDetailsInfo.Text = "Referece site about lorem lpsum. Referece site about lorem lpsum. Referece site about lorem lpsum";
                secondDetailsInfo.TextColor = Color.Gray;
                secondDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                secondDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                secondDetailsInfo.HeightRequest = 45;
                secondDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


                Label secondDateInfo = new Label();
                secondDateInfo.Text = (item.event_datetime != null && item.event_datetime.Count > 1) ? item.event_datetime[1] : "empty";
                secondDateInfo.TextColor = Color.Black;
                secondDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                secondDateInfo.FontSize = Device.OnPlatform(dateFontSize, dateFontSize, dateFontSize);


                Image secondEmotionsImage = new Image();
                secondEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform(25, 25, 20) / 100;
                secondEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform(25, 25, 20) / 100;
                string secondImageSource = (item.event_media != null && item.event_media.Count > 1) ? Constants.SERVICE_BASE_URL + gemsEmotionsObject.mediathumbpath + item.event_media[1] : "no_image_found.jpg";
                secondEmotionsImage.Source = Device.OnPlatform("manali.jpg", secondImageSource, "//Assets//manali.jpg");


                Button moreButton = new Button();
                moreButton.BackgroundColor = Color.Transparent;
                moreButton.BorderColor = Color.Transparent;
                moreButton.BorderWidth = 0;
                moreButton.Text = "more";
                moreButton.FontSize = 15;
                moreButton.MinimumHeightRequest = 20;
                moreButton.TextColor = Color.Silver;
                moreButton.ClassId = item.emotion_id.ToString();
                moreButton.Clicked += OnMoreButtonClicked;

                customLayout.WidthRequest = screenWidth;
                customLayout.HeightRequest = 200;//screenHeight * Device.OnPlatform(30, 31, 7) / 100;


                StackLayout viewContainer = new StackLayout();
                viewContainer.WidthRequest = App.screenWidth;
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

                if( index == 0)
                headerLayout.Children.Add(mainTitle);
                headerLayout.Children.Add(subTitle);


                customLayout.AddChildToLayout(viewContainer, 0, Device.OnPlatform(-5, 0, 0));
                customLayout.AddChildToLayout(firstDetailsInfo, 5, Device.OnPlatform(-3, 2, 2));
                customLayout.AddChildToLayout(firstDateInfo, 5, Device.OnPlatform(4, 9, 5));
                customLayout.AddChildToLayout(firstEmotionsImage, 65, Device.OnPlatform(-5, 0, 0));
                customLayout.AddChildToLayout(divider, 5, 14);

                customLayout.AddChildToLayout(secondDetailsInfo, 5, Device.OnPlatform(10, 15, 11));
                customLayout.AddChildToLayout(secondDateInfo, 5, Device.OnPlatform(17, 22, 15));
                customLayout.AddChildToLayout(secondEmotionsImage, 65, Device.OnPlatform(8, 13, 10));
                customLayout.AddChildToLayout(moreButton, 75, Device.OnPlatform(25, 25, 19));

                masterStack.Children.Add(headerLayout);
                masterStack.Children.Add(customLayout);
                index++;
                // masterStack.Children.Add( cellMasterLayout );
            }
            index = 0;



            masterScroll.Scrolled += OnScroll;
            masterScroll.Content = masterStack;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(masterScroll, 0, 10);
            Content = masterLayout;


            progress.HideProgressbar();
        }


        async void OnMoreButtonClicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            List<GemsEmotionsDetails> emotionDetailsList = gemsEmotionsObject.resultarray.FindAll(item => item.emotion_id == btn.ClassId).ToList();
            await Navigation.PushModalAsync(new GemsMoreDetailsPage(emotionDetailsList));
        }

        void OnScroll(object sender, ScrolledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Scroll pos : " + masterScroll.ScrollY.ToString());

            if( masterScroll.ScrollY > 1 && masterScroll.ScrollY < 307 )
            {
                if( mainTitleBar.title.Text != "0 th Menu" )
                mainTitleBar.title.Text = "My Supporting Emotions";
            }
            else if( masterScroll.ScrollY > 307 && masterScroll.ScrollY < 610 )
            {
                if( mainTitleBar.title.Text != "1 th Menu")
                mainTitleBar.title.Text = "1 th Menu";
            }
            else if (masterScroll.ScrollY > 610 && masterScroll.ScrollY < 900)
            {
                if( mainTitleBar.title.Text != "2 th Menu")
                mainTitleBar.title.Text = "2 th Menu";
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
