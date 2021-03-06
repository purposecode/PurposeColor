﻿using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class CreateEventPage : ContentPage, IDisposable
    {
        CustomLayout masterLayout;
        IDeviceSpec deviceSpec;
        public CreateEventPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            deviceSpec = DependencyService.Get<IDeviceSpec>();

			PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Create Reminder", false, true);
            subTitleBar.BackButtonTapRecognizer.Tapped += BackButtonTapRecognizer_Tapped;

			CustomImageButton startDatePickerButton = new CustomImageButton();
			CustomImageButton startTimePickerButton = new CustomImageButton();
			CustomImageButton endDatePickerButton = new CustomImageButton();
			CustomImageButton endTimePickerButton = new CustomImageButton();


            CustomEntry title = new CustomEntry();
			title.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
			title.Placeholder = " Title";
			title.TextColor = Color.Black;
			title.BackgroundColor = Color.White;


            CustomEntry messege = new CustomEntry();
			messege.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
			messege.Placeholder = " Description";
			messege.TextColor = Color.Black;
			messege.BackgroundColor = Color.White;


			DatePicker startDatePicker = new DatePicker();
			startDatePicker.WidthRequest = 0;
			startDatePicker.HeightRequest = 0;
			startDatePicker.IsVisible = false;
			startDatePicker.DateSelected += (object sender, DateChangedEventArgs e) => 
			{
				
				startDatePickerButton.Text = startDatePicker.Date.ToString("dd/MM/yyyy");
			};

			DatePicker endDatePicker = new DatePicker();
			endDatePicker.WidthRequest = 0;
			endDatePicker.HeightRequest = 0;
			endDatePicker.IsVisible = false;
			endDatePicker.DateSelected += (object sender, DateChangedEventArgs e) => 
			{
				endDatePickerButton.Text = endDatePicker.Date.ToString("dd/MM/yyyy");
			};

			TimePicker startTimePicker = new TimePicker();
			startTimePicker.WidthRequest = 0;
			startTimePicker.HeightRequest = 0;
			startTimePicker.IsVisible = false;
			startTimePicker.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => 
			{
                try
                {

                    if ("Time" == e.PropertyName)
                    {
                        //	string tie = startTimePicker.Time.ToString("hh:mm tt");
                        string amPM = (startTimePicker.Time.Hours > 12) ? "PM" : "AM";
                        startTimePickerButton.Text = startTimePicker.Time.ToString(); //startTimePicker.Time.Hours.ToString () + " : " + startTimePicker.Time.Minutes.ToString() + "  " + amPM;

                    }

                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
			};


			TimePicker endTimePicker = new TimePicker();
			endTimePicker.WidthRequest = 0;
			endTimePicker.HeightRequest = 0;
			endTimePicker.IsVisible = false;
			endTimePicker.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => 
			{
                try
                {
                    if ("Time" == e.PropertyName)
                    {
                        string amPM = (endTimePicker.Time.Hours > 12) ? "PM" : "AM";
                        endTimePickerButton.Text = endTimePicker.Time.ToString();// endTimePicker.Time.Hours.ToString () + " : " + endTimePicker.Time.Minutes.ToString() + "  " + amPM;
                    }
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
			};

            //startDatePickerButton = new CustomImageButton();
            startDatePickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
            startDatePickerButton.Text = "Start Date";
            startDatePickerButton.FontSize = 17;
            startDatePickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            startDatePickerButton.TextOrientation = interfaces.TextOrientation.Left;
            startDatePickerButton.TextColor = Color.Gray;
            startDatePickerButton.WidthRequest = deviceSpec.ScreenWidth * 40 / 100;
			startDatePickerButton.Clicked += (object sender, EventArgs e) => 
			{
                try
                {
					startDatePicker.Date = DateTime.Now.AddDays(1);
                    startDatePicker.Focus();
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
			};



           // startTimePickerButton = new CustomImageButton();
            startTimePickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
            startTimePickerButton.Text = " Start Time";
            startTimePickerButton.TextOrientation = TextOrientation.Middle;         
            startTimePickerButton.FontSize = 17;
            startTimePickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            startTimePickerButton.TextOrientation = interfaces.TextOrientation.Left;
            startTimePickerButton.TextColor = Color.Gray;
            startTimePickerButton.WidthRequest = deviceSpec.ScreenWidth * 40 / 100;
			startTimePickerButton.Clicked += (object sender, EventArgs e) => 
			{
                try
                {
                    startTimePicker.Time = new TimeSpan(12, 00, 00);
                    startTimePicker.Focus();
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
			};
		
		
		   // endDatePickerButton = new CustomImageButton();
            endDatePickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
			endDatePickerButton.Text = "End Date";
			endDatePickerButton.FontSize = 17;
			endDatePickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
			endDatePickerButton.TextOrientation = interfaces.TextOrientation.Left;
			endDatePickerButton.TextColor = Color.Gray;
			endDatePickerButton.WidthRequest = deviceSpec.ScreenWidth * 40 / 100;
			endDatePickerButton.Clicked += (object sender, EventArgs e) => 	
			{
                try
                {
					endDatePicker.Date = DateTime.Now.AddDays(1);
                    endDatePicker.Focus();
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
			};


		   // endTimePickerButton = new CustomImageButton();
            endTimePickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
			endTimePickerButton.Text = " End Time";
			endTimePickerButton.FontSize = 17;
			endTimePickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
			endTimePickerButton.TextOrientation = interfaces.TextOrientation.Left;
			endTimePickerButton.TextColor = Color.Gray;
			endTimePickerButton.WidthRequest = deviceSpec.ScreenWidth * 40 / 100;
			endTimePickerButton.Clicked += (object sender, EventArgs e) => 	
			{
                try
                {
                    endTimePicker.Time = new TimeSpan(12, 00, 00);
                    endTimePicker.Focus();
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
			};

			CustomImageButton reminderPickerButton = new CustomImageButton ();
			Picker reminderPicker = new Picker ();
			reminderPicker.Items.Add ("15");
			reminderPicker.Items.Add ("30");
			reminderPicker.Items.Add ("45");
			reminderPicker.Items.Add ("60");
			reminderPicker.WidthRequest = 0;
			reminderPicker.HeightRequest = 0;
			reminderPicker.SelectedIndexChanged += (object sender, EventArgs e) => 
			{
				reminderPickerButton.Text = reminderPicker.Items[reminderPicker.SelectedIndex];
			};


            reminderPickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
			reminderPickerButton.Text = " Reminder";
			reminderPickerButton.TextOrientation = TextOrientation.Middle;         
			reminderPickerButton.FontSize = 17;
			reminderPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
			reminderPickerButton.TextOrientation = interfaces.TextOrientation.Left;
			reminderPickerButton.TextColor = Color.Gray;
			reminderPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
			reminderPickerButton.Clicked += (object sender, EventArgs e) => 
			{
				reminderPicker.Focus();
			};

			Button createReminderButton = new Button ();
			createReminderButton.Text = "Create Reminder";
			createReminderButton.TextColor = Color.White;
			createReminderButton.BackgroundColor = Color.FromRgb( 30, 126, 210 );
			createReminderButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
			createReminderButton.Clicked += async (object sender, EventArgs e) =>
			{
                try
                {

                    App.CalPage = this;
                    if (startDatePickerButton.Text == "Start Date" || endDatePickerButton.Text == "End Date")
                    {
                        await DisplayAlert(Constants.ALERT_TITLE, "Please select start date and end date to proceed", Constants.ALERT_OK);
                        return;
                    }
                    IReminderService reminder = DependencyService.Get<IReminderService>();

                    if (Device.OS == TargetPlatform.iOS)
                    {
                        var access = await reminder.RequestAccessAsync();
                        if (!access)
                            return;
                    }

                    int reminderValue = 0;
                    if (reminderPickerButton.Text != null && reminderPickerButton.Text.Length > 0 && reminderPicker.SelectedIndex >= 0)
                    {
                        reminderValue = Convert.ToInt32(reminderPickerButton.Text);
                    }

                    DateTime startDateAndTime = new DateTime(startDatePicker.Date.Year, startDatePicker.Date.Month, startDatePicker.Date.Day, startTimePicker.Time.Hours, startTimePicker.Time.Minutes, 0);
                    DateTime endDateAndTime = new DateTime(endDatePicker.Date.Year, endDatePicker.Date.Month, endDatePicker.Date.Day, endTimePicker.Time.Hours, endTimePicker.Time.Minutes, 0);

                    // add this to app datetime fields to serve to api.
                    App.SelectedActionStartDate = startDateAndTime.ToString(); //to be converted into UTC
                    App.SelectedActionEndDate = endDateAndTime.ToString(); //to be converted into UTC
                    App.SelectedActionReminderValue = reminderValue;
                    if (!reminder.Remind(startDateAndTime, endDateAndTime, title.Text, messege.Text, reminderValue))
                    {
                        await DisplayAlert("Purpose Color", "Error in creating calendar event", Constants.ALERT_OK);
                    }
                    else
                    {
                        IProgressBar progress = DependencyService.Get<IProgressBar>();
                        progress.ShowToast("Calander event created");
                        if (Device.OS != TargetPlatform.iOS)
                        { //Navigation.PopAsync(); 
                            await Navigation.PopModalAsync();
                        }

                    }

                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
			};

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));

			masterLayout.AddChildToLayout(startDatePicker, 0, 0);
            masterLayout.AddChildToLayout(startTimePicker, 0, 0);
			masterLayout.AddChildToLayout(endDatePicker, 0, 0);
			masterLayout.AddChildToLayout(endTimePicker, 0, 0);
			masterLayout.AddChildToLayout(reminderPicker, 0, 0);

			masterLayout.AddChildToLayout (title, 5, 20);
			masterLayout.AddChildToLayout(messege, 5, 30);
			masterLayout.AddChildToLayout(startDatePickerButton, 5, 40);
			masterLayout.AddChildToLayout(startTimePickerButton, 55, 40);
			masterLayout.AddChildToLayout(endDatePickerButton, 5, 50);
			masterLayout.AddChildToLayout(endTimePickerButton, 55, 50);
			masterLayout.AddChildToLayout(reminderPickerButton, 5, 70);
			masterLayout.AddChildToLayout(createReminderButton, 5, 80);
            Content = masterLayout;
        }

        void BackButtonTapRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
            }
        }

        void startDatePickerButton_Clicked(object sender, EventArgs e)
        {

        }

        void timePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string test = "test";
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }
			

        public void Dispose()
        {

        }
    }
}
