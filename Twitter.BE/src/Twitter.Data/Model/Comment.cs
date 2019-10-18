namespace Twitter.Data.Model
{
    public class Comment : BaseEntity
    {
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public string Text { get; set; }
    }
}
