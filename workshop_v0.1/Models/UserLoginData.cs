using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class UserLoginData
    {
        [Key]
        public int id_userLoginData { get; set; }

        public int id_user { get; set; }
        [ForeignKey("id_user")]
        public virtual User user { get; set; }
        public string username { get; set; } = string.Empty;
        public byte[] userPassword { get; set; }
        public byte[] userSalt { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
