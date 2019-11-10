using System;
using System.Collections.Generic;
using System.Text;

namespace Twitter.Services.Helpers.NotificationParameters
{
    public class LikeParameters
    {
        public int CommentsCount { get; set; }
        public string LikingUserFullName { get; set; }
        public string TweetAuthorFullName { get; set; }
    }
}
