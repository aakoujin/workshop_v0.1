using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workshop_v0._1.Models;

namespace workshop_v0._1.DAL
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<Content> Content { get; set; }
        public DbSet<Listing> Listing { get; set; }
        public DbSet<SavedListing> SavedListing { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserLoginData> UserLoginData { get; set; }
        public DbSet<ChatRoom> ChatRoom { get; set; }
        public DbSet<ChatRoomMessage> ChatRoomMessage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Listing>()
                .HasMany(li => li.locations)
                .WithOne(lo => lo.listing)
                .OnDelete(DeleteBehavior.Cascade);

            /*modelBuilder.Entity<Listing>()
                .HasMany(li => li.comments)
                .WithOne(co => co.listing)
                .OnDelete(DeleteBehavior.Cascade);*/

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
            //switched Foreign keys

            modelBuilder.Entity<Content>()
                 .HasOne(li => li.listing)
                 .WithMany(c => c.contents)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedListing>()
               .HasOne(sl => sl.User)
               .WithMany(u => u.savedListings)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.listings)
                .WithOne(l => l.user)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(u => u.creds)
               .WithOne(c => c.user)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.savedListings)
                .WithOne(s => s.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLoginData>()
             .HasOne(uld => uld.user)
             .WithMany(u => u.creds)
             .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChatRoom>()
                .HasMany(cr => cr.chat_room_messages)
                .WithOne(crm => crm.chat_room)
                .OnDelete(DeleteBehavior.Cascade);
        }

        
    }
}
