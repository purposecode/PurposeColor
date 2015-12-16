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
using PurposeColor.CustomControls;
using Xamarin.Forms.Platform.Android;
using PurposeColor.Droid.Renderers;

[assembly: ExportRenderer(typeof(GemsListView), typeof(AndroidGemsListViewRenderer))]
namespace PurposeColor.Droid.Renderers
{
    class AndroidGemsListViewRenderer : ListViewRenderer
    {
        Android.Widget.ListView nativeListView;
        GemsListView formsListView;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            nativeListView = this.Control;
            formsListView =(GemsListView) this.Element;
            nativeListView.Scroll += nativeListView_Scroll;

        }

        void nativeListView_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {

            int visblepos = nativeListView.FirstVisiblePosition;
            if( formsListView != null && formsListView.Scroll != null )
            {
                formsListView.Scroll(visblepos - 1);
            }

            System.Diagnostics.Debug.WriteLine( "Current visible :  " +  visblepos.ToString() );
        }
    }
}