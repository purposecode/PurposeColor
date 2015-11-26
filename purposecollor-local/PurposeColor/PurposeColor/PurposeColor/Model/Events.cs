using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class Events
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<string> event_id { get; set; }
        public List<string> event_title { get; set; }
    }

    public class UserEvent
    {
        [SQLite.Net.Attributes.PrimaryKey,SQLite.Net.Attributes.AutoIncrement]
        public int ID { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
    }


    public class EventDetails
    {
        public string emotion_id { get; set; }
        public string event_id { get; set; }
        public string user_id { get; set; }
        public string event_title { get; set; }
        public string event_details { get; set; }
        public string event_datetime { get; set; }
        public string status { get; set; }
    }

    public class AllEvents
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<EventDetails> resultarray { get; set; }
    }


    public class TestJSon
    {
        public string code { get; set; }
        public string text { get; set; }
        public string outputstring { get; set; }
    }



}
