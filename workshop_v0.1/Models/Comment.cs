using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace workshop_v0._1.Models
{
    public class Comment
    {
        public int id_comment { get; set; }
        public Listing listing { get; set; }
        public string comment_text { get; set; }
    }
}
