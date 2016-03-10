using System;
using System.Collections.Generic;

namespace Adafy.TwitterToRSS
{
    public class TweetInfo
    {
        public TweetInfo()
        {
            HashTags = new List<string>();
        }

        public string Caption { get; set; }
        public string Text { get; set; }
        public string PostUrl { get; set; }
        public string PictureUrl { get; set; }
        public string SenderName { get; set; }
        public string SenderUserName { get; set; }
        public DateTime SendTime { get; set; }
        public List<string> HashTags { get; set; }

        public string FormattedName => $"{SenderName} (@{SenderUserName})";
    }
}