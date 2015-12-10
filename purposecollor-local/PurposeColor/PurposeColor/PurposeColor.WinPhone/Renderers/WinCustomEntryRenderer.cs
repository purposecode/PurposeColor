using PurposeColor.CustomControls;
using PurposeColor.WinPhone.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(WinCustomEntryRenderer))]
namespace PurposeColor.WinPhone.Renderers
{
    public class WinCustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            
        }
    }
}
