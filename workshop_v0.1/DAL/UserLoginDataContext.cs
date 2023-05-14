using Microsoft.EntityFrameworkCore;
using workshop_v0._1.Models;

namespace workshop_v0._1.DAL
{
    public class UserLoginDataContext : DbContext
    {
        public UserLoginDataContext(DbContextOptions<UserLoginDataContext> options) : base(options)
        {
        }
        public DbSet<UserLoginData> UserLoginData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLoginData>()
              .HasOne(uld => uld.user)
              .WithMany(u => u.creds)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
