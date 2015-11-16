
using System;
using Xamarin.Forms;


namespace PurposeColor.interfaces
{

    public enum TextOrientation
    {

        Middle,

        Left,

        Right
      
    }

    public class CustomImageButton : Button
    {

        public string ImageName { get; set; }


        public TextOrientation TextOrientation { get; set; }


        public CustomImageButton() { }
    }
}
