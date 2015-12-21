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
        IProgressBar progressBar;
        StackLayout listContainer;
        List<GemsPageInfo> gemsList;
        int listViewVislbleIndex;
        GemsPageTitleBar mainTitleBar;
        ScrollView masterScroll;
        StackLayout masterStack;
        List<GemsEmotionsDetails> emotionsMasterList;
        public GemsMoreDetailsPage( List<GemsEmotionsDetails> emotionsList )
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();

 

            emotionsMasterList = emotionsList;


            // PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar = new GemsPageTitleBar(Color.FromRgb(8, 135, 224), "Add Supporting Emotions", Color.White, "", false);



            masterScroll = new ScrollView();
            masterScroll.WidthRequest = App.screenWidth;
            masterScroll.HeightRequest = App.screenHeight * 85 / 100;

            masterStack = new StackLayout();
            masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.Transparent;

            this.Appearing += OnAppearing;


        }

        async void OnAppearing(object sender, EventArgs e)
        {

            int index = 0;
            foreach (var item in emotionsMasterList )
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
                string trimmedFirstDetails = (item.event_details != null && item.event_details.Count > 0) ? item.event_details[index] : "empty";
                if( trimmedFirstDetails != null && trimmedFirstDetails.Length > 55 )
                {
                    trimmedFirstDetails = trimmedFirstDetails.Substring(0, 55);
                    trimmedFirstDetails = trimmedFirstDetails + "....";
                }
                firstDetailsInfo.Text = trimmedFirstDetails;
                firstDetailsInfo.TextColor = Color.Gray;
                firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                firstDetailsInfo.HeightRequest = 45;
                int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(17, 16, 13) : 15;
                firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


                Label firstDateInfo = new Label();
                firstDateInfo.Text = (item.event_datetime != null && item.event_datetime.Count > 0) ? item.event_datetime[index] : "empty";
                //firstDateInfo.Text = "2015 Januvary 30";
                firstDateInfo.TextColor = Color.Black;
                firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                int dateFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(15, 15, 13) : 12;
                firstDateInfo.FontSize = Device.OnPlatform(dateFontSize, dateFontSize, dateFontSize);


                Image firstEmotionsImage = new Image();
                firstEmotionsImage.WidthRequest = App.screenWidth * Device.OnPlatform(25, 25, 20) / 100;
                firstEmotionsImage.HeightRequest = App.screenWidth * Device.OnPlatform(25, 25, 20) / 100;
                firstEmotionsImage.Source = Device.OnPlatform("manali.jpg", "manali.jpg", "//Assets//manali.jpg");
                //firstEmotionsImage.SetBinding(Image.SourceProperty, "FirstImage");



                /*Label secondDetailsInfo = new Label();
                string trimmedSecondDetails = (item.event_details != null && item.event_details.Count > 1) ? item.event_details[1] : "empty";
                if (trimmedSecondDetails != null && trimmedSecondDetails.Length > 55)
                {
                    trimmedSecondDetails = trimmedSecondDetails.Substring(0, 55);
                    trimmedSecondDetails = trimmedSecondDetails + "....";
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
                secondEmotionsImage.Source = Device.OnPlatform("manali.jpg", "manali.jpg", "//Assets//manali.jpg");


                Button moreButton = new Button();
                moreButton.BackgroundColor = Color.Transparent;
                moreButton.BorderColor = Color.Transparent;
                moreButton.BorderWidth = 0;
                moreButton.Text = "more";
                moreButton.FontSize = 15;
                moreButton.MinimumHeightRequest = 20;
                moreButton.TextColor = Color.Silver;
                */


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


           //     if( index == 0)
               // headerLayout.Children.Add(mainTitle);
               // headerLayout.Children.Add(subTitle);


                customLayout.AddChildToLayout(viewContainer, 0, Device.OnPlatform(-5, 0, 0));
                customLayout.AddChildToLayout(firstDetailsInfo, 5, Device.OnPlatform(-3, 2, 2));
                customLayout.AddChildToLayout(firstDateInfo, 5, Device.OnPlatform(4, 9, 5));
                customLayout.AddChildToLayout(firstEmotionsImage, 65, Device.OnPlatform(-5, 0, 0));
                //customLayout.AddChildToLayout(divider, 5, 14);

              /*  customLayout.AddChildToLayout(secondDetailsInfo, 5, Device.OnPlatform(10, 15, 11));
                customLayout.AddChildToLayout(secondDateInfo, 5, Device.OnPlatform(17, 22, 15));
               // customLayout.AddChildToLayout(secondEmotionsImage, 65, Device.OnPlatform(8, 13, 10));
                customLayout.AddChildToLayout(moreButton, 75, Device.OnPlatform(25, 25, 19));*/

                masterStack.Children.Add(headerLayout);
                masterStack.Children.Add(customLayout);
                index++;
                // masterStack.Children.Add( cellMasterLayout );
            }
            index = 0;



          //  masterScroll.Scrolled += OnScroll;
            masterScroll.Content = masterStack;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            //  masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(masterScroll, 0, 10);
            Content = masterLayout;
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
