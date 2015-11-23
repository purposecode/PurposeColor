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
        double SliderValue;
        bool drag;
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            control = (SeekBar)Control;
           
            //control.ScaleY = 3;
            //control.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.drag_bg));
            formsSlider = (CustomSlider)this.Element;
            formsSlider.Value = 1;
            formsSlider.CurrentValue = 1;
            Android.Graphics.Drawables.Drawable drawable = Resources.GetDrawable(Resource.Drawable.drag_btn);
            control.SetThumb(drawable);

            control.ProgressChanged += control_ProgressChanged;
           control.StopTrackingTouch += control_StopTrackingTouch;
            control.StartTrackingTouch += control_StartTrackingTouch;
            
        }



        void control_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
           int sliderPercent = control.Progress * 100 / control.Max;

            if( sliderPercent > 50 )
            {
                Android.Graphics.Drawables.Drawable drawable = Resources.GetDrawable(Resource.Drawable.drag_btn);
                control.SetThumb(drawable);
            }
            else
            {
                Android.Graphics.Drawables.Drawable drawable = Resources.GetDrawable(Resource.Drawable.drag_btn_no);
                control.SetThumb(drawable);
            }
            
            if (sliderPercent >= 80)
            {
                formsSlider.CurrentValue = 2;

            }
            else if (sliderPercent >= 60)
            {
                formsSlider.CurrentValue = 1;
            }
            else if (sliderPercent >= 40)
            {
                formsSlider.CurrentValue = 0;
            }
            else if (sliderPercent >= 20)
            {
                formsSlider.CurrentValue = -1;
            }
            else
            {
                formsSlider.CurrentValue = -2;
            }

        }

        void control_StopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {

            formsSlider.StopGesture( false );
        }

        void control_StartTrackingTouch(object sender, SeekBar.StartTrackingTouchEventArgs e)
        {
            drag = true;
        }



    }
}

