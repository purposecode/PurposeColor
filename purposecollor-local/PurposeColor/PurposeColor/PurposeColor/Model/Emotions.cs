using System.Collections.Generic;

namespace PurposeColor.Model
{
    public class Emotions
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<string> emotion_id { get; set; }
        public List<string> emotion_title { get; set; }
    }

    public class Emotion
    {
        [SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.AutoIncrement]
        public int ID { get; set; }
        public int EmotionId { get; set; }
        public string EmpotionName { get; set; }
        public int EmotionValue { get; set; }
    }
}
