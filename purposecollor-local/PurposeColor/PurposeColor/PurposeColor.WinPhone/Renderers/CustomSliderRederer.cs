using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PurposeColor.CustomControls;
using Xamarin.Forms.Platform.WinPhone;
using PurposeColor.WinPhone.Renderers;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(MySliderRenderer))]
namespace PurposeColor.WinPhone.Renderers
{

    class MySliderRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            System.Windows.Controls.Slider slider = (System.Windows.Controls.Slider)Control;           
        }
    }
}





