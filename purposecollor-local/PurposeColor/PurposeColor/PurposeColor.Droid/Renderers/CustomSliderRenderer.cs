using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PurposeColor.CustomControls;
using PurposeColor.Droid.Renderers;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(CustomSliderRenderer))]
namespace PurposeColor.Droid.Renderers
{
    class CustomSliderRenderer : SliderRenderer
    {
        CustomSlider formsSlider;
        SeekBar control;
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            control = (SeekBar)Control;


            //control.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.drag_bg));
            formsSlider = (CustomSlider)this.Element;
            formsSlider.ValueChanged += formsSlider_ValueChanged;
            formsSlider.Value = 1;

        }

        void formsSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {

            if (formsSlider.Value > 0)
            {
                Android.Graphics.Drawables.Drawable drawable = Resources.GetDrawable(Resource.Drawable.drag_btn);
                control.SetThumb(drawable);
            }
            else
            {
                Android.Graphics.Drawables.Drawable drawable = Resources.GetDrawable(Resource.Drawable.drag_btn_no);
                control.SetThumb(drawable);
            }
        }
    }
}

