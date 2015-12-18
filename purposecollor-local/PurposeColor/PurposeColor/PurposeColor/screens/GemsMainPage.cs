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
    public class GemsMainPage : ContentPage, IDisposable
    {
        CustomLayout masterLayout;
        IProgressBar progressBar;
        StackLayout listContainer;
        List<GemsPageInfo> gemsList;
        int listViewVislbleIndex;
        GemsPageTitleBar mainTitleBar;
        ScrollView masterScroll;
        public GemsMainPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();

           // PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar = new GemsPageTitleBar(Color.FromRgb(8, 135, 224), "Add Supporting Emotions", Color.White, "", false);
           
            listContainer = new StackLayout();
            listContainer.WidthRequest = App.screenWidth;

            GemsPageInfo gemsInfo1 = new GemsPageInfo();
            gemsInfo1.SubTitle = "This is sub Title";
            gemsInfo1.FirstDateInfo = "2015 jan 30";
            gemsInfo1.FirstDetailsInfo = "This is first details info. am i right ?";
            gemsInfo1.SecondDateInfo = "2015 ottober 31";
            gemsInfo1.SecondDetailsInfo = "i believe in omens and that should act on time";
            gemsInfo1.SecondImage = "manali.jpg";
            gemsInfo1.FirstImage = "manali.jpg";
            gemsInfo1.MainTitle = "Goals and Dreams";
            gemsInfo1.IsMainTitleVisible = false;


            GemsPageInfo gemsInfo2 = new GemsPageInfo();
            gemsInfo2.SubTitle = "Mixed";
            gemsInfo2.FirstDateInfo = "2014 December  25";
            gemsInfo2.FirstDetailsInfo = "travelled more than 55 km in side seat";
            gemsInfo2.SecondDateInfo = "2014 December 26";
            gemsInfo2.SecondDetailsInfo = "It was a merry xmas this year";
            gemsInfo2.SecondImage = "manali.jpg";
            gemsInfo2.FirstImage = "manali.jpg";
            gemsInfo2.IsMainTitleVisible = false;


            GemsPageInfo gemsInfo3 = new GemsPageInfo();
            gemsInfo3.SubTitle = "Emotion 3";
            gemsInfo3.FirstDateInfo = "2015 october 31";
            gemsInfo3.FirstDetailsInfo = "had a happy moment";
            gemsInfo3.SecondDateInfo = "2015 April 19";
            gemsInfo3.SecondDetailsInfo = "First and last experiement";
            gemsInfo3.SecondImage = "manali.jpg";
            gemsInfo3.FirstImage = "manali.jpg";
            gemsInfo3.IsMainTitleVisible = false;


            GemsPageInfo gemsGoals = new GemsPageInfo();
            gemsGoals.SubTitle = "Emotion 3";
            gemsGoals.FirstDateInfo = "2015 october 31";
            gemsGoals.FirstDetailsInfo = "had a happy moment";
            gemsGoals.SecondDateInfo = "2015 April 19";
            gemsGoals.SecondDetailsInfo = "First and last experiement";
            gemsGoals.SecondImage = "manali.jpg";
            gemsGoals.FirstImage = "manali.jpg";
            gemsGoals.MainTitle = "My Goals and Dreams";
            gemsGoals.IsMainTitleVisible = true;


            gemsList = new List<GemsPageInfo>();
            gemsList.Add( gemsInfo1 );
            gemsList.Add(gemsInfo2);
            gemsList.Add(gemsGoals);
            gemsList.Add(gemsInfo2);
            gemsList.Add(gemsInfo3);
            //gemsList.Add(gemsInfo);


          /*  GemsListView mainListView = new GemsListView();
            mainListView.BackgroundColor = Constants.LIST_BG_COLOR;
            mainListView.ItemsSource = gemsList;
            mainListView.ItemTemplate = new DataTemplate(typeof(GemsListCellTemplate));
            //mainListView.RowHeight =(int) App.screenHeight * 40 / 100;
            mainListView.HeightRequest = App.screenHeight;
            mainListView.HasUnevenRows = true;
            mainListView.Scroll = ScrollVisibleItems;
            listContainer.Children.Add( mainListView );*/


            masterScroll = new ScrollView();
            masterScroll.WidthRequest = App.screenWidth;
            masterScroll.HeightRequest = App.screenHeight * 85 / 100;

            StackLayout masterStack = new StackLayout();
            masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.Transparent;

            for( int index = 0; index < 4; index++ )
            {
                StackLayout cellMasterLayout = new StackLayout();
                cellMasterLayout.Orientation = StackOrientation.Vertical;
                cellMasterLayout.BackgroundColor = Color.White;


                StackLayout headerLayout = new StackLayout();
                headerLayout.Orientation = StackOrientation.Vertical;
				headerLayout.BackgroundColor = Color.Red;// Color.FromRgb(244, 244, 244);

                CustomLayout customLayout = new CustomLayout();
                customLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
                double screenWidth = App.screenWidth;
                double screenHeight = App.screenHeight;

                CustomImageButton mainTitle = new CustomImageButton();
                //  mainTitle.IsEnabled = false;
                mainTitle.BackgroundColor = Color.FromRgb(30, 126, 210);
                mainTitle.ImageName = Device.OnPlatform("top_bg.png", "light_blue_bg.png", "//Assets//top_bg.png");
                mainTitle.Text = "Main Title  -- " + index.ToString();
                mainTitle.TextColor = Color.White;
                mainTitle.FontSize = 12;
                mainTitle.WidthRequest = App.screenWidth;
                mainTitle.TextOrientation = TextOrientation.Middle;
                headerLayout.VerticalOptions = LayoutOptions.Center;


                Label subTitle = new Label();
                subTitle.Text = "Sub Title";
                subTitle.TextColor = Color.Gray;
                subTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                subTitle.XAlign = TextAlignment.Center;
                int subTitleFontSize = (App.screenDensity > 1.5) ? 18 : 16;
                subTitle.VerticalOptions = LayoutOptions.Center;
                subTitle.FontSize = Device.OnPlatform(subTitleFontSize, subTitleFontSize, 22);
                subTitle.WidthRequest = App.screenWidth * 90 / 100;
				headerLayout.HorizontalOptions = LayoutOptions.Center;
				subTitle.HeightRequest = Device.OnPlatform( 40, 40, 30 );

                Label firstDetailsInfo = new Label();
                firstDetailsInfo.Text = "First Details Info";
                //firstDetailsInfo.Text = "Referece site about lorem lpsum. Referece site about lorem lpsum. Referece site about lorem lpsum";
                firstDetailsInfo.TextColor = Color.Gray;
                firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                firstDetailsInfo.HeightRequest = 45;
                int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? 17 : 15;
                firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, 22);


                Label firstDateInfo = new Label();
                firstDateInfo.Text = "First Date Info";
                //firstDateInfo.Text = "2015 Januvary 30";
                firstDateInfo.TextColor = Color.Black;
                firstDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                int detailsFontSize = (App.screenDensity > 1.5) ? 15 : 12;
                firstDateInfo.FontSize = Device.OnPlatform(detailsFontSize, detailsFontSize, 22);


                Image firstEmotionsImage = new Image();
                firstEmotionsImage.WidthRequest = App.screenWidth * 25 / 100;
                firstEmotionsImage.HeightRequest = App.screenWidth * 25 / 100;
                firstEmotionsImage.Source =  Device.OnPlatform("manali.jpg","manali.jpg" , "//Assets//manali.jpg");
                //firstEmotionsImage.SetBinding(Image.SourceProperty, "FirstImage");



                Label secondDetailsInfo = new Label();
                secondDetailsInfo.Text = "second details info";
                // secondDetailsInfo.Text = "Referece site about lorem lpsum. Referece site about lorem lpsum. Referece site about lorem lpsum";
                secondDetailsInfo.TextColor = Color.Gray;
                secondDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                secondDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                secondDetailsInfo.HeightRequest = 45;
                secondDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, 22);


                Label secondDateInfo = new Label();
                secondDateInfo.Text = "second date info";
                secondDateInfo.TextColor = Color.Black;
                secondDateInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                firstDateInfo.FontSize = Device.OnPlatform(detailsFontSize, detailsFontSize, 22);


                Image secondEmotionsImage = new Image();
                secondEmotionsImage.WidthRequest = App.screenWidth * 25 / 100;
                secondEmotionsImage.HeightRequest = App.screenWidth * 25 / 100;
                secondEmotionsImage.Source = Device.OnPlatform("manali.jpg", "manali.jpg", "//Assets//manali.jpg");


                Button moreButton = new Button();
                moreButton.BackgroundColor = Color.Transparent;
                moreButton.BorderColor = Color.Transparent;
                moreButton.BorderWidth = 0;
                moreButton.Text = "more";
                moreButton.FontSize = 15;
                moreButton.MinimumHeightRequest = 20;
                moreButton.TextColor = Color.Silver;

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

                headerLayout.Children.Add(mainTitle);
                headerLayout.Children.Add(subTitle);


				customLayout.AddChildToLayout(viewContainer, 0, Device.OnPlatform( -5, 0, 0 ));
				customLayout.AddChildToLayout(firstDetailsInfo, 5, Device.OnPlatform( -3 ,2 ,2 ));
				customLayout.AddChildToLayout(firstDateInfo, 5, Device.OnPlatform( 4, 9, 9 ));
				customLayout.AddChildToLayout(firstEmotionsImage, 65, Device.OnPlatform( -5, 0, 0 ));
                customLayout.AddChildToLayout(divider, 5, 14);

				customLayout.AddChildToLayout(secondDetailsInfo, 5, Device.OnPlatform( 10, 15, 15 ));
				customLayout.AddChildToLayout(secondDateInfo, 5, Device.OnPlatform( 17, 22, 22 ));
				customLayout.AddChildToLayout(secondEmotionsImage, 65, Device.OnPlatform( 8, 13, 13 ));
				customLayout.AddChildToLayout(moreButton, 75, Device.OnPlatform( 25, 25, 25 ));

                masterStack.Children.Add(headerLayout);
                masterStack.Children.Add(customLayout);

               // masterStack.Children.Add( cellMasterLayout );
            }




            masterScroll.Scrolled += OnScroll;
            masterScroll.Content = masterStack;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
          //  masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(masterScroll, 0, 10);
            Content = masterLayout;
        }

        void OnScroll(object sender, ScrolledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Scroll pos : " + masterScroll.ScrollY.ToString());

            if( masterScroll.ScrollY > 1 && masterScroll.ScrollY < 307 )
            {
                if( mainTitleBar.title.Text != "0 th Menu" )
                mainTitleBar.title.Text = "0 th Menu";
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
