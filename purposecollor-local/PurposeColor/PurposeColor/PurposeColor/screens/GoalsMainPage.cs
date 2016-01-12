using CustomControls;
using Multiselect;
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
    public class GoalsMainPage : ContentPage, IDisposable
    {
        CustomLayout masterLayout;
        IProgressBar progressBar;
        StackLayout listContainer;
        List<GemsPageInfo> gemsList;
        int listViewVislbleIndex;
        GemsPageTitleBar mainTitleBar;
        ScrollView masterScroll;
        StackLayout masterStack;
     //   StackLayout cellContainer;
        public GoalsMainPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();


            this.Appearing += OnAppearing;
            // PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar = new GemsPageTitleBar(Color.FromRgb(8, 135, 224), "Add Supporting Emotions", Color.White, "", false);



            masterScroll = new ScrollView();
            masterScroll.WidthRequest = App.screenWidth - 20;
            masterScroll.HeightRequest = App.screenHeight * 85 / 100;
            masterScroll.BackgroundColor = Color.FromRgb( 244, 244, 244 );

            masterStack = new StackLayout();
            masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.FromRgb(244, 244, 244);
            masterStack.Spacing = 0;
            //masterStack.Padding = new Thickness( 10, 10, 10, 10 );




        }

        async void OnAppearing(object sender, EventArgs e)
        {
            IProgressBar progress = DependencyService.Get<IProgressBar>();
            progress.ShowProgressbar("Loading gems..");

            try
            {
                for (int index = 0; index < 3; index++)
                {

                    StackLayout cellContainer = new StackLayout();
                    cellContainer.Orientation = StackOrientation.Vertical;
                    cellContainer.BackgroundColor = Color.White;// Color.FromRgb(244, 244, 244);
                    cellContainer.Spacing = 0;
                    cellContainer.Padding = new Thickness(10, 10, 10, 10);

                    CustomLayout customLayout = new CustomLayout();
                    customLayout.BackgroundColor = Color.White;
                    customLayout.WidthRequest = App.screenWidth * 90 / 100;
                    double screenWidth = App.screenWidth;
                    double screenHeight = App.screenHeight;


                    Label subTitle = new Label();
                    subTitle.Text = "Pending";
                    subTitle.TextColor = Color.Gray;
                    subTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                    subTitle.XAlign = TextAlignment.Center;
                    int subTitleFontSize = (App.screenDensity > 1.5) ? 18 : 16;
                    subTitle.VerticalOptions = LayoutOptions.Center;
                    subTitle.FontSize = Device.OnPlatform(subTitleFontSize, subTitleFontSize, 22);
                    subTitle.WidthRequest = App.screenWidth * 90 / 100;
                   // headerLayout.HorizontalOptions = LayoutOptions.Center;
                    subTitle.HeightRequest = Device.OnPlatform(40, 40, 30);

                    Label firstDetailsInfo = new Label();
                    string firstDetails = "The meaning of life, or the answer to the question What is the meaning of life?, pertains to the significance of living or existence in general. Many other questions also seek the meaning of life, including What should I do?";
                    if (firstDetails.Length > 130)
                    {
                        firstDetails = firstDetails.Substring(0, 130);
                        firstDetails += "....";
                    }
                    firstDetailsInfo.Text = firstDetails;
                    firstDetailsInfo.TextColor = Color.Gray;
                    firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                    firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                    firstDetailsInfo.HeightRequest = 45;
                    int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(17, 17, 13) : 15;
                    firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);


                    Image goalImage = new Image();
                    goalImage.Source = "goals_badge_blue.png";
                    goalImage.HeightRequest = 100;
                    goalImage.WidthRequest = 30;
                    goalImage.Aspect = Aspect.Fill;


                    Image mediaImage = new Image();
                    mediaImage.Source = "avatar.jpg";
                    mediaImage.HeightRequest = 80;
                    mediaImage.WidthRequest = 80;


                    StackLayout firstRow = new StackLayout();
                    firstRow.Orientation = StackOrientation.Horizontal;
                    firstRow.BackgroundColor = Color.White;
                    firstRow.WidthRequest = App.screenWidth - 20;
                    firstRow.Padding = new Thickness(0, 10, 0, 10);

                    firstRow.Children.Add(goalImage);
                    firstRow.Children.Add(firstDetailsInfo);
                    firstRow.Children.Add(mediaImage);

                    cellContainer.Children.Add(firstRow);



                    for (int pendingIndex = 0; pendingIndex < 5; pendingIndex++)
                    {
                        Image bgImage = new Image();
                        bgImage.Source = "select_box_whitebg.png";
                        bgImage.WidthRequest = App.screenWidth - 40;
                        bgImage.HeightRequest = 50;

                        Label pendingGoalTitle = new Label();
                        pendingGoalTitle.TextColor = Color.Black;
                        pendingGoalTitle.XAlign = TextAlignment.Center;
                        pendingGoalTitle.FontSize = 15;
                        pendingGoalTitle.HeightRequest = Device.OnPlatform(15, 25, 25);
                        pendingGoalTitle.Text = "Go to gym";

                        Switch goalDoneSwitch = new Switch();
                        goalDoneSwitch.BackgroundColor = Color.Transparent;
                        goalDoneSwitch.VerticalOptions = LayoutOptions.Center;

                        CustomLayout pendingRow = new CustomLayout();
                        pendingRow.WidthRequest = App.screenWidth * 90 / 100;
                        pendingRow.HeightRequest = 50;
                        pendingRow.AddChildToLayout(bgImage, 0, 0, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
                        pendingRow.AddChildToLayout(goalDoneSwitch, 5, 20, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
                        pendingRow.AddChildToLayout(pendingGoalTitle, 20, 25, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
                        cellContainer.Children.Add(pendingRow);
                    }

                    masterStack.Children.Add(cellContainer);

                    StackLayout trans = new StackLayout();
                    trans.BackgroundColor = Color.FromRgb(244, 244, 244);
                    trans.HeightRequest = 30;
                    trans.WidthRequest = App.screenWidth;
                    masterStack.Children.Add(trans);
                }

                masterScroll.Padding = new Thickness(10, 10, 10, 0);
                masterScroll.Scrolled += OnScroll;
                masterScroll.Content = masterStack;

                masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
                //  masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
                masterLayout.AddChildToLayout(masterScroll, 0, 10);
                Content = masterLayout;


            }
            catch (Exception)
            {
                
            }
            progress.HideProgressbar();
        }

        void OnScroll(object sender, ScrolledEventArgs e)
        {
            try
            {

                System.Diagnostics.Debug.WriteLine("Scroll pos : " + masterScroll.ScrollY.ToString());

                if (masterScroll.ScrollY > 1 && masterScroll.ScrollY < 307)
                {
                    if (mainTitleBar.title.Text != "0 th Menu")
                        mainTitleBar.title.Text = "0 th Menu";
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

            }
            catch (Exception)
            {
            }
        }

        void NextButtonTapRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        public void Dispose()
        {

        }

        public void ScrollVisibleItems(int visbleIndex)
        {
            try
            {

                if (visbleIndex >= 0 && visbleIndex < gemsList.Count && listViewVislbleIndex != visbleIndex)
                {
                    GemsPageInfo gems = gemsList[visbleIndex];

                    /*if( gems.IsMainTitleVisible )
                    {
                    
                    }*/
                    if (visbleIndex < 2)
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
            catch (Exception)
            {
            }
        }
    }


  


    public class GoalsPageInfo
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
