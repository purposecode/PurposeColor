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

    public class EmotionDetails
    {
        public string emotion_id { get; set; }
        public string user_id { get; set; }
        public string emotion_title { get; set; }
        public string emotion_value { get; set; }
        public string status { get; set; }
    }

    public class EmotionsCollections
    {
        public string code { get; set; }
        public string text { get; set; }
        public List<EmotionDetails> resultarray { get; set; }
    }
}
