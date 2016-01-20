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
}
