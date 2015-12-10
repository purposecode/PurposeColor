using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PurposeColor.CustomControls;
using Xamarin.Forms.Platform.WinPhone;
using PurposeColor.WinPhone.Renderers;
using System.Windows.Media;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(MySliderRenderer))]
namespace PurposeColor.WinPhone.Renderers
{

    class MySliderRenderer : SliderRenderer
    {
        CustomSlider formsSlider;
        System.Windows.Controls.Slider winSlider;

        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            formsSlider = (CustomSlider)this.Element;
            formsSlider.Value = 1;
            formsSlider.CurrentValue = 1;
            FeelingNowPage.sliderValue = 1;
            winSlider = (System.Windows.Controls.Slider)Control;
            winSlider.Style = (System.Windows.Style)App.Current.Resources["newtheme"];
            winSlider.LostMouseCapture += slider_LostMouseCapture;
        }

        void slider_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            formsSlider.Value =(int) winSlider.Value;
            formsSlider.CurrentValue = (int)winSlider.Value;
            if (formsSlider.StopGesture != null)
            {
                formsSlider.StopGesture(false);
            }
        }
    }
}





