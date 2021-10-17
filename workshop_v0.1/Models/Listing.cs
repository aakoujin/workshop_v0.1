using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Listing
    {
        public int id_listing { get; set; }
        public User user { get; set; }
        public string post_name { get; set; }
        public string post_desc { get; set; }
        public DateTime post_date { get; set; }
        public int state { get; set; }
    }
}
