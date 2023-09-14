using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class ListingTag
    {
        public int id_tag { get; set; }
        public int id_listing { get; set; }
    }
}
