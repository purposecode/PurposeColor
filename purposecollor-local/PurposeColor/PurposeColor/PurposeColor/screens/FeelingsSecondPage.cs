using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using System;
using System.Threading;
using System.Threading.Tasks;
using PurposeColor.screens;
using PurposeColor.interfaces;
using PurposeColor.Service;
using System.Collections.ObjectModel;
using PurposeColor.Model;

namespace PurposeColor
{

    public class FeelingsSecondPage : ContentPage, IDisposable
    {
        CustomLayout masterLayout = null;
        //IDeviceSpec deviceSpec;
        PurposeColor.interfaces.CustomImageButton actionPickerButton = null;
        PurposeColor.interfaces.CustomImageButton goalsAndDreamsPickerButton = null;
        public static ObservableCollection<PreviewItem> actionPreviewListSource = null;
        PurposeColorSubTitleBar subTitleBar = null;
        PurposeColorTitleBar mainTitleBar = null;
        ListView actionPreviewListView = null;
        StackLayout listContainer = null;
        CustomSlider slider = null;
        CustomListViewItem selectedGoal = null;
        List<CustomListViewItem> selectedActions = null;
        IProgressBar progressBar;
        double screenHeight;
        double screenWidth;

        public FeelingsSecondPage()
        {
            progressBar = DependencyService.Get<IProgressBar>();
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            //deviceSpec = DependencyService.Get<IDeviceSpec>();
            this.Appearing += FeelingsSecondPage_Appearing;
            actionPreviewListSource = new ObservableCollection<PreviewItem>();

            mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;

            subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Awareness");
            subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;
            subTitleBar.NextButtonTapRecognizer.Tapped += NextButtonTapRecognizer_Tapped;
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;

            Label firstLine = new Label();
            firstLine.Text = "Does being";
            firstLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            firstLine.TextColor = Color.FromRgb(40, 47, 50);
            firstLine.HeightRequest = screenHeight * 15 / 100;
            firstLine.HorizontalOptions = LayoutOptions.Center;
            firstLine.WidthRequest = screenWidth;
            firstLine.XAlign = TextAlignment.Center;

            Label secondLine = new Label();
            secondLine.Text = App.SelectedEmotion;
            secondLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            secondLine.FontAttributes = FontAttributes.Italic;
            secondLine.TextColor = Constants.BLUE_BG_COLOR;//Color.FromRgb(40, 47, 50);
            secondLine.HeightRequest = screenHeight * 15 / 100;
            secondLine.HorizontalOptions = LayoutOptions.Center;
            secondLine.WidthRequest = screenWidth;
            secondLine.XAlign = TextAlignment.Center;

            Label thirdLine = new Label();
            thirdLine.Text = "support your goals and dreams?";
            thirdLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            thirdLine.TextColor = Color.FromRgb(40, 50, 50);
            thirdLine.HeightRequest = screenHeight * 11 / 100;
            thirdLine.HorizontalOptions = LayoutOptions.Center;
            thirdLine.WidthRequest = screenWidth;
            thirdLine.XAlign = TextAlignment.Center;


            goalsAndDreamsPickerButton = new PurposeColor.interfaces.CustomImageButton();
            goalsAndDreamsPickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", @"/Assets/select_box_whitebg.png");
            goalsAndDreamsPickerButton.Text = "Goals & Dreams";
            goalsAndDreamsPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            goalsAndDreamsPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            goalsAndDreamsPickerButton.FontSize = 17;
            goalsAndDreamsPickerButton.TextColor = Color.Gray;
            goalsAndDreamsPickerButton.WidthRequest = screenWidth * 90 / 100;

            goalsAndDreamsPickerButton.Clicked += OnGoalsPickerButtonClicked;


            actionPickerButton = new CustomImageButton();
            actionPickerButton.IsVisible = false;
            actionPickerButton.BackgroundColor = Color.FromRgb(30, 126, 210);
            actionPickerButton.Text = "Add Supporting Actions";
            actionPickerButton.TextColor = Color.White;

            actionPickerButton.TextOrientation = TextOrientation.Middle;
            actionPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            // actionPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            actionPickerButton.WidthRequest = screenWidth * 90 / 100;

            actionPickerButton.Clicked += OnActionPickerButtonClicked;


            if (App.screenDensity > 1.5)
            {
                firstLine.FontSize = Device.OnPlatform(20, 20, 30);
                secondLine.FontSize = Device.OnPlatform(20, 22, 30);
                thirdLine.FontSize = Device.OnPlatform(20, 20, 30);
                actionPickerButton.FontSize = 17;
                actionPickerButton.HeightRequest = screenHeight * 6 / 100;
                goalsAndDreamsPickerButton.HeightRequest = screenHeight * 6 / 100;
            }
            else
            {
                firstLine.FontSize = Device.OnPlatform(16, 18, 26);
                secondLine.FontSize = Device.OnPlatform(16, 20, 26);
                thirdLine.FontSize = Device.OnPlatform(16, 18, 26);
                actionPickerButton.FontSize = 15;
                actionPickerButton.HeightRequest = screenHeight * 9 / 100;
                goalsAndDreamsPickerButton.HeightRequest = screenHeight * 9 / 100;
            }

            slider = new CustomSlider
            {
                Minimum = -2,
                Maximum = 2,
                WidthRequest = screenWidth * 90 / 100
            };
            slider.StopGesture = GetstopGetsture;


            //Image sliderDivider1 = new Image();
            //sliderDivider1.Source = "drag_sepeate.png";


            //Image sliderDivider2 = new Image();
            //sliderDivider2.Source = "drag_sepeate.png";


            //Image sliderDivider3 = new Image();
            //sliderDivider3.Source = "drag_sepeate.png";

            //Image sliderBG = new Image();
            //sliderBG.Source = "drag_bg.png";

            this.Appearing += FeelingNowPage_Appearing;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 9));
            masterLayout.AddChildToLayout(firstLine, 0, 20);
            masterLayout.AddChildToLayout(secondLine, 0, 25);
            masterLayout.AddChildToLayout(thirdLine, 0, 30);
            //masterLayout.AddChildToLayout(sliderBG, 7, 45);
            masterLayout.AddChildToLayout(slider, 5, Device.OnPlatform(37, 35, 34));
            /*  masterLayout.AddChildToLayout(sliderDivider1, 30, 45.5f);
              masterLayout.AddChildToLayout(sliderDivider2, 50, 45.5f);
              masterLayout.AddChildToLayout(sliderDivider3, 70, 45.5f);*/
            masterLayout.AddChildToLayout(goalsAndDreamsPickerButton, 5, Device.OnPlatform(50, 50, 45));
            masterLayout.AddChildToLayout(actionPickerButton, 5, Device.OnPlatform(65, 65, 55));


