using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Location
    {
        [Key]
        public int id_location { get; set; }
        public int id_listing { get; set; }
        [Display(Name = "Listing")]
        [ForeignKey("id_listing")]
        public virtual Listing listing { get; set; }
        [Display(Name = "Country")]
        public string country { get; set; }
        [Display(Name = "State/Region")]
        public string state { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Street")]
        public string ctreet { get; set; } // fix typo in db
        [Display(Name = "Zip Code")]
        public string postalCode { get; set; }
    }
}
