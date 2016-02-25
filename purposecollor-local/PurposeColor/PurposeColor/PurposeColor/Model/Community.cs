using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace PurposeColor.Model
{
	public class ChatHistoryDetails
	{
		public string from_id { get; set; }
		public string msg { get; set; }
		public string chat_datetime { get; set; }
	}

	public class ChatHistoryObject
	{
		public string code { get; set; }
		public string text { get; set; }
		public List<ChatHistoryDetails> resultarray { get; set; }
	}

	public class ChatError
	{
		public string error { get; set; }
	}

	public class ChatRespObject
	{
		public long multicast_id { get; set; }
		public int success { get; set; }
		public int failure { get; set; }
		public int canonical_ids { get; set; }
		public List<ChatError> results { get; set; }
	}

	public class NotificationRespResult
	{
		public string message_id { get; set; }
	}

	public class NotitictionRequestRespObject
	{
		public long multicast_id { get; set; }
		public int success { get; set; }
		public int failure { get; set; }
		public int canonical_ids { get; set; }
		public List<NotificationRespResult> results { get; set; }
	}

	public class RegTokenResponse
	{
		public string code { get; set; }
		public string text { get; set; }
	}

	public class FolowersDetails
	{
		public string follow_id { get; set; }
		public string follow_status { get; set; }
		public string follow_datetime { get; set; }
		public string firstname { get; set; }
		public string profileimage { get; set; }
		public string profileImgUrl
		{
			get 
			{
				string fileName = Path.GetFileName(profileimage);
				return App.DownloadsPath + fileName;
			}
			set
			{
			}
		}
	}

	public class FollowersObject
	{
		public string code { get; set; }
		public string text { get; set; }
		public List<FolowersDetails> resultarray { get; set; }
	}


	public class FollowResponse
	{
		public string code { get; set; }
		public string text { get; set; }
	}

	public class ChatUsersDetails
	{
		public string user_id { get; set; }
		public string firstname { get; set; }
		public string profileimg { get; set; }
		public string logged_status { get; set; }
		public string profileImgUrl {
			get 
			{
				string fileName = Path.GetFileName(profileimg);
				return App.DownloadsPath + fileName;
			}
			set
			{
			}
		}
	}

	public class ChatDetails
	{
		public string FromUserID { get; set; }
		public string AuthorName { get; set; }
		public string Message{ get; set; }
		public string Timestamp{ get; set; }
		public string CurrentUserid { get; set; }

		public LayoutOptions BubblePos
		{
			get
			{
				if (CurrentUserid != FromUserID)
					return LayoutOptions.Start;
				else
					return LayoutOptions.End;
			}
		}

		public bool IsMine
		{
			get
			{
				if (CurrentUserid != FromUserID)
					return true;
				else
					return false;
			}
		}

		public Color BubbleColor
		{
			get
			{
				if (CurrentUserid != FromUserID)
					return Color.FromRgb( 0, 153, 255 );
				else
					return Color.FromRgb( 250, 250, 250 );
			}

		}

		public string ImageTip
		{
			get 
			{
				if (CurrentUserid != FromUserID)
					return "blue_tip.png";
				else
					return "yellow_tip.png";
			}
		}
	}


	public class ChatUsersInfo
	{
		public string user_id { get; set; }
		public string firstname { get; set; }
		public string profileimage { get; set; }
		public string profileImgUrl {
			get 
			{
				string fileName = Path.GetFileName(profileimage);
				return App.DownloadsPath + fileName;
			}
			set
			{
			}
		}
	}

	public class ChatObject
	{
		public string code { get; set; }
		public string text { get; set; }
		public List<ChatUsersInfo> resultarray { get; set; }
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
		public string gem_type { get; set; }
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
		public string gem_type { get; set; }
		public string media_type { get; set; }
		public string gem_media { get; set; }
	}

	public class CommunityGemsDetails
	{
		public string user_id { get; set; }
		public int like_status { get; set; }
		public int likecount { get; set; }
		public string gem_id { get; set; }
		public string gem_type { get; set; }
		public string gem_title { get; set; }
		public string gem_details { get; set; }
		public string gem_datetime { get; set; }
		public string share_status { get; set; }
		public int follow_status { get; set; }
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
