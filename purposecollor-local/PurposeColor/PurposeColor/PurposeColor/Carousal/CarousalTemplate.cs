using System;
using Xamarin.Forms;

namespace CustomLayouts
{
	public class CarousalTemplate : ContentView
	{
        public CarousalTemplate()
		{
			BackgroundColor = Color.White;

			var label = new Label {
				XAlign = TextAlignment.Center,
				TextColor = Color.Black
			};

			label.SetBinding(Label.TextProperty, "Title");
            this.SetBinding(ContentView.BackgroundColorProperty, "Background");

            Image img = new Image();
            img.Source = "manali.jpg";
            img.Aspect = Aspect.Fill;

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					img
				}
			};
		}
	}
}

