using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace PurposeColor.screens
{
    public class AddEventsSituationsOrThoughts : ContentPage, System.IDisposable
    {
        #region MEMBERS

        CustomLayout masterLayout;
        StackLayout TopTitleBar;
        PurposeColorSubTitleBar subTitleBar;
        Editor textInput;
        StackLayout textInputContainer;
        StackLayout audioInputStack;
        Image cameraInput;
        Image audioInput;
        StackLayout cameraInputStack;
        Image galleryInput;
        StackLayout galleryInputStack;
        Image locationInput;
        StackLayout locationInputStack;
        Image contactInput;
        StackLayout contactInputStack;
        StackLayout iconContainer;
        StackLayout textinputAndIconsHolder;
        TapGestureRecognizer CameraTapRecognizer;
        
        #endregion
                
        public AddEventsSituationsOrThoughts(string title)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;

            #region TITLE BARS
            TopTitleBar = new StackLayout
            {
                BackgroundColor = Constants.BLUE_BG_COLOR,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = 0,
                Spacing = 0,
                Children = { new BoxView { WidthRequest = deviceSpec.ScreenWidth } }
            };
            masterLayout.AddChildToLayout(TopTitleBar, 0, 0);

            string pageTitle = string.Empty;
            if (title.Length > 20)
            {
                pageTitle = title.Substring(0, 20);
                pageTitle += "...";
            }

            subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, pageTitle, true, true);
            masterLayout.AddChildToLayout(subTitleBar, 0, 1);
            subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;
            subTitleBar.NextButtonTapRecognizer.Tapped += NextButtonTapRecognizer_Tapped;
            #endregion

            #region TEXT INPUT CONTROL

            textInput = new Editor
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 200,
                Text = title
            };
            textInputContainer = new StackLayout
            {
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Padding = 2,
                Children = { textInput }
            };

            int devWidth = (int)deviceSpec.ScreenWidth;
            int textInputWidth = (int)(devWidth * .8); // 80% of screen
            textInput.WidthRequest = textInputWidth;

            int textInputX = (devWidth - textInputWidth) / 2;

            #endregion

            #region ICONS

            audioInput = new Image()
            {
                Source = Device.OnPlatform("ic_music.png", "ic_music.png", "//Assets//ic_music.png"),
                Aspect = Aspect.AspectFit
            };
            audioInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { audioInput, new Label { Text = "Audio", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };

            cameraInput = new Image()
            {
                Source = Device.OnPlatform("icn_camera.png", "icn_camera.png", "//Assets//icn_camera.png"),
                Aspect = Aspect.AspectFit
            };
            cameraInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { cameraInput, new Label { Text = "Camera", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };

            CameraTapRecognizer = new TapGestureRecognizer();
            cameraInputStack.GestureRecognizers.Add(CameraTapRecognizer);
            CameraTapRecognizer.Tapped += (s, e) =>
                {
                    if (Media.Plugin.CrossMedia.Current.IsCameraAvailable)
                    {
                        
                    }

                };

            TapGestureRecognizer emptyAreaTapGestureRecognizer = new TapGestureRecognizer();
            emptyAreaTapGestureRecognizer.Tapped += (s, e) =>
            {
               //to do..

            };
            cameraInputStack.GestureRecognizers.Add(emptyAreaTapGestureRecognizer);


            galleryInput = new Image()
            {
                Source = Device.OnPlatform("icn_gallery.png", "icn_gallery.png", "//Assets//icn_gallery.png"),
                Aspect = Aspect.AspectFit
            };
            galleryInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { galleryInput, new Label { Text = "Gallery", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };
            locationInput = new Image()
            {
                Source = Device.OnPlatform("icn_location.png", "icn_location.png", "//Assets//icn_location.png"),
                Aspect = Aspect.AspectFit
            };
            locationInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { locationInput, new Label { Text = "Location", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };
            
            contactInput = new Image()
            {
                Source = Device.OnPlatform("icn_contact.png", "icn_contact.png", "//Assets//icn_contact.png"),
                Aspect = Aspect.AspectFit
            };
            
            contactInputStack = new StackLayout
            {
                Padding = 10,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                Spacing = 0,
                Children = { contactInput, new Label { Text = "Contact", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
            };

            #endregion

            #region CONTAINERS

            iconContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = (int)(devWidth * .8) + 4, /// 4 pxl padding added to text input.
                Padding = 0,
                Children = { galleryInputStack, cameraInputStack, audioInputStack, locationInputStack, contactInputStack }
            };
            
            textinputAndIconsHolder = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0,
                Padding = 0,
                Children = { textInputContainer, iconContainer }
            };

            int iconY = (int)textInput.Y + (int)textInput.Height + 5;
            masterLayout.AddChildToLayout(textinputAndIconsHolder, 10, 10);
            
            #endregion

            Content = masterLayout;
        }

        void CameraTapRecognizer_Tapped(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        void NextButtonTapRecognizer_Tapped(object sender, System.EventArgs e)
        {
            string input = (sender as Editor).Text;
            // to do.. according to the page title, save to the corresponding local db

            Navigation.PopAsync();
        }

        void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        public void Dispose()
        {
            subTitleBar.BackButtonTapRecognizer.Tapped -= OnBackButtonTapRecognizerTapped;
            subTitleBar.NextButtonTapRecognizer.Tapped -= NextButtonTapRecognizer_Tapped;
            this.masterLayout = null;
            this.TopTitleBar = null;
            this.subTitleBar = null;
            this.textInput = null;
            this.textInputContainer = null;
            this.audioInputStack = null;
            this.cameraInput = null;
            this.audioInput = null;
            this.cameraInputStack = null;
            this.galleryInput = null;
            this.galleryInputStack = null;
            this.locationInput = null;
            this.locationInputStack = null;
            this.contactInput = null;
            this.contactInputStack = null;
            this.iconContainer = null;
            this.textinputAndIconsHolder = null;
        }
    }
}
