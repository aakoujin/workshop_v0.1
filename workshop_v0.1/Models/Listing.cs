using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Listing
    {
        [Key]
        public int id_listing { get; set; }
        [Display(Name = "Owner")]
        [ForeignKey("id_user")]
        public User user { get; set; }
        [Display(Name = "Title")]
        public string post_name { get; set; }
        [Display(Name = "Description")]
        public string post_desc { get; set; }
        [Display(Name = "PostDate")]
        public DateTime post_date { get; set; }
        [Display(Name = "Status")]
        public int state { get; set; }
        [Display(Name = "Comments")]
        public HashSet<Comment> comments { get; set;}
        [Display(Name = "Media")]
        public HashSet<Content> contents { get; set; }
        [Display(Name = "Location")]
        public HashSet<Location> locations { get; set; }
    }
}
