
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(PurposeColor.Droid.Renderers.CustomEditorRenderer))]
namespace PurposeColor.Droid.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        private static global::Android.Graphics.Color _textColor;
        private static global::Android.Graphics.Color _bgColor;

        static CustomEditorRenderer()
        {
            _textColor = global::Android.Graphics.Color.Gray;
            _bgColor = global::Android.Graphics.Color.White;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            Control.SetTextColor(_textColor);
            Control.SetBackgroundColor(_bgColor);
        }
    }
}