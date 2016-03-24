using Cross;
using CustomControls;
using Multiselect;
using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using PurposeColor.Model;
using PurposeColor.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace PurposeColor.screens
{
	public class CommonActionTitle
	{
		public string goal_id { get; set; }
		public string savedgoal_id { get; set; }
		public string actionstatus_id { get; set; }
		public string goalaction_id { get; set; }
		public string action_title { get; set; }
	}

	public class CommonGoalsDetails
	{
		public string GoalTitle { get; set; }
		public string GoalDesc { get; set; }
		public string GoalID { get; set; }
		public string GoalMedia{ get; set; }
		public bool IsPending{ get; set; }
		public List<CommonActionTitle> ActionTitle { get; set; }
	}

	public class CommonGoalsObject
	{
		public List<CommonGoalsDetails> GoalsDetails { get; set; }
	}

    public class GoalsPage : ContentPage, IDisposable
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
		PendingGoalsObject pendingGoalsObject;
		CommonGoalsObject commonGoalsObject;
		CommonGoalsObject allCommonGoalsObject;
		bool reachedEnd;
		bool reachedFront;
		int MAX_ROWS_AT_A_TIME = 10;

		public GoalsPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            progressBar = DependencyService.Get<IProgressBar>();

            this.Appearing += OnAppearing;
			mainTitleBar = new GemsPageTitleBar(Color.FromRgb(8, 135, 224), "Goals & Dreams", Color.White, "", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += OnImageAreaTapGestureRecognizerTapped;


            masterScroll = new ScrollView();
            masterScroll.WidthRequest = App.screenWidth - 20;
            masterScroll.HeightRequest = App.screenHeight * 85 / 100;
            masterScroll.BackgroundColor = Color.FromRgb( 244, 244, 244 );
			masterScroll.IsClippedToBounds = true;

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
			if (commonGoalsObject != null && commonGoalsObject.GoalsDetails != null && commonGoalsObject.GoalsDetails.Count > 0)
				return;

	
			commonGoalsObject = null;
			commonGoalsObject = new CommonGoalsObject ();
			commonGoalsObject.GoalsDetails = new List<CommonGoalsDetails>();

            IProgressBar progress = DependencyService.Get<IProgressBar>();
            progress.ShowProgressbar("Loading pending items..");


            try
            {
			    pendingGoalsObject = await ServiceHelper.GetAllPendingGoalsAndActions ();
				if( pendingGoalsObject == null || pendingGoalsObject.resultarray == null )
				{
					var success = await DisplayAlert (Constants.ALERT_TITLE, "Error in fetching Pending goals", Constants.ALERT_OK, Constants.ALERT_RETRY);
					if (!success) 
					{
						OnAppearing (sender, EventArgs.Empty);
						return;
					} 
					else
					{
                        if (Device.OS != TargetPlatform.WinPhone)
                            pendingGoalsObject = App.Settings.GetPendingGoalsObject();
						progress.HideProgressbar ();
					}
				}
                else
                {

					foreach (var goalItem in pendingGoalsObject.resultarray) 
					{
						CommonGoalsDetails commonGoalsDetails = new CommonGoalsDetails();
						commonGoalsDetails.GoalMedia = goalItem.goal_media;
						commonGoalsDetails.GoalTitle = goalItem.goal_title;
						commonGoalsDetails.GoalID = goalItem.goal_id;
						commonGoalsDetails.GoalDesc = goalItem.goal_details;
						commonGoalsDetails.IsPending = true;
						commonGoalsDetails.ActionTitle = new List<CommonActionTitle>();


						foreach (var actionItem in goalItem.pending_action_title) 
						{
							CommonActionTitle commonTitle = new CommonActionTitle();
							commonTitle.actionstatus_id = actionItem.actionstatus_id;
							commonTitle.action_title = actionItem.action_title;
							commonTitle.goalaction_id = actionItem.goalaction_id;
							commonTitle.goal_id = actionItem.goal_id;
							commonTitle.savedgoal_id = actionItem.savedgoal_id;

							commonGoalsDetails.ActionTitle.Add( commonTitle );						
						}

						if ( null == commonGoalsObject.GoalsDetails.Find( tst => tst.GoalID == commonGoalsDetails.GoalID ))
							commonGoalsObject.GoalsDetails.Add( commonGoalsDetails );
					}
                    App.Settings.DeleteAllPendingGoals();
                    App.Settings.SavePendingGoalsDetails( pendingGoalsObject );
                }

				commonGoalsObject.GoalsDetails.Add( new CommonGoalsDetails{ GoalID="header"} );

				gemsGoalsObject = await ServiceHelper.GetAllMyGoals();
				if( gemsGoalsObject == null || gemsGoalsObject.resultarray == null )
				{
					var success = await DisplayAlert (Constants.ALERT_TITLE, "Error in fetching all goals", Constants.ALERT_OK, Constants.ALERT_RETRY);
					if (!success) 
					{
						OnAppearing (sender, EventArgs.Empty);
						return;
					} 
					else
					{
                        if (Device.OS != TargetPlatform.WinPhone)
                            gemsGoalsObject = App.Settings.GetCompletedGoalsObject();
                        progress.HideProgressbar();
					}
				}
                else
                {
					foreach (var allGoalItem in gemsGoalsObject.resultarray) 
					{
						CommonGoalsDetails commonGoalsDetails = new CommonGoalsDetails();
						commonGoalsDetails.GoalMedia = allGoalItem.goal_media;
						commonGoalsDetails.GoalTitle = allGoalItem.goal_title;
						commonGoalsDetails.GoalID = allGoalItem.goal_id;
						commonGoalsDetails.GoalDesc = allGoalItem.goal_details;
						commonGoalsDetails.ActionTitle = new List<CommonActionTitle>();
						commonGoalsDetails.IsPending = false;

						if( allGoalItem.action_title != null && allGoalItem.action_title.Count > 0 )
						{
							foreach (var allActionItem in allGoalItem.action_title) 
							{
								CommonActionTitle commonTitle = new CommonActionTitle();
								commonTitle.action_title = allActionItem.action_title;
								commonTitle.goalaction_id = allActionItem.goalaction_id;
								commonTitle.goal_id = allActionItem.goal_id;
								commonGoalsDetails.ActionTitle.Add( commonTitle );						
							}
						}


						if ( null == commonGoalsObject.GoalsDetails.Find( tst => tst.GoalID == commonGoalsDetails.GoalID ))
						commonGoalsObject.GoalsDetails.Add( commonGoalsDetails );
					}

                    App.Settings.DeleteAllCompletedGoals();
                    App.Settings.SaveCompletedGoalsDetails(gemsGoalsObject);
                }


				allCommonGoalsObject = new CommonGoalsObject();
				allCommonGoalsObject.GoalsDetails = commonGoalsObject.GoalsDetails.Skip(0).Take( commonGoalsObject.GoalsDetails.Count ).ToList();
			


				/*if( pendingGoalsObject.resultarray.Count  + gemsGoalsObject.resultarray.Count < 30 )
				{
					#region PENDING GOALS
					CreatePendingGoalsView( true );
					#endregion

					#region ALL GOALS HEADING
					CreateAllGoalsHeading();
					#endregion

					#region ALL GOALS
					CreateAllGoalsView();
					#endregion
				}
				else if( pendingGoalsObject.resultarray.Count > 30 )
				{
					pendingGoalsObject.resultarray = pendingGoalsObject.resultarray.Skip(0).Take(30).ToList();
					#region PENDING GOALS
					CreatePendingGoalsView( true );
					#endregion
				}*/

				commonGoalsObject.GoalsDetails = commonGoalsObject.GoalsDetails.Skip(0).Take( MAX_ROWS_AT_A_TIME ).ToList();

				await DownloadMedias();

				RenderGoalsPage( false );



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


        void CreateAllGoalsView()
        {
			try 
			{
				if (gemsGoalsObject.resultarray == null)
					return;

				if( gemsGoalsObject.resultarray.Count < 1 )
				{
					DisplayAlert ( Constants.ALERT_TITLE, "No goals present", Constants.ALERT_OK );
					return;
				}

				foreach (var item in gemsGoalsObject.resultarray) 
				{

					StackLayout cellContainer = new StackLayout();
					cellContainer.Orientation = StackOrientation.Vertical;
					cellContainer.BackgroundColor = Color.White;// Color.FromRgb(244, 244, 244);
					cellContainer.Spacing = 0;
					cellContainer.Padding = new Thickness(10, 10, 10, 10);

					double screenWidth = App.screenWidth;
					double screenHeight = App.screenHeight;

					TapGestureRecognizer goalsTap = new TapGestureRecognizer();
					goalsTap.Tapped += OnGoalsTapped;
					Label firstDetailsInfo = new Label();
                    string firstDetails = (item.goal_details != null) ? item.goal_details : "";
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
                    firstDetailsInfo.ClassId = item.goal_id;
					firstDetailsInfo.GestureRecognizers.Add(goalsTap);


					Image goalImage = new Image();
					goalImage.Source = Device.OnPlatform("goals_badge_blue.png", "goals_badge_blue.png", "//Assets//goals_badge_blue.png");
					goalImage.HeightRequest = 100;
					goalImage.WidthRequest = 30;
					goalImage.Aspect = Aspect.Fill;


                    Image mediaImage = new Image();
                    mediaImage.Source = Constants.SERVICE_BASE_URL + item.goal_media;//Device.OnPlatform("avatar.jpg", "avatar.jpg", "//Assets//avatar.jpg");
                    mediaImage.HeightRequest = 80;
                    mediaImage.WidthRequest = 100;
                    mediaImage.ClassId = item.goal_id;
                    mediaImage.GestureRecognizers.Add(goalsTap);
                    mediaImage.Aspect = Aspect.Fill;


					StackLayout firstRow = new StackLayout();
					firstRow.Orientation = StackOrientation.Horizontal;
					firstRow.BackgroundColor = Color.White;
					firstRow.WidthRequest = App.screenWidth - 20;
					firstRow.Padding = new Thickness(0, 10, 0, 10);

					firstRow.Children.Add(goalImage);
					firstRow.Children.Add(firstDetailsInfo);
					firstRow.Children.Add(mediaImage);

					cellContainer.Children.Add(firstRow);


					//int actionTitleCount = ( item.action_title != null  && item.action_title.Count > 0 ) ? goalsObject.resultarray[index].action_title.Count : 0;
					if( item.action_title != null )
					{
						foreach (var itemAction in item.action_title)
						{

							Image bgImage = new Image();
							bgImage.Source = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
							bgImage.WidthRequest = App.screenWidth - 40;
							bgImage.HeightRequest = Device.OnPlatform(50, 50, 50);
							bgImage.Aspect = Aspect.Fill;
							//bgImage.ClassId = goalsObject.resultarray [index].goal_id + "&&" + goalsObject.resultarray [index].action_title [pendingIndex].goalaction_id;
							//bgImage.GestureRecognizers.Add ( actionTap );

							Label pendingGoalTitle = new Label();
							pendingGoalTitle.TextColor = Color.Black;
							pendingGoalTitle.XAlign = TextAlignment.Center;
							pendingGoalTitle.FontSize = Device.OnPlatform(15, 15, 18);
							pendingGoalTitle.WidthRequest = App.screenWidth * Device.OnPlatform( 65, 60, 60 ) / 100;
							//  pendingGoalTitle.HeightRequest = Device.OnPlatform(25, 20, 40);
							pendingGoalTitle.Text =  ( itemAction != null && itemAction.action_title != null ) ? itemAction.action_title : "";

							if (pendingGoalTitle.Text.Length > 25)
							{
								pendingGoalTitle.Text = pendingGoalTitle.Text.Substring(0, 25);
								pendingGoalTitle.Text = pendingGoalTitle.Text + "....";
							}


							StackLayout trasprntClickLayout = new StackLayout();
							trasprntClickLayout.WidthRequest = 50;
							trasprntClickLayout.HeightRequest = 50;
							trasprntClickLayout.BackgroundColor = Color.Transparent;
							trasprntClickLayout.VerticalOptions = LayoutOptions.Center;


							Image tickImage = new Image();
							tickImage.IsEnabled = false;
                            tickImage.Source = Device.OnPlatform("tic_active.png", "tic_active.png", "//Assets//tic_active.png");
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
							pendingRow.ClassId = item.goal_id + "&&" + itemAction.goalaction_id;

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
					else
					{
						masterStack.Children.Add(cellContainer);
					}

				}
			} 
			catch (Exception ex) 
			{
				System.Diagnostics.Debug.WriteLine ( ex.Message );
			}
		
        }



        async void OnActionTapped (object sender, EventArgs e)
        {
			CustomLayout clikedLayout = sender as CustomLayout;
	
			if (clikedLayout != null && clikedLayout.ClassId != null)
			{
				string[] delimiters = { "&&" };
				string[] clasIDArray = clikedLayout.ClassId.Split(delimiters, StringSplitOptions.None);
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

                    DetailsPageModel model = new DetailsPageModel() { eventMediaArray = null, actionMediaArray = actionMediaList, pageTitleVal = "Action Details", titleVal = title, description = details, Media = gemsGoalsObject.mediapath, NoMedia = gemsGoalsObject.noimageurl, gemId = actionTitle.goalaction_id, gemType = GemType.Action };
					//await Navigation.PushAsync (new GemsDetailsPage (null, actionMediaList, "Action Details", title, details, gemsGoalsObject.mediapath, gemsGoalsObject.noimageurl, actionTitle.goalaction_id ,GemType.Action));
                    await Navigation.PushAsync(new GemsDetailsPage(model));
				}
				else
				{
					DisplayAlert ( Constants.ALERT_TITLE, "No informations about selected action", "cancel" );
				}

			}
        }

        void CreatePendingGoalsView( bool pendingGoals )
        {
            try
            {
                if (pendingGoalsObject == null || pendingGoalsObject.resultarray == null)
                    return;

                if (pendingGoalsObject.resultarray.Count < 1)
                {
                    DisplayAlert(Constants.ALERT_TITLE, "You have no pending goals !.", Constants.ALERT_OK);
                    return;
                }

                // Pending goals
                foreach (var item in pendingGoalsObject.resultarray)
                {

                    StackLayout cellContainer = new StackLayout();
                    cellContainer.Orientation = StackOrientation.Vertical;
                    cellContainer.BackgroundColor = Color.White;// Color.FromRgb(244, 244, 244);
                    cellContainer.Spacing = 0;
                    cellContainer.Padding = new Thickness(10, 10, 10, 10);

                    double screenWidth = App.screenWidth;
                    double screenHeight = App.screenHeight;

                    TapGestureRecognizer goalsTap = new TapGestureRecognizer();
                    goalsTap.Tapped += OnGoalsTapped;
                    Label firstDetailsInfo = new Label();
                    string firstDetails = (item.goal_details != null) ? item.goal_details : "";
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
                    firstDetailsInfo.ClassId = item.goal_id;
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
                    mediaImage.Source =  Constants.SERVICE_BASE_URL +  item.goal_media;//Device.OnPlatform("avatar.jpg", "avatar.jpg", "//Assets//avatar.jpg");
                    mediaImage.HeightRequest = 80;
                    mediaImage.WidthRequest = 100;
                    mediaImage.GestureRecognizers.Add(goalsTap);
                    mediaImage.ClassId = item.goal_id;
                    mediaImage.Aspect = Aspect.Fill;


                    StackLayout firstRow = new StackLayout();
                    firstRow.Orientation = StackOrientation.Horizontal;
                    firstRow.BackgroundColor = Color.White;
                    firstRow.WidthRequest = App.screenWidth - 20;
                    firstRow.Padding = new Thickness(0, 10, 0, 10);

                    firstRow.Children.Add(goalImage);
                    firstRow.Children.Add(firstDetailsInfo);
                    firstRow.Children.Add(mediaImage);

                    cellContainer.Children.Add(firstRow);

                    if (item.pending_action_title != null)
                    {
                        foreach (var pendingItem in item.pending_action_title)
                        {
                            TapGestureRecognizer checkboxTap = new TapGestureRecognizer();
                            checkboxTap.Tapped += OnPendingGoalsTapped;

                            Image bgImage = new Image();
                            bgImage.Source = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
                            bgImage.WidthRequest = App.screenWidth - 40;
                            bgImage.HeightRequest = Device.OnPlatform(50, 50, 50);
                            bgImage.Aspect = Aspect.Fill;

                            Label pendingGoalTitle = new Label();
                            pendingGoalTitle.TextColor = Color.Black;
                            pendingGoalTitle.XAlign = TextAlignment.Center;
                            pendingGoalTitle.FontSize = Device.OnPlatform(15, 15, 18);
                            pendingGoalTitle.WidthRequest = App.screenWidth * Device.OnPlatform(65, 60, 60) / 100;
                            //  pendingGoalTitle.HeightRequest = Device.OnPlatform(25, 20, 40);
                            pendingGoalTitle.Text = (pendingItem != null && pendingItem.action_title != null) ? pendingItem.action_title : "";

                            if (pendingGoalTitle.Text.Length > 20)
                            {
                                pendingGoalTitle.Text = pendingGoalTitle.Text.Substring(0, 20);
                                pendingGoalTitle.Text = pendingGoalTitle.Text + "....";
                            }

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
                            tickImage.ClassId = pendingItem.goal_id + "&&" + pendingItem.savedgoal_id;
                            tickImage.HorizontalOptions = LayoutOptions.Center;
                            tickImage.VerticalOptions = LayoutOptions.End;
                            tickImage.TranslationY = 15;
                            trasprntClickLayout.Children.Add(tickImage);


                            CustomLayout pendingRow = new CustomLayout();
                            pendingRow.ClassId = pendingItem.savedgoal_id;
                            pendingRow.WidthRequest = App.screenWidth * 90 / 100;
                            pendingRow.HeightRequest = 50;
                            pendingRow.AddChildToLayout(bgImage, 0, 0, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);

                            //pendingRow.AddChildToLayout(tickImage, Device.OnPlatform( 4, 2, 2 ), Device.OnPlatform( -5, 25, 25 ), (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
                            pendingRow.AddChildToLayout(trasprntClickLayout, 0, 0, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
                            pendingRow.AddChildToLayout(pendingGoalTitle, Device.OnPlatform(15, 20, 20), Device.OnPlatform(31, 25, 25), (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
                            cellContainer.ClassId = pendingItem.savedgoal_id;
                            cellContainer.Children.Add(pendingRow);
                        }

                        masterStack.Children.Add(cellContainer);
                        StackLayout trans = new StackLayout();
                        trans.BackgroundColor = Color.FromRgb(244, 244, 244);
                        trans.HeightRequest = 30;
                        trans.WidthRequest = App.screenWidth;
                        trans.ClassId = "translayout";
                        masterStack.Children.Add(trans);
                    }



                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine( ex.Message );
            }
			
        }



		void RenderGoalsPage( bool prevButtonNeeded )
		{
			try
			{
				if (commonGoalsObject == null || commonGoalsObject.GoalsDetails == null)
					return;


				// Pending goals
				foreach (var item in commonGoalsObject.GoalsDetails)
				{

					if( item.GoalID == "header" )
					{
						CreateAllGoalsHeading();
						continue;
					}
					StackLayout cellContainer = new StackLayout();
					cellContainer.Orientation = StackOrientation.Vertical;
					cellContainer.BackgroundColor = Color.White;// Color.FromRgb(244, 244, 244);
					cellContainer.Spacing = 0;
					cellContainer.Padding = new Thickness(10, 10, 10, 10);

					double screenWidth = App.screenWidth;
					double screenHeight = App.screenHeight;

					TapGestureRecognizer goalsTap = new TapGestureRecognizer();
					goalsTap.Tapped += OnGoalsTapped;
					Label firstDetailsInfo = new Label();
					string firstDetails = (item.GoalDesc != null) ? item.GoalDesc : "";
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
					firstDetailsInfo.ClassId = item.GoalID;
					firstDetailsInfo.GestureRecognizers.Add(goalsTap);


					Image goalImage = new Image();
					if ( item.IsPending )
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

					string fileName = Path.GetFileName( item.GoalMedia );
					Image mediaImage = new Image();
					mediaImage.Source = App.DownloadsPath + fileName;//Device.OnPlatform("avatar.jpg", "avatar.jpg", "//Assets//avatar.jpg");
					mediaImage.HeightRequest = 80;
					mediaImage.WidthRequest = 100;
					mediaImage.GestureRecognizers.Add(goalsTap);
					mediaImage.ClassId = item.GoalID;
					mediaImage.Aspect = Aspect.Fill;


					StackLayout firstRow = new StackLayout();
					firstRow.Orientation = StackOrientation.Horizontal;
					firstRow.BackgroundColor = Color.White;
					firstRow.WidthRequest = App.screenWidth - 20;
					firstRow.Padding = new Thickness(0, 10, 0, 10);

					firstRow.Children.Add(goalImage);
					firstRow.Children.Add(firstDetailsInfo);
					firstRow.Children.Add(mediaImage);

					cellContainer.Children.Add(firstRow);

					if (item.ActionTitle != null)
					{
						foreach (var pendingItem in item.ActionTitle)
						{
							TapGestureRecognizer checkboxTap = new TapGestureRecognizer();
							checkboxTap.Tapped += OnPendingGoalsTapped;

							Image bgImage = new Image();
							bgImage.Source = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
							bgImage.WidthRequest = App.screenWidth - 40;
							bgImage.HeightRequest = Device.OnPlatform(50, 50, 50);
							bgImage.Aspect = Aspect.Fill;

							Label pendingGoalTitle = new Label();
							pendingGoalTitle.TextColor = Color.Black;
							pendingGoalTitle.XAlign = TextAlignment.Center;
							pendingGoalTitle.FontSize = Device.OnPlatform(15, 15, 18);
							pendingGoalTitle.WidthRequest = App.screenWidth * Device.OnPlatform(65, 60, 60) / 100;
							//  pendingGoalTitle.HeightRequest = Device.OnPlatform(25, 20, 40);
							pendingGoalTitle.Text = (pendingItem != null && pendingItem.action_title != null) ? pendingItem.action_title : "";

							if (pendingGoalTitle.Text.Length > 20)
							{
								pendingGoalTitle.Text = pendingGoalTitle.Text.Substring(0, 20);
								pendingGoalTitle.Text = pendingGoalTitle.Text + "....";
							}

							StackLayout trasprntClickLayout = new StackLayout();
							trasprntClickLayout.WidthRequest = 50;
							trasprntClickLayout.HeightRequest = 50;
							trasprntClickLayout.BackgroundColor = Color.Transparent;
							trasprntClickLayout.VerticalOptions = LayoutOptions.Center;
							trasprntClickLayout.GestureRecognizers.Add(checkboxTap);


							if( item.IsPending )
							{
								Image tickImage = new Image();
								tickImage.IsEnabled = false;
								tickImage.Source = Device.OnPlatform("tick_box.png", "tick_box.png", "//Assets//tick_box.png");
								tickImage.Aspect = Aspect.Fill;
								tickImage.WidthRequest = 20;
								tickImage.HeightRequest = 20;
								tickImage.ClassId = pendingItem.goal_id + "&&" + pendingItem.savedgoal_id;
								tickImage.HorizontalOptions = LayoutOptions.Center;
								tickImage.VerticalOptions = LayoutOptions.End;
								tickImage.TranslationY = 15;
								trasprntClickLayout.Children.Add(tickImage);
							}
							else
							{
								Image tickImage = new Image();
								tickImage.IsEnabled = false;
								tickImage.Source = Device.OnPlatform("tic_active.png", "tic_active.png", "//Assets//tic_active.png");
								tickImage.Aspect = Aspect.Fill;
								tickImage.WidthRequest = 20;
								tickImage.HeightRequest = 20;
								tickImage.ClassId = "off";
								tickImage.HorizontalOptions = LayoutOptions.Center;
								tickImage.VerticalOptions = LayoutOptions.End;
								tickImage.TranslationY = 15;
								trasprntClickLayout.Children.Add(tickImage);
							}


							CustomLayout pendingRow = new CustomLayout();
							pendingRow.ClassId = pendingItem.savedgoal_id;
							pendingRow.WidthRequest = App.screenWidth * 90 / 100;
							pendingRow.HeightRequest = 50;
							pendingRow.AddChildToLayout(bgImage, 0, 0, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);

							//pendingRow.AddChildToLayout(tickImage, Device.OnPlatform( 4, 2, 2 ), Device.OnPlatform( -5, 25, 25 ), (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
							pendingRow.AddChildToLayout(trasprntClickLayout, 0, 0, (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
							pendingRow.AddChildToLayout(pendingGoalTitle, Device.OnPlatform(15, 20, 20), Device.OnPlatform(31, 25, 25), (int)pendingRow.WidthRequest, (int)pendingRow.HeightRequest);
							cellContainer.ClassId = pendingItem.savedgoal_id;
							cellContainer.Children.Add(pendingRow);
						}

						masterStack.Children.Add(cellContainer);
						StackLayout trans = new StackLayout();
						trans.BackgroundColor = Color.FromRgb(244, 244, 244);
						trans.HeightRequest = 30;
						trans.WidthRequest = App.screenWidth;
						trans.ClassId = "translayout";
						masterStack.Children.Add(trans);
					}



				}


				if( prevButtonNeeded )
				{
					Button backToTop = new Button();
					backToTop.BackgroundColor = Color.Transparent;
					backToTop.TextColor = Constants.BLUE_BG_COLOR;
					backToTop.Text = "Go back to previous page";
					backToTop.FontSize = 12;
					backToTop.BorderWidth = 0;
					backToTop.BorderColor = Color.Transparent;
					backToTop.ClassId = "prev page";
					backToTop.Clicked += (object sender, EventArgs e) => 
					{
						masterScroll.Scrolled -= OnScroll;
						OnLoadPreviousGemsClicked(  masterScroll, EventArgs.Empty );
						masterScroll.Scrolled += OnScroll;
					};
					masterStack.Children.Add ( backToTop );
				}
			}
			catch (Exception ex)
			{

				Debug.WriteLine( ex.Message );
			}

		}



		private async void RefreshPendingGoalsView( string savedGoalID )
		{
            try
            {
                masterStack.Children.Clear();

                #region PENDING GOALS
                CreatePendingGoalsView(true);
                #endregion

                #region ALL GOALS HEADING
                CreateAllGoalsHeading();
                #endregion

                #region ALL GOALS
                CreateAllGoalsView();
                #endregion
            }
            catch (Exception ex)
            {

                Debug.WriteLine(  ex.Message );
            }
		
		}

        async void OnGoalsTapped(object sender, EventArgs e)
        {
            IProgressBar progress = DependencyService.Get<IProgressBar>();
            progress.ShowProgressbar( "Detailed view is loading...." );
            Label detailslabel = sender as Label;
            Image mediaImg = sender as Image;
            string goalID = "";
            if( detailslabel != null )
            {
                goalID = detailslabel.ClassId;
            }
            else if( mediaImg != null )
            {
                goalID = mediaImg.ClassId;
            }

            if( !string.IsNullOrEmpty( goalID ) )
            {
                SelectedGoal goalInfo = await ServiceHelper.GetSelectedGoalDetails(goalID);
                if( goalInfo != null )
                {
                    DetailsPageModel model = new DetailsPageModel();
                    model.actionMediaArray = null;
                    model.eventMediaArray = null;
                    model.goal_media = goalInfo.resultarray.goal_media;
                    model.Media = null;
                    model.NoMedia = null;
                    model.pageTitleVal = "Goal Details";
                    model.titleVal = goalInfo.resultarray.goal_title;
                    model.description = goalInfo.resultarray.goal_details;
                    model.gemType = GemType.Goal;
                    model.gemId = goalID;


					List<SelectedGoalMedia> downloadedMediaList = new List<SelectedGoalMedia> ();
					downloadedMediaList = await DownloadMedias ( goalInfo.resultarray.goal_media );
					progress.HideProgressbar();
                    await Navigation.PushAsync(new GemsDetailsPage(model));
                }
                else
                {
                    progress.HideProgressbar();
                    DisplayAlert(Constants.ALERT_TITLE, "Error in getting goals details", Constants.ALERT_OK);
                }
            }
            else
            {
                progress.HideProgressbar();
                DisplayAlert(Constants.ALERT_TITLE, "Not a valid goal", Constants.ALERT_OK);
            }

        }


		async Task<List<SelectedGoalMedia>>  DownloadMedias( List<SelectedGoalMedia> mediaList )
		{
			IDownload downloader =  DependencyService.Get<IDownload> ();
			List<string> mediaListToDownload = new List<string> ();
			List<SelectedGoalMedia> newList = new List<SelectedGoalMedia> ();

			foreach (var item in mediaList )
			{
				SelectedGoalMedia newMedia = new SelectedGoalMedia ();
				newMedia = item;
				if (item.media_type == "png" || item.media_type == "jpg" || item.media_type == "jpeg") 
				{
					mediaListToDownload.Add( Constants.SERVICE_BASE_URL + item.goal_media );
					string fileName = System.IO.Path.GetFileName(item.goal_media);
					newMedia.goal_media = App.DownloadsPath + fileName;
				}
				newList.Add ( newMedia );
					
			}

			var val = await downloader.DownloadFiles ( mediaListToDownload );
			return newList;
		}

        private async  Task<bool> DeletePendingActionRowFromStack(string selectedGoalID, string selectedSavedGoalID)
        {
			CommonGoalsDetails selgoal = commonGoalsObject.GoalsDetails.FirstOrDefault(itm => itm.GoalID == selectedGoalID);
            if (selgoal != null)
            {
				foreach (var item in selgoal.ActionTitle)
                {
                    View selView = masterStack.Children.FirstOrDefault(itm => itm.ClassId == item.savedgoal_id);
                    if (selView != null)
                    {
                        StackLayout selLayout = selView as StackLayout;
                        View layView = selLayout.Children.FirstOrDefault(itm => itm.ClassId == selectedSavedGoalID);
                        if (layView != null)
                        {
                            await layView.TranslateTo(300, 0, 200, Easing.BounceOut);
                            selLayout.Children.Remove(layView);
                            if (selLayout.Children.Count == 1)
                            {
                                await selLayout.TranslateTo(1000, 0, 200, Easing.BounceOut);
                                selLayout.Children.Clear();
                                int index = masterStack.Children.IndexOf( selView );
                                if( index > -1 && (index + 1) < masterStack.Children.Count )
                                {
                                    View trans = masterStack.Children[ index + 1 ];
                                    if (trans != null)
                                    {
                                        await trans.TranslateTo(1000, 0, 200, Easing.BounceOut);
                                        masterStack.Children.Remove(trans);
                                    }
                                        
                                }
                               
                            }
                        }
                    }
                }

            }

            var returmn = true;
            return returmn;
        }

        async void OnPendingGoalsTapped(object sender, EventArgs e)
        {
            StackLayout layout = sender as StackLayout;
            if( layout != null && layout.Children != null && layout.Children.Count > 0 )
            {
                Image img = (Image)layout.Children[0];
				if (img != null && img.ClassId != null )
                {
					string[] delimiters = { "&&" };
					string[] clasIDArray = img.ClassId.Split(delimiters, StringSplitOptions.None);
					string selectedSavedGoalID = clasIDArray [1];
					string selectedGoalID = clasIDArray [0];


					if (!string.IsNullOrEmpty (selectedSavedGoalID)) 
					{

						IProgressBar progress = DependencyService.Get<IProgressBar> ();
						progress.ShowProgressbar ( "Completing action...." );

						var change = await ServiceHelper.ChangePendingActionStatus ( selectedSavedGoalID );
						if (!change) 
						{
                            progress.HideProgressbar();
							DisplayAlert (Constants.ALERT_OK, "Failed to complete action", Constants.ALERT_OK);
							return;
						} 
						else
						{
                            await DeletePendingActionRowFromStack(selectedGoalID, selectedSavedGoalID);
						}
						progress.HideProgressbar ();
					}
	

                }
            }
        }


		async Task<bool>  DownloadMedias()
		{
			IDownload downloader =  DependencyService.Get<IDownload> ();
			List<string> mediaListToDownload = new List<string> ();

			foreach (var item in  allCommonGoalsObject.GoalsDetails )
			{
				if (item.GoalMedia != null ) 
				{
					string fileType = Path.GetExtension ( item.GoalMedia );

					if( fileType == ".png" || fileType == ".jpg" || fileType == ".jpeg"  )
						mediaListToDownload.Add( Constants.SERVICE_BASE_URL + item.GoalMedia );				
				}
			}

			var val = await downloader.DownloadFiles ( mediaListToDownload );
			return val;
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


		void OnLoadPreviousGemsClicked (object sender, EventArgs e)
		{
			try
			{
				pendingGoalsObject = null;
				gemsGoalsObject = null;

				CommonGoalsDetails firstItem =   commonGoalsObject.GoalsDetails.First ();

				CommonGoalsObject gemsObj =  new CommonGoalsObject();
				gemsObj.GoalsDetails = allCommonGoalsObject.GoalsDetails.Skip(0).Take( allCommonGoalsObject.GoalsDetails.Count ).ToList();
			

				int firstRendererItemIndex = gemsObj.GoalsDetails.FindIndex (itm => itm.GoalID == firstItem.GoalID);;//gemsObj.resultarray.IndexOf ( lastItem );
				if (firstRendererItemIndex > 0 && ( firstRendererItemIndex + 1 ) < gemsObj.GoalsDetails.Count )
				{
					reachedEnd = false;
					reachedFront = false;
					int itemCountToCopy = MAX_ROWS_AT_A_TIME;
					commonGoalsObject = null;
					commonGoalsObject = new CommonGoalsObject ();
					commonGoalsObject.GoalsDetails = new List<CommonGoalsDetails>();
					gemsObj.GoalsDetails.RemoveRange( firstRendererItemIndex, gemsObj.GoalsDetails.Count - firstRendererItemIndex );
					if( firstRendererItemIndex > MAX_ROWS_AT_A_TIME )
						gemsObj.GoalsDetails.RemoveRange( 0, firstRendererItemIndex - MAX_ROWS_AT_A_TIME );

					commonGoalsObject.GoalsDetails = gemsObj.GoalsDetails;

					gemsObj = null;

					masterStack.Children.Clear();

					masterScroll.Content = null;
					GC.Collect();

					RenderGoalsPage( false );

					masterScroll.Content = masterStack;
				}
				else
				{
					reachedFront = true;
				}

			} 
			catch (Exception ex) 
			{
				DisplayAlert ( Constants.ALERT_TITLE, "Low memory error.", Constants.ALERT_OK );
			}
		}


		async void OnLoadMoreGemsClicked (object sender, EventArgs e)
		{
			try 
			{
				pendingGoalsObject = null;
				gemsGoalsObject = null;

				CommonGoalsDetails lastItem =   commonGoalsObject.GoalsDetails.Last ();

				CommonGoalsObject gemsObj =  new CommonGoalsObject();
				gemsObj.GoalsDetails = allCommonGoalsObject.GoalsDetails.Skip(0).Take( allCommonGoalsObject.GoalsDetails.Count ).ToList();

				int lastRendererItemIndex = gemsObj.GoalsDetails.FindIndex (itm => itm.GoalID == lastItem.GoalID);

				Debug.WriteLine( "------- last rendered item : " + lastRendererItemIndex + "  lastitem.GoalId : " + lastItem.GoalID );
				if (lastRendererItemIndex > 0 && ( lastRendererItemIndex + 1 ) < gemsObj.GoalsDetails.Count )
				{
					reachedEnd = false;
					reachedFront = false;
					int itemCountToCopy = gemsObj.GoalsDetails.Count - lastRendererItemIndex;
					itemCountToCopy = (itemCountToCopy > MAX_ROWS_AT_A_TIME) ? MAX_ROWS_AT_A_TIME : itemCountToCopy;
					commonGoalsObject = null;
					commonGoalsObject = new CommonGoalsObject ();
					commonGoalsObject.GoalsDetails = new List<CommonGoalsDetails>();

					commonGoalsObject.GoalsDetails = gemsObj.GoalsDetails.Skip (lastRendererItemIndex).Take (itemCountToCopy).ToList();

					gemsObj = null;


					masterStack.Children.Clear();

					masterScroll.Content = null;
					GC.Collect();




					if( itemCountToCopy < MAX_ROWS_AT_A_TIME )
						RenderGoalsPage( true );
					else
						RenderGoalsPage( false );

					masterScroll.Content = masterStack;
				}
				else
				{
					reachedEnd = true;
				}

			} 
			catch (Exception ex)
			{
				DisplayAlert ( Constants.ALERT_TITLE, "Low memory error.", Constants.ALERT_OK );
			}

		}


		async  void OnScroll(object sender, ScrolledEventArgs e)
        {
            try
            {

             /*   if (masterScroll.ScrollY > headingLayout.Y )
                {
                    if (mainTitleBar.title.Text != "All Goals and Dreams")
                        mainTitleBar.title.Text = "All Goals and Dreams";
                }

                else
                {
                    if (mainTitleBar.title.Text != "Pending")
                        mainTitleBar.title.Text = "Pending";
                }*/


				if(  masterScroll.ScrollY + masterScroll.Height > ( masterStack.Height - masterStack.Y ) && !reachedEnd )
				{
					masterScroll.Scrolled -= OnScroll;
					progressBar.ShowProgressbar( "loading gems..." );


					Debug.WriteLine( "---------------  new page ----------------" );
					OnLoadMoreGemsClicked( masterScroll, EventArgs.Empty );

					//await Task.Delay( TimeSpan.FromSeconds( 1 ) );
					await masterScroll.ScrollToAsync( 0, 10, false );

					progressBar.HideProgressbar();


					await Task.Delay( TimeSpan.FromSeconds( 2 ) );
					masterScroll.Scrolled += OnScroll;


				}
				else if( masterScroll.ScrollY < Device.OnPlatform( -45, 1, 0 ) && !reachedFront  )
				{
					masterScroll.Scrolled -= OnScroll;
					progressBar.ShowProgressbar( "loading gems..." );

					Debug.WriteLine( "---------------  prev. page ----------------" );
					OnLoadPreviousGemsClicked( masterScroll, EventArgs.Empty );

					//await Task.Delay( TimeSpan.FromSeconds( 1 ) );
					await masterScroll.ScrollToAsync( 0,  masterStack.Height - 750, false );


					progressBar.HideProgressbar();

					await Task.Delay( TimeSpan.FromSeconds( 2 ) );
					masterScroll.Scrolled += OnScroll;

				}

            }
            catch (Exception ex)
            {
				Debug.WriteLine (  "#####-----" + ex.Message );
            }
        }


		/*void OnLoadPreviousGemsClicked (object sender, EventArgs e)
		{
			try
			{
				CommonGoalsDetails firstItem =   commonGoalsObject.GoalsDetails.First ();

				CommunityGemsObject gemsObj =  App.Settings.GetCommunityGemsObject ();
				int firstRendererItemIndex = gemsObj.resultarray.FindIndex (itm => itm.gem_id == firstItem.GoalID);;//gemsObj.resultarray.IndexOf ( lastItem );
				if (firstRendererItemIndex > 0 && ( firstRendererItemIndex + 1 ) < gemsObj.resultarray.Count )
				{
					reachedEnd = false;
					reachedFront = false;
					int itemCountToCopy = MAX_ROWS_AT_A_TIME;
					communityGems = null;
					communityGems = new CommunityGemsObject ();
					communityGems.resultarray = new List<CommunityGemsDetails> ();
					gemsObj.resultarray.RemoveRange( firstRendererItemIndex, gemsObj.resultarray.Count - firstRendererItemIndex );
					if( firstRendererItemIndex > MAX_ROWS_AT_A_TIME )
						gemsObj.resultarray.RemoveRange( 0, firstRendererItemIndex - MAX_ROWS_AT_A_TIME );

					communityGems.resultarray = gemsObj.resultarray;

					gemsObj = null;

					masterStackLayout.Children.Clear();

					masterScroll.Content = null;
					GC.Collect();

					RenderGems ( communityGems, false );
				}
				else
				{
					reachedFront = true;
				}

			} 
			catch (Exception ex) 
			{
				DisplayAlert ( Constants.ALERT_TITLE, "Low memory error.", Constants.ALERT_OK );
			}
		}*/


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
            headingLayout = null;
            gemsGoalsObject = null;
            pendingGoalsObject = null;
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
