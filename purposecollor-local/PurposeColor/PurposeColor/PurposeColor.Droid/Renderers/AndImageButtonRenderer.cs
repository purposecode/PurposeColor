
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;
using PurposeColor.interfaces;
using Android.Graphics;


[assembly: ExportRenderer(typeof(CustomImageButton), typeof(AndImageButtonRenderer))]
namespace PurposeColor.interfaces
{

    public class AndImageButtonRenderer : ButtonRenderer
    {
        #region PRIVATE VARIABLES

        private CustomImageButton formsElement;

        global::Android.Widget.Button androidButton;
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
                androidButton = (global::Android.Widget.Button)Control;
                formsElement = (CustomImageButton)this.Element;
                androidButton.SetPadding(20, 20, 20, 20);
                androidButton.SetAllCaps(false);
                if (formsElement.ImageName != null)
                    androidButton.SetBackgroundDrawable(Context.Resources.GetDrawable(formsElement.ImageName));


                if (formsElement.TextOrientation != null)
                {
                    if (Convert.ToString(formsElement.TextOrientation) == "Left")
                    {

                        androidButton.Gravity = Android.Views.GravityFlags.Left;
                    }
                    else if (Convert.ToString(formsElement.TextOrientation) == "Right")
                    {
                        androidButton.Gravity = Android.Views.GravityFlags.Right;
                    }
                    else
                    {
                        androidButton.Gravity = Android.Views.GravityFlags.Center;
                    }
                }
            }
        }

       
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (formsElement.ImageName != null)
                androidButton.SetBackgroundDrawable(Context.Resources.GetDrawable(formsElement.ImageName));
        }
      
        public override void ChildDrawableStateChanged(Android.Views.View child)
        {
            base.ChildDrawableStateChanged(child);
            Control.Text = Control.Text;
        }

    }
}