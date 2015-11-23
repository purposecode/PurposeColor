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
}
