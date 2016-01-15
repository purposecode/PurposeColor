using Cross;
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
using System.Threading.Tasks;
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
        CustomLayout headingLayout;
        GemsGoalsObject gemsGoalsObject;
        public GoalsMainPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();


            this.Appearing += OnAppearing;
            mainTitleBar = new GemsPageTitleBar(Color.FromRgb(8, 135, 224), "Add Supporting Emotions", Color.White, "", false);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += OnImageAreaTapGestureRecognizerTapped;


            masterScroll = new ScrollView();
            masterScroll.WidthRequest = App.screenWidth - 20;
            masterScroll.HeightRequest = App.screenHeight * 85 / 100;
            masterScroll.BackgroundColor = Color.FromRgb( 244, 244, 244 );

            masterStack = new StackLayout();
            masterStack.Orientation = StackOrientation.Vertical;
            masterStack.BackgroundColor = Color.FromRgb(244, 244, 244);
            masterStack.Spacing = 0;
            //masterStack.Padding = new Thickness( 10, 10, 10, 10 );




        }

        void OnImageAreaTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        async void OnAppearing(object sender, EventArgs e)
        {
			if (gemsGoalsObject != null && gemsGoalsObject.resultarray != null && gemsGoalsObject.resultarray.Count > 0)
				return;

            IProgressBar progress = DependencyService.Get<IProgressBar>();
            progress.ShowProgressbar("Loading gems..");


            try
            {
                gemsGoalsObject = await ServiceHelper.GetAllSupportingGoals();

                #region PENDING GOALS
                CreateGoalsPage( true );
                #endregion

                #region ALL GOALS HEADING
                CreateAllGoalsHeading();
                #endregion

                #region ALL GOALS
                CreateGoalsPage(false, gemsGoalsObject);
                #endregion


                masterScroll.Padding = new Thickness(10, 10, 10, 0);
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

        private  void CreateAllGoalsHeading()
        {
            Image bgImage = new Image();
            bgImage.Source = Device.OnPlatform("sec_bg.png", "sec_bg.png", "//Assets//sec_bg.png");
            bgImage.WidthRequest = App.screenWidth - 20;
            bgImage.HeightRequest = 80;
            bgImage.Aspect = Aspect.Fill;

            Label pendingGoalTitle = new Label();
            pendingGoalTitle.TextColor = Color.Gray;
            pendingGoalTitle.XAlign = TextAlignment.Center;
            pendingGoalTitle.FontSize = Device.OnPlatform( 18, 18, 22 );
            pendingGoalTitle.HeightRequest = Device.OnPlatform(25, 25, 25);
            pendingGoalTitle.Text = "All Goals and Dreams";

            TapGestureRecognizer addGestureRecz = new TapGestureRecognizer();
            addGestureRecz.Tapped += OnAddGoals;
            Image addGoals = new Image();
            addGoals.Source = Device.OnPlatform("add_btn.png", "add_btn.png", "//Assets//add_btn.png");
            addGoals.WidthRequest = App.screenWidth * 15 / 100;
            addGoals.HeightRequest = App.screenHeight * 5 / 100;
            addGoals.Aspect = Aspect.Fill;
            addGoals.GestureRecognizers.Add( addGestureRecz );
            

            headingLayout = new CustomLayout();
            headingLayout.WidthRequest = App.screenWidth * 90 / 100;
            headingLayout.HeightRequest = Device.OnPlatform( 50, 50, 80 );
            headingLayout.AddChildToLayout(bgImage, 0, 0, (int)headingLayout.WidthRequest, (int)headingLayout.HeightRequest);
            headingLayout.AddChildToLayout(pendingGoalTitle, Device.OnPlatform( 20, 15, 20 ), Device.OnPlatform( 30, 25, 30 ), (int)headingLayout.WidthRequest, (int)headingLayout.HeightRequest);
            headingLayout.AddChildToLayout(addGoals, Device.OnPlatform( 83, 83, 83 ), Device.OnPlatform(25, 20, 25), (int)headingLayout.WidthRequest, (int)headingLayout.HeightRequest);
            headingLayout.Scale = 1.10;
            masterStack.Children.Add(headingLayout);
        }


        async void OnAddGoals(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddEventsSituationsOrThoughts( Constants.ADD_GOALS ));
        }


        void CreateGoalsPage( bool pendingGoals, GemsGoalsObject goalsObject )
        {

            for (int index = 0; index < goalsObject.resultarray.Count; index++)
            {

                StackLayout cellContainer = new StackLayout();
                cellContainer.Orientation = StackOrientation.Vertical;
                cellContainer.BackgroundColor = Color.White;// Color.FromRgb(244, 244, 244);
                cellContainer.Spacing = 0;
                cellContainer.Padding = new Thickness(10, 10, 10, 10);

                CustomLayout customLayout = new CustomLayout();
                customLayout.BackgroundColor = Color.White;
                customLayout.WidthRequest = App.screenWidth * 90 / 100;
                double screenWidth = App.screenWidth;
                double screenHeight = App.screenHeight;


                Label subTitle = new Label();
                subTitle.Text = "Pending";
                subTitle.TextColor = Color.Gray;
                subTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                subTitle.XAlign = TextAlignment.Center;
                int subTitleFontSize = (App.screenDensity > 1.5) ? 18 : 16;
                subTitle.VerticalOptions = LayoutOptions.Center;
                subTitle.FontSize = Device.OnPlatform(subTitleFontSize, subTitleFontSize, 22);
                subTitle.WidthRequest = App.screenWidth * 90 / 100;
                // headerLayout.HorizontalOptions = LayoutOptions.Center;
                subTitle.HeightRequest = Device.OnPlatform(40, 40, 30);

                TapGestureRecognizer goalsTap = new TapGestureRecognizer();
                goalsTap.Tapped += OnGoalsTapped;
                Label firstDetailsInfo = new Label();
                string firstDetails = "goal desripton will replace this area. now this is a dummy text to fill the empty place"; //goalsObject.resultarray[index].goal_title;
                if (firstDetails.Length > 120)
                {
                    firstDetails = firstDetails.Substring(0, 120);
                    firstDetails += "....";
                }
                firstDetailsInfo.Text = firstDetails;
                firstDetailsInfo.TextColor = Color.Gray;
                firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                firstDetailsInfo.HeightRequest = 45;
                int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(17, 15, 13) : 20;
                firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);
                firstDetailsInfo.GestureRecognizers.Add(goalsTap);


                Image goalImage = new Image();
                if (pendingGoals)
                {
                    goalImage.Source = Device.OnPlatform("goals_badge_red.png", "goals_badge_red.png", "//Assets//goals_badge_red.png");
                }
                else
                {
                    goalImage.Source = Device.OnPlatform("goals_badge_blue.png", "goals_badge_blue.png", "//Assets//goals_badge_blue.png");
                }
                goalImage.HeightRequest = 100;
                goalImage.WidthRequest = 30;
                goalImage.Aspect = Aspect.Fill;


                Image mediaImage = new Image();
                mediaImage.Source = Device.OnPlatform("avatar.jpg", "avatar.jpg", "//Assets//avatar.jpg");
                mediaImage.HeightRequest = 80;
                mediaImage.WidthRequest = 80;
				mediaImage.GestureRecognizers.Add(goalsTap);


                StackLayout firstRow = new StackLayout();
                firstRow.Orientation = StackOrientation.Horizontal;
                firstRow.BackgroundColor = Color.White;
                firstRow.WidthRequest = App.screenWidth - 20;
                firstRow.Padding = new Thickness(0, 10, 0, 10);

                firstRow.Children.Add(goalImage);
                firstRow.Children.Add(firstDetailsInfo);
                firstRow.Children.Add(mediaImage);

                cellContainer.Children.Add(firstRow);


                int actionTitleCount = ( goalsObject.resultarray[index].action_title != null && goalsObject.resultarray[index].action_title.Count > 0 ) ? goalsObject.resultarray[index].action_title.Count : 0;
                for (int pendingIndex = 0; pendingIndex < actionTitleCount; pendingIndex++)
                {
                    TapGestureRecognizer checkboxTap = new TapGestureRecognizer();
                    checkboxTap.Tapped += OnCheckboxTapTapped;

					TapGestureRecognizer actionTap = new TapGestureRecognizer ();
					actionTap.Tapped += OnActionTapped;
                    Image bgImage = new Image();
                    bgImage.Source = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
                    bgImage.WidthRequest = App.screenWidth - 40;
                    bgImage.HeightRequest = Device.OnPlatform(50, 50, 50);
                    bgImage.Aspect = Aspect.Fill;
					bgImage.ClassId = goalsObject.resultarray [index].goal_id + "&&" + goalsObject.resultarray [index].action_title [pendingIndex].goalaction_id;
					bgImage.GestureRecognizers.Add ( actionTap );

                    Label pendingGoalTitle = new Label();
                    pendingGoalTitle.TextColor = Color.Black;
                    pendingGoalTitle.XAlign = TextAlignment.Center;
                    pendingGoalTitle.FontSize = Device.OnPlatform(15, 15, 18);
					pendingGoalTitle.WidthRequest = App.screenWidth * Device.OnPlatform( 65, 60, 60 ) / 100;
                    //  pendingGoalTitle.HeightRequest = Device.OnPlatform(25, 20, 40);
                    pendingGoalTitle.Text =  goalsObject.resultarray[index].action_title[pendingIndex].action_title; //"Go to gym Go to gym Go to gym Go to gymGo to gymGo to gymGo to gymGo to gymGo to gymGo to gym Go to gymGo to gym";

                    if (pendingGoalTitle.Text.Length > 25)
                    {
                        pendingGoalTitle.Text = pendingGoalTitle.Text.Substring(0, 25);
                        pendingGoalTitle.Text = pendingGoalTitle.Text + "....";
                    }

                    Switch goalDoneSwitch = new Switch();
                    goalDoneSwitch.BackgroundColor = Color.White;
                    goalDoneSwitch.VerticalOptions = LayoutOptions.Center;
                    goalDoneSwitch.WidthRequest = 50;

                    StackLayout trasprntClickLayout = new StackLayout();
                    trasprntClickLayout.WidthRequest = 50;
                    trasprntClickLayout.HeightRequest = 50;
                    trasprntClickLayout.BackgroundColor = Color.Transparent;
                    trasprntClickLayout.VerticalOptions = LayoutOptions.Center;
                    trasprntClickLayout.GestureRecognizers.Add(checkboxTap);

                    Image tickImage = new Image();
                    tickImage.IsEnabled = false;
                    tickImage.Source = Device.OnPlatform("tick_box.png", "tick_box.png", "//Assets//tick_box.png");
                    tickImage.Aspect = Aspect.Fill;
                    tickImage.WidthRequest = 20;
                    tickImage.HeightRequest = 20;
                    tickImage.ClassId = "off";
                    tickImage.HorizontalOptions = LayoutOptions.Center;
                    tickImage.VerticalOptions = LayoutOptions.End;
                    tickImage.TranslationY = 15;
                    trasprntClickLayout.Children.Add(tickImage);


                    CustomLayout pendingRow = new CustomLayout();
                    pendingRow.WidthRequest = App.screenWidth * 90 / 100;
                    pendingRow.HeightRequest = 50;

					pendingRow.AddChildToLayout(bgImage, 0, 0, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);

					//pendingRow.AddChildToLayout(tickImage, Device.OnPlatform( 4, 2, 2 ), Device.OnPlatform( -5, 25, 25 ), (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
					pendingRow.AddChildToLayout(trasprntClickLayout, 0, 0, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
					pendingRow.AddChildToLayout(pendingGoalTitle, Device.OnPlatform(15, 20, 20), Device.OnPlatform(31, 25, 25), (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
					cellContainer.Children.Add(pendingRow);
                }

                masterStack.Children.Add(cellContainer);

                StackLayout trans = new StackLayout();
                trans.BackgroundColor = Color.FromRgb(244, 244, 244);
                trans.HeightRequest = 30;
                trans.WidthRequest = App.screenWidth;
                masterStack.Children.Add(trans);
            }
        }

        async void OnActionTapped (object sender, EventArgs e)
        {
			Image img = sender as Image;
	
			if (img != null && img.ClassId != null)
			{
				string[] delimiters = { "&&" };
				string[] clasIDArray = img.ClassId.Split(delimiters, StringSplitOptions.None);
				string selectedGoalID = clasIDArray [0];
				string selectedActionID = clasIDArray [1];

				GemsGoalsDetails selectedGoal = gemsGoalsObject.resultarray.FirstOrDefault (item => item.goal_id == selectedGoalID);
				if (selectedGoal != null ) 
				{
					List<ActionMedia> actionMediaList = selectedGoal.action_media.FindAll (item => item.goalaction_id == selectedActionID).ToList ();
					ActionTitle actionTitle = selectedGoal.action_title.FirstOrDefault ( item => item.goalaction_id == selectedActionID );
					string title = (actionTitle != null && actionTitle.action_title != null) ? actionTitle.action_title : "";
					ActionDetail actionDetail = selectedGoal.action_details.FirstOrDefault ( item => item.goalaction_id == selectedActionID );
					string details = (actionDetail != null && actionDetail.action_details != null) ? actionDetail.action_details : "";
					await Navigation.PushAsync (new GemsDetailsPage (null, actionMediaList, "Action Details", title, details, gemsGoalsObject.mediapath, gemsGoalsObject.noimageurl, actionTitle.goalaction_id ,GemType.Action));
				}
				else
				{
					DisplayAlert ( Constants.ALERT_TITLE, "No informations about selected action", "cancel" );
				}

			}
        }

        void CreateGoalsPage( bool pendingGoals )
        {
            // Pending goals
            for (int index = 0; index < 3; index++)
            {

                StackLayout cellContainer = new StackLayout();
                cellContainer.Orientation = StackOrientation.Vertical;
                cellContainer.BackgroundColor = Color.White;// Color.FromRgb(244, 244, 244);
                cellContainer.Spacing = 0;
                cellContainer.Padding = new Thickness(10, 10, 10, 10);

                CustomLayout customLayout = new CustomLayout();
                customLayout.BackgroundColor = Color.White;
                customLayout.WidthRequest = App.screenWidth * 90 / 100;
                double screenWidth = App.screenWidth;
                double screenHeight = App.screenHeight;


                Label subTitle = new Label();
                subTitle.Text = "Pending";
                subTitle.TextColor = Color.Gray;
                subTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                subTitle.XAlign = TextAlignment.Center;
                int subTitleFontSize = (App.screenDensity > 1.5) ? 18 : 16;
                subTitle.VerticalOptions = LayoutOptions.Center;
                subTitle.FontSize = Device.OnPlatform(subTitleFontSize, subTitleFontSize, 22);
                subTitle.WidthRequest = App.screenWidth * 90 / 100;
                // headerLayout.HorizontalOptions = LayoutOptions.Center;
                subTitle.HeightRequest = Device.OnPlatform(40, 40, 30);

                TapGestureRecognizer goalsTap = new TapGestureRecognizer();
                goalsTap.Tapped += OnGoalsTapped;
                Label firstDetailsInfo = new Label();
                string firstDetails = "The meaning of life, or the answer to the question What is the meaning of life?, pertains to the significance of living or existence in general. Many other questions also seek the meaning of life, including What should I do?";
                if (firstDetails.Length > 120)
                {
                    firstDetails = firstDetails.Substring(0, 120);
                    firstDetails += "....";
                }
                firstDetailsInfo.Text = firstDetails;
                firstDetailsInfo.TextColor = Color.Gray;
                firstDetailsInfo.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
                firstDetailsInfo.WidthRequest = App.screenWidth * 60 / 100;
                firstDetailsInfo.HeightRequest = 45;
                int firstDetailsInfoFontSize = (App.screenDensity > 1.5) ? Device.OnPlatform(17, 15, 13) : 20;
                firstDetailsInfo.FontSize = Device.OnPlatform(firstDetailsInfoFontSize, firstDetailsInfoFontSize, firstDetailsInfoFontSize);
                firstDetailsInfo.GestureRecognizers.Add(goalsTap);


                Image goalImage = new Image();
                if (pendingGoals)
                {
                    goalImage.Source = Device.OnPlatform("goals_badge_red.png", "goals_badge_red.png", "//Assets//goals_badge_red.png");
                }
                else
                {
                    goalImage.Source = Device.OnPlatform("goals_badge_blue.png", "goals_badge_blue.png", "//Assets//goals_badge_blue.png");
                }
                goalImage.HeightRequest = 100;
                goalImage.WidthRequest = 30;
                goalImage.Aspect = Aspect.Fill;


                Image mediaImage = new Image();
                mediaImage.Source = Device.OnPlatform("avatar.jpg", "avatar.jpg", "//Assets//avatar.jpg");
                mediaImage.HeightRequest = 80;
                mediaImage.WidthRequest = 80;
				mediaImage.GestureRecognizers.Add(goalsTap);


                StackLayout firstRow = new StackLayout();
                firstRow.Orientation = StackOrientation.Horizontal;
                firstRow.BackgroundColor = Color.White;
                firstRow.WidthRequest = App.screenWidth - 20;
                firstRow.Padding = new Thickness(0, 10, 0, 10);

                firstRow.Children.Add(goalImage);
                firstRow.Children.Add(firstDetailsInfo);
                firstRow.Children.Add(mediaImage);

                cellContainer.Children.Add(firstRow);



                for (int pendingIndex = 0; pendingIndex < 5; pendingIndex++)
                {
                    TapGestureRecognizer checkboxTap = new TapGestureRecognizer();
                    checkboxTap.Tapped += OnCheckboxTapTapped;

                    Image bgImage = new Image();
                    bgImage.Source = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
                    bgImage.WidthRequest = App.screenWidth - 40;
                    bgImage.HeightRequest = Device.OnPlatform(50, 50, 50);
                    bgImage.Aspect = Aspect.Fill;

                    Label pendingGoalTitle = new Label();
                    pendingGoalTitle.TextColor = Color.Black;
                    pendingGoalTitle.XAlign = TextAlignment.Center;
                    pendingGoalTitle.FontSize = Device.OnPlatform(15, 15, 18);
					pendingGoalTitle.WidthRequest = App.screenWidth * Device.OnPlatform( 65, 60, 60 ) / 100;
                    //  pendingGoalTitle.HeightRequest = Device.OnPlatform(25, 20, 40);
                    pendingGoalTitle.Text = "Go to gym Go to gym Go to gym Go to gymGo to gymGo to gymGo to gymGo to gymGo to gymGo to gym Go to gymGo to gym";

                    if (pendingGoalTitle.Text.Length > 25)
                    {
                        pendingGoalTitle.Text = pendingGoalTitle.Text.Substring(0, 25);
                        pendingGoalTitle.Text = pendingGoalTitle.Text + "....";
                    }

                    Switch goalDoneSwitch = new Switch();
                    goalDoneSwitch.BackgroundColor = Color.White;
                    goalDoneSwitch.VerticalOptions = LayoutOptions.Center;
                    goalDoneSwitch.WidthRequest = 50;

                    StackLayout trasprntClickLayout = new StackLayout();
                    trasprntClickLayout.WidthRequest = 50;
                    trasprntClickLayout.HeightRequest = 50;
					trasprntClickLayout.BackgroundColor = Color.Transparent;
                    trasprntClickLayout.VerticalOptions = LayoutOptions.Center;
                    trasprntClickLayout.GestureRecognizers.Add(checkboxTap);

                    Image tickImage = new Image();
                    tickImage.IsEnabled = false;
                    tickImage.Source = Device.OnPlatform("tick_box.png", "tick_box.png", "//Assets//tick_box.png");
                    tickImage.Aspect = Aspect.Fill;
                    tickImage.WidthRequest = 20;
                    tickImage.HeightRequest = 20;
                    tickImage.ClassId = "off";
                    tickImage.HorizontalOptions = LayoutOptions.Center;
                    tickImage.VerticalOptions = LayoutOptions.End;
                    tickImage.TranslationY = 15;
                    trasprntClickLayout.Children.Add(tickImage);


                    CustomLayout pendingRow = new CustomLayout();
                    pendingRow.WidthRequest = App.screenWidth * 90 / 100;
                    pendingRow.HeightRequest = 50;
					pendingRow.AddChildToLayout(bgImage, 0, 0, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);

					//pendingRow.AddChildToLayout(tickImage, Device.OnPlatform( 4, 2, 2 ), Device.OnPlatform( -5, 25, 25 ), (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
					pendingRow.AddChildToLayout(trasprntClickLayout, 0, 0, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
					pendingRow.AddChildToLayout(pendingGoalTitle, Device.OnPlatform(15, 20, 20), Device.OnPlatform(31, 25, 25), (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
					cellContainer.Children.Add(pendingRow);
                }

                masterStack.Children.Add(cellContainer);

                StackLayout trans = new StackLayout();
                trans.BackgroundColor = Color.FromRgb(244, 244, 244);
                trans.HeightRequest = 30;
                trans.WidthRequest = App.screenWidth;
                masterStack.Children.Add(trans);
            }
        }


        async void OnGoalsTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GemsDetailsPage(null, null, "", "", "", "", "", "", GemType.Action));
        }

        void OnCheckboxTapTapped(object sender, EventArgs e)
        {
            StackLayout layout = sender as StackLayout;
            if( layout != null && layout.Children != null && layout.Children.Count > 0 )
            {
                Image img = (Image)layout.Children[0];
                if (img != null && img.ClassId == "off")
                {
                    img.Source = Device.OnPlatform("tic_active.png", "tic_active.png", "//Assets//tic_active.png");
                    img.ClassId = "on";
                }
                else if( img != null && img.ClassId == "on" )
                {
                    img.Source = img.Source = Device.OnPlatform("tick_box.png", "tick_box.png", "//Assets//tick_box.png"); 
                    img.ClassId = "off";
                }
            }
        }



        protected override bool OnBackButtonPressed()
        {
            //var success =  DisplayAlert(Constants.ALERT_TITLE, "Do you want to exit from App ?", Constants.ALERT_OK, "Cancel").Result;

            Task<bool> action = DisplayAlert(Constants.ALERT_TITLE, "Do you want to exit from App ?", Constants.ALERT_OK, "Cancel");
            action.ContinueWith(task =>
            {
                bool val = task.Result;
                if (task.Result)
                {
                    CloseAllPages();

                }

            });

            return true;

        }

        private void CloseAllPages()
        {
            if (Device.OS != TargetPlatform.iOS)
            {
                Dispose();
                GC.Collect();
                IDeviceSpec device = DependencyService.Get<IDeviceSpec>();
                device.ExitApp();
            }

        }

        void OnScroll(object sender, ScrolledEventArgs e)
        {
            try
            {

              /*  System.Diagnostics.Debug.WriteLine("Scroll pos : " + masterScroll.ScrollY.ToString());
                System.Diagnostics.Debug.WriteLine("headingLayout y :" + headingLayout.Y);*/

                if (masterScroll.ScrollY > headingLayout.Y )
                {
                    if (mainTitleBar.title.Text != "All Goals and Dreams")
                        mainTitleBar.title.Text = "All Goals and Dreams";
                }

                else
                {
                    if (mainTitleBar.title.Text != "Pending")
                        mainTitleBar.title.Text = "Pending";
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
            masterLayout = null;
            progressBar = null;
            listContainer = null;
            gemsList = null;
            mainTitleBar = null;
            masterScroll = null;
            masterStack = null;
            GC.Collect();
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
}
