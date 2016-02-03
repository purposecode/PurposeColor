using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using Xamarin.Forms;
using PurposeColor.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace PurposeColor.screens
{
	public class PieGraphPage : ContentPage, IDisposable
	{
		private OxyPlotView plotView;
		CustomLayout masterLayout = null;

		public CustomeGraphModel graphModel { get; set; }
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

		Label region1WarmLabel = null;
		Label region2AssertiveLabel = null;
		Label region3PatientLabel = null;
		Label region4DetailedLabel = null;

		List<EmotionValues> emotionList = null;
		int currentSubMenuDisplaying = 0;
		StackLayout graphAndEmotionListContainer = null;
		StackLayout BottomStackContainer = null;


		public PieGraphPage()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
			progressBar = DependencyService.Get<IProgressBar>();

			this.Appearing += OnGraphPageAppearing;

			mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
			subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Intellegence", false);
			subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;
			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));

			graphAndEmotionListContainer = new StackLayout
			{
				Spacing = 0,
				Orientation = StackOrientation.Vertical
			};

			Label headingLabel = new Label
			{
				Text = "My Emotional Zone",
				FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
				TextColor = Constants.BLUE_BG_COLOR, //Color.FromRgb(40, 47, 50),
				FontSize = Device.OnPlatform(12, 16, 20)

			};

			Image filter = new Image {
				Source = "filter.png",
				HeightRequest = 25,
				WidthRequest = 25,
				Aspect = Aspect.Fill
			};
			TapGestureRecognizer filterTapRecognizer = new TapGestureRecognizer();
			filter.GestureRecognizers.Add(filterTapRecognizer);
			filterTapRecognizer.Tapped += async (sender, e) => {
				await DisplayAlert(Constants.ALERT_TITLE,"Filter option to be implemented",Constants.ALERT_OK);
			};

			graphAndEmotionListContainer.Children.Add(new StackLayout{ Spacing = 50, Padding  = new Thickness(80,0,10,0), Orientation = StackOrientation.Horizontal , Children = {headingLabel,filter}});


			#region REGION 1 BUTTON STACK


			region1WarmLabel = new Label
			{
				Text = "Warm",
				WidthRequest = 180, 
				FontSize = Device.OnPlatform(14, 16, 20 ),
				VerticalOptions = LayoutOptions.End,
				TextColor = Color.FromRgb(40, 47, 50),
			};

			region1DownArrow = new Image
			{
				Source = "downarrow",
				HeightRequest = 20,
				WidthRequest = 20
			};
			Image region1Icon = new Image
			{
				Source = "ic_red",
				HeightRequest = 25,
				WidthRequest = 25
			};

			StackLayout region1Button = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = Device.OnPlatform(20, 10, 40),
				Padding = new Thickness(10,10,10,1),
				Children = { region1Icon, region1WarmLabel, region1DownArrow }
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


			region2AssertiveLabel = new Label
			{
				Text = "Assertive",
				WidthRequest = 180,
				FontSize = Device.OnPlatform(14, 16, 20 ),
				VerticalOptions = LayoutOptions.End,
				TextColor = Color.FromRgb(40, 47, 50),
			};
			region2DownArrow = new Image
			{
				Source = "downarrow",
				HeightRequest = 20,
				WidthRequest = 20
			};
			Image region2Icon = new Image
			{
				Source = "ic_orge",
				HeightRequest = 25,
				WidthRequest = 25
			};
			StackLayout region2Button = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = Device.OnPlatform(20, 10, 40),
				Padding = new Thickness(10,10,10,1),
				Children = { region2Icon, region2AssertiveLabel, region2DownArrow }
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
				HeightRequest = 20,
				WidthRequest = 20
			};

			region3PatientLabel = new Label
			{
				Text = "Patient",
				WidthRequest = 180,
				FontSize = Device.OnPlatform(14, 16, 20 ),
				VerticalOptions = LayoutOptions.End,
				TextColor = Color.FromRgb(40, 47, 50),
			};
			Image region3Icon = new Image
			{
				Source = "ic_green",
				HeightRequest = 25,
				WidthRequest = 25
			};

			StackLayout region3Button = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = Device.OnPlatform(20, 10, 40),
				Padding = new Thickness(10,10,10,1),
				Children = { region3Icon, region3PatientLabel, region3DownArrow }
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
				HeightRequest = 20,
				WidthRequest = 20
			};


			region4DetailedLabel = new Label
			{
				Text = "Detailed",
				WidthRequest = 180,
				FontSize = Device.OnPlatform(14, 16, 20 ),
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.FromRgb(40, 47, 50),
			};
			Image region4Icon = new Image
			{
				Source = "ic_blu",
				HeightRequest = 25,
				WidthRequest = 25
			};


			StackLayout region4Button = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = Device.OnPlatform(20, 10, 40),
				Padding = new Thickness(10,10,10,1),
				Children = { region4Icon, region4DetailedLabel, region4DownArrow }
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

			BottomStackContainer = new StackLayout
			{
				// add all buttons to this container.
				BackgroundColor = Color.Transparent,
				//Padding = 10,
				WidthRequest = App.screenWidth * .80,
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness(0,0,0,10),
				Children = { new StackLayout {Orientation = StackOrientation.Vertical, Padding = new Thickness(Device.OnPlatform(5, 10, 10),0,0,10), Spacing = 5, Children = {region4ButtonStack, region3ButtonStack, region2ButtonStack, region1ButtonStack} }} // 1. red, 2. blue
			};

			ScrollView contentScrool = new ScrollView {
				HeightRequest = App.screenHeight - (App.screenHeight * .20),
				WidthRequest = App.screenWidth,
				Content = graphAndEmotionListContainer,
				IsClippedToBounds = true,
				Orientation = ScrollOrientation.Vertical
			};

			masterLayout.AddChildToLayout (contentScrool, 0, 18);

			Content = new StackLayout {
				HeightRequest = App.screenHeight,
				WidthRequest = App.screenWidth,
				Children = { masterLayout }
			};
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

		private async void ShowEmotionList(int regionCode)
		{
			try
			{
				//remove sub list from from display//
				var stck1 = region1ButtonStack.Children.FirstOrDefault(c => c.ClassId == "emotionSubContainer1");
				if (stck1 != null)
				{
					await stck1.FadeTo(0, 100, Easing.CubicOut);
					region1ButtonStack.Children.Remove(stck1);
					region1DownArrow.Rotation = 0;
				}
				var stck2 = region2ButtonStack.Children.FirstOrDefault(c => c.ClassId == "emotionSubContainer2");
				if (stck2 != null)
				{
					await stck2.FadeTo(0, 100, Easing.CubicOut);
					region2ButtonStack.Children.Remove(stck2);
					region2DownArrow.Rotation = 0;
				}
				var stck3 = region3ButtonStack.Children.FirstOrDefault(c => c.ClassId == "emotionSubContainer3");
				if (stck3 != null)
				{
					await stck3.FadeTo(0, 100, Easing.CubicOut);
					region3ButtonStack.Children.Remove(stck3);
					region3DownArrow.Rotation = 0;
				}
				var stck4 = region4ButtonStack.Children.FirstOrDefault(c => c.ClassId == "emotionSubContainer4");
				if (stck4 != null)
				{
					await stck4.FadeTo(0,100, Easing.CubicOut);
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

				if (emotionList == null) {
					return;
				}
				List<EmotionValues> currentRegionEmotions = null;

				// show emotion sub list according to the region parameter
				switch (regionCode)
				{
				case 1: // red - Warm
					currentRegionEmotions = emotionList.Where(e => e.emotion_value == "-2").ToList();
					currentSubMenuDisplaying = 1;
					break;
				case 2: // orange - Assertive
					currentRegionEmotions = emotionList.Where(e => e.emotion_value == "-1").ToList();
					currentSubMenuDisplaying = 2;
					break;
				case 3: // green - Patient
					currentRegionEmotions = emotionList.Where(e => e.emotion_value == "1").ToList();
					currentSubMenuDisplaying = 3;
					break;
				case 4: // blue - Detailed
					currentRegionEmotions = emotionList.Where(e => e.emotion_value == "2").ToList();
					currentSubMenuDisplaying = 4;
					break;
				default:
					break;
				}

				if (currentRegionEmotions == null || currentRegionEmotions.Count < 1 )
				{
					return;
				}

				StackLayout subEmotionsContainer = new StackLayout {Orientation = StackOrientation.Vertical, Spacing = 5, Padding = new Thickness(Device.OnPlatform(55, 47, 30),0,0,2), ClassId = "emotionSubContainer" + regionCode };
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

				foreach (EmotionValues emmotionVal in currentRegionEmotions)
				{
					try {
						if (!string.IsNullOrEmpty(emmotionVal.emotion_title))
						{
							Label emotionLabel = new Label { Text = emmotionVal.emotion_title.Trim() + " ("+emmotionVal.count.ToString() + ")", FontSize = Device.OnPlatform(12, 15, 18), TextColor = Constants.TEXT_COLOR_GRAY, HorizontalOptions = LayoutOptions.Start };//, WidthRequest = 90

							TapGestureRecognizer emotionlabelRecognizer = new TapGestureRecognizer();
							emotionLabel.GestureRecognizers.Add(emotionlabelRecognizer);
							emotionlabelRecognizer.Tapped += EmotionlabelRecognizer_Tapped;
							emotionLabel.ClassId = emmotionVal.emotion_id.ToString();

							subEmotionsContainer.Children.Add(emotionLabel);

							await Task.Delay(10);
						}
					} catch (System.Exception ex) {

					}
				}
				//subEmotionsContainer.Children.Add(new BoxView{BackgroundColor = Color.Black, WidthRequest = App.screenWidth, HeightRequest = 1});
			}
			catch (System.Exception ex)
			{
				var test = ex.Message;
			}
		}

		async void EmotionlabelRecognizer_Tapped (object sender, EventArgs e)
		{
			try {
				
				Label selectedLabel = sender as Label;
				if (selectedLabel != null) {

					bool doAdd = await DisplayAlert (Constants.ALERT_TITLE, "Would you like to add this to Supporting emotions?", "Add", "Cancel");
					if (doAdd) {
						progressBar.ShowProgressbar("Adding to supporting emotions");
						string emotionId = selectedLabel.ClassId;
						// call api - addtosupportemotion
						string serviceResult = await PurposeColor.Service.ServiceHelper.Addtosupportemotion(selectedLabel.ClassId);
						progressBar.HideProgressbar ();
						if (serviceResult == "200") {
							await DisplayAlert(Constants.ALERT_TITLE,"Added to Supporting emotions.",Constants.ALERT_OK);
						}else{
							await DisplayAlert(Constants.ALERT_TITLE,"Network error, please try again later.",Constants.ALERT_OK);
						}
					}
				}
			} catch (Exception ex) {
				DisplayAlert(Constants.ALERT_TITLE,"Network error, please try again later.",Constants.ALERT_OK);
			}
			progressBar.HideProgressbar ();
		}

		void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
		{
			App.masterPage.IsPresented = !App.masterPage.IsPresented;
		}

		void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
		{
			App.masterPage.IsPresented = !App.masterPage.IsPresented;
		}

		async void OnGraphPageAppearing(object sender, System.EventArgs e)
		{
			bool isValueReceied = false;
			try {

				progressBar.ShowProgressbar ("Analysing your emotions.");

				AllEmotions getEmotionsResult = await PurposeColor.Service.ServiceHelper.GetEmotionsDetailsForGraph ("2"); // user id for testing only.. in actual get it from app.settings.

				if (getEmotionsResult.resultarray != null) 
				{
					emotionList = getEmotionsResult.resultarray;
					isValueReceied = true;

					#region OXYPLOT VIEW

					plotView = new OxyPlotView();
					plotView.HeightRequest = App.screenWidth * .80;//.60;//300;
					plotView.WidthRequest = App.screenWidth * .80; //.60; // H & W same to keep it square. //300;
					plotView.BackgroundColor = Color.Transparent;

					graphModel = new CustomeGraphModel(emotionList);
					plotView.Model = graphModel.plotModel;
					plotView.VerticalOptions = LayoutOptions.Center;
					plotView.HorizontalOptions = LayoutOptions.Center;

					graphAndEmotionListContainer.Children.Add(plotView);
					graphAndEmotionListContainer.Children.Add(BottomStackContainer);

					StackLayout emptySpacingAtBottom = new StackLayout
					{
						HeightRequest = 50,
						BackgroundColor = Color.Transparent
					};
					graphAndEmotionListContainer.Children.Add(emptySpacingAtBottom);

					#endregion

					region1WarmLabel.Text = "Warm (" + getEmotionsResult.warm_percent+ "%)";
					region2AssertiveLabel.Text = "Assertive (" + getEmotionsResult.assertive_percent + "%)";
					region3PatientLabel.Text = "Patient (" + getEmotionsResult.patient_percent + "%)";
					region4DetailedLabel.Text = "Detailed (" + getEmotionsResult.detailed_percent + "%)";

					isValueReceied = true;
				}
				else
				{
					isValueReceied = false;
					emotionList = null;
				}


			} 
			catch (System.Exception ex) {
			}
			progressBar.HideProgressbar ();
			if(!isValueReceied)
			{
				await DisplayAlert (Constants.ALERT_TITLE, "Network error, please try again later.", Constants.ALERT_OK);
			}
		}

		public void Dispose()
		{
			GC.Collect ();
		}
	}
}
