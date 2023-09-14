using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Tag
    {
        [Key]
        public int id_tag { get; set; }
        public int? id_parent { get; set; }
        public string tag_name { get; set; }
        public virtual HashSet<Listing> listings { get; set; }
    }
}
