using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workshop_v0._1.Models;

namespace workshop_v0._1.DAL
{
    public class ContentContext : DbContext
    {
        public ContentContext(DbContextOptions<ContentContext> options) : base(options)
        {

        }

        public DbSet<Content> Content { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
        }
    }
}
