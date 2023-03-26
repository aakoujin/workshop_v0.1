using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class UserLoginData
    {
        public string username { get; set; } = string.Empty;
        public byte[] userPassword { get; set; }
        public byte[] userSalt { get; set; }
    }
}
