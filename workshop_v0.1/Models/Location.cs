using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Location
    {
        public int id_location { get; set; }
        public Listing listing { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string ctreet { get; set; } // fix typo
        public string postalCode { get; set; }
    }
}
