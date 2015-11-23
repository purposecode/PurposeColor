using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurposeColor.Model
{
    public class Emotions
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<string> emotion_id { get; set; }
        public List<string> emotion_title { get; set; }
    }
}
