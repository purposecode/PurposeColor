using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PurposeColor.CustomControls.CustomEditor), typeof(PurposeColor.iOS.Renderers.CustomEditorRenderer))]
namespace PurposeColor.iOS.Renderers
{

    public class CustomEditorRenderer : EditorRenderer
    {
        UILabel labelPlaceHolder;
        UITextView replacingControl;

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

            replacingControl = new UITextView(Control.Bounds);
            var adelegate = new CustomTextViewDelegate();
            var element = this.Element as PurposeColor.CustomControls.CustomEditor;

            adelegate.Placeholder = element.Placeholder;

            replacingControl.Delegate = adelegate;
            replacingControl.TextColor = UIColor.LightGray;
            replacingControl.Text = adelegate.Placeholder;

            this.SetNativeControl(replacingControl);

        }//OnElementChanged()

        public class CustomTextViewDelegate : UITextViewDelegate
        {
            public string Placeholder { get; set; }

            public CustomTextViewDelegate()
            {
            }

            public override void EditingStarted(UITextView textView)
            {
                if (textView.Text == Placeholder)
                {
                    textView.Text = "";
                    textView.TextColor = UIColor.Black;
                }
                textView.BecomeFirstResponder();
                textView.BackgroundColor = UIColor.White;
                UIView view = getRootSuperView(textView);
                CoreGraphics.CGRect rect = view.Frame;
                rect.Y -= 80;
                view.Frame = rect;
            }

            private UIView getRootSuperView(UIView view)
            {
                if (view.Superview == null)
                    return view;
                else
                    return getRootSuperView(view.Superview);
            }

            public override void EditingEnded(UITextView textView)
            {
                if (textView.Text == "")
                {
                    textView.Text = Placeholder;
                    textView.TextColor = UIColor.LightGray;
                }
                textView.ResignFirstResponder();
                UIView view = getRootSuperView(textView);
                textView.BackgroundColor = UIColor.White;
                CoreGraphics.CGRect rect = view.Frame;
                rect.Y += 80;
                view.Frame = rect;
            }

        } // class CustomTextViewDelegate

    } //class CustomEditorRenderer

}//namespace