            listContainer = new StackLayout();
            listContainer.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            listContainer.WidthRequest = screenWidth * 90 / 100;
            listContainer.HeightRequest = screenHeight * 20 / 100;
            listContainer.ClassId = "preview";

            actionPreviewListView = new ListView();
            actionPreviewListView.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            actionPreviewListView.ItemTemplate = new DataTemplate(typeof(ActionPreviewCellItem));
            ActionPreviewCellItem.mainPage = this;
            actionPreviewListView.SeparatorVisibility = SeparatorVisibility.None;
            actionPreviewListView.Opacity = 1;
            actionPreviewListView.ItemsSource = actionPreviewListSource;
            listContainer.Children.Add(actionPreviewListView);
            masterLayout.AddChildToLayout(listContainer, 5, Device.OnPlatform(73, 73, 64));

            Content = masterLayout;

        }

        async void NextButtonTapRecognizer_Tapped(object sender, System.EventArgs e)
        {
            bool isReadyToSave = false;
            try
            {
                this.subTitleBar.NextButtonTapRecognizer.Tapped -= NextButtonTapRecognizer_Tapped;
                if (goalsAndDreamsPickerButton.Text == "Goals & Dreams")
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Plese select Goals & Dreams.", Constants.ALERT_OK);
                }
                else if (selectedActions == null || selectedActions.Count == 0)
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Plese Supporting Actions.", Constants.ALERT_OK);
                }
                else if (slider.Value == 0)
                {
                    await DisplayAlert(Constants.ALERT_TITLE, "Please select a supporting value using the slider", Constants.ALERT_OK);
                }
                else
                {
                    isReadyToSave = true;
					SaveData();
                }
            }
            catch (System.Exception ex)
            {
                var test = ex.Message;
                DisplayAlert(Constants.ALERT_TITLE, "Network error, unable to save the detais, please try again", Constants.ALERT_OK);
            }

            if (!isReadyToSave)
            {
                this.subTitleBar.NextButtonTapRecognizer.Tapped += NextButtonTapRecognizer_Tapped;
            }
        }

        private async void SaveData()
        {
            bool isValueSaved = false;
			try 
			{
				isValueSaved = await ServiceHelper.SaveGoalsAndActions (slider.Value.ToString (), App.newEmotionId, selectedGoal.EventID, selectedActions);
				if (!isValueSaved) 
				{
					bool doRetry = await DisplayAlert (Constants.ALERT_TITLE, "Network error, unable to save the detais", "Retry", "Cancel");
					if (doRetry) 
					{
						SaveData ();
                        return;
					}
				} 
				else 
				{
                    try
                    {
                        ILocalNotification notfiy = DependencyService.Get<ILocalNotification> ();
						notfiy.ShowNotification (Constants.ALERT_TITLE, "Emotional awareness created", false);
                    }
                    catch (System.Exception)
                    {
                    }
                    progressBar.HideProgressbar();
                    App.masterPage.IsPresented = false;
                    App.masterPage.Detail = new NavigationPage(new FeelingNowPage());
                    
                    return;
				}
			} 
			catch (Exception ex) 
			{
                DisplayAlert(Constants.ALERT_TITLE, "Network error, unable to save the detais", "OK");
			}

            progressBar.HideProgressbar();
            this.subTitleBar.NextButtonTapRecognizer.Tapped += NextButtonTapRecognizer_Tapped;
        }

        public async void GetstopGetsture(bool pressed)
        {
            try
            {
                var goalsList = await DownloadAllGoals();
				await Task.Delay(100);
                OnGoalsPickerButtonClicked(goalsAndDreamsPickerButton, EventArgs.Empty);

            }
            catch (System.Exception ex)
            {
                DisplayAlert(Constants.ALERT_TITLE, "Could not update the goals", Constants.ALERT_OK);
            }
        }

        public static async Task<bool> DownloadAllGoals()
        {
            try
            {
                var goals = await ServiceHelper.GetAllGoals("2"); //for testing only
                if (goals != null)
                {
                    App.goalsListSource = null;
                    App.goalsListSource = new List<CustomListViewItem>();
                    foreach (var item in goals)
                    {
                        App.goalsListSource.Add(item);
                    }
                }
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

        public static async Task<bool> DownloadAllSupportingActions()
        {
            try
            {
                var actions = await ServiceHelper.GetAllSpportingActions();
                if (actions != null)
                {
                    App.actionsListSource = null;
                    App.actionsListSource = new List<CustomListViewItem>();
                    foreach (var item in actions)
                    {
                        App.actionsListSource.Add(item);
                    }
                }
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

        void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        async void FeelingsSecondPage_Appearing(object sender, System.EventArgs e)
        {
            IProgressBar progressBar = DependencyService.Get<IProgressBar>();
            progressBar.ShowProgressbar("Loading goals...");
            try
            {
                base.OnAppearing();

                if (App.goalsListSource == null || App.goalsListSource.Count < 1)
                {

                    bool isGoalsAvailable = await DownloadAllGoals();
                    if (!isGoalsAvailable)
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Could not retrive the goals.", Constants.ALERT_OK);
                        progressBar.HideProgressbar();
                        return;
                    }
                    else
                    {
                        // save goals to local db
                    }
                }

                if (App.actionsListSource == null || App.actionsListSource.Count < 1)
                {
                    bool isActionsAvailable = await DownloadAllSupportingActions();

                    if (!isActionsAvailable)
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Could not retrive the acions.", Constants.ALERT_OK);
                        return;
                    }
                    else
                    {
                        // save goals to local db
                    }

                }
                progressBar.HideProgressbar();

            }
            catch (System.Exception ex)
            {
                progressBar.HideProgressbar();
                DisplayAlert(Constants.ALERT_TITLE, "somthing went wrong, please try again", Constants.ALERT_OK);
            }
            progressBar.HideProgressbar();

            // this.Animate("", (s) => Layout(new Rectangle(((1 - s) * Width), Y, Width, Height)), 0, 600, Easing.SpringIn, null, null);
            //  this.Animate("", (s) => Layout(new Rectangle(X, (s - 1) * Height, Width, Height)), 0, 600, Easing.SpringIn, null, null); //slide down

            // this.Animate("", (s) => Layout(new Rectangle(X, (1 - s) * Height, Width, Height)), 0, 600, Easing.SpringIn, null, null); // slide up
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        void backButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new GraphPage());
        }

        async void OnActionPickerButtonClicked(object sender, System.EventArgs e)
        {
            try
            {
                await actionPickerButton.ScaleTo(1.5, 100, Easing.Linear);
                await actionPickerButton.ScaleTo(1, 100, Easing.Linear);

                CustomPicker ePicker = new CustomPicker(masterLayout, App.actionsListSource, 35, Constants.ADD_ACTIONS, true, true);
                ePicker.WidthRequest = screenWidth;
                ePicker.HeightRequest = screenHeight;
                ePicker.ClassId = "ePicker";
                ePicker.listView.ItemSelected += OnActionPickerItemSelected;
				masterLayout.AddLayout(ePicker, 0, 0);

                //double yPos = 60 * screenHeight / 100;
                // ePicker.TranslateTo(0, -yPos, 250, Easing.BounceIn);
                // ePicker.FadeTo(1, 750, Easing.Linear); 

            }
            catch (System.Exception ex)
            {
                var test = ex.Message;
                //DisplayAlert(Constants.ALERT_TITLE, "please try again", Constants.ALERT_OK);
            }

        }

        async void OnGoalsPickerButtonClicked(object sender, System.EventArgs e)
        {
            try
            {
                await goalsAndDreamsPickerButton.ScaleTo(1.5, 100, Easing.Linear);
                await goalsAndDreamsPickerButton.ScaleTo(1, 100, Easing.Linear);

                CustomPicker ePicker = new CustomPicker(masterLayout, App.GetGoalsList(), 35, Constants.ADD_GOALS, true, true);
                ePicker.WidthRequest = screenWidth;
                ePicker.HeightRequest = screenHeight;
                ePicker.ClassId = "ePicker";
                ePicker.listView.ItemSelected += OnGoalsPickerItemSelected;
				masterLayout.AddLayout(ePicker, 0, 0);
                //double yPos = 60 * screenHeight / 100;
                //ePicker.TranslateTo(0, yPos, 250, Easing.BounceIn);
                // ePicker.FadeTo(1, 750, Easing.Linear); 

            }
            catch (System.Exception ex)
            {
                var test = ex.Message;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {

                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                if (pickView != null)
                {
                    masterLayout.Children.Remove(pickView);
                    pickView = null;
                    return true;
                }

            }
            catch (System.Exception ex)
            {
                var test = ex.Message;
            }
            return base.OnBackButtonPressed();
        }

        void OnActionPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                CustomListViewItem item = e.SelectedItem as CustomListViewItem;
                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                masterLayout.Children.Remove(pickView);
                pickView = null;
                actionPreviewListSource.Add(new PreviewItem { Name = item.Name, Image = null });
                if (selectedActions == null)
                {
                    selectedActions = new List<CustomListViewItem>();
                }
                selectedActions.Add(item);

            }
            catch (System.Exception ex)
            {
                var test = ex.Message;
            }
        }


        public async void ShowAlert(string messege, PreviewItem toDelete)
        {
            var alert = await DisplayAlert(Constants.ALERT_TITLE, messege, Constants.ALERT_OK, "Cancel");
            if (alert)
            {
                FeelingsSecondPage.actionPreviewListSource.Remove(toDelete);
            }
        }

        async void OnGoalsPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                CustomListViewItem item = e.SelectedItem as CustomListViewItem;
                goalsAndDreamsPickerButton.Text = item.Name;
                goalsAndDreamsPickerButton.TextColor = Color.Black;
                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                masterLayout.Children.Remove(pickView);
                pickView = null;
                actionPickerButton.IsVisible = true;
                selectedGoal = item;
                if (selectedActions != null)
                {
                    selectedActions.Clear();
                }
                if (actionPreviewListSource != null)
                {
                    actionPreviewListSource.Clear();
                }
				await Task.Delay(100);
                OnActionPickerButtonClicked(actionPickerButton, EventArgs.Empty);

            }
            catch (System.Exception ex)
            {
                var test = ex.Message;
            }
        }

        void emotionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        async void FeelingNowPage_Appearing(object sender, System.EventArgs e)
        {
            /* int val = 2;
             for( int index = 0; index < 200; index++ )

             {
                 await Task.Delay(2);

                 if( slider.Value > 90 )
                 {
                     val = -2;
                 }
                 slider.Value += val;
             }*/
        }

        public void Dispose()
        {
            this.mainTitleBar.imageAreaTapGestureRecognizer.Tapped -= imageAreaTapGestureRecognizer_Tapped;
            this.mainTitleBar = null;
            this.subTitleBar.BackButtonTapRecognizer.Tapped -= OnBackButtonTapRecognizerTapped;
            this.subTitleBar.NextButtonTapRecognizer.Tapped -= NextButtonTapRecognizer_Tapped;
            this.Appearing -= FeelingNowPage_Appearing;
            this.goalsAndDreamsPickerButton.Clicked -= OnGoalsPickerButtonClicked;
            this.goalsAndDreamsPickerButton = null;
            this.actionPickerButton.Clicked -= OnActionPickerButtonClicked;
            this.actionPickerButton = null;
            this.subTitleBar = null;
            this.masterLayout = null;
            // this.deviceSpec = null;
            actionPreviewListSource.Clear();
            actionPreviewListSource = null;
            this.actionPreviewListView = null;
            this.listContainer = null;
            this.slider = null;
            this.selectedGoal = null;
            this.selectedActions = null;

            GC.Collect();
        }
    }

    public class ActionPreviewCellItem : ViewCell
    {
        CustomLayout masterLayout = null;
        Label name = null;
        StackLayout divider = null;
        CustomImageButton deleteButton = null;
        public static FeelingsSecondPage mainPage;

        public ActionPreviewCellItem()
        {
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;
            // IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            name = new Label();
            name.SetBinding(Label.TextProperty, "Name");
            name.TextColor = Color.Gray;
            name.FontSize = Device.OnPlatform(12, 15, 18);
            name.WidthRequest = App.screenWidth * 50 / 100;

            divider = new StackLayout();
            divider.WidthRequest = App.screenWidth;
            divider.HeightRequest = .75;
            divider.BackgroundColor = Color.FromRgb(255, 255, 255);

            deleteButton = new CustomImageButton();
            deleteButton.ImageName = Device.OnPlatform("delete_button.png", "delete_button.png", @"/Assets/delete_button.png");
            deleteButton.WidthRequest = Device.OnPlatform( 20, 20, 60 );
            deleteButton.HeightRequest = Device.OnPlatform(20, 20, 60);
            deleteButton.SetBinding(CustomImageButton.ClassIdProperty, "Name");

            deleteButton.Clicked += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
               {
                   try
                   {
                       CustomImageButton button = sender as CustomImageButton;
                       PreviewItem itemToDel = FeelingsSecondPage.actionPreviewListSource.FirstOrDefault(item => item.Name == button.ClassId);
                       if (itemToDel != null)
                       {
                           mainPage.ShowAlert("Are you sure you want to delete this item ?.", itemToDel);
                       }
                   }
                   catch (Exception ex)
                   {
                       
                   }
               });

            };

            masterLayout.WidthRequest = App.screenWidth;
            masterLayout.HeightRequest = App.screenHeight * Device.OnPlatform(30, 50, 6) / 100;
            masterLayout.AddChildToLayout(name, (float)Device.OnPlatform(5, 5, 5), (float)Device.OnPlatform(5, 5, 25), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(deleteButton, (float)Device.OnPlatform( 80, 80, 75 ), (float)Device.OnPlatform(5, 3.5, 5), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            // masterLayout.AddChildToLayout(divider, (float)1, (float)20, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            this.View = masterLayout;

        }

        public void Dispose()
        {
            this.masterLayout = null;
            this.name = null;
            this.divider = null;
            this.deleteButton = null;

            GC.Collect();
        }
    }
}
