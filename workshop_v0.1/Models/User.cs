﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class User
    {
        [Key]
        public int id_user { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false)]
        public string name { get; set; }

        [Display(Name = "Surname")]
        [Required(AllowEmptyStrings = false)]
        public string surname { get; set; }

        public string phonenumber { get; set; }

        public string email { get; set; }

        [Display(Name = "Listings")]
        public virtual HashSet<Listing> listings { get; set; }

        public virtual HashSet<UserLoginData> creds { get; set; }
        
        //TODO: add session
    }
}
