using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Content
    {
        [Key]
        public int id_content { get; set; }
        [Display(Name = "Post")]
        public Listing listing { get; set; }
        //todo media: image
    }
}
