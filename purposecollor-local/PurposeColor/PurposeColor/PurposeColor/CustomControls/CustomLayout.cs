using Cross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomControls
{
    public class CustomLayout : AbsoluteLayout
    {
        double screenHeight;
        double screenWidth;
        public CustomLayout()
        {
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            screenHeight = deviceSpec.ScreenHeight;
            screenWidth = deviceSpec.ScreenWidth;
        }
        public void AddChildToLayout( View view, float xPercent, float yPercent )
        {
            double xVal = screenWidth * xPercent / 100;
            double yVal = screenHeight * yPercent / 100;

			yVal = Device.OnPlatform (yVal + 20, yVal, yVal);
			Point pos = new Point( xVal, yVal);
            Children.Add(view, pos);
        }

        public void AddChildToLayout(View view, float xPercent, float yPercent, int parentWidth, int parentHeight)
        {
            double xVal = parentWidth * xPercent / 100;
            double yVal = parentHeight * yPercent / 100;

            Point pos = new Point(xVal, yVal);
            Children.Add(view, pos);
        }
    }
}
