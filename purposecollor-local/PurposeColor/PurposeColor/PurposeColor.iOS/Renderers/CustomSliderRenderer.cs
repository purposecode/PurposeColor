using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using PurposeColor.CustomControls;
using PurposeColor.Droid.Renderers;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using PurposeColor.CustomControls;


[assembly: ExportRenderer(typeof(CustomSlider), typeof(CustomSliderRenderer))]
namespace PurposeColor.Droid.Renderers
{
    class CustomSliderRenderer : SliderRenderer
    {
		CustomSlider formsSlider;
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
			formsSlider = (CustomSlider)this.Element;
			formsSlider.Value = 1;
			formsSlider.CurrentValue = 1;
			FeelingNowPage.sliderValue = 1;
            UISlider control = (UISlider)Control;
			control.Value = 1.0f;
			control.SetThumbImage(new UIImage("drag_btn.png"), UIControlState.Normal);
			control.ValueChanged += (object sender, EventArgs evnt) => 
			{
				if( control.Value > 0 )
				{
					control.SetThumbImage(new UIImage("drag_btn.png"), UIControlState.Normal);
				}
				else
				{
					control.SetThumbImage(new UIImage("drag_btn_no.png"), UIControlState.Normal);
				}
			};

	
			control.TouchUpInside += (object sender, EventArgs be) => 
			{
				if( formsSlider.StopGesture != null )
				{
					formsSlider.CurrentValue = (int)formsSlider.Value;
					formsSlider.StopGesture(false);
				}
			};
           
        }
    }
}

