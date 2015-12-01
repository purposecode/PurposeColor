using Cross;
using CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class CustomeMultiSelectionList : ContentView
    {
        string pageTitle;
        CustomLayout masterLayout;
        Label listTitle;
        IDeviceSpec deviceSpec;



        string name;

        public string Name
        {
            get { return name; }
            set
            {
                string trimmedName = string.Empty;
                if (value.Length > 20)
                {
                    trimmedName = value.Substring(0, 28);
                    trimmedName += "...";
                }
                else
                {
                    trimmedName = value;
                }

                name = trimmedName;
            }
        }

        public string EmotionID { get; set; }
        public string EventID { get; set; }


        public CustomeMultiSelectionList()
        {
            Content = new Label { Text = "Hello ContentView" };
        }
    }
}
