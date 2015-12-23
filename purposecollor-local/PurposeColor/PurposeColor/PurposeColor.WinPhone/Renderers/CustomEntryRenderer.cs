using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(PurposeColor.CustomControls.CustomEntry), typeof(PurposeColor.WinPhone.Renderers.CustomEntryRenderer))]
namespace PurposeColor.WinPhone.Renderers
{
    class CustomEntryRenderer : Xamarin.Forms.Platform.WinPhone.EntryRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.WinPhone.ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Control. = UITextBorderStyle.Line;
                Control.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
                Control.Margin = new System.Windows.Thickness(0);//System.Windows.FrameworkElement.VisibilityProperty.;
                var nativePhoneTextBox = (Microsoft.Phone.Controls.PhoneTextBox)Control.Children[0];
                nativePhoneTextBox.Padding = new System.Windows.Thickness(0);
                nativePhoneTextBox.Margin = new System.Windows.Thickness(0);

                //nativePhoneTextBox.BorderThickness = new System.Windows.Thickness(0); // wont work for password fields.
                //nativePhoneTextBox.BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0,0,0,0)); // wont work for password fields.
            }
        }
    }
}
