using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PurposeColor.interfaces;
using Xamarin.Forms.Platform.WinPhone;
using PurposeColor.WinPhone.Renderers;
using System.Windows.Media;
using System.Windows.Media.Imaging;



[assembly: ExportRenderer(typeof(CustomImageButton), typeof(WinImageButtonRenderer))]
namespace PurposeColor.WinPhone.Renderers
{

    public class WinImageButtonRenderer : ButtonRenderer
    {
        #region PRIVATE VARIABLES

        private CustomImageButton formsElement;
        System.Windows.Controls.Button winButton;

        #endregion

        #region RESOURCE KEY

        const string LEFT_TEXT = "COMMON_TEXT_LEFT";

        const string RIGHT_TEXT = "COMMON_TEXT_RIGHT";
        #endregion

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {

                winButton = Control;
                formsElement = (CustomImageButton)this.Element;
             

                if( formsElement.TextOrientation == TextOrientation.Left )
                {
                    winButton.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                }
                else if (formsElement.TextOrientation == TextOrientation.Middle)
                {
                   winButton.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                }
                else
                {
                    winButton.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
                }

                formsElement.BorderColor = Xamarin.Forms.Color.Transparent;
                formsElement.BorderWidth = 0;
                if (formsElement.ImageName != null )
                winButton.Background = new ImageBrush { Stretch = System.Windows.Media.Stretch.Fill, ImageSource = new BitmapImage(new Uri(formsElement.ImageName, UriKind.Relative)) };

      
               
            }
        }

       
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
      


    }
}
