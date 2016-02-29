using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    class ResponseModel
    {
    }

    public class AddCommentsResponse
    {
        public string code { get; set; }
        public string text { get; set; }
        public int comment_id { get; set; }
    }
    public class CodeAndTextOnlyResponce
    {
        public string code{ get; set; }
        public string text { get; set; }
    }
    public class ShareStatusAndCommentsCount
    {
        public int share_status { get; set; } // 0 == not shared , 1 == shared
        public int comment_count { get; set; }
        public int favourite_count { get; set; }
    }
    public class ShareStatusResult
    {
        public string code { get; set; }
        public ShareStatusAndCommentsCount resultarray { get; set; }
    }
    public class EmotionValues
    {
        public string emotion_value { get; set; }
        public int count { get; set; }
        public string emotion_title { get; set; }
        public int emotion_id { get; set; }
    }

	public class AllEmotions
	{
		public string code { get; set; }
		public string text { get; set; }
		public int warm_percent {
			get;
			set;
		}
		public int patient_percent {
			get;
			set;
		}
		public int detailed_percent {
			get;
			set;
		}
		public int assertive_percent {
			get;
			set;
		}
		public List<EmotionValues> resultarray { get; set; }
	}

	public class EventWithImage
	{
		public string emotion_title {
			get;
			set;
		}

		public int event_id {
			get;
			set;
		}

		public string event_details {
			get;
			set;
		}

		public string event_media {
			get;
			set;
		}

	}

	public class AllEventsWithImage
	{
		public string code {
			get;
			set;
		}
		public string text {
			get;
			set;
		}
		public List<EventWithImage> resultarray {
			get;
			set;
		}
	}

	public class ActionWithImage
	{
		public string goal_title {
			get;
			set;
		}
		public string goalaction_id {
			get;
			set;
		}

		public string action_details {
			get;
			set;
		}

		public string action_media {
			get;
			set;
		}
	}

	public class AllActionsWithImage
	{
		public string code {
			get;
			set;
		}
		public string text {
			get;
			set;
		}
		public List<ActionWithImage> resultarray {
			get;
			set;
		}
	}

	public class ProfileDetails
	{
		public string firstname {
			get;
			set;
		}

		public string email {
			get;
			set;
		}

		public string note {
			get;
			set;
		}

		public int verified_status {
			get;
			set;
		}

		public string profileurl {
			get;
			set;
		}
	}
	public class ProfileDetailsResponse
	{
		public string code {
			get;
			set;
		}
		public string text {
			get;
			set;
		}

		public ProfileDetails resultarray {
			get;
			set;
		}
	}


}
