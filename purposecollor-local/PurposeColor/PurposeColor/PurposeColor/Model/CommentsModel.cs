using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class Comment
    {
        //comment_id":"179","comment_txt":"testeventcomment","comment_datetime":"2016-01-06 05:04:13",
        //"user_id":"2","firstname":"Purpose Code","profileurl":"uploads\/profile\/137696586592021.jpg"
        public string comment_id { get; set; }
        public string comment_txt { get; set; }
        public string comment_datetime { get; set; }
        public string user_id { get; set; }
        public string firstname { get; set; }
        public string profileurl { get; set; }
    }

    public class GetCommentsResult
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<Comment> resultarray { get; set; }
    }

}
