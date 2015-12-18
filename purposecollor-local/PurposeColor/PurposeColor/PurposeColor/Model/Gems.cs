using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{

    public class GemsEmotionsDetails
    {
        public string user_id { get; set; }
        public string emotion_title { get; set; }
        public string event_title { get; set; }
        public string event_details { get; set; }
        public string event_datetime { get; set; }
        public string event_id { get; set; }
        public List<string> event_media { get; set; }
        public List<string> media_type { get; set; }
    }

    public class GemsEmotionsObject
    {
        public string code { get; set; }
        public string mediapath { get; set; }
        public string mediathumbpath { get; set; }
        public List<GemsEmotionsDetails> resultarray { get; set; }
    }
}
