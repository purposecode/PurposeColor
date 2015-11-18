using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor.CustomControls
{
    public class CustomEditor : Editor
    {
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create<CustomEditor, string>(view => view.Placeholder, String.Empty);

        public CustomEditor()
        {
        }

        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }

            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }
    }
}
