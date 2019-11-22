namespace Twitter.Data.Model
{
    public class Language : BaseEntity, IBaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
