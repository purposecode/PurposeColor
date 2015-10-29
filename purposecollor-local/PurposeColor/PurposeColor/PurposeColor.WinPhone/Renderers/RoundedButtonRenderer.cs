using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.WinPhone;
using PurposeColor.CustomControls;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using PurposeColor.WinPhone.Renderers;
using Xamarin.Forms;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundedButtonRenderer))]
namespace PurposeColor.WinPhone.Renderers
{
   /* public class RoundedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var button = (RoundedButton)e.NewElement;

                button.SizeChanged += (s, args) =>
                {
                    Control.ApplyTemplate();
                    var borders = Control.GetVisuals<Border>();
                    var radius = Math.Min(button.WidthRequest, button.HeightRequest) / 2.0;

                    foreach (var border in borders)
                    {
                        border.CornerRadius = new CornerRadius(radius);
                        
                    }
                };
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "BorderRadius")
            {
                var borders = Control.GetVisuals<Border>();

                foreach (var border in borders)
                {
                    border.CornerRadius = new CornerRadius(((Xamarin.Forms.Button)sender).BorderRadius);
                }
            }
        }
    }

    static class DependencyObjectExtensions
    {
        public static IEnumerable<T> GetVisuals<T>(this DependencyObject root)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(root);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                if (child is T)
                    yield return child as T;

                foreach (var descendants in child.GetVisuals<T>())
                {
                    yield return descendants;
                }
            }
        }
    }*/


    public class RoundedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var button = (RoundedButton)e.NewElement;
                button.SizeChanged += (s, args) =>
                {
                    Control.ApplyTemplate();
                    var borders = Control.GetVisuals<Border>();
                    var radius = Math.Min(button.WidthRequest, button.HeightRequest) / 2.0;

                    foreach (var border in borders)
                    {
                        border.CornerRadius = new CornerRadius(radius);
                    }
                };
            }
        }
    }

    static class DependencyObjectExtensions
    {
        public static IEnumerable<T> GetVisuals<T>(this DependencyObject root)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(root);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                if (child is T)
                    yield return child as T;

                foreach (var descendants in child.GetVisuals<T>())
                {
                    yield return descendants;
                }
            }
        }
    }
}
