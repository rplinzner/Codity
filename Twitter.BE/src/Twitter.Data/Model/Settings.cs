namespace Twitter.Data.Model
{
    public class Settings : BaseEntity
    {
        public bool IsDarkTheme { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
