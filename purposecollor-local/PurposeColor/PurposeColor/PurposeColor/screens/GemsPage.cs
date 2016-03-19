using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
   

    public class CommunityGemsCell : ViewCell
    {
        double screenHeight;
        double screenWidth;

        public CommunityGemsCell()
        {
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(230, 255, 254);
            //IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            Image profileImage = new Image();
            profileImage.WidthRequest = screenWidth * 15 / 100;
            profileImage.HeightRequest = screenHeight * 9 / 100;
            profileImage.SetBinding(Image.SourceProperty, "ProfilePhoto");
            profileImage.Aspect = Aspect.Fill;

            Label profileName = new Label();
            profileName.SetBinding( Label.TextProperty, "Name" );
            profileName.TextColor = Color.Blue;

            Label dateInfo = new Label();
            dateInfo.SetBinding( Label.TextProperty, "DateInfo" );
            dateInfo.TextColor = Color.Gray;

            Label gemInfo = new Label();
            gemInfo.TextColor = Color.Black;
            gemInfo.SetBinding(Label.TextProperty, "TruncatedGemInfo");
            gemInfo.WidthRequest = screenWidth * 90 / 100;


            Image gemImage = new Image();
            gemImage.WidthRequest = screenWidth * 90 / 100;
            gemImage.HeightRequest = screenHeight * 25 / 100;
            gemImage.SetBinding( Image.SourceProperty, "GemImage" );
            gemImage.Aspect = Aspect.Fill;

            Button seeMore = new Button();
            seeMore.Text = "see more";
            seeMore.FontSize = Device.OnPlatform(12, 12, 18);
            seeMore.TextColor = Color.Blue;
            seeMore.BackgroundColor = Color.Transparent;
            seeMore.BorderColor = Color.Transparent;
            seeMore.BorderWidth = 0;
            seeMore.SetBinding(Button.ClassIdProperty, "Name");
            seeMore.SetBinding( Button.IsVisibleProperty, "IsSeeMoreVisible" );
            seeMore.Clicked += (object sender, EventArgs e) =>
            {
                Button btn = sender as Button;
                Gems selGem = GemsPage.gemsSource.FirstOrDefault(i => i.Name == btn.ClassId);
                if( selGem != null )
                {
                    App.Navigator.PushModalAsync( new GemsDetailedView( selGem.GemInfo, selGem.GemImage ) );
                }
            };



            masterLayout.HeightRequest = screenHeight * 50 / 100;
            masterLayout.WidthRequest = screenWidth;


            masterLayout.AddChildToLayout(profileImage, 5, 5);
            masterLayout.AddChildToLayout(profileName, 23, 5);
            masterLayout.AddChildToLayout( dateInfo, 23, 8 );
            masterLayout.AddChildToLayout(gemInfo, 5, 16);
            masterLayout.AddChildToLayout( seeMore, 2, Device.OnPlatform( 20, 20 , 22 ) );
            masterLayout.AddChildToLayout( gemImage, 5, 28 );

            this.View = masterLayout;

        }


    }

    public class GemsPage : BasePage, IDisposable
    {
        public static List<Gems> gemsSource = new List<Gems>();
        double screenHeight;
        double screenWidth;
        public GemsPage()
        {
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;

            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(230, 255, 254);

			PurposeColorTitleBar titleBar = new PurposeColorTitleBar(Color.FromRgb(8, 137, 216), "Purpose Color", Color.Black, "back", true);
            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Color.FromRgb(12, 113, 210), "Emotional Awareness");

            gemsSource = new List<Gems>();
            Gems gemItems = new Gems();
            gemItems.ArrowImage = "";
            gemItems.DateInfo = "2015 Janury 30";
            gemItems.GemImage = Device.OnPlatform("manali.jpg", "manali.jpg", "//Assets//manali.jpg");
            gemItems.Name = "Lance Clusner";
            gemItems.ProfilePhoto = Device.OnPlatform("avatar.jpg", "avatar.jpg", "//Assets//avatar.jpg"); 
            gemItems.GemInfo = "This is just a dummy page to check how it displays in mobile devices. below picture is taken from manali. Manali is a high-altitude Himalayan resort town in India’s northern Himachal Pradesh state. It has a reputation as a backpacking center and honeymoon destination. Set on the Beas River, it’s a gateway for skiing in the Solang Valley and trekking in Parvati Valley. It's also a jumping-off point for paragliding, rafting and mountaineering in the Pir Panjal mountains, home to 4,000m-high Rohtang Pass..Manali is a high-altitude Himalayan resort town in India’s northern Himachal Pradesh state. It has a reputation as a backpacking center and honeymoon destination. Set on the Beas River, it’s a gateway for skiing in the Solang Valley and trekking in Parvati Valley. It's also a jumping-off point for paragliding, rafting and mountaineering in the Pir Panjal mountains, home to 4,000m-high Rohtang Pass.Manali is a high-altitude Himalayan resort town in India’s northern Himachal Pradesh state. It has a reputation as a backpacking center and honeymoon destination. Set on the Beas River, it’s a gateway for skiing in the Solang Valley and trekking in Parvati Valley. It's also a jumping-off point for paragliding, rafting and mountaineering in the Pir Panjal mountains, home to 4,000m-high Rohtang Pass..Manali is a high-altitude Himalayan resort town in India’s northern Himachal Pradesh state. It has a reputation as a backpacking center and honeymoon destination. Set on the Beas River, it’s a gateway for skiing in the Solang Valley and trekking in Parvati Valley. It's also a jumping-off point for paragliding, rafting and mountaineering in the Pir Panjal mountains, home to 4,000m-high Rohtang Pass.Manali is a high-altitude Himalayan resort town in India’s northern Himachal Pradesh state. It has a reputation as a backpacking center and honeymoon destination. Set on the Beas River, it’s a gateway for skiing in the Solang Valley and trekking in Parvati Valley. It's also a jumping-off point for paragliding, rafting and mountaineering in the Pir Panjal mountains, home to 4,000m-high Rohtang Pass..Manali is a high-altitude Himalayan resort town in India’s northern Himachal Pradesh state. It has a reputation as a backpacking center and honeymoon destination. Set on the Beas River, it’s a gateway for skiing in the Solang Valley and trekking in Parvati Valley. It's also a jumping-off point for paragliding, rafting and mountaineering in the Pir Panjal mountains, home to 4,000m-high Rohtang Pass..Manali is a high-altitude Himalayan resort town in India’s northern Himachal Pradesh state. It has a reputation as a backpacking center and honeymoon destination. Set on the Beas River, it’s a gateway for skiing in the Solang Valley and trekking in Parvati Valley. It's also a jumping-off point for paragliding, rafting and mountaineering in the Pir Panjal mountains, home to 4,000m-high Rohtang Pass.Manali is a high-altitude Himalayan resort town in India’s northern Himachal Pradesh state. It has a reputation as a backpacking center and honeymoon destination. Set on the Beas River, it’s a gateway for skiing in the Solang Valley and trekking in Parvati Valley. It's also a jumping-off point for paragliding, rafting and mountaineering in the Pir Panjal mountains, home to 4,000m-high Rohtang Pass.Manali is a high-altitude Himalayan resort town in India’s northern Himachal Pradesh state. It has a reputation as a backpacking center and honeymoon destination. Set on the Beas River, it’s a gateway for skiing in the Solang Valley and trekking in Parvati Valley. It's also a jumping-off point for paragliding, rafting and mountaineering in the Pir Panjal mountains, home to 4,000m-high Rohtang Pass.";

            if (gemItems.GemInfo.Length > 100)
            {
                gemItems.TruncatedGemInfo = gemItems.GemInfo.Substring(0, 100);
                gemItems.TruncatedGemInfo = gemItems.TruncatedGemInfo + "........";
            }
  


            Gems gemItem2 = new Gems();
            gemItem2.ArrowImage = "";
            gemItem2.DateInfo = "2015 Janury 30";
            gemItem2.GemImage = Device.OnPlatform("manali.jpg", "manali.jpg", "//Assets//manali.jpg");
            gemItem2.Name = "Virender Sehwag";
            gemItem2.ProfilePhoto = Device.OnPlatform("avatar.jpg", "avatar.jpg", "//Assets//avatar.jpg"); 
            gemItem2.GemInfo = "This is just a dummy page to check how ";

            if (gemItem2.GemInfo.Length > 100)
            {
                gemItem2.TruncatedGemInfo = gemItem2.GemInfo.Substring(0, 100);
                gemItem2.TruncatedGemInfo = gemItem2.TruncatedGemInfo + "........";
            }
            else
            {
                gemItem2.TruncatedGemInfo = gemItem2.GemInfo;
            }


            Gems gemItem3 = new Gems();
            gemItem3.ArrowImage = "";
            gemItem3.DateInfo = "2015 Janury 30";
            gemItem3.GemImage = Device.OnPlatform("manali.jpg", "manali.jpg", "//Assets//manali.jpg");
            gemItem3.Name = "Brian lara";
            gemItem3.ProfilePhoto = Device.OnPlatform("avatar.jpg", "avatar.jpg", "//Assets//avatar.jpg"); 
            gemItem3.GemInfo = "This is just a dummy page to check how This is just a dummy page to check how This is just a dummy page to check how This is just a dummy page to check how This is just a dummy page to check how This is just a dummy page to check how This is just a dummy page to check how ";

            if (gemItem3.GemInfo.Length > 100)
            {
                gemItem3.TruncatedGemInfo = gemItem3.GemInfo.Substring(0, 100);
                gemItem3.TruncatedGemInfo = gemItem3.TruncatedGemInfo + "........";
            }
            else
            {
                gemItem3.TruncatedGemInfo = gemItem3.GemInfo;
            }


            gemsSource.Add(gemItems);
            gemsSource.Add(gemItem2);
            gemsSource.Add(gemItem3);
          /*  gemsSource.Add(new Gems { ArrowImage = "", DateInfo = "2015 Janury 30", GemImage = "manali.jpg", Name = "Lance Clusner", ProfilePhoto = "avatar.jpg", Title = "This is just a dummy page to check how it displays in mobile devices. below picture is taken from manali" });
            gemsSource.Add(new Gems { ArrowImage = "", DateInfo = "2015 Janury 30", GemImage = "manali.jpg", Name = "Lance Clusner", ProfilePhoto = "avatar.jpg", Title = "This is just a dummy page to check how it displays in mobile devices. below picture is taken from manali" });
            gemsSource.Add(new Gems { ArrowImage = "", DateInfo = "2015 Janury 30", GemImage = "manali.jpg", Name = "Lance Clusner", ProfilePhoto = "avatar.jpg", Title = "This is just a dummy page to check how it displays in mobile devices. below picture is taken from manali" });
            gemsSource.Add(new Gems { ArrowImage = "", DateInfo = "2015 Janury 30", GemImage = "manali.jpg", Name = "Lance Clusner", ProfilePhoto = "avatar.jpg", Title = "This is just a dummy page to check how it displays in mobile devices. below picture is taken from manali" });
            gemsSource.Add(new Gems { ArrowImage = "", DateInfo = "2015 Janury 30", GemImage = "manali.jpg", Name = "Lance Clusner", ProfilePhoto = "avatar.jpg", Title = "This is just a dummy page to check how it displays in mobile devices. below picture is taken from manali" });
            gemsSource.Add(new Gems { ArrowImage = "", DateInfo = "2015 Janury 30", GemImage = "manali.jpg", Name = "Lance Clusner", ProfilePhoto = "avatar.jpg", Title = "This is just a dummy page to check how it displays in mobile devices. below picture is taken from manali" });*/

            ListView gemsList = new ListView();
            gemsList.ItemTemplate = new DataTemplate(typeof(CommunityGemsCell));
            gemsList.SeparatorVisibility = SeparatorVisibility.None;
            gemsList.BackgroundColor = Color.FromRgb(230, 255, 254);
            gemsList.ItemsSource = gemsSource;
            gemsList.RowHeight = (int)screenHeight * 50 / 100;
            gemsList.HeightRequest = screenHeight * 75 / 100;
         //   gemsList.HasUnevenRows = true;

            masterLayout.AddChildToLayout(titleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, 10);
            masterLayout.AddChildToLayout( gemsList, 0, 20 ); 
            Content = masterLayout;
        }

        public void Dispose()
        {

        }
    }
}
