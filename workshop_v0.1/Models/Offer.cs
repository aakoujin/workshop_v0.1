using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Offer
    {
        [Key]
        [Required(AllowEmptyStrings =false)]
        public int Id_offer { get; set; }
        
        [Display(Name = "Title")]
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
        
        [Display(Name = "Price")]
        [Required(AllowEmptyStrings = false)]
        public decimal Price { get; set; }
        
        [Display(Name = "Destination")]
        [Required(AllowEmptyStrings = false)]
        public string Destination { get; set; }
        
        [Display(Name = "Start")]
        [Required(AllowEmptyStrings = false)]
        public string Start { get; set; }

    }
}
