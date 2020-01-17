namespace Codity.Data.Model
{
    public class CodeSnippet : BaseEntity, IBaseEntity
    {
        public string Text { get; set; }
        public string GistURL { get; set; }
        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
    }
}
