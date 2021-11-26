using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workshop_v0._1.Models;

namespace workshop_v0._1.DAL
{
    public class ListingContext : DbContext
    {
        public ListingContext(DbContextOptions<ListingContext> options) : base(options)
        {

        }

        public DbSet<Listing> Listing { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Listing>()
                .HasMany(li => li.locations)
                .WithOne(lo => lo.listing)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Listing>()
                .HasMany(li => li.comments)
                .WithOne(co => co.listing)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Listing>()
                .HasMany(li => li.contents)
                .WithOne(c => c.listing)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
