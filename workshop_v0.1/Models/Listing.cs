﻿using System;
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
        public int id_user { get; set; }
        [Display(Name = "Owner")]
        [ForeignKey("id_user")]
        public virtual User user { get; set; }
        [Display(Name = "Title")]
        public string post_name { get; set; }
        [Display(Name = "Description")]
        public string post_desc { get; set; }
        [Display(Name = "PostDate")]
        public DateTime post_date { get; set; }
        [Display(Name = "Status")]
        public int state { get; set; } //use as marker for userId
        public int price { get; set; }

        //[Display(Name = "Comments")]
       // public virtual HashSet<Comment> comments { get; set;}
        [Display(Name = "Media")]
        public virtual HashSet<Content> contents { get; set; }
        [Display(Name = "Location")]
        public virtual HashSet<Location> locations { get; set; }
        public virtual HashSet<Tag> tags { get; set; }
    }
}
