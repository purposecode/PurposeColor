﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PurposeColor
{
    public class Constants
    {
        public const string EMOTIONAL_AWARENESS = "Emotional Awareness";
        public const string GEM = "Goals Enabling Materials";
        public const string GOALS_AND_DREAMS = "Goals & Dreams";
        public const string EMOTIONAL_INTELLIGENCE = "Emotional Intelligence";
        public const string COMMUNITY_GEMS = "Community GEMs";
        public const string APPLICATION_SETTTINGS = "Application Settings";
        public const string SELECT_EMOTIONS = "Select Emotions";
		//-------------- for testing ----
		public const string AUDIO_RECORDING = "Audio recording";
		//-------------- for testing ----

        public const string HOW_YOU_ARE_FEELING = "Do you like how you are";

        public const string ADD_EVENTS = "Add Events, Situation or Thoughts";

        public const string ADD_GOALS = "Add Goals And Dreams";

        public const string ADD_ACTIONS = "Add Supporting Actions";

        public const string ALERT_TITLE = "Purpose Color";
        public const string ALERT_OK = "Ok";
        public const string ALERT_RETRY = "Retry";
        public const string NETWORK_ERROR_MSG = "Network error occured.";
        public const string COMMENTS_VIEW_CLASS_ID = "commentsView";

        public const int PICKER_ANIMATION_OFFSET = 40;

        public static Color MENU_BG_COLOR = Color.FromRgb(255, 255, 255);

        public static Color SUB_TITLE_BG_COLOR = Color.FromRgb(231, 234, 238);

        public static Color LIST_BG_COLOR = Color.FromRgb(230, 230, 230);

        public const string HELVERTICA_NEUE_LT_STD = "Helvetica Neue LT Std";

        public static Color BLUE_BG_COLOR = Color.FromRgb(8, 135, 224);

        public static Color TEXT_COLOR_GRAY = Color.FromHex("#949494");

        public static Color STACK_BG_COLOR_GRAY = Color.FromHex("#d6d6c2");
        
        public static Color PAGE_BG_COLOR_LIGHT_GRAY = Color.FromHex("#f4f4f4");

        public static Color MAIN_MENU_TEXT_COLOR = Color.FromHex("#42464d");

		public static Color INPUT_GRAY_LINE_COLOR = Color.FromHex("#f2f2f2");

        //public const string emailRegexString = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        public const string emailRegexString = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";



        #region SERVICE
        public const string SERVICE_BASE_URL = "http://purposecodes.com/pc/";
        #endregion


        #region ENUMS
        public enum MediaType
        {
	        Audio,
	        Video,
	        Image
        };
        #endregion
    }

}
