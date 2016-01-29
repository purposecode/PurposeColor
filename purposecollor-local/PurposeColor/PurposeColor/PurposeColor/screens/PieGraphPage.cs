using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using Xamarin.Forms;
using PurposeColor.interfaces;

namespace PurposeColor.screens
{
    public class PieGraphPage : ContentPage
    {
        /// <summary>
        /// The plot view.
        /// </summary>
        private OxyPlotView plotView;
		CustomLayout masterLayout = null;

        /// <summary>
        /// grah model
        /// </summary>
        public CustomeGraphModel graphModel { get; set; }
		double screenHeight;
		double screenWidth;
		IProgressBar progressBar = null;
		PurposeColorSubTitleBar subTitleBar = null;
		PurposeColorTitleBar mainTitleBar = null;

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
			subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Intellegence");
			subTitleBar.NextButtonTapRecognizer.Tapped += OnNextButtonTapRecognizerTapped;
			subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;
			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));

			Label headingLabel = new Label {
				Text = "My Emotional Zone",
				FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
				TextColor = Color.FromRgb(40, 47, 50),
				FontSize = Device.OnPlatform(20, 20, 30)

			};
			masterLayout.AddChildToLayout(headingLabel , 10, 20);

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

            masterLayout.AddChildToLayout(plotView, 10, 27);

			#region List views
			StackLayout ListCoontainer = new StackLayout
			{
				// add all buttons to this container.
			};

			StackLayout warmButtonStack = new StackLayout
			{
				// add all warm button elements to this container.
				// head icon, label and arrow icon. rotate the arrow icon 180 degree on each click.
			};

			#endregion



            Content = masterLayout;

        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

		async void OnNextButtonTapRecognizerTapped(object sender, System.EventArgs e)
		{
			// what to do?
		}

		void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
		{
			App.masterPage.IsPresented = !App.masterPage.IsPresented;
		}
    }
}
