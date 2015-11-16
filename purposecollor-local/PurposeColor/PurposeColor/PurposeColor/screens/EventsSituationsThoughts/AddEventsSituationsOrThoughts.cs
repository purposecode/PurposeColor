using CustomControls;
using PurposeColor.CustomControls;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class AddEventsSituationsOrThoughts : ContentPage
    {
        public AddEventsSituationsOrThoughts()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(230, 255, 254);

            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Color.FromRgb(12, 113, 210), "Emotional Awareness", false);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(2, 4, 4));

            Editor textInput = new Editor
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            masterLayout.AddChildToLayout(textInput, 10, 20);
            Entry entry = new Entry
            {
                Placeholder = "placeholder"
            };

            Content = masterLayout;
        }
    }
}
