using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Editor), typeof(PurposeColor.iOS.Renderers.CustomEditorRenderer))]
namespace PurposeColor.iOS.Renderers
{
    
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
                base.OnElementChanged(e);

                base.OnElementChanged(e);
                UITextView textView = (UITextView)Control;
                
            //Color
                textView.BackgroundColor = UIColor.White;
                textView.TextColor = UIColor.Gray;
                
            //font
                //textField.Font = UIFont.FromName("Ubuntu-light", 13);
        }
    }
}
