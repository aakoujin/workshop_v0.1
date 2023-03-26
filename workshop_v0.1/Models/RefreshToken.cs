using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class RefreshToken
    {
        public int id_token { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}
