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
    public class ReoveCommentResponse
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
		public string warm_percent {
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
}
