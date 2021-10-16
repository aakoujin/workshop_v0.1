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

        public DbSet<Listing> Listings { get; set; }
    }
}
