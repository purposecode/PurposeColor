
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PurposeColor.CustomControls.CustomEditor), typeof(PurposeColor.Droid.Renderers.CustomEditorRenderer))]
namespace PurposeColor.Droid.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        private static global::Android.Graphics.Color _textColor;
        private static global::Android.Graphics.Color _bgColor;
        PurposeColor.CustomControls.CustomEditor editor;

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
            if (e.NewElement != null)
            {
                var element = e.NewElement as PurposeColor.CustomControls.CustomEditor;
                this.Control.Hint = element.Placeholder;
                if (element.Text != null && element.Text != element.Placeholder)
                {
                    _textColor = global::Android.Graphics.Color.Black;
                }
                this.Control.TextSize = 16;
            }
        }
        
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == PurposeColor.CustomControls.CustomEditor.PlaceholderProperty.PropertyName)
            {
                var element = this.Element as PurposeColor.CustomControls.CustomEditor;
                this.Control.Hint = element.Placeholder;
            }
            
        }
    }
}