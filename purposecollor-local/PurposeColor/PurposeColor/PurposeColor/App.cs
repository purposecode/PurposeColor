﻿using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Database;
using PurposeColor.Model;
using PurposeColor.screens;
using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor
{
    public class App : Application
    {
        public static bool IsLoggedIn { get; set; }
		public static string SelectedVideoPath { get; set; }
        public static INavigation Navigator { get; set; }
        public static bool IsGoogleLogin { get; set; }
        public static bool IsFacebookLogin { get; set; }
        public static string SelectedEmotion { get; set; }
        public static PurposeMasterDetailPage masterPage;
        static string token;
		public static double Lattitude;
		public static double Longitude;
        public static string SelectedActionStartDate { get; set; }
        public static string SelectedActionEndDate { get; set; }
        static ApplicationSettings applicationSettings;
        public static List<CustomListViewItem> goalsListSource;
        public static List<CustomListViewItem> actionsListSource;
        public static List<CustomListViewItem> eventsListSource;
        public static List<CustomListViewItem> emotionsListSource;
        public static List<CustomListViewItem> nearByLocationsSource;
        public static List<MediaItem> MediaArray { get; set; }
        public static List<string> ExtentionArray { get; set; }
        public static List<string> ContactsArray { get; set; }
        public static ObservableCollection<PreviewItem> PreviewListSource = new ObservableCollection<PreviewItem>();
        public static string newEmotionId;
        public static ApplicationSettings Settings
        {
            get
            {
                return applicationSettings;
            }
        }

        public static string Token
        {
            get { return token; }
        }


        IDeviceSpec deviceSpec;
        public static double screenHeight;
        public static double screenWidth;
        public static double screenDensity;
		public static CreateEventPage CalPage{ get; set; }

        public App()
        {
			
            deviceSpec = DependencyService.Get<IDeviceSpec>();
            screenHeight = deviceSpec.ScreenHeight;
            screenWidth = deviceSpec.ScreenWidth;
            screenDensity = deviceSpec.ScreenDensity;
            MediaArray = new List<MediaItem>();
            ContactsArray = new List<string>();
            ExtentionArray = new List<string>();
            NavigationPage.SetHasNavigationBar(this, false);
            nearByLocationsSource = new List<CustomListViewItem>();
            MenuPage menuPage = new MenuPage();
            masterPage = new PurposeMasterDetailPage();
            MainPage = masterPage;

        }

        protected override void OnStart()
        {
            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {
                CrossPushNotification.Current.Register();
            }

            if (applicationSettings == null)
            {
                applicationSettings = new ApplicationSettings();
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static void NavigateToChangePassword(User userInfo)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigator.PushAsync(new ChangePassword(userInfo));
            });
        }

        public static void SaveToken(string tkn)
        {
            token = tkn;
        }


        public static List<CustomListViewItem> GetEmotionsList()
        {
            if (emotionsListSource != null && emotionsListSource.Count > 0)
                return emotionsListSource;
          /*  eventsListSource.Add(new CustomListViewItem { Name = "married" });
            eventsListSource.Add(new CustomListViewItem { Name = "divorsed" });
            eventsListSource.Add(new CustomListViewItem { Name = "got promotion" });
            eventsListSource.Add(new CustomListViewItem { Name = "got a trip" });
            eventsListSource.Add(new CustomListViewItem { Name = "bought a car" });*/
            return emotionsListSource;
        }



        public static List<CustomListViewItem> GetEventsList()
        {
            if (eventsListSource != null && eventsListSource.Count > 0)
                return eventsListSource;

            //eventsListSource = new List<CustomListViewItem>();
            //eventsListSource.Add(new CustomListViewItem { Name = "Lost Job" });
            //eventsListSource.Add(new CustomListViewItem { Name = "married" });
            //eventsListSource.Add(new CustomListViewItem { Name = "divorsed" });
            //eventsListSource.Add(new CustomListViewItem { Name = "got promotion" });
            //eventsListSource.Add(new CustomListViewItem { Name = "got a trip" });
            //eventsListSource.Add(new CustomListViewItem { Name = "bought a car" });
            return eventsListSource;
        }


        public static  List<CustomListViewItem> GetActionsList()
        {
            if (actionsListSource != null && actionsListSource.Count > 0)
                return actionsListSource;

            return actionsListSource;
        }


        public static List<CustomListViewItem> GetGoalsList()
        {
            if (goalsListSource != null && goalsListSource.Count > 0)
                return goalsListSource;

         /*   goalsListSource = new List<CustomListViewItem>();
            goalsListSource.Add(new CustomListViewItem { Name = "Loose Weight" });
            goalsListSource.Add(new CustomListViewItem { Name = "Learn Yoga" });
            goalsListSource.Add(new CustomListViewItem { Name = "To be Rich" });
            goalsListSource.Add(new CustomListViewItem { Name = "Peace Of Mind" });
            goalsListSource.Add(new CustomListViewItem { Name = "Buy a New Car" });
            goalsListSource.Add(new CustomListViewItem { Name = "Visit Everest" });
            goalsListSource.Add(new CustomListViewItem { Name = "Meet Prime Minister" });*/
            return goalsListSource;
        }

        public static Action SuccessfulLoginAction
        {
            get
            {
                NavigationPage navPage = new NavigationPage(new FeelingNowPage());
                return new Action(() =>
                {
                    try
                    {
                        Navigator.PushModalAsync(navPage);
                        //Navigator.PushAsync(new FeelingNowPage());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("SuccessfulLoginAction : " + ex.Message);
                    }
                });
            }
        }
    }


    public class MyMasterDetailPage : MasterDetailPage
    {
        public MyMasterDetailPage()
        {
            App.Navigator = Navigation;
            NavigationPage.SetHasNavigationBar(this, false);
            MasterPage page = new MasterPage();
            Master = page;
            Detail = new NavigationPage(new LogInPage());
            this.Title = "test";
        }
    }
    
    public class MasterPage : ContentPage
    {
        public MasterPage()
        {
            Icon = "icon.png";
            Title = "Menu";
            //  BackgroundColor = Color.FromHex("444444");
            BackgroundColor = Color.Red;
            Content = new Label
            {
                Text = "master"
            };
        }
    }

    public class DetailsPage : ContentPage
    {
        MyMasterDetailPage masterPage;
        public DetailsPage(MyMasterDetailPage master)
        {
            masterPage = master;

            //  Title = "details";
            int titlebarHeight = (int)App.screenHeight * 10 / 100;
            int titlebarWidth = (int)App.screenWidth;

            CustomLayout masteLlayout = new CustomLayout();
            masteLlayout.WidthRequest = (int)App.screenWidth;
            masteLlayout.HeightRequest = (int)App.screenHeight;

    //        CustomTitleBar titleBar = new CustomTitleBar();

            StackLayout layout = new StackLayout();
            layout.WidthRequest = (int)App.screenWidth;
            layout.HeightRequest = (int)App.screenHeight * 90 / 100;
        
            masteLlayout.AddChildToLayout(layout, 0, 10);

            NavigationPage.SetHasNavigationBar(this, false);
            masteLlayout.BackgroundColor = Color.White;
            Content = masteLlayout;
        }

        void backButton_Clicked(object sender, EventArgs e)
        {
            masterPage.IsPresented = true;
        }
    }
}
