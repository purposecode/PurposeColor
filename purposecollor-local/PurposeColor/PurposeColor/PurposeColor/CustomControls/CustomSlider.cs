using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.CustomControls
{
    public class CustomSlider : XLabs.Forms.Controls.ExtendedSlider
    {
        public Action<bool> StopGesture { get; set; }
        public int CurrentValue { get; set; }
    }
}
