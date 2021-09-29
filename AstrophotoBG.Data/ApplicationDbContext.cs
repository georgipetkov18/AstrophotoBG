using AstrophotoBG.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AstrophotoBG.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<UserPicture> UserPicture {get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserPicture>()
                .HasKey(up => new { up.UserId, up.PictureId });
            builder.Entity<UserPicture>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPictures)
                .HasForeignKey(up => up.UserId);
            builder.Entity<UserPicture>()
                .HasOne(up => up.Picture)
                .WithMany(p => p.UserPictures)
                .HasForeignKey(up => up.PictureId);
        }
    }
}
