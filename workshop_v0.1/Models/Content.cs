using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Content
    {
        [Key]
        public int id_content { get; set; }
        [Display(Name = "Post")]
        [ForeignKey("id_listing")]
        public Listing listing { get; set; }
        public string media { get; set; }
    }
}
