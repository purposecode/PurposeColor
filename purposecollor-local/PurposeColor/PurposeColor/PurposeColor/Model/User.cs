using SQLite.Net.Attributes;
using System.Collections.Generic;

namespace PurposeColor.Model
{
    public class User
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The I.</value>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the UserId
        /// </summary>
		public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ConfirmPassword
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the Token
        /// </summary>
        public string AuthenticationToken { get; set; }

        /// <summary>
        /// Gets or sets the Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the Age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the Mobile
        /// </summary>
        public int Mobile { get; set; }

        public int UserType { get; set; }
        
        public string StatusNote { get; set; }

        public string RegistrationDate { get; set; }

        public string ProfileImageUrl{ get; set; }

        public string Email { get; set; }

		public bool AllowCommunitySharing { get; set; }

		public int VerifiedStatus { get; set; }
		public bool AllowFollowers {
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the PreferredGEMS
        /// </summary>
        //public List<GEMCategory> PreferredGEMS { get; set; }

    }

    public class LoginUser
    {
//        public string user_id;
//        public string firstname;
//        public string email;
//        public string profileurl;
//        public string note;
//        public string regdate;
//        public string usertype_id;
//		public int verified_status;
//
//

		public string user_id { get; set; }
		public string usertype_id { get; set; }
		public string firstname { get; set; }
		public string email { get; set; }
		public string note { get; set; }
		public string regdate { get; set; }
		public string logged_status { get; set; }
		public int verified_status { get; set; }
		public string profileurl { get; set; }
		public string follow_status { get; set; }
		public string community_status { get; set; }
    }

    public class UserDetailsOnLogin
    {
        public string code;
        public string text;
        public LoginUser resultarray;
    }
}
