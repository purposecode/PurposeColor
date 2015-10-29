using CustomControls;
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
        public Color TitleBarBackGroudColor { get; set; }
        public Color Title { get; set; }
        public Color TitleColor { get; set; }
        public Color BackButtonTitle { get; set; }
        public Button backButton;

        public CustomTitleBar()
        {
            Cross.IDeviceSpec spec = DependencyService.Get<Cross.IDeviceSpec>();
            int titlebarHeight = (int)spec.ScreenHeight * 10 / 100;
            int titlebarWidth = (int)spec.ScreenWidth;

            CustomLayout masterLayout = new CustomLayout();
            masterLayout.HeightRequest = titlebarHeight;
            masterLayout.WidthRequest = titlebarWidth;
            masterLayout.BackgroundColor = Color.Gray;

            backButton = new Button();
            backButton.Text = "<";
            backButton.TextColor = Color.Black;
            backButton.FontSize = 25;
            backButton.BackgroundColor = Color.Transparent;


            Label title = new Label();
            title.Text = "Screen Title";
            title.FontSize = 22;
            title.TextColor = Color.Black;


            masterLayout.AddChildToLayout( backButton, 2, 10,(int) masterLayout.WidthRequest, (int)masterLayout.HeightRequest );
            masterLayout.AddChildToLayout(title, 25, 25, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);

            Content = masterLayout;

        }
    }
}
