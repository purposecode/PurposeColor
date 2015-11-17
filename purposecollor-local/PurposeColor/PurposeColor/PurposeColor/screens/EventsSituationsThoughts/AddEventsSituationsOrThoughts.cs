using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace PurposeColor.screens
{
    public class AddEventsSituationsOrThoughts : ContentPage
    {
        public AddEventsSituationsOrThoughts(string title)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;

            StackLayout TopTitleBar = new StackLayout
            {
                BackgroundColor = Constants.BLUE_BG_COLOR,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = 0,
                Spacing = 0,
                Children = { new BoxView { WidthRequest = deviceSpec.ScreenWidth } }
            };
            masterLayout.AddChildToLayout(TopTitleBar, 0, 0);

            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, title,true,true);
            masterLayout.AddChildToLayout(subTitleBar, 0, 1);

            Editor textInput = new Editor
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 200,
                Text = title
            };

            int devWidth = (int)deviceSpec.ScreenWidth;
            int textInputWidth = (int)(devWidth * .8); // 80% of screen
            textInput.WidthRequest = textInputWidth;

            int textInputX = (devWidth - textInputWidth) / 2;
            masterLayout.AddChildToLayout(textInput, 10, 10);

            #region ICONS

            Image audioInput = new Image()
            {
                Source = Device.OnPlatform("ic_music.png", "ic_music.png", "//Assets//ic_music.png"),
                Aspect = Aspect.AspectFit
            };
            StackLayout audioInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { audioInput, new Label { Text = "Audio", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };

            Image cameraInput = new Image()
            {
                Source = Device.OnPlatform("icn_camera.png", "icn_camera.png", "//Assets//icn_camera.png"),
                Aspect = Aspect.AspectFit
            };
            StackLayout cameraInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { cameraInput, new Label { Text = "Camera", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };

            Image galleryInput = new Image()
            {
                Source = Device.OnPlatform("icn_gallery.png", "icn_gallery.png", "//Assets//icn_gallery.png"),
                Aspect = Aspect.AspectFit
            };
            StackLayout galleryInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { galleryInput, new Label { Text = "Gallery", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };
            Image locationInput = new Image()
            {
                Source = Device.OnPlatform("icn_location.png", "icn_location.png", "//Assets//icn_location.png"),
                Aspect = Aspect.AspectFit
            };
            StackLayout locationInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { locationInput, new Label { Text = "Location", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };

            Image contactInput = new Image()
            {
                Source = Device.OnPlatform("icn_contact.png", "icn_contact.png", "//Assets//icn_contact.png"),
                Aspect = Aspect.AspectFit
            };
            StackLayout contactInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { contactInput, new Label { Text = "Contact", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };
            
            #endregion

            StackLayout iconContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = textInput.Width,
                Padding = 0,
                Children = { galleryInputStack, cameraInputStack, audioInputStack, locationInputStack, contactInputStack }
            };

            int iconY = (int)textInput.Y + (int)textInput.Height + 5;
            masterLayout.AddChildToLayout(iconContainer, 10, 44);
            Content = masterLayout;
        }
    }
}
