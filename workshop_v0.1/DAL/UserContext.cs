using Microsoft.EntityFrameworkCore;
using workshop_v0._1.Models;

namespace workshop_v0._1.DAL
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(u => u.listings)
                .WithOne(l => l.user)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(u => u.creds)
               .WithOne(c => c.user)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
