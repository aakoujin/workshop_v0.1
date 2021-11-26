using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Comment
    {
        [Key]
        public int id_comment { get; set; }
        [Display(Name = "Post")]
        public Listing listing { get; set; }
        [Display(Name = "Comment")]
        public string comment_text { get; set; }
    }
}
