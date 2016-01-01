using System;

using Xamarin.Forms;
using CustomControls;
using PurposeColor.interfaces;
using System.Collections;
using PurposeColor.Model;
using System.Collections.Generic;
using PurposeColor.CustomControls;
using System.Diagnostics;

namespace PurposeColor
{
    public class GemsDetailsPage : ContentPage, IDisposable
    {
        CustomLayout masterLayout;
        IProgressBar progressBar;
        StackLayout masterStack;
        ScrollView masterScroll;
        Label title;
        Label description;
        List<EventMedia> mediaList { get; set; }
        List<ActionMedia> actionMediaList { get; set; }

        public GemsDetailsPage(List<EventMedia> mediaArray, List<ActionMedia> actionMediaArray, string pageTitleVal, string titleVal, string desc, string Media, string NoMedia)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            masterScroll = new ScrollView();
            masterScroll.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();
            masterStack = new StackLayout();
            masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.FromRgb(244, 244, 244);
            mediaList = mediaArray;
            actionMediaList = actionMediaArray;

            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Goal Enabling Materials", false);

            Label pageTitle = new Label();
            pageTitle.Text = pageTitleVal;
            pageTitle.TextColor = Color.Black;
            pageTitle.FontAttributes = FontAttributes.Bold;
            pageTitle.WidthRequest = App.screenWidth * 90 / 100;
            pageTitle.HeightRequest = 50;
            pageTitle.XAlign = TextAlignment.Start;
            pageTitle.YAlign = TextAlignment.Center;
            pageTitle.FontSize = Device.OnPlatform(15, 20, 15);


            StackLayout emptyLayout = new StackLayout();
            emptyLayout.BackgroundColor = Color.White;
            emptyLayout.WidthRequest = App.screenWidth * 90 / 100;
            emptyLayout.HeightRequest = 60;

            StackLayout toolsLayout = new StackLayout();
            toolsLayout.BackgroundColor = Color.White;
            toolsLayout.Spacing = 10;
            toolsLayout.Orientation = StackOrientation.Horizontal;
            toolsLayout.WidthRequest = App.screenWidth * 90 / 100;
            toolsLayout.Padding = new Thickness(10, 0, 10, 0);
            toolsLayout.TranslationY = -55;
            //toolsLayout.HeightRequest = 100;

            Image likeButton = new Image();
            likeButton.Source = Device.OnPlatform("like.png", "like.png", "//Assets//like.png");
            likeButton.WidthRequest = Device.OnPlatform(15, 15, 15);
            likeButton.HeightRequest = Device.OnPlatform(15, 15, 15);

            Image shareButton = new Image();
            shareButton.Source = Device.OnPlatform("share.png", "share.png", "//Assets//share.png");
            shareButton.WidthRequest = Device.OnPlatform(15, 15, 15);
            shareButton.HeightRequest = Device.OnPlatform(15, 15, 15);

            CustomEntry comment = new CustomEntry();
            comment.BackGroundImageName = Device.OnPlatform("comnt_box.png", "comnt_box.png", "//Assets//comnt_box.png");
            comment.BackgroundColor = Color.White;
            comment.Placeholder = "comments";
            comment.WidthRequest = App.screenWidth * 70 / 100;

            toolsLayout.Children.Add(likeButton);
            toolsLayout.Children.Add(shareButton);
            toolsLayout.Children.Add(comment);

            title = new Label();
            title.Text = titleVal;
            title.TextColor = Color.Black;
            title.WidthRequest = App.screenWidth * 90 / 100;
            title.FontSize = Device.OnPlatform(12, 12, 12);

            description = new Label();
            description.WidthRequest = App.screenWidth * 90 / 100;
            description.Text = desc;
            description.TextColor = Color.Black;


            masterStack.Children.Add(pageTitle);
            masterStack.Children.Add(title);
            masterStack.Children.Add(description);

            if (mediaList != null)
            {
                for (int index = 0; index < mediaList.Count; index++)
                {

                    bool isValidUrl = (mediaList[index].event_media != null && !string.IsNullOrEmpty(mediaList[index].event_media)) ? true : false;
                    string source = (isValidUrl) ? Constants.SERVICE_BASE_URL + Media + mediaList[index].event_media : comment.BackGroundImageName = Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");
                    Image img = new Image();
                    img.WidthRequest = App.screenWidth * 90 / 100;
                    img.HeightRequest = App.screenWidth * 90 / 100;
                    img.Aspect = Aspect.AspectFill;
                    img.Source = source;
                    img.ClassId = source;
                    var indicator = new ActivityIndicator { Color = new Color(.5), };
                    indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
                    indicator.BindingContext = img;
                    masterStack.Children.Add(indicator);
                    masterStack.Children.Add(img);
                }
            }

            if (actionMediaList != null)
            {
                for (int index = 0; index < actionMediaList.Count; index++)
                {
                    Image img = new Image();
                    bool isValidUrl = (actionMediaList[index].event_media != null && !string.IsNullOrEmpty(actionMediaList[index].event_media)) ? true : false;
                    img.Source = (isValidUrl) ? Constants.SERVICE_BASE_URL + Media + actionMediaList[index].event_media : Device.OnPlatform("noimage.png", "noimage.png", "//Assets//noimage.png");
                    img.Aspect = Aspect.AspectFit;
                    var indicator = new ActivityIndicator { Color = new Color(.5), };
                    indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
                    indicator.BindingContext = img;
                    masterStack.Children.Add(indicator);
                    masterStack.Children.Add(img);
                    masterStack.Children.Add(img);
                }
            }

            masterStack.Children.Add(emptyLayout);
            masterStack.Children.Add(toolsLayout);

            StackLayout spaceOffsetlayout = new StackLayout();
            spaceOffsetlayout.WidthRequest = App.screenWidth * 50 / 100;
            spaceOffsetlayout.HeightRequest = Device.OnPlatform(0, 100, 200);
            spaceOffsetlayout.BackgroundColor = Color.Transparent;
            masterStack.Children.Add(spaceOffsetlayout);


            masterScroll.HeightRequest = App.screenHeight - 10;
            masterScroll.WidthRequest = App.screenWidth * 90 / 100;

            masterScroll.Content = masterStack;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(masterScroll, 5, 18);
            Content = masterLayout;

            /*	Content = new StackLayout
                {
                    BackgroundColor = Color.Red,
                    WidthRequest = 500,
                    HeightRequest = 900
                };*/
        }

		protected override bool OnBackButtonPressed ()
		{
			Dispose ();
			return base.OnBackButtonPressed ();
		}

        public void Dispose()
        {
            masterScroll = null;
            Content = null;
            masterLayout = null;
			progressBar = null;
			masterStack = null;
			masterScroll = null;
			title = null;
			description = null;
			mediaList = null;
			actionMediaList = null;
			this.Content = null;
			GC.Collect ();
        }
    }

}

