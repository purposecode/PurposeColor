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
		PendingGoalsObject pendingGoalsObject;
        public GoalsMainPage()
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
			if (gemsGoalsObject != null && gemsGoalsObject.resultarray != null && gemsGoalsObject.resultarray.Count > 0)
				return;

            IProgressBar progress = DependencyService.Get<IProgressBar>();
            progress.ShowProgressbar("Loading pending items..");


            try
            {
			    pendingGoalsObject = await ServiceHelper.GetAllPendingGoalsAndActions ();
				if( pendingGoalsObject == null || pendingGoalsObject.resultarray == null )
				{
					progress.HideProgressbar();
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
                    App.Settings.DeleteAllPendingGoals();
                    App.Settings.SavePendingGoalsDetails( pendingGoalsObject );
                }

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
                    App.Settings.DeleteAllCompletedGoals();
                    App.Settings.SaveCompletedGoalsDetails(gemsGoalsObject);
                }
				
				if( pendingGoalsObject.resultarray.Count  + gemsGoalsObject.resultarray.Count < 30 )
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
				}



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
            PendingGoalsDetails selgoal = pendingGoalsObject.resultarray.FirstOrDefault(itm => itm.goal_id == selectedGoalID);
            if (selgoal != null)
            {
                foreach (var item in selgoal.pending_action_title)
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

	public class GemsPageInfo
	{
		public string MainTitle { get; set; }
		public string SubTitle { get; set; }
		public string FirstDetailsInfo { get; set; }
		public string FirstDateInfo { get; set; }
		public string SecondDetailsInfo { get; set; }
		public string SecondDateInfo { get; set; }
		public string FirstImage { get; set; }
		public string SecondImage { get; set; }
		public bool IsMainTitleVisible { get; set; }
		public string TrimmedFirstDetailsInfo
		{
			get
			{
				if (FirstDetailsInfo != null && FirstDetailsInfo.Length > 50)
				{
					string trimmedInfo = FirstDetailsInfo.Substring(0, 50);
					return trimmedInfo + "....";
				}
				return FirstDetailsInfo;
			}
		}
	}
}
