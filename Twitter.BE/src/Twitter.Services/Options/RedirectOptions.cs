namespace Twitter.Services.Options
{
    public class RedirectOptions
    {
        public string SuccessEmailConfirmationUrl { get; set; }
        public string InvalidEmailConfirmationUrl { get; set; }
        public string ConfirmEmailUrl { get; set; }
        public string TweetUrl { get; set; }
        public string UserUrl { get; set; }
        public string ResetPasswordUrl { get; set; }
    }
}
