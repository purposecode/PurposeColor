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
            nativeTextView.SetBackgroundColor( Android.Graphics.Color.LightSteelBlue );

        }
    }
}