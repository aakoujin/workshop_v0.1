using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class SavedListing
    {
        [Key]
        public int id_saved_listing { get; set; }

        public int id_listing { get; set; }

        public int id_user { get; set; }

        [ForeignKey("id_user")]
        public virtual User User { get; set; }
    }
}
