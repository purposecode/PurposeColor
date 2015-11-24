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
}
