namespace Twitter.Data.Model
{
    public class CodeSnippet : BaseEntity, IBaseEntity
    {
        public string Text { get; set; }

        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
    }
}
