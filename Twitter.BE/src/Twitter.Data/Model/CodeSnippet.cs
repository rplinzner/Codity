namespace Twitter.Data.Model
{
    public class CodeSnippet : BaseEntity
    {
        public string Text { get; set; }

        public int ProgrammingLanguageId { get; set; }
        public string ProgrammingLanguage { get; set; }
    }
}
