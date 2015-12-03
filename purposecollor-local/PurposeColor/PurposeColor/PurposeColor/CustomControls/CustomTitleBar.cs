using CustomControls;
using SolTech.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.CustomControls
{
    class CustomTitleBar: ContentView
    {
       /* public Color TitleBarBackGroudColor { get; set; }
        public string Title { get; set; }
        public Color TitleColor { get; set; }
        public Color BackButtonTitle { get; set; }*/
        CustomLayout masterLayout;
        public TapGestureRecognizer imageAreaTapGestureRecognizer;
        Label title;

        public CustomTitleBar( Color backGroundColor, string titleValue, Color titleColor, string backButtonTitle )
        {
            //Cross.IDeviceSpec spec = DependencyService.Get<Cross.IDeviceSpec>();
            int titlebarHeight = (int)App.screenHeight * 10 / 100;//(int)spec.ScreenHeight * 10 / 100;
            int titlebarWidth = (int)App.screenWidth;//(int)spec.ScreenWidth;
            this.BackgroundColor = backGroundColor;

            masterLayout = new CustomLayout();
            masterLayout.HeightRequest = titlebarHeight;
            masterLayout.WidthRequest = titlebarWidth;
            masterLayout.BackgroundColor = backGroundColor;

            ImageButton menuButton = new ImageButton();
            menuButton.Source = Device.OnPlatform("menu.png", "menu.png", "//Assets//menu.png");
            menuButton.HeightRequest = 50;
            menuButton.WidthRequest = 50;


            imageAreaTapGestureRecognizer = new TapGestureRecognizer();
            menuButton.GestureRecognizers.Add(imageAreaTapGestureRecognizer);


            title = new Label();
            title.Text = titleValue;
            title.FontSize = 15;
            title.TextColor = Color.Black;

            Image logo = new Image();
            logo.Source = Device.OnPlatform("logo.png", "logo.png", "//Assets//logo.png");
            logo.WidthRequest = App.screenWidth; //spec.ScreenWidth;
            logo.HeightRequest = titlebarHeight;




            masterLayout.AddChildToLayout(logo, 0, 0, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest); 
            masterLayout.AddChildToLayout(menuButton, 2, 10, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);

            Content = masterLayout;

        }
    }
}
