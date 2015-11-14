using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class PieGraphPage : ContentPage
    {
        /// <summary>
        /// The plot view.
        /// </summary>
        private OxyPlotView plotView;

        /// <summary>
        /// grah model
        /// </summary>
        public CustomeGraphModel graphModel { get; set; }

        public PieGraphPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            masterLayout.BackgroundColor = Color.FromRgb(230, 255, 254);

            #region MAINTITLE BAR

            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            #endregion

            #region SUBTITLE BAR

            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Color.FromRgb(12, 113, 210), "Emotional intelligence");
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            
            #endregion

            #region OXYPLOT VIEW

            plotView = new OxyPlotView();
            plotView.HeightRequest = 300;
            plotView.WidthRequest = 300;
            plotView.BackgroundColor = Color.Transparent;

            graphModel = new CustomeGraphModel();
            plotView.Model = graphModel.plotModel;
            plotView.VerticalOptions = LayoutOptions.StartAndExpand;
            plotView.HorizontalOptions = LayoutOptions.StartAndExpand;
            #endregion

            masterLayout.AddChildToLayout(plotView, 0, 30);

            Content = masterLayout;

        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }
    }
}
