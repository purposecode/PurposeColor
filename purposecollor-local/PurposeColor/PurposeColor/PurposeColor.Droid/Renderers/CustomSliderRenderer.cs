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
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            SeekBar control = (SeekBar)Control;
            Android.Graphics.Drawables.Drawable drawable = Resources.GetDrawable(Resource.Drawable.icon);
            control.SetThumb(drawable);
        }
    }
}

