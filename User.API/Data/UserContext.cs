using Microsoft.EntityFrameworkCore;
using User.API.Models;

namespace User.API.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>()
                .ToTable("Users")
                .HasKey(m => m.Id);

            builder.Entity<UserProperty>()
                .ToTable("UserPropertities")
                .HasKey(m => new {m.AppUserId, m.Key, m.Value});

            builder.Entity<UserProperty>()
                .Property(m => m.Key).HasMaxLength(100);
            builder.Entity<UserProperty>()
                .Property(m => m.Value).HasMaxLength(100);

            builder.Entity<UserTag>()
                .ToTable("UserTags")
                .HasKey(m => new {m.AppUserId, m.Tag});
            builder.Entity<UserTag>()
                .Property(m => m.Tag).HasMaxLength(100);

            builder.Entity<BPFile>()
                .ToTable("BPFiles")
                .HasKey(m => m.Id);
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserProperty> UserProperties { get; set; }
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<BPFile> BPFiles { get; set; }

    }
}