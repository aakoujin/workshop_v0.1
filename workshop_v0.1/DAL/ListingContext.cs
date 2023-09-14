using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Listing>()
                .HasMany(li => li.comments)
                .WithOne(co => co.listing)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Listing>()
                .HasMany(li => li.contents)
                .WithOne(c => c.listing)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Listing>()
                .HasOne(u => u.user)
                .WithMany(li => li.listings)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Listing>()
                .HasMany(l => l.tags)
                .WithMany(t => t.listings)
                .UsingEntity<ListingTag>(
                l => l.HasOne<Tag>().WithMany().HasForeignKey(e => e.id_tag),
                r => r.HasOne<Listing>().WithMany().HasForeignKey(e => e.id_listing));
        }
    }
}
