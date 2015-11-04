using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using PurposeColor.screens;
using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor
{
    public class App : Application
    {
        public static bool IsLoggedIn { get; set; }
        public static INavigation Navigator { get; set; }
        public static bool IsGoogleLogin { get; set; }
        public static bool IsFacebookLogin { get; set; }
        static string token;
        public static PurposeMasterDetailPage masterPage;
        public static string Token
        {
            get { return token; }
        }
 
        public App()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            MenuPage menuPage = new MenuPage();
            masterPage = new PurposeMasterDetailPage();
            MainPage = masterPage;
        }

        protected override void OnStart()
        {
            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
                CrossPushNotification.Current.Register();
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
            Cross.IDeviceSpec spec = DependencyService.Get<Cross.IDeviceSpec>();
            int titlebarHeight = (int)spec.ScreenHeight * 10 / 100;
            int titlebarWidth = (int)spec.ScreenWidth;

            CustomLayout masteLlayout = new CustomLayout();
            masteLlayout.WidthRequest = (int)spec.ScreenWidth;
            masteLlayout.HeightRequest = (int)spec.ScreenHeight;

            CustomTitleBar titleBar = new CustomTitleBar();

            StackLayout layout = new StackLayout();
            layout.WidthRequest = (int)spec.ScreenWidth; ;
            layout.HeightRequest = (int)spec.ScreenHeight * 90 / 100;
            titleBar.backButton.Clicked += backButton_Clicked;

            masteLlayout.AddChildToLayout(titleBar, 0, 0);
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
