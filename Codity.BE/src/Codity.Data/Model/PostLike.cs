namespace Codity.Data.Model
{
    public class PostLike: BaseEntity, IBaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
