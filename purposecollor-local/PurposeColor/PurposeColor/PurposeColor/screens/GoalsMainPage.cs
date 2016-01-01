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
            masterScroll.WidthRequest = App.screenWidth;
            masterScroll.HeightRequest = App.screenHeight * 85 / 100;

            masterStack = new StackLayout();
            masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.Transparent;


        }

        async void OnAppearing(object sender, EventArgs e)
        {
            IProgressBar progress = DependencyService.Get<IProgressBar>();
            progress.ShowProgressbar("Loading gems..");

            try
            {
                for (int index = 0; index < 3; index++)
                {
                    StackLayout cellMasterLayout = new StackLayout();
                    cellMasterLayout.Orientation = StackOrientation.Vertical;
                    cellMasterLayout.BackgroundColor = Color.White;


                    StackLayout headerLayout = new StackLayout();
                    headerLayout.Orientation = StackOrientation.Vertical;
                    headerLayout.BackgroundColor = Color.FromRgb(244, 244, 244);

                    CustomLayout customLayout = new CustomLayout();
                    customLayout.BackgroundColor = Color.White;
                    double screenWidth = App.screenWidth;
                    double screenHeight = App.screenHeight;

                    CustomImageButton mainTitle = new CustomImageButton();
                    //  mainTitle.IsEnabled = false;
                    mainTitle.BackgroundColor = Color.FromRgb(30, 126, 210);
                    mainTitle.ImageName = Device.OnPlatform("blue_bg.png", "blue_bg.png", @"/Assets/blue_bg.png");
                    mainTitle.Text = "sample title";
                    mainTitle.TextColor = Color.White;
                    mainTitle.FontSize = Device.OnPlatform(12, 12, 18);
                    mainTitle.WidthRequest = App.screenWidth;
                    mainTitle.TextOrientation = TextOrientation.Middle;
                    headerLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
                    mainTitle.HeightRequest = 80;


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
                    subTitle.HeightRequest = Device.OnPlatform(40, 40, 30);

                    Label firstDetailsInfo = new Label();
                    firstDetailsInfo.Text = "The meaning of life, or the answer to the question What is the meaning of life?, pertains to the significance of living or existence in general. Many other questions also seek the meaning of life, including What should I do?";
                    //firstDetailsInfo.Text = "Referece site about lorem lpsum. Referece site about lorem lpsum. Referece site about lorem lpsum";
                    firstDetailsInfo.TextColor = Color.Gray;
                    firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                    firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                    firstDetailsInfo.HeightRequest = 45;
                    int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(17, 17, 13) : 15;
                    firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);

                    customLayout.WidthRequest = screenWidth;
                    //   customLayout.HeightRequest = 200;//screenHeight * Device.OnPlatform(30, 31, 7) / 100;


                    StackLayout viewContainer = new StackLayout();
                    viewContainer.WidthRequest = App.screenWidth;
                    viewContainer.HeightRequest = 175;//screenHeight * Device.OnPlatform(30, 27, 7) / 100;
                    viewContainer.BackgroundColor = Color.White;


                    headerLayout.Children.Add(mainTitle);
                    headerLayout.Children.Add(subTitle);

                    var items = new List<CheckItem>();
                    items.Add(new CheckItem { Name = "Xamarin.com" });
                    items.Add(new CheckItem { Name = "Twitter" });
                    items.Add(new CheckItem { Name = "Facebook" });
                    items.Add(new CheckItem { Name = "Xamarin.com" });
                    items.Add(new CheckItem { Name = "Twitter" });
                    items.Add(new CheckItem { Name = "Facebook" });
                    items.Add(new CheckItem { Name = "Xamarin.com" });

                    var multiPage = new SelectMultipleBasePage<CheckItem>(items);
                    multiPage.HeightRequest = items.Count * 50;

                    //  customLayout.AddChildToLayout(viewContainer, 0, Device.OnPlatform(-5, 0, 0));
                    customLayout.AddChildToLayout(firstDetailsInfo, 5, Device.OnPlatform(-3, 2, 2));
                    customLayout.AddChildToLayout(multiPage, 5, Device.OnPlatform(-3, 10, 20));

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
