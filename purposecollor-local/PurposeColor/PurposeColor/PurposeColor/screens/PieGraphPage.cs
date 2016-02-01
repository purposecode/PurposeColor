using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using Xamarin.Forms;
using PurposeColor.interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PurposeColor.screens
{
    public class PieGraphPage : ContentPage
    {
        private OxyPlotView plotView;
        CustomLayout masterLayout = null;

        public CustomeGraphModel graphModel { get; set; }
        double screenHeight;
        double screenWidth;
        IProgressBar progressBar = null;
        PurposeColorSubTitleBar subTitleBar = null;
        PurposeColorTitleBar mainTitleBar = null;

        Image region1DownArrow;
        Image region2DownArrow;
        Image region3DownArrow;
        Image region4DownArrow;
        StackLayout region1ButtonStack;
        StackLayout region2ButtonStack;
        StackLayout region3ButtonStack;
        StackLayout region4ButtonStack;

        List<EmotionValues> Emotions;
        int currentSubMenuDisplaying = 0;
        
        public PieGraphPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            progressBar = DependencyService.Get<IProgressBar>();

            mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
            subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Intellegence", false);
            subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));

            Label headingLabel = new Label
            {
                Text = "My Emotional Zone",
                FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
                TextColor = Color.FromRgb(40, 47, 50),
                FontSize = Device.OnPlatform(20, 20, 30)

            };
            masterLayout.AddChildToLayout(headingLabel, 10, 20);

            #region OXYPLOT VIEW

            plotView = new OxyPlotView();
            plotView.HeightRequest = App.screenWidth * .60;//300;
            plotView.WidthRequest = App.screenWidth * .60; // H & W same to keep it square. //300;
            plotView.BackgroundColor = Color.Transparent;

            graphModel = new CustomeGraphModel();
            plotView.Model = graphModel.plotModel;
            plotView.VerticalOptions = LayoutOptions.StartAndExpand;
            plotView.HorizontalOptions = LayoutOptions.StartAndExpand;
            #endregion

            masterLayout.AddChildToLayout(plotView, 20, 27);

            #region List views


            /////////////////////// for testing only ////////////////
            Emotions = new List<EmotionValues>
            {
                new EmotionValues{count = 2, emotion_id = 1, emotion_title="Happy",emotion_value="1"},
                new EmotionValues{count = 3, emotion_id = 1, emotion_title="Excited",emotion_value="1"},
                new EmotionValues{count = 5, emotion_id = 1, emotion_title="Satisfaction",emotion_value="1"},
                new EmotionValues{count = 2, emotion_id = 1, emotion_title="Amusement",emotion_value="1"},


                new EmotionValues{count = 4, emotion_id = 2, emotion_title="Creativity",emotion_value="2"},
                new EmotionValues{count = 4, emotion_id = 2, emotion_title="Great",emotion_value="2"},
                new EmotionValues{count = 4, emotion_id = 2, emotion_title="Trust",emotion_value="2"},
                new EmotionValues{count = 4, emotion_id = 2, emotion_title="Motivated",emotion_value="2"},

                new EmotionValues{count = 1, emotion_id = 3, emotion_title="Fedup",emotion_value="-1"},
                new EmotionValues{count = 1, emotion_id = 3, emotion_title="Worried",emotion_value="-1"},
                new EmotionValues{count = 1, emotion_id = 3, emotion_title="Disturbed",emotion_value="-1"},
                new EmotionValues{count = 1, emotion_id = 3, emotion_title="Irritated",emotion_value="-1"},


                new EmotionValues{count = 2, emotion_id = 4, emotion_title="Very bad",emotion_value="-2"},
                new EmotionValues{count = 2, emotion_id = 4, emotion_title="Arrogant",emotion_value="-2"},
                new EmotionValues{count = 2, emotion_id = 4, emotion_title="frustrated",emotion_value="-2"}
            };

                // already sorted // Emotions.GroupBy(emo => emo.emotion_value).ToList();

            /////////////////////// for testing only ////////////////


            #region REGION 1 BUTTON STACK


            Label region1Label = new Label
            {
                Text = "Warm",
                WidthRequest = 60
            };
            region1DownArrow = new Image
            {
                Source = "downarrow",
                HeightRequest = 15,
                WidthRequest = 15
            };
            Image region1Icon = new Image
            {
                Source = "ic_red"
            };

            StackLayout region1Button = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Padding = 10,
                Children = { region1Icon, region1Label, region1DownArrow }
            };
            TapGestureRecognizer emotion1TapRecognizer = new TapGestureRecognizer();
            region1Button.GestureRecognizers.Add(emotion1TapRecognizer);
            emotion1TapRecognizer.Tapped += Emotion1TapRecognizerTapped;

            region1ButtonStack = new StackLayout
            {
                // add all warm button elements to this container.
                // head icon, label and arrow icon. rotate the arrow icon 180 degree on each click.
                Orientation = StackOrientation.Vertical,
                Children = { region1Button }
            };

            #endregion

            #region REGION 2 BUTTON STACK


            Label region2Label = new Label
            {
                Text = "Assertive",
                WidthRequest = 60
            };
            region2DownArrow = new Image
            {
                Source = "downarrow",
                HeightRequest = 15,
                WidthRequest = 15
            };
            Image region2Icon = new Image
            {
                Source = "ic_orge"
            };
            StackLayout region2Button = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Padding = 10,
                Children = { region2Icon, region2Label, region2DownArrow }
            };
            region2ButtonStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = { region2Button }
            };
            TapGestureRecognizer emotion2TapRecognizer = new TapGestureRecognizer();
            region2Button.GestureRecognizers.Add(emotion2TapRecognizer);
            emotion2TapRecognizer.Tapped += Emotion2TapRecognizerTapped;
            #endregion

            #region REGION 3 BUTTON STACK
            region3DownArrow = new Image
            {
                Source = "downarrow",
                HeightRequest = 15,
                WidthRequest = 15
            };

            Label region3Label = new Label
            {
                Text = "Patient",
                WidthRequest = 60
            };
            Image region3Icon = new Image
            {
                Source = "ic_green"
            };

            StackLayout region3Button = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Padding = 10,
                Children = { region3Icon, region3Label, region3DownArrow }
            };
            TapGestureRecognizer emotion3TapRecognizer = new TapGestureRecognizer();
            region3Button.GestureRecognizers.Add(emotion3TapRecognizer);
            emotion3TapRecognizer.Tapped += Emotion3TapRecognizerTapped;

            region3ButtonStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = { region3Button }
            };


            #endregion

            #region REGION 4 BUTTON STACK

            region4DownArrow = new Image
            {
                Source = "downarrow",
                HeightRequest = 15,
                WidthRequest = 15
            };


            Label region4Label = new Label
            {
                Text = "Detailed",
                WidthRequest = 60
            };
            Image region4Icon = new Image
            {
                Source = "ic_blu"
            };


            StackLayout region4Button = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Padding = 10,
                Children = { region4Icon, region4Label, region4DownArrow }
            };
            TapGestureRecognizer emotion4TapRecognizer = new TapGestureRecognizer();
            region4Button.GestureRecognizers.Add(emotion4TapRecognizer);
            emotion4TapRecognizer.Tapped += Emotion4TapRecognizerTapped;
            region4ButtonStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = { region4Button }
            };

            #endregion

            StackLayout BottomStackContainer = new StackLayout
            {
                // add all buttons to this container.
                BackgroundColor = Color.White,
                //Padding = 10,
                WidthRequest = App.screenWidth * .80,
                Orientation = StackOrientation.Vertical,
                Spacing = 5, // should be same as that of inner stack spacing //
                Children = { new StackLayout { Orientation = StackOrientation.Horizontal, Padding = new Thickness(10,0,0,0), Spacing = 10, Children = { region1ButtonStack, region4ButtonStack } }, // 1. red, 2. blue
                             new StackLayout { Orientation = StackOrientation.Horizontal, Padding = new Thickness(10,0,0,0),Spacing = 10, Children = { region2ButtonStack, region3ButtonStack } }} // 3. organe, 4. green
            };

            #endregion

            masterLayout.AddChildToLayout(BottomStackContainer, 10, 65);

            Content = masterLayout;
        }

        void Emotion4TapRecognizerTapped(object sender, System.EventArgs e)
        {
            ShowEmotionList(4);// blue
        }

        void Emotion3TapRecognizerTapped(object sender, System.EventArgs e)
        {
            ShowEmotionList(3); // green
        }

        void Emotion2TapRecognizerTapped(object sender, System.EventArgs e)
        {
            ShowEmotionList(2); // orange
        }

        void Emotion1TapRecognizerTapped(object sender, System.EventArgs e)
        {
            //call show emotion for 1st region.
            ShowEmotionList(1); // red
        }

        private void ShowEmotionList(int regionCode)
        {
            try
            {
                //remove sub list from from display//
                var stck1 = region1ButtonStack.Children.FirstOrDefault(c => c.ClassId == "emotionSubContainer1");
                if (stck1 != null)
                {
                    region1ButtonStack.Children.Remove(stck1);
                    region1DownArrow.Rotation = 0;
                }
                var stck2 = region2ButtonStack.Children.FirstOrDefault(c => c.ClassId == "emotionSubContainer2");
                if (stck2 != null)
                {
                    region2ButtonStack.Children.Remove(stck2);
                    region2DownArrow.Rotation = 0;
                }
                var stck3 = region3ButtonStack.Children.FirstOrDefault(c => c.ClassId == "emotionSubContainer3");
                if (stck3 != null)
                {
                    region3ButtonStack.Children.Remove(stck3);
                    region3DownArrow.Rotation = 0;
                }
                var stck4 = region4ButtonStack.Children.FirstOrDefault(c => c.ClassId == "emotionSubContainer4");
                if (stck4 != null)
                {
                    region4ButtonStack.Children.Remove(stck4);
                    region4DownArrow.Rotation = 0;
                }

                if (currentSubMenuDisplaying == regionCode && (stck1 != null ||stck2 != null || stck3 != null || stck4 != null) )
                {
                    stck1 = null;
                    stck2 = null;
                    stck3 = null;
                    stck4 = null;

                    return;
                }

                stck1 = null; 
                stck2 = null;
                stck3 = null;
                stck4 = null;

                List<EmotionValues> currentRegionEmotions = null;

                // show emotion sub list according to the region parameter
                switch (regionCode)
                {
                    case 1: // red
                        currentRegionEmotions = Emotions.Where(e => e.emotion_value == "-2").ToList();
                        currentSubMenuDisplaying = 1;
                        break;
                    case 2: // orange
                        currentRegionEmotions = Emotions.Where(e => e.emotion_value == "-1").ToList();
                        currentSubMenuDisplaying = 2;
                        break;
                    case 3: // green
                        currentRegionEmotions = Emotions.Where(e => e.emotion_value == "1").ToList();
                        currentSubMenuDisplaying = 3;
                        break;
                    case 4: // blue
                        currentRegionEmotions = Emotions.Where(e => e.emotion_value == "2").ToList();
                        currentSubMenuDisplaying = 4;
                        break;
                    default:
                        break;
                }

                if (currentRegionEmotions == null || currentRegionEmotions.Count < 1 )
                {
                    return;
                }


                StackLayout subEmotionsContainer = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 5, Padding = new Thickness(15,0,0,2), ClassId = "emotionSubContainer" + regionCode };

                foreach (EmotionValues emmotionVal in currentRegionEmotions)
                {
                    if (!string.IsNullOrEmpty(emmotionVal.emotion_title))
                    {
                        Label emotionLabel = new Label { Text = emmotionVal.emotion_title };
                        subEmotionsContainer.Children.Add(emotionLabel);
                    }
                }

                switch (regionCode)
                {
                    case 1: // red
                        region1ButtonStack.Children.Add(subEmotionsContainer);
                        region1DownArrow.Rotation = 180;
                        break;
                    case 2: // orange
                        region2ButtonStack.Children.Add(subEmotionsContainer);
                        region2DownArrow.Rotation = 180;
                        break;
                    case 3: // green
                        region3ButtonStack.Children.Add(subEmotionsContainer);
                        region3DownArrow.Rotation = 180;
                        break;
                    case 4: // blue
                        region4ButtonStack.Children.Add(subEmotionsContainer);
                        region4DownArrow.Rotation = 180;
                        break;
                    default:
                        break;
                }


            }
            catch (System.Exception ex)
            {
                var test = ex.Message;
            }
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }
    }
}
