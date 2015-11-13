using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PurposeColor.screens
{
    public partial class GemsDetailedView : ContentPage
    {
        public GemsDetailedView( string gemsInfo, string imageName )
        {
            InitializeComponent();
            gemsInfoLabel.Text = gemsInfo;
            gemsImage.Source = imageName;
        }
    }
}
