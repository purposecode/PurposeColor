using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class ActionModel
    {
        public string action_id { get; set; }
        public string user_id { get; set; }
        public string action_title { get; set; }
        public string action_details { get; set; }
        public string action_datetime { get; set; }
        public string location_latitude { get; set; }
        public string location_longitude { get; set; }
        public string location_address { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string action_repeat { get; set; }
        public string action_alert { get; set; }
        public string goalaction_id { get; set; }
        public string status { get; set; }
    }

    public class SupportingActions
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<string> action_id { get; set; }
        public List<string> action_title { get; set; }
        
    }

    public class AllSupportingActions
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<ActionModel> resultarray { get; set; }
    }

}
