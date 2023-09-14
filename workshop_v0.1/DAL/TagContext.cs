using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using workshop_v0._1.Models;

namespace workshop_v0._1.DAL
{
    public class TagContext : DbContext
    {
        public TagContext(DbContextOptions<TagContext> options)
       : base(options)
        {
        }

        public DbSet<Tag> Tag { get; set; }

       
    }
}
