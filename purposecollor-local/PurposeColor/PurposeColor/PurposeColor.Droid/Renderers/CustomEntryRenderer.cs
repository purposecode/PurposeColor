using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PurposeColor.Droid.Renderers;
using PurposeColor.CustomControls;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace PurposeColor.Droid.Renderers
{
    class CustomEntryRenderer : EntryRenderer
    {
        CustomEntry formsEntry;
        TextView nativeTextView;
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            nativeTextView = Control;
            if (nativeTextView == null)
            {
                return;
            }
            nativeTextView.SetBackgroundColor(Android.Graphics.Color.White);
            nativeTextView.SetTextColor(Android.Graphics.Color.Gray);
            if (nativeTextView != null)
            {
                if (nativeTextView.Text != null)
                {
                   // nativeTextView.SetTextColor(Android.Graphics.Color.Black);
                }
                
            }
            if (App.screenDensity > 1.5)
            {
                nativeTextView.SetTextSize(Android.Util.ComplexUnitType.Pt, 8);
            }
            else
            {
                nativeTextView.SetTextSize(Android.Util.ComplexUnitType.Pt, 5);
            }
        }
    }
}