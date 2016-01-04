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
        public int UserId { get; set; }

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

		public int ShareToCommunity { get; set; }

        /// <summary>
        /// Gets or sets the PreferredGEMS
        /// </summary>
        //public List<GEMCategory> PreferredGEMS { get; set; }

    }

    public class LoginUser
    {
        public int user_id;
        public string firstname;
        public string email;
        public string profileurl;
        public string note;
        public string regdate;
        public string usertype_id;
    }

    public class UserDetailsOnLogin
    {
        public string code;
        public string text;
        public LoginUser resultarray;
    }
}
