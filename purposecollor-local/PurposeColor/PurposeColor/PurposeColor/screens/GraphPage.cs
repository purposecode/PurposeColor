using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class GraphPage : ContentPage, IDisposable
    {
        StackLayout canvas;
        CustomLayout masterLayout;
        double canvasXPos;
        double canvasYPos;
        Entry YEntry;
        Entry XEntry;


        const int CANVAS_X_POS_PERCENTAGE = 5;
        const int CANVAS_Y_POS_PERCENTAGE = 25;
        const int ANDROID_GRAPH_OFFSET = 5;
        const int WINDOWS_GRAPH_OFFSET = 20;
		const int IOS_GRAPH_OFFSET = 5;

        public GraphPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            PurposeColorTitleBar titleBar = new PurposeColorTitleBar(Color.FromRgb(8, 137, 216), "Purpose Color", Color.Black, "back");

            canvas = new StackLayout();
            canvas.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            canvas.HeightRequest = deviceSpec.ScreenHeight * 50 / 100;
            canvas.BackgroundColor = Color.Aqua;

            StackLayout yAxis = new StackLayout();
            yAxis.BackgroundColor = Color.Black;
            yAxis.WidthRequest = 1;
            yAxis.HeightRequest = deviceSpec.ScreenHeight * 50 / 100;

            StackLayout xAxis = new StackLayout();
            xAxis.BackgroundColor = Color.Black;
            xAxis.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            xAxis.HeightRequest = 1;

            RoundedButton button = new RoundedButton();
            button.BackgroundColor = Color.Red;
            button.WidthRequest = 5;
            button.HeightRequest = 5;

            canvasXPos = deviceSpec.ScreenWidth * CANVAS_X_POS_PERCENTAGE / 100;
            canvasYPos = deviceSpec.ScreenHeight * CANVAS_Y_POS_PERCENTAGE / 100;

            masterLayout.AddChildToLayout(canvas, CANVAS_X_POS_PERCENTAGE, CANVAS_Y_POS_PERCENTAGE);
            masterLayout.AddChildToLayout(yAxis, 50, 25);
            masterLayout.AddChildToLayout(xAxis, 5, 50);

            Point point1 = new Point(1.75, 1.75);
            Point point2 = new Point(1.75, -1.75);
            Point point3 = new Point(-1.75, 1.75);
            Point point4 = new Point(-1.75, -1.75);

            List<Point> pointList = new List<Point>();
            pointList.Add( point1 );
            pointList.Add(point2);
            pointList.Add(point3);
            pointList.Add(point4);

            XEntry = new Entry
            {
                Placeholder = "X Pos",
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 100
            };


            YEntry = new Entry
            {
                Placeholder = "Y Pos",
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                WidthRequest = 100
            };


            Button submit = new Button
            {
                Text = "Draw",
                BackgroundColor = Color.Red,
                WidthRequest = 150,
                TextColor = Color.White
            };
            submit.Clicked += OnSubmitClicked;

            masterLayout.AddChildToLayout(titleBar, 0, 0);
            masterLayout.AddChildToLayout(XEntry, 5, 15);
            masterLayout.AddChildToLayout(YEntry, 25,15);
            masterLayout.AddChildToLayout(submit, 55, 15);
            CreateGraphFromPoints(pointList);
           // masterLayout.Children.Add(button, new Point(100, 150));
            masterLayout.BackgroundColor = Color.White;
            Content = masterLayout;
        }

        void OnSubmitClicked(object sender, EventArgs e)
        {    
            List<Point> list = new List<Point>{ new Point( Convert.ToDouble( XEntry.Text  ), Convert.ToDouble( YEntry.Text ) ) };
            CreateGraphFromPoints(list);
        }

        private void CreateGraphFromPoints(  List<Point> points )
        {
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            double singlePointPixelWidth = canvas.WidthRequest / 4;
            double singlePointPixelHeight = canvas.HeightRequest / 4;

 
            for( int index = 0; index < points.Count; index ++ )
            {
                double xPoint = 0;
                double yPoint = 0;
                double xCenter = canvas.WidthRequest / 2;
                double yCenter = canvas.HeightRequest / 2;
                Point currenPoint = points[index];
                double currentAbsX = Math.Abs(currenPoint.X);
                double currentAbsY = Math.Abs(currenPoint.Y);

                if (currenPoint.X > 0 && currenPoint.Y > 0)
                {
					xPoint = xCenter + currentAbsX * singlePointPixelWidth - Device.OnPlatform( IOS_GRAPH_OFFSET, ANDROID_GRAPH_OFFSET,WINDOWS_GRAPH_OFFSET );
                    yPoint = yCenter - currentAbsY * singlePointPixelHeight - Device.OnPlatform(IOS_GRAPH_OFFSET, ANDROID_GRAPH_OFFSET, WINDOWS_GRAPH_OFFSET); ;
                }
                else if (currenPoint.X > 0 && currenPoint.Y < 0)
                {
					xPoint = xCenter + currentAbsX * singlePointPixelWidth - Device.OnPlatform(IOS_GRAPH_OFFSET, ANDROID_GRAPH_OFFSET, WINDOWS_GRAPH_OFFSET);
                    yPoint = yCenter + Math.Abs(currenPoint.Y) * singlePointPixelHeight - Device.OnPlatform(IOS_GRAPH_OFFSET, ANDROID_GRAPH_OFFSET, WINDOWS_GRAPH_OFFSET);
                }
                else if (currenPoint.X < 0 && currenPoint.Y < 0)
                {
					xPoint = xCenter - currentAbsX * singlePointPixelWidth - Device.OnPlatform(IOS_GRAPH_OFFSET, ANDROID_GRAPH_OFFSET, WINDOWS_GRAPH_OFFSET);
                    yPoint = yCenter + currentAbsY * singlePointPixelHeight - Device.OnPlatform(IOS_GRAPH_OFFSET, ANDROID_GRAPH_OFFSET, WINDOWS_GRAPH_OFFSET);
                }
                else
                {
					xPoint = xCenter - currentAbsX * singlePointPixelWidth - Device.OnPlatform(IOS_GRAPH_OFFSET, ANDROID_GRAPH_OFFSET, WINDOWS_GRAPH_OFFSET);
                    yPoint = yCenter - currentAbsY * singlePointPixelHeight - Device.OnPlatform(IOS_GRAPH_OFFSET, ANDROID_GRAPH_OFFSET, WINDOWS_GRAPH_OFFSET); ;
                }

                RoundedButton button = new RoundedButton();
                button.BorderColor = Color.Transparent;
                button.BackgroundColor = Color.Red;
                button.WidthRequest = Device.OnPlatform( 15,20,40 );
                button.HeightRequest = Device.OnPlatform( 15,20,40 );
                button.ClassId = currenPoint.X.ToString() + " , " + currenPoint.Y.ToString();
                button.Clicked += OnRoundedButtonClicked;
                masterLayout.Children.Add(button, new Point(canvasXPos + xPoint, canvasYPos + yPoint));
            }
         
        }

        void OnRoundedButtonClicked(object sender, EventArgs e)
        {
            RoundedButton button = sender as RoundedButton;
            DisplayAlert("Alert", button.ClassId, "OK");

            App.masterPage.IsPresented = false;
            App.masterPage.Detail = new NavigationPage(new PieGraphPage());

        }

        public void Dispose()
        { 
            canvas = null;
            masterLayout = null;
            XEntry = null;
            YEntry = null;
            GC.Collect();
        }
        
    }
}
