using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class CommentsModel
    {
        public string uImageUrl { get; set; }
        public string AutherName { get; set; }
        public string CommentText { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
