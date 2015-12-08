using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.screens
{

    public class VeggieViewModel 
    {

        public string Name { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public VeggieViewModel()
        {
        }

    }


    public class CustomListView : ContentPage
    {
        public ObservableCollection<VeggieViewModel> veggies { get; set; }
        public CustomListView()
        {
            veggies = new ObservableCollection<VeggieViewModel>();
            ListView lstView = new ListView();
            lstView.RowHeight = 60;
            lstView.ItemsSource = veggies;
            lstView.ItemTemplate = new DataTemplate(typeof(CustomVeggieCell));
            
            Content = lstView;
        }
    }

    public class CustomVeggieCell : ViewCell
    {
        public CustomVeggieCell()
        {
            AbsoluteLayout cellView = new AbsoluteLayout() { BackgroundColor = Color.Olive };
            var nameLabel = new Label();
            nameLabel.SetBinding(Label.TextProperty, "Name");
            AbsoluteLayout.SetLayoutBounds(nameLabel,
                new Rectangle(.25, .25, 400, 40));
            nameLabel.FontSize = 24;
            cellView.Children.Add(nameLabel);
            var typeLabel = new Label();
            typeLabel.SetBinding(Label.TextProperty, "Type");
            AbsoluteLayout.SetLayoutBounds(typeLabel,
                new Rectangle(50, 35, 200, 25));
            cellView.Children.Add(typeLabel);
            var image = new Image();
            image.SetBinding(Image.SourceProperty, "Image");
            AbsoluteLayout.SetLayoutBounds(image,
                new Rectangle(250, .25, 200, 25));
            cellView.Children.Add(image);
            this.View = cellView;
        }

    }
}
