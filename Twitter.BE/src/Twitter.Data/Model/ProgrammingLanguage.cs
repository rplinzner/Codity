namespace Twitter.Data.Model
{
    public class ProgrammingLanguage : BaseEntity, IBaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
