using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class UpdatedListingDto
    {
        public int id_listing { get; set; }
        public string post_name { get; set; }
        public string post_desc { get; set; }
        public DateTime post_date { get; set; }
        public int state { get; set; } 
        public int price { get; set; }

        //[Display(Name = "Comments")]
        // public virtual HashSet<Comment> comments { get; set;}
        public virtual HashSet<Content> contents { get; set; }
        public virtual HashSet<Location> locations { get; set; }
        public virtual HashSet<Tag> tags { get; set; }
    }
}
