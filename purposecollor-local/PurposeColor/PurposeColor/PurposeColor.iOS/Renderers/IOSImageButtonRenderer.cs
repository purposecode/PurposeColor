
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;


using System;
using PurposeColor.interfaces;


[assembly: ExportRenderer(typeof(CustomImageButton), typeof(IOSImageButtonRenderer))]
namespace PurposeColor.interfaces
{

    class IOSImageButtonRenderer : ButtonRenderer
    {
        #region RESOURCE KEYS

        const string LEFT = "COMMON_TEXT_LEFT";

        const string RIGHT = "COMMON_TEXT_RIGHT";
        #endregion
        #region PRIVATE VARIABLES

        private CustomImageButton formsElement;

        UIButton iosButton;
        #endregion

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                iosButton = (UIButton) Control;
                formsElement = (CustomImageButton)this.Element;

                if (formsElement.ImageName != null)
                {
                    iosButton.SetBackgroundImage(UIImage.FromBundle(formsElement.ImageName), UIControlState.Normal);
                }

            
              
                 /*   if (Convert.ToString(callingElement.TextOrientation) == CustomLocalization.GetString(LEFT,string.Empty))
                    {

                        nativeButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                    }
                    else if (Convert.ToString(callingElement.TextOrientation) == CustomLocalization.GetString(RIGHT, string.Empty))
                    {

                        nativeButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
                    }
                    else
                    {

                        nativeButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
                    }*/
                
                
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ((iosButton != null) && (formsElement.ImageName != null))
            {
                iosButton.SetBackgroundImage(UIImage.FromBundle(formsElement.ImageName), UIControlState.Normal);
            }
                
        }
    }
}
