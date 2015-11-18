using System.Windows.Media;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(PurposeColor.CustomControls.CustomEditor), typeof(PurposeColor.WinPhone.Renderers.CustomEditorRenderer))]
namespace PurposeColor.WinPhone.Renderers
{
    public class CustomEditorRenderer : Xamarin.Forms.Platform.WinPhone.EditorRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.WinPhone.ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = new SolidColorBrush(Colors.White);
                
                //Control.BorderBrush = new SolidColorBrush(Colors.Transparent);

            }
        }
    }
}
