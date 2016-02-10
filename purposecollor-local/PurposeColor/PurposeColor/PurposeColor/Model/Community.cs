using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
	public class ChatUsersDetails
	{
		public string user_id { get; set; }
		public string firstname { get; set; }
		public string profileimg { get; set; }
		public string logged_status { get; set; }
		public string profileImgUrl
		{
			get
			{
				return Constants.SERVICE_BASE_URL + profileimg;
			}
		}
	}

	public class ChatUsersObject
	{
		public string code { get; set; }
		public string text { get; set; }
		public List<ChatUsersDetails> resultarray { get; set; }
	}

	public class CommunityGemsDetailsDB
	{
		public string user_id { get; set; }
		public int like_status { get; set; }
		public int likecount { get; set; }
		public string gem_id { get; set; }
		public string gem_title { get; set; }
		public string gem_details { get; set; }
		public string gem_datetime { get; set; }
		public string share_status { get; set; }
		public string firstname { get; set; }
		public string profileimg { get; set; }
	}


	public class GemMedia
	{
		public string gem_id { get; set; }
		public string media_type { get; set; }
		public string gem_media { get; set; }
	}

	public class CommunityGemsDetails
	{
		public string user_id { get; set; }
		public int like_status { get; set; }
		public int likecount { get; set; }
		public string gem_id { get; set; }
		public string gem_title { get; set; }
		public string gem_details { get; set; }
		public string gem_datetime { get; set; }
		public string share_status { get; set; }
		public string firstname { get; set; }
		public string profileimg { get; set; }
		public List<GemMedia> gem_media { get; set; }
	}

	public class CommunityGemsObject
	{
		public string code { get; set; }
		public string text { get; set; }
		public List<CommunityGemsDetails> resultarray { get; set; }
	}

	public class MyGemsDetails
	{
		public string user_id { get; set; }
		public string gem_id { get; set; }
		public string gem_title { get; set; }
		public string gem_details { get; set; }
		public string gem_datetime { get; set; }
		public string share_status { get; set; }
		public string firstname { get; set; }
		public string profileimg { get; set; }
		public List<GemMedia> gem_media { get; set; }
	}

	public class MyGemsObject
	{
		public string code { get; set; }
		public string text { get; set; }
		public List<MyGemsDetails> resultarray { get; set; }
	}


	public class LikeResponse
	{
		public string code { get; set; }
		public string text { get; set; }
		public int like_status { get; set; }
		public int likecount { get; set; }
	}

}
