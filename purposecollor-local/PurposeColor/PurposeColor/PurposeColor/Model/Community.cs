using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class GemMedia
    {
        public string gem_id { get; set; }
        public string media_type { get; set; }
        public string gem_media { get; set; }
    }

    public class CommunityGemsDetails
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

    public class CommunityGemsObject
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<CommunityGemsDetails> resultarray { get; set; }
    }
}
