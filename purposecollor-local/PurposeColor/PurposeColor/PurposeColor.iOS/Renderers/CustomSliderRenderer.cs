using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using PurposeColor.CustomControls;
using PurposeColor.Droid.Renderers;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(CustomSliderRenderer))]
namespace PurposeColor.Droid.Renderers
{
    class CustomSliderRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            UISlider control = (UISlider)Control;
            control.SetThumbImage(new UIImage("Icon-76.png"), UIControlState.Normal);
        }
    }
}

