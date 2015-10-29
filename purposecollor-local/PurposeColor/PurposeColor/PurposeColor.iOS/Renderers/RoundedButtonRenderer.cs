using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Platform.iOS;
using PurposeColor.CustomControls;
using Xamarin.Forms;
using PurposeColor.iOS.Renderers;

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundedButtonRenderer))]
namespace PurposeColor.iOS.Renderers
{
    public class RoundedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var button = (RoundedButton)e.NewElement;

                button.SizeChanged += (s, args) =>
                {
                    var radius = Math.Min(button.Width, button.Height) / 2.0;
                    button.BorderRadius = (int)(radius);
                };
            }
        }
    }
}
