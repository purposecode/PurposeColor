

using Cross;
using CustomControls;
using PurposeColor.interfaces;
using PurposeColor.screens;
using PurposeColor.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace PurposeColor.CustomControls
{


    public class CustomEventView : ContentView, IDisposable
    {
        CustomLayout pageContainedLayout;
        string pageTitle;
        CustomLayout masterLayout;
        Label listTitle;
        IDeviceSpec deviceSpec;
        CustomImageButton startDatePickerButton;
        CustomImageButton endDatePickerButton;
        double screenHeight;
        double screenWidth;

        int topYPos;
        public CustomEventView(CustomLayout containerLayout, int topY, string title, bool titelBarRequired, bool addButtonRequired)
        {
            pageContainedLayout = containerLayout;
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.Transparent;
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;

            pageTitle = title;
            topYPos = topY;

            StackLayout layout = new StackLayout();
            layout.BackgroundColor = Color.Black;
            layout.Opacity = .4;
            layout.WidthRequest = screenWidth;
            layout.HeightRequest = screenHeight;

            TapGestureRecognizer emptyAreaTapGestureRecognizer = new TapGestureRecognizer();
            emptyAreaTapGestureRecognizer.Tapped += (s, e) =>
            {
                View pickView = pageContainedLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                pageContainedLayout.Children.Remove(pickView);
                pickView = null;

            };
            layout.GestureRecognizers.Add(emptyAreaTapGestureRecognizer);


            StackLayout listContainer = new StackLayout();
            listContainer.WidthRequest = screenWidth * 96 / 100;
            listContainer.HeightRequest = screenHeight * topY / 100;
            listContainer.Orientation = StackOrientation.Vertical;


            StackLayout listHeader = new StackLayout();
            listHeader.WidthRequest = screenWidth * 96 / 100;
            listHeader.HeightRequest = screenHeight * 10 / 100;
            listHeader.BackgroundColor = Color.FromRgb(30, 126, 210);

            listTitle = new Label();
            listTitle.Text = title;
            listTitle.TextColor = Color.White;
            listTitle.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            listTitle.FontSize = 17;


            startDatePickerButton = new PurposeColor.interfaces.CustomImageButton();
            startDatePickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
            if (App.SelectedActionStartDate != null && App.SelectedActionStartDate.Trim().Length > 0)
            {
                startDatePickerButton.Text = App.SelectedActionStartDate;
            }
            else
            {
                startDatePickerButton.Text = "Start Date";
            }
           
            startDatePickerButton.FontSize = 18;
            startDatePickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            startDatePickerButton.TextOrientation = interfaces.TextOrientation.Left;
            startDatePickerButton.TextColor = Color.Gray;
            startDatePickerButton.WidthRequest = screenWidth * 90 / 100;
            startDatePickerButton.HeightRequest = screenHeight * 8 / 100;
            startDatePickerButton.Clicked += startDatePickerButton_Clicked;

            endDatePickerButton = new PurposeColor.interfaces.CustomImageButton();
            endDatePickerButton.ImageName = Device.OnPlatform("select_box_whitebg.png", "select_box_whitebg.png", "//Assets//select_box_whitebg.png");
            if (App.SelectedActionStartDate != null && App.SelectedActionStartDate.Trim().Length > 0)
            {
                endDatePickerButton.Text = App.SelectedActionEndDate;
            }
            else
            {
                endDatePickerButton.Text = "End Date";
            }

            endDatePickerButton.FontSize = 18;
            endDatePickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            endDatePickerButton.TextOrientation = interfaces.TextOrientation.Left;
            endDatePickerButton.TextColor = Color.Gray;
            endDatePickerButton.WidthRequest = screenWidth * 90 / 100;
            endDatePickerButton.HeightRequest = screenHeight * 8 / 100;
            endDatePickerButton.Clicked += endDatePickerButton_Clicked;




            this.WidthRequest = screenWidth;
            this.HeightRequest = screenHeight;

            masterLayout.AddChildToLayout(layout, 0, 0);
          /*  if (titelBarRequired)
            {
                masterLayout.AddChildToLayout(listHeader, 2, (100 - topY - 1) - 10);
                masterLayout.AddChildToLayout(listTitle, 5, (100 - topY - 1) - 7);

            }*/

            listContainer.Children.Add(startDatePickerButton);
            listContainer.Children.Add(endDatePickerButton);
            masterLayout.AddChildToLayout(listContainer, 2, 100 - topY - 1);
            this.BackgroundColor = Color.Transparent;

            Content = masterLayout;
        }

        void endDatePickerButton_Clicked(object sender, EventArgs e)
        {
            CalendarView endCalendarView = new CalendarView()
            {
                MinDate = CalendarView.FirstDayOfMonth(DateTime.Now),
                MaxDate = CalendarView.LastDayOfMonth(DateTime.Now.AddMonths(3)),
                HighlightedDateBackgroundColor = Color.FromRgb(227, 227, 227),
                ShouldHighlightDaysOfWeekLabels = false,
                SelectionBackgroundStyle = CalendarView.BackgroundStyle.CircleFill,
                TodayBackgroundStyle = CalendarView.BackgroundStyle.CircleOutline,
                HighlightedDaysOfWeek = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday },
                ShowNavigationArrows = true,
                MonthTitleFont = Font.OfSize("Open 24 Display St", NamedSize.Medium)

            };
            endCalendarView.ClassId = "endcalendar";
            endCalendarView.DateSelected += OnEndCalendarViewDateSelected;
            masterLayout.AddChildToLayout(endCalendarView, 0, 20);
        }

        void OnEndCalendarViewDateSelected(object sender, DateTime e)
        {
            endDatePickerButton.Text = e.Year.ToString() + e.Month.ToString() + e.Day.ToString();
            App.SelectedActionEndDate = endDatePickerButton.Text;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "endcalendar");
            masterLayout.Children.Remove(pickView);
            pickView = null;
        }

        void startDatePickerButton_Clicked(object sender, EventArgs e)
        {
            CalendarView startCalendarView = new CalendarView()
            {
                MinDate = CalendarView.FirstDayOfMonth(DateTime.Now),
                MaxDate = CalendarView.LastDayOfMonth(DateTime.Now.AddMonths(3)),
                HighlightedDateBackgroundColor = Color.FromRgb(227, 227, 227),
                ShouldHighlightDaysOfWeekLabels = false,
                SelectionBackgroundStyle = CalendarView.BackgroundStyle.CircleFill,
                TodayBackgroundStyle = CalendarView.BackgroundStyle.CircleOutline,
                HighlightedDaysOfWeek = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday },
                ShowNavigationArrows = true,
                MonthTitleFont = Font.OfSize("Open 24 Display St", NamedSize.Medium)

            };
            startCalendarView.ClassId = "startcalendar";
            startCalendarView.DateSelected += startCalendarView_DateSelected;
            masterLayout.AddChildToLayout(startCalendarView, 0,20);
        }

        void startCalendarView_DateSelected(object sender, DateTime e)
        {
            startDatePickerButton.Text = e.Year.ToString() + e.Month.ToString() + e.Day.ToString();
            App.SelectedActionStartDate = startDatePickerButton.Text;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "startcalendar");
            masterLayout.Children.Remove(pickView);
            pickView = null;
        }

       


        public void Dispose()
        {
            pageContainedLayout = null;
            pageTitle = null;
            masterLayout = null;
            listTitle = null;
            deviceSpec = null;
            GC.Collect();
        }
    }
}

