using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workshop_v0._1.Models;

namespace workshop_v0._1.DAL
{
    public class SavedListingContext : DbContext
    {
        public SavedListingContext(DbContextOptions<SavedListingContext> options)
        : base(options)
        {
        }

        public DbSet<SavedListing> SavedListing { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SavedListing>()
                .HasOne(sl => sl.User)
                .WithMany(u => u.savedListings)
                .OnDelete(DeleteBehavior.Cascade);
                //.HasForeignKey(sl => sl.IdUser);

        }

    }
}
