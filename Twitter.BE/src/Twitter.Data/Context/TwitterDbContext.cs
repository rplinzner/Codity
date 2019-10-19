using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Twitter.Data.Model;

namespace Twitter.Data.Context
{
    public class TwitterDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<CodeSnippet> CodeSnippets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<TweetLike> TweetLikes { get; set; }

        public TwitterDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity
                    .HasMany(c=>c.Following)
                    .WithOne(c => c.Follower)
                    .HasForeignKey(c => c.FollowerId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity
                    .HasMany(c=>c.Followers)
                    .WithOne(c => c.Following)
                    .HasForeignKey(c => c.FollowingId);

                entity.ToTable("Users");
            });

            builder.Entity<Tweet>(entity =>
            {
                entity
                    .HasMany(c => c.Comments)
                    .WithOne(c => c.Tweet)
                    .HasForeignKey(c => c.TweetId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity
                    .HasMany(c => c.Likes)
                    .WithOne(c => c.Tweet)
                    .HasForeignKey(c => c.TweetId)
                    .OnDelete(DeleteBehavior.Restrict);
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
