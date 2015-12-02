using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{

    public class Goals
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<string> goal_id { get; set; }
        public List<string> goal_title { get; set; }
    }

    public class GoalDetails
    {
        public string goal_id { get; set; }
        public string user_id { get; set; }
        public string emotion_value { get; set; }
        public string category_id { get; set; }
        public string goal_title { get; set; }
        public string goal_details { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string goal_datetime { get; set; }
        public string actionstatus_id { get; set; }
        public string share_status { get; set; }
        public string gems_status { get; set; }
        public string status { get; set; }
        public string location_latitude { get; set; }
        public string location_longitude { get; set; }
        public string location_address { get; set; }
    }
    public class AllGoals
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<GoalDetails> resultarray { get; set; }
    }
}
