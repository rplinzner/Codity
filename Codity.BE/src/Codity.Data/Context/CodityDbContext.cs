using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Codity.Data.Model;

namespace Codity.Data.Context
{
    public class CodityDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<CodeSnippet> CodeSnippets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }

        public CodityDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity
                    .HasMany(c => c.Following)
                    .WithOne(c => c.Follower)
                    .HasForeignKey(c => c.FollowerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(c => c.Followers)
                    .WithOne(c => c.Following)
                    .HasForeignKey(c => c.FollowingId);

                entity
                    .HasMany<Comment>()
                    .WithOne(c => c.Author)
                    .HasForeignKey(c => c.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany<PostLike>()
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict); 

                entity.ToTable("Users");
            });

            builder.Entity<Post>(entity =>
            {
                entity
                    .HasMany(c => c.Comments)
                    .WithOne(c => c.Post)
                    .HasForeignKey(c => c.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasMany(c => c.Likes)
                    .WithOne(c => c.Post)
                    .HasForeignKey(c => c.PostId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            builder.Entity<IdentityRole<int>>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        }
    }
}
