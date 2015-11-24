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
}
